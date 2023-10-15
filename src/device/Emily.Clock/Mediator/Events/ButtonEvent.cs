using Emily.Clock.Device.Gpio;
using MakoIoT.Device.Services.Mediator;

namespace Emily.Clock.Mediator.Events
{
    public class ButtonEvent : IEvent
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
}
