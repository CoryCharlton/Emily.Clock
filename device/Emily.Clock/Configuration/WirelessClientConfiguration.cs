using System.Collections;
using CCSWE.nanoFramework.Configuration;

namespace Emily.Clock.Configuration;

public class WirelessClientConfiguration
{
    public const string Section = "WirelessClient";

    public static readonly WirelessClientConfiguration Defaults = new()
    {
        ConnectionTimeout = 60,
        Password = string.Empty,
        Ssid = string.Empty
    };

    /// <summary>
    /// Connection timeout in seconds
    /// </summary>
    public int ConnectionTimeout { get; set; } = -1;

    /// <summary>
    /// The password used to connect
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The SSID to connect to
    /// </summary>
    public string Ssid { get; set; } = string.Empty;
}

public class WirelessClientConfigurationValidator : IValidateConfiguration
{
    public ValidateConfigurationResult Validate(object? configuration)
    {
        if (configuration is not WirelessClientConfiguration wirelessClientConfiguration)
        {
            return ValidateConfigurationResult.Fail("Configuration object is not the correct type");
        }

        var failures = new ArrayList();

        if (wirelessClientConfiguration.ConnectionTimeout is < 1 or > 300)
        {
            failures.Add("Connection timeout must be between 1 and 300 seconds");
        }

        if (string.IsNullOrEmpty(wirelessClientConfiguration.Ssid))
        {
            failures.Add("SSID must not be empty");
        }

        return failures.Count > 0 ? ValidateConfigurationResult.Fail((string[]) failures.ToArray(typeof(string))) : ValidateConfigurationResult.Success;
    }
}