using Emily.Clock.App.Hardware;
using Emily.Clock.Device;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.Led;
using Emily.Clock.IO;
using Emily.Clock.UI;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Hosting;

namespace Emily.Clock.App
{
    public static class Bootstrapper
    {
        public static IHostBuilder ConfigureHardware(this IHostBuilder builder)
        {
            return builder.ConfigureServices(services => services.ConfigureHardware());
        }

        private static IServiceCollection ConfigureHardware(this IServiceCollection services)
        {
            services
                .AddSingleton(typeof(IButtonManager), typeof(ButtonManager))
                .AddSingleton(typeof(IDeviceManager), typeof(DeviceManager))
                .AddSingleton(typeof(IDisplayManager), typeof(DisplayManager))
                .AddSingleton(typeof(IFileStorageProvider), typeof(FileStorageProvider))
                .AddSingleton(typeof(ILedManager), typeof(NeoPixelStripManager));

            return services;
        }
    }
}
