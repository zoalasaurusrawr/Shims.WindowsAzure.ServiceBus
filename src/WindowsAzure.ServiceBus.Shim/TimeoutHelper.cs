namespace Microsoft.ServiceBus.Messaging
{
    using Microsoft.Azure.ServiceBus.Primitives;
    using System;
    using System.Diagnostics;
    using System.Threading;

    [DebuggerStepThrough]
    struct TimeoutHelper
    {
        DateTime _deadline;
        bool _deadlineSet;
        readonly TimeSpan _originalTimeout;
        public static readonly TimeSpan MaxWait = TimeSpan.FromMilliseconds(int.MaxValue);

        public TimeoutHelper(TimeSpan timeout) :
            this(timeout, false)
        {
        }

        public TimeoutHelper(TimeSpan timeout, bool startTimeout)
        {
            timeout = timeout >= TimeSpan.Zero ? timeout : throw new ArgumentOutOfRangeException(nameof(timeout));

            this._originalTimeout = timeout;
            this._deadline = DateTime.MaxValue;
            this._deadlineSet = (timeout == TimeSpan.MaxValue);

            if (startTimeout && !this._deadlineSet)
            {
                this.SetDeadline();
            }
        }

        public TimeSpan OriginalTimeout => this._originalTimeout;

        public static bool IsTooLarge(TimeSpan timeout)
        {
            return (timeout > TimeoutHelper.MaxWait) && (timeout != TimeSpan.MaxValue);
        }

        public static TimeSpan FromMilliseconds(int milliseconds)
        {
            return milliseconds == Timeout.Infinite
                ? TimeSpan.MaxValue
                : TimeSpan.FromMilliseconds(milliseconds);
        }

        public static int ToMilliseconds(TimeSpan timeout)
        {
            if (timeout == TimeSpan.MaxValue)
            {
                return Timeout.Infinite;
            }

            long ticks = Ticks.FromTimeSpan(timeout);

            return ticks / TimeSpan.TicksPerMillisecond > int.MaxValue
                ? int.MaxValue
                : Ticks.ToMilliseconds(ticks);
        }

        public static TimeSpan Min(TimeSpan val1, TimeSpan val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        public static DateTime Min(DateTime val1, DateTime val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        public static TimeSpan Add(TimeSpan timeout1, TimeSpan timeout2)
        {
            return Ticks.ToTimeSpan(Ticks.Add(Ticks.FromTimeSpan(timeout1), Ticks.FromTimeSpan(timeout2)));
        }

        public static DateTime Add(DateTime time, TimeSpan timeout)
        {
            if (timeout >= TimeSpan.Zero && DateTime.MaxValue - time <= timeout)
            {
                return DateTime.MaxValue;
            }
            if (timeout <= TimeSpan.Zero && DateTime.MinValue - time >= timeout)
            {
                return DateTime.MinValue;
            }
            return time + timeout;
        }

        public static DateTime Subtract(DateTime time, TimeSpan timeout)
        {
            return Add(time, TimeSpan.Zero - timeout);
        }

        public static TimeSpan Divide(TimeSpan timeout, int factor)
        {
            return timeout == TimeSpan.MaxValue
                ? TimeSpan.MaxValue
                : Ticks.ToTimeSpan((Ticks.FromTimeSpan(timeout) / factor) + 1);
        }

        public TimeSpan RemainingTime()
        {
            if (!this._deadlineSet)
            {
                this.SetDeadline();
                return this._originalTimeout;
            }

            if (this._deadline == DateTime.MaxValue)
            {
                return TimeSpan.MaxValue;
            }

            TimeSpan remaining = this._deadline - DateTime.UtcNow;
            return remaining <= TimeSpan.Zero ? TimeSpan.Zero : remaining;
        }

        public TimeSpan ElapsedTime()
        {
            return this._originalTimeout - this.RemainingTime();
        }

        void SetDeadline()
        {
            if (!_deadlineSet)
                throw new InvalidOperationException("Deadline set twice");
            this._deadline = DateTime.UtcNow + this._originalTimeout;
            this._deadlineSet = true;
        }

        public static void ThrowIfNegativeArgument(TimeSpan timeout)
        {
            ThrowIfNegativeArgument(timeout, "timeout");
        }

        public static void ThrowIfNegativeArgument(TimeSpan timeout, string argumentName)
        {
            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void ThrowIfNonPositiveArgument(TimeSpan timeout)
        {
            ThrowIfNonPositiveArgument(timeout, "timeout");
        }

        public static void ThrowIfNonPositiveArgument(TimeSpan timeout, string argumentName)
        {
            if (timeout <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static bool WaitOne(WaitHandle waitHandle, TimeSpan timeout)
        {
            ThrowIfNegativeArgument(timeout);
            if (timeout == TimeSpan.MaxValue)
            {
                waitHandle.WaitOne();
                return true;
            }

            return waitHandle.WaitOne(timeout);
        }
    }
}