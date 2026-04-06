using System;
using CCSWE.nanoFramework.Hosting;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Buttons;
using Emily.Clock.Device.Display;
using Emily.Clock.Device.FileStorage;
using Emily.Clock.Device.Led;
using Emily.Clock.Events;
using Emily.Clock.UI;
using Emily.Clock.UI.Navigation;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.Device;

public class DeviceInitialization : IDeviceInitializer
{
        private readonly IButtonManager _buttonManager;
        private readonly DeviceFeatures _deviceFeatures;
        private readonly IDisplayManager _displayManager;
        private readonly ILedManager _ledManager;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        public DeviceInitialization(IButtonManager buttonManager, DeviceFeatures deviceFeatures, IDisplayManager displayManager, ILedManager ledManager, ILogger logger, IMediator mediator, INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _buttonManager = buttonManager;
            _deviceFeatures = deviceFeatures;
            _displayManager = displayManager;
            _ledManager = ledManager;
            _logger = logger;
            _mediator = mediator;
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
        }

        public bool Initialize()
        {
            if (!InitializeDisplay())
            {
                _logger.LogError("Failed to initialize display");

                return false;
            }

            if (!InitializeButtons())
            {
                _logger.LogError("Failed to initialize buttons");

                return false;
            }

            // TODO: Initialize Audio provider (I2S or Piezo)

            // TODO: Initialize RTC and restore time if valid

            if (!InitializeFileStorage())
            {
                _logger.LogError("Failed to initialize file storage");
            }

            if (!_ledManager.Initialize())
            {
                _logger.LogError("Failed to initialize LEDs.");

                return false;
            }

            return true;
        }

        private bool InitializeButtons()
        {
            if (!_buttonManager.Initialize())
            {
                return false;
            }

            if (_buttonManager.IsPressed(Button.One) || _buttonManager.IsPressed(Button.Two) || _buttonManager.IsPressed(Button.Three))
            {
                _navigationService.Navigate(NavigationDestination.ResetToDefaults);

                return false;
            }

            return true;
        }

        private bool InitializeDisplay()
        {
            if (!_displayManager.Initialize())
            {
                return false;
            }

            var screen = _displayManager.GetBitmap();

            Controls.DrawTitle(screen, "Emily.Clock");
            Controls.DrawContent(screen, "Powered by nanoFramework", " ");
            // TODO: Fix for smaller text (pass font?)
            //Controls.DrawContent(screen, $"Powered by nanoFramework", $"https://github.com/CoryCharlton/Emily.Clock");

            Controls.DrawLogo(screen, Resources.BitmapResources.Loading_48);
            screen.Flush();

            PublishStatusEvent("Loading...");

            return true;
        }

        private bool InitializeFileStorage()
        {
            var fileStorageProvider = (IFileStorageProvider)_serviceProvider.GetService(typeof(IFileStorageProvider));

            if (fileStorageProvider is null)
            {
                return true;
            }

            PublishStatusEvent("Initializing file storage...");

            _deviceFeatures.HasFileStorage = fileStorageProvider.Initialize();

            PublishStatusEvent(_deviceFeatures.HasFileStorage ? "File storage initialized" : "Failed to initialize file storage");

            return _deviceFeatures.HasFileStorage;
        }

        private void PublishStatusEvent(string message)
        {
            _mediator.Publish(new StatusEvent(message));
        }
    }
