using System.Text;
using Microsoft.ServiceBus.Messaging;
using WindowsAzure.ServiceBus.NetFx.Runner;

class Program
{
    public static void Main()
    {
        StartQueueClient();
        StartTopicClient();
        Console.Read();
    }

    private static void StartQueueClient()
    {
        var connectionString = "";
        var queueName = "test-queue";

        var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
        var message = new BrokeredMessage(new TestModel("Test"));

        client.Send(message);

        client.OnMessage(msg =>
        {
            var body = msg.GetBody<Stream>();
            var bodyString = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            Console.WriteLine(String.Format("Message body: {0}", bodyString));
            Console.WriteLine(String.Format("Message id: {0}", msg.MessageId));
        });
    }

    private static void StartTopicClient()
    {
        var connectionString = "";
        var topicName = "test-topic";
        var subName = "test-sub";

        var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
        var message = new BrokeredMessage(new TestModel("Test"));

        client.Send(message);

        // Configure the callback options.
        OnMessageOptions options = new OnMessageOptions
        {
            AutoComplete = false,
            AutoRenewTimeout = TimeSpan.FromMinutes(1)
        };

        SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subName);

        Client.OnMessage((msg) =>
        {
            try
            {
                // Process message from subscription.
                Console.WriteLine("Body: " + msg.GetBody<TestModel>());
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