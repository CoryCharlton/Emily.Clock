using System;
using System.Drawing;
using Emily.Clock.UI;

// ReSharper disable once CheckNamespace
namespace nanoFramework.Presentation
{
    public static class HorizontalAlignmentExtensions
    {
        public static int AlignContent(this HorizontalAlignment alignment, Size container, Size content, Padding padding)
        {
            return alignment.AlignContent(container, content.Width, padding);
        }

        public static int AlignContent(this HorizontalAlignment alignment, Size container, int contentWidth, Padding padding)
        {
            var x = alignment switch
            {
                HorizontalAlignment.Center => (container.Width - contentWidth) / 2,
                HorizontalAlignment.Left => 0 + padding.Left,
                HorizontalAlignment.Right => container.Width - contentWidth - padding.Right,
                HorizontalAlignment.Stretch => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };

            return x > 0 ? x : 0;
        }
    }
}
