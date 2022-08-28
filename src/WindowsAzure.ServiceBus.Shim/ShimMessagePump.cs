using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus.Messaging;

internal class ShimMessagePump : IMessagePump, IDisposable
{
    public ShimMessagePump(ServiceBusReceiver receiver, Action<BrokeredMessage> callback, OnMessageOptions onMessageOptions)
    {
        Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        OnMessageOptions = onMessageOptions ?? throw new ArgumentNullException(nameof(onMessageOptions));
    }

    private Timer? _pump;
    public ServiceBusReceiver Receiver { get; }
    public Action<BrokeredMessage> Callback { get; }
    public OnMessageOptions OnMessageOptions { get; }

    public void Run()
    {
        _pump = new Timer(new TimerCallback(OnMessageReceived));
        _pump.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));
    }

    public void Stop()
    {
        _pump?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    private void OnMessageReceived(object? state)
    {
        var message = Receiver.ReceiveMessageAsync(OnMessageOptions.ReceiveTimeOut).GetAwaiter().GetResult();
        var result = BrokeredMessage.Create(message);
        Callback(result);
    }

    public void Dispose()
    {
        _pump?.Change(Timeout.Infinite, Timeout.Infinite);
        _pump?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationToken = new CancellationToken(true);
        return Task.CompletedTask;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await Receiver.ReceiveMessageAsync(OnMessageOptions.ReceiveTimeOut);
            var result = BrokeredMessage.Create(message);
            Callback(result);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
        }
    }
}
