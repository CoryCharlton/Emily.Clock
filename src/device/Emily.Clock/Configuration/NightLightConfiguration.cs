using Emily.Clock.UI.Lights;

namespace Emily.Clock.Configuration
{
    public class NightLightConfiguration
    {
        public static NightLightConfiguration Default { get; } = new() {Brightness = 0.5, Color = NightLightColor.Violet, SunPixel = 1, MoonPixel = 0};
        public const string SectionName = "NightLight";

        public double Brightness { get; set; }

        public NightLightColor Color { get; set; }

        public int MoonPixel { get; set; }

        public int SunPixel { get; set; }
    }
}
