using System.Runtime.Serialization;

namespace WindowsAzure.ServiceBus.Net.Runner;

[DataContract]
public class TestModel
{
    public TestModel()
    {
    }

    public TestModel(string value)
    {
        Value = value;
    }

    [DataMember]
    public string Value { get; set; } = "";
}
