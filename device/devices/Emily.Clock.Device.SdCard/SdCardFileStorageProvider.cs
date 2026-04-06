using System;
using System.Diagnostics.CodeAnalysis;
using Emily.Clock.Device.FileStorage;
using Emily.Clock.Device.Gpio;
using Microsoft.Extensions.Logging;
using nanoFramework.System.IO.FileSystem;

namespace Emily.Clock.Device.SdCard;

internal class SdCardFileStorageProvider : IFileStorageProvider, IDisposable
{
    private bool _disposed;
    private readonly IGpioProvider _gpioProvider;
    private readonly ILogger _logger;
    private readonly SDCardMmcParameters? _mmcParameters;
    private readonly DevicePreInitializeDelegate? _preInitialize;
    private SDCard? _sdCard;
    private readonly SDCardSpiParameters? _spiParameters;
    private readonly object _syncLock = new();

    public SdCardFileStorageProvider(IGpioProvider gpioProvider, ILogger logger, SdCardOptions options)
    {
        _gpioProvider = gpioProvider;
        _logger = logger;
        _mmcParameters = options.MmcParameters;
        _preInitialize = options.PreInitialize;
        _spiParameters = options.SpiParameters;

        Root = (char)('D' + GetSlotIndex()) + ":";
    }

    ~SdCardFileStorageProvider() => Dispose(false);

    public bool IsMounted => _sdCard is not null && _sdCard.IsMounted;

    public string Root { get; }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (_syncLock)
        {
            if (_disposed)
            {
                return;
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Unmount();

            _sdCard?.Dispose();
            _sdCard = null;
        }

        _disposed = true;
    }

    private uint GetSlotIndex()
    {
        if (_mmcParameters is not null)
        {
            return _mmcParameters.slotIndex;
        }

        if (_spiParameters is not null)
        {
            return _spiParameters.slotIndex;
        }

        throw new InvalidOperationException("No SD card parameters provided");
    }

    [MemberNotNullWhen(true, nameof(_sdCard))]
    public bool Initialize()
    {
        _preInitialize?.Invoke(_gpioProvider);

        if (_spiParameters is not null)
        {
            _sdCard ??= new SDCard(_spiParameters);
        }
        else if (_mmcParameters is not null)
        {
            _sdCard ??= new SDCard(_mmcParameters);
        }
        else
        {
            return false;
        }

        return Mount();
    }

    private bool Mount()
    {
        if (IsMounted)
        {
            return true;
        }

        try
        {
            _sdCard?.Mount();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Failed to mount file storage", e);
        }

        return IsMounted;
    }

    private void Unmount()
    {
        if (!IsMounted)
        {
            return;
        }

        try
        {
            _sdCard?.Unmount();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Failed to unmount file storage", e);
        }
    }
}
