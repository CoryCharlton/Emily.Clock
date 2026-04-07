using CCSWE.nanoFramework.NeoPixel;
using CCSWE.nanoFramework.NeoPixel.Drivers;
using Emily.Clock.Device;

namespace Emily.Clock.Device.Led.NeoPixel;

/// <summary>
/// Configuration for a NeoPixel LED strip.
/// </summary>
public class NeoPixelLedOptions
{
    /// <summary>
    /// The number of LEDs on the strip.
    /// </summary>
    public ushort Count { get; set; }

    /// <summary>
    /// The NeoPixel driver specifying chipset timings and color order.
    /// </summary>
    public NeoPixelDriver Driver { get; set; } = new Ws2812B();

    /// <summary>
    /// The GPIO pin number for the data line.
    /// </summary>
    public byte Pin { get; set; }

    /// <summary>
    /// Optional delegate invoked during <see cref="NeoPixelLedManager.Initialize"/> to configure
    /// platform-specific pin functions before the LED strip is used.
    /// </summary>
    public DevicePreInitializeDelegate? PreInitialize { get; set; }
}
