using MakoIoT.Device.Services.Interface;

namespace Emily.Clock.Configuration
{
    public static class ConfigurationServiceExtensions
    {
        public static DateTimeConfiguration GetDateTimeConfiguration(this IConfigurationService configurationService)
        {
            return (DateTimeConfiguration) configurationService.GetConfigSection(DateTimeConfiguration.SectionName, typeof(DateTimeConfiguration));
        }

        public static WirelessAccessPointConfiguration GetWirelessAccessPointConfiguration(this IConfigurationService configurationService)
        {
            return (WirelessAccessPointConfiguration) configurationService.GetConfigSection(WirelessAccessPointConfiguration.SectionName, typeof(WirelessAccessPointConfiguration));
        }

        public static WirelessClientConfiguration GetWirelessClientConfiguration(this IConfigurationService configurationService)
        {
            return (WirelessClientConfiguration) configurationService.GetConfigSection(WirelessClientConfiguration.SectionName, typeof(WirelessClientConfiguration));
        }
    }
}
