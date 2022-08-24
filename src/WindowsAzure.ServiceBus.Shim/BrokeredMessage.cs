using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.ServiceBus.Messaging;

public class BrokeredMessage : ServiceBusMessage
{
    public BrokeredMessage(string payload)
        : base(payload)
    {

    }

    public BrokeredMessage(object payload)
        : this(payload, (payload == null) ? null : new DataContractBinarySerializer(GetObjectType(payload)))
    {

    }

    public BrokeredMessage(object serializableObject, XmlObjectSerializer serializer)
            : base(BrokeredMessage.SerializePayload(serializableObject, serializer))
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

    public T? GetBody<T>()
        where T : class
    {
        if (typeof(T) == typeof(string))
        {
            return (T)(object)Body.ToString();
        }
        else if (typeof(T) == typeof(Stream))
        {
            var stream = Body.ToStream();
            return (T)(object)stream;
        }
        else
        {
            return TryDeserializeBody<T>();
        }

        throw new InvalidOperationException("");
    }

    private T? TryDeserializeBody<T>()
    {
        var bodyStream = GetBody<Stream>() ?? throw new Exception("Could not get stream of body");
        var bodyString = new StreamReader(bodyStream, Encoding.UTF8).ReadToEnd();

        if (TryDeserializeJsonBody<T>(bodyString, out var body))
        {
            return body;
        }

        return default(T);
    }

    private bool TryDeserializeJsonBody<T>(string value, out T? body)
    {
        body = default(T);

        try
        {
            var token = JToken.Parse(value);
            body = JsonConvert.DeserializeObject<T>(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Complete() 
    {
        AcknowledgeMessageClient?.CompleteMessage(this);
    }

    public void Abandon() 
    {
        AcknowledgeMessageClient?.AbandonMessage(this);
    }

    private static ReadOnlyMemory<byte> SerializePayload(object payload, XmlObjectSerializer serializer)
    {
        MemoryStream memoryStream = new MemoryStream(256);
        serializer.WriteObject(memoryStream, payload);
        memoryStream.Flush();
        memoryStream.Position = 0L;
        return memoryStream.ToArray();
    }

    private static Type GetObjectType(object value)
    {
        if (value != null)
        {
            return value.GetType();
        }

        return typeof(object);
    }
}
