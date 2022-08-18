using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

class Program
{
    public static void Main()
    {
        //string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

        //var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

        //if (!namespaceManager.QueueExists("testqueue"))
        //{
        //    namespaceManager.CreateQueue("testqueue");
        //}

        StartQueueClient();
        StartTopicClient();
    }

    private static void StartQueueClient()
    {
        var connectionString = "<Service Bus Namespace Primary or Secondary Connection String>";
        var queueName = "<Your queue name>";

        var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
        var message = new BrokeredMessage("Test Message");

        client.Send(message);

        client.OnMessage(msg =>
        {
            Console.WriteLine(String.Format("Message body: {0}", msg.GetBody<String>()));
            Console.WriteLine(String.Format("Message id: {0}", msg.MessageId));
        });
    }

    private static void StartTopicClient()
    {
        var connectionString = "<Service Bus Namespace Primary or Secondary Connection String>";
        var topicName = "topic-name";

        var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
        var message = new BrokeredMessage("Test Message");

        client.Send(message);

        // Configure the callback options.
        OnMessageOptions options = new OnMessageOptions
        {
            AutoComplete = false,
            AutoRenewTimeout = TimeSpan.FromMinutes(1)
        };

        SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, "topic-path", "topic-name");

        Client.OnMessage((msg) =>
        {
            try
            {
                // Process message from subscription.
                Console.WriteLine("Body: " + msg.GetBody<string>());
                Console.WriteLine("MessageID: " + msg.MessageId);
                Console.WriteLine("Message Number: " +
                msg.Properties["MessageNumber"]);

                msg.Complete();
            }
            catch (Exception)
            {
                msg.Abandon();
            }
        }, options);
    }
}