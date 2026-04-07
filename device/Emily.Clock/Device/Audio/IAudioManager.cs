using Emily.Clock.Audio;

namespace Emily.Clock.Device.Audio;

/// <summary>
/// High-level API for playing WAV audio files.
/// </summary>
public interface IAudioManager
{
    /// <summary>
    /// Plays the given WAV file, blocking until playback is complete.
    /// Returns <see langword="false"/> if no audio hardware is available or a lease is already active.
    /// </summary>
    /// <param name="wavFile">The WAV file to play.</param>
    /// <param name="loopCount">The number of times to play the audio. Defaults to 1.</param>
    /// <param name="loopDelayMilliseconds">The delay in milliseconds between loops. Defaults to 0.</param>
    bool Play(WavFile wavFile, int loopCount = 1, int loopDelayMilliseconds = 0);
}
