using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Unit.Tests.Fixtures;
using Xunit;

namespace Unit.Tests;
public class ServiceBusEnvironmentTests : IClassFixture<AzureMocksFixture>
{
    public ServiceBusEnvironmentTests(AzureMocksFixture fixture)
    {
        Fixture = fixture;
    }

    internal AzureMocksFixture Fixture { get; }

    [Fact]
    public void CreateServiceUri_Succeeds()
    {
        var expectedResult = new Uri("sb://fake-namespace.servicebus.windows.net/");
        var scheme = Fixture.GetDefaultScheme();
        var namespaceName = Fixture.GetNamespaceName();
        var serviceBusUri = ServiceBusEnvironment.CreateServiceUri(scheme, namespaceName, string.Empty);
        Assert.Equal(expectedResult, serviceBusUri);
    }

    [Theory]
    [InlineData("fake")]
    [InlineData(null)]
    public void CreateServiceUri_Throws_ArgumentException_When_Scheme_Is_Not_Valid(string scheme)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var namespaceName = Fixture.GetNamespaceName();
            var serviceBusUri = ServiceBusEnvironment.CreateServiceUri(scheme, namespaceName, string.Empty);
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateServiceUri_Throws_ArgumentException_When_Namespace_Is_Not_Valid(string namespaceName)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var scheme = Fixture.GetDefaultScheme();
            var expectedResult = new Uri("sb://fake-namespace.servicebus.windows.net/");
            var serviceBusUri = ServiceBusEnvironment.CreateServiceUri(scheme, namespaceName, string.Empty);
        });
    }
}
