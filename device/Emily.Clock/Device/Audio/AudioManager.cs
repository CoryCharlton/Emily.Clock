using System;
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
    public bool Play(WavFile wavFile, int loopCount = 1, int loopDelayMilliseconds = 0)
    {
        if (_audioProvider is null || !_audioProvider.IsInitialized)
        {
            return false;
        }

        var device = _audioProvider.Prepare(wavFile);
        if (device is null)
        {
            return false;
        }

        try
        {
            // TODO: Do this in a dedicated thread?
            // TODO: Expose a Stop() method?
            device.Play(loopCount, loopDelayMilliseconds);
            return true;
        }
        finally
        {
            device.Dispose();
        }
    }
}
