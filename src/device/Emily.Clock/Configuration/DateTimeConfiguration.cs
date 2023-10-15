namespace Emily.Clock.Configuration
{
    public class DateTimeConfiguration
    {
        public static DateTimeConfiguration Default { get; } = new() { TimeZone = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00" };
        public const string SectionName = "DateTime";

        /// <summary>
        /// A posix timezone string: https://support.cyberdata.net/portal/en/kb/articles/010d63c0cfce3676151e1f2d5442e311
        /// </summary>
        public string TimeZone { get; set; }
    }
}
