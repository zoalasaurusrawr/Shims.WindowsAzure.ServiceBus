using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusReceiver : ServiceBusReceiver
{
    public MockServiceBusReceiver()
    {
    }

    public MockServiceBusReceiver(ServiceBusClient client, string queueName, ServiceBusReceiverOptions options) : base(client, queueName, options)
    {
    }

    public MockServiceBusReceiver(ServiceBusClient client, string topicName, string subscriptionName, ServiceBusReceiverOptions options) : base(client, topicName, subscriptionName, options)
    {
    }

    public override string FullyQualifiedNamespace => base.FullyQualifiedNamespace;

    public override string EntityPath => base.EntityPath;

    public override ServiceBusReceiveMode ReceiveMode => base.ReceiveMode;

    public override int PrefetchCount => base.PrefetchCount;

    public override string Identifier => base.Identifier;

    public override bool IsClosed => base.IsClosed;

    public override Task AbandonMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task CloseAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, string deadLetterReason, string deadLetterErrorDescription = null, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task DeferMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
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

    public override string ToString()
    {
        return base.ToString();
    }
}
