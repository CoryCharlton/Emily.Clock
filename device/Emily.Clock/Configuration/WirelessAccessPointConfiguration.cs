namespace Emily.Clock.Configuration;

public class WirelessAccessPointConfiguration
{
    public const string Section = "WirelessAccessPoint";

    public string IpAddress { get; set; } = "192.168.4.1";

    public string Password { get; set; } = string.Empty;

    public string Ssid { get; set; } = "Emily.Clock";

    public string SubnetMask { get; set; } = "255.255.255.0";
}