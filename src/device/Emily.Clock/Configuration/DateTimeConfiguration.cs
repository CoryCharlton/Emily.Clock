using System;

namespace Emily.Clock.Configuration
{
    public class DateTimeConfiguration
    {
        public static DateTimeConfiguration Default { get; } = new()
        {
            BedTime = TimeSpan.FromHours(20),
            TimeZone = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00",
            WakeTime = TimeSpan.FromHours(7)
        };

        public const string SectionName = "DateTime";

        public TimeSpan BedTime { get; set; }

        /// <summary>
        /// A posix timezone string: https://support.cyberdata.net/portal/en/kb/articles/010d63c0cfce3676151e1f2d5442e311
        /// </summary>
        public string TimeZone { get; set; }

        public TimeSpan WakeTime { get; set; }
    }
}
