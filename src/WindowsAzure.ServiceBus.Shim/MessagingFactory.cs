namespace Microsoft.ServiceBus.Messaging;

public class MessagingFactory
{
    internal MessagingFactory(NamespaceManager namespaceManager)
    {
        NamespaceManager = namespaceManager;
    }

    internal NamespaceManager NamespaceManager { get; }

    public QueueClient CreateQueueClient(string queueName)
    {
        var connectionString = NamespaceManager.GetQueueConnectionString(queueName);
        return new QueueClient(connectionString, queueName);
    }

    public TopicClient CreateTopicClient(string topicPath)
    {
        var connectionString = NamespaceManager.GetTopicConnectionString(topicPath);
        return new TopicClient(connectionString, topicPath);
    }

    public SubscriptionClient CreateSubscriptionClient(string topicPath, string name)
    {
        var connectionString = NamespaceManager.GetTopicConnectionString(topicPath);
        return new SubscriptionClient(connectionString, topicPath, name);
    }

    public static MessagingFactory Create(Uri uri, TokenProvider tokenProvider)
    {
        var namespaceManager = new NamespaceManager(uri, tokenProvider);
        return new MessagingFactory(namespaceManager);
    }

    public static MessagingFactory Create(Uri uri, TokenProvider tokenProvider, string resourceId)
    {
        var namespaceManager = new NamespaceManager(uri, tokenProvider, resourceId);
        return new MessagingFactory(namespaceManager);
    }
}
