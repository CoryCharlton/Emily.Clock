namespace Emily.Clock.Configuration;

public class WirelessClientConfiguration
{
    public const string Section = "WirelessClient";

    /// <summary>
    /// Connection timeout in seconds
    /// </summary>
    public int ConnectionTimeout { get; set; } = 60;

    /// <summary>
    /// The password used to connect
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The SSID to connect to
    /// </summary>
    public string Ssid { get; set; } = string.Empty;
}