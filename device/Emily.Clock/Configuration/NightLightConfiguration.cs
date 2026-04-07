using Emily.Clock.UI.Lights;

namespace Emily.Clock.Configuration;

public class NightLightConfiguration
{
    public const string Section = "NightLight";

    public float Brightness { get; set; } = 0.5f;

    public NightLightColor Color { get; set; } = NightLightColor.Violet;

    public float PanelBrightness { get; set; } = 0.25f;
}
