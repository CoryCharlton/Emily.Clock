// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    // Based on: https://gist.github.com/UweKeim/fb7f829b852c209557bc49c51ba14c8b

    /// <summary>
    /// Represents a HSL color space.
    /// http://en.wikipedia.org/wiki/HSV_color_space
    /// </summary>
    public sealed class HslColor
    {
        public HslColor(double hue, double saturation, double light, int alpha)
        {
            PreciseHue = hue;
            PreciseSaturation = saturation;
            PreciseLight = light;
            Alpha = alpha;
        }

        public HslColor(int hue, int saturation, int light, int alpha)
        {
            PreciseHue = hue;
            PreciseSaturation = saturation;
            PreciseLight = light;
            Alpha = alpha;
        }

        /// <summary>
        /// Gets the hue. Values from 0 to 360.
        /// </summary>
        public int Hue => (int)PreciseHue;// Convert.ToInt32(PreciseHue);

        /// <summary>
        /// Gets the precise hue. Values from 0 to 360.
        /// </summary>
        public double PreciseHue { get; }

        /// <summary>
        /// Gets the saturation. Values from 0 to 100.
        /// </summary>
        public int Saturation => (int)PreciseSaturation;// Convert.ToInt32(PreciseSaturation);

        /// <summary>
        /// Gets the precise saturation. Values from 0 to 100.
        /// </summary>
        public double PreciseSaturation { get; }

        /// <summary>
        /// Gets the light. Values from 0 to 100.
        /// </summary>
        public int Light => (int)PreciseLight;// Convert.ToInt32(PreciseLight);

        /// <summary>
        /// Gets the precise light. Values from 0 to 100.
        /// </summary>
        public double PreciseLight { get; }

        /// <summary>
        /// Gets the alpha. Values from 0 to 255
        /// </summary>
        public int Alpha { get; }

        public override bool Equals(object obj)
        {
            if (obj is not HslColor color)
            {
                return false;
            }

            // TODO: Why are the equal methods different?
            return Math.Abs(Hue - color.PreciseHue) < double.Epsilon &&
                   Math.Abs(Saturation - color.PreciseSaturation) < double.Epsilon &&
                   Math.Abs(Light - color.PreciseLight) < double.Epsilon;
        }

        public static HslColor FromColor(Color color)
        {
            return ColorConverter.ToHslColor(color);
        }

        public override int GetHashCode()
        {
            return $@"H:{PreciseHue}-S:{PreciseSaturation}-L:{PreciseLight}".GetHashCode();
        }

        public Color ToColor()
        {
            return ColorConverter.ToColor(this);
        }

        public override string ToString()
        {
            return Alpha < 255
                ? $@"hsla({Hue}, {Saturation}%, {Light}%, {Alpha / 255f})"
                : $@"hsl({Hue}, {Saturation}%, {Light}%)";
        }
    }
}
