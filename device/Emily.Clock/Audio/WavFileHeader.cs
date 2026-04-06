using System;
using System.Text;

namespace Emily.Clock.Audio;

/// <summary>
/// This struct can be used to parse a WAV file header.
/// </summary>
/// <remarks>
/// For WAV header specification see <see href="https://docs.fileformat.com/audio/wav/" />.
/// </remarks>
public struct WavFileHeader
{
    /// <summary>
    /// Creates a new instance of <see cref="WavFileHeader" /> based on the provided header bytes.
    /// </summary>
    /// <param name="header">The header bytes to parse.</param>
    /// <exception cref="ArgumentException">Throws if the header array does not contain exactly 44 bytes.</exception>
    public WavFileHeader(byte[] header)
    {
        if (header is not { Length: 44 })
        {
            throw new ArgumentException("Only WAV file headers with 44 bytes are supported.");
        }

        AudioFormat = BitConverter.ToInt16(header, 20);
        BitsPerSample = BitConverter.ToInt16(header, 34);
        BytesPerSampleFrame = BitConverter.ToInt16(header, 32);
        BytesPerSecond = BitConverter.ToInt32(header, 28);
        DataChunkId = Encoding.UTF8.GetString(header, 36, 4);
        DataChunkSize = BitConverter.ToInt32(header, 40);
        FileSize = BitConverter.ToInt32(header, 4);
        FormatChunkId = Encoding.UTF8.GetString(header, 12, 4);
        FormatChunkSize = BitConverter.ToInt32(header, 16);
        NumberOfChannels = BitConverter.ToInt16(header, 22);
        RiffChunkId = Encoding.UTF8.GetString(header, 0, 4);
        SampleRate = BitConverter.ToInt32(header, 24);
        WaveFormat = Encoding.UTF8.GetString(header, 8, 4);
    }

    /// <summary>
    /// AudioFormat: Indicates how the sample data for the wave file is stored.
    /// The most common format is integer PCM, which has a code of 1.
    /// <para>
    /// Other formats include:
    /// <list type="bullet">
    /// <item><term>2</term><description>ADPCM</description></item>
    /// <item><term>3</term><description>floating point PCM</description></item>
    /// <item><term>6</term><description>A-law</description></item>
    /// <item><term>7</term><description>μ-law</description></item>
    /// <item><term>65534</term><description>WaveFormatExtensible</description></item>
    /// </list>
    /// </para>
    /// </summary>
    public short AudioFormat { get; }

    /// <summary>
    /// Bits per sample
    /// <para>
    /// For integer PCM data, typical values will be 8, 16, or 32.
    /// </para>
    /// </summary>
    public short BitsPerSample { get; }

    /// <summary>
    /// Block align: the number of bytes required to store a single sample frame across all channels.
    /// <para>
    /// Equals <c>BitsPerSample × NumberOfChannels / 8</c>.
    /// </para>
    /// </summary>
    public short BytesPerSampleFrame { get; }

    /// <summary>
    /// Byte rate: the number of bytes required for one second of audio data.
    /// <para>
    /// Equals <c>SampleRate × BitsPerSample × NumberOfChannels / 8</c>.
    /// For example, 44100 Hz × 16-bit × stereo / 8 = 176400.
    /// </para>
    /// </summary>
    public int BytesPerSecond { get; }

    /// <summary>
    /// Marks the start of the data chunk.
    /// <para>
    /// Should always be "data" for PCM WAV files.
    /// </para>
    /// </summary>
    public string DataChunkId { get; }

    /// <summary>
    /// Data chunk size
    /// <para>
    /// The size of the chunk data.
    /// </para>
    /// </summary>
    public int DataChunkSize { get; }

    /// <summary>
    /// The total file size minus 8 bytes (excludes the RIFF chunk identifier and this field itself).
    /// </summary>
    public int FileSize { get; }

    /// <summary>
    /// Format chunk marker (4 characters).
    /// Includes trailing <see cref="string.Empty" />.
    /// </summary>
    public string FormatChunkId { get; }

    /// <summary>
    /// Length of format data. Typically 16 for PCM.
    /// </summary>
    public int FormatChunkSize { get; }

    /// <summary>
    /// Number of channels: Typically a file will have 1 channel (mono) or 2 channels (stereo).
    /// A 5.1 surround sound file will have 6 channels.
    /// </summary>
    public short NumberOfChannels { get; }

    /// <summary>
    /// Marks the file as a riff file.
    /// Characters are each 1 byte long.
    /// </summary>
    public string RiffChunkId { get; }

    /// <summary>
    /// Sample Rate: The number of sample frames that occur each second.
    /// A typical value would be 44100, which is the same as an audio CD.
    /// </summary>
    public int SampleRate { get; }

    /// <summary>
    /// File Type Header.
    /// For our purposes, it must always equal to "WAVE".
    /// </summary>
    public string WaveFormat { get; }
}
