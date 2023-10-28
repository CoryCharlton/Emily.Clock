using MakoIoT.Device.Utilities.TimeZones;
using System;
using System.Collections;
using CCSWE.nanoFramework.Configuration;

namespace Emily.Clock.Configuration
{
    public class DateTimeConfiguration
    {
        public const string Section = "DateTime";

        public TimeSpan BedTime { get; set; } = TimeSpan.FromHours(20);

        /// <summary>
        /// A posix timezone string: https://support.cyberdata.net/portal/en/kb/articles/010d63c0cfce3676151e1f2d5442e311
        /// </summary>
        public string TimeZone { get; set; } = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00";

        public TimeSpan WakeTime { get; set; } = TimeSpan.FromHours(7);
    }

    // TODO: Add unit tests
    public class DateTimeConfigurationValidator : IValidateConfiguration
    {
        private static bool IsValidTimeOfDay(TimeSpan timeOfDay)
        {
            return timeOfDay >= TimeSpan.Zero && timeOfDay <= new TimeSpan(0, 23, 59, 59, 999);
        }

        public static bool IsValidTimeZone(string timeZone)
        {
            try
            {
                TimeZoneConverter.FromPosixString(timeZone);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ValidateConfigurationResult Validate(object configuration)
        {
            if (configuration is not DateTimeConfiguration dateTimeConfiguration)
            {
                return ValidateConfigurationResult.Fail("Configuration object is not the correct type");
            }

            var failures = new ArrayList();

            if (!IsValidTimeOfDay(dateTimeConfiguration.BedTime))
            {
                failures.Add($"Bed time must be between 0:00 and 23:59 [{dateTimeConfiguration.BedTime}]");
            }

            if (!IsValidTimeZone(dateTimeConfiguration.TimeZone))
            {
                failures.Add($"Invalid time zone [{dateTimeConfiguration.TimeZone}]");
            }

            if (!IsValidTimeOfDay(dateTimeConfiguration.WakeTime))
            {
                failures.Add($"Wake time must be between 0:00 and 23:59 [{dateTimeConfiguration.WakeTime}]");
            }

            return failures.Count > 0 ? ValidateConfigurationResult.Fail((string[]) failures.ToArray()) : ValidateConfigurationResult.Success;
        }
    }
}
