using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.Threading;
using Emily.Clock.Configuration;
using Emily.Clock.Device.Led;
using Emily.Clock.Events;

namespace Emily.Clock.UI.Lights
{
    public interface INightLightManager
    {
        float Brightness { get; set; }
        bool Enabled { get; set; }

        void CycleBrightness();
        void CycleColor();
        bool Initialize();
        void Toggle();
    }

    public class NightNightLightManager : INightLightManager, IMediatorEventHandler
    {
        private static readonly Color MoonColor = NightLightColorConverter.ToColor(NightLightColor.Blue);
        private static readonly Color SunColor = NightLightColorConverter.ToColor(NightLightColor.Orange);

        private NightLightConfiguration _configuration;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILedManager _ledManager;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly IMediator _mediator;
        private PanelLight _panelMode;
        private readonly ConsumerThreadPool _updateLedsThread;

        public NightNightLightManager(IConfigurationManager configurationManager, ILedManager ledManager, ILocalTimeProvider localTimeProvider, IMediator mediator)
        {
            _configurationManager = configurationManager;
            _ledManager = ledManager;
            _localTimeProvider = localTimeProvider;
            _configurationManager.ConfigurationChanged += OnConfigurationChanged;
            _mediator = mediator;
            _updateLedsThread = new ConsumerThreadPool(1, UpdateLedsThread);

            UpdateConfiguration((NightLightConfiguration)_configurationManager.Get(NightLightConfiguration.Section));
        }

        public float Brightness
        {
            get => _configuration.Brightness;
            set
            {
                var brightness = NormalizeBrightness(value);
                if (Math.Abs(_configuration.Brightness - brightness) < double.Epsilon)
                {
                    return;
                }

                /* TODO: Pass previous and target brightness to fade?
                var currentBrightness = _configuration.Brightness;
                var targetBrightness = value;
                */

                _configuration.Brightness = value;
                _updateLedsThread.Enqueue(UpdateLeds.All);

                WriteConfiguration();
            }
        }

        public NightLightColor Color
        {
            get => _configuration.Color;
            set
            {
                if (value == _configuration.Color)
                {
                    return;
                }

                _configuration.Color = value;
                _updateLedsThread.Enqueue(UpdateLeds.Nightlight);

                WriteConfiguration();
            }
        }

        public bool Enabled
        {
            get => _configuration.Brightness > 0;
            set => Brightness = value ? 1.0f : 0.0f;
        }

        private PanelLight PanelMode
        {
            get => _panelMode;
            set
            {
                if (value == _panelMode)
                {
                    return;
                }

                _panelMode = value;
                _updateLedsThread.Enqueue(UpdateLeds.SunAndMoon);
            }
        }

        public void CycleBrightness()
        {
            Brightness = Brightness switch
            {
                >= 1.00f => 0.00f,
                >= 0.75f => 1.00f,
                >= 0.50f => 0.75f,
                >= 0.25f => 0.50f,
                _ => 0.25f
            };
        }

        public void CycleColor()
        {
            Color = Color switch
            {
                NightLightColor.Red => NightLightColor.Orange,
                NightLightColor.Orange => NightLightColor.Yellow,
                NightLightColor.Yellow => NightLightColor.Green,
                NightLightColor.Green => NightLightColor.Blue,
                NightLightColor.Blue => NightLightColor.Indigo,
                NightLightColor.Indigo => NightLightColor.Violet,
                NightLightColor.Violet => NightLightColor.Red,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private PanelLight GetPanelMode() => _localTimeProvider.IsBedTime ? PanelLight.Moon : PanelLight.Sun;

        public void HandleEvent(IMediatorEvent mediatorEvent)
        {
            if (mediatorEvent is not TimeChangedEvent)
            {
                return;
            }

            PanelMode = GetPanelMode();
        }

        public bool Initialize()
        {
            _mediator.Subscribe(typeof(TimeChangedEvent), this);
            _panelMode = GetPanelMode();
            _updateLedsThread.Enqueue(UpdateLeds.All);

            return true;
        }

        private static float NormalizeBrightness(float value)
        {
            var brightness = Math.Clamp(value, 0.0f, 1.0f);

            return brightness switch
            {
                >= 1.00f => 1.00f,
                >= 0.75f => 0.75f,
                >= 0.50f => 0.50f,
                >= 0.25f => 0.25f,
                _ => 0.0f
            };
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Section != NightLightConfiguration.Section)
            {
                return;
            }

            if (e.Configuration is not NightLightConfiguration configuration)
            {
                return;
            }

            UpdateConfiguration(configuration);
        }

        public void Toggle() => Enabled = !Enabled;

        [MemberNotNull(nameof(_configuration))]
        private void UpdateConfiguration(NightLightConfiguration configuration)
        {
            configuration.Brightness = NormalizeBrightness(configuration.Brightness);

            if (_configuration is null)
            {
                _configuration = configuration;
                // TODO: Should I not be updating leds here?
                return;
            }

            var updatePixels = Math.Abs(configuration.Brightness - _configuration.Brightness) < double.Epsilon || configuration.Color != _configuration.Color;
            
            _configuration = configuration;
            
            if (updatePixels)
            {
                _updateLedsThread.Enqueue(UpdateLeds.All);
            }
        }

        private void UpdateLedsThread(object item)
        {
            if (item is not UpdateLeds workItem)
            {
                return;
            }

            Debug.WriteLine("UpdateLedsThread start");
            var startTime = DateTime.UtcNow;

            var brightness = Brightness;
            var nightlightColor = NightLightColorConverter.ToColor(Color);
            var panelMode = PanelMode;
            
            if (workItem.UpdateNightLight)
            {
                _ledManager.SetNightlightLeds(nightlightColor, brightness);
            }

            if (workItem.UpdateSunAndMoon)
            {
                // Check always on 
                if (brightness == 0.0f)
                {
                    brightness = 0.25f;
                }

                _ledManager.SetMoonLed(PanelLight.Moon == panelMode ? MoonColor : System.Drawing.Color.Black, brightness);
                _ledManager.SetSunLed(PanelLight.Sun == panelMode ? SunColor : System.Drawing.Color.Black, brightness);
            }

            _ledManager.Update();

            Debug.WriteLine($"UpdateLedsThread took {(DateTime.UtcNow - startTime)}");
        }

        private void WriteConfiguration()
        {
            Thread.Sleep(0);

            _configurationManager.SaveAsync(NightLightConfiguration.Section, _configuration);
        }
    }

    public static class NightLightManagerExtensions
    {
        internal static Resources.BitmapResources GetEnabledBitmapId(this INightLightManager nightLightManager)
        {
            return nightLightManager.Enabled
                ? Resources.BitmapResources.Lightbulb_22
                : Resources.BitmapResources.Lightbulb_22_Outline;
        }
    }

    internal class UpdateLeds
    {
        private UpdateLeds(bool updateNightLight, bool updateSunAndMoon)
        {
            UpdateNightLight = updateNightLight;
            UpdateSunAndMoon = updateSunAndMoon;
        }

        public bool UpdateNightLight { get; }
        public bool UpdateSunAndMoon { get; }

        public static UpdateLeds All { get; } = new(true, true);
        public static UpdateLeds Nightlight { get; } = new(true, false);
        public static UpdateLeds SunAndMoon { get; } = new(false, true);
    }
}
