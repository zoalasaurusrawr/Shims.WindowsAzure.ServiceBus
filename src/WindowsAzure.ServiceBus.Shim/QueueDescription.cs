using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class QueueDescription : SBQueue
{
    public string Path => base.Name;
}
