using System.Drawing;
using nanoFramework.UI;
using Point = System.Drawing.Point;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace Emily.Clock.UI.Layout
{
    // TODO: Switch to this
    // TODO: Support padding
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
            return VAlign(alignThis, HAlign(alignThis, withinThis, align), align);
        }

        private static Rectangle HAlign(Size alignThis, Rectangle withinThis, ContentAlignment align)
        {
            //var aligned = new Rectangle(withinThis.Location, withinThis.Size);

            if ((align & AnyRight) != 0)
            {
                withinThis.X += withinThis.Width - alignThis.Width;
            }
            else if ((align & AnyCenter) != 0)
            {
                withinThis.X += (withinThis.Width - alignThis.Width) / 2;
            }

            withinThis.Width = alignThis.Width;

            return withinThis;
        }

        private static Rectangle VAlign(Size alignThis, Rectangle withinThis, ContentAlignment align)
        {
            //var aligned = new Rectangle(withinThis.Location, withinThis.Size);

            if ((align & AnyBottom) != 0)
            {
                withinThis.Y += withinThis.Height - alignThis.Height;
            }
            else if ((align & AnyMiddle) != 0)
            {
                withinThis.Y += (withinThis.Height - alignThis.Height) / 2;
            }

            withinThis.Height = alignThis.Height;

            return withinThis;
        }
    }
}
