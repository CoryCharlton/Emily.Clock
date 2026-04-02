using Emily.Clock.UI;
using Emily.Clock.UI.Layout;
using nanoFramework.UI;

// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    public static class BitmapExtensions
    {
        public static void Clear(this Bitmap bitmap, Rectangle region)
        {
            if (Rectangle.Empty == region)
            {
                return;
            }
            
            using var image = BitmapFactory.Create(region);
            bitmap.DrawImage(region.Location, image);
        }

        public static Rectangle DrawImage(this Bitmap bitmap, Point point, Bitmap image)
        {
            bitmap.DrawImage(point.X, point.Y, image);

            return new Rectangle(point, image.GetSize());
        }

        public static Rectangle DrawImage(this Bitmap bitmap, Bitmap image, ContentAlignment alignment)
        {
            var rectangle = LayoutUtils.Align(image.GetSize(), bitmap.GetRectangle(), alignment);
            bitmap.DrawImage(rectangle.Location, image);

            return rectangle;
        }

        public static void DrawImage(this Bitmap bitmap, int x, int y, Bitmap image)
        {
            bitmap.DrawImage(x, y, image, 0, 0, image.Width, image.Height, Bitmap.OpacityOpaque);
        }

        public static Rectangle DrawText(this Bitmap bitmap, string text, Font font, Color color, ContentAlignment alignment)
        {
            return bitmap.DrawText(text, font, color, alignment, Padding.Empty);
        }

        public static Rectangle DrawText(this Bitmap bitmap, string text, Font font, Color color, ContentAlignment alignment, Padding padding)
        {
            var rectangle = font.ComputeRectangle(text, bitmap, alignment, padding);

            // If they are the same size assume the bitmap was created specifically for this text and already has padding added
            if (bitmap.GetSize() == rectangle.Size)
            {
                rectangle = font.ComputeRectangle(text, bitmap, alignment);
            }

            bitmap.DrawText(text, font, color, rectangle.X, rectangle.Y);

            return rectangle;
        }

        public static void Fill(this Bitmap bitmap, Color color)
        {
            bitmap.FillRectangle(0, 0, bitmap.Width, bitmap.Height, color, Bitmap.OpacityOpaque);
        }

        public static void FillRectangle(this Bitmap bitmap, Rectangle rectangle, Color color)
        {
            bitmap.FillRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color, Bitmap.OpacityOpaque);
        }

        public static void FillRoundRectangle(this Bitmap bitmap, Rectangle rectangle, int cornerRadius, Color color)
        {
            bitmap.FillRoundRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, cornerRadius, cornerRadius, color, Bitmap.OpacityOpaque);
        }

        public static Size GetSize(this Bitmap bitmap) => new(bitmap.Width, bitmap.Height);

        public static Rectangle GetRectangle(this Bitmap bitmap) => new(new Point(0, 0), bitmap.GetSize());
    }
}
