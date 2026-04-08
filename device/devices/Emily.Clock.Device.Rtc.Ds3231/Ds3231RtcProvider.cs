using System;
using System.Device.I2c;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.Rtc;
using Iot.Device.Rtc;

namespace Emily.Clock.Device.Rtc.Ds3231;

/// <summary>
/// RTC provider backed by a DS3231 connected via I2C.
/// </summary>
public class Ds3231RtcProvider : IRtcProvider
{
    private readonly IGpioProvider _gpioProvider;
    private readonly Ds3231RtcOptions _options;
    private Iot.Device.Rtc.Ds3231? _rtc;

    public Ds3231RtcProvider(IGpioProvider gpioProvider, Ds3231RtcOptions options)
    {
        _gpioProvider = gpioProvider;
        _options = options;
    }

    public DateTime DateTime
    {
        get => _rtc!.DateTime;
        set => _rtc!.DateTime = value;
    }

    public bool IsInitialized { get; private set; }

    public bool Initialize()
    {
        if (IsInitialized)
        {
            return true;
        }

        _options.PreInitialize?.Invoke(_gpioProvider);

        var settings = new I2cConnectionSettings(_options.BusId, _options.I2cAddress);
        var i2cDevice = I2cDevice.Create(settings);
        _rtc = new Iot.Device.Rtc.Ds3231(i2cDevice);
        IsInitialized = true;

        return true;
    }
}
