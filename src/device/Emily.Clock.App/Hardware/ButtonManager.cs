using System;
using System.Device.Gpio;
using Emily.Clock.Device.Gpio;
using MakoIoT.Device.Services.Mediator;

namespace Emily.Clock.App.Hardware
{
    public class ButtonManager : ButtonManagerBase
    {
        public ButtonManager(IGpioProvider gpioProvider, IMediator mediator) : base(gpioProvider, mediator)
        {
        }

        protected override int GetPin(Button button)
        {
            return button switch
            {
                Button.One => 39,
                Button.Two => 37,
                Button.Three => 38,
                _ => throw new ArgumentOutOfRangeException(nameof(button))
            };
        }

        protected override PinMode GetPinMode(Button button)
        {
            return PinMode.InputPullUp;
        }
    }
}
