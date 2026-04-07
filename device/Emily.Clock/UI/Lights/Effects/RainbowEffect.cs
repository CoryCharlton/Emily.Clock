using System.Drawing;
using Emily.Clock.Device.Led;

namespace Emily.Clock.UI.Lights.Effects;

/// <summary>
/// Cycles a rainbow across all nightlight LEDs.
/// </summary>
public class RainbowEffect : INightLightEffect
{
    private int _offset;

    /// <inheritdoc/>
    public int Delay => 20;

    /// <inheritdoc/>
    public void Start(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        _offset = 0;
        Render(ledManager, ledConfiguration);
    }

    /// <inheritdoc/>
    public bool Step(ILedManager ledManager, LedConfiguration ledConfiguration)
    {
        _offset = (_offset + 1) % 255;
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
        var count = ledConfiguration.NightlightEndIndex - ledConfiguration.NightlightStartIndex + 1;

        for (var i = 0; i < count; i++)
        {
            ledManager.SetLed(ledConfiguration.NightlightStartIndex + i, Wheel(((i * 255 / count) + _offset) & 255));
        }
    }

    private static Color Wheel(int position)
    {
        if (position < 85)
        {
            return Color.FromArgb(255, position * 3, 255 - position * 3, 0);
        }

        if (position < 170)
        {
            position -= 85;
            return Color.FromArgb(255, 255 - position * 3, 0, position * 3);
        }

        position -= 170;
        return Color.FromArgb(255, 0, position * 3, 255 - position * 3);
    }
}
