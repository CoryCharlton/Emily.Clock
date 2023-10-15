using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.Mediator.Events;
using Emily.Clock.UI;
using Emily.Clock.UI.Navigation;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.Device
{
    public class DeviceInitialization : IDeviceStartBehavior
    {
        private readonly IButtonManager _buttonManager;
        private readonly IDisplayManager _displayManager;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly INavigationService _navigationService;
        private readonly INeoPixelManager _neoPixelManager;

        public DeviceInitialization(IButtonManager buttonManager, IDisplayManager displayManager, ILogger logger, IMediator mediator, INavigationService navigationService, INeoPixelManager neoPixelManager)
        {
            _buttonManager = buttonManager;
            _displayManager = displayManager;
            _logger = logger;
            _mediator = mediator;
            _navigationService = navigationService;
            _neoPixelManager = neoPixelManager;
        }

        public bool DeviceStarting()
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

            if (!_neoPixelManager.Initialize())
            {
                _logger.LogError("Failed to initialize led strip.");

                return false;
            }

            // TODO: Initialize SD card

            /*
            var screen = _displayManager.GetBitmap();
            screen.Fill(Color.DeepPink);
            screen.Flush();

            Thread.Sleep(Timeout.Infinite);
            */

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
            Controls.DrawContent(screen, $"Powered by nanoFramework", $" ");
            // TODO: Fix for smaller text (pass font?)
            //Drawing.DrawContent(screen, $"Powered by nanoFramework", $"https://github.com/CoryCharlton/Emily.Clock");

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
