using Emily.Clock.Device.Rtc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Emily.Clock.Device.Rtc.Ds3231;

/// <summary>
/// Extension methods for registering DS3231 RTC services.
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Registers the DS3231 RTC provider with the host.
    /// </summary>
    public static IHostBuilder AddDs3231Rtc(this IHostBuilder builder, Ds3231RtcOptions options)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(Ds3231RtcOptions), options);
            services.AddSingleton(typeof(IRtcProvider), typeof(Ds3231RtcProvider));
        });

        return builder;
    }
}
