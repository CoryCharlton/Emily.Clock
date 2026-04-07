using System;
using System.Threading;
using Emily.Clock.Audio;

namespace Emily.Clock.Device.Audio;

/// <summary>
/// High-level audio playback manager. Acquires a device lease, plays audio, then releases the lease.
/// </summary>
public class AudioManager : IAudioManager
{
    private readonly IAudioProvider? _audioProvider;

    /// <summary>
    /// Creates a new instance of <see cref="AudioManager"/>.
    /// </summary>
    /// <param name="serviceProvider">Used to resolve an optional <see cref="IAudioProvider"/>.</param>
    public AudioManager(IServiceProvider serviceProvider)
    {
        _audioProvider = (IAudioProvider)serviceProvider.GetService(typeof(IAudioProvider));
    }

    /// <inheritdoc />
    public IAudioDevice? Prepare(WavFile wavFile)
    {
        if (_audioProvider is null || !_audioProvider.IsInitialized)
        {
            return null;
        }

        return _audioProvider.Prepare(wavFile);
    }

    /// <inheritdoc />
    public bool Play(WavFile wavFile, WaitHandle? stopEvent = null)
    {
        var device = Prepare(wavFile);
        if (device is null)
        {
            return false;
        }

        try
        {
            device.Play(stopEvent);
            return true;
        }
        finally
        {
            device.Dispose();
        }
    }
}
