using System.IO;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.Management.ServiceBus.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;

namespace Microsoft.ServiceBus;
public class NamespaceManager
{
    public NamespaceManager(Uri uri, TokenProvider tokenProvider)
        : this(uri, tokenProvider, BuildConfiguration().GetSection(ServiceBusSectionName).GetValue<string>(ResourceIdConfigKey))
    {
        SettingsCache = BuildConfiguration();
        AppOptions = new NamespaceManagerOptions();
        SettingsCache.GetSection(ArmConfigSectionName).Bind(AppOptions);
    }

    public NamespaceManager(Uri uri, TokenProvider tokenProvider, string resourceId)
        : base()
    {
        if (SettingsCache is null)
            SettingsCache = BuildConfiguration();

        Uri = uri;
        ResourceIdentifier = ParseResourceId(resourceId);
        NamespaceInfo = GetNamespaceInfo(uri);
        TokenProvider = tokenProvider;
        ArmClient = new ArmClient(tokenProvider.GetToken());

        if (AppOptions == null)
        {
            AppOptions = new NamespaceManagerOptions();
            SettingsCache.GetSection(ArmConfigSectionName).Bind(AppOptions);
        }
        var task = GetTokenCredentials();
        Task.WaitAny(task);
        ServiceBusClient = new ServiceBusManagementClient(task.Result);
        ServiceBusClient.SubscriptionId = ResourceIdentifier.SubscriptionId;
    }

    private static IConfigurationRoot? SettingsCache;
    private static DateTime TokenExpiresAtUtc = DateTime.MinValue;
    private static NamespaceManagerOptions AppOptions = new NamespaceManagerOptions();
    private static string TokenValue = string.Empty;
    private const string ServiceBusSectionName = "ServiceBus";
    private const string ResourceIdConfigKey = "ResourceId";
    private const string ArmConfigSectionName = "Arm";

    public ServiceBusManagementClient ServiceBusClient { get; }
    public ArmClient ArmClient { get; set; }
    public Uri Uri { get; }
    internal NamespaceInfo NamespaceInfo { get; set; }
    internal ResourceIdentifier ResourceIdentifier { get; set; }
    public TokenProvider TokenProvider { get; }

    public string GetQueueConnectionString(string queueName)
    {
        var authorizationRules = ServiceBusClient.Queues.ListAuthorizationRules(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, queueName);

        if (authorizationRules == null)
            return string.Empty;

        foreach (var authorizationRule in authorizationRules)
        {
            var keys = ServiceBusClient.Queues.ListKeys(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, queueName, authorizationRule.Name);

            if (keys == null)
                return string.Empty;

            return keys.PrimaryConnectionString;
        }

        return string.Empty;
    }

    public string GetTopicConnectionString(string topicPath)
    {
        var authorizationRules = ServiceBusClient.Topics.ListAuthorizationRules(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath);

        if (authorizationRules == null)
            return string.Empty;

        foreach (var authorizationRule in authorizationRules)
        {
            var keys = ServiceBusClient.Topics.ListKeys(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath, authorizationRule.Name);

            if (keys == null)
                return string.Empty;

            return keys.PrimaryConnectionString;
        }

        return string.Empty;
    }

    public SubscriptionDescription GetSubscription(string topicPath, string subscriptionName)
    {
        var subscription = ServiceBusClient.Subscriptions.Get(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath, subscriptionName);
        return new SubscriptionDescription(topicPath, subscriptionName, subscription);
    }

    public SubscriptionDescription GetSubscription(SubscriptionDescription description)
    {
        return GetSubscription(description.TopicPath, description.Name);
    }

    public IEnumerable<SubscriptionDescription> GetSubscriptions(string topicPath)
    {
        var subscriptions = ServiceBusClient.Subscriptions.ListByTopic(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath);

        if (subscriptions == null)
            return Enumerable.Empty<SubscriptionDescription>();

        return subscriptions.Select(s => new SubscriptionDescription(topicPath, s.Name, s));
    }

