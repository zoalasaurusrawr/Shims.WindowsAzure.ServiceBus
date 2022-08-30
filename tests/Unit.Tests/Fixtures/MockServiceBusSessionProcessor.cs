using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Unit.Tests.Fixtures;
public class MockServiceBusSessionProcessor : ServiceBusSessionProcessor
{
    public MockServiceBusSessionProcessor()
    {
    }

    public MockServiceBusSessionProcessor(ServiceBusClient client, string queueName, ServiceBusSessionProcessorOptions options) : base(client, queueName, options)
    {
    }

    public MockServiceBusSessionProcessor(ServiceBusClient client, string topicName, string subscriptionName, ServiceBusSessionProcessorOptions options) : base(client, topicName, subscriptionName, options)
    {
    }

    public override string EntityPath => base.EntityPath;

    public override string Identifier => base.Identifier;

    public override ServiceBusReceiveMode ReceiveMode => base.ReceiveMode;

    public override int PrefetchCount => base.PrefetchCount;

    public override bool IsProcessing => base.IsProcessing;

    public override bool AutoCompleteMessages => base.AutoCompleteMessages;

    public override bool IsClosed => base.IsClosed;

    public override TimeSpan MaxAutoLockRenewalDuration => base.MaxAutoLockRenewalDuration;

    public override int MaxConcurrentSessions => base.MaxConcurrentSessions;

    public override int MaxConcurrentCallsPerSession => base.MaxConcurrentCallsPerSession;

    public override string FullyQualifiedNamespace => base.FullyQualifiedNamespace;

    public override TimeSpan? SessionIdleTimeout => base.SessionIdleTimeout;

    protected override ServiceBusProcessor InnerProcessor => base.InnerProcessor;

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

    protected override Task OnProcessSessionMessageAsync(ProcessSessionMessageEventArgs args)
    {
        return base.OnProcessSessionMessageAsync(args);
    }

    protected override Task OnSessionClosingAsync(ProcessSessionEventArgs args)
    {
        return base.OnSessionClosingAsync(args);
    }

    protected override Task OnSessionInitializingAsync(ProcessSessionEventArgs args)
    {
        return base.OnSessionInitializingAsync(args);
    }
}
