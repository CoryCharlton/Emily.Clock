using System;
using System.Drawing;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Configuration;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.Mediator.Events;
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
        private readonly IConfigurationManager _configurationManager;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly Color _moonColor = NightLightConverter.ToColor(NightLightColor.Blue);
        private readonly INeoPixelManager _neoPixelManager;
        private PanelLight _panelMode;
        private readonly Color _sunColor = NightLightConverter.ToColor(NightLightColor.Orange);

        public NightNightLightManager(IConfigurationManager configurationManager, ILocalTimeProvider localTimeProvider , ILogger logger, IMediator mediator, INeoPixelManager neoPixelManager)
        {
            _configurationManager = configurationManager;
            _localTimeProvider = localTimeProvider;
            _configurationManager.ConfigurationChanged += OnConfigurationChanged;
            _configuration = (NightLightConfiguration)_configurationManager.Get(NightLightConfiguration.Section);
            _logger = logger;
            _mediator = mediator;
            _neoPixelManager = neoPixelManager;
        }

        // TODO: Stop ready/writing direct to the configuration so that "UpdateConfiguration" can set the individual properties
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

                UpdateAllPixels();
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

                UpdateStripPixels(true);
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

            BenchmarkSetPixel();

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

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Section != NightLightConfiguration.Section)
            {
                return;
            }

            _logger.LogDebug("Configuration updated");

            _configuration = (NightLightConfiguration) e.Configuration;

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

            var color = ColorConverter.ScaleBrightness(NightLightConverter.ToColor(Color), Brightness);

            for (var i = 0; i < _neoPixelManager.Count; i++)
            {
                if (i != MoonPixel && i != SunPixel)
                {
                    _neoPixelManager.SetPixelFast(i, color);
                }
            }

            _logger.LogDebug($"UpdateStripPixels: {NightLightConverter.ToString(Color)} {Brightness} - Starting update");

            if (update)
            {
                _neoPixelManager.Update();
            }

            _logger.LogDebug($"UpdateStripPixels: {NightLightConverter.ToString(Color)} {Brightness} - Update completed");

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
            _logger.LogDebug("Writing configuration");

            _configurationManager.SaveAsync(NightLightConfiguration.Section, _configuration);
        }

        private void BenchmarkSetPixel(int loops = 10)
        {
            return;

            var color = NightLightConverter.ToColor(Color);

            for (var loop = 0; loop < loops; loop++)
            {
                var startTime = DateTime.UtcNow;
                for (var i = 0; i < _neoPixelManager.Count; i++)
                {
                    if (i != MoonPixel && i != SunPixel)
                    {
                        _neoPixelManager.SetPixel(i, color);
                    }
                }

                _logger.LogDebug($"SetPixel took {DateTime.UtcNow - startTime}");
            }

            var scaledColor = ColorConverter.ScaleBrightness(color, Brightness);

            for (var loop = 0; loop < loops; loop++)
            {
                var startTime = DateTime.UtcNow;
                for (var i = 0; i < _neoPixelManager.Count; i++)
                {
                    if (i != MoonPixel && i != SunPixel)
                    {
                        _neoPixelManager.SetPixelFast(i, scaledColor);
                    }
                }

                _logger.LogDebug($"SetPixelFast took {DateTime.UtcNow - startTime}");
            }
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
}
