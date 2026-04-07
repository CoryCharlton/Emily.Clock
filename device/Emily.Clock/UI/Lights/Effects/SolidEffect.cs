using System.Drawing;
using System.Threading;
using Emily.Clock.Device.Led;

namespace Emily.Clock.UI.Lights.Effects;

/// <summary>
/// Static solid-color effect. Sets all nightlight LEDs to a single color and brightness.
/// </summary>
public class SolidEffect : INightLightEffect
{
    private readonly float _brightness;
    private readonly Color _color;

    public SolidEffect(Color color, float brightness)
    {
        _brightness = brightness;
        _color = color;
    }

    /// <inheritdoc/>
    public int Delay => Timeout.Infinite;

    /// <inheritdoc/>
    public void Start(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        ledManager.SetLeds(ledConfiguration.NightlightStartIndex, ledConfiguration.NightlightEndIndex, _color, _brightness);
    }

    /// <inheritdoc/>
    public bool Step(ILedManager ledManager, LedConfiguration ledConfiguration) => false;

    /// <inheritdoc/>
    public void Stop(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        ledManager.SetLeds(ledConfiguration.NightlightStartIndex, ledConfiguration.NightlightEndIndex, Color.Black);
    }
}
