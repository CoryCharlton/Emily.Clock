using System.Drawing;

namespace Emily.Clock.UI
{
    public readonly struct Padding
    {
        public static readonly Padding Empty = new(0);

        public Padding(ushort padding) : this(padding, padding)
        {

        }

        public Padding(ushort vertical, ushort horizontal) : this(vertical, horizontal, vertical, horizontal)
        {

        }

        public Padding(ushort top, ushort right, ushort bottom, ushort left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public ushort Top { get; }
        public ushort Right { get; }
        public ushort Bottom { get; }
        public ushort Left { get; }

        public int Horizontal => Left + Right;
        public int Vertical => Top + Bottom;

        public Size Size => new(Horizontal, Vertical);

        public override bool Equals(object? other)
        {
            return other is Padding otherPadding && Equals(otherPadding);
        }

        public bool Equals(Padding other)
            => Left == other.Left
               && Top == other.Top
               && Right == other.Right
               && Bottom == other.Bottom;

        public override int GetHashCode()
        {
            // HashCode.Combine(Left, Top, Right, Bottom);
            unchecked
            {
                // ReSharper disable UsageOfDefaultStructEquality
                return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
                // ReSharper restore UsageOfDefaultStructEquality
            }
        }

        /*
        /// <summary>
        ///  Performs vector addition of two <see cref="Padding"/> objects.
        /// </summary>
        public static Padding operator +(Padding p1, Padding p2)
        {
            return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
        }

        /// <summary>
        ///  Contracts a <see cref="Drawing.Size"/> by another <see cref="Drawing.Size"/>.
        /// </summary>
        public static Padding operator -(Padding p1, Padding p2)
        {
            return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
        }
        */

        /// <summary>
        ///  Tests whether two <see cref="Padding"/> objects are identical.
        /// </summary>
        public static bool operator ==(Padding p1, Padding p2)
        {
            return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom;
        }

        /// <summary>
        ///  Tests whether two <see cref="Padding"/> objects are different.
        /// </summary>
        public static bool operator !=(Padding p1, Padding p2) => !(p1 == p2);
    }
}