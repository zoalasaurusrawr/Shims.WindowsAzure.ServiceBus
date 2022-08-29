using Azure.Messaging.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Microsoft.ServiceBus;
public class MessageSession
{
	public MessageSession(ServiceBusSessionReceiver receiver)
	{
		this._receiver = receiver;
	}

    private readonly ServiceBusSessionReceiver _receiver;
	public ServiceBusSessionReceiver Receiver => _receiver;

    public string EntityPath => _receiver.EntityPath;
	public string FullyQualifiedNamespace => _receiver.FullyQualifiedNamespace;
	public string Identifier => _receiver.Identifier;
	public bool IsClosed => _receiver.IsClosed;
	public int PrefetchCount => _receiver.PrefetchCount;
	public ServiceBusReceiveMode ReceiveMode => _receiver.ReceiveMode;
	public string SessionId => _receiver.SessionId;
	public DateTimeOffset SessionLockedUntil => _receiver.SessionLockedUntil;
    public Func<BrokeredMessage> Receive => new Func<BrokeredMessage>(OnReceive);

    public BrokeredMessage ReceiveMessage(TimeSpan? maxWaitTime)
	{
		var message = _receiver.ReceiveMessageAsync(maxWaitTime).GetAwaiter().GetResult();
		return new BrokeredMessage(message);
	}

    public BrokeredMessage ReceiveDeferredMessage(long sequenceNumber)
    {
        var message = _receiver.ReceiveDeferredMessageAsync(sequenceNumber).GetAwaiter().GetResult();
        return new BrokeredMessage(message);
    }

    public BrokeredMessage PeekMessage(long? fromSequenceNumber)
    {
        var message = _receiver.PeekMessageAsync(fromSequenceNumber).GetAwaiter().GetResult();
        return new BrokeredMessage(message);
    }

	public void CompleteMessage(BrokeredMessage message)
	{
		_receiver.CompleteMessageAsync(message.ServiceBusReceivedMessage).GetAwaiter().GetResult();
	}

    public void AbandonMessage(BrokeredMessage message)
    {
        _receiver.AbandonMessageAsync(message.ServiceBusReceivedMessage).GetAwaiter().GetResult();
    }

    public void DeadLetterMessage(BrokeredMessage message)
    {
        _receiver.DeadLetterMessageAsync(message.ServiceBusReceivedMessage).GetAwaiter().GetResult();
    }

    public void RenewMessageLock(BrokeredMessage message)
    {
        _receiver.RenewMessageLockAsync(message.ServiceBusReceivedMessage).GetAwaiter().GetResult();
    }

    public void Close()
	{
		_receiver.CloseAsync().GetAwaiter().GetResult();
	}

    public void RenewSessionLock()
    {
        _receiver.RenewSessionLockAsync().GetAwaiter().GetResult();
    }

    private BrokeredMessage OnReceive()
    {
        var result = Receiver.ReceiveMessageAsync().GetAwaiter().GetResult();
        return new BrokeredMessage(result);
    }
}
