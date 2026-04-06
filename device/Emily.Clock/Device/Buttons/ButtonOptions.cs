using System;

namespace Emily.Clock.Device.Buttons;

public class ButtonOptions
{
    public ButtonOptions(ButtonConfiguration buttonOne, ButtonConfiguration buttonTwo, ButtonConfiguration buttonThree)
    {
        ButtonOne = buttonOne;
        ButtonTwo = buttonTwo;
        ButtonThree = buttonThree;
    }

    public ButtonConfiguration ButtonOne { get; }
    public ButtonConfiguration ButtonTwo { get; }
    public ButtonConfiguration ButtonThree { get; }

    public TimeSpan DebounceTime { get; set; } = TimeSpan.FromMilliseconds(50);
    public TimeSpan DoublePressTime { get; set; } = TimeSpan.FromMilliseconds(1500);
    public TimeSpan HoldingTime { get; set; } = TimeSpan.FromMilliseconds(1000);

    internal ButtonConfiguration GetConfiguration(Button button)
    {
        return button switch
        {
            Button.One => ButtonOne,
            Button.Two => ButtonTwo,
            Button.Three => ButtonThree,
            _ => throw new ArgumentOutOfRangeException(nameof(button))
        };
    }
}