    public async Task<SubscriptionDescription> GetSubscriptionAsync(string topicPath, string subscriptionName)
    {
        var subscription = await ServiceBusClient.Subscriptions.GetAsync(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath, subscriptionName);
        return new SubscriptionDescription(topicPath, subscriptionName, subscription);
    }

    public Task<SubscriptionDescription> GetSubscriptionAsync(SubscriptionDescription description)
    {
        return GetSubscriptionAsync(description.TopicPath, description.Name);
    }

    public async Task<IEnumerable<SubscriptionDescription>> GetSubscriptionsAsync(string topicPath)
    {
        var subscriptions = await ServiceBusClient.Subscriptions.ListByTopicAsync(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath);

        if (subscriptions == null)
            return Enumerable.Empty<SubscriptionDescription>();

        return subscriptions.Select(s => new SubscriptionDescription(topicPath, s.Name, s));
    }

    public SubscriptionDescription CreateSubscription(string topicPath, string subscriptionName)
    {
        var subscription = ServiceBusClient.Subscriptions.CreateOrUpdate(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath, subscriptionName, new SBSubscription());
        return new SubscriptionDescription(topicPath, subscriptionName, subscription);
    }

    public SubscriptionDescription CreateSubscription(SubscriptionDescription description)
    {
        var subscription = ServiceBusClient.Subscriptions.CreateOrUpdate(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, description.TopicPath, description.Name, new SBSubscription());
        description.Entity = subscription;
        return description;
    }

    public void DeleteSubscription(string topicPath, string subscriptionName)
    {
        if (SubscriptionExists(topicPath, subscriptionName))
            ServiceBusClient.Subscriptions.Delete(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, topicPath, subscriptionName);
    }

    public void DeleteSubscription(SubscriptionDescription description)
    {
        if (SubscriptionExists(description.TopicPath, description.Name))
            ServiceBusClient.Subscriptions.Delete(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, description.TopicPath, description.Name);
    }

