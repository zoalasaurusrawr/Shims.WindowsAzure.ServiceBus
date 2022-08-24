using System.Runtime.Serialization;
using System.Xml;

namespace Microsoft.ServiceBus.Messaging;
internal sealed class DataContractBinarySerializer : XmlObjectSerializer
{
    private readonly DataContractSerializer dataContractSerializer;

    public DataContractBinarySerializer(Type type)
    {
        dataContractSerializer = new DataContractSerializer(type);
    }

    public override object ReadObject(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        return ReadObject(XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max));
    }

    public override void WriteObject(Stream stream, object graph)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream, null, null, ownsStream: false);
        WriteObject(xmlDictionaryWriter, graph);
        xmlDictionaryWriter.Flush();
    }

    public override void WriteObject(XmlDictionaryWriter writer, object graph)
    {
        if (writer == null)
        {
            throw new ArgumentNullException("writer");
        }

        dataContractSerializer.WriteObject(writer, graph);
    }

    public override bool IsStartObject(XmlDictionaryReader reader)
    {
        return dataContractSerializer.IsStartObject(reader);
    }

    public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
    {
        return dataContractSerializer.ReadObject(reader, verifyObjectName);
    }

    public override void WriteEndObject(XmlDictionaryWriter writer)
    {
        dataContractSerializer.WriteEndObject(writer);
    }

    public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
    {
        dataContractSerializer.WriteObjectContent(writer, graph);
    }

    public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
    {
        dataContractSerializer.WriteStartObject(writer, graph);
    }
}