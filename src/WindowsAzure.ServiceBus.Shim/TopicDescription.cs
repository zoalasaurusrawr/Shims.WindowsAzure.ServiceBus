using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class TopicDescription : SBTopic
{
    public TopicDescription()
    {
    }

    public TopicDescription(SBTopic topic)
    {
        this.CopyFrom(topic);
    }

    public string Path => base.Name;
}
