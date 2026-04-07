using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using CCSWE.nanoFramework.NeoPixel;
using Emily.Clock.Device.Gpio;
using nanoFramework.Runtime.Native;

namespace Emily.Clock.Device.Led.NeoPixel;

internal class NeoPixelLedManager : ILedManager
{
    private readonly IGpioProvider _gpioProvider;
    private NeoPixelStrip? _neoPixelStrip;
    private readonly NeoPixelLedOptions _options;

    public NeoPixelLedManager(IGpioProvider gpioProvider, NeoPixelLedOptions options)
    {
        _gpioProvider = gpioProvider;
        _options = options;
    }

    // ReSharper disable once MergeConditionalExpression
    public int Count => _neoPixelStrip is not null ? _neoPixelStrip.Count : 0;

    public bool IsInitialized { get; private set; }

    [MemberNotNull(nameof(_neoPixelStrip))]
    private void CheckInitialized()
    {
        if (_neoPixelStrip is null || !IsInitialized)
        {
            throw new InvalidOperationException("NeoPixelStrip is not initialized.");
        }
    }

    public void Clear(bool update = true)
    {
        CheckInitialized();

        _neoPixelStrip.Clear();

        if (update)
        {
            Update();
        }
    }

    [MemberNotNull(nameof(_neoPixelStrip))]
    public bool Initialize()
    {
        if (_neoPixelStrip is not null && IsInitialized)
        {
            return true;
        }

        _options.PreInitialize?.Invoke(_gpioProvider);

        _neoPixelStrip = new NeoPixelStrip(_options.Pin, _options.Count, _options.Driver);
        _neoPixelStrip.Clear();
        _neoPixelStrip.Update();

        IsInitialized = true;

        Power.OnRebootEvent += OnReboot;

        return true;
    }

    private void OnReboot()
    {
        if (!IsInitialized)
        {
            return;
        }

        Clear();
    }

    public void SetLed(int index, Color color)
    {
        CheckInitialized();

        _neoPixelStrip.SetLed(index, color);
    }

    public void SetLed(int index, Color color, float brightness)
    {
        CheckInitialized();
        
        _neoPixelStrip.SetLed(index, color, brightness);
    }

    public void SetLeds(int startIndex, int endIndex, Color color)
    {
        CheckInitialized();

        _neoPixelStrip.SetLeds(startIndex, endIndex, color);

    }

    public void SetLeds(int startIndex, int endIndex, Color color, float brightness)
    {
        CheckInitialized();
        
        _neoPixelStrip.SetLeds(startIndex, endIndex, color, brightness);
    }

    public void Update()
    {
        CheckInitialized();

        _neoPixelStrip.Update();
    }
}
