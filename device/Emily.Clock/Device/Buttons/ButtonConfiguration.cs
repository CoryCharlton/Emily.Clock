using System.Device.Gpio;

namespace Emily.Clock.Device.Buttons;

public class ButtonConfiguration
{
    public PinMode PinMode { get; set; }
    public int Pin { get; set; }
}
