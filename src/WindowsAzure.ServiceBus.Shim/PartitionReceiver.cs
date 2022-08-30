using Azure;
using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;

namespace Microsoft.ServiceBus.Messaging;
public class PartitionReceiver : EventHubReceiver
{
    public PartitionReceiver(string consumerGroup, string connectionString, string partitionId, EventPosition eventPosition) : base(consumerGroup, connectionString)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, string connectionString, string partitionId, EventPosition eventPosition, EventHubConsumerClientOptions clientOptions) : base(consumerGroup, connectionString, clientOptions)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, string connectionString, string partitionId, string eventHubName, EventPosition eventPosition) : base(consumerGroup, connectionString, eventHubName)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, EventHubConnection connection, string partitionId, EventPosition eventPosition, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, connection, clientOptions)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, string connectionString, string partitionId, string eventHubName, EventPosition eventPosition, EventHubConsumerClientOptions clientOptions) : base(consumerGroup, connectionString, eventHubName, clientOptions)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, string fullyQualifiedNamespace, string partitionId, string eventHubName, EventPosition eventPosition, AzureNamedKeyCredential credential, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, string fullyQualifiedNamespace, string partitionId, string eventHubName, EventPosition eventPosition, AzureSasCredential credential, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public PartitionReceiver(string consumerGroup, string fullyQualifiedNamespace, string partitionId, string eventHubName, EventPosition eventPosition, TokenCredential credential, EventHubConsumerClientOptions clientOptions = null) : base(consumerGroup, fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        PartitionId = partitionId;
        EventPosition = eventPosition;
    }

    public string ClientId => base.Identifier;
    public string PartitionId { get; }
    public EventPosition EventPosition { get; }

    private ReadEventOptions Options
    {
        get
        {
            return new ReadEventOptions
            {
                
            };
        }
    }

    public async IAsyncEnumerable<EventData> ReceiveAsync()
    {
        var partitionEvents = base.ReadEventsFromPartitionAsync(PartitionId, EventPosition);

        await foreach (PartitionEvent receivedEvent in partitionEvents)
        {
            yield return receivedEvent.Data;
        }
    }
}
