using System;
using Emily.Clock.Configuration;
using MakoIoT.Device.Services.Interface;
using System.Net.NetworkInformation;
using Iot.Device.DhcpServer;
using System.Net;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Mediator.Events;
using MakoIoT.Device.Utilities.Invoker;
using AuthenticationType = System.Net.NetworkInformation.AuthenticationType;

namespace Emily.Clock.Networking
{
    public interface IWirelessAccessPointManager
    {
        string IpAddress { get; }
        bool IsEnabled { get; }
        string Ssid { get; }

        void Disable();
        void Enable();
        bool Start();
        void Stop();
    }

    public class WirelessAccessPointManager : IWirelessAccessPointManager
    {
        private WirelessAccessPointConfiguration _configuration;
        private readonly IConfigurationService _configurationService;
        private DhcpServer _dhcpServer;
        private readonly IMediator _mediator;
        private readonly INetworkInterfaceProvider _networkInterfaceProvider;

        public WirelessAccessPointManager(IConfigurationService configurationService, IMediator mediator, INetworkInterfaceProvider networkInterfaceProvider)
        {
            _configurationService = configurationService;
            _configurationService.ConfigurationUpdated += OnConfigurationUpdated;
            _configuration = GetConfiguration();
            _mediator = mediator;
            _networkInterfaceProvider = networkInterfaceProvider;
        }

        public string IpAddress => GetNetworkInterface()?.IPv4Address;

        /*
         * Add IsConfigured and check IsEnabled + IpAddress is correct?
         *
         *             // Check if already Enabled and return true
            if (wapconf.Options == (WirelessAPConfiguration.ConfigurationOptions.Enable |
                                    WirelessAPConfiguration.ConfigurationOptions.AutoStart) &&
                ni.IPv4Address == SoftApIP )
            {
                return true;
            }
         */
        //public bool IsConfigured => !string.IsNullOrEmpty(_configuration.Ssid);

        //public bool IsConnected => Status == NetworkHelperStatus.NetworkIsReady;

        public bool IsEnabled => (GetNetworkConfiguration().Options & WirelessAPConfiguration.ConfigurationOptions.Enable) == WirelessAPConfiguration.ConfigurationOptions.Enable;

        public string Ssid => _configuration.Ssid;

        //private NetworkHelperStatus Status => WifiNetworkHelper.Status;

        public void Disable()
        {
            var networkInterface = GetNetworkInterface();
            networkInterface.EnableDhcp();

            var networkConfiguration = GetNetworkConfiguration();
            networkConfiguration.Options = WirelessAPConfiguration.ConfigurationOptions.Disable;

            networkConfiguration.SaveConfiguration();
        }

        public void Enable()
        {
            var networkInterface = GetNetworkInterface();
            networkInterface.EnableStaticIPv4(_configuration.IpAddress, _configuration.SubnetMask, _configuration.IpAddress);

            var networkConfiguration = GetNetworkConfiguration();
            networkConfiguration.Authentication = string.IsNullOrEmpty(_configuration.Password) ? AuthenticationType.Open : AuthenticationType.WPA2;
            networkConfiguration.MaxConnections = 2;
            networkConfiguration.Options = WirelessAPConfiguration.ConfigurationOptions.AutoStart | WirelessAPConfiguration.ConfigurationOptions.Enable;
            networkConfiguration.Password = _configuration.Password;
            networkConfiguration.Ssid = _configuration.Ssid;

            networkConfiguration.SaveConfiguration();
        }

        private WirelessAccessPointConfiguration GetConfiguration()
        {
            return (WirelessAccessPointConfiguration) _configurationService.GetConfigSection(WirelessAccessPointConfiguration.SectionName, typeof(WirelessAccessPointConfiguration));
        }

        private WirelessAPConfiguration GetNetworkConfiguration() => WirelessAPConfiguration.GetAllWirelessAPConfigurations()[GetNetworkInterface().SpecificConfigId];

        private NetworkInterface GetNetworkInterface() => _networkInterfaceProvider.GetInterface(NetworkInterfaceType.WirelessAP);

        private void OnConfigurationUpdated(object sender, EventArgs e)
        {
            if (e is not ObjectEventArgs objectEventArgs || !string.Equals(WirelessAccessPointConfiguration.SectionName, objectEventArgs.Data as string))
            {
                return;
            }

            _configuration = GetConfiguration();
        }

        private void PublishStatusEvent(string message)
        {
            _mediator.Publish(new StatusEvent(message));
        }

        public bool Start()
        {
            PublishStatusEvent($"Starting access point {_configuration.Ssid}...");

            _dhcpServer ??= new DhcpServer { CaptivePortalUrl = $"http://{_configuration.IpAddress}" };

            var started = false;

            Invoker.Retry(() =>
            {
                started = _dhcpServer.Start(IPAddress.Parse(_configuration.IpAddress), new IPAddress(new byte[] { 255, 255, 255, 0 }));
                if (!started)
                {
                    throw new Exception("DHCP failed to start");
                }
            }, 3);

            PublishStatusEvent(started ? $"Access point started {_configuration.Ssid}" : $"Failed to start access point {_configuration.Ssid}");

            return started;
        }

        public void Stop()
        {
            PublishStatusEvent($"Stopping access point...");

            _dhcpServer?.Stop();
            _dhcpServer = null;
        }
    }
}
