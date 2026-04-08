using Emily.Clock.UI.Navigation;
using Emily.Clock.Testing.Mocks;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Navigation;

[TestClass]
public class NavigationServiceTests
{
    [TestMethod]
    public void Navigate_starts_window()
    {
        var windowFactory = new WindowFactoryMock();

        var sut = new NavigationService(windowFactory);

        sut.Navigate(NavigationDestination.Clock);

        var window = windowFactory.Get(NavigationDestination.Clock);

        Assert.IsNotNull(window);

        Assert.IsTrue(window.StartCalled);
        Assert.IsFalse(window.StopCalled);
        Assert.IsFalse(window.DisposeCalled);
    }

    [TestMethod]
    public void Navigate_stops_window()
    {
        var windowFactory = new WindowFactoryMock();

        var sut = new NavigationService(windowFactory);

        sut.Navigate(NavigationDestination.Clock);

        var window = windowFactory.Get(NavigationDestination.Clock);

        Assert.IsNotNull(window);

        Assert.IsTrue(window.StartCalled);
        Assert.IsFalse(window.StopCalled);
        Assert.IsFalse(window.DisposeCalled);

        sut.Navigate(NavigationDestination.Configuration);

        Assert.IsTrue(window.StopCalled);
        Assert.IsTrue(window.DisposeCalled);
    }
}