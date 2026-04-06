using System;
using CCSWE.nanoFramework.Mediator;

namespace Emily.Clock.Events;

public class TimeChangedEvent : IMediatorEvent
{
    public TimeChangedEvent(DateTime time)
    {
        Time = time.Time();
    }

    public DateTime Time { get; }

    // ReSharper disable once SimplifyStringInterpolation
    public override string ToString() => $"{nameof(TimeChangedEvent)} - Time: {Time.ToString("h:mm")}";
}
