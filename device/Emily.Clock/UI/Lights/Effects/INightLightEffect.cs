using Emily.Clock.Device.Led;

namespace Emily.Clock.UI.Lights.Effects;

/// <summary>
/// Represents a nightlight lighting effect applied to the nightlight LEDs.
/// </summary>
public interface INightLightEffect
{
    /// <summary>
    /// Milliseconds between animation steps. Use <see cref="System.Threading.Timeout.Infinite"/>
    /// for static effects that do not animate.
    /// </summary>
    int Delay { get; }

    /// <summary>
    /// Apply the initial frame.
    /// </summary>
    void Start(ILedManager ledManager, LedConfiguration ledConfiguration);

    /// <summary>
    /// Advance one animation step. Returns true if LEDs were updated and
    /// <see cref="ILedManager.Update"/> should be called; false if no change.
    /// </summary>
    bool Step(ILedManager ledManager, LedConfiguration ledConfiguration);

    /// <summary>
    /// Clear nightlight LEDs.
    /// </summary>
    void Stop(ILedManager ledManager, LedConfiguration ledConfiguration);
}
