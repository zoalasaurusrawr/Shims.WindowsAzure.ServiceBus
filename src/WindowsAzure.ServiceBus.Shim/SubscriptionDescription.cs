using Microsoft.Azure.Management.ServiceBus.Models;

namespace Microsoft.ServiceBus;

public class SubscriptionDescription
{
    public SubscriptionDescription(string topicPath, string name)
        : this(topicPath, name, null)
    {
    }

    public SubscriptionDescription(string topicPath, string name, SBSubscription? entity)
    {
        TopicPath = topicPath ?? throw new ArgumentNullException(nameof(topicPath));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Entity = entity;
    }

    public string TopicPath { get; set; } = "";
    public string Name { get; set; } = "";

    public SBSubscription? Entity { get; set; }
}
