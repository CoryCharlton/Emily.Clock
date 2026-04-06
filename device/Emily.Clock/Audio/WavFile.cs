using System;
using System.IO;

namespace Emily.Clock.Audio;

/// <summary>
/// Parses a WAV file from a stream and provides access to the header and audio data.
/// </summary>
public class WavFile : IDisposable
{
    private readonly Stream _stream;

    /// <summary>
    /// Creates a new instance of <see cref="WavFile" />, reading and parsing the WAV header from <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The stream to read from. <see cref="WavFile" /> takes ownership and will dispose it.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="stream"/> is null.</exception>
    public WavFile(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        _stream = stream;

        var headerBytes = new byte[44];
        _stream.Read(headerBytes, 0, 44);

        Header = new WavFileHeader(headerBytes);
    }

    /// <summary>
    /// The parsed WAV file header.
    /// </summary>
    public WavFileHeader Header { get; }

    /// <summary>
    /// Returns the underlying stream positioned at the start of the audio data (just past the 44-byte header).
    /// </summary>
    /// <returns>The audio data stream. Do not dispose — <see cref="WavFile" /> owns the stream.</returns>
    public Stream GetAudioStream()
    {
        return _stream;
    }

    /// <inheritdoc />
    public void Dispose() => _stream.Dispose();
}
