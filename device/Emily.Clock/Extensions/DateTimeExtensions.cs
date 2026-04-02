// ReSharper disable once CheckNamespace
namespace System
{
    // TODO: Move to Core?
    internal static class DateTimeExtensions
    {
        /*
        public static DateTime Date(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
        */

        public static DateTime Time(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }
    }
}
