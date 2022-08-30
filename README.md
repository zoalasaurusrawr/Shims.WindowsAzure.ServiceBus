# Shims.WindowsAzure.ServiceBus

[![.NET](https://github.com/zoeysaurusrex/Shims.WindowsAzure.ServiceBus/actions/workflows/dotnet.yml/badge.svg)](https://github.com/zoeysaurusrex/Shims.WindowsAzure.ServiceBus/actions/workflows/dotnet.yml)

[![NuGet version (Shims.WindowsAzure.ServiceBus)](https://img.shields.io/nuget/v/Shims.WindowsAzure.ServiceBus.svg?style=flat-square)](https://www.nuget.org/packages/Shims.WindowsAzure.ServiceBus/)


A path forward for WindowsAzure.ServiceBus users that are moving to modern .NET

Shims.WindowsAzure.ServiceBus provides a way for WindowsAzure.ServiceBus users to move to modern .NET by providing a set of facades and interfaces utilize the latest Azure SDKs. The primary goal is to support the most common features and use cases so that replacing WindowsAzure.ServiceBus with this library can be a 1:1 drop in for most things. In the case where a usage scenario isn't supported in this library, the Azure SDK interfaces should still be accessible (i.e. EventHubSender inherits EventHubProducerClient).

## Getting Started

1. Uninstall WindowsAzure.ServiceBus
2. Install Shims.WindowsAzure.ServiceBus `dotnet package add Shims.WindowsAzure.ServiceBus`
3. Checkout the [.NET Framework and .NET sample runners](https://github.com/zoeysaurusrex/WindowsAzure.ServiceBus.Shims/blob/main/samples/WindowsAzure.ServiceBus.Net.Runner/Program.cs) to see what's supported so far.

### Notes

**NamespaceManager**

In order to deal with compatibility issues in the management clients, you must provide the Azure ResourceId for the Service Bus. NamespaceManager provides a constructor overload to take the ResourceId. Optionally, you can put the id into appsettings.*.json and NamespaceManager will attempt to get it from there during construction. The configuration looks like the following:

`
"ServiceBus": {
        "ResourceId": ""
}
`

In order to get minimal support for NamespaceManager, currently credentials utilize DefaultAzureCredential. In WindowsAzure.ServiceBus, you would normally construct NamespaceManager somewhat like this:

`
TokenProvider.CreateSharedAccessSignatureTokenProvider(ServiceBusOptions.ServiceBusKeyName, ServiceBusOptions.ServiceBusKey);
`

You will need to change it to this:

`
var tokenProvider = TokenProvider.CreateTokenProvider();
`

***

## Compatibility

WindowsAzure.ServiceBus >= 6.X
- TopicClient
  - Supported Through: Azure.Messaging.ServiceBus
  - CreateFromConnectionString
  - Send
  
- QueueClient
  - Supported Through: Azure.Messaging.ServiceBus
  - CreateFromConnectionString
  - Send
  
- SubscriptionClient
  - Supported Through: Azure.Messaging.ServiceBus
  - CreateFromConnectionString
  - Send
  
- NamespaceManager
  - Supported Through: Azure.ResourceManager.ServiceBus
  - Credentials through DefaultAzureCredential
  - CRUD operations for Topics, Subscriptions, and Queues (Sync and Async)
  - Entity Description
  
- MessagingFactory
  - Supported Through: Azure.Messaging.ServiceBus
  - Creation of Topic, Queue, and Subscription Clients

- EventHubClient
  - Supported Through: Azure.Messaging.EventHubs
  - Senders: EventHubSender, PartitionSender, 
  - Receivers: EventHubReceiver, PartitionReceiver

**Rebuilt Types**

- ServiceBusConnectionStringBuilder
  - Connection string constructor only
  
- BrokeredMessage

***

## Goals

- Support the most common use cases from WindowsAzure.ServiceBus 6.x
  - BrokeredMessage, TopicClient, QueueClient, SubscriptionClient 

## Non-Goals

- 100% API Compatibility

***

## Roadmap

- NamespaceManager
  - Relay CRUD operations
- QueueClient
  - Async Send
- TopicClient
  - Async Send
- SubscriptionClient
  - Replace ShimMessagePump with a safer impl

***

# Contribution

Want to add support for to help bring parity to the shim API surface? Please do! You can use the "help wanted" label to see needed areas, though, helpful contributions are welcome regardless of the roadmap
