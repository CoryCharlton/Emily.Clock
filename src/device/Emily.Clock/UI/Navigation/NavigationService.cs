using Emily.Clock.UI.Windows;

namespace Emily.Clock.UI.Navigation
{
    public interface INavigationService
    {
        void Navigate(NavigationDestination destination);
    }
    
    public class NavigationService : INavigationService
    {
        private readonly object _syncLock = new();
        private readonly IStatusService _statusService;
        private readonly IWindowFactory _windowFactory;

        public NavigationService(IStatusService statusService, IWindowFactory windowFactory)
        {
            _statusService = statusService;
            _windowFactory = windowFactory;
        }

        private IWindow Current { get; set; }

        public void Navigate(NavigationDestination destination)
        {
            /*
            if (Current is null)
            {
                // TODO: Make the specific windows handle this.
                _statusService.SuppressEvents = true;
            }
            */

            SetCurrent(_windowFactory.Create(destination));
        }

        private void SetCurrent(IWindow window)
        {
            lock (_syncLock)
            {
                if (Current is not null)
                {
                    Current.Stop();
                    Current.Dispose();
                }

                Current = window;
                Current.Start();
            }
        }
    }
}
