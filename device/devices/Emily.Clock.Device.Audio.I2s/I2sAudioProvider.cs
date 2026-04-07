using System;
using System.Device.I2s;
using Emily.Clock.Audio;
using Emily.Clock.Device;
using Emily.Clock.Device.Audio;
using Emily.Clock.Device.Gpio;

namespace Emily.Clock.Device.Audio.I2s;

/// <summary>
/// I2S audio provider. Calls the pre-initialize delegate for pin configuration, then creates
/// <see cref="I2sAudioDevice"/> instances with a single-lease mechanism.
/// </summary>
public class I2sAudioProvider : IAudioProvider
{
    private readonly IGpioProvider _gpioProvider;
    private bool _isInitialized;
    private bool _leaseActive;
    private readonly object _leaseLock = new object();
    private readonly I2sAudioOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="I2sAudioProvider"/>.
    /// </summary>
    public I2sAudioProvider(IGpioProvider gpioProvider, I2sAudioOptions options)
    {
        _gpioProvider = gpioProvider;
        _options = options;
    }

    /// <inheritdoc />
    public bool IsInitialized => _isInitialized;

    /// <inheritdoc />
    public bool Initialize()
    {
        if (_isInitialized)
        {
            return true;
        }

        _options.PreInitialize?.Invoke(_gpioProvider);

        _isInitialized = true;

        return true;
    }

    /// <inheritdoc />
    public IAudioDevice? Prepare(WavFile wavFile)
    {
        if (!_isInitialized)
        {
            return null;
        }

        lock (_leaseLock)
        {
            if (_leaseActive)
            {
                return null;
            }

            _leaseActive = true;
        }

        var header = wavFile.Header;
        var i2sDevice = new I2sDevice(new I2sConnectionSettings(_options.BusId)
        {
            Mode = I2sMode.Master | I2sMode.Tx | I2sMode.Pdm,
            CommunicationFormat = I2sCommunicationFormat.I2S,
            SampleRate = header.SampleRate,
            BitsPerSample = ToBitsPerSample(header.BitsPerSample),
            ChannelFormat = ToChannelFormat(header.NumberOfChannels),
            BufferSize = 40000,
        });

        return new I2sAudioDevice(wavFile.GetAudioStream(), i2sDevice, ReleaseLease);
    }

    private void ReleaseLease()
    {
        lock (_leaseLock)
        {
            _leaseActive = false;
        }
    }

    private static I2sBitsPerSample ToBitsPerSample(short bitsPerSample)
    {
        return bitsPerSample switch
        {
            8 => I2sBitsPerSample.Bit8,
            16 => I2sBitsPerSample.Bit16,
            24 => I2sBitsPerSample.Bit24,
            32 => I2sBitsPerSample.Bit32,
            _ => throw new ArgumentOutOfRangeException(nameof(bitsPerSample), "Only 8, 16, 24, or 32 bits per sample are supported.")
        };
    }

    private static I2sChannelFormat ToChannelFormat(short numberOfChannels)
    {
        return numberOfChannels switch
        {
            1 => I2sChannelFormat.OnlyLeft,
            2 => I2sChannelFormat.RightLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(numberOfChannels), "Only mono and stereo WAV files are supported.")
        };
    }
}
