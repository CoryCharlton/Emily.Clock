using CCSWE.nanoFramework.Hosting;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.Led;
using Emily.Clock.IO;
using Emily.Clock.Mediator.Events;
using Emily.Clock.UI;
using Emily.Clock.UI.Navigation;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.Device
{
    public class DeviceInitialization : IDeviceInitializer
    {
        private readonly IButtonManager _buttonManager;
        private readonly IDisplayManager _displayManager;
        private readonly IFileStorageManager _fileStorageManager;
        private readonly ILedManager _ledManager;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly INavigationService _navigationService;

        public DeviceInitialization(IButtonManager buttonManager, IDisplayManager displayManager, IFileStorageManager fileStorageManager, ILedManager ledManager, ILogger logger, IMediator mediator, INavigationService navigationService)
        {
            _buttonManager = buttonManager;
            _displayManager = displayManager;
            _fileStorageManager = fileStorageManager;
            _ledManager = ledManager;
            _logger = logger;
            _mediator = mediator;
            _navigationService = navigationService;
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

                return false;
            }

            if (!_ledManager.Initialize())
            {
                _logger.LogError("Failed to initialize LEDs.");

                return false;
            }

            return true;
        }

        private bool InitializeFileStorage()
        {
            PublishStatusEvent("Initializing file storage...");

            var fileStorageInitialized = _fileStorageManager.Initialize();

            PublishStatusEvent(fileStorageInitialized ? "File storage initialized" : "Failed to initialize file storage");

            return fileStorageInitialized;
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
            Controls.DrawContent(screen, $"Powered by nanoFramework", $" ");
            // TODO: Fix for smaller text (pass font?)
            //Controls.DrawContent(screen, $"Powered by nanoFramework", $"https://github.com/CoryCharlton/Emily.Clock");

            Controls.DrawLogo(screen, Resources.BitmapResources.Loading_48);
            screen.Flush();

            PublishStatusEvent("Loading...");

            return true;
        }

        private void PublishStatusEvent(string message)
        {
            _mediator.Publish(new StatusEvent(message));
        }
    }
}
