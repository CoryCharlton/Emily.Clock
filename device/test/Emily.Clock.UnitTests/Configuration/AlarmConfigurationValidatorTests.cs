using Emily.Clock.Configuration;
using nanoFramework.TestFramework;
using System;

namespace Emily.Clock.UnitTests.Configuration;

[TestClass]
public class AlarmConfigurationValidatorTests
{
    [TestMethod]
    public void Validate_should_return_failure_when_AlarmTime_is_negative()
    {
        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(-1),
            MaxDurationMinutes = 10
        };

        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Alarm time must be between 0:00 and 23:59", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_AlarmTime_exceeds_23_59()
    {
        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(24),
            MaxDurationMinutes = 10
        };

        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Alarm time must be between 0:00 and 23:59", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_configuration_is_not_AlarmConfiguration()
    {
        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(new object());

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Configuration object is not the correct type", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_MaxDurationMinutes_is_less_than_1()
    {
        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(7),
            MaxDurationMinutes = 0
        };

        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Max duration must be between 1 and 30 minutes", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_MaxDurationMinutes_exceeds_30()
    {
        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(7),
            MaxDurationMinutes = 31
        };

        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Max duration must be between 1 and 30 minutes", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_multiple_failures_when_multiple_fields_are_invalid()
    {
        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(25),
            MaxDurationMinutes = 0
        };

        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(2, result.Failures.Length);
    }

    [TestMethod]
    public void Validate_should_return_success_when_configuration_is_valid()
    {
        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(7),
            MaxDurationMinutes = 10
        };

        var validator = new AlarmConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsFalse(result.Failed);
        Assert.IsTrue(result.Succeeded);
    }
}
