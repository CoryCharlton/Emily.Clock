using nanoFramework.UI;
using System.Drawing;

namespace Emily.Clock.UI
{
    public static class BitmapFactory
    {
        public static Bitmap Create(Rectangle rectangle) => Create(rectangle.Size);

        public static Bitmap Create(Size size) => new(size.Width, size.Height);

        public static Bitmap Create(string text, Font font, Color color, ContentAlignment alignment)
        {
            return Create(text, font, color, alignment, Padding.Empty);
        }

        public static Bitmap Create(string text, Font font, Color color, ContentAlignment alignment, Padding padding)
        {
            var bitmap = Create(font.ComputeSize(text) + padding.Size);
            bitmap.DrawText(text, font, color, alignment, padding);
            return bitmap;
        }

        public static Bitmap Create(int width, string text, Font font, Color color, ContentAlignment alignment, Padding padding)
        {
            var bitmap = new Bitmap(width, font.Height + padding.Vertical);
            bitmap.DrawText(text, font, color, alignment, padding);

            return bitmap;
        }

    }
}
