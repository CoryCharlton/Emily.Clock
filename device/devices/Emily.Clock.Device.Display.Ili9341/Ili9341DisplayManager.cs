using System.Device.Gpio;
using System.Threading;
using Emily.Clock.Device.Gpio;
using Microsoft.Extensions.Logging;
using nanoFramework.UI;
using Ili9341Driver = nanoFramework.UI.GraphicDrivers.Ili9341;

namespace Emily.Clock.Device.Display.Ili9341;

internal class Ili9341DisplayManager : DisplayManagerBase
{
    private readonly IGpioProvider _gpioProvider;
    private readonly ILogger _logger;

    private SpiDisplayOptions SpiOptions => (SpiDisplayOptions)_options;

    public Ili9341DisplayManager(IGpioProvider gpioProvider, ILogger logger, SpiDisplayOptions options)
        : base(options)
    {
        _gpioProvider = gpioProvider;
        _logger = logger;
    }

    protected override bool InitializeDisplay()
    {
        SpiOptions.PreInitialize?.Invoke(_gpioProvider);

        if (SpiOptions.BacklightPin > 0)
        {
            _gpioProvider.OpenPin(SpiOptions.BacklightPin, PinMode.Output);
        }

        var dcPin = SpiOptions.SpiConfiguration.DataCommand;

        _gpioProvider.OpenPin(dcPin, PinMode.OutputOpenDrain);
        _gpioProvider.Write(dcPin, PinValue.Low);

        Thread.Sleep(100);

        _gpioProvider.Write(dcPin, PinValue.High);

        var graphicDriver = Ili9341Driver.GraphicDriverWithDefaultManufacturingSettings;
        var screenConfiguration = new ScreenConfiguration(0, 0, Width, Height, graphicDriver);

        DisplayControl.Initialize(SpiOptions.SpiConfiguration, screenConfiguration, GetBufferSize());
        DisplayControl.ChangeOrientation(SpiOptions.Orientation);

        return true;
    }

    public override void SetBackLight(bool enabled)
    {
        _gpioProvider.Write(SpiOptions.BacklightPin, enabled ? PinValue.High : PinValue.Low);
    }
}
