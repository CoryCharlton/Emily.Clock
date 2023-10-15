using Emily.Clock.UI;
using System;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace nanoFramework.Presentation
{
    public static class VerticalAlignmentExtensions
    {
        public static int AlignContent(this VerticalAlignment alignment, Size container, Size content, Padding padding)
        {
            return alignment.AlignContent(container, content.Height, padding);
        }

        public static int AlignContent(this VerticalAlignment alignment, Size container, int contentHeight, Padding padding)
        {
            var y = alignment switch
            {
                VerticalAlignment.Bottom => container.Height - contentHeight - padding.Bottom,
                VerticalAlignment.Center => (container.Height - contentHeight) / 2,
                VerticalAlignment.Stretch => throw new ArgumentOutOfRangeException(),
                VerticalAlignment.Top => 0 + padding.Top,
                _ => throw new ArgumentOutOfRangeException()
            };

            return y > 0 ? y : 0;
        }
    }
}
