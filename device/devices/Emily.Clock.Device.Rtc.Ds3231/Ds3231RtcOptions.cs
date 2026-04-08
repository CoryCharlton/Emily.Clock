using Emily.Clock.Device;

namespace Emily.Clock.Device.Rtc.Ds3231;

/// <summary>
/// Configuration for a DS3231 RTC device.
/// </summary>
public class Ds3231RtcOptions
{
    /// <summary>
    /// The I2C bus ID. Typically 1 on ESP32 platforms.
    /// </summary>
    public int BusId { get; set; } = 1;

    /// <summary>
    /// The I2C address of the DS3231. Defaults to <see cref="Iot.Device.Rtc.Ds3231.DefaultI2cAddress"/>.
    /// </summary>
    public byte I2cAddress { get; set; } = Iot.Device.Rtc.Ds3231.DefaultI2cAddress;

    /// <summary>
    /// Optional delegate invoked during <see cref="Ds3231RtcProvider.Initialize"/> to configure
    /// platform-specific pin functions before the I2C device is used.
    /// </summary>
    public DevicePreInitializeDelegate? PreInitialize { get; set; }
}
