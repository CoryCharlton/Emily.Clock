using Emily.Clock.Device.Gpio;
using Microsoft.Extensions.Logging;
using System;
using System.Device.I2c;
using System.Diagnostics.CodeAnalysis;

namespace Emily.Clock.Device.Rtc.Ds3231;

/// <summary>
/// RTC provider backed by a DS3231 connected via I2C.
/// </summary>
public class Ds3231RtcProvider : IRtcProvider
{
    private readonly IGpioProvider _gpioProvider;
    private readonly ILogger _logger;
    private readonly Ds3231RtcOptions _options;
    private Iot.Device.Rtc.Ds3231? _rtc;

    public Ds3231RtcProvider(IGpioProvider gpioProvider, ILogger logger, Ds3231RtcOptions options)
    {
        _gpioProvider = gpioProvider;
        _logger = logger;
        _options = options;
    }

    public DateTime DateTime
    {
        get => ReadDateTime();
        set => SetDateTime(value);
    }

    public bool IsInitialized { get; private set; }

    [MemberNotNull(nameof(_rtc))]
    private void CheckInitialized()
    {
        if (_rtc is null || !IsInitialized)
        {
            throw new InvalidOperationException("NeoPixelStrip is not initialized.");
        }
    }
    
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

    private DateTime ReadDateTime()
    {
        CheckInitialized();
        
        try
        {
            return _rtc.DateTime;
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, "Failed to read time from RTC");
        }

        return DateTime.MinValue;
    }

    private void SetDateTime(DateTime value)
    {
        CheckInitialized();

        try
        {
            _rtc.DateTime = value;
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, "Failed to set time to RTC");
        }
    }
}
