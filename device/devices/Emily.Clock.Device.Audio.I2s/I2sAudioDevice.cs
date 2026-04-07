using System.IO;
using System.Threading;
using Emily.Clock.Audio;

namespace Emily.Clock.Device.Audio.I2s;

internal delegate void LeaseReleasedCallback();

/// <summary>
/// Stub I2S audio device. Reads the audio stream to exercise the playback loop
/// but does not write to hardware until a real I2S driver is wired in.
/// </summary>
public class I2sAudioDevice : IAudioDevice
{
    private const int AudioDataStartOffset = 44;
    private const int BufferSize = 4096;

    private bool _disposed;
    private readonly LeaseReleasedCallback _leaseReleasedCallback;
    private readonly Stream _stream;

    internal I2sAudioDevice(Stream stream, WavFileHeader header, LeaseReleasedCallback leaseReleasedCallback)
    {
        _stream = stream;
        _leaseReleasedCallback = leaseReleasedCallback;

        // TODO: Configure I2S device using header.SampleRate, header.BitsPerSample, header.NumberOfChannels
        _ = header;
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
        _leaseReleasedCallback();
    }

    /// <inheritdoc />
    public void Play(int loopCount = 1, int loopDelayMilliseconds = 0)
    {
        if (_disposed)
        {
            return;
        }

        for (var i = 0; i < loopCount; i++)
        {
            if (i > 0)
            {
                if (loopDelayMilliseconds > 0)
                {
                    Thread.Sleep(loopDelayMilliseconds);
                }

                _stream.Seek(AudioDataStartOffset, SeekOrigin.Begin);
            }

            PlayOnce();
        }
    }

    private void PlayOnce()
    {
        var buffer = new byte[BufferSize];
        int length;

        while ((length = _stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            // TODO: Write to I2S device
            // var spanBytes = new SpanByte(buffer);
            // _i2sDevice.Write(length == buffer.Length ? spanBytes : spanBytes.Slice(0, length));
            _ = length;
        }
    }
}
