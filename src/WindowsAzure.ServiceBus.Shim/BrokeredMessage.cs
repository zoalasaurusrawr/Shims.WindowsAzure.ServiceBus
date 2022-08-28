using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
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

    public BrokeredMessage(object serializableObject, XmlObjectSerializer? serializer)
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
    public IDictionary<string, object> Properties => base.ApplicationProperties;

    public static BrokeredMessage Create(ServiceBusReceivedMessage serviceBusReceivedMessage)
    {
        var result = new BrokeredMessage(serviceBusReceivedMessage);
        return result;
    }

    internal static readonly int MessageVersion1 = 1;

    internal static readonly int MessageVersion2 = 2;

    internal static readonly int MessageVersion3 = 3;

    internal static readonly int MessageVersion4 = 4;

    internal static readonly int MessageVersion5 = 5;

    internal static readonly int MessageVersion6 = 6;

    internal static readonly int MessageVersion7 = 7;

    internal static readonly int MessageVersion8 = 8;

    internal static readonly int MessageVersion9 = 9;

    internal static readonly int MessageVersion10 = 10;

    internal static readonly int MessageVersion11 = 11;

    internal static readonly int MessageVersion12 = 12;

    internal static readonly int MessageVersion13 = 13;

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

        if (TryDeserializeBinaryBody<T>(bodyStream, out var body))
        {
            return body;
        }

        return default(T);
    }

    private bool TryDeserializeBinaryBody<T>(Stream stream, out T? body)
    {
        body = default(T);

        try
        {
            body = Deserialize<T>(stream);
            return body != null;
        }
        catch
        {
            return false;
        }
    }

    public static T? Deserialize<T>(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        var deserializer = new DataContractBinarySerializer(typeof(T));
        return (T?)deserializer.ReadObject(stream);
    }

    public void Complete() 
    {
        AcknowledgeMessageClient?.CompleteMessage(this);
    }

    public void Abandon() 
    {
        AcknowledgeMessageClient?.AbandonMessage(this);
    }

    private static ReadOnlyMemory<byte> SerializePayload(object payload, XmlObjectSerializer? serializer)
    {
        if (serializer == null)
            throw new ArgumentNullException(nameof(serializer));

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
