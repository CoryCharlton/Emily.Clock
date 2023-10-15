using System;
using nanoFramework.UI;
using System.Device.Gpio;
using nanoFramework.Hardware.Esp32;
using nanoFramework.UI.GraphicDrivers;
using Emily.Clock.Device.Gpio;
using Emily.Clock.UI;

namespace Emily.Clock.App.Hardware
{
    public class DisplayManager: IDisplayManager
    {
        // ReSharper disable InconsistentNaming
        public const int SCREEN_MISO = 12;
        public const int SCREEN_MOSI = 23;
        public const int SCREEN_CLOCK = 18;
        public const int SCREEN_BACKLIGHT = 4;
        public const int SCREEN_CHIP_SELECT = 27;
        public const int SCREEN_DATA_COMMAND = 32;
        public const int SCREEN_RESET = 5;

        private const ushort _height = 240;
        private const ushort _width = 320;
        // ReSharper restore InconsistentNaming

        private readonly IGpioProvider _gpioProvider;
        private bool _initialized;

        public DisplayManager(IGpioProvider gpioProvider)
        {
            _gpioProvider = gpioProvider;
        }

        public ushort Height => _height;

        public bool IsInitialized => _initialized;

        public ushort Width => _width;

        public void Clear(bool flush = true)
        {
            RequireInitialization();

            var bitmap = GetBitmap();

            bitmap.Clear();

            if (flush)
            {
                bitmap.Flush();
            }
        }

        private static uint GetBufferSize()
        {
            return _height * _width * 16;
        }

        public Bitmap GetBitmap()
        {
            RequireInitialization();
            
            return DisplayControl.FullScreen;
        }

        public bool Initialize()
        {
            if (_initialized)
            {
                return true;
            }

            _initialized = true;
            _gpioProvider.OpenPin(SCREEN_BACKLIGHT, PinMode.Output);

            SetPinFunction(SCREEN_MISO, DeviceFunction.SPI1_MISO);
            SetPinFunction(SCREEN_MOSI, DeviceFunction.SPI1_MOSI);
            SetPinFunction(SCREEN_CLOCK, DeviceFunction.SPI1_CLOCK);

            var spiConfiguration = new SpiConfiguration(1, SCREEN_CHIP_SELECT, SCREEN_DATA_COMMAND, SCREEN_RESET, -1);
            var graphicDriver = Ili9341.GraphicDriverWithDefaultManufacturingSettings;
            var screenConfiguration = new ScreenConfiguration(0, 0, Width, Height, graphicDriver);

            DisplayControl.Initialize(spiConfiguration, screenConfiguration, GetBufferSize());
            DisplayControl.ChangeOrientation(DisplayOrientation.Landscape);

            Clear();
            SetBackLight(true);

            return true;
        }

        private void RequireInitialization()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException();
            }
        }

        public void SetBackLight(bool enabled)
        {
            _gpioProvider.Write(SCREEN_BACKLIGHT, enabled ? PinValue.High : PinValue.Low);
        }

        private static void SetPinFunction(int pin, DeviceFunction function)
        {
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(pin, function);
        }
    }
}
