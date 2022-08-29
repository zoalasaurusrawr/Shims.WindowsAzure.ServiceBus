using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;
public class TopicDescription : SBTopic
{
    public TopicDescription()
    {
    }

    public TopicDescription(string path)
        : base(name: path)
    {
    }

    public TopicDescription(SBTopic topic)
        : base(topic.Id, topic.Name, topic.Type, topic.Location, topic.SizeInBytes, topic.CreatedAt, 
            topic.UpdatedAt, topic.AccessedAt, topic.SubscriptionCount, topic.CountDetails, 
            topic.DefaultMessageTimeToLive, topic.MaxSizeInMegabytes, topic.MaxMessageSizeInKilobytes, 
            topic.RequiresDuplicateDetection, topic.DuplicateDetectionHistoryTimeWindow, topic.EnableBatchedOperations, 
            topic.Status, topic.SupportOrdering, topic.AutoDeleteOnIdle, topic.EnablePartitioning, topic.EnableExpress, topic.SystemData)
    {
    }

    public string Path => base.Name;
}
