using System;
using System.Collections;
using CCSWE.nanoFramework.Configuration;

namespace Emily.Clock.Configuration;

public class AlarmConfiguration
{
    public const string Section = "Alarm";

    public TimeSpan AlarmTime { get; set; } = TimeSpan.MinValue;

    public bool Enabled { get; set; }

    public int MaxDurationMinutes { get; set; } = -1;

    public static readonly AlarmConfiguration Defaults = new()
    {
        AlarmTime = TimeSpan.FromHours(7),
        Enabled = false,
        MaxDurationMinutes = 10
    };
}

public class AlarmConfigurationValidator : IValidateConfiguration
{
    private static bool IsValidTimeOfDay(TimeSpan timeOfDay)
    {
        return timeOfDay >= TimeSpan.Zero && timeOfDay <= new TimeSpan(0, 23, 59, 59, 999);
    }

    public ValidateConfigurationResult Validate(object? configuration)
    {
        if (configuration is not AlarmConfiguration alarmConfiguration)
        {
            return ValidateConfigurationResult.Fail("Configuration object is not the correct type");
        }

        var failures = new ArrayList();

        if (!IsValidTimeOfDay(alarmConfiguration.AlarmTime))
        {
            failures.Add("Alarm time must be between 0:00 and 23:59");
        }

        if (alarmConfiguration.MaxDurationMinutes is < 1 or > 30)
        {
            failures.Add("Max duration must be between 1 and 30 minutes");
        }

        return failures.Count > 0 ? ValidateConfigurationResult.Fail((string[]) failures.ToArray(typeof(string))) : ValidateConfigurationResult.Success;
    }
}
