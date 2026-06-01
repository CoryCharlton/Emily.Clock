using System.Collections;
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
    // Manual IPv4 parser. IPAddress.Parse is broken in nanoFramework.System.Net 1.11.52
    // (throws for every valid IPv4 input); see nanoframework/Home#1781.
    private static bool IsValidIpAddress(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        var parts = value.Split('.');
        if (parts.Length != 4)
        {
            return false;
        }

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part) || part.Length > 3)
            {
                return false;
            }

            for (var i = 0; i < part.Length; i++)
            {
                if (part[i] < '0' || part[i] > '9')
                {
                    return false;
                }
            }

            int octet;
            try
            {
                octet = int.Parse(part);
            }
            catch
            {
                return false;
            }

            if (octet < 0 || octet > 255)
            {
                return false;
            }
        }

        return true;
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