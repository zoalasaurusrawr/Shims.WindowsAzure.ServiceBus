using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class QueueDescription : EntityDescription
{
    public QueueDescription(string path) 
        : base(path)
    {
    }

    public QueueDescription(string path, SBQueue? entity) : this(path)
    {
        Entity = entity;
    }

    public SBQueue? Entity { get; set; }
}
