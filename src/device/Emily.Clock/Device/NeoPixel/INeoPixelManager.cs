using System.Drawing;

namespace Emily.Clock.Device.NeoPixel
{
    // ReSharper disable once InconsistentNaming
    public interface INeoPixelManager
    {
        /// <summary>
        /// Default brightness of the pixel between 0.0 and 1.0.
        /// </summary>
        /// <remarks>
        /// This will take effect on the next call to <see cref="SetPixel(int,Color,double)"/>
        /// </remarks>
        double Brightness { get; set; }

        /// <summary>
        /// The total number of LEDs in the strip
        /// </summary>
        int Count { get; }

        bool IsInitialized { get; }

        /// <summary>Clears whole image.</summary>
        void Clear();

        /// <summary>Clears the image to specific color.</summary>
        /// <param name="color">Color to clear the image..</param>
        void Clear(Color color);

        /// <summary>Clears selected pixel.</summary>
        /// <param name="pixel">The index of the pixel.</param>
        void Clear(int pixel);

        /// <summary>
        /// Initialize the LED driver.
        /// </summary>
        /// <returns></returns>
        bool Initialize();

        /// <summary>Sets pixel at specific position.</summary>
        /// <remarks>The color will be adjusted for brightness.</remarks>
        /// <param name="pixel">The index of the pixel.</param>
        /// <param name="color">Color to set the pixel to.</param>
        /// <param name="brightness">Brightness of the pixel between 0.0 and 1.0.</param>
        void SetPixel(int pixel, Color color, double brightness = -1.0);

        /// <summary>Sets pixel at specific position.</summary>
        /// <remarks>The specified color should be adjusted for brightness.</remarks>
        /// <param name="pixel">The index of the pixel.</param>
        /// <param name="color">Color to set the pixel to.</param>
        void SetPixelFast(int pixel, Color color);

        /// <summary>Sends backing image to the LED driver.</summary>
        void Update();
    }
}
