using System.Threading;
using Emily.Clock.Audio;

namespace Emily.Clock.Device.Audio;

/// <summary>
/// High-level API for playing WAV audio files.
/// </summary>
public interface IAudioManager
{
    /// <summary>
    /// Prepares an <see cref="IAudioDevice"/> for the given WAV file.
    /// Returns <see langword="null"/> if no audio hardware is available or a lease is already active.
    /// The caller is responsible for disposing the returned device.
    /// </summary>
    /// <param name="wavFile">The WAV file to prepare for playback.</param>
    IAudioDevice? Prepare(WavFile wavFile);

    /// <summary>
    /// Plays the given WAV file once, blocking until complete or <paramref name="stopEvent"/> is signaled.
    /// Returns <see langword="false"/> if no audio hardware is available or a lease is already active.
    /// </summary>
    /// <param name="wavFile">The WAV file to play.</param>
    /// <param name="stopEvent">Optional wait handle that, when signaled, stops playback early.</param>
    bool Play(WavFile wavFile, WaitHandle? stopEvent = null);
}
