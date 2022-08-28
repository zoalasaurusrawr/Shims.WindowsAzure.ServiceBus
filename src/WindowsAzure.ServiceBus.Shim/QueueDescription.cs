using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class QueueDescription : SBQueue
{
    public QueueDescription()
    {
    }

    public QueueDescription(SBQueue queue)
    {
        this.CopyFrom(queue);
    }

    public string Path => base.Name;
}
