using Emily.Clock.Configuration;
using System.Net.NetworkInformation;
using System.Net;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.DhcpServer;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Events;
using Microsoft.Extensions.Logging;
using AuthenticationType = System.Net.NetworkInformation.AuthenticationType;

namespace Emily.Clock.Networking;

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
    private readonly IConfigurationManager _configurationManager;
    private readonly ILogger _logger;
    private DhcpServer? _dhcpServer;
    private readonly IMediator _mediator;
    private readonly INetworkInterfaceProvider _networkInterfaceProvider;

    public WirelessAccessPointManager(IConfigurationManager configurationManager, ILogger logger, IMediator mediator, INetworkInterfaceProvider networkInterfaceProvider)
    {
        _configurationManager = configurationManager;
        _configurationManager.ConfigurationChanged += OnConfigurationChanged;
        _configuration = (WirelessAccessPointConfiguration) _configurationManager.Get(WirelessAccessPointConfiguration.Section);
        _logger = logger;
        _mediator = mediator;
        _networkInterfaceProvider = networkInterfaceProvider;
    }

    public string IpAddress => GetNetworkInterface().IPv4Address ?? string.Empty;

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

    private WirelessAPConfiguration GetNetworkConfiguration() => WirelessAPConfiguration.GetAllWirelessAPConfigurations()[GetNetworkInterface().SpecificConfigId];

    private NetworkInterface GetNetworkInterface() => _networkInterfaceProvider.RequireInterface(NetworkInterfaceType.WirelessAP);

    private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
    {
        if (e.Section != WirelessAccessPointConfiguration.Section)
        {
            return;
        }

        if (e.Configuration is not WirelessAccessPointConfiguration configuration)
        {
            return;
        }

        // TODO: We should respond to this...
        _configuration = configuration;
    }

    private void PublishStatusEvent(string message)
    {
        _mediator.Publish(new StatusEvent(message));
    }

    public bool Start()
    {
        PublishStatusEvent($"Starting access point {_configuration.Ssid}...");

        _dhcpServer ??= new DhcpServer(IPAddress.Parse(_configuration.IpAddress), _logger)
        {
            CaptivePortalUrl = $"http://{_configuration.IpAddress}/api.json",
            DnsServer = IPAddress.Parse(_configuration.IpAddress),
        };

        var started = _dhcpServer.Start();

        PublishStatusEvent(started ? $"Access point started {_configuration.Ssid}" : $"Failed to start access point {_configuration.Ssid}");

        return started;
    }

    public void Stop()
    {
        PublishStatusEvent("Stopping access point...");

        _dhcpServer?.Stop();
        _dhcpServer = null;
    }
}