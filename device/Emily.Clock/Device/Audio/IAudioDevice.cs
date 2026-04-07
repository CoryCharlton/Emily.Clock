using System;
using System.Threading;

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
    /// Seeks to the start of the audio data and plays once, blocking until complete or <paramref name="stopEvent"/> is signaled.
    /// </summary>
    /// <param name="stopEvent">Optional wait handle that, when signaled, stops playback early.</param>
    void Play(WaitHandle? stopEvent = null);
}
