using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.FileStorage;
using CCSWE.nanoFramework.Hosting;
using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.WebServer;
using CCSWE.nanoFramework.WebServer.Evaluate;
using Emily.Clock.Configuration;
using Emily.Clock.Controllers;
using Emily.Clock.Device;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Logging;
using Emily.Clock.Mediator.Events;
using Emily.Clock.Networking;
using Emily.Clock.UI;
using Emily.Clock.UI.Lights;
using Emily.Clock.UI.Navigation;
using Emily.Clock.UI.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using nanoFramework.Hosting;
using nanoFramework.Json;

namespace Emily.Clock
{
    public static class Bootstrapper
    {
        public static IHostBuilder AddCore(this IHostBuilder builder)
        {
            JsonSerializerOptions.Default.PropertyNameCaseInsensitive = true;

            builder.ConfigureServices(services => services.AddCore());

            builder
                .AddLogging()
                .AddMediator()
                .AddWebServer();

            return builder;
        }

        private static IServiceCollection AddCore(this IServiceCollection services)
        {
            services
                .AddFileStorage();

            // These execute in order
            services
                .AddSingleton(typeof(IDeviceInitializer), typeof(DeviceInitialization))
                .AddSingleton(typeof(IDeviceInitializer), typeof(NetworkInitialization))
                .AddSingleton(typeof(IDeviceInitializer), typeof(ApplicationInitialization));

            services
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
                .BindConfiguration(DateTimeConfiguration.Section, new DateTimeConfiguration(), new DateTimeConfigurationValidator())
                .BindConfiguration(NightLightConfiguration.Section, new NightLightConfiguration())
                .BindConfiguration(WirelessAccessPointConfiguration.Section, new WirelessAccessPointConfiguration())
                .BindConfiguration(WirelessClientConfiguration.Section, new WirelessClientConfiguration());

            return services;
        }

        private static IHostBuilder AddLogging(this IHostBuilder builder)
        {
#if DEBUG
            var loggerConfig = new LoggerOptions(LogLevel.Trace);
#else
            var loggerConfig = new LoggerOptions(LogLevel.Warning);
#endif

            builder.ConfigureServices(services =>
            {
                services.AddSingleton(typeof(ILogger), typeof(DebugLogger));
                services.AddSingleton(typeof(LoggerOptions), loggerConfig);
            });

            LoggerFormatter.Initialize();

            return builder;
        }

        private static IHostBuilder AddMediator(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddMediator(options =>
                {
                    options.AddSubscriber(typeof(StatusEvent), typeof(IStatusService));

                    options.LogLevel = LogLevel.Debug;
                });
            });

            return builder;
        }

        private static IHostBuilder AddWebServer(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddWebServer(options =>
                {
                    options.Port = 80;
                    options.Protocol = HttpProtocol.Http;

                    options.AddController(typeof(ConfigurationController));
                    options.AddController(typeof(DeviceController));
                    options.AddController(typeof(StaticContentController));
                });
            });

            return builder;
        }
    }
}