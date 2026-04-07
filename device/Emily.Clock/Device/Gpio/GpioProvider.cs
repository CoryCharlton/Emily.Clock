using System;
using System.Device.Gpio;
using Iot.Device.Button;

namespace Emily.Clock.Device.Gpio;

public interface IGpioProvider
{
    GpioController Controller { get; }

    GpioButton OpenButton(int pin, TimeSpan doublePress, TimeSpan holding, PinMode pinMode = PinMode.InputPullUp, TimeSpan debounceTime = default);
    GpioPin OpenPin(int pin, PinMode mode);
    PinValue Read(int pin);
    void Write(int pin, PinValue value);
}

public class GpioProvider : IGpioProvider
{
    public GpioController Controller { get; } = new();

    public GpioButton OpenButton(int pin, TimeSpan doublePress, TimeSpan holding, PinMode pinMode, TimeSpan debounceTime)
    {
        return new GpioButton(pin, doublePress, holding, Controller, false, pinMode, debounceTime);
    }

    public GpioPin OpenPin(int pin, PinMode mode)
    {
        return Controller.OpenPin(pin, mode);
    }

    public PinValue Read(int pin)
    {
        return Controller.Read(pin);
    }

    public void Write(int pin, PinValue value)
    {
        Controller.Write(pin, value);
    }
}