using Emily.Clock.UI;
using nanoFramework.Presentation;

// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    // TODO: Replace this with LayoutUtils calls
    public static class SizeExtensions
    {
        public static Rectangle AlignContent(this Size container, Size content, ContentAlignment alignment)
        {
            return container.AlignContent(content, alignment, Padding.Empty);
        }

        public static Rectangle AlignContent(this Size container, Size content, ContentAlignment alignment, Padding padding)
        {
            alignment.GetAlignments(out var horizontal, out var vertical);
            return container.AlignContent(content, horizontal, vertical, padding);
        }

        private static Rectangle AlignContent(this Size container, Size content, HorizontalAlignment horizontal, VerticalAlignment vertical, Padding padding)
        {
            var x = horizontal.AlignContent(container, content, padding);
            var y = vertical.AlignContent(container, content, padding);

            return new Rectangle(new Point(x, y), content);
        }
    }

}
