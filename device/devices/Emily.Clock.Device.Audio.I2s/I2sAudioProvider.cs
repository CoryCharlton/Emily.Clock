using Emily.Clock.Audio;
using Emily.Clock.Device.Audio;

namespace Emily.Clock.Device.Audio.I2s;

/// <summary>
/// Stub I2S audio provider. Manages the device lease and creates <see cref="I2sAudioDevice"/> instances.
/// </summary>
public class I2sAudioProvider : IAudioProvider
{
    private bool _isInitialized;
    private bool _leaseActive;
    private readonly object _leaseLock = new object();
    private readonly I2sAudioOptions _options;

    /// <summary>
    /// Creates a new instance of <see cref="I2sAudioProvider"/>.
    /// </summary>
    public I2sAudioProvider(I2sAudioOptions options)
    {
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

        // TODO: Configure I2S pins using _options.BusId, _options.BckPin, _options.DataPin, _options.WsPin
        // Example:
        // Configuration.SetPinFunction(_options.BckPin, DeviceFunction.I2S1_BCK);
        // Configuration.SetPinFunction(_options.DataPin, DeviceFunction.I2S1_DATA_OUT);
        // Configuration.SetPinFunction(_options.WsPin, DeviceFunction.I2S1_WS);

        _isInitialized = true;

        return true;
    }

    /// <inheritdoc />
    public IAudioDevice Prepare(WavFile wavFile)
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

        return new I2sAudioDevice(wavFile.GetAudioStream(), wavFile.Header, ReleaseLease);
    }

    private void ReleaseLease()
    {
        lock (_leaseLock)
        {
            _leaseActive = false;
        }
    }
}
