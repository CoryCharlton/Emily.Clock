using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.FileStorage;
using CCSWE.nanoFramework.Hosting;
using CCSWE.nanoFramework.Logging;
using CCSWE.nanoFramework.MdnsServer;
using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.WebServer;
using Emily.Clock.Configuration;
using Emily.Clock.Device;
using Emily.Clock.Device.Audio;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Events;
using Emily.Clock.Networking;
using Emily.Clock.UI;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Navigation;
using Emily.Clock.UI.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using nanoFramework.Json;

namespace Emily.Clock;

public static class Bootstrapper
{
    public static IHostBuilder AddCore(this IHostBuilder builder)
    {
        JsonSerializerOptions.Default.PropertyNameCaseInsensitive = true;

        builder.ConfigureServices(services => services.AddCore());

        builder
            .AddLogging()
            .AddMdnsServer()
            .AddMediator()
            .AddWebServer();

        return builder;
    }

    private static IServiceCollection AddCore(this IServiceCollection services)
    {
        JsonSerializerOptions.Default.PropertyNameCaseInsensitive = true;

        services
            .AddFileStorage()
            .AddSingleton(typeof(DeviceFeatures), new DeviceFeatures());

        // These execute in order
        services
            .AddSingleton(typeof(IDeviceInitializer), typeof(DeviceInitialization))
            .AddSingleton(typeof(IDeviceInitializer), typeof(NetworkInitialization))
            .AddSingleton(typeof(IDeviceInitializer), typeof(ApplicationInitialization));

        services
            .AddSingleton(typeof(IAudioManager), typeof(AudioManager))
            .AddSingleton(typeof(IAlarmService), typeof(AlarmService))
            .AddSingleton(typeof(IGpioProvider), typeof(GpioProvider))
            .AddSingleton(typeof(ILocalTimeProvider), typeof(LocalTimeProvider))
            .AddSingleton(typeof(INavigationService), typeof(NavigationService))
            .AddSingleton(typeof(INetworkInterfaceProvider), typeof(NetworkInterfaceProvider))
            .AddSingleton(typeof(INightLightManager), typeof(NightNightLightManager))
            .AddSingleton(typeof(IStatusService), typeof(StatusService))
            .AddSingleton(typeof(IWindowFactory), typeof(WindowFactory))
            .AddSingleton(typeof(IWirelessAccessPointManager), typeof(WirelessAccessPointManager))
            .AddSingleton(typeof(IWirelessClientManager), typeof(WirelessClientManager))
            .AddSingleton(typeof(IWirelessNetworkManager), typeof(WirelessNetworkManager));

        services
            .AddTransient(typeof(ClockWindow))
            .AddTransient(typeof(ConfigurationWindow))
            .AddTransient(typeof(NetworkFailureWindow))
            .AddTransient(typeof(ResetToDefaultsWindow));

        services
            .AddConfigurationManager()
            .BindConfiguration(AlarmConfiguration.Section, AlarmConfiguration.Defaults, new AlarmConfigurationValidator())
            .BindConfiguration(DateTimeConfiguration.Section, DateTimeConfiguration.Defaults, new DateTimeConfigurationValidator())
            .BindConfiguration(NightLightConfiguration.Section, NightLightConfiguration.Defaults, new NightLightConfigurationValidator())
            .BindConfiguration(WirelessAccessPointConfiguration.Section, WirelessAccessPointConfiguration.Defaults, new WirelessAccessPointConfigurationValidator())
            .BindConfiguration(WirelessClientConfiguration.Section, WirelessClientConfiguration.Defaults, new WirelessClientConfigurationValidator());

        return services;
    }

    private static IHostBuilder AddLogging(this IHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddLogging(options =>
            {
#if DEBUG
                options.MinLogLevel = LogLevel.Trace;
#else
                options.MinLogLevel = LogLevel.Warning;
#endif
            });
        });

        return builder;
    }

    private static IHostBuilder AddMdnsServer(this IHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddMdnsServer(options =>
            {
                options.AddService(new MdnsServiceRegistration("_http._tcp.local", 80, "path=/"));
                options.AddService(new MdnsServiceRegistration("_emily-clock._tcp.local", 80, "path=/"));
            });
        });
    }
    
    private static IHostBuilder AddMediator(this IHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddMediator(options =>
            {
                options.AddSubscriber(typeof(StatusEvent), typeof(IStatusService));

                options.LogLevel = LogLevel.Debug;
            });
        });
    }

    private static IHostBuilder AddWebServer(this IHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddCors();
            //services.AddStaticFiles(typeof(FileProvider));

            services.AddControllers(typeof(Bootstrapper).Assembly);

            services.AddWebServer(options =>
            {
                options.Port = 80;
                options.Protocol = HttpProtocol.Http;
            });
        });
    }
}