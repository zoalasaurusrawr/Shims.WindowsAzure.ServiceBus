using Azure;
using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace Microsoft.ServiceBus.Messaging;

public class PartitionSender : EventHubSender
{
    public PartitionSender(string connectionString, string partitionId) : base(connectionString)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(string connectionString, string partitionId, EventHubProducerClientOptions clientOptions) : base(connectionString, clientOptions)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(string connectionString, string partitionId, string eventHubName) : base(connectionString, eventHubName)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(EventHubConnection connection, string partitionId, EventHubProducerClientOptions clientOptions = null) : base(connection, clientOptions)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(string connectionString, string eventHubName, string partitionId, EventHubProducerClientOptions clientOptions) : base(connectionString, eventHubName, clientOptions)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(string fullyQualifiedNamespace, string eventHubName, string partitionId, AzureNamedKeyCredential credential, EventHubProducerClientOptions clientOptions = null) : base(fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(string fullyQualifiedNamespace, string eventHubName, string partitionId, AzureSasCredential credential, EventHubProducerClientOptions clientOptions = null) : base(fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        PartitionId = partitionId;
    }

    public PartitionSender(string fullyQualifiedNamespace, string eventHubName, string partitionId, TokenCredential credential, EventHubProducerClientOptions clientOptions = null) : base(fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        PartitionId = partitionId;
    }

    protected PartitionSender()
    {
        PartitionId = string.Empty;
    }

    public string PartitionId { get; }
    private SendEventOptions Options
    {
        get
        {
            return new SendEventOptions
            {
                PartitionId = this.PartitionId
            };
        }
    }

    public override Task SendAsync(EventDataBatch eventBatch, CancellationToken cancellationToken = default)
    {
        return base.SendAsync(eventBatch, cancellationToken);
    }

    public override Task SendAsync(IEnumerable<EventData> eventBatch, CancellationToken cancellationToken = default)
    {
        return base.SendAsync(eventBatch, Options, cancellationToken);
    }

    public override Task SendAsync(IEnumerable<EventData> eventBatch, SendEventOptions options, CancellationToken cancellationToken = default)
    {
        return base.SendAsync(eventBatch, options, cancellationToken);
    }
}
