using System.Collections;
using Emily.Clock.UI.Navigation;
using Emily.Clock.UI.Windows;

namespace Emily.Clock.UnitTests.Mocks
{
    internal class WindowFactoryMock: IWindowFactory
    {
        private readonly Hashtable _windows = new();

        public IWindow Create(NavigationDestination destination)
        {
            var window = new WindowMock(destination);

            _windows[destination] = window;

            return window;
        }

        public WindowMock? Get(NavigationDestination destination)
        {
            return _windows[destination] as WindowMock;
        }
    }
}
