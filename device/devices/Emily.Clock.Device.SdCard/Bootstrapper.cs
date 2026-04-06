using Emily.Clock.Device;
using Emily.Clock.Device.FileStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nanoFramework.System.IO.FileSystem;

namespace Emily.Clock.Device.SdCard;

public static class Bootstrapper
{
    public static IHostBuilder AddSdCard(this IHostBuilder builder, SDCardSpiParameters spiParameters, DevicePreInitializeDelegate? preInitialize = null)
    {
        var options = new SdCardOptions { SpiParameters = spiParameters, PreInitialize = preInitialize };

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(SdCardOptions), options);
            services.AddSingleton(typeof(IFileStorageProvider), typeof(SdCardFileStorageProvider));
        });

        return builder;
    }

    public static IHostBuilder AddSdCard(this IHostBuilder builder, SDCardMmcParameters mmcParameters, DevicePreInitializeDelegate? preInitialize = null)
    {
        var options = new SdCardOptions { MmcParameters = mmcParameters, PreInitialize = preInitialize };

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(SdCardOptions), options);
            services.AddSingleton(typeof(IFileStorageProvider), typeof(SdCardFileStorageProvider));
        });

        return builder;
    }
}
