using System.Drawing;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace Emily.Clock.UI.Layout;

// TODO: Switch to this
// TODO: Should Padding impact the size of the output? Without adjusting the size Padding acts more like a margin.
internal class LayoutUtils
{
    public const ContentAlignment AnyTop = ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight;
    public const ContentAlignment AnyBottom = ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight;
    public const ContentAlignment AnyLeft = ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft;
    public const ContentAlignment AnyRight = ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight;
    public const ContentAlignment AnyCenter = ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter;
    public const ContentAlignment AnyMiddle = ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight;

    public static Rectangle Align(Size alignThis, Rectangle withinThis, ContentAlignment align)
    {
        return Align(alignThis, withinThis, align, Padding.Empty);
    }

    public static Rectangle Align(Size alignThis, Rectangle withinThis, ContentAlignment align, Padding padding)
    {
        return VAlign(alignThis, HAlign(alignThis, withinThis, align, padding), align, padding);
    }

    private static Rectangle HAlign(Size alignThis, Rectangle withinThis, ContentAlignment align, Padding padding)
    {
        var aligned = new Rectangle(withinThis.Location, withinThis.Size);

        if ((align & AnyRight) != 0)
        {
            aligned.X += withinThis.Width - alignThis.Width - padding.Right;
        }
        else if ((align & AnyCenter) != 0)
        {
            aligned.X += (withinThis.Width - alignThis.Width) / 2;
        }
        else
        {
            aligned.X += padding.Left;
        }

        aligned.Width = alignThis.Width;

        return aligned;
    }

    private static Rectangle VAlign(Size alignThis, Rectangle withinThis, ContentAlignment align, Padding padding)
    {
        var aligned = new Rectangle(withinThis.Location, withinThis.Size);

        if ((align & AnyBottom) != 0)
        {
            aligned.Y += withinThis.Height - alignThis.Height - padding.Bottom;
        }
        else if ((align & AnyMiddle) != 0)
        {
            aligned.Y += (withinThis.Height - alignThis.Height) / 2;
        }
        else
        {
            aligned.Y += padding.Top;
        }

        aligned.Height = alignThis.Height;

        return aligned;
    }
}