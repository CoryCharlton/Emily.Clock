using nanoFramework.Presentation;

// ReSharper disable once CheckNamespace
namespace System.Drawing;

public static class ContentAlignmentExtensions
{
    public static void GetAlignments(this ContentAlignment alignment, out HorizontalAlignment horizontal, out VerticalAlignment vertical)
    {
        horizontal = alignment.GetHorizontalAlignment();
        vertical = alignment.GetVerticalAlignment();
    }

    public static HorizontalAlignment GetHorizontalAlignment(this ContentAlignment alignment)
    {
        return alignment switch
        {
            ContentAlignment.TopLeft => HorizontalAlignment.Left,
            ContentAlignment.TopCenter => HorizontalAlignment.Center,
            ContentAlignment.TopRight => HorizontalAlignment.Right,
            ContentAlignment.MiddleLeft => HorizontalAlignment.Left,
            ContentAlignment.MiddleCenter => HorizontalAlignment.Center,
            ContentAlignment.MiddleRight => HorizontalAlignment.Right,
            ContentAlignment.BottomLeft => HorizontalAlignment.Left,
            ContentAlignment.BottomCenter => HorizontalAlignment.Center,
            ContentAlignment.BottomRight => HorizontalAlignment.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment))
        };
    }

    public static VerticalAlignment GetVerticalAlignment(this ContentAlignment alignment)
    {
        return alignment switch
        {
            ContentAlignment.TopLeft => VerticalAlignment.Top,
            ContentAlignment.TopCenter => VerticalAlignment.Top,
            ContentAlignment.TopRight => VerticalAlignment.Top,
            ContentAlignment.MiddleLeft => VerticalAlignment.Center,
            ContentAlignment.MiddleCenter => VerticalAlignment.Center,
            ContentAlignment.MiddleRight => VerticalAlignment.Center,
            ContentAlignment.BottomLeft => VerticalAlignment.Bottom,
            ContentAlignment.BottomCenter => VerticalAlignment.Bottom,
            ContentAlignment.BottomRight => VerticalAlignment.Bottom,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment))
        };
    }
}