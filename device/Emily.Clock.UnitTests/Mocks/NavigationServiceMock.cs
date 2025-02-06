using Emily.Clock.UI.Navigation;

namespace Emily.Clock.UnitTests.Mocks
{
    internal class NavigationServiceMock: INavigationService
    {
        public NavigationDestination Destination { get; set; }
     
        public void Navigate(NavigationDestination destination)
        {
            Destination = destination;
            NavigateCalled = true;
        }

        public bool NavigateCalled { get; set; }
    }
}
