using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus.Messaging;

public class QueueClient : ServiceBusClient, IDisposable, IMessageClient, IAcknowledgeMessageClient
{
    public QueueClient(string connectionString, string queueName)
        : base(connectionString)
    {
        _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
    }

    private readonly string _queueName;
    private Action<BrokeredMessage>? _callback;
    private ServiceBusSender? _sender;
    protected ServiceBusSender Sender
    {
        get
        {
            if (_sender == null)
                _sender = base.CreateSender(_queueName);

            return _sender;
        }
    }

    private ServiceBusReceiver? _receiver;
    protected ServiceBusReceiver Receiver
    {
        get
        {
            if (_receiver == null)
                _receiver = base.CreateReceiver(_queueName);

            return _receiver;
        }
    }

    private IMessagePump? _messagePump;
    private CancellationToken _defaultToken = new CancellationToken();

    public static QueueClient CreateFromConnectionString(string connectionString, string queueName)
    {
        var result = new QueueClient(connectionString, queueName);
        return result;
    }

    public void Send(BrokeredMessage message)
    {
        Sender.SendMessageAsync(message).Wait();
    }

    public void OnMessage(Action<BrokeredMessage> callback)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        AsyncHelpers.RunSync(() => RunAsync(callback, new OnMessageOptions()));
    }

    public void OnMessage(Action<BrokeredMessage> callback, OnMessageOptions onMessageOptions)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        onMessageOptions ??= new OnMessageOptions();
        onMessageOptions.MessageClientEntity = this;
        AsyncHelpers.RunSync(() => RunAsync(callback, onMessageOptions));
    }

    private async Task RunAsync(Action<BrokeredMessage> callback, OnMessageOptions onMessageOptions)
    {
        _callback = callback;
        _messagePump = new ShimMessagePump(Receiver, InternalCallback, onMessageOptions);
        await _messagePump.StartAsync(_defaultToken);
    }

    private void InternalCallback(BrokeredMessage message)
    {
        message.AcknowledgeMessageClient = this;

        _callback?.Invoke(message);
    }

    public void Close()
    {
        _messagePump?.StopAsync(_defaultToken).Wait();
    }

    public void CompleteMessage(BrokeredMessage brokeredMessage)
    {
        if (brokeredMessage == null)
            throw new ArgumentNullException(nameof(brokeredMessage));

        Receiver.CompleteMessageAsync(brokeredMessage.ServiceBusReceivedMessage).Wait();
    }

    public void AbandonMessage(BrokeredMessage brokeredMessage)
    {
        if (brokeredMessage == null)
            throw new ArgumentNullException(nameof(brokeredMessage));

        Receiver.AbandonMessageAsync(brokeredMessage.ServiceBusReceivedMessage).Wait();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Close();
        }
    }
}
