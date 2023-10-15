using nanoFramework.UI;

namespace Emily.Clock.UI
{
    public interface IDisplayManager
    {
        ushort Height { get; }
        bool IsInitialized { get; }
        ushort Width { get; }

        void Clear(bool flush = true);
        Bitmap GetBitmap();
        bool Initialize();
        void SetBackLight(bool enabled);
    }
}