using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ServiceBus;
public class ReceiverOptions
{
    public ReceiverOptions()
    {
    }

    public bool EnableReceiverRuntimeMetric { get; set; } = false;
    public string Identifier { get; set; } = "";
}
