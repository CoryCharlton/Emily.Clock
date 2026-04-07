using System.Device.Gpio;
using Emily.Clock.App.Hardware;
using Emily.Clock.Device;
using Emily.Clock.Device.Buttons;
using Emily.Clock.Device.Display;
using Emily.Clock.Device.Display.Ili9341;
using Emily.Clock.Device.Led;
using Emily.Clock.Device.Audio.I2s;
using Emily.Clock.Device.FileStorage.SdCard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nanoFramework.Hardware.Esp32;
using nanoFramework.System.IO.FileSystem;
using nanoFramework.UI;
using Esp32 = nanoFramework.Hardware.Esp32.Configuration;

namespace Emily.Clock.App;

public static class Bootstrapper
{
    private const int BUTTON_ONE_PIN = 39;
    private const PinMode BUTTON_ONE_PIN_MODE = PinMode.InputPullUp;
    private const int BUTTON_TWO_PIN = 37;
    private const PinMode BUTTON_TWO_PIN_MODE = PinMode.InputPullUp;
    private const int BUTTON_THREE_PIN = 38;
    private const PinMode BUTTON_THREE_PIN_MODE = PinMode.InputPullUp;

    private const int DISPLAY_BACKLIGHT = 4;
    private const int DISPLAY_CHIP_SELECT = 27;
    private const int DISPLAY_CLOCK = 18;
    private const int DISPLAY_DATA_COMMAND = 32;
    private const int DISPLAY_SPI_BUS = 1;
    private const int DISPLAY_MISO = 12;
    private const int DISPLAY_MOSI = 23;
    private const int DISPLAY_RESET = 5;

    private const int AUDIO_BCK_PIN = 26;
    private const int AUDIO_BUS_ID = 1;
    private const int AUDIO_DATA_OUT_PIN = 33;
    private const int AUDIO_WS_PIN = 25;

    private const uint SDCARD_CHIP_SELECT_PIN = 13;
    private const int SDCARD_SLOT_INDEX = 0;
    private const uint SDCARD_SPI_BUS = 2;
    private const int SDCARD_SPI2_CLOCK_PIN = 14;
    private const int SDCARD_SPI2_MISO_PIN = 2;
    private const int SDCARD_SPI2_MOSI_PIN = 15;

    public static IHostBuilder ConfigureHardware(this IHostBuilder builder)
    {
        return builder
            .AddButtons(new ButtonOptions(
                new ButtonConfiguration { Pin = BUTTON_ONE_PIN, PinMode = BUTTON_ONE_PIN_MODE },
                new ButtonConfiguration { Pin = BUTTON_TWO_PIN, PinMode = BUTTON_TWO_PIN_MODE },
                new ButtonConfiguration { Pin = BUTTON_THREE_PIN, PinMode = BUTTON_THREE_PIN_MODE }
            ))
            .AddIli9341Display(new SpiDisplayOptions
            {
                Width = 320,
                Height = 240,
                Orientation = DisplayOrientation.Landscape,
                BacklightPin = DISPLAY_BACKLIGHT,
                SpiConfiguration = new SpiConfiguration(DISPLAY_SPI_BUS, DISPLAY_CHIP_SELECT, DISPLAY_DATA_COMMAND, DISPLAY_RESET, -1),
                PreInitialize = _ =>
                {
                    Esp32.SetPinFunction(DISPLAY_MISO, DeviceFunction.SPI1_MISO);
                    Esp32.SetPinFunction(DISPLAY_MOSI, DeviceFunction.SPI1_MOSI);
                    Esp32.SetPinFunction(DISPLAY_CLOCK, DeviceFunction.SPI1_CLOCK);
                }
            })
            .AddI2sAudio(new I2sAudioOptions
            {
                BusId = AUDIO_BUS_ID,
                PreInitialize = _ =>
                {
                    Esp32.SetPinFunction(AUDIO_BCK_PIN, DeviceFunction.I2S1_BCK);
                    Esp32.SetPinFunction(AUDIO_DATA_OUT_PIN, DeviceFunction.I2S1_DATA_OUT);
                    Esp32.SetPinFunction(AUDIO_WS_PIN, DeviceFunction.I2S1_WS);
                }
            })
            .AddSdCard(new SDCardSpiParameters { slotIndex = SDCARD_SLOT_INDEX, spiBus = SDCARD_SPI_BUS, chipSelectPin = SDCARD_CHIP_SELECT_PIN }, gpio =>
            {
                Esp32.SetPinFunction(SDCARD_SPI2_MISO_PIN, DeviceFunction.SPI2_MISO);
                Esp32.SetPinFunction(SDCARD_SPI2_MOSI_PIN, DeviceFunction.SPI2_MOSI);
                Esp32.SetPinFunction(SDCARD_SPI2_CLOCK_PIN, DeviceFunction.SPI2_CLOCK);

                gpio.OpenPin(SDCARD_SPI2_MISO_PIN, PinMode.InputPullUp);
            })
            .ConfigureServices(services => services.ConfigureHardware());
    }

    private static IServiceCollection ConfigureHardware(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IDeviceManager), typeof(DeviceManager))
            .AddSingleton(typeof(ILedManager), typeof(NeoPixelStripManager));

        return services;
    }
}
