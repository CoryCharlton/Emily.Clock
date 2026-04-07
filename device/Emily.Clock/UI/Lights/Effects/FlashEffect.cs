using System.Drawing;
using Emily.Clock.Device.Led;

namespace Emily.Clock.UI.Lights.Effects;

/// <summary>
/// Alternates all nightlight LEDs between two colors at a fixed interval.
/// </summary>
public class FlashEffect : INightLightEffect
{
    private readonly int _delay;
    private readonly Color _offColor;
    private readonly Color _onColor;
    private bool _state;

    public FlashEffect(Color onColor, Color offColor, int delay)
    {
        _delay = delay;
        _offColor = offColor;
        _onColor = onColor;
    }

    /// <inheritdoc/>
    public int Delay => _delay;

    /// <inheritdoc/>
    public void Start(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        _state = true;
        Render(ledManager, ledConfiguration);
    }

    /// <inheritdoc/>
    public bool Step(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        _state = !_state;
        Render(ledManager, ledConfiguration);
        return true;
    }

    /// <inheritdoc/>
    public void Stop(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        ledManager.SetLeds(ledConfiguration.NightlightStartIndex, ledConfiguration.NightlightEndIndex, Color.Black);
    }

    private void Render(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        var color = _state ? _onColor : _offColor;
        ledManager.SetLeds(ledConfiguration.NightlightStartIndex, ledConfiguration.NightlightEndIndex, color);
    }
}
