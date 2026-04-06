using System.Threading;
using Emily.Clock.Configuration;
using Emily.Clock.Events;
using Emily.Clock.UnitTests.Mocks;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests
{
    [TestClass]
    public class LocalTimeProviderTests
    {
        [TestMethod]
        public void DateTimeConfiguration_update_generates_events()
        {
            var configurationManager = new ConfigurationManagerMock();

            configurationManager.Save(DateTimeConfiguration.Section, new DateTimeConfiguration());

            var logger = new LoggerMock();
            var mediator = new MediatorMock();

            using var sut = new LocalTimeProvider(configurationManager, logger, mediator);

            sut.Start();

            Thread.Sleep(100);

            Assert.IsTrue(mediator.WasEventPublished(typeof(DateChangedEvent)));
            Assert.IsTrue(mediator.WasEventPublished(typeof(TimeChangedEvent)));

            var dateChangedEventCount = mediator.GetPublishedCount(typeof(DateChangedEvent));
            var timeChangedEventCount = mediator.GetPublishedCount(typeof(TimeChangedEvent));

            configurationManager.Save(DateTimeConfiguration.Section, new DateTimeConfiguration());
            configurationManager.Save(DateTimeConfiguration.Section, new DateTimeConfiguration());

            Thread.Sleep(100);

            Assert.IsTrue(mediator.GetPublishedCount(typeof(DateChangedEvent)) >= dateChangedEventCount + 2);
            Assert.IsTrue(mediator.GetPublishedCount(typeof(TimeChangedEvent)) >= timeChangedEventCount + 2);
        }

        [TestMethod]
        public void Start_generates_events()
        {
            var configurationManager = new ConfigurationManagerMock();

            configurationManager.Save(DateTimeConfiguration.Section, new DateTimeConfiguration());

            var logger = new LoggerMock();
            var mediator = new MediatorMock();

            using var sut = new LocalTimeProvider(configurationManager, logger, mediator);

            sut.Start();

            Thread.Sleep(100);

            Assert.IsTrue(mediator.WasEventPublished(typeof(DateChangedEvent)));
            Assert.IsTrue(mediator.WasEventPublished(typeof(TimeChangedEvent)));
        }
    }
}
