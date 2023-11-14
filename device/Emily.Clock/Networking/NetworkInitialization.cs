using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.WebServer;
using Emily.Clock.Mediator.Events;
using Emily.Clock.UI.Navigation;
using MakoIoT.Device.Services.Interface;

namespace Emily.Clock.Networking
{
    public class NetworkInitialization : IDeviceStartBehavior
    {
        private readonly IMediator _mediator;
        private readonly INavigationService _navigationService;
        private readonly IWebServer _webServer;
        private readonly IWirelessNetworkManager _wirelessNetworkManager;

        public NetworkInitialization(IMediator mediator, INavigationService navigationService, IWebServer webServer, IWirelessNetworkManager wirelessNetworkManager)
        {
            _mediator = mediator;
            _navigationService = navigationService;
            _webServer = webServer;
            _wirelessNetworkManager = wirelessNetworkManager;
        }

        public bool DeviceStarting()
        {
            if (!_wirelessNetworkManager.Initialize())
            {
                _navigationService.Navigate(NavigationDestination.NetworkFailure);

                return false;
            }

            if (_wirelessNetworkManager.GetMode() == WirelessMode.AccessPoint)
            {
                _navigationService.Navigate(NavigationDestination.Configuration);
            }

            _mediator.Publish(new StatusEvent(string.Empty));
            
            _webServer.Initialize();
            _webServer.Start();

            return _wirelessNetworkManager.GetMode() != WirelessMode.AccessPoint;
        }
    }
}
