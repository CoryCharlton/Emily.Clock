using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Buttons;

namespace Emily.Clock.Events;

public class ButtonEvent : IMediatorEvent
{
    public ButtonEvent(Button button, ButtonEventType type)
    {
        Button = button;
        Type = type;
    }

    public Button Button { get; }

    public ButtonEventType Type { get; }

    public override string ToString() => $"{nameof(ButtonEvent)} - Button: {Button} Type: {Type}";
}

public enum ButtonEventType
{
    DoublePress,
    Holding,
    Press,
    Release
}
