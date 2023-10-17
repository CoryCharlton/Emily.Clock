using CCSWE.nanoFramework.Mediator;
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
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.Extensions;
using MakoIoT.Device.Services.Server.WebServer;
using Microsoft.Extensions.Logging;
using nanoFramework.DependencyInjection;

namespace Emily.Clock
{
    public static class Bootstrapper
    {
        private static IDeviceBuilder AddConfigurations(this IDeviceBuilder builder)
        {
            builder.AddConfiguration(service =>
            {
                // TODO: Make sure these are not overwriting
                service.WriteDefault(DateTimeConfiguration.SectionName, DateTimeConfiguration.Default);
                service.WriteDefault(NightLightConfiguration.SectionName, NightLightConfiguration.Default);
                service.WriteDefault(WirelessAccessPointConfiguration.SectionName, WirelessAccessPointConfiguration.Default);
                service.WriteDefault(WirelessClientConfiguration.SectionName, WirelessClientConfiguration.Default);
            });

            builder.Services.AddSingleton(typeof(IConfigurationTypeFactory), typeof(ConfigurationTypeFactory));
            
            return builder;
        }

        public static IDeviceBuilder AddCore(this IDeviceBuilder builder)
        {
            nanoFramework.Json.Configuration.Settings.CaseSensitive = false;

            builder.Services.AddCore();

            builder
                .AddConfigurations()
                .AddFileStorage()
                .AddLogging()
                .AddMediator()
                .AddWebServer();

            return builder;
        }

        private static IServiceCollection AddCore(this IServiceCollection services)
        {
            // These execute in order
            services
                .AddSingleton(typeof(IDeviceStartBehavior), typeof(DeviceInitialization))
                .AddSingleton(typeof(IDeviceStartBehavior), typeof(NetworkInitialization))
                .AddSingleton(typeof(IDeviceStartBehavior), typeof(ApplicationInitialization));

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

            return services;
        }

        private static IDeviceBuilder AddLogging(this IDeviceBuilder builder)
        {
#if DEBUG
            var loggerConfig = new LoggerOptions(LogLevel.Debug);
#else
            var loggerConfig = new LoggerOptions(LogLevel.Warning);
#endif

            builder.Services.AddSingleton(typeof(ILogger), typeof(DebugLogger));
            //builder.Services.AddSingleton(typeof(ILoggerFactory), typeof(DebugLoggerFactory));
            builder.Services.AddSingleton(typeof(LoggerOptions), loggerConfig);

            LoggerFormatter.Initialize();

            return builder;
        }

        private static IDeviceBuilder AddMediator(this IDeviceBuilder builder)
        {
            builder.Services.AddMediator(options =>
            {
                options.AddSubscriber(typeof(StatusEvent), typeof(IStatusService));
            });

            return builder;
        }

        private static IDeviceBuilder AddWebServer(this IDeviceBuilder builder)
        {
            builder.AddWebServer(options =>
            {
                options.Port = 80;
                options.Protocol = HttpProtocol.Http;

                options.AddController(typeof(ConfigurationController));
                options.AddController(typeof(DeviceController));
                options.AddController(typeof(StaticContentController));
            });

            return builder;
        }
    }
}