using MakoIoT.Device.Utilities.TimeZones;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.Threading;
using Emily.Clock.Configuration;
using Emily.Clock.Mediator.Events;

namespace Emily.Clock
{
    public interface ILocalTimeProvider
    {
        bool IsBedTime { get; }
        DateTime Now { get; }
        DateTime UtcNow { get; }

        void Start();
    }

    public sealed class LocalTimeProvider : ILocalTimeProvider, IDisposable
    {
        private TimeSpan _bedTime;
        private readonly ManualResetEvent _cancellationRequested = new(false);
        private readonly IConfigurationManager _configurationManager;
        private readonly AutoResetEvent _generateEvents = new(false);
        private Thread? _generateEventsThread;
        private bool _started;
        private DateTime _lastDate = DateTime.MinValue;
        private DateTime _lastTime = DateTime.MinValue;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly object _syncLock = new ();
        private TimeZone? _timeZone;
        private TimeSpan _wakeTime;

        public bool IsBedTime => Now.TimeOfDay >= _bedTime || Now.TimeOfDay <= _wakeTime;

        // ReSharper disable once MergeConditionalExpression
        public DateTime Now => _timeZone is not null ? _timeZone.GetLocalTime(UtcNow) : UtcNow;
        public DateTime UtcNow => DateTime.UtcNow;

        public LocalTimeProvider(IConfigurationManager configurationManager, ILogger logger, IMediator mediator)
        {
            _configurationManager = configurationManager;
            _configurationManager.ConfigurationChanged += OnConfigurationChanged;
            _logger = logger;
            _mediator = mediator;

            UpdateConfiguration((DateTimeConfiguration) _configurationManager.Get(DateTimeConfiguration.Section));
        }

        ~LocalTimeProvider()
        {
            Dispose();
        }

        public void Dispose()
        {
            _cancellationRequested.Set();
        }

        private void GenerateEventsAsync()
        {
            var generateEvents = _generateEvents.WaitOne(0, false);

            while (!_cancellationRequested.WaitOne(0, false))
            {
                var currentDateTime = Now;

                var currentDate = currentDateTime.Date;
                if (currentDate != _lastDate || generateEvents)
                {
                    _lastDate = currentDate;
                    _mediator.Publish(new DateChangedEvent(currentDate));
                }

                var currentTime = currentDateTime.Time();
                if (currentTime != _lastTime || generateEvents)
                {
                    _lastTime = currentTime;
                    _mediator.Publish(new TimeChangedEvent(currentTime));
                }

                // Set the timeout to the next minute change
                var millisecondsTimeout = (59 - currentDateTime.Second) * 1000 + (1000 - currentDateTime.Millisecond);

                // ReSharper disable once SimplifyStringInterpolation
                _logger.LogDebug($"{nameof(LocalTimeProvider)} will update in {millisecondsTimeout} - {currentDateTime.ToString("O")}");

                generateEvents = WaitHandles.WaitAny(millisecondsTimeout, false, _generateEvents, _cancellationRequested) == 0;
            }
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Section != DateTimeConfiguration.Section)
            {
                return;
            }

            if (e.Configuration is not DateTimeConfiguration configuration)
            {
                return;
            }

            UpdateConfiguration(configuration);
        }

        public void Start()
        {
            // ReSharper disable once InvertIf
            if (!_started)
            {
                lock (_syncLock)
                {
                    if (_started)
                    {
                        return;
                    }

                    _started = true;

                    _generateEventsThread = new Thread(GenerateEventsAsync);
                    _generateEventsThread.Start();
                }
            }
        }

        private void UpdateConfiguration(DateTimeConfiguration configuration)
        {
            _bedTime = configuration.BedTime;
            _wakeTime = configuration.WakeTime;

            try
            {
                _timeZone = TimeZoneConverter.FromPosixString(configuration.TimeZone);
            }
            catch (Exception exception)
            {
                _logger.LogError("Invalid timezone configuration", exception);
            }

            _generateEvents.Set();
        }
    }
}
