using System;
using CCSWE.nanoFramework.Hosting;
using Emily.Clock.Device;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Navigation;
using Microsoft.Extensions.Logging;

namespace Emily.Clock
{
    public class ApplicationInitialization : IDeviceInitializer
    {
        private readonly IDeviceManager _deviceManager;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly INightLightManager _nightLightManager;

        public ApplicationInitialization(IDeviceManager deviceManager, ILocalTimeProvider localTimeProvider, ILogger logger, INavigationService navigationService, INightLightManager nightLightManager)
        {
            _deviceManager = deviceManager;
            _localTimeProvider = localTimeProvider;
            _logger = logger;
            _navigationService = navigationService;
            _nightLightManager = nightLightManager;
        }

        public bool Initialize()
        {
            _deviceManager.StartedAt = DateTime.UtcNow;
            _localTimeProvider.Start();

            if (!_nightLightManager.Initialize())
            {
                _logger.LogError("Failed to initialize lights");
                // TODO: Show screen to indicate light failure ??
                return false;
            }

            // TODO: Start alarm service

            _navigationService.Navigate(NavigationDestination.Clock);

            return true;
        }
    }
}
