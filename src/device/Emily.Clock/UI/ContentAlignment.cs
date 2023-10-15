using System;
using nanoFramework.Presentation;

namespace Emily.Clock.UI
{
    public enum ContentAlignment
    {
        /// <summary>
        /// Content is vertically aligned at the top, and horizontally aligned on the left.
        /// </summary>
        TopLeft = 0x001,
        /// <summary>
        /// Content is vertically aligned at the top, and horizontally aligned at the center.
        /// </summary>
        TopCenter = 0x002,
        /// <summary>
        /// Content is vertically aligned at the top, and horizontally aligned on the right.
        /// </summary>
        TopRight = 0x004,
        /// <summary>
        /// Content is vertically aligned in the middle, and horizontally aligned on the left.
        /// </summary>
        MiddleLeft = 0x010,
        /// <summary>
        /// Content is vertically aligned in the middle, and horizontally aligned at the center.
        /// </summary>
        MiddleCenter = 0x020,
        /// <summary>
        /// Content is vertically aligned in the middle, and horizontally aligned on the right.
        /// </summary>
        MiddleRight = 0x040,
        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally aligned on the left.
        /// </summary>
        BottomLeft = 0x100,
        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally aligned at the center.
        /// </summary>
        BottomCenter = 0x200,
        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally aligned on the right.
        /// </summary>
        BottomRight = 0x400,
    }

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
}
