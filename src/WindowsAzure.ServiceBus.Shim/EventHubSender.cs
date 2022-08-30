using Azure;
using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace Microsoft.ServiceBus.Messaging;
public class EventHubSender : EventHubProducerClient
{
    public EventHubSender(string connectionString) : base(connectionString)
    {
        ClientOptions = new EventHubProducerClientOptions();
    }

    public EventHubSender(string connectionString, EventHubProducerClientOptions clientOptions) : base(connectionString, clientOptions)
    {
        ClientOptions = clientOptions;
    }

    public EventHubSender(string connectionString, string eventHubName) : base(connectionString, eventHubName)
    {
        ClientOptions = new EventHubProducerClientOptions();
    }

    public EventHubSender(EventHubConnection connection, EventHubProducerClientOptions clientOptions = null) : base(connection, clientOptions)
    {
        ClientOptions = clientOptions;
    }

    public EventHubSender(string connectionString, string eventHubName, EventHubProducerClientOptions clientOptions) : base(connectionString, eventHubName, clientOptions)
    {
        ClientOptions = clientOptions;
    }

    public EventHubSender(string fullyQualifiedNamespace, string eventHubName, AzureNamedKeyCredential credential, EventHubProducerClientOptions clientOptions = null) : base(fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        ClientOptions = clientOptions;
    }

    public EventHubSender(string fullyQualifiedNamespace, string eventHubName, AzureSasCredential credential, EventHubProducerClientOptions clientOptions = null) : base(fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        ClientOptions = clientOptions;
    }

    public EventHubSender(string fullyQualifiedNamespace, string eventHubName, TokenCredential credential, EventHubProducerClientOptions clientOptions = null) : base(fullyQualifiedNamespace, eventHubName, credential, clientOptions)
    {
        ClientOptions = clientOptions;
    }

    protected EventHubSender()
    {
        ClientOptions = new EventHubProducerClientOptions();
    }

    public string PartitionId { get; set; } = "";
    public string Path => base.EventHubName;

    protected EventHubProducerClientOptions ClientOptions { get; }

    public Task SendAsync(EventData eventData, CancellationToken cancellationToken)
    {
        return base.SendAsync(new List<EventData> { eventData }, cancellationToken);
    }

    public Task SendBatchAsync(IEnumerable<EventData> eventDataList, CancellationToken cancellationToken)
    {
        return base.SendAsync(eventDataList, cancellationToken);
    }

    public static EventHubSender CreateFromConnectionString(string connectionString)
    {
        return new EventHubSender(connectionString);
    }
}
