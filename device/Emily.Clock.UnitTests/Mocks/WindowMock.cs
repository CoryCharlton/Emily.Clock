using Emily.Clock.UI.Navigation;
using Emily.Clock.UI.Windows;

namespace Emily.Clock.UnitTests.Mocks
{
    internal class WindowMock: IWindow
    {
        public WindowMock(NavigationDestination destination)
        {
            Destination = destination;
        }

        public NavigationDestination Destination { get; }

        public void Dispose()
        {
            DisposeCalled = true;
        }

        public bool DisposeCalled { get; private set; }

        public void Start()
        {
            StartCalled = true;
        }

        public bool StartCalled { get; private set; }

        public void Stop()
        {
            StopCalled = true;
        }

        public bool StopCalled { get; set; }
    }
}
