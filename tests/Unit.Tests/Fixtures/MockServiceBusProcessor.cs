using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusProcessor : ServiceBusProcessor
{
    public MockServiceBusProcessor()
    {
    }

    public MockServiceBusProcessor(ServiceBusClient client, string queueName, ServiceBusProcessorOptions options) : base(client, queueName, options)
    {
    }

    public MockServiceBusProcessor(ServiceBusClient client, string topicName, string subscriptionName, ServiceBusProcessorOptions options) : base(client, topicName, subscriptionName, options)
    {
    }

    public override string FullyQualifiedNamespace => base.FullyQualifiedNamespace;

    public override string EntityPath => base.EntityPath;

    public override string Identifier => base.Identifier;

    public override ServiceBusReceiveMode ReceiveMode => base.ReceiveMode;

    public override int PrefetchCount => base.PrefetchCount;

    public override bool IsProcessing => base.IsProcessing;

    public override int MaxConcurrentCalls => base.MaxConcurrentCalls;

    public override bool AutoCompleteMessages => base.AutoCompleteMessages;

    public override TimeSpan MaxAutoLockRenewalDuration => base.MaxAutoLockRenewalDuration;

    public override bool IsClosed => base.IsClosed;

    public override Task CloseAsync(CancellationToken cancellationToken = default)
    {
        return base.CloseAsync(cancellationToken);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override Task StartProcessingAsync(CancellationToken cancellationToken = default)
    {
        return base.StartProcessingAsync(cancellationToken);
    }

    public override Task StopProcessingAsync(CancellationToken cancellationToken = default)
    {
        return base.StopProcessingAsync(cancellationToken);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override Task OnProcessErrorAsync(ProcessErrorEventArgs args)
    {
        return base.OnProcessErrorAsync(args);
    }

    protected override Task OnProcessMessageAsync(ProcessMessageEventArgs args)
    {
        return base.OnProcessMessageAsync(args);
    }
}
