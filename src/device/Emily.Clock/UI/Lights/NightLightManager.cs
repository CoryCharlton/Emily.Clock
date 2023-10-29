using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using CCSWE.nanoFramework.Threading;
using Emily.Clock.Configuration;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.Mediator.Events;

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
        private static readonly Color MoonColor = NightLightConverter.ToColor(NightLightColor.Blue);
        private static readonly Color SunColor = NightLightConverter.ToColor(NightLightColor.Orange);

        private NightLightConfiguration _configuration;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILocalTimeProvider _localTimeProvider;
        private readonly IMediator _mediator;
        private Color _moonColor = MoonColor;
        private readonly INeoPixelManager _neoPixelManager;
        private PanelLight _panelMode;
        private Color _stripColor;
        private Color _sunColor = SunColor;
        private readonly ConsumerThreadPool _updatePixelsThread;

        public NightNightLightManager(IConfigurationManager configurationManager, ILocalTimeProvider localTimeProvider, IMediator mediator, INeoPixelManager neoPixelManager)
        {
            _configurationManager = configurationManager;
            _localTimeProvider = localTimeProvider;
            _configurationManager.ConfigurationChanged += OnConfigurationChanged;
            _mediator = mediator;
            _neoPixelManager = neoPixelManager;
            _updatePixelsThread = new ConsumerThreadPool(1, UpdatePixelsThread);

            UpdateConfiguration((NightLightConfiguration)_configurationManager.Get(NightLightConfiguration.Section));
        }

        public double Brightness
        {
            get => _configuration.Brightness;
            set
            {
                var brightness = NormalizeBrightness(value);
                if (Math.Abs(_configuration.Brightness - brightness) < double.Epsilon)
                {
                    return;
                }

                _configuration.Brightness = brightness;

                _moonColor = ColorConverter.ScaleBrightness(MoonColor, brightness);
                _stripColor = ColorConverter.ScaleBrightness(NightLightConverter.ToColor(Color), brightness);
                _sunColor = ColorConverter.ScaleBrightness(SunColor, brightness);
                
                _updatePixelsThread.Enqueue(UpdatePixels.All);

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
                _stripColor = ColorConverter.ScaleBrightness(NightLightConverter.ToColor(Color), Brightness);
                _updatePixelsThread.Enqueue(UpdatePixels.Strip);

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
                _updatePixelsThread.Enqueue(UpdatePixels.SunAndMoon);
            }
        }

        private int SunPixel => _configuration.SunPixel;

        public void CycleBrightness()
        {
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
            _updatePixelsThread.Enqueue(UpdatePixels.All);

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

            UpdateConfiguration((NightLightConfiguration) e.Configuration);
        }

        private void SetStripPixels()
        {
            for (var i = 0; i < _neoPixelManager.Count; i++)
            {
                if (i != MoonPixel && i != SunPixel)
                {
                    _neoPixelManager.SetPixelFast(i, _stripColor);
                }
            }
        }

        private void SetSunAndMoonPixels()
        {
            switch (PanelMode)
            {
                case PanelLight.Off:
                    _neoPixelManager.SetPixelFast(MoonPixel, System.Drawing.Color.Black);
                    _neoPixelManager.SetPixelFast(SunPixel, System.Drawing.Color.Black);
                    break;
                case PanelLight.Sun:
                    _neoPixelManager.SetPixelFast(MoonPixel, System.Drawing.Color.Black);
                    _neoPixelManager.SetPixelFast(SunPixel, _sunColor);
                    break;
                case PanelLight.Moon:
                    _neoPixelManager.SetPixelFast(MoonPixel, _moonColor);
                    _neoPixelManager.SetPixelFast(SunPixel, System.Drawing.Color.Black);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Toggle() => Enabled = !Enabled;

        private void UpdateConfiguration(NightLightConfiguration configuration)
        {
            configuration.Brightness = NormalizeBrightness(configuration.Brightness);

            if (_configuration is null)
            {
                _configuration = configuration;

                _moonColor = ColorConverter.ScaleBrightness(MoonColor, _configuration.Brightness);
                _stripColor = ColorConverter.ScaleBrightness(NightLightConverter.ToColor(Color), _configuration.Brightness);
                _sunColor = ColorConverter.ScaleBrightness(SunColor, _configuration.Brightness);

                return;
            }

            var updatePixels = false;

            if (Math.Abs(configuration.Brightness - _configuration.Brightness) < double.Epsilon)
            {
                updatePixels = true;
            }

            if (configuration.Color != _configuration.Color)
            {
                updatePixels = true;
            }

            if (configuration.MoonPixel != _configuration.MoonPixel)
            {
                updatePixels = true;
            }

            if (configuration.SunPixel != _configuration.SunPixel)
            {
                updatePixels = true;
            }

            _configuration = configuration;
            
            if (updatePixels)
            {
                _updatePixelsThread.Enqueue(UpdatePixels.All);
            }
        }

        private void UpdatePixelsThread(object item)
        {
            if (item is not UpdatePixels workItem)
            {
                return;
            }

            Debug.WriteLine($"UpdatePixelsThread start");
            var startTime = DateTime.UtcNow;

            _neoPixelManager.Brightness = Brightness;

            if (workItem.UpdateStrip)
            {
                SetStripPixels();
            }

            if (workItem.UpdateSunAndMoon)
            {
                SetSunAndMoonPixels();
            }

            _neoPixelManager.Update();

            Debug.WriteLine($"UpdatePixelsThread took {(DateTime.UtcNow - startTime)}");
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

    internal class UpdatePixels
    {
        private UpdatePixels(bool updateStrip, bool updateSunAndMoon)
        {
            UpdateStrip = updateStrip;
            UpdateSunAndMoon = updateSunAndMoon;
        }

        public bool UpdateStrip { get; }
        public bool UpdateSunAndMoon { get; }

        public static UpdatePixels All { get; } = new(true, true);
        public static UpdatePixels Strip { get; } = new(true, false);
        public static UpdatePixels SunAndMoon { get; } = new(false, true);
    }
}
