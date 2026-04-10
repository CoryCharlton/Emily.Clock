using System.Collections;
using System.Net;
using CCSWE.nanoFramework.Configuration;

namespace Emily.Clock.Configuration;

public class WirelessAccessPointConfiguration
{
    public const string Section = "WirelessAccessPoint";

    public static readonly WirelessAccessPointConfiguration Defaults = new()
    {
        IpAddress = "192.168.4.1",
        Password = string.Empty,
        Ssid = "Emily.Clock",
        SubnetMask = "255.255.255.0"
    };

    public string IpAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Ssid { get; set; } = string.Empty;

    public string SubnetMask { get; set; } = string.Empty;
}

public class WirelessAccessPointConfigurationValidator : IValidateConfiguration
{
    private static bool IsValidIpAddress(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        try
        {
            IPAddress.Parse(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public ValidateConfigurationResult Validate(object? configuration)
    {
        if (configuration is not WirelessAccessPointConfiguration wirelessAccessPointConfiguration)
        {
            return ValidateConfigurationResult.Fail("Configuration object is not the correct type");
        }

        var failures = new ArrayList();

        if (!IsValidIpAddress(wirelessAccessPointConfiguration.IpAddress))
        {
            failures.Add("IP address is not valid");
        }

        if (string.IsNullOrEmpty(wirelessAccessPointConfiguration.Ssid))
        {
            failures.Add("SSID must not be empty");
        }

        if (!IsValidIpAddress(wirelessAccessPointConfiguration.SubnetMask))
        {
            failures.Add("Subnet mask is not valid");
        }

        return failures.Count > 0 ? ValidateConfigurationResult.Fail((string[]) failures.ToArray(typeof(string))) : ValidateConfigurationResult.Success;
    }
}