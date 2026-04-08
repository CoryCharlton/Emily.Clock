using System;
using Emily.Clock.Configuration;
using Emily.Clock.Device;
using Emily.Clock.Events;
using Emily.Clock.Testing.Mocks;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests;

[TestClass]
public class AlarmServiceTests
{
    private static AlarmService CreateService(
        out AudioManagerMock audioManager,
        out ConfigurationManagerMock configurationManager,
        out MediatorMock mediator,
        bool enabled = false)
    {
        audioManager = new AudioManagerMock();
        configurationManager = new ConfigurationManagerMock();
        mediator = new MediatorMock();

        var config = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(7),
            Enabled = enabled,
            MaxDurationMinutes = 10
        };

        configurationManager.Save(AlarmConfiguration.Section, config);

        return new AlarmService(
            audioManager,
            configurationManager,
            new DeviceFeatures(),
            new LoggerMock(),
            mediator);
    }

    [TestMethod]
    public void HandleEvent_should_ignore_non_TimeChangedEvent()
    {
        using var service = CreateService(out _, out _, out _, enabled: true);
        service.Initialize();

        service.HandleEvent(new AlarmStateChangedEvent(false, false));

        Assert.IsFalse(service.IsAlarming);
    }

    [TestMethod]
    public void HandleEvent_should_do_nothing_when_disabled()
    {
        using var service = CreateService(out _, out _, out _, enabled: false);
        service.Initialize();

        var matchingTime = new DateTime(2024, 1, 1, 7, 0, 0);
        service.HandleEvent(new TimeChangedEvent(matchingTime));

        Assert.IsFalse(service.IsAlarming);
    }

    [TestMethod]
    public void HandleEvent_should_do_nothing_when_time_does_not_match()
    {
        using var service = CreateService(out _, out _, out _, enabled: true);
        service.Initialize();

        var nonMatchingTime = new DateTime(2024, 1, 1, 8, 0, 0);
        service.HandleEvent(new TimeChangedEvent(nonMatchingTime));

        Assert.IsFalse(service.IsAlarming);
    }

    [TestMethod]
    public void HandleEvent_should_start_alarm_when_time_matches()
    {
        using var service = CreateService(out _, out _, out var mediator, enabled: true);
        service.Initialize();

        var matchingTime = new DateTime(2024, 1, 1, 7, 0, 0);
        service.HandleEvent(new TimeChangedEvent(matchingTime));

        Assert.IsTrue(service.IsAlarming);
        Assert.IsTrue(mediator.WasEventPublished(typeof(AlarmStateChangedEvent)));
    }

    [TestMethod]
    public void Initialize_should_subscribe_to_TimeChangedEvent()
    {
        using var service = CreateService(out _, out _, out var mediator);

        service.Initialize();

        Assert.IsTrue(mediator.WasEventSubscribed(typeof(TimeChangedEvent)));
    }

    [TestMethod]
    public void OnConfigurationChanged_should_update_configuration()
    {
        using var service = CreateService(out _, out var configurationManager, out _);

        var newConfig = new AlarmConfiguration
        {
            AlarmTime = TimeSpan.FromHours(8),
            Enabled = true,
            MaxDurationMinutes = 15
        };

        configurationManager.Save(AlarmConfiguration.Section, newConfig);

        Assert.IsTrue(service.Enabled);
    }

    [TestMethod]
    public void StartAlarm_should_guard_against_double_start()
    {
        using var service = CreateService(out _, out _, out var mediator, enabled: true);
        service.Initialize();

        var matchingTime = new DateTime(2024, 1, 1, 7, 0, 0);
        service.HandleEvent(new TimeChangedEvent(matchingTime));
        service.HandleEvent(new TimeChangedEvent(matchingTime));

        Assert.AreEqual(1, mediator.GetPublishedCount(typeof(AlarmStateChangedEvent)));
    }

    [TestMethod]
    public void Toggle_should_enable_and_publish_event()
    {
        using var service = CreateService(out _, out _, out var mediator, enabled: false);

        service.Toggle();

        Assert.IsTrue(service.Enabled);
        Assert.IsTrue(mediator.WasEventPublished(typeof(AlarmStateChangedEvent)));
    }

    [TestMethod]
    public void Toggle_should_disable_and_publish_event()
    {
        using var service = CreateService(out _, out _, out var mediator, enabled: true);

        service.Toggle();

        Assert.IsFalse(service.Enabled);
        Assert.IsTrue(mediator.WasEventPublished(typeof(AlarmStateChangedEvent)));
    }

    [TestMethod]
    public void Toggle_should_stop_alarm_when_disabling_while_alarming()
    {
        using var service = CreateService(out _, out _, out _, enabled: true);
        service.Initialize();

        var matchingTime = new DateTime(2024, 1, 1, 7, 0, 0);
        service.HandleEvent(new TimeChangedEvent(matchingTime));

        Assert.IsTrue(service.IsAlarming);

        service.Toggle();

        Assert.IsFalse(service.Enabled);
    }
}
