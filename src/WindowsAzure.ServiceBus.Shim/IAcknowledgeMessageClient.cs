using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ServiceBus.Messaging;
public interface IAcknowledgeMessageClient
{
    void CompleteMessage(BrokeredMessage brokeredMessage);
    void AbandonMessage(BrokeredMessage brokeredMessage);
}
