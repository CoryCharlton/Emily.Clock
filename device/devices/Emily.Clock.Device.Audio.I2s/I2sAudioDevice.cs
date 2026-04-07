using System;
using System.Device.I2s;
using System.IO;
using System.Threading;

namespace Emily.Clock.Device.Audio.I2s;

internal delegate void LeaseReleasedCallback();

/// <summary>
/// I2S audio device. Plays audio from a stream by writing chunks to an <see cref="I2sDevice"/>.
/// </summary>
public class I2sAudioDevice : IAudioDevice
{
    // TODO: This feels like it belongs in WavFile
    private const int AudioDataStartOffset = 44;
    private const int BufferSize = 10000;

    private bool _disposed;
    private readonly I2sDevice _i2sDevice;
    private readonly LeaseReleasedCallback _leaseReleasedCallback;
    private readonly Stream _stream;

    internal I2sAudioDevice(Stream stream, I2sDevice i2sDevice, LeaseReleasedCallback leaseReleasedCallback)
    {
        _stream = stream;
        _i2sDevice = i2sDevice;
        _leaseReleasedCallback = leaseReleasedCallback;
    }

    /// <inheritdoc />
    public bool IsDisposed => _disposed;

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _i2sDevice.Dispose();
        _leaseReleasedCallback();
    }

    /// <inheritdoc />
    public void Play(WaitHandle? stopEvent = null)
    {
        if (_disposed)
        {
            return;
        }

        _stream.Seek(AudioDataStartOffset, SeekOrigin.Begin);

        PlayOnce(stopEvent);
    }

    private void PlayOnce(WaitHandle? stopEvent)
    {
        var buffer = new byte[BufferSize];
        var spanBytes = new SpanByte(buffer);
        int length;

        while ((length = _stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            if (stopEvent is not null && stopEvent.WaitOne(0, false))
            {
                break;
            }

            _i2sDevice.Write(length == buffer.Length ? spanBytes : spanBytes.Slice(0, length));
        }
    }
}
