using System;

namespace Unit.Tests.Fixtures;
public class AzureMocksFixture : IDisposable
{
    public AzureMocksFixture()
    {

    }

    public string GenerateServiceBusNamespaceResourceId()
    {
        return $"/subscriptions/{Guid.NewGuid()}/resourceGroups/fake-rg/providers/Microsoft.ServiceBus/namespaces/fake-namespace";
    }

    public string GetNamespaceName()
    {
        return "fake-namespace";
    }

    public string GetDefaultScheme()
    {
        return "sb";
    }

    public void Dispose()
    {
    }
}
