using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusClient : ServiceBusClient
{
    public override string FullyQualifiedNamespace => FullyQualifiedNamespace;

    public override bool IsClosed => IsClosed;

    public override string Identifier => Identifier;

    public override Task<ServiceBusSessionReceiver> AcceptNextSessionAsync(string queueName, ServiceBusSessionReceiverOptions options = null, CancellationToken cancellationToken = default)
    {
        ServiceBusSessionReceiver receiver = new MockServiceBusSessionReceiver(queueName, options);
        return Task.FromResult(receiver);
    }

    public override Task<ServiceBusSessionReceiver> AcceptNextSessionAsync(string topicName, string subscriptionName, ServiceBusSessionReceiverOptions options = null, CancellationToken cancellationToken = default)
    {
        ServiceBusSessionReceiver receiver = new MockServiceBusSessionReceiver(topicName, subscriptionName, options);
        return Task.FromResult(receiver);
    }

    public override Task<ServiceBusSessionReceiver> AcceptSessionAsync(string queueName, string sessionId, ServiceBusSessionReceiverOptions options = null, CancellationToken cancellationToken = default)
    {
        ServiceBusSessionReceiver receiver = new MockServiceBusSessionReceiver(queueName, sessionId, options);
        return Task.FromResult(receiver);
    }

    public override Task<ServiceBusSessionReceiver> AcceptSessionAsync(string topicName, string subscriptionName, string sessionId, ServiceBusSessionReceiverOptions options = null, CancellationToken cancellationToken = default)
    {
        ServiceBusSessionReceiver receiver = new MockServiceBusSessionReceiver(topicName, subscriptionName, sessionId, options);
        return Task.FromResult(receiver);
    }

    public override ServiceBusProcessor CreateProcessor(string queueName)
    {
        return new MockServiceBusProcessor(this, queueName, new ServiceBusProcessorOptions());
    }

    public override ServiceBusProcessor CreateProcessor(string queueName, ServiceBusProcessorOptions options)
    {
        return new MockServiceBusProcessor(this, queueName, options);
    }

    public override ServiceBusProcessor CreateProcessor(string topicName, string subscriptionName)
    {
        return new MockServiceBusProcessor(this, topicName, subscriptionName, new ServiceBusProcessorOptions());
    }

    public override ServiceBusProcessor CreateProcessor(string topicName, string subscriptionName, ServiceBusProcessorOptions options)
    {
        return new MockServiceBusProcessor(this, topicName, subscriptionName, options);
    }

    public override ServiceBusReceiver CreateReceiver(string queueName)
    {
        return new MockServiceBusReceiver(this, queueName, new ServiceBusReceiverOptions());
    }

    public override ServiceBusReceiver CreateReceiver(string queueName, ServiceBusReceiverOptions options)
    {
        return new MockServiceBusReceiver(this, queueName, options);
    }

    public override ServiceBusReceiver CreateReceiver(string topicName, string subscriptionName)
    {
        return new MockServiceBusReceiver(this, topicName, subscriptionName, new ServiceBusReceiverOptions());
    }

    public override ServiceBusReceiver CreateReceiver(string topicName, string subscriptionName, ServiceBusReceiverOptions options)
    {
        return new MockServiceBusReceiver(this, topicName, subscriptionName, options);
    }

    public override ServiceBusRuleManager CreateRuleManager(string topicName, string subscriptionName)
    {
        return new MockServiceBusRuleManager(topicName, subscriptionName);
    }

    public override ServiceBusSender CreateSender(string queueOrTopicName)
    {
        return new MockServiceBusSender(this, queueOrTopicName);
    }

    public override ServiceBusSender CreateSender(string queueOrTopicName, ServiceBusSenderOptions options)
    {
        return new MockServiceBusSender(this, queueOrTopicName, options);
    }

    public override ServiceBusSessionProcessor CreateSessionProcessor(string queueName, ServiceBusSessionProcessorOptions options = null)
    {
        return new MockServiceBusSessionProcessor(this, queueName, options);
    }

    public override ServiceBusSessionProcessor CreateSessionProcessor(string topicName, string subscriptionName, ServiceBusSessionProcessorOptions options = null)
    {
        return new MockServiceBusSessionProcessor(this, topicName, subscriptionName, options);
    }

    public override ValueTask DisposeAsync()
    {
        return DisposeAsync();
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj);
    }

    public override int GetHashCode()
    {
        return GetHashCode();
    }

    public override string? ToString()
    {
        return ToString();
    }
}
