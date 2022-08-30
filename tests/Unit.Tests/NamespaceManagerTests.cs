using System;
using Microsoft.ServiceBus;
using Unit.Tests.Fixtures;
using Xunit;

namespace Unit.Tests;

public class NamespaceManagerTests : IClassFixture<AzureMocksFixture>
{
    public NamespaceManagerTests(AzureMocksFixture fixture)
    {
        Fixture = fixture;
    }

    internal AzureMocksFixture Fixture { get; }

    [Fact]
    public void Construction_Succeeds_When_Uri_Is_Valid()
    {
        var scheme = Fixture.GetDefaultScheme();
        var namespaceName = Fixture.GetNamespaceName();
        var resourceId = Fixture.GenerateServiceBusNamespaceResourceId();
        var serviceBusUri = ServiceBusEnvironment.CreateServiceUri(scheme, namespaceName, string.Empty);
        var namespaceManager = new NamespaceManager(serviceBusUri, TokenProvider.CreateTokenProvider(), resourceId);
        Assert.NotNull(namespaceManager);
    }

    [Fact]
    public void Construction_Throws_ArgumentException_When_ResourceId_Is_Not_Valid()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var scheme = Fixture.GetDefaultScheme();
            var namespaceName = Fixture.GetNamespaceName();
            var resourceId = string.Empty;
            var serviceBusUri = ServiceBusEnvironment.CreateServiceUri(scheme, namespaceName, string.Empty);
            var namespaceManager = new NamespaceManager(serviceBusUri, TokenProvider.CreateTokenProvider(), resourceId);
        });
    }
}