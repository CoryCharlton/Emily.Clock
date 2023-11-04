// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    // Based on: https://gist.github.com/UweKeim/fb7f829b852c209557bc49c51ba14c8b

    /// <summary>
    /// Represents a HSB color space.
    /// http://en.wikipedia.org/wiki/HSV_color_space
    /// </summary>
    public sealed class HsbColor
    {
        public HsbColor(double hue, double saturation, double brightness, int alpha)
        {
            PreciseHue = hue;
            PreciseSaturation = saturation;
            PreciseBrightness = brightness;
            Alpha = alpha;
        }

        /// <summary>
        /// Gets or sets the hue. Values from 0 to 360.
        /// </summary>
        public double PreciseHue { get; }

        /// <summary>
        /// Gets or sets the saturation. Values from 0 to 100.
        /// </summary>
        public double PreciseSaturation { get; }

        /// <summary>
        /// Gets or sets the brightness. Values from 0 to 100.
        /// </summary>
        public double PreciseBrightness { get; }

        public int Hue => (int)PreciseHue;// Convert.ToInt32(PreciseHue);

        public int Saturation => (int)PreciseSaturation;// Convert.ToInt32(PreciseSaturation);

        public int Brightness => (int)PreciseBrightness;// Convert.ToInt32(PreciseBrightness);

        /// <summary>
        /// Gets or sets the alpha. Values from 0 to 255.
        /// </summary>
        public int Alpha { get; }

        public override bool Equals(object obj)
        {
            if (obj is not HsbColor color)
            {
                return false;
            }

            return Math.Abs(PreciseHue - color.PreciseHue) < 0.001 &&
                   Math.Abs(PreciseSaturation - color.PreciseSaturation) < 0.001 &&
                   Math.Abs(PreciseBrightness - color.PreciseBrightness) < 0.001;
        }

        public static HsbColor FromColor(Color color)
        {
            return ColorConverter.ToHsbColor(color);
        }

        public override int GetHashCode()
        {
            return $"H:{PreciseHue}-S:{PreciseSaturation}-B:{PreciseBrightness}-A:{Alpha}".GetHashCode();
        }

        public Color ToColor()
        {
            return ColorConverter.ToColor(this);
        }

        public override string ToString()
        {
            return $"Hue: {Hue}; Saturation: {Saturation}; Brightness: {Brightness}; Alpha: {Alpha}";
        }
    }
}
