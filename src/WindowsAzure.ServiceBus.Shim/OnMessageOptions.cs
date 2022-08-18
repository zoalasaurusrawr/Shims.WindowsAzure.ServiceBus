using Azure.Messaging.ServiceBus;

namespace Microsoft.ServiceBus.Messaging;
public sealed class OnMessageOptions
{
    private int _maxConcurrentCalls;

    private TimeSpan _autoRenewTimeout;

    //
    // Summary:
    //     Gets or sets the maximum number of concurrent calls to the callback the message
    //     pump should initiate.
    //
    // Value:
    //     The maximum number of concurrent calls to the callback.
    public int MaxConcurrentCalls
    {
        get
        {
            return _maxConcurrentCalls;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxConcurrentCalls));
            }

            _maxConcurrentCalls = value;
        }
    }

    //
    // Summary:
    //     Gets or sets a value that indicates whether the message-pump should call Microsoft.ServiceBus.Messaging.QueueClient.Complete(System.Guid)
    //     or Microsoft.ServiceBus.Messaging.SubscriptionClient.Complete(System.Guid) on
    //     messages after the callback has completed processing.
    //
    // Value:
    //     true to complete the message processing automatically on successful execution
    //     of the operation; otherwise, false.
    public bool AutoComplete
    {
        get;
        set;
    }

    internal bool AutoRenewLock => AutoRenewTimeout > TimeSpan.Zero;

    //
    // Summary:
    //     Gets or sets the maximum duration within which the lock will be renewed automatically.
    //     This value should be greater than the longest message lock duration; for example,
    //     the LockDuration Property.
    //
    // Value:
    //     The maximum duration during which locks are automatically renewed. The default
    //     value is 5 minutes, and if you set this value to System.TimeSpan.Zero the lock
    //     will not be automatically renewed.
    public TimeSpan AutoRenewTimeout
    {
        get
        {
            return _autoRenewTimeout;
        }
        set
        {
            TimeoutHelper.ThrowIfNegativeArgument(value, "value");
            _autoRenewTimeout = value;
        }
    }

    internal ServiceBusClient? MessageClientEntity
    {
        get;
        set;
    }

    internal TimeSpan ReceiveTimeOut
    {
        get;
        set;
    }

    //
    // Summary:
    //     Occurs when an exception is received. Enables you to be notified of any errors
    //     encountered by the message pump. When errors are received calls will automatically
    //     be retried, so this is informational.
    public event EventHandler<ExceptionReceivedEventArgs>? ExceptionReceived;

    //
    // Summary:
    //     Initializes a new instance of the Microsoft.ServiceBus.Messaging.OnMessageOptions
    //     class.
    public OnMessageOptions()
    {
        MaxConcurrentCalls = 1;
        AutoComplete = true;
        ReceiveTimeOut = Constants.DefaultOperationTimeout;
        AutoRenewTimeout = Constants.ClientPumpRenewLockTimeout;
    }

    internal void RaiseExceptionReceived(ExceptionReceivedEventArgs e)
    {
        this.ExceptionReceived?.Invoke(MessageClientEntity, e);
    }
}