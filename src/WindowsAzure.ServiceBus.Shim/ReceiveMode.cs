using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ServiceBus;

public enum ReceiveMode
{
    /// <summary>
    /// Once a message is received in this mode, the receiver has a lock on the message for a
    /// particular duration. If the message is not settled by this time, it lands back on Service Bus
    /// to be fetched by the next receive operation.
    /// </summary>
    ///
    /// <remarks>This is the default value for <see cref="ServiceBusReceiveMode" />, and should be used for guaranteed delivery.</remarks>
    PeekLock,

    /// <summary>
    /// ReceiveAndDelete will delete the message from Service Bus as soon as the message is delivered.
    /// </summary>
    ReceiveAndDelete,
}