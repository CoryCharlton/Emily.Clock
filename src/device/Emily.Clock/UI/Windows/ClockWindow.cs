using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Drawing;
using nanoFramework.UI;
using Emily.Clock.UI.Lights;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Mediator.Events;
using System.Threading;
using CCSWE.nanoFramework.Mediator;

#nullable enable
namespace Emily.Clock.UI.Windows
{
    public class ClockWindow: Window, IMediatorEventHandler
    {
        private readonly IAlarmService _alarmService;
        private DateTime _clearProgressAt = DateTime.MinValue;
        private Thread? _clearProgressThread;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly INightLightManager _nightLightManager;
        private readonly IMediator _mediator;

        public ClockWindow(IAlarmService alarmService, IDisplayManager displayManager, ILocalTimeProvider localTimeProvider, ILogger logger, IMediator mediator, INightLightManager nightLightManager) : base(displayManager, logger)
        {
            _alarmService = alarmService;
            _localTimeProvider = localTimeProvider;
            _nightLightManager = nightLightManager;
            _mediator = mediator;

            Logger.LogDebug($"{nameof(ClockWindow)}.ctor");
        }

        private Padding DatePadding => Theme.ControlPadding;
        private Rectangle DateRectangle { get; set; } = Rectangle.Empty;
        private Rectangle ProgressRectangle { get; set; }
        private Padding TimePadding { get; } = new(5, 10);
        private Rectangle TimeRectangle { get; set; } = Rectangle.Empty;

        private void ClearProgressAsync()
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                if (_localTimeProvider.UtcNow >= _clearProgressAt)
                {
                    var screen = GetBitmap();
                    screen.Clear(ProgressRectangle);
                    screen.Flush();

                    _clearProgressAt = DateTime.MinValue;
                }

                CancellationToken.WaitHandle.WaitOne(1000, false);
            }
        }

        private void DrawButtons(Bitmap screen, bool flush = false)
        {
            Controls.PerformDrawingAndFlush(screen, flush, () =>
            {
                Controls.DrawButton(screen, Button.One, _nightLightManager.GetEnabledBitmapId());
                Controls.DrawButton(screen, Button.Two, Resources.BitmapResources.Palette_22);
                Controls.DrawButton(screen, Button.Three, _alarmService.GetEnabledBitmapId());
            });
        }

        private void DrawDate(Bitmap screen, DateTime date, bool flush = false)
        {
            Controls.PerformDrawingAndFlush(screen, flush, () =>
            {
                screen.Clear(DateRectangle);

                using var source = BitmapFactory.Create(date.ToString("dddd, MMMM dd"), Theme.SmallFont, Theme.SecondaryText, ContentAlignment.MiddleLeft, DatePadding);
                DateRectangle = screen.DrawImage(source, ContentAlignment.TopLeft);
            });
        }

        private void DrawProgressBar(Bitmap screen, bool flush = false)
        {
            Debug.WriteLine($"{DateTime.UtcNow.TimeOfDay} Drawing progress bar");

            _clearProgressAt = _localTimeProvider.UtcNow.AddSeconds(2);

            Controls.PerformDrawingAndFlush(screen, flush, () =>
            {
                ProgressRectangle = Controls.DrawProgressBar(screen, _nightLightManager.Brightness, flush);
            });

            Debug.WriteLine($"{DateTime.UtcNow.TimeOfDay} Drawing progress bar - Completed");
        }

        private void DrawTime(Bitmap screen, DateTime time, bool flush = false)
        {
            Controls.PerformDrawingAndFlush(screen, flush, () =>
            {
                screen.Clear(TimeRectangle);

                using var source = BitmapFactory.Create(time.ToString("h:mm"), Theme.ClockFont, Theme.PrimaryText, ContentAlignment.MiddleCenter, TimePadding);
                TimeRectangle = screen.DrawImage(source, ContentAlignment.MiddleCenter);
            });
        }

        public void HandleEvent(IMediatorEvent mediatorEvent)
        {
            switch (mediatorEvent)
            {
                case ButtonEvent buttonEvent:
                {
                    Logger.LogDebug(buttonEvent.ToString());

                    switch (buttonEvent.Button)
                    {
                        case Button.One:
                            switch (buttonEvent.Type)
                            {
                                case ButtonEventType.Holding:
                                    _nightLightManager.Toggle();
                                    break;
                                case ButtonEventType.Press:
                                    _nightLightManager.CycleBrightness();
                                    DrawProgressBar(GetBitmap(), true);
                                    break;
                            }
                            break;
                        case Button.Two:
                            if (ButtonEventType.Press == buttonEvent.Type)
                            {
                                _nightLightManager.CycleColor();
                            }
                            break;
                        case Button.Three:
                            if (ButtonEventType.Press == buttonEvent.Type)
                            {
                                _alarmService.Enabled = !_alarmService.Enabled;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    DrawButtons(GetBitmap(), true);

                    break;
                }
                case DateChangedEvent dateChangedEvent:
                {
                    DrawDate(GetBitmap(), dateChangedEvent.Date, true);
                    break;
                }
                case TimeChangedEvent timeChangedEvent:
                {
                    DrawTime(GetBitmap(), timeChangedEvent.Time);
                    break;
                }
            }
        }

        protected override void OnStart()
        {
            _mediator.Subscribe(typeof(ButtonEvent), this);
            _mediator.Subscribe(typeof(DateChangedEvent), this);
            _mediator.Subscribe(typeof(TimeChangedEvent), this);

            ClearDisplay();

            var screen = GetBitmap();

            DrawButtons(screen);
            DrawDate(screen, _localTimeProvider.Now.Date);
            DrawTime(screen, _localTimeProvider.Now.Time());

            screen.Flush();

            if (_clearProgressThread is null)
            {
                lock (SyncLock)
                {
                    if (_clearProgressThread is not null)
                    {
                        return;
                    }

                    _clearProgressThread = new Thread(ClearProgressAsync);
                    _clearProgressThread.Start();
                }
            }
        }

        protected override void OnStop()
        {
            _mediator.Unsubscribe(typeof(ButtonEvent), this);
            _mediator.Unsubscribe(typeof(DateChangedEvent), this);
            _mediator.Unsubscribe(typeof(TimeChangedEvent), this);

            if (_clearProgressThread is null)
            {
                return;
            }

            try
            {
                if (Thread.CurrentThread != _clearProgressThread)
                {
                    _clearProgressThread.Join(TimeSpan.FromSeconds(10));
                }
            }
            finally
            {
                _clearProgressThread = null;
            }
        }
    }
}
