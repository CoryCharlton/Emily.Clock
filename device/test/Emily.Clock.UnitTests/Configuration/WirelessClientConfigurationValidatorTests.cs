using Emily.Clock.Configuration;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.Configuration;

[TestClass]
public class WirelessClientConfigurationValidatorTests
{
    [TestMethod]
    public void Validate_should_return_failure_when_configuration_is_not_WirelessClientConfiguration()
    {
        var validator = new WirelessClientConfigurationValidator();
        var result = validator.Validate(new object());

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Configuration object is not the correct type", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_ConnectionTimeout_exceeds_300()
    {
        var config = new WirelessClientConfiguration
        {
            ConnectionTimeout = 301,
            Ssid = "MyNetwork"
        };

        var validator = new WirelessClientConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Connection timeout must be between 1 and 300 seconds", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_ConnectionTimeout_is_less_than_1()
    {
        var config = new WirelessClientConfiguration
        {
            ConnectionTimeout = 0,
            Ssid = "MyNetwork"
        };

        var validator = new WirelessClientConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Connection timeout must be between 1 and 300 seconds", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_Ssid_is_empty()
    {
        var config = new WirelessClientConfiguration
        {
            ConnectionTimeout = 60,
            Ssid = string.Empty
        };

        var validator = new WirelessClientConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("SSID must not be empty", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_multiple_failures_when_multiple_fields_are_invalid()
    {
        var config = new WirelessClientConfiguration
        {
            ConnectionTimeout = 0,
            Ssid = string.Empty
        };

        var validator = new WirelessClientConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(2, result.Failures.Length);
    }

    [TestMethod]
    public void Validate_should_return_success_when_configuration_is_valid()
    {
        var config = new WirelessClientConfiguration
        {
            ConnectionTimeout = 60,
            Password = string.Empty,
            Ssid = "MyNetwork"
        };

        var validator = new WirelessClientConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsFalse(result.Failed);
        Assert.IsTrue(result.Succeeded);
    }
}
