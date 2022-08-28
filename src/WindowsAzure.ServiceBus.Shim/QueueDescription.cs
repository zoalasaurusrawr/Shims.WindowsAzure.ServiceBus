using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class QueueDescription : SBQueue
{
    public QueueDescription()
    {
    }

    public QueueDescription(SBQueue queue)
        : base(queue.Id, queue.Name, queue.Type, queue.Location, queue.CountDetails, queue.CreatedAt, 
            queue.UpdatedAt, queue.AccessedAt, queue.SizeInBytes, queue.MessageCount, queue.LockDuration, 
            queue.MaxSizeInMegabytes, queue.MaxMessageSizeInKilobytes, queue.RequiresDuplicateDetection, 
            queue.RequiresSession, queue.DefaultMessageTimeToLive, queue.DeadLetteringOnMessageExpiration, 
            queue.DuplicateDetectionHistoryTimeWindow, queue.MaxDeliveryCount, queue.Status, queue.EnableBatchedOperations, 
            queue.AutoDeleteOnIdle, queue.EnablePartitioning, queue.EnableExpress, queue.ForwardTo, 
            queue.ForwardDeadLetteredMessagesTo, queue.SystemData)
    {
    }


    public string Path => base.Name;
}
