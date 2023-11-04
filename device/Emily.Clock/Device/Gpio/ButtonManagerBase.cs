using System;
using System.Collections;
using System.Device.Gpio;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Mediator.Events;
using Iot.Device.Button;

namespace Emily.Clock.Device.Gpio
{
    public abstract class ButtonManagerBase : IButtonManager
    {
        private readonly Hashtable _buttons = new();
        private readonly IGpioProvider _gpioProvider;
        private readonly Hashtable _holdingStates = new();
        private bool _initialized;
        private readonly IMediator _mediator;

        protected ButtonManagerBase(IGpioProvider gpioProvider, IMediator mediator)
        {
            _gpioProvider = gpioProvider;
            _mediator = mediator;
        }

        protected virtual TimeSpan DebounceTime { get; } = TimeSpan.FromMilliseconds(50); // 25?

        protected virtual TimeSpan DoublePressTime { get; } = TimeSpan.FromMilliseconds(1500); // Default is 1500 millis

        protected virtual TimeSpan HoldingTime { get; } = TimeSpan.FromMilliseconds(1000); // Default is 2000 millis

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
            var gpioButton = _gpioProvider.OpenButton(GetPin(button), DoublePressTime, HoldingTime, GetPinMode(button), DebounceTime);
            gpioButton.IsDoublePressEnabled = false;
            gpioButton.IsHoldingEnabled = true;

            gpioButton.DoublePress += (_, _) => OnDoublePress(button);
            gpioButton.Holding += (_, eventArgs) => OnHolding(button, eventArgs);
            gpioButton.Press += (_, _) => OnPress(button);

            _buttons.Add(button, gpioButton);
        }

        public bool IsPressed(Button button)
        {
            var value = _gpioProvider.Read(GetPin(button));
            return value == GetPressedValue(button);
        }

        protected GpioButton GetButton(Button button)
        {
            return (GpioButton)_buttons[button];
        }

        protected abstract int GetPin(Button button);

        protected abstract PinMode GetPinMode(Button button);

        private PinValue GetPressedValue(Button button)
        {
            return GetPinMode(button) == PinMode.InputPullUp ? PinValue.Low : PinValue.High;
        }

        private void OnDoublePress(Button button)
        {
            _mediator.Publish(new ButtonEvent(button, ButtonEventType.DoublePress));
        }

        private void OnHolding(Button button, ButtonHoldingEventArgs eventArgs)
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
}
