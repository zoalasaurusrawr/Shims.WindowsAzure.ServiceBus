# WindowsAzure.ServiceBus.Shims

![Github.io linkchecker](https://img.shields.io/nuget/v/Shims.WindowsAzure.ServiceBus)


A path forward for WindowsAzure.ServiceBus users that are moving to modern .NET

## Getting Started

1. Uninstall WindowsAzure.ServiceBus
2. Install Shims.WindowsAzure.ServiceBus `dotnet package add Shims.WindowsAzure.ServiceBus`

## Compatibility

WindowsAzure.ServiceBus >= 6.X

## Goals

- Support the most common use cases from WindowsAzure.ServiceBus 6.x
  - BrokeredMessage, TopicClient, QueueClient, SubscriptionClient 

## Non-Goals

- 100% API Compatibility

## Roadmap

- NamespaceManager
