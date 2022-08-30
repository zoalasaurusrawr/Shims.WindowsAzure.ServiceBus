using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using WindowsAzure.ServiceBus.Net.Runner;

class Program
{
    private static IConfiguration? Configuration;
    private static ServiceBusOptions? ServiceBusOptions;
    private static string ConnectionString = "";

    public static async Task Main()
    {
        Configuration = SetupConfiguration();
        ServiceBusOptions = new ServiceBusOptions();
        Configuration.GetSection("ServiceBus").Bind(ServiceBusOptions);
        ConnectionString = Configuration.GetConnectionString("AzureServiceBus");
        var namespaceManager = SetupNamespaceManager(ServiceBusOptions);
        await StartServiceHubClient();
        StartTopicClient(namespaceManager, ServiceBusOptions);
        StartQueueClient(namespaceManager, ServiceBusOptions);
        Console.Read();
    }

    private static IConfigurationRoot SetupConfiguration()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
        return builder.Build();
    }

    private static NamespaceManager SetupNamespaceManager(ServiceBusOptions options)
    {
        var scheme = "sb";
        var tokenProvider = TokenProvider.CreateTokenProvider();
        Uri uri = ServiceBusEnvironment.CreateServiceUri(scheme, options.NamespaceName, string.Empty);
        var namespaceManager = new NamespaceManager(uri, tokenProvider);
        return namespaceManager;
    }

    private static void StartQueueClient(NamespaceManager namespaceManager, ServiceBusOptions options)
    {
        Console.WriteLine("Starting Queues Sample Run");

        if (namespaceManager.QueueExists(options.QueueName))
            namespaceManager.DeleteQueue(options.QueueName);
        var queue = namespaceManager.CreateQueue(options.QueueName);

        var client = QueueClient.CreateFromConnectionString(ConnectionString, options.QueueName);
        var message = new BrokeredMessage(new TestModel("Test"));

        client.Send(message);

        client.OnMessage(msg => {
            Console.WriteLine("Handle Queue Message");
            var body = msg.GetBody<TestModel>();

            if (body == null)
            {
                msg.Abandon();
                return;
            }

            Console.WriteLine(String.Format("Message body: {0}", body.Value));
            Console.WriteLine(String.Format("Message id: {0}", msg.MessageId));
            msg.Complete();
        });
    }

    private static void StartTopicClient(NamespaceManager namespaceManager, ServiceBusOptions options)
    {
        Console.WriteLine("Starting Topics Sample Run");

        if (namespaceManager.TopicExists(options.TopicName))
            namespaceManager.DeleteTopic(options.TopicName);
        var topic = namespaceManager.CreateTopic(options.TopicName);

        if (namespaceManager.SubscriptionExists(options.TopicName, options.SubscriptionName))
            namespaceManager.DeleteSubscription(options.TopicName, options.SubscriptionName);
        var subscription = namespaceManager.CreateSubscription(options.TopicName, options.SubscriptionName);

        var client = TopicClient.CreateFromConnectionString(ConnectionString, options.TopicName);
        var message = new BrokeredMessage(new TestModel("Test"));

        client.Send(message);

        // Configure the callback options.
        OnMessageOptions onMessageOptions = new OnMessageOptions
        {
            AutoComplete = false,
            AutoRenewTimeout = TimeSpan.FromMinutes(1)
        };

        SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(ConnectionString, options.TopicName, options.SubscriptionName);

        Client.OnMessage((msg) => {
            try
            {
                Console.WriteLine("Handle Topic Message");
                var body = msg.GetBody<TestModel>();

                if (body == null)
                {
                    msg.Abandon();
                    return;
                }
                // Process message from subscription.
                Console.WriteLine("Body: " + body.Value);
                Console.WriteLine("MessageID: " + msg.MessageId);
                Console.WriteLine("Message Number: " +
                msg.Properties["MessageNumber"]);

                msg.Complete();
            }
            catch (Exception)
            {
                msg.Abandon();
            }
        }, onMessageOptions);
    }

    private static async Task StartServiceHubClient()
    {
        string EventHubConnectionString = Configuration.GetConnectionString("AzureEventHubs");
        Console.WriteLine("Connecting to the Event Hub...");
        var eventHubClient = EventHubClient.CreateFromConnectionString(EventHubConnectionString);
        var runtimeInformation = await eventHubClient.GetRuntimeInformationAsync();
        var testModel = new TestModel($"Test Message: {Guid.NewGuid()}");
        var jsonDeviceDetail = JsonConvert.SerializeObject(testModel);
        var encodedPayload = Encoding.UTF8.GetBytes(jsonDeviceDetail);
        var eventData = new EventData(encodedPayload);
        await eventHubClient.SendAsync(eventData);
        Console.WriteLine("Sent test message");
        EventHubConsumerGroup group = eventHubClient.GetDefaultConsumerGroup();
        var receivers = runtimeInformation.PartitionIds.Select(s => group.CreateReceiver(s));

        foreach (var receiver in receivers)
        {
            Console.WriteLine("Receiver created...");
            var events = receiver.ReceiveAsync();
            await foreach (var item in events)
            {
                Console.WriteLine(item.SequenceNumber);
            }
            Console.WriteLine("Receiving events...");
        }

        await eventHubClient.CloseAsync();
    }
}