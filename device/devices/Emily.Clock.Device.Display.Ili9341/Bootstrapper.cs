using Emily.Clock.Device.Display;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Emily.Clock.Device.Display.Ili9341;

public static class Bootstrapper
{
    public static IHostBuilder AddIli9341Display(this IHostBuilder builder, SpiDisplayOptions options)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(SpiDisplayOptions), options);
            services.AddSingleton(typeof(IDisplayManager), typeof(Ili9341DisplayManager));
        });

        return builder;
    }
}
