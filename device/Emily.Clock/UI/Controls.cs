using System;
using System.Drawing;
using Emily.Clock.Device.Gpio;
using Emily.Clock.UI.Layout;
using nanoFramework.Presentation;
using nanoFramework.UI;

namespace Emily.Clock.UI
{
    internal static class Controls
    {
        private const bool DebugSetting = false;

        private const short IconSize = 22;
        private const short LogoSize = 48;

        public static void DrawButton(Bitmap screen, Button button, bool debug = DebugSetting, bool flush = false)
        {
            using var source = new Bitmap(IconSize, IconSize);
            DrawButton(screen, button, source, debug, flush);
        }

        public static void DrawButton(Bitmap screen, Button button, Resources.BitmapResources id, bool debug = DebugSetting, bool flush = false)
        {
            using var source = Resources.GetBitmap(id);
            DrawButton(screen, button, source, debug, flush);
        }

        private static void DrawButton(Bitmap screen, Button button, Bitmap source, bool debug = DebugSetting, bool flush = false)
        {
            PerformDrawingAndFlush(screen, flush, () =>
            {
                var iconRectangle = GetButtonIconRectangle(screen, button);
                screen.DrawImage(iconRectangle.Location, source);

                if (debug)
                {
                    screen.DrawLine(Theme.Highlight, 1, 0, GetButtonY(screen, button), screen.Width, GetButtonY(screen, button));
                }
            });
        }

        public static void DrawContent(Bitmap screen, string line1 = null, string line2 = null, bool flush = false)
        {
            PerformDrawingAndFlush(screen, flush, () =>
            {
                var color = Theme.SecondaryText;
                var fontLine1 = Theme.MediumFont;
                var fontLine2 = Theme.MediumFont;
                var padding = Theme.ContentPadding;

                using var source = new Bitmap(screen.Width, (fontLine1.Height + padding.Vertical) + (fontLine2.Height + padding.Vertical));

                if (!string.IsNullOrEmpty(line1))
                {
                    var alignment = string.IsNullOrEmpty(line2)
                        ? ContentAlignment.MiddleCenter
                        : ContentAlignment.TopCenter;

                    source.DrawText(line1, fontLine1, color, alignment, padding);
                }

                if (!string.IsNullOrEmpty(line2))
                {
                    source.DrawText(line2, fontLine2, color, ContentAlignment.BottomCenter, padding);
                }

                screen.DrawImage(0, (int)(screen.Height * 0.70 - (source.Height / 2.0) + (LogoSize / 2.0)), source);
            });
        }

        public static void DrawLogo(Bitmap screen, Resources.BitmapResources id, bool flush = false)
        {
            PerformDrawingAndFlush(screen, flush, () =>
            {
                using var source = Resources.GetBitmap(id);
                screen.DrawImage(source, ContentAlignment.MiddleCenter);
            });
        }

        public static Rectangle DrawProgressBar(Bitmap screen, double progress, bool flush = false)
        {
            return DrawProgressBar(screen, progress, new Size(200, 18), flush);
        }

        public static Rectangle DrawProgressBar(Bitmap screen, double progress, Size size, bool flush = false)
        {
            progress = Math.Clamp(progress, 0.0, 1.0);

            var cornerRadius = (int)(size.Height / 2.0 - 1.0);
            
            var screenRectangle = new Rectangle(0, (int)(screen.Height * 0.75), screen.Width, screen.Height / 4);
            var progressRectangle = LayoutUtils.Align(size, screenRectangle, ContentAlignment.TopCenter);

            screen.FillRoundRectangle(progressRectangle, cornerRadius, Theme.Secondary);

            if (progress > 0)
            {
                var filledProgressRectangle = progressRectangle;
                filledProgressRectangle.Width = (int)(filledProgressRectangle.Width * progress);
                screen.FillRoundRectangle(filledProgressRectangle, cornerRadius, Theme.Highlight);
            }

            if (flush)
            {
                screen.Flush();
            }

            return progressRectangle;
        }

        public static void DrawTitle(Bitmap screen, string title, bool flush = false)
        {
            PerformDrawingAndFlush(screen, flush, () =>
            {
                using var source = BitmapFactory.Create(screen.Width, title, Theme.LargeFont, Theme.PrimaryText, HorizontalAlignment.Center, Theme.ControlPadding);
                screen.DrawImage(0, (int)(screen.Height * 0.30 - (source.Height / 2.0) - (LogoSize / 2.0)), source);
            });
        }

        private static int GetButtonY(Bitmap screen, Button button)
        {
            const int adjustment = 10;
            var offset = screen.Height / 4;

            var button1= offset - adjustment;
            var button2 = offset * 2;
            var button3 = offset * 3 + adjustment;

            return button switch
            {
                Button.One => button1,
                Button.Two => button2,
                Button.Three => button3,
                _ => throw new ArgumentOutOfRangeException(nameof(button))
            };
        }

        private static Rectangle GetButtonIconRectangle(Bitmap screen, Button button)
        {
            var x = screen.Width - IconSize - Theme.ControlPadding.Right;
            var y = GetButtonY(screen, button) - (IconSize / 2);

            return new Rectangle(x, y, IconSize, IconSize);
        }

        // TODO: Is there a way I can return a value here?
        public static void PerformDrawingAndFlush(Bitmap screen, bool flush, Action action)
        {
            action();

            if (flush)
            {
                screen.Flush();
            }
        }
    }
}
