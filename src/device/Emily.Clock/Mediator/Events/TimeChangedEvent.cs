using MakoIoT.Device.Services.Mediator;
using System;

namespace Emily.Clock.Mediator.Events
{
    public class TimeChangedEvent : IEvent
    {
        public TimeChangedEvent(DateTime time)
        {
            Time = time.Time();
        }

        public DateTime Time { get; }

        // ReSharper disable once SimplifyStringInterpolation
        public override string ToString() => $"{nameof(TimeChangedEvent)} - Time: {Time.ToString("h:mm")}";
    }
}
