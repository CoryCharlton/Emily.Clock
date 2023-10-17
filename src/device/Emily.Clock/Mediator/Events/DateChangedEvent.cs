using System;
using CCSWE.nanoFramework.Mediator;

namespace Emily.Clock.Mediator.Events
{
    public class DateChangedEvent : IMediatorEvent
    {
        public DateChangedEvent(DateTime date)
        {
            Date = date.Date;
        }

        public DateTime Date { get; }

        // ReSharper disable once SimplifyStringInterpolation
        public override string ToString() => $"{nameof(DateChangedEvent)} - Date: {Date.ToString("dddd, MMMM dd")}";
    }
}
