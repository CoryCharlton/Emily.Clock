using System.Collections;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.FileStorage;
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
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.Extensions;
using MakoIoT.Device.Services.Server.WebServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Emily.Clock
{
    public static class Bootstrapper
    {
        public static IDeviceBuilder AddCore(this IDeviceBuilder builder)
        {
            nanoFramework.Json.Configuration.Settings.CaseSensitive = false;

            builder.Services.AddCore();

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

            services
                .AddConfigurationManager(options => { options.LogLevel = LogLevel.Debug; })
                .BindConfiguration(DateTimeConfiguration.Section, new DateTimeConfiguration(), new DateTimeConfigurationValidator())
                .BindConfiguration(NightLightConfiguration.Section, new NightLightConfiguration())
                .BindConfiguration(WirelessAccessPointConfiguration.Section, new WirelessAccessPointConfiguration())
                .BindConfiguration(WirelessClientConfiguration.Section, new WirelessClientConfiguration());

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
            builder.Services.AddSingleton(typeof(LoggerOptions), loggerConfig);

            LoggerFormatter.Initialize();

            return builder;
        }

        private static IDeviceBuilder AddMediator(this IDeviceBuilder builder)
        {
            builder.Services.AddMediator(options =>
            {
                options.AddSubscriber(typeof(StatusEvent), typeof(IStatusService));

                options.LogLevel = LogLevel.Debug;
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

    public class TestConfiguration
    {
        public const string Name = nameof(TestConfiguration);

        public ArrayList Configurations { get; set; } = new ArrayList();

        public static TestConfiguration Create()
        {
            var test = new TestConfiguration();

            for (var i = 0; i < 10; i++)
            {
                var list = new ArrayList();
                
                for (var j = 0; j < 10; j++)
                {
                    list.Add(j);
                }

                test.Configurations.Add(list);
            }

            return test;
        }
    }
}