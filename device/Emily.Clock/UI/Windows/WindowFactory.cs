using System;
using Emily.Clock.UI.Navigation;
using nanoFramework.DependencyInjection;

namespace Emily.Clock.UI.Windows
{
    public interface IWindowFactory
    {
        IWindow Create(NavigationDestination destination);
    }

    public class WindowFactory : IWindowFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IWindow Create(NavigationDestination destination)
        {
            return destination switch
            {
                NavigationDestination.Clock => _serviceProvider.GetRequiredService(typeof(ClockWindow)),
                NavigationDestination.Configuration => _serviceProvider.GetRequiredService(typeof(ConfigurationWindow)),
                NavigationDestination.NetworkFailure => _serviceProvider.GetRequiredService(typeof(NetworkFailureWindow)),
                NavigationDestination.ResetToDefaults => _serviceProvider.GetRequiredService(typeof(ResetToDefaultsWindow)),
                _ => throw new ArgumentOutOfRangeException(nameof(destination))
            } as IWindow;
        }
    }
}
