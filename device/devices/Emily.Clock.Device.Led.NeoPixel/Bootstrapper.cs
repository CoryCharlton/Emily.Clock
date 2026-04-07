using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Emily.Clock.Device.Led.NeoPixel;

/// <summary>
/// Extension methods for registering NeoPixel LED services.
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Registers the NeoPixel LED manager with the host.
    /// </summary>
    public static IHostBuilder AddNeoPixelLeds(this IHostBuilder builder, NeoPixelLedOptions options)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(NeoPixelLedOptions), options);
            services.AddSingleton(typeof(ILedManager), typeof(NeoPixelLedManager));
        });

        return builder;
    }
}
