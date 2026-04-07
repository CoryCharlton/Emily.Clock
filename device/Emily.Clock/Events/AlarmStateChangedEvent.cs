using CCSWE.nanoFramework.Mediator;

namespace Emily.Clock.Events;

public class AlarmStateChangedEvent : IMediatorEvent
{
    public AlarmStateChangedEvent(bool enabled, bool isAlarming)
    {
        Enabled = enabled;
        IsAlarming = isAlarming;
    }

    public bool Enabled { get; }

    public bool IsAlarming { get; }

    public override string ToString() => $"{nameof(AlarmStateChangedEvent)} - Enabled: {Enabled}, IsAlarming: {IsAlarming}";
}
