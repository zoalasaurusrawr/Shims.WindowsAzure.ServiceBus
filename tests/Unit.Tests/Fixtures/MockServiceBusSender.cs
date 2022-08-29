using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusSender : ServiceBusSender
{
    private readonly string _queueOrTopicName;

    public MockServiceBusSender(ServiceBusClient client, string queueOrTopicName) 
        : base(client, queueOrTopicName)
    {
        this._queueOrTopicName = queueOrTopicName;
    }

    public MockServiceBusSender(ServiceBusClient client, string queueOrTopicName, ServiceBusSenderOptions options) 
        : base(client, queueOrTopicName, options)
    {
        this._queueOrTopicName = queueOrTopicName;
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
        AzureMocksFixture.MockSend(_queueOrTopicName, message);

        return Task.FromResult((long)message.GetHashCode());
    }

    public override Task<IReadOnlyList<long>> ScheduleMessagesAsync(IEnumerable<ServiceBusMessage> messages, DateTimeOffset scheduledEnqueueTime, CancellationToken cancellationToken = default)
    {
        foreach (var message in messages)
        {
            AzureMocksFixture.MockSend(_queueOrTopicName, message);
        }

        IReadOnlyList<long> ids = messages.Select(s => (long)s.GetHashCode()).ToList();
        return Task.FromResult(ids);
    }

    public override Task SendMessageAsync(ServiceBusMessage message, CancellationToken cancellationToken = default)
    {
        AzureMocksFixture.MockSend(_queueOrTopicName, message);
        return Task.CompletedTask;
    }

    public override Task SendMessagesAsync(IEnumerable<ServiceBusMessage> messages, CancellationToken cancellationToken = default)
    {
        foreach (var message in messages)
        {
            AzureMocksFixture.MockSend(_queueOrTopicName, message);
        }
        return Task.CompletedTask;
    }

    public override Task SendMessagesAsync(ServiceBusMessageBatch messageBatch, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
