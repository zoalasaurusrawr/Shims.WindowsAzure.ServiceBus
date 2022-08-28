using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;

public class SubscriptionDescription : SBSubscription
{
    public SubscriptionDescription()
        : this(string.Empty)
    {    
    }

    public SubscriptionDescription(string topicPath)
    {
        TopicPath = topicPath;
    }

    public SubscriptionDescription(string topicPath, SBSubscription subscription)
        : base(subscription.Id, subscription.Name, subscription.Type, subscription.Location, subscription.MessageCount, 
            subscription.CreatedAt, subscription.AccessedAt, subscription.UpdatedAt, subscription.CountDetails, 
            subscription.LockDuration, subscription.RequiresSession, subscription.DefaultMessageTimeToLive, 
            subscription.DeadLetteringOnFilterEvaluationExceptions, subscription.DeadLetteringOnMessageExpiration, 
            subscription.DuplicateDetectionHistoryTimeWindow, subscription.MaxDeliveryCount, subscription.Status, 
            subscription.EnableBatchedOperations, subscription.AutoDeleteOnIdle, subscription.ForwardTo, 
            subscription.ForwardDeadLetteredMessagesTo, subscription.IsClientAffine, subscription.ClientAffineProperties, subscription.SystemData)
    {
        TopicPath = topicPath;
    }

    public string TopicPath { get; set; }
}
