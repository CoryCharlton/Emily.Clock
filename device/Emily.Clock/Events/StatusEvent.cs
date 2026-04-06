using System;
using CCSWE.nanoFramework.Mediator;

namespace Emily.Clock.Events;

public class StatusEvent : IMediatorEvent
{
    public static TimeSpan DefaultTimeout { get; } = System.Threading.Timeout.InfiniteTimeSpan;

    public StatusEvent() : this(string.Empty) { }

    public StatusEvent(string message) : this(message, DefaultTimeout) { }

    public StatusEvent(string message, TimeSpan timeout)
    {
        Message = message;
        Timeout = timeout;
    }

    public string Message { get; }

    // TODO: Is the timeout really necessary?
    public TimeSpan Timeout { get; }
}
