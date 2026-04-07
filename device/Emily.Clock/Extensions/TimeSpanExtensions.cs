// ReSharper disable once CheckNamespace
namespace System;

// TODO: Move to Core?
internal static class TimeSpanExtensions
{
    public static string ToShortString(this TimeSpan value)
    {
        var timeSpanParts = value.ToString().Split('.');
        return timeSpanParts.Length == 3 ? $"{timeSpanParts[0]}.{timeSpanParts[1]}" : timeSpanParts[0];
    }
}