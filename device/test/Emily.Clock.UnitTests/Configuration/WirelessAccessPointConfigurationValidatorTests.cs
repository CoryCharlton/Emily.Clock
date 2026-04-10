using Emily.Clock.Configuration;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.Configuration;

[TestClass]
public class WirelessAccessPointConfigurationValidatorTests
{
    [TestMethod]
    public void Validate_should_return_failure_when_configuration_is_not_WirelessAccessPointConfiguration()
    {
        var validator = new WirelessAccessPointConfigurationValidator();
        var result = validator.Validate(new object());

        Assert.IsTrue(result.Failed);
        Assert.IsFalse(result.Succeeded);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Configuration object is not the correct type", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_IpAddress_is_invalid()
    {
        var config = new WirelessAccessPointConfiguration
        {
            IpAddress = "not-an-ip",
            Ssid = "Emily.Clock",
            SubnetMask = "255.255.255.0"
        };

        var validator = new WirelessAccessPointConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("IP address is not valid", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_Ssid_is_empty()
    {
        var config = new WirelessAccessPointConfiguration
        {
            IpAddress = "192.168.4.1",
            Ssid = string.Empty,
            SubnetMask = "255.255.255.0"
        };

        var validator = new WirelessAccessPointConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("SSID must not be empty", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_failure_when_SubnetMask_is_invalid()
    {
        var config = new WirelessAccessPointConfiguration
        {
            IpAddress = "192.168.4.1",
            Ssid = "Emily.Clock",
            SubnetMask = "not-a-mask"
        };

        var validator = new WirelessAccessPointConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(1, result.Failures.Length);
        Assert.AreEqual("Subnet mask is not valid", result.Failures[0]);
    }

    [TestMethod]
    public void Validate_should_return_multiple_failures_when_multiple_fields_are_invalid()
    {
        var config = new WirelessAccessPointConfiguration
        {
            IpAddress = "bad-ip",
            Ssid = string.Empty,
            SubnetMask = "bad-mask"
        };

        var validator = new WirelessAccessPointConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsTrue(result.Failed);
        Assert.IsNotNull(result.Failures);
        Assert.AreEqual(3, result.Failures.Length);
    }

    [TestMethod]
    public void Validate_should_return_success_when_configuration_is_valid()
    {
        var config = new WirelessAccessPointConfiguration
        {
            IpAddress = "192.168.4.1",
            Password = string.Empty,
            Ssid = "Emily.Clock",
            SubnetMask = "255.255.255.0"
        };

        var validator = new WirelessAccessPointConfigurationValidator();
        var result = validator.Validate(config);

        Assert.IsFalse(result.Failed);
        Assert.IsTrue(result.Succeeded);
    }
}
