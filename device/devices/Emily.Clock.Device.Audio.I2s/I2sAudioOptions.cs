using System;

namespace Emily.Clock.Device.Audio.I2s;

/// <summary>
/// Pin and bus configuration for an I2S audio device.
/// </summary>
public class I2sAudioOptions
{
    /// <summary>
    /// The I2S bus ID. Typically 1 or 2 on ESP32 platforms.
    /// </summary>
    public int BusId { get; set; } = 1;

    /// <summary>
    /// The pin ID for the BCK (bit clock) line.
    /// </summary>
    public int BckPin { get; set; }

    /// <summary>
    /// The pin ID for the data output line.
    /// </summary>
    public int DataPin { get; set; }

    /// <summary>
    /// The pin ID for the WS (word select / LRCLK) line.
    /// </summary>
    public int WsPin { get; set; }
}
