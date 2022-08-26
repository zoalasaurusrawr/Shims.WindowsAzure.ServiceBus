# WindowsAzure.ServiceBus.Shims

[![NuGet version (Shims.WindowsAzure.ServiceBus)](https://img.shields.io/nuget/v/Shims.WindowsAzure.ServiceBus.svg?style=flat-square)](https://www.nuget.org/packages/Shims.WindowsAzure.ServiceBus/)


A path forward for WindowsAzure.ServiceBus users that are moving to modern .NET

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
- QueueClient
- SubscriptionClient
- NamespaceManager
  - Credentials through DefaultAzureCredential

***

## Goals

- Support the most common use cases from WindowsAzure.ServiceBus 6.x
  - BrokeredMessage, TopicClient, QueueClient, SubscriptionClient 

## Non-Goals

- 100% API Compatibility

***

## Roadmap

- Relay Management via NamespaceManager
- Replace ShimMessagePump with a safer impl

***

# Contribution

Want to add support for to help bring parity to the shim API surface? Please do!
