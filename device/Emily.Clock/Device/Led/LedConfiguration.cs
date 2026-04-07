namespace Emily.Clock.Device.Led;

/// <summary>
/// Describes the layout of application-specific LEDs within the strip.
/// </summary>
public class LedConfiguration
{
    public int MoonLedIndex { get; set; }
    public int NightlightEndIndex { get; set; }
    public int NightlightStartIndex { get; set; }
    public int SunLedIndex { get; set; }
}
