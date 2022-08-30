using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;

namespace Microsoft.ServiceBus.Messaging;
public class EventHubClient : IDisposable
{

    public EventHubClient(string connectionString)
    {
        this.ConnectionString = connectionString;
        _producerClient = new EventHubProducerClient(ConnectionString);
    }

    protected readonly string ConnectionString;
    public string Path => _producerClient.EventHubName;
    public string ClientId => _producerClient.Identifier;
    public bool EnableReceiverRuntimeMetric => false;
    public string EventHubName => _producerClient.EventHubName;
    public bool IsClosed => _producerClient.IsClosed;
    public int PrefetchCount { get; set; }
    public long? PrefetchSizeInBytes { get; set; }

    private EventHubProducerClient _producerClient;

    public static EventHubClient CreateFromConnectionString(string connectionString)
    {
        return new EventHubClient(connectionString);
    }

    public Task<EventHubSender> CreateSenderAsync(string publisherId)
    {
        var sender = new EventHubSender(ConnectionString, new EventHubProducerClientOptions { Identifier = publisherId });
        return Task.FromResult(sender);
    }

    public PartitionSender CreatePartitionSender(string partitionId)
    {
        var sender = new PartitionSender(ConnectionString, partitionId, new EventHubProducerClientOptions());
        return sender;
    }

    public Task<PartitionSender> CreatePartitionedSenderAsync(string partitionId)
    {
        var sender = new PartitionSender(ConnectionString, partitionId, new EventHubProducerClientOptions());
        return Task.FromResult(sender);
    }

    public PartitionReceiver CreateReceiver(string consumerGroupName, string partitionId, EventPosition eventPosition)
    {
        var receiver = new PartitionReceiver(consumerGroupName, ConnectionString, partitionId, eventPosition);
        return receiver;
    }

    public Task<PartitionReceiver> CreateReceiverAsync(string consumerGroupName, string partitionId, EventPosition eventPosition)
    {
        var receiver = new PartitionReceiver(consumerGroupName, ConnectionString, partitionId, eventPosition);
        return Task.FromResult(receiver);
    }

    public EventHubConsumerGroup GetDefaultConsumerGroup()
    {
        return GetConsumerGroup("$Default");
    }

    public EventHubConsumerGroup GetConsumerGroup(string name)
    {
        return new EventHubConsumerGroup(ConnectionString, name, EventHubName);
    }

    public PartitionRuntimeInformation GetPartitionRuntimeInformation(string partitionId)
    {
        return GetPartitionRuntimeInformationAsync(partitionId).GetAwaiter().GetResult();
    }

    public async Task<PartitionRuntimeInformation> GetPartitionRuntimeInformationAsync(string partitionId)
    {
        var properties = await _producerClient.GetPartitionPropertiesAsync(partitionId);
        var result = new PartitionRuntimeInformation
        {
            PartitionId = properties.Id,
            BeginSequenceNumber = properties.BeginningSequenceNumber,
            IsEmpty = properties.IsEmpty,
            LastEnqueuedOffset = properties.LastEnqueuedOffset.ToString(),
            LastEnqueuedSequenceNumber = properties.LastEnqueuedSequenceNumber,
            LastEnqueuedTimeUtc = properties.LastEnqueuedTime.DateTime,
            Path = properties.EventHubName
        };
        result.PartitionId = partitionId;
        return result;
    }

    public EventHubRuntimeInformation GetRuntimeInformation()
    {
        var result = GetRuntimeInformationAsync().GetAwaiter().GetResult();
        return result;
    }

    public async Task<EventHubRuntimeInformation> GetRuntimeInformationAsync()
    {
        var properties = await _producerClient.GetEventHubPropertiesAsync();
        var result = new EventHubRuntimeInformation
        {
            CreatedAt = properties.CreatedOn.DateTime,
            PartitionIds = properties.PartitionIds,
            PartitionCount = properties.PartitionIds?.Length ?? 0,
            Path = properties.Name
        };
        return result;
    }

    public Task SendAsync(EventData eventData)
    {
        return _producerClient.SendAsync(new List<EventData> { eventData });
    }

    public Task SendAsync(EventData eventData, string partitionKey)
    {
        var options = new SendEventOptions
        {
            PartitionKey = partitionKey
        };

        return _producerClient.SendAsync(new List<EventData> { eventData }, options);
    }

    public Task SendBatchAsync(IEnumerable<EventData> messages)
    {
        return _producerClient.SendAsync(messages);
    }

    public Task SendBatchAsync(IEnumerable<EventData> messages, string partitionKey)
    {
        var options = new SendEventOptions
        {
            PartitionKey = partitionKey
        };

        return _producerClient.SendAsync(messages, options);
    }

    public Task SendAsync(IEnumerable<EventData> messages)
    {
        return _producerClient.SendAsync(messages);
    }

    public Task SendAsync(IEnumerable<EventData> messages, string partitionKey)
    {
        var options = new SendEventOptions
        {
            PartitionKey = partitionKey
        };

        return _producerClient.SendAsync(messages, options);
    }

    public Task CloseAsync()
    {
        return _producerClient.CloseAsync();
    }

    public void Close()
    {
        _producerClient.CloseAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Close();
        }
    }
}
