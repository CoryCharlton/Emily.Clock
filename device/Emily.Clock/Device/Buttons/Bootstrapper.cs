using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Emily.Clock.Device.Buttons;

public static class Bootstrapper
{
    public static IHostBuilder AddButtons(this IHostBuilder builder, ButtonOptions options)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(typeof(ButtonOptions), options);
            services.AddSingleton(typeof(IButtonManager), typeof(ButtonManager));
        });

        return builder;
    }
}
