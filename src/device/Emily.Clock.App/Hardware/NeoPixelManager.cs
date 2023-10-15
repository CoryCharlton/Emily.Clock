using Emily.Clock.Device.NeoPixel;
using Iot.Device.Ws28xx.Esp32;

namespace Emily.Clock.App.Hardware
{
    // ReSharper disable once InconsistentNaming
    internal class NeoPixelManager: NeoPixelManagerBase
    {
        private const int Pin = 19;

        public override int Count => 3;

        protected override Ws28xx InitializeNeoPixel() => new Ws2812c(Pin, Count);
    }
}
