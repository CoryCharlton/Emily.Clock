using System.Drawing;
using Emily.Clock.UI;
using Emily.Clock.UI.Layout;

// ReSharper disable once CheckNamespace
namespace nanoFramework.UI;

public static class FontExtensions
{
    public static Rectangle ComputeRectangle(this Font font, string text, Bitmap container, ContentAlignment alignment)
    {
        return font.ComputeRectangle(text, container, alignment, Padding.Empty);
    }

    public static Rectangle ComputeRectangle(this Font font, string text, Bitmap container, ContentAlignment alignment, Padding padding)
    {
        return font.ComputeRectangle(text, container.GetRectangle(), alignment, padding);
    }

    public static Rectangle ComputeRectangle(this Font font, string text, Rectangle container, ContentAlignment alignment)
    {
        return font.ComputeRectangle(text, container, alignment, Padding.Empty);
    }

    public static Rectangle ComputeRectangle(this Font font, string text, Rectangle container, ContentAlignment alignment, Padding padding)
    {
        return LayoutUtils.Align(font.ComputeSize(text), container, alignment, padding);
    }

    public static Size ComputeSize(this Font font, string text)
    {
        font.ComputeExtent(text, out var width, out var height);
        return new Size(width, height);
    }
}