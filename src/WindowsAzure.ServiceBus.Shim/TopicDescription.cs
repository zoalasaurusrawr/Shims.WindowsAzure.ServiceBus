using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class TopicDescription : EntityDescription
{
    public TopicDescription(string path)
        : base(path)
    {
    }

    public TopicDescription(string path, SBTopic? entity) : this(path)
    {
        Entity = entity;
    }

    public SBTopic? Entity { get; set; }
}
