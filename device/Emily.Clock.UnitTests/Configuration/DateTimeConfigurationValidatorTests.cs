using Emily.Clock.Configuration;
using nanoFramework.TestFramework;
using System;

namespace Emily.Clock.UnitTests.Configuration;

[TestClass]
public class DateTimeConfigurationValidatorTests
{
    [TestMethod]
    public void Validate_should_return_failure_when_BedTime_is_invalid()
    {
        var config = new DateTimeConfiguration
        {
            BedTime = TimeSpan.FromHours(24),
            TimeZone = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00",
            WakeTime = TimeSpan.FromHours(6)
        };

        var validator = new DateTimeConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);

        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Bed time must be between 0:00 and 23:59", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_configuration_is_not_DateTimeConfiguration()
    {
        var validator = new DateTimeConfigurationValidator();
        var result = validator.Validate(new object());

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);

        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Configuration object is not the correct type", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_TimeZone_is_invalid()
    {
        var config = new DateTimeConfiguration
        {
            BedTime = TimeSpan.FromHours(22),
            TimeZone = "InvalidTimeZone",
            WakeTime = TimeSpan.FromHours(6)
        };

        var validator = new DateTimeConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);

        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Invalid time zone", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_WakeTime_is_invalid()
    {
        var config = new DateTimeConfiguration
        {
            BedTime = TimeSpan.FromHours(22),
            TimeZone = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00",
            WakeTime = TimeSpan.FromHours(24)
        };

        var validator = new DateTimeConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);

        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Wake time must be between 0:00 and 23:59", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_success_when_configuration_is_valid()
    {
        var config = new DateTimeConfiguration
        {
            BedTime = TimeSpan.FromHours(22),
            TimeZone = "PST8PDT,M3.2.0/2:00:00,M11.1.0/2:00:00",
            WakeTime = TimeSpan.FromHours(6)
        };

        var validator = new DateTimeConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsFalse(result.Failed);
        Assert.IsTrue(result.Succeeded);
    }
}