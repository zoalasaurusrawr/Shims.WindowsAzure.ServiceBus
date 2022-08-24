using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus.Messaging;

public class TopicClient : ServiceBusClient
{
    public TopicClient(string connectionString, string topicPath)
        : base(connectionString)
    {
        _topicPath = topicPath ?? throw new ArgumentNullException(nameof(topicPath));
    }

    private readonly string _topicPath;

    private ServiceBusSender? _sender;
    protected ServiceBusSender Sender
    {
        get
        {
            if (_sender == null)
                _sender = base.CreateSender(_topicPath);

            return _sender;
        }
    }

    public static TopicClient CreateFromConnectionString(string connectionString, string topicPath)
    {
        var result = new TopicClient(connectionString, topicPath);
        return result;
    }

    public void Send(BrokeredMessage message)
    {
        Sender.SendMessageAsync(message).Wait();
    }
}
