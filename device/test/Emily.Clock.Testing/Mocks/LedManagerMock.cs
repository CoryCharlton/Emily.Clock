using System.Collections;
using System.Drawing;
using Emily.Clock.Device.Led;

namespace Emily.Clock.Testing.Mocks;

public class LedManagerMock : ILedManager
{
    private readonly Hashtable _leds = new();

    public int Count { get; set; } = 47;
    public bool IsInitialized { get; private set; }

    public int SetLedCallCount { get; private set; }
    public int SetLedsCallCount { get; private set; }
    public Color LastSetLedColor { get; private set; }
    public Color LastSetLedsColor { get; private set; }

    public void Clear(bool update = true)
    {
        _leds.Clear();
    }

    public Color GetLedColor(int index)
    {
        return _leds.Contains(index) ? (Color)_leds[index] : Color.Black;
    }

    public bool Initialize()
    {
        IsInitialized = true;

        return true;
    }

    public void SetLed(int index, Color color)
    {
        _leds[index] = color;
        LastSetLedColor = color;
        SetLedCallCount++;
    }

    public void SetLed(int index, Color color, float brightness)
    {
        var scaled = Color.FromArgb(
            (int)(color.R * brightness),
            (int)(color.G * brightness),
            (int)(color.B * brightness));

        _leds[index] = scaled;
        LastSetLedColor = scaled;
        SetLedCallCount++;
    }

    public void SetLeds(int startIndex, int endIndex, Color color)
    {
        for (var i = startIndex; i <= endIndex; i++)
        {
            _leds[i] = color;
        }

        LastSetLedsColor = color;
        SetLedsCallCount++;
    }

    public void SetLeds(int startIndex, int endIndex, Color color, float brightness)
    {
        var scaled = Color.FromArgb(
            (int)(color.R * brightness),
            (int)(color.G * brightness),
            (int)(color.B * brightness));

        for (var i = startIndex; i <= endIndex; i++)
        {
            _leds[i] = scaled;
        }

        LastSetLedsColor = scaled;
        SetLedsCallCount++;
    }

    public void Update()
    {
    }
}
