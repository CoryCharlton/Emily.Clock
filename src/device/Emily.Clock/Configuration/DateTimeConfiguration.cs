using MakoIoT.Device.Utilities.TimeZones;
using Microsoft.Extensions.Logging;
using System;

namespace Emily.Clock.Configuration
{
    public class DateTimeConfiguration
    {
        public const string SectionName = "DateTime";

        public TimeSpan BedTime { get; set; } = TimeSpan.FromHours(20);

        /// <summary>
        /// A posix timezone string: https://support.cyberdata.net/portal/en/kb/articles/010d63c0cfce3676151e1f2d5442e311
        /// </summary>
        public string TimeZone { get; set; } = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00";

        public TimeSpan WakeTime { get; set; } = TimeSpan.FromHours(7);
    }

    // TODO: Add unit tests
    public class DateTimeConfigurationValidator : IConfigurationValidator
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

        public ConfigurationValidationResults ValidateConfiguration(object configuration)
        {
            if (configuration is not DateTimeConfiguration dateTimeConfiguration)
            {
                return new ConfigurationValidationResults();
            }

            var validationResults = new ConfigurationValidationResults();

            if (!IsValidTimeOfDay(dateTimeConfiguration.BedTime))
            {
                validationResults.AddFailure($"Bed time must be between 0:00 and 23:59 [{dateTimeConfiguration.BedTime}]");
            }

            if (!IsValidTimeZone(dateTimeConfiguration.TimeZone))
            {
                validationResults.AddFailure($"Invalid time zone [{dateTimeConfiguration.TimeZone}]");
            }

            if (!IsValidTimeOfDay(dateTimeConfiguration.WakeTime))
            {
                validationResults.AddFailure($"Wake time must be between 0:00 and 23:59 [{dateTimeConfiguration.WakeTime}]");
            }

            return validationResults;
        }
    }
}
