using System;
using System.Drawing;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Configuration;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.Mediator.Events;
using MakoIoT.Device.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.UI.Lights
{
    public interface INightLightManager
    {
        double Brightness { get; set; }
        bool Enabled { get; set; }

        void CycleBrightness();
        void CycleColor();
        bool Initialize();
        void Toggle();
    }

    public class NightNightLightManager : INightLightManager, IMediatorEventHandler
    {
        private NightLightConfiguration _configuration;
        private readonly IConfigurationService _configurationService;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly Color _moonColor = NightLightConverter.ToColor(NightLightColor.Blue);
        private readonly INeoPixelManager _neoPixelManager;
        private PanelLight _panelMode;
        private readonly Color _sunColor = NightLightConverter.ToColor(NightLightColor.Yellow);

        public NightNightLightManager(IConfigurationService configurationService, ILocalTimeProvider localTimeProvider , ILogger logger, IMediator mediator, INeoPixelManager neoPixelManager)
        {
            _configurationService = configurationService;
            _localTimeProvider = localTimeProvider;
            _configurationService.ConfigurationUpdated += OnConfigurationUpdated;
            _configuration = GetConfiguration();
            _logger = logger;
            _mediator = mediator;
            _neoPixelManager = neoPixelManager;
        }

        public double Brightness
        {
            get => NormalizeBrightness(_configuration.Brightness);
            set
            {
                var brightness = NormalizeBrightness(value);
                if (Math.Abs(_configuration.Brightness - brightness) < double.Epsilon)
                {
                    return;
                }

                _configuration.Brightness = brightness;

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

                WriteConfiguration();
            }
        }

        public bool Enabled
        {
            get => _configuration.Brightness > 0;
            set => Brightness = value ? 1.0 : 0.0;
        }

        private int MoonPixel => _configuration.MoonPixel;

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

                UpdateSunAndMoonPixels(true);
            }
        }

        private int SunPixel => _configuration.SunPixel;

        public void CycleBrightness()
        {
            // Decrease
            /*
            Brightness = Brightness switch
            {
                >= 1.00 => 0.75,
                >= 0.75 => 0.50,
                >= 0.50 => 0.25,
                >= 0.25 => 0.00,
                _ => 1.00
            };
            */

            // Increase
            Brightness = Brightness switch
            {
                >= 1.00 => 0.00,
                >= 0.75 => 1.00,
                >= 0.50 => 0.75,
                >= 0.25 => 0.50,
                _ => 0.25
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

        private NightLightConfiguration GetConfiguration()
        {
            return (NightLightConfiguration) _configurationService.GetConfigSection(NightLightConfiguration.SectionName, typeof(NightLightConfiguration));
        }

        private PanelLight GetPanelMode()
        {
            var currentTime = _localTimeProvider.Now.Time();

            // Make the wake / sleep time configurable
            return currentTime.Hour is > 6 and < 20 ? PanelLight.Sun : PanelLight.Moon;
        }

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

            UpdateAllPixels();

            return true;
        }

        private static double NormalizeBrightness(double value)
        {
            var brightness = Math.Clamp(value, 0.0, 1.0);

            return brightness switch
            {
                >= 1.00 => 1.00,
                >= 0.75 => 0.75,
                >= 0.50 => 0.50,
                >= 0.25 => 0.25,
                _ => 0.0
            };
        }

        private void OnConfigurationUpdated(object sender, System.EventArgs e)
        {
            if (e is not ObjectEventArgs objectEventArgs || !string.Equals(NightLightConfiguration.SectionName, objectEventArgs.Data as string))
            {
                return;
            }

            _configuration = GetConfiguration();

            UpdateAllPixels();
        }

        public void Toggle() => Enabled = !Enabled;

        private void UpdateAllPixels()
        {

            _neoPixelManager.Brightness = Brightness;

            UpdateStripPixels(false);
            UpdateSunAndMoonPixels(false);

            _neoPixelManager.Update();
        }

        private void UpdateStripPixels(bool update = false)
        {
            _logger.LogDebug($"UpdateStripPixels: {NightLightConverter.ToString(Color)} {Brightness}");

            var color = NightLightConverter.ToColor(Color);

            for (var i = 0; i < _neoPixelManager.Count; i++)
            {
                if (i != MoonPixel && i != SunPixel)
                {
                    _neoPixelManager.SetPixel(i, color);
                }
            }

            if (update)
            {
                _neoPixelManager.Update();
            }
        }

        private void UpdateSunAndMoonPixels(bool update = false)
        {
            var panelMode = PanelMode;

            switch (panelMode)
            {
                case PanelLight.Off:
                    _neoPixelManager.Clear(MoonPixel);
                    _neoPixelManager.Clear(SunPixel);
                    break;
                case PanelLight.Sun:
                    _neoPixelManager.Clear(MoonPixel);
                    _neoPixelManager.SetPixel(SunPixel, _sunColor);
                    break;
                case PanelLight.Moon:
                    _neoPixelManager.SetPixel(MoonPixel, _moonColor);
                    _neoPixelManager.Clear(SunPixel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (update)
            {
                _neoPixelManager.Update();
            }
        }

        private void WriteConfiguration()
        {
            _configurationService.UpdateConfigSection(NightLightConfiguration.SectionName, _configuration);
        }
    }

    public static class LightManagerExtensions
    {
        internal static Resources.BitmapResources GetEnabledBitmapId(this INightLightManager nightLightManager)
        {
            return nightLightManager.Enabled
                ? Resources.BitmapResources.Lightbulb_22
                : Resources.BitmapResources.Lightbulb_22_Outline;
        }
    }
}