    public bool SubscriptionExists(string topicPath, string subscriptionName)
    {
        try
        {
            var subscription = ServiceBusClient.Subscriptions.Get(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, topicPath, subscriptionName);
            return subscription != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("SubscriptionNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }
        
    }

    public bool SubscriptionExists(SubscriptionDescription description)
    {
        try
        {
            var subscription = ServiceBusClient.Subscriptions.Get(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, description.TopicPath, description.Name);
            return subscription != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("SubscriptionNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }

    }

    public async Task<SubscriptionDescription> CreateSubscriptionAsync(string topicPath, string subscriptionName)
    {
        var subscription = await ServiceBusClient.Subscriptions.CreateOrUpdateAsync(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, topicPath, subscriptionName, new SBSubscription());
        return new SubscriptionDescription(topicPath, subscriptionName, subscription);
    }

    public async Task<SubscriptionDescription> CreateSubscriptionAsync(SubscriptionDescription description)
    {
        var subscription = await ServiceBusClient.Subscriptions.CreateOrUpdateAsync(ResourceIdentifier.ResourceGroupName, ResourceIdentifier.Name, description.TopicPath, description.Name, new SBSubscription());
        return new SubscriptionDescription(description.TopicPath, description.Name, subscription);
    }

    public Task DeleteSubscriptionAsync(string topicPath, string subscriptionName)
    {
        if (SubscriptionExists(topicPath, subscriptionName))
            return ServiceBusClient.Subscriptions.DeleteAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, topicPath, subscriptionName);

        return Task.CompletedTask;
    }

    public Task DeleteSubscriptionAsync(SubscriptionDescription description)
    {
        if (SubscriptionExists(description.TopicPath, description.Name))
            return ServiceBusClient.Subscriptions.DeleteAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, description.TopicPath, description.Name);

        return Task.CompletedTask;
    }

    public async ValueTask<bool> SubscriptionExistsAsync(string topicPath, string subscriptionName)
    {
        try
        {
            var subscription = await ServiceBusClient.Subscriptions.GetAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, topicPath, subscriptionName);
            return subscription != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("SubscriptionNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }

    }

    public async ValueTask<bool> SubscriptionExistsAsync(SubscriptionDescription description)
    {
        try
        {
            var subscription = await ServiceBusClient.Subscriptions.GetAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, description.TopicPath, description.Name);
            return subscription != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("SubscriptionNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }

    }

    public bool TopicExists(string path)
    {
        try
        {
            var topic = ServiceBusClient.Topics.Get(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path);
            return topic != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("TopicNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }
    }

    public TopicDescription GetTopic(string path)
    {
        var topic = ServiceBusClient.Topics.Get(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path);
        return new TopicDescription(path, topic);
    }

    public IEnumerable<TopicDescription> GetTopics()
    {
        var topics = ServiceBusClient.Topics.ListByNamespace(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name);

        if (topics == null)
            yield break;

        foreach (var topic in topics)
        {
            yield return new TopicDescription(topic.Name, topic);
        }
    }

    public void DeleteTopic(string path)
    {
        ServiceBusClient.Topics.Delete(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path);
    }

    public TopicDescription CreateTopic(string path)
    {
        var parameters = new SBTopic();
        var sbTopic = ServiceBusClient.Topics.CreateOrUpdate(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path, parameters);
        return new TopicDescription(path, sbTopic);
    }

    public TopicDescription CreateTopic(TopicDescription topicDescription)
    {
        var parameters = new SBTopic();
        var sbTopic = ServiceBusClient.Topics.CreateOrUpdate(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, topicDescription.Path, parameters);
        topicDescription.Entity = sbTopic;
        return topicDescription;
    }

    public async ValueTask<bool> TopicExistsAsync(string path)
    {
        try
        {
            var topic = await ServiceBusClient.Topics.GetAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path);
            return topic != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("TopicNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }
    }

    public async Task<TopicDescription> GetTopicAsync(string path)
    {
        var topic = await ServiceBusClient.Topics.GetAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path);
        return new TopicDescription(path, topic);
    }

    public async Task<IEnumerable<TopicDescription>> GetTopicsAsync()
    {
        var topics = await ServiceBusClient.Topics.ListByNamespaceAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name);

        if (topics == null)
            return Enumerable.Empty<TopicDescription>();

        return topics.Select(s => new TopicDescription(s.Name, s));
    }

    public Task DeleteTopicAsync(string path)
    {
        if (TopicExists(path))
            return ServiceBusClient.Topics.DeleteAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path);

        return Task.CompletedTask;
    }

    public async Task<TopicDescription> CreateTopicAsync(string path)
    {
        var parameters = new SBTopic();
        var sbTopic = await ServiceBusClient.Topics.CreateOrUpdateAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, path, parameters);
        return new TopicDescription(path, sbTopic);
    }

    public async Task<TopicDescription> CreateTopicAsync(TopicDescription topicDescription)
    {
        var parameters = new SBTopic();
        var sbTopic = await ServiceBusClient.Topics.CreateOrUpdateAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, topicDescription.Path, parameters);
        topicDescription.Entity = sbTopic;
        return topicDescription;
    }


    public QueueDescription CreateQueue(string name)
    {
        var parameters = new SBQueue();
        var sbQueue = ServiceBusClient.Queues.CreateOrUpdate(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name, parameters);
        return new QueueDescription(name, sbQueue);
    }

    public QueueDescription CreateQueue(QueueDescription queueDescription)
    {
        var parameters = new SBQueue();
        var sbQueue = ServiceBusClient.Queues.CreateOrUpdate(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, queueDescription.Path, parameters);
        queueDescription.Entity = sbQueue;
        return queueDescription;
    }

    public bool QueueExists(string name)
    {
        try
        {
            var sbQueue = ServiceBusClient.Queues.Get(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name);
            return sbQueue != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("QueueNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }
    }

    public void DeleteQueue(string name)
    {
        if (QueueExists(name))
            ServiceBusClient.Queues.Delete(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name);
    }

    public QueueDescription GetQueue(string name)
    {
        var queue = ServiceBusClient.Queues.Get(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name);
        return new QueueDescription(name, queue);
    }

    public IEnumerable<QueueDescription> GetQueues()
    {
        var queues = ServiceBusClient.Queues.ListByNamespace(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name);

        if (queues == null)
            yield break;

        foreach (var queue in queues)
        {
            yield return new QueueDescription(queue.Name, queue);
        }
    }

    public async Task<QueueDescription> CreateQueueAsync(QueueDescription queueDescription)
    {
        var parameters = new SBQueue();
        var sbQueue = await ServiceBusClient.Queues.CreateOrUpdateAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, queueDescription.Path, parameters);
        queueDescription.Entity = sbQueue;
        return queueDescription;
    }

    public async ValueTask<bool> QueueExistsAsync(string name)
    {
        try
        {
            var sbQueue = await ServiceBusClient.Queues.GetAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name);
            return sbQueue != null;
        }
        catch (ErrorResponseException ex)
        {
            if (ex.Body.Error.Code.Equals("QueueNotFound", StringComparison.InvariantCultureIgnoreCase))
                return false;
            throw;
        }
    }

    public Task DeleteQueueAsync(string name)
    {
        if (QueueExists(name))
            return ServiceBusClient.Queues.DeleteAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name);

        return Task.CompletedTask;
    }

    public async Task<QueueDescription> GetQueueAsync(string name)
    {
        var queue = await ServiceBusClient.Queues.GetAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name, name);
        return new QueueDescription(name, queue);
    }

