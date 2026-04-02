using System;
using System.Drawing;

namespace Emily.Clock.UI.Lights
{
    public static class NightLightColorConverter
    {
        // ReSharper disable InconsistentNaming
        //private static readonly Color COLOR_RED = Color.FromArgb(255, 255, 0, 0);
        //private static readonly Color COLOR_ORANGE = Color.FromArgb(255, 255, 119, 89);
        //private static readonly Color COLOR_YELLOW = Color.FromArgb(255, 255, 175, 10);

        private static readonly Color COLOR_RED = Color.DarkRed;
        private static readonly Color COLOR_ORANGE = Color.FromArgb(255, 255, 150, 0);
        private static readonly Color COLOR_YELLOW = Color.Yellow;
        private static readonly Color COLOR_GREEN = Color.FromArgb(255, 0, 255, 0);
        private static readonly Color COLOR_BLUE = Color.Blue;
        private static readonly Color COLOR_INDIGO = Color.FromArgb(255, 255, 0, 255);
        private static readonly Color COLOR_VIOLET = Color.DeepPink;
        // ReSharper restore InconsistentNaming

        public static Color ToColor(NightLightColor color)
        {
            return color switch
            {
                NightLightColor.Red => COLOR_RED,
                NightLightColor.Orange => COLOR_ORANGE,
                NightLightColor.Yellow => COLOR_YELLOW,
                NightLightColor.Green => COLOR_GREEN,
                NightLightColor.Blue => COLOR_BLUE,
                NightLightColor.Indigo => COLOR_INDIGO,
                NightLightColor.Violet => COLOR_VIOLET,
                _ => throw new ArgumentOutOfRangeException(nameof(color))
            };
        }

        public static string ToString(NightLightColor color)
        {
            return color switch
            {
                NightLightColor.Red => nameof(COLOR_RED),
                NightLightColor.Orange => nameof(COLOR_ORANGE),
                NightLightColor.Yellow => nameof(COLOR_YELLOW),
                NightLightColor.Green => nameof(COLOR_GREEN),
                NightLightColor.Blue => nameof(COLOR_BLUE),
                NightLightColor.Indigo => nameof(COLOR_INDIGO),
                NightLightColor.Violet => nameof(COLOR_VIOLET),
                _ => throw new ArgumentOutOfRangeException(nameof(color))
            };
        }
    }
}
