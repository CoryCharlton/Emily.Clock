using Emily.Clock.Device.Audio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Emily.Clock.Device.Audio.I2s;

/// <summary>
/// Extension methods for registering I2S audio services.
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Registers the I2S audio provider with the host.
    /// </summary>
    public static IHostBuilder AddI2sAudio(this IHostBuilder builder, I2sAudioOptions options)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(I2sAudioOptions), options);
            services.AddSingleton(typeof(IAudioProvider), typeof(I2sAudioProvider));
        });

        return builder;
    }
}
