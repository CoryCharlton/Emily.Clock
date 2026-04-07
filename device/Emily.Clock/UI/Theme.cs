using System.Drawing;
using nanoFramework.UI;

// ReSharper disable InconsistentNaming
namespace Emily.Clock.UI;

internal static class Theme
{
    public static readonly Color Primary = Color.Black;
    public static readonly Color Secondary = Color.DimGray;

    public static readonly Color OnPrimary = Color.FromHex("#F8F8F8");
    public static readonly Color OnSecondary = Color.FromHex("#F8F8F8");

    public static readonly Color Highlight = Color.FromHex("#659EC7");

    public static readonly Color PrimaryText = Color.FromHex("#F8F8F8");
    public static readonly Color SecondaryText = Color.FromHex("#C2C2C2");

    public static readonly Padding ContentPadding = new(2, 5);
    public static readonly Padding ControlPadding = new(5, 5);

    public const byte CornerRadius = 7;

    public static Font ClockFont { get; } = Resources.GetFont(Resources.FontResources.Inter_Medium_72);
    public static Font LargeFont { get; } = Resources.GetFont(Resources.FontResources.Inter_Medium_20);
    public static Font MediumFont { get; } = Resources.GetFont(Resources.FontResources.Inter_Medium_16);
    public static Font SmallFont { get; } = Resources.GetFont(Resources.FontResources.Inter_Medium_14);
    public static Font TinyFont { get; } = Resources.GetFont(Resources.FontResources.Consolas_Regular_10);
}