
using Emily.Clock.App.Hardware;
using Emily.Clock.Device;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.IO;
using Emily.Clock.UI;
using MakoIoT.Device.Services.Interface;
using nanoFramework.DependencyInjection;

namespace Emily.Clock.App
{
    public static class Bootstrapper
    {
        public static IDeviceBuilder ConfigureDependencyInjection(this IDeviceBuilder builder)
        {
            return builder.ConfigureDI(services => services.ConfigureDependencyInjection());
        }

        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
        {
            services
                .AddSingleton(typeof(IButtonManager), typeof(ButtonManager))
                .AddSingleton(typeof(IDeviceManager), typeof(DeviceManager))
                .AddSingleton(typeof(IDisplayManager), typeof(DisplayManager))
                .AddSingleton(typeof(IFileStorageProvider), typeof(FileStorageProvider))
                .AddSingleton(typeof(INeoPixelManager), typeof(NeoPixelManager));

            return services;
        }
    }
}
