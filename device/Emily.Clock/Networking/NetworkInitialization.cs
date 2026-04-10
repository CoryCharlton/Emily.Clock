using System;
using System.Net;
using CCSWE.nanoFramework.Hosting;
using CCSWE.nanoFramework.MdnsServer;
using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.WebServer;
using Emily.Clock.Device;
using Emily.Clock.Device.Rtc;
using Emily.Clock.Events;
using Emily.Clock.UI.Navigation;

namespace Emily.Clock.Networking;

public class NetworkInitialization : IDeviceInitializer
{
    private readonly DeviceFeatures _deviceFeatures;
    private readonly IDeviceManager _deviceManager;
    private readonly IMediator _mediator;
    private readonly IMdnsServer _mdnsServer;
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebServer _webServer;
    private readonly IWirelessNetworkManager _wirelessNetworkManager;

    public NetworkInitialization(DeviceFeatures deviceFeatures, IDeviceManager deviceManager, IMediator mediator, IMdnsServer mdnsServer, INavigationService navigationService, IServiceProvider serviceProvider, IWebServer webServer, IWirelessNetworkManager wirelessNetworkManager)
    {
        _deviceFeatures = deviceFeatures;
        _deviceManager = deviceManager;
        _mediator = mediator;
        _mdnsServer = mdnsServer;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
        _webServer = webServer;
        _wirelessNetworkManager = wirelessNetworkManager;
    }

    public bool Initialize()
    {
        if (!_wirelessNetworkManager.Initialize())
        {
            if (!_deviceFeatures.HasValidTime)
            {
                _navigationService.Navigate(NavigationDestination.NetworkFailure);

                return false;
            }

            // Continue without network — RTC provided a valid time
            return true;
        }

        if (_wirelessNetworkManager.GetMode() == WirelessMode.AccessPoint)
        {
            _navigationService.Navigate(NavigationDestination.Configuration);
        }
        else
        {
            // WiFi client connected — write current time back to RTC so future
            // offline boots start with an accurate time (NTP sync happens during connect).
            _deviceFeatures.HasValidTime = true;
            WriteTimeToRtc();
        }

        _mediator.Publish(new StatusEvent(string.Empty));

        _mdnsServer.Hostname = $"emily-clock-{_deviceManager.SerialNumber}";
        _mdnsServer.IPAddress = IPAddress.Parse(_wirelessNetworkManager.IpAddress);
        _mdnsServer.Start();
        
        _webServer.Start();

        return _wirelessNetworkManager.GetMode() != WirelessMode.AccessPoint;
    }

    private void WriteTimeToRtc()
    {
        var rtcProvider = (IRtcProvider)_serviceProvider.GetService(typeof(IRtcProvider));

        if (rtcProvider is null || !rtcProvider.IsInitialized)
        {
            return;
        }

        rtcProvider.DateTime = DateTime.UtcNow;
    }
}
