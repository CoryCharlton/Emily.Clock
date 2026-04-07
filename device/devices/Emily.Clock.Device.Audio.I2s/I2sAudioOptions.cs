using System;

using Emily.Clock.Device;

namespace Emily.Clock.Device.Audio.I2s;

/// <summary>
/// Configuration for an I2S audio device.
/// </summary>
public class I2sAudioOptions
{
    /// <summary>
    /// The I2S bus ID. Typically 1 or 2 on ESP32 platforms.
    /// </summary>
    public int BusId { get; set; } = 1;

    /// <summary>
    /// Optional delegate invoked during <see cref="I2sAudioProvider.Initialize"/> to configure
    /// platform-specific pin functions before the I2S device is used.
    /// </summary>
    public DevicePreInitializeDelegate? PreInitialize { get; set; }
}
