using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ServiceBus.Messaging;
public class EventHubRuntimeInformation
{
    public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    public int PartitionCount { get; set; } = 0;
    public string[] PartitionIds { get; set; } = new string[0];
    public string Path { get; set; } = string.Empty;
}
