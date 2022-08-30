using System.Collections.Concurrent;
using Azure;
using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;

namespace Microsoft.ServiceBus.Messaging;
public class EventHubReceiver : EventHubConsumerClient
{
    public EventHubReceiver(string consumerGroup, string connectionString) : base(consumerGroup, connectionString)
    {
    }

    public EventHubReceiver(string consumerGroup, string connectionString, EventHubConsumerClientOptions clientOptions) : base(consumerGroup, connectionString, clientOptions)
    {
    }

    public EventHubReceiver(string consumerGroup, string connectionString, string eventHubName) : base(consumerGroup, connectionString, eventHubName)
    {
    }

    public EventHubReceiver(string consumerGroup, EventHubConnection connection, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, connection, clientOptions)
    {
    }

    public EventHubReceiver(string consumerGroup, string connectionString, string eventHubName, EventHubConsumerClientOptions clientOptions) : base(consumerGroup, connectionString, eventHubName, clientOptions)
    {
    }

    public EventHubReceiver(string consumerGroup, string fullyQualifiedNamespace, string eventHubName, AzureNamedKeyCredential credential, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
    }

    public EventHubReceiver(string consumerGroup, string fullyQualifiedNamespace, string eventHubName, AzureSasCredential credential, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
    }

    public EventHubReceiver(string consumerGroup, string fullyQualifiedNamespace, string eventHubName, TokenCredential credential, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
    }

    public static EventHubSender CreateFromConnectionString(string connectionString)
    {
        return new EventHubSender(connectionString);
    }

    public IEnumerable<EventData> Receive()
    {
        var partitionEvents = base.ReadEventsAsync().ToListAsync().GetAwaiter().GetResult().AsEnumerable();

        if (partitionEvents == null)
            return Enumerable.Empty<EventData>();

        return partitionEvents.Select(s => s.Data);
    }
}
