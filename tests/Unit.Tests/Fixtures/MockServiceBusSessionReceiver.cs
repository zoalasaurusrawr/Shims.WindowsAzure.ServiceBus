using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusSessionReceiver : ServiceBusSessionReceiver
{
    public MockServiceBusSessionReceiver()
    {
    }

    public MockServiceBusSessionReceiver(string name, ServiceBusSessionReceiverOptions options = null)
    {

    }

    public MockServiceBusSessionReceiver(string name, string sessionId, ServiceBusSessionReceiverOptions options = null)
    {

    }

    public MockServiceBusSessionReceiver(string name, string subscriptionName, string sessionId, ServiceBusSessionReceiverOptions options = null)
    {

    }

    public override string FullyQualifiedNamespace => base.FullyQualifiedNamespace;

    public override string EntityPath => base.EntityPath;

    public override ServiceBusReceiveMode ReceiveMode => base.ReceiveMode;

    public override int PrefetchCount => base.PrefetchCount;

    public override string Identifier => base.Identifier;

    public override string SessionId => base.SessionId;

    public override bool IsClosed => base.IsClosed;

    public override DateTimeOffset SessionLockedUntil => base.SessionLockedUntil;

    public override Task AbandonMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return base.AbandonMessageAsync(message, propertiesToModify, cancellationToken);
    }

    public override Task CloseAsync(CancellationToken cancellationToken = default)
    {
        return base.CloseAsync(cancellationToken);
    }

    public override Task CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken = default)
    {
        return base.CompleteMessageAsync(message, cancellationToken);
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return base.DeadLetterMessageAsync(message, propertiesToModify, cancellationToken);
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, string deadLetterReason, string deadLetterErrorDescription = null, CancellationToken cancellationToken = default)
    {
        return base.DeadLetterMessageAsync(message, deadLetterReason, deadLetterErrorDescription, cancellationToken);
    }

    public override Task DeferMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return base.DeferMessageAsync(message, propertiesToModify, cancellationToken);
    }

    public override ValueTask DisposeAsync()
    {
        return base.DisposeAsync();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override Task<BinaryData> GetSessionStateAsync(CancellationToken cancellationToken = default)
    {
        return base.GetSessionStateAsync(cancellationToken);
    }

    public override Task<ServiceBusReceivedMessage> PeekMessageAsync(long? fromSequenceNumber = null, CancellationToken cancellationToken = default)
    {
        return base.PeekMessageAsync(fromSequenceNumber, cancellationToken);
    }

    public override Task<IReadOnlyList<ServiceBusReceivedMessage>> PeekMessagesAsync(int maxMessages, long? fromSequenceNumber = null, CancellationToken cancellationToken = default)
    {
        return base.PeekMessagesAsync(maxMessages, fromSequenceNumber, cancellationToken);
    }

    public override Task<ServiceBusReceivedMessage> ReceiveDeferredMessageAsync(long sequenceNumber, CancellationToken cancellationToken = default)
    {
        return base.ReceiveDeferredMessageAsync(sequenceNumber, cancellationToken);
    }

    public override Task<IReadOnlyList<ServiceBusReceivedMessage>> ReceiveDeferredMessagesAsync(IEnumerable<long> sequenceNumbers, CancellationToken cancellationToken = default)
    {
        return base.ReceiveDeferredMessagesAsync(sequenceNumbers, cancellationToken);
    }

    public override Task<ServiceBusReceivedMessage> ReceiveMessageAsync(TimeSpan? maxWaitTime = null, CancellationToken cancellationToken = default)
    {
        return base.ReceiveMessageAsync(maxWaitTime, cancellationToken);
    }

    public override Task<IReadOnlyList<ServiceBusReceivedMessage>> ReceiveMessagesAsync(int maxMessages, TimeSpan? maxWaitTime = null, CancellationToken cancellationToken = default)
    {
        return base.ReceiveMessagesAsync(maxMessages, maxWaitTime, cancellationToken);
    }

    public override IAsyncEnumerable<ServiceBusReceivedMessage> ReceiveMessagesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        return base.ReceiveMessagesAsync(cancellationToken);
    }

    public override Task RenewMessageLockAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken = default)
    {
        return base.RenewMessageLockAsync(message, cancellationToken);
    }

    public override Task RenewSessionLockAsync(CancellationToken cancellationToken = default)
    {
        return base.RenewSessionLockAsync(cancellationToken);
    }

    public override Task SetSessionStateAsync(BinaryData sessionState, CancellationToken cancellationToken = default)
    {
        return base.SetSessionStateAsync(sessionState, cancellationToken);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
