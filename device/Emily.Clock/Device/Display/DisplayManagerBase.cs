using System;
using nanoFramework.Runtime.Native;
using nanoFramework.UI;

namespace Emily.Clock.Device.Display;

public abstract class DisplayManagerBase : IDisplayManager
{
    protected readonly DisplayOptions _options;
    private bool _initialized;

    protected DisplayManagerBase(DisplayOptions options)
    {
        _options = options;
    }

    public ushort Height => _options.Height;
    public bool IsInitialized => _initialized;
    public ushort Width => _options.Width;

    private void CheckInitialized()
    {
        if (!_initialized)
        {
            throw new InvalidOperationException("Display is not initialized");
        }
    }

    public void Clear(bool flush = true)
    {
        CheckInitialized();

        var bitmap = GetBitmap();

        bitmap.Clear();

        if (flush)
        {
            bitmap.Flush();
        }
    }

    public Bitmap GetBitmap()
    {
        CheckInitialized();

        return DisplayControl.FullScreen;
    }

    protected virtual uint GetBufferSize() => (uint)(Height * Width * 16);

    public bool Initialize()
    {
        if (_initialized)
        {
            return true;
        }

        _initialized = InitializeDisplay();

        if (_initialized)
        {
            Clear();
            SetBackLight(true);

            Power.OnRebootEvent += OnReboot;
        }

        return _initialized;
    }

    protected abstract bool InitializeDisplay();

    private void OnReboot()
    {
        if (!IsInitialized)
        {
            return;
        }

        Clear();
        SetBackLight(false);
    }

    public abstract void SetBackLight(bool enabled);
}
