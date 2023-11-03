using System.Drawing;
using CCSWE.nanoFramework.NeoPixel;
using CCSWE.nanoFramework.NeoPixel.Drivers;
using Emily.Clock.Device.Led;
using ColorConverter = CCSWE.nanoFramework.NeoPixel.ColorConverter;

namespace Emily.Clock.App.Hardware
{
    internal class NeoPixelStripManager: ILedManager
    {
        private const ushort Count = 47;
        private const byte Pin = 19;

        private const byte MoonLedIndex = 0;
        private const byte SunLedIndex = 1;

        private NeoPixelStrip _neoPixelStrip;

        public bool IsInitialized { get; private set; }

        public void Clear(bool update = true)
        {
            _neoPixelStrip.Clear();

            if (update)
            {
                Update();
            }
        }

        public bool Initialize()
        {
            if (IsInitialized)
            {
                return true;
            }

            IsInitialized = true;

            _neoPixelStrip = new NeoPixelStrip(Pin, Count, new Ws2812B());
            _neoPixelStrip.Clear();
            _neoPixelStrip.Update();

            return true;
        }

        public void SetMoonLed(Color color, double brightness)
        {
            // TODO: Come back to this
            if (Color.Black.Equals(color))
            {
                _neoPixelStrip.SetLed(MoonLedIndex, color);
            }
            else
            {
                _neoPixelStrip.SetLed(MoonLedIndex, color, brightness);
            }
        }

        public void SetNightlightLeds(Color color, double brightness)
        {
            var scaledColor = ColorConverter.ScaleBrightness(color, brightness);

            for (var i = 0; i < _neoPixelStrip.Count; i++)
            {
                if (i != MoonLedIndex && i != SunLedIndex)
                {
                    _neoPixelStrip.SetLed(i, scaledColor);
                }
            }
        }

        public void SetSunLed(Color color, double brightness)
        {
            // TODO: Come back to this
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
            _neoPixelStrip.Update();
        }
    }
}