    public async Task<IEnumerable<QueueDescription>> GetQueuesAsync()
    {
        var queues = await ServiceBusClient.Queues.ListByNamespaceAsync(ResourceIdentifier.ResourceGroupName, NamespaceInfo.Name);

        if (queues == null)
            return Enumerable.Empty<QueueDescription>();

        return queues.Select(s => new QueueDescription(s.Name, s));
    }

    private NamespaceInfo GetNamespaceInfo(Uri uri)
    {
        if (uri == null)
            throw new ArgumentNullException(nameof(uri));

        var uriParts = uri.ToString().Split(new char[] { '/', '.', ':' }, StringSplitOptions.RemoveEmptyEntries);

        if (uriParts.Length == 0)
            throw new InvalidOperationException($"Could not parse service url: {uri.ToString()}");

        //The namespace name will always appear between the scheme and the windows url parts
        //e.g. sb://YOUR-NAMESPACE-NAME.servicebus.windows.net
        if (uriParts.Length < 2)
            throw new InvalidOperationException($"Could not parse service url: {uri.ToString()}");

        var scheme = uriParts[0];
        var namespaceName = uriParts[1];

        return new NamespaceInfo(scheme, namespaceName);
    }

    private ResourceIdentifier ParseResourceId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        var resourceIdentifier = new ResourceIdentifier(value);
        return resourceIdentifier;
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder();
        builder.AddEnvironmentVariables();
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //This is bad and dumb. Come back and fix it later
        if (string.IsNullOrWhiteSpace(env))
            env = "Production";

        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
        return builder.Build();
    }

    //Big shoutout to https://stackoverflow.com/users/1389463/michi-werner for this solution
    private static async Task<TokenCredentials> GetTokenCredentials()
    {
        var defaultCredential = new DefaultAzureCredential(true);
        var context = new TokenRequestContext(new[] { "https://management.azure.com/.default" });
        var tokenResponse = await defaultCredential.GetTokenAsync(context);
        var defaultTokenCredentials = new Microsoft.Rest.TokenCredentials(tokenResponse.Token);
        return defaultTokenCredentials;
    }
}

public class NamespaceManagerOptions
{
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

internal class NamespaceInfo
{
    internal NamespaceInfo(string scheme, string name)
    {
        Scheme = scheme ?? throw new ArgumentNullException(nameof(scheme));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Scheme { get; set; } = "";
    public string Name { get; set; } = "";
}