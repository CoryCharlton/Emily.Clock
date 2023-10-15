using System;

namespace Emily.Clock.Configuration
{
    public class WirelessAccessPointConfiguration
    {
        public static WirelessAccessPointConfiguration Default { get; } = new() { Ssid = "Emily.Clock" };
        public const string SectionName = "WirelessAccessPoint";

        public string IpAddress { get; set; } = "192.168.4.1";

        public string Password { get; set; } = string.Empty;

        public string Ssid { get; set; } = string.Empty;

        public string SubnetMask { get; set; } = "255.255.255.0";
    }
}
