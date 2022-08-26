using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;

public class SubscriptionDescription
{
    public SubscriptionDescription(string topicPath, string subscriptionName)
        : this(topicPath, subscriptionName, null)
    {
    }

    public SubscriptionDescription(string topicPath, string subscriptionName, SBSubscription? entity)
    {
        TopicPath = topicPath ?? throw new ArgumentNullException(nameof(topicPath));
        SubscriptionName = subscriptionName ?? throw new ArgumentNullException(nameof(subscriptionName));
        Entity = entity;
    }

    public string TopicPath { get; set; } = "";
    public string SubscriptionName { get; set; } = "";

    public SBSubscription? Entity { get; set; }
}
