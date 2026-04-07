using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Lights.Effects;

namespace Emily.Clock.Configuration;

public class NightLightConfiguration
{
    public const string Section = "NightLight";

    public float Brightness { get; set; } = 0.5f;

    public NightLightColor Color { get; set; } = NightLightColor.Violet;

    public NightLightEffectType Effect { get; set; } = NightLightEffectType.Solid;

    public float PanelBrightness { get; set; } = 0.5f;
}
