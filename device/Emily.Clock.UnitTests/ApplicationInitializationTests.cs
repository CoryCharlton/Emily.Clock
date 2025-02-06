using Emily.Clock.UI.Navigation;
using Emily.Clock.UnitTests.Mocks;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests
{
    [TestClass]
    public class ApplicationInitializationTests
    {
        [TestMethod]
        public void DeviceStarting_should_handle_night_light_manager_failing_to_initialize()
        {
            // Arrange
            var deviceManager = new DeviceManagerMock();
            var localTimeProvider = new LocalTimeProviderMock();
            var logger = new LoggerMock();
            var navigationService = new NavigationServiceMock();
            var nightLightManager = new NightLightManagerMock() { InitializeResult = false };

            var sut = new ApplicationInitialization(deviceManager, localTimeProvider, logger, navigationService, nightLightManager);

            // Act
            var result = sut.Initialize();

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(navigationService.NavigateCalled);
        }

        [TestMethod]
        public void DeviceStarting_should_navigate_to_clock_on_success()
        {
            // Arrange
            var deviceManager = new DeviceManagerMock();
            var localTimeProvider = new LocalTimeProviderMock();
            var logger = new LoggerMock();
            var navigationService = new NavigationServiceMock();
            var nightLightManager = new NightLightManagerMock();

            var sut = new ApplicationInitialization(deviceManager, localTimeProvider, logger, navigationService, nightLightManager);

            // Act
            var result = sut.Initialize();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(navigationService.NavigateCalled);
            Assert.AreEqual(NavigationDestination.Clock, navigationService.Destination);
        }

        [TestMethod]
        public void DeviceStarting_should_set_application_start_time()
        {
            // Arrange
            var deviceManager = new DeviceManagerMock();
            var localTimeProvider = new LocalTimeProviderMock();
            var logger = new LoggerMock();
            var navigationService = new NavigationServiceMock();
            var nightLightManager = new NightLightManagerMock();

            var sut = new ApplicationInitialization(deviceManager, localTimeProvider, logger, navigationService, nightLightManager);

            // Act
            var result = sut.Initialize();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(deviceManager.StartedAtSet);
        }

        [TestMethod]
        public void DeviceStarting_should_start_time_provider()
        {
            // Arrange
            var deviceManager = new DeviceManagerMock();
            var localTimeProvider = new LocalTimeProviderMock();
            var logger = new LoggerMock();
            var navigationService = new NavigationServiceMock();
            var nightLightManager = new NightLightManagerMock();

            var sut = new ApplicationInitialization(deviceManager, localTimeProvider, logger, navigationService, nightLightManager);

            // Act
            var result = sut.Initialize();

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(localTimeProvider.StartCalled);
        }
    }
}
