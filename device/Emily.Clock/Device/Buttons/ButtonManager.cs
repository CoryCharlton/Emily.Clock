using System.Collections;
using System.Device.Gpio;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Mediator.Events;
using Iot.Device.Button;

namespace Emily.Clock.Device.Buttons;

public interface IButtonManager
{
    bool Initialize();
    bool IsPressed(Button button);
}

public class ButtonManager : IButtonManager
{
    private readonly Hashtable _buttons = new();
    private readonly IGpioProvider _gpioProvider;
    private readonly Hashtable _holdingStates = new();
    private bool _initialized;
    private readonly IMediator _mediator;
    private readonly ButtonOptions _options;

    public ButtonManager(IGpioProvider gpioProvider, IMediator mediator, ButtonOptions options)
    {
        _gpioProvider = gpioProvider;
        _mediator = mediator;
        _options = options;
    }

    public bool Initialize()
    {
        if (_initialized)
        {
            return true;
        }

        _initialized = true;

        InitializeButton(Button.One);
        InitializeButton(Button.Two);
        InitializeButton(Button.Three);

        return true;
    }

    private void InitializeButton(Button button)
    {
        var config = _options.GetConfiguration(button);
        var gpioButton = _gpioProvider.OpenButton(config.Pin, _options.DoublePressTime, _options.HoldingTime, config.PinMode, _options.DebounceTime);
        gpioButton.IsDoublePressEnabled = false;
        gpioButton.IsHoldingEnabled = true;

        gpioButton.DoublePress += (_, _) => OnDoublePress(button);
        gpioButton.Holding += (_, eventArgs) => OnHolding(button, eventArgs);
        gpioButton.Press += (_, _) => OnPress(button);

        _buttons.Add(button, gpioButton);
    }

    public bool IsPressed(Button button)
    {
        var config = _options.GetConfiguration(button);
        var value = _gpioProvider.Read(config.Pin);
        return value == (config.PinMode == PinMode.InputPullUp ? PinValue.Low : PinValue.High);
    }

    private void OnDoublePress(Button button)
    {
        _mediator.Publish(new ButtonEvent(button, ButtonEventType.DoublePress));
    }

    private void OnHolding(Button button, ButtonHoldingEventArgs? eventArgs)
    {
        if (eventArgs is null)
        {
            return;
        }

        _holdingStates[button] = eventArgs.HoldingState;

        if (ButtonHoldingState.Started != eventArgs.HoldingState)
        {
            return;
        }

        _mediator.Publish(new ButtonEvent(button, ButtonEventType.Holding));
    }

    private void OnPress(Button button)
    {
        var holdingState = _holdingStates[button];
        if (holdingState != null)
        {
            if (ButtonHoldingState.Started == (ButtonHoldingState)holdingState)
            {
                _holdingStates.Remove(button);

                return;
            }
        }

        _mediator.Publish(new ButtonEvent(button, ButtonEventType.Press));
    }
}
