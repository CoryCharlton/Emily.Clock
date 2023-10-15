using System;
using Emily.Clock.Device;
using Emily.Clock.Mediator.Events;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Navigation;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace Emily.Clock
{
    public class ApplicationService : IDeviceStartBehavior
    {
        private readonly IDeviceManager _deviceManager;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly INightLightManager _nightLightManager;

        public ApplicationService(IDeviceManager deviceManager, ILocalTimeProvider localTimeProvider, ILogger logger, INavigationService navigationService, INightLightManager nightLightManager)
        {
            _deviceManager = deviceManager;
            _localTimeProvider = localTimeProvider;
            _logger = logger;
            _navigationService = navigationService;
            _nightLightManager = nightLightManager;
        }

        public bool DeviceStarting()
        {
            _deviceManager.StartedAt = DateTime.UtcNow;
            _localTimeProvider.Initialize();

            if (!_nightLightManager.Initialize())
            {
                _logger.LogError("Failed to initialize lights");
                // TODO: Show screen to indicate light failure
                return false;
            }

            _navigationService.Navigate(NavigationDestination.Clock);

            // TODO: Start alarm service

            return true;
        }
    }
}
