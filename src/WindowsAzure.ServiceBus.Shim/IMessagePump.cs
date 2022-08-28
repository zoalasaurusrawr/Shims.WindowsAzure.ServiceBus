namespace Microsoft.ServiceBus;
public interface IMessagePump
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
