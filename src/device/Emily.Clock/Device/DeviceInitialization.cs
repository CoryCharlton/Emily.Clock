using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.IO;
using Emily.Clock.Mediator.Events;
using Emily.Clock.UI;
using Emily.Clock.UI.Navigation;
using MakoIoT.Device.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.Device
{
    public class DeviceInitialization : IDeviceStartBehavior
    {
        private readonly IButtonManager _buttonManager;
        private readonly IDisplayManager _displayManager;
        private readonly IFileStorageProvider _fileStorageProvider;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly INavigationService _navigationService;
        private readonly INeoPixelManager _neoPixelManager;

        public DeviceInitialization(IButtonManager buttonManager, IDisplayManager displayManager, IFileStorageProvider fileStorageProvider, ILogger logger, IMediator mediator, INavigationService navigationService, INeoPixelManager neoPixelManager)
        {
            _buttonManager = buttonManager;
            _displayManager = displayManager;
            _fileStorageProvider = fileStorageProvider;
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

            // TODO: Initialize Audio provider (I2S or Piezo)

            // TODO: Initialize RTC and restore time if valid

            if (!InitializeFileStorage())
            {
                _logger.LogError("Failed to initialize file storage");
            }

            if (!_neoPixelManager.Initialize())
            {
                _logger.LogError("Failed to initialize led strip.");

                return false;
            }

            /*
            var internalFiles = Directory.GetFiles(@"I:\");
            foreach (var internalFile in internalFiles)
            {
                Debug.WriteLine($"Checking {internalFile}");

                if (internalFile.ToLower().EndsWith(".cfg") || internalFile.ToLower().EndsWith(".config"))
                {
                    Debug.WriteLine($"Deleting {internalFile}");
                    //File.Delete(internalFile);
                }
            }
            */

            return true;
        }

        private bool InitializeFileStorage()
        {
            PublishStatusEvent("Initializing file storage...");

            var fileStorageInitialized = _fileStorageProvider.Initialize();

            if (!fileStorageInitialized)
            {
                PublishStatusEvent("Failed to initialize file storage");
            }
            else
            {
                PublishStatusEvent("File storage initialized");

                var fileExists1 = _fileStorageProvider.FileExists(@"D:\Variation-CLJ013901.wav");
                var fileExists2 = _fileStorageProvider.FileExists(@"D:/Variation-CLJ013901.wav");
                var fileExists3 = _fileStorageProvider.FileExists(@"\Variation-CLJ013901.wav");
                var fileExists4 = _fileStorageProvider.FileExists(@"/Variation-CLJ013901.wav");
                var fileExists5 = _fileStorageProvider.FileExists(@"I:/Variation-CLJ013901.wav");

                _logger.LogDebug($"1: {fileExists1}, 2: {fileExists2}, 3: {fileExists3}, 4: {fileExists4}, 5: {fileExists5}");

                var directories = _fileStorageProvider.GetDirectories(@"D:\");
                foreach (var directory in directories)
                {
                    _logger.LogDebug($"Directory: {directory}");
                }

                var files = _fileStorageProvider.GetFiles(@"D:\");
                foreach (var file in files)
                {
                    _logger.LogDebug($"File: {file}");
                }
            }

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
