
namespace Microsoft.ServiceBus.Messaging;

public interface IMessageClient
{
    void OnMessage(Action<BrokeredMessage> callback);
    void OnMessage(Action<BrokeredMessage> callback, OnMessageOptions onMessageOptions);
    void Send(BrokeredMessage message);
}