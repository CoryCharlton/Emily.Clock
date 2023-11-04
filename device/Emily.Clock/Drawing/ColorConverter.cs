// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    // Based on: https://gist.github.com/UweKeim/fb7f829b852c209557bc49c51ba14c8b

    /// <summary>
    /// Provides color conversion functionality.
    /// </summary>
    public static class ColorConverter
    {
        private static double Hue2Rgb(double v1, double v2, double vH)
        {
            if (vH < 0.0)
            {
                vH += 1.0;
            }
            if (vH > 1.0)
            {
                vH -= 1.0;
            }
            if (6.0 * vH < 1.0)
            {
                return v1 + (v2 - v1) * 6.0 * vH;
            }
            if (2.0 * vH < 1.0)
            {
                return v2;
            }
            if (3.0 * vH < 2.0)
            {
                return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
            }

            return v1;
        }

        /// <summary>
        /// Determines the maximum value of all of the numbers provided in the variable argument list.
        /// </summary>
        private static double Max(params double[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var max = values[0];

            foreach (var value in values)
            {
                max = Math.Max(max, value);
            }

            return max;
        }

        /// <summary>
        /// Determines the minimum value of all of the numbers provided in the variable argument list.
        /// </summary>
        private static double Min(params double[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var min = values[0];

            foreach (var value in values)
            {
                min = Math.Min(min, value);
            }

            return min;
        }

        public static Color ScaleBrightness(Color color, double brightness)
        {
            var brightnessAdjusted = Math.Max(0.0, Math.Min(1.0, brightness));

            var hsb = ToHslColor(color);
            var hsbAdjusted = new HsbColor(hsb.PreciseHue, hsb.PreciseSaturation, 100 * brightnessAdjusted, hsb.Alpha);

            return hsbAdjusted.ToColor();
        }

        public static Color ToColor(HsbColor hsb)
        {
            double red = 0, green = 0, blue = 0;

            double hue = hsb.Hue;
            var saturation = (double)hsb.Saturation / 100;
            var brightness = (double)hsb.Brightness / 100;

            if (Math.Abs(saturation - 0) < double.Epsilon)
            {
                red = brightness;
                green = brightness;
                blue = brightness;
            }
            else
            {
                // the color wheel has six sectors.

                var sectorPosition = hue / 60;
                var sectorNumber = (int)Math.Floor(sectorPosition);
                var fractionalSector = sectorPosition - sectorNumber;

                var p = brightness * (1 - saturation);
                var q = brightness * (1 - saturation * fractionalSector);
                var t = brightness * (1 - saturation * (1 - fractionalSector));

                // Assign the fractional colors to r, g, and b
                // based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        red = brightness;
                        green = t;
                        blue = p;
                        break;

                    case 1:
                        red = q;
                        green = brightness;
                        blue = p;
                        break;

                    case 2:
                        red = p;
                        green = brightness;
                        blue = t;
                        break;

                    case 3:
                        red = p;
                        green = q;
                        blue = brightness;
                        break;

                    case 4:
                        red = t;
                        green = p;
                        blue = brightness;
                        break;

                    case 5:
                        red = brightness;
                        green = p;
                        blue = q;
                        break;
                }
            }

            var nRed = (int)(red * 255);// Convert.ToInt32(red * 255);
            var nGreen = (int)(green * 255);//Convert.ToInt32(green * 255);
            var nBlue = (int)(blue * 255);//Convert.ToInt32(blue * 255);

            return Color.FromArgb(hsb.Alpha, nRed, nGreen, nBlue);
        }

        public static Color ToColor(HslColor hsl)
        {
            double red, green, blue;

            var hue = hsl.PreciseHue / 360.0;
            var saturation = hsl.PreciseSaturation / 100.0;
            var light = hsl.PreciseLight / 100.0;

            if (Math.Abs(saturation - 0.0) < double.Epsilon)
            {
                red = light;
                green = light;
                blue = light;
            }
            else
            {
                double var2;

                if (light < 0.5)
                {
                    var2 = light * (1.0 + saturation);
                }
                else
                {
                    var2 = light + saturation - saturation * light;
                }

                var var1 = 2.0 * light - var2;

                red = Hue2Rgb(var1, var2, hue + 1.0 / 3.0);
                green = Hue2Rgb(var1, var2, hue);
                blue = Hue2Rgb(var1, var2, hue - 1.0 / 3.0);
            }

            var nRed = (int)(red * 255.0);//Convert.ToInt32(red * 255.0);
            var nGreen = (int)(green * 255.0);//Convert.ToInt32(green * 255.0);
            var nBlue = (int)(blue * 255.0);//Convert.ToInt32(blue * 255.0);

            return Color.FromArgb(hsl.Alpha, nRed, nGreen, nBlue);
        }

        public static HsbColor ToHsbColor(Color color)
        {
            // _NOTE #1: Even though we're dealing with a very small range of
            // numbers, the accuracy of all calculations is fairly important.
            // For this reason, I've opted to use double data types instead
            // of float, which gives us a little bit extra precision (recall
            // that precision is the number of significant digits with which
            // the result is expressed).

            var r = color.R / 255d;
            var g = color.G / 255d;
            var b = color.B / 255d;

            var min = Min(r, g, b);
            var max = Max(r, g, b);
            var delta = max - min;

            double hue = 0;
            double saturation;
            var brightness = max * 100;

            if (Math.Abs(max - 0) < double.Epsilon || Math.Abs(delta - 0) < double.Epsilon)
            {
                hue = 0;
                saturation = 0;
            }
            else
            {
                // _NOTE #2: FXCop insists that we avoid testing for floating 
                // point equality (CA1902). Instead, we'll perform a series of
                // tests with the help of Double.Epsilon that will provide 
                // a more accurate equality evaluation.

                if (Math.Abs(min - 0) < double.Epsilon)
                {
                    saturation = 100;
                }
                else
                {
                    saturation = delta / max * 100;
                }

                if (Math.Abs(r - max) < double.Epsilon)
                {
                    hue = (g - b) / delta;
                }
                else if (Math.Abs(g - max) < double.Epsilon)
                {
                    hue = 2 + (b - r) / delta;
                }
                else if (Math.Abs(b - max) < double.Epsilon)
                {
                    hue = 4 + (r - g) / delta;
                }
            }

            hue *= 60;
            if (hue < 0)
            {
                hue += 360;
            }

            return new HsbColor(hue, saturation, brightness, color.A);
        }

        public static HslColor ToHslColor(Color color)
        {
            var r = color.R / 255.0; //Where RGB values = 0 ÷ 255
            var g = color.G / 255.0;
            var b = color.B / 255.0;

            var min = Min(r, g, b); //Min. value of RGB
            var max = Max(r, g, b); //Max. value of RGB
            var delta = max - min; //Delta RGB value

            double hue;
            double saturation;
            var light = (max + min) / 2;

            if (Math.Abs(delta - 0) < double.Epsilon) //This is a gray, no chroma...
            {
                hue = 0; //HSL results = 0 ÷ 1
                saturation = 0;
                // UK:
                //				s = 1.0;
            }
            else //Chromatic data...
            {
                if (light < 0.5)
                {
                    saturation = delta / (max + min);
                }
                else
                {
                    saturation = delta / (2.0 - max - min);
                }

                var deltaR = ((max - r) / 6.0 + delta / 2.0) / delta;
                var deltaG = ((max - g) / 6.0 + delta / 2.0) / delta;
                var deltaB = ((max - b) / 6.0 + delta / 2.0) / delta;

                if (Math.Abs(r - max) < double.Epsilon)
                {
                    hue = deltaB - deltaG;
                }
                else if (Math.Abs(g - max) < double.Epsilon)
                {
                    hue = 1.0 / 3.0 + deltaR - deltaB;
                }
                else if (Math.Abs(b - max) < double.Epsilon)
                {
                    hue = 2.0 / 3.0 + deltaG - deltaR;
                }
                else
                {
                    hue = 0.0;
                }

                if (hue < 0.0)
                {
                    hue += 1.0;
                }
                if (hue > 1.0)
                {
                    hue -= 1.0;
                }
            }

            return new HslColor(hue * 360.0, saturation * 100.0, light * 100.0, color.A); 
        }
    }
}
