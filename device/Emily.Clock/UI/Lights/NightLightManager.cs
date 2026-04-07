using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Configuration;
using Emily.Clock.Device.Led;
using Emily.Clock.Events;
using Emily.Clock.UI.Lights.Effects;

namespace Emily.Clock.UI.Lights;

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
    private const int AlarmFlashDelay = 300;
    private const int StopRequestedHandle = 1;
    private const int UpdateRequestedHandle = 0;

    // TODO: Maybe move these colors to the configuration?
    private static readonly Color AlarmColor = NightLightColorConverter.ToColor(NightLightColor.Orange);
    private static readonly Color MoonColor = NightLightColorConverter.ToColor(NightLightColor.Blue);
    private static readonly Color SunColor = NightLightColorConverter.ToColor(NightLightColor.Orange);

    private readonly FlashEffect _alarmFlashEffect;
    private NightLightConfiguration _configuration;
    private readonly IConfigurationManager _configurationManager;
    private INightLightEffect _currentEffect;
    private Thread? _effectThread;
    private readonly WaitHandle[] _effectWaitHandles;
    private bool _isAlarming;
    private readonly LedConfiguration _ledConfiguration;
    private readonly ILedManager _ledManager;
    private readonly ILocalTimeProvider _localTimeProvider;
    private readonly IMediator _mediator;
    private PanelLight _panelMode;
    private readonly ManualResetEvent _stopRequested = new(false);
    private readonly ManualResetEvent _updateRequested = new(false);

    public NightNightLightManager(IConfigurationManager configurationManager, LedConfiguration ledConfiguration, ILedManager ledManager, ILocalTimeProvider localTimeProvider, IMediator mediator)
    {
        _alarmFlashEffect = new FlashEffect(AlarmColor, System.Drawing.Color.Black, AlarmFlashDelay);
        _configurationManager = configurationManager;
        _configurationManager.ConfigurationChanged += OnConfigurationChanged;
        _currentEffect = new SolidEffect(System.Drawing.Color.Black, 0.0f);
        _effectWaitHandles = [_updateRequested, _stopRequested];
        _ledConfiguration = ledConfiguration;
        _ledManager = ledManager;
        _localTimeProvider = localTimeProvider;
        _mediator = mediator;

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

            _configuration.Brightness = brightness;

            UpdateCurrentEffect();
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

            UpdateCurrentEffect();
            WriteConfiguration();
        }
    }

    public bool Enabled
    {
        get => _configuration.Brightness > 0;
        set => Brightness = value ? 1.0f : 0.0f;
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

    private void EffectLoop()
    {
        while (true)
        {
            var effect = GetEffectiveEffect();
            var signaled = WaitHandle.WaitAny(_effectWaitHandles, effect.Delay, false);

            if (signaled == StopRequestedHandle) break;

            if (signaled == UpdateRequestedHandle)
            {
                _updateRequested.Reset();
                effect = GetEffectiveEffect();
                effect.Start(_ledManager, _ledConfiguration);
                UpdatePanelLeds();
                _ledManager.Update();
            }
            else
            {
                // Timeout - advance animation
                if (effect.Step(_ledManager, _ledConfiguration))
                {
                    UpdatePanelLeds();
                    _ledManager.Update();
                }
            }
        }
    }

    private INightLightEffect GetEffectiveEffect() => _isAlarming ? _alarmFlashEffect : _currentEffect;

    private PanelLight GetPanelMode() => _localTimeProvider.IsBedTime ? PanelLight.Moon : PanelLight.Sun;

    public void HandleEvent(IMediatorEvent mediatorEvent)
    {
        switch (mediatorEvent)
        {
            case TimeChangedEvent:
                var panelMode = GetPanelMode();
                if (panelMode == _panelMode)
                {
                    break;
                }

                _panelMode = panelMode;
                _updateRequested.Set();
                break;
            case AlarmStateChangedEvent alarmStateChangedEvent:
                _isAlarming = alarmStateChangedEvent.IsAlarming;
                _updateRequested.Set();
                break;
        }
    }

    public bool Initialize()
    {
        _mediator.Subscribe(typeof(AlarmStateChangedEvent), this);
        _mediator.Subscribe(typeof(TimeChangedEvent), this);
        _panelMode = GetPanelMode();

        _effectThread = new Thread(EffectLoop);
        _effectThread.Start();

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
        _configuration = configuration;
        UpdateCurrentEffect();
    }

    private void UpdateCurrentEffect()
    {
        var color = NightLightColorConverter.ToColor(_configuration.Color);
        var brightness = _configuration.Brightness;

        _currentEffect = _configuration.Effect switch
        {
            NightLightEffectType.Breathe => new BreatheEffect(color, brightness),
            NightLightEffectType.Rainbow => new RainbowEffect(),
            _ => new SolidEffect(color, brightness)
        };
        _updateRequested.Set();
    }

    private void UpdatePanelLeds()
    {
        var panelBrightness = _configuration.PanelBrightness;
        var panelMode = _panelMode;

        var moonColor = PanelLight.Moon == panelMode ? MoonColor : System.Drawing.Color.Black;
        _ledManager.SetLed(_ledConfiguration.MoonLedIndex, moonColor, panelBrightness);

        var sunColor = PanelLight.Sun == panelMode ? SunColor : System.Drawing.Color.Black;
        _ledManager.SetLed(_ledConfiguration.SunLedIndex, sunColor, panelBrightness);
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
