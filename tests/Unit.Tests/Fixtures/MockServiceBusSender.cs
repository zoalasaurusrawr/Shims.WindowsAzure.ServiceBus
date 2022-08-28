using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusSender : ServiceBusSender
{
    public MockServiceBusSender()
    {
    }

    public MockServiceBusSender(ServiceBusClient client, string queueOrTopicName) : base(client, queueOrTopicName)
    {
    }

    public MockServiceBusSender(ServiceBusClient client, string queueOrTopicName, ServiceBusSenderOptions options) : base(client, queueOrTopicName, options)
    {
    }

    public override string FullyQualifiedNamespace => base.FullyQualifiedNamespace;

    public override string EntityPath => base.EntityPath;

    public override bool IsClosed => base.IsClosed;

    public override string Identifier => base.Identifier;

    public override Task CancelScheduledMessageAsync(long sequenceNumber, CancellationToken cancellationToken = default)
    {
        return base.CancelScheduledMessageAsync(sequenceNumber, cancellationToken);
    }

    public override Task CancelScheduledMessagesAsync(IEnumerable<long> sequenceNumbers, CancellationToken cancellationToken = default)
    {
        return base.CancelScheduledMessagesAsync(sequenceNumbers, cancellationToken);
    }

    public override Task CloseAsync(CancellationToken cancellationToken = default)
    {
        return base.CloseAsync(cancellationToken);
    }

    public override ValueTask<ServiceBusMessageBatch> CreateMessageBatchAsync(CancellationToken cancellationToken = default)
    {
        return base.CreateMessageBatchAsync(cancellationToken);
    }

    public override ValueTask<ServiceBusMessageBatch> CreateMessageBatchAsync(CreateMessageBatchOptions options, CancellationToken cancellationToken = default)
    {
        return base.CreateMessageBatchAsync(options, cancellationToken);
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

    public override Task<long> ScheduleMessageAsync(ServiceBusMessage message, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default)
    {
        return base.ScheduleMessageAsync(message, scheduledEnqueueTime, cancellationToken);
    }

    public override Task<IReadOnlyList<long>> ScheduleMessagesAsync(IEnumerable<ServiceBusMessage> messages, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default)
    {
        return base.ScheduleMessagesAsync(messages, scheduledEnqueueTime, cancellationToken);
    }

    public override Task SendMessageAsync(ServiceBusMessage message, CancellationToken cancellationToken = default)
    {
        return base.SendMessageAsync(message, cancellationToken);
    }

    public override Task SendMessagesAsync(IEnumerable<ServiceBusMessage> messages, CancellationToken cancellationToken = default)
    {
        return base.SendMessagesAsync(messages, cancellationToken);
    }

    public override Task SendMessagesAsync(ServiceBusMessageBatch messageBatch, CancellationToken cancellationToken = default)
    {
        return base.SendMessagesAsync(messageBatch, cancellationToken);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
