namespace Microsoft.ServiceBus.Messaging;
public class EventHubPartitionRuntimeInformation
{
    public EventHubPartitionRuntimeInformation()
    {
    }

    public long BeginSequenceNumber { get; set; } = 0;
    public bool IsEmpty { get; set; } = false;
    public string LastEnqueuedOffset { get; set; } = "";
    public long LastEnqueuedSequenceNumber { get; set; } = 0;
    public DateTime LastEnqueuedTimeUtc { get; set; } = DateTime.MinValue;
    public string PartitionId { get; set; } = "";
    public string Path { get; set; } = "";
}
