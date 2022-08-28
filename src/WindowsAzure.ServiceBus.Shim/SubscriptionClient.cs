using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus.Messaging;

public class SubscriptionClient : ServiceBusClient, IAcknowledgeMessageClient
{
    public SubscriptionClient(string connectionString, string topicPath, string subscriptionName)
        : this(connectionString, topicPath, subscriptionName, ServiceBusReceiveMode.PeekLock)
    {
    }

    public SubscriptionClient(string connectionString, string topicPath, string subscriptionName, ServiceBusReceiveMode receiveMode)
        : this(connectionString, topicPath, subscriptionName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock })
    {
    }

    public SubscriptionClient(string connectionString, string topicPath, string subscriptionName, ServiceBusReceiverOptions receiverOptions)
        : base(connectionString)
    {
        _topicPath = topicPath ?? throw new ArgumentNullException(nameof(topicPath));
        _subscriptionName = subscriptionName ?? throw new ArgumentNullException(nameof(subscriptionName));
        ReceiverOptions = receiverOptions;
    }

    private readonly string _topicPath;
    private readonly string _subscriptionName;
    private Action<BrokeredMessage>? _callback;
    private ServiceBusReceiver? _receiver;
    protected ServiceBusReceiver Receiver
    {
        get
        {
            if (_receiver == null)
            {
                _receiver = base.CreateReceiver(_topicPath, _subscriptionName, ReceiverOptions);
            }
            return _receiver;
        }
    }

    public string Name => base.Identifier;
    public Func<BrokeredMessage> Receive => new Func<BrokeredMessage>(OnReceive);
    public ServiceBusReceiverOptions ReceiverOptions { get; }
    private IMessagePump? _messagePump;
    private CancellationToken _defaultToken = new CancellationToken();

    public static SubscriptionClient CreateFromConnectionString(string connectionString, string topicPath, string name)
    {
        var result = new SubscriptionClient(connectionString, topicPath, name);
        return result;
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

    public void Close()
    {
        _messagePump?.StopAsync(_defaultToken).Wait();
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

    private BrokeredMessage OnReceive()
    {
        var result = Receiver.ReceiveMessageAsync().GetAwaiter().GetResult();
        return new BrokeredMessage(result);
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
}
