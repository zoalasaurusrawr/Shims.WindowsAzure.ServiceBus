using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus.Messaging;

public class BrokeredMessage : ServiceBusMessage
{
    public BrokeredMessage(string payload)
        : base(payload)
    {

    }

    public BrokeredMessage(ServiceBusReceivedMessage serviceBusReceivedMessage)
        : base(serviceBusReceivedMessage)
    {
        ServiceBusReceivedMessage = serviceBusReceivedMessage;
    }

    internal ServiceBusReceivedMessage? ServiceBusReceivedMessage;
    internal IAcknowledgeMessageClient? AcknowledgeMessageClient;
    public IDictionary<string, object> Properties = new Dictionary<string, object>();

    public static BrokeredMessage Create(ServiceBusReceivedMessage serviceBusReceivedMessage)
    {
        var result = new BrokeredMessage(serviceBusReceivedMessage);
        return result;
    }

    public T GetBody<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return (T)(object)Body.ToString();
        }

        throw new InvalidOperationException("");
    }

    public void Complete() 
    {
        AcknowledgeMessageClient?.CompleteMessage(this);
    }

    public void Abandon() 
    {
        AcknowledgeMessageClient?.AbandonMessage(this);
    }
}
