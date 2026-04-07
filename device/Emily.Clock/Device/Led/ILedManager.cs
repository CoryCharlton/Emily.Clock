using System.Drawing;

namespace Emily.Clock.Device.Led;

// TODO: In order to support multi-color functionality this will need to expose a Count property and there will need to be a SetNightlightLeds that takes an array of colors
/// <summary>
/// Represents the LEDs used for the nightlight and day/night functions.
/// </summary>
public interface ILedManager
{
    public bool IsInitialized { get; }

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
    /// Sets the <see cref="Color"/> and brightness of the moon LED.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to adjust.</param>
    /// <param name="brightness">The brightness value between 0.0 and 1.0.</param>
    public void SetMoonLed(Color color, float brightness);

    /// <summary>
    /// Sets the <see cref="Color"/> and brightness of the nightlight LEDs.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to adjust.</param>
    /// <param name="brightness">The brightness value between 0.0 and 1.0.</param>
    public void SetNightlightLeds(Color color, float brightness);

    /// <summary>
    /// Sets the <see cref="Color"/> and brightness of the sun LED.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to adjust.</param>
    /// <param name="brightness">The brightness value between 0.0 and 1.0.</param>
    public void SetSunLed(Color color, float brightness);

    /// <summary>
    /// Send the data to the LED driver.
    /// </summary>
    public void Update();
}