using System.Drawing;
using Emily.Clock.Device.Led;

namespace Emily.Clock.UI.Lights.Effects;

/// <summary>
/// Fades all nightlight LEDs in and out at a fixed interval.
/// </summary>
public class BreatheEffect : INightLightEffect
{
    private const float MinBrightness = 0.10f;
    private const int Steps = 20;
    private const int TotalSteps = Steps * 2;

    private readonly Color _color;
    private readonly float _maxBrightness;
    private int _stepIndex;

    public BreatheEffect(Color color, float maxBrightness)
    {
        _color = color;
        _maxBrightness = maxBrightness;
    }

    /// <inheritdoc/>
    public int Delay => 25;

    /// <inheritdoc/>
    public void Start(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        _stepIndex = 0;
        Render(ledManager, ledConfiguration);
    }

    /// <inheritdoc/>
    public bool Step(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        _stepIndex = (_stepIndex + 1) % TotalSteps;
        Render(ledManager, ledConfiguration);
        return true;
    }

    /// <inheritdoc/>
    public void Stop(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        ledManager.SetLeds(ledConfiguration.NightlightStartIndex, ledConfiguration.NightlightEndIndex, Color.Black);
    }

    // TODO: If this is too slow, we could pre-compute the brightness values for each step and just look them up here instead of calculating them on the fly.
    private void Render(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        var step = _stepIndex < Steps ? _stepIndex : TotalSteps - _stepIndex;
        var brightness = MinBrightness + (step / (float)Steps) * (_maxBrightness - MinBrightness);
        ledManager.SetLeds(ledConfiguration.NightlightStartIndex, ledConfiguration.NightlightEndIndex, _color, brightness);
    }
}
