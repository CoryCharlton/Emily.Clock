using System;
using System.Drawing;
using Iot.Device.Ws28xx.Esp32;

namespace Emily.Clock.Device.NeoPixel
{
    // ReSharper disable once InconsistentNaming
    public abstract class NeoPixelManagerBase: INeoPixelManager
    {
        private double _brightness = 1.0;

        public double Brightness
        {
            get => _brightness;
            set => _brightness = Math.Clamp(value, 0.0, 1.0);
        }

        public abstract int Count { get; }

        protected Ws28xx LedStrip { get; private set; }

        public bool IsInitialized { get; private set; }

        public void Clear()
        {
            LedStrip.Image.Clear();
        }

        public void Clear(Color color)
        {
            LedStrip.Image.Clear(ScaleBrightness(color, Brightness));
        }

        public void Clear(int pixel)
        {
            LedStrip.Image.Clear(pixel, 0);
        }

        public bool Initialize()
        {
            if (IsInitialized)
            {
                return true;
            }

            IsInitialized = true;

            LedStrip = InitializeNeoPixel();

            if (LedStrip is null)
            {
                return false;
            }

            if (LedStrip.Image.Height > 1)
            {
                // I'm making assumptions that the bitmap will be a single row
                throw new InvalidOperationException();
            }

            Clear();
            Update();

            return true;
        }

        protected abstract Ws28xx InitializeNeoPixel();

        protected static Color ScaleBrightness(Color color, double brightness)
        {
            return ColorConverter.ScaleBrightness(color, brightness);
        }

        public void SetPixel(int pixel, Color color, double brightness = -1)
        {
            if (brightness < 0)
            {
                brightness = Brightness;
            }

            LedStrip.Image.SetPixel(pixel, 0, ScaleBrightness(color, brightness));
        }

        public void SetPixelFast(int pixel, Color color) => LedStrip.Image.SetPixel(pixel, 0, color);

        public void Update()
        {
            LedStrip.Update();
        }
    }
}
