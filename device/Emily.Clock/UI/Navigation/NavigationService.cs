using System.Diagnostics.CodeAnalysis;
using Emily.Clock.UI.Windows;

namespace Emily.Clock.UI.Navigation;

public interface INavigationService
{
    void Navigate(NavigationDestination destination);
}
    
public class NavigationService : INavigationService
{
    private readonly object _syncLock = new();
    private readonly IWindowFactory _windowFactory;

    public NavigationService(IWindowFactory windowFactory)
    {
        _windowFactory = windowFactory;
    }

    private IWindow? Current { get; set; }

    [MemberNotNull(nameof(Current))]
    public void Navigate(NavigationDestination destination)
    {
        SetCurrent(_windowFactory.Create(destination));
    }

    [MemberNotNull(nameof(Current))]
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