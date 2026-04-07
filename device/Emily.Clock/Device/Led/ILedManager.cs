using System.Drawing;

namespace Emily.Clock.Device.Led;

/// <summary>
/// Represents a strip of addressable LEDs.
/// </summary>
public interface ILedManager
{
    /// <summary>
    /// The total number of LEDs in the strip.
    /// </summary>
    int Count { get; }

    bool IsInitialized { get; }

    /// <summary>
    /// Reset all LEDs to <see cref="Color.Black"/>.
    /// </summary>
    void Clear(bool update = true);

    /// <summary>
    /// Initialize the LED driver.
    /// </summary>
    /// <returns>true if successful; otherwise false</returns>
    bool Initialize();

    /// <summary>
    /// Sets the <see cref="Color"/> of the LED at the given index.
    /// </summary>
    /// <param name="index">The zero-based LED index.</param>
    /// <param name="color">The <see cref="Color"/> to set.</param>
    void SetLed(int index, Color color);

    /// <summary>
    /// Sets the <see cref="Color"/> and brightness of the LED at the given index.
    /// </summary>
    /// <param name="index">The zero-based LED index.</param>
    /// <param name="color">The <see cref="Color"/> to set.</param>
    /// <param name="brightness">The brightness value between 0.0 and 1.0.</param>
    void SetLed(int index, Color color, float brightness);

    /// <summary>
    /// Sets the <see cref="Color"/> of a contiguous range of LEDs.
    /// </summary>
    /// <param name="startIndex">The zero-based index of the first LED (inclusive).</param>
    /// <param name="endIndex">The zero-based index of the last LED (inclusive).</param>
    /// <param name="color">The <see cref="Color"/> to set.</param>
    void SetLeds(int startIndex, int endIndex, Color color);

    /// <summary>
    /// Sets the <see cref="Color"/> and brightness of a contiguous range of LEDs.
    /// Brightness scaling is performed once for the group rather than per-LED.
    /// </summary>
    /// <param name="startIndex">The zero-based index of the first LED (inclusive).</param>
    /// <param name="endIndex">The zero-based index of the last LED (inclusive).</param>
    /// <param name="color">The <see cref="Color"/> to set.</param>
    /// <param name="brightness">The brightness value between 0.0 and 1.0.</param>
    void SetLeds(int startIndex, int endIndex, Color color, float brightness);

    /// <summary>
    /// Send the data to the LED driver.
    /// </summary>
    void Update();
}
