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

    public string TopicPath { get; set; }
}
