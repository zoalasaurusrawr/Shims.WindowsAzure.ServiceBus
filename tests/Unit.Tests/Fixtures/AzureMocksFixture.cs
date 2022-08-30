using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Unit.Tests.Fixtures;
public class AzureMocksFixture : IDisposable
{
    public AzureMocksFixture()
    {

    }

    public static Dictionary<string, Queue<ServiceBusMessage>> Queue = new Dictionary<string, Queue<ServiceBusMessage>>();

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

    public static void MockSend(string queueOrTopicName, ServiceBusMessage message)
    {
        if (!Queue.ContainsKey(queueOrTopicName))
            Queue[queueOrTopicName] = new Queue<ServiceBusMessage>();

        Queue[queueOrTopicName].Enqueue(message);
    }

    public static ServiceBusMessage? MockReceive(string queueOrTopicName)
    {
        if (!Queue.ContainsKey(queueOrTopicName))
            Queue[queueOrTopicName] = new Queue<ServiceBusMessage>();

        if (Queue[queueOrTopicName].TryDequeue(out var result))
        {
            return result;
        }

        return null;
    }
}
