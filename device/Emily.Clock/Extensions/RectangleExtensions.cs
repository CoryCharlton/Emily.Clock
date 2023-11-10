using nanoFramework.Presentation;

// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    // TODO: Replace this with LayoutUtils calls
    internal static class RectangleExtensions
    {
        public static Rectangle AlignContent(this Rectangle container, Size content, ContentAlignment alignment)
        {
            alignment.GetAlignments(out var horizontal, out var vertical);
            return container.AlignContent(content, horizontal, vertical);
        }

        public static Rectangle AlignContent(this Rectangle container, Size content, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            var x = horizontal switch
            {
                HorizontalAlignment.Center => (container.Width - content.Width) / 2,
                HorizontalAlignment.Left => 0,
                HorizontalAlignment.Right => container.Width - content.Width,
                HorizontalAlignment.Stretch => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };

            x = x > 0 ? x : 0;

            var y = vertical switch
            {
                VerticalAlignment.Bottom => container.Height - content.Height,
                VerticalAlignment.Center => (container.Height - content.Height) / 2,
                VerticalAlignment.Stretch => throw new ArgumentOutOfRangeException(),
                VerticalAlignment.Top => 0,
                _ => throw new ArgumentOutOfRangeException()
            };

            y = y > 0 ? y : 0;

            return new Rectangle(new Point(x, y), content);
        }
    }
}
