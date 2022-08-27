using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class TopicDescription : SBTopic
{
    public string Path => base.Name;
}
