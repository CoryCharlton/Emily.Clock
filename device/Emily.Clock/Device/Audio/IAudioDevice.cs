using System;

namespace Emily.Clock.Device.Audio;

/// <summary>
/// Represents an audio output device configured for a single WAV file.
/// </summary>
public interface IAudioDevice : IDisposable
{
    /// <summary>
    /// Gets a value indicating whether the device has been disposed.
    /// </summary>
    bool IsDisposed { get; }

    /// <summary>
    /// Plays the prepared audio file, blocking until playback is complete.
    /// </summary>
    /// <param name="loopCount">The number of times to play the audio. Defaults to 1.</param>
    /// <param name="loopDelayMilliseconds">The delay in milliseconds between loops. Defaults to 0.</param>
    void Play(int loopCount = 1, int loopDelayMilliseconds = 0);
}
