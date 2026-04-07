using Emily.Clock.Audio;

namespace Emily.Clock.Device.Audio;

/// <summary>
/// Manages audio hardware initialization and creates <see cref="IAudioDevice"/> instances.
/// Only one device lease may be active at a time.
/// </summary>
public interface IAudioProvider
{
    /// <summary>
    /// Gets a value indicating whether the provider has been initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes the audio hardware.
    /// </summary>
    /// <returns>true if successful; otherwise false.</returns>
    bool Initialize();

    /// <summary>
    /// Creates an <see cref="IAudioDevice"/> configured for the given WAV file.
    /// Returns <see langword="null"/> if a lease is already active or the provider is not initialized.
    /// Disposing the returned device releases the lease.
    /// </summary>
    /// <param name="wavFile">The WAV file to prepare for playback.</param>
    IAudioDevice? Prepare(WavFile wavFile);
}
