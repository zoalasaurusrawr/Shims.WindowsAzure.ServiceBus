using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Unit.Tests.Fixtures;
public class MockServiceBusRuleManager : ServiceBusRuleManager
{
    public MockServiceBusRuleManager(string path)
    {
    }

    public MockServiceBusRuleManager(string path, string name)
    {
    }

    public override string SubscriptionPath => base.SubscriptionPath;

    public override string FullyQualifiedNamespace => base.FullyQualifiedNamespace;

    public override bool IsClosed => base.IsClosed;

    public override Task CloseAsync(CancellationToken cancellationToken = default)
    {
        return base.CloseAsync(cancellationToken);
    }

    public override Task CreateRuleAsync(string ruleName, RuleFilter filter, CancellationToken cancellationToken = default)
    {
        return base.CreateRuleAsync(ruleName, filter, cancellationToken);
    }

    public override Task CreateRuleAsync(CreateRuleOptions options, CancellationToken cancellationToken = default)
    {
        return base.CreateRuleAsync(options, cancellationToken);
    }

    public override Task DeleteRuleAsync(string ruleName, CancellationToken cancellationToken = default)
    {
        return base.DeleteRuleAsync(ruleName, cancellationToken);
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

    public override IAsyncEnumerable<RuleProperties> GetRulesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        return base.GetRulesAsync(cancellationToken);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
