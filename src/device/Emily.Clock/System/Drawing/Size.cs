// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Emily.Clock.UI;
using nanoFramework.Presentation;
using nanoFramework.UI;

// ReSharper disable once CheckNamespace
#nullable enable
namespace System.Drawing
{
    public struct Size
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class with <see cref="Width"/> and <see cref="Height"/> set to zero.
        /// </summary>
        public static readonly Size Empty = new(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class from the specified
        /// <see cref='Point'/>.
        /// </summary>
        public Size(Point point)
        {
            Width = point.X;
            Height = point.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class from the specified dimensions.
        /// </summary>
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Represents the horizontal component of this <see cref='Size'/>.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Represents the vertical component of this <see cref='Size'/>.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Performs vector addition of two <see cref="Size"/> objects.
        /// </summary>
        public static Size operator +(Size sz1, Size sz2) => Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref="Size"/> by another <see cref="Size"/>
        /// </summary>
        public static Size operator -(Size sz1, Size sz2) => Subtract(sz1, sz2);

        /// <summary>
        /// Tests whether two <see cref="Size"/> objects are identical.
        /// </summary>
        public static bool operator ==(Size sz1, Size sz2) => sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref="Size"/> objects are different.
        /// </summary>
        public static bool operator !=(Size sz1, Size sz2) => !(sz1 == sz2);

        /// <summary>
        /// Performs vector addition of two <see cref="Size"/> objects.
        /// </summary>
        public static Size Add(Size sz1, Size sz2) =>
            new(unchecked(sz1.Width + sz2.Width), unchecked(sz1.Height + sz2.Height));
        
        /// <summary>
        /// Tests to see whether the specified object is a <see cref="Size"/>  with the same dimensions
        /// as this <see cref="Size"/>.
        /// </summary>
        public readonly override bool Equals(object? obj) => obj is Size size && Equals(size);

        public readonly bool Equals(Size other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public readonly override int GetHashCode()
        {
            // HashCode.Combine(Width, Height);
            unchecked
            {
                return Width.GetHashCode() ^ Height.GetHashCode();
            }
        }

        /// <summary>
        /// Multiplies <see cref="Size"/> by an <see cref="int"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref='int'/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        private static Size Multiply(Size size, int multiplier) => new(unchecked(size.Width * multiplier), unchecked(size.Height * multiplier));

        /// <summary>
        /// Contracts a <see cref="Size"/> by another <see cref="Size"/> .
        /// </summary>
        public static Size Subtract(Size sz1, Size sz2) =>
            new(unchecked(sz1.Width - sz2.Width), unchecked(sz1.Height - sz2.Height));


        /// <summary>
        /// Creates a human-readable string that represents this <see cref="Size"/>.
        /// </summary>
        public readonly override string ToString() => $"{{Width={Width}, Height={Height}}}";
    }

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
