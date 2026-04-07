using System;
using System.Drawing;
using System.IO;
using System.Threading;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Audio;
using Emily.Clock.Configuration;
using Emily.Clock.Device;
using Emily.Clock.Device.Audio;
using Emily.Clock.Device.Led;
using Emily.Clock.Events;
using Microsoft.Extensions.Logging;

namespace Emily.Clock;

public interface IAlarmService
{
    bool Enabled { get; }
    bool IsAlarming { get; }

    bool Initialize();
    void StartAlarm();
    void Stop();
    void Toggle();
}

public class AlarmService : IAlarmService, IMediatorEventHandler
{
    private static readonly Color AlarmColor = Color.Red;

    private Thread? _alarmThread;
    private readonly IAudioManager _audioManager;
    private readonly ManualResetEvent _cancelAlarm = new(false);
    private AlarmConfiguration _configuration;
    private readonly IConfigurationManager _configurationManager;
    private readonly DeviceFeatures _deviceFeatures;
    private bool _isAlarming;
    private readonly ILedManager _ledManager;
    private Thread? _ledThread;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public AlarmService(IAudioManager audioManager, IConfigurationManager configurationManager, DeviceFeatures deviceFeatures, ILedManager ledManager, ILogger logger, IMediator mediator)
    {
        _audioManager = audioManager;
        _configurationManager = configurationManager;
        _configurationManager.ConfigurationChanged += OnConfigurationChanged;
        _deviceFeatures = deviceFeatures;
        _ledManager = ledManager;
        _logger = logger;
        _mediator = mediator;

        _configuration = (AlarmConfiguration)_configurationManager.Get(AlarmConfiguration.Section);
    }

    public bool Enabled => _configuration.Enabled;

    public bool IsAlarming => _isAlarming;

    private void AlarmLoop()
    {
        var deadline = DateTime.UtcNow.AddMinutes(_configuration.MaxDurationMinutes);

        if (_deviceFeatures.HasAudio)
        {
            try
            {
                using var wavFile = new WavFile(new FileStream(@"D:\alarm.wav", FileMode.Open, FileAccess.Read));
                using var device = _audioManager.Prepare(wavFile);

                if (device is not null)
                {
                    while (!_cancelAlarm.WaitOne(0, false) && DateTime.UtcNow < deadline)
                    {
                        device.Play(_cancelAlarm);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to play alarm audio", exception);
            }
        }
        else
        {
            while (!_cancelAlarm.WaitOne(0, false) && DateTime.UtcNow < deadline)
            {
                _cancelAlarm.WaitOne(1000, false);
            }
        }

        // Signal the LED thread to stop (deadline reached or audio error)
        _cancelAlarm.Set();
    }

    public void HandleEvent(IMediatorEvent mediatorEvent)
    {
        if (mediatorEvent is not TimeChangedEvent timeChangedEvent)
        {
            return;
        }

        if (!Enabled || IsAlarming)
        {
            return;
        }

        if (timeChangedEvent.Time.Hour == _configuration.AlarmTime.Hours &&
            timeChangedEvent.Time.Minute == _configuration.AlarmTime.Minutes)
        {
            StartAlarm();
        }
    }

    public bool Initialize()
    {
        _mediator.Subscribe(typeof(TimeChangedEvent), this);
        return true;
    }

    private void LedLoop()
    {
        while (true)
        {
            _ledManager.SetNightlightLeds(AlarmColor, 1.0f);
            _ledManager.Update();

            if (_cancelAlarm.WaitOne(300, false)) break;

            _ledManager.SetNightlightLeds(Color.Black, 1.0f);
            _ledManager.Update();

            if (_cancelAlarm.WaitOne(300, false)) break;
        }

        StopInternal();
    }

    private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
    {
        if (e.Section != AlarmConfiguration.Section)
        {
            return;
        }

        if (e.Configuration is not AlarmConfiguration configuration)
        {
            return;
        }

        _configuration = configuration;
    }

    public void StartAlarm()
    {
        if (_isAlarming)
        {
            return;
        }

        _cancelAlarm.Reset();
        _isAlarming = true;
        _mediator.Publish(new AlarmStateChangedEvent(Enabled, true));

        _alarmThread = new Thread(AlarmLoop);
        _alarmThread.Start();

        _ledThread = new Thread(LedLoop);
        _ledThread.Start();
    }

    public void Stop()
    {
        _cancelAlarm.Set();
    }

    private void StopInternal()
    {
        _isAlarming = false;
        _ledManager.SetNightlightLeds(Color.Black, 1.0f);
        _ledManager.Update();
        _mediator.Publish(new AlarmStateChangedEvent(Enabled, false));
    }

    public void Toggle()
    {
        _configuration.Enabled = !_configuration.Enabled;
        _configurationManager.SaveAsync(AlarmConfiguration.Section, _configuration);

        if (!_configuration.Enabled && _isAlarming)
        {
            Stop();
            return;
        }

        _mediator.Publish(new AlarmStateChangedEvent(Enabled, IsAlarming));
    }
}

public static class AlarmServiceExtensions
{
    internal static Resources.BitmapResources GetEnabledBitmapId(this IAlarmService alarmService)
    {
        return alarmService.Enabled
            ? Resources.BitmapResources.Alarm_22
            : Resources.BitmapResources.Alarm_22_Outline;
    }
}
