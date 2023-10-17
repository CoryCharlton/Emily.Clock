using System;
using System.Net.NetworkInformation;
using System.Threading;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Configuration;
using Emily.Clock.Mediator.Events;
using MakoIoT.Device.Services.Interface;
using Microsoft.Extensions.Logging;
using nanoFramework.Networking;

namespace Emily.Clock.Networking
{
    public interface IWirelessClientManager
    {
        string IpAddress { get; }
        bool IsConfigured { get; }
        bool IsConnected { get; }
        bool IsEnabled { get; }
        string Ssid { get; }

        bool Connect();
        void Disable();
        void Disconnect();
        void Enable(bool autoConnect = false);
    }

    public class WirelessClientManager : IWirelessClientManager
    {
        private WirelessClientConfiguration _configuration;
        private readonly IConfigurationService _configurationService;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly INetworkInterfaceProvider _networkInterfaceProvider;

        public WirelessClientManager(IConfigurationService configurationService, ILogger logger, IMediator mediator, INetworkInterfaceProvider networkInterfaceProvider)
        {
            _configurationService = configurationService;
            _configurationService.ConfigurationUpdated += OnConfigurationUpdated;
            _configuration = GetConfiguration();
            _logger = logger;
            _mediator = mediator;
            _networkInterfaceProvider = networkInterfaceProvider;
        }

        public string IpAddress => GetNetworkInterface()?.IPv4Address;

        public bool IsConfigured => !string.IsNullOrEmpty(_configuration.Ssid);

        public bool IsConnected => Status == NetworkHelperStatus.NetworkIsReady;

        public bool IsEnabled => (GetNetworkConfiguration().Options & Wireless80211Configuration.ConfigurationOptions.Enable) == Wireless80211Configuration.ConfigurationOptions.Enable;

        public string Ssid => _configuration.Ssid;

        private NetworkHelperStatus Status => WifiNetworkHelper.Status;

        public bool Connect()
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException();
            }

            PublishStatusEvent($"Connecting to {_configuration.Ssid}...");

            if (!IsConnected)
            {
                var cancellationTokenSource = new CancellationTokenSource(_configuration.ConnectionTimeout * 1000);
                var connected = WifiNetworkHelper.ConnectDhcp(_configuration.Ssid, _configuration.Password, requiresDateTime: true, token: cancellationTokenSource.Token);

                if (!connected)
                {
                    _logger.LogWarning($"Failed to connect to {_configuration.Ssid} [{Status}]: {WifiNetworkHelper.HelperException}");
                }
            }

            PublishStatusEvent(IsConnected ? $"Connected to {_configuration.Ssid}" : $"Failed to connect to {_configuration.Ssid}");

            return IsConnected;
        }

        public void Disable()
        {
            var configuration = GetNetworkConfiguration();
            configuration.Options = Wireless80211Configuration.ConfigurationOptions.Disable;
            configuration.SaveConfiguration();
        }

        public void Disconnect() => WifiNetworkHelper.Disconnect();

        public void Enable(bool autoConnect = false)
        {
            var configuration = GetNetworkConfiguration();
            configuration.Options = Wireless80211Configuration.ConfigurationOptions.Enable;

            if (autoConnect)
            {
                configuration.Options |= Wireless80211Configuration.ConfigurationOptions.AutoConnect;
            }

            configuration.SaveConfiguration();
        }

        private WirelessClientConfiguration GetConfiguration()
        {
            return (WirelessClientConfiguration) _configurationService.GetConfigSection(WirelessClientConfiguration.SectionName, typeof(WirelessClientConfiguration));
        }

        private Wireless80211Configuration GetNetworkConfiguration() => Wireless80211Configuration.GetAllWireless80211Configurations()[GetNetworkInterface().SpecificConfigId];

        private NetworkInterface GetNetworkInterface() => _networkInterfaceProvider.GetInterface(NetworkInterfaceType.Wireless80211);

        private void OnConfigurationUpdated(object sender, EventArgs e)
        {
            if (e is not ObjectEventArgs objectEventArgs || !string.Equals(WirelessClientConfiguration.SectionName, objectEventArgs.Data as string))
            {
                return;
            }

            _configuration = GetConfiguration();
        }

        private void PublishStatusEvent(string message)
        {
            _mediator.Publish(new StatusEvent(message));
        }
    }
}
