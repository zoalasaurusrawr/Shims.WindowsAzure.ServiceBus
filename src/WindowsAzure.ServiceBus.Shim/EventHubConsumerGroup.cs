using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.ServiceBus.Messaging;

namespace Microsoft.ServiceBus;
public class EventHubConsumerGroup : EventHubClient
{
    public EventHubConsumerGroup(string connectionString, string consumerGroupName, string eventHubName) 
        : base(connectionString)
    {
        ConsumerGroupName = consumerGroupName;
        _eventHubName = eventHubName;
    }

    public string ConsumerGroupName { get; }
    private string _eventHubName;
    public const string DefaultGroupName = "$Default";

    public PartitionReceiver CreateReceiver(string partitionId)
    {
        var receiver = new PartitionReceiver(ConsumerGroupName, ConnectionString, partitionId, _eventHubName, EventPosition.Earliest);
        return receiver;
    }

    public PartitionReceiver CreateReceiver(string partitionId, EventPosition eventPosition)
    {
        var receiver = new PartitionReceiver(ConsumerGroupName, ConnectionString, partitionId, _eventHubName, eventPosition);
        return receiver;
    }
}
