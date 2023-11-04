using Emily.Clock.UI;
using nanoFramework.Presentation;
using nanoFramework.UI;

// ReSharper disable once CheckNamespace
#nullable enable
namespace System.Drawing
{
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new(0, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class with the specified location
        /// and size.
        /// </summary>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class with the specified location and size.
        /// </summary>
        public Rectangle(Point location, Size size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref="Rectangle"/>.
        /// </summary>
        public Point Location
        {
            readonly get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref="Rectangle"/>.
        /// </summary>
        public Size Size
        {
            readonly get => new(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref="Rectangle"/>.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref="Rectangle"/>.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref="Rectangle"/>.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref="Rectangle"/>.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref="Rectangle"/> .
        /// </summary>
        public readonly int Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref="Rectangle"/>.
        /// </summary>
        public readonly int Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref="Rectangle"/>.
        /// </summary>
        public readonly int Right => unchecked(X + Width);

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref="Rectangle"/>.
        /// </summary>
        public readonly int Bottom => unchecked(Y + Height);

        /// <summary>
        /// Tests whether this <see cref="Rectangle"/> has a <see cref="Width"/>
        /// or a <see cref="Height"/> of 0.
        /// </summary>
        public readonly bool IsEmpty => Height == 0 && Width == 0 && X == 0 && Y == 0;

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref="Rectangle"/> with the same location
        /// and size of this Rectangle.
        /// </summary>
        public readonly override bool Equals(object? obj) => obj is Rectangle rectangle && Equals(rectangle);

        public readonly bool Equals(Rectangle other) => this == other;

        public readonly override int GetHashCode()
        {
            //HashCode.Combine(X, Y, Width, Height);

            unchecked
            {
                return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
            }
        }

        /// <summary>
        /// Tests whether two <see cref="Rectangle"/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(Rectangle left, Rectangle right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref="Rectangle"/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

        //public readonly override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Converts the attributes of this <see cref="Rectangle"/> to a human readable string.
        /// </summary>
        public readonly override string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
    }

    // TODO: Replace this with LayoutUtils calls
    public static class RectangleExtensions
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
