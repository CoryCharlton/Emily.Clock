using System.Device.Gpio;
using Emily.Clock.App.Hardware;
using Emily.Clock.Device;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.Led;
using Emily.Clock.Device.SdCard;
using Emily.Clock.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nanoFramework.Hardware.Esp32;
using nanoFramework.System.IO.FileSystem;
using Esp32 = nanoFramework.Hardware.Esp32.Configuration;

namespace Emily.Clock.App;

public static class Bootstrapper
{
    private const uint CHIP_SELECT_PIN = 13;
    private const int SDCARD_SLOT_INDEX = 0;
    private const uint SPI_BUS = 2;
    private const int SPI2_CLOCK_PIN = 14;
    private const int SPI2_MISO_PIN = 2;
    private const int SPI2_MOSI_PIN = 15;

    public static IHostBuilder ConfigureHardware(this IHostBuilder builder)
    {
        return builder
            .AddSdCard(new SDCardSpiParameters { slotIndex = SDCARD_SLOT_INDEX, spiBus = SPI_BUS, chipSelectPin = CHIP_SELECT_PIN }, gpio =>
            {
                Esp32.SetPinFunction(SPI2_MISO_PIN, DeviceFunction.SPI2_MISO);
                Esp32.SetPinFunction(SPI2_MOSI_PIN, DeviceFunction.SPI2_MOSI);
                Esp32.SetPinFunction(SPI2_CLOCK_PIN, DeviceFunction.SPI2_CLOCK);

                gpio.OpenPin(SPI2_MISO_PIN, PinMode.InputPullUp);
            })
            .ConfigureServices(services => services.ConfigureHardware());
    }

    private static IServiceCollection ConfigureHardware(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IButtonManager), typeof(ButtonManager))
            .AddSingleton(typeof(IDeviceManager), typeof(DeviceManager))
            .AddSingleton(typeof(IDisplayManager), typeof(DisplayManager))
            .AddSingleton(typeof(ILedManager), typeof(NeoPixelStripManager));

        return services;
    }
}
