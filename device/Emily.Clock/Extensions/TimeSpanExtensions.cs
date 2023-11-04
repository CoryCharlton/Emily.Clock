// ReSharper disable once CheckNamespace
namespace System
{
    internal static class TimeSpanExtensions
    {
        public static string ToShortString(this TimeSpan value)
        {
            // TODO: This could use work. Days will have a second period and maybe no millis
            var timeSpanParts = value.ToString().Split('.');
            return timeSpanParts[0];
        }
    }
}
