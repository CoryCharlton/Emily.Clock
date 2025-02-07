using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using CCSWE.nanoFramework.NeoPixel;
using CCSWE.nanoFramework.NeoPixel.Drivers;
using Emily.Clock.Device.Led;
using nanoFramework.Runtime.Native;
using ColorConverter = CCSWE.nanoFramework.NeoPixel.ColorConverter;

namespace Emily.Clock.App.Hardware
{
    internal class NeoPixelStripManager: ILedManager
    {
        private const ushort Count = 47;
        private const byte Pin = 19;

        private const byte MoonLedIndex = 0;
        private const byte SunLedIndex = 1;

        private NeoPixelStrip? _neoPixelStrip;

        public bool IsInitialized { get; private set; }

        public void Clear(bool update = true)
        {
            RequireInitialization();

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

            IsInitialized = true;

            _neoPixelStrip = new NeoPixelStrip(Pin, Count, new Ws2812B());
            _neoPixelStrip.Clear();
            _neoPixelStrip.Update();

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

        [MemberNotNull(nameof(_neoPixelStrip))]
        private void RequireInitialization()
        {
            if (_neoPixelStrip is null)
            {
                throw new InvalidOperationException("NeoPixelStrip is not initialized.");
            }
        }
        public void SetMoonLed(Color color, float brightness)
        {
            RequireInitialization();

            // TODO: Come back to this (I should add better comments as I don't recall what I was coming back to...)
            if (Color.Black.Equals(color))
            {
                _neoPixelStrip.SetLed(MoonLedIndex, color);
            }
            else
            {
                _neoPixelStrip.SetLed(MoonLedIndex, color, brightness);
            }
        }

        public void SetNightlightLeds(Color color, float brightness)
        {
            RequireInitialization();

            var scaledColor = ColorConverter.ScaleBrightness(color, brightness);

            for (var i = 0; i < _neoPixelStrip.Count; i++)
            {
                if (i != MoonLedIndex && i != SunLedIndex)
                {
                    _neoPixelStrip.SetLed(i, scaledColor);
                }
            }
        }

        public void SetSunLed(Color color, float brightness)
        {
            RequireInitialization();

            // TODO: Come back to this (I should add better comments as I don't recall what I was coming back to...)
            if (Color.Black.Equals(color))
            {
                _neoPixelStrip.SetLed(SunLedIndex, color);
            }
            else
            {
                _neoPixelStrip.SetLed(SunLedIndex, color, brightness);
            }
        }

        public void Update()
        {
            RequireInitialization();

            _neoPixelStrip.Update();
        }
    }
}
