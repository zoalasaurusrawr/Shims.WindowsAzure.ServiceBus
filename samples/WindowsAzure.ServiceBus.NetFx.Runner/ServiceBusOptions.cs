using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAzure.ServiceBus.NetFx.Runner;
public class ServiceBusOptions
{
    public ServiceBusOptions()
    {
    }

    public ServiceBusOptions(string queueName, string topicName, string subscriptionName, string namespaceName, string serviceBusName, string serviceBusKey)
    {
        QueueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
        TopicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
        SubscriptionName = subscriptionName ?? throw new ArgumentNullException(nameof(subscriptionName));
        NamespaceName = namespaceName ?? throw new ArgumentNullException(nameof(namespaceName));
        ServiceBusKeyName = serviceBusName ?? throw new ArgumentNullException(nameof(serviceBusName));
        ServiceBusKey = serviceBusKey ?? throw new ArgumentNullException(nameof(serviceBusKey));
    }

    public string QueueName { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public string SubscriptionName { get; set; } = string.Empty;
    public string NamespaceName { get; set; } = string.Empty;
    public string ServiceBusKeyName { get; set; } = string.Empty;
    public string ServiceBusKey { get; set; } = string.Empty;
}
