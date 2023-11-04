using Emily.Clock.UI.Lights;

namespace Emily.Clock.Configuration
{
    public class NightLightConfiguration
    {
        public const string Section = "NightLight";

        public double Brightness { get; set; } = 0.5;

        public NightLightColor Color { get; set; } = NightLightColor.Violet;
    }
}
