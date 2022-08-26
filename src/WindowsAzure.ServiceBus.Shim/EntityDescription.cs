namespace Microsoft.ServiceBus;

public class EntityDescription
{
    public EntityDescription(string path)
    {
        Path = path;
    }

    public string Path { get; set; } = "";
}
