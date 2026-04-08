using System;
using Emily.Clock.Audio;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.Audio;

[TestClass]
public class WavFileHeaderTests
{
    private static byte[] CreateValidHeader()
    {
        // Minimal valid 44-byte WAV header for a 44100 Hz, 16-bit, stereo PCM file
        var header = new byte[44];

        // RIFF chunk: "RIFF"
        header[0] = (byte)'R'; header[1] = (byte)'I'; header[2] = (byte)'F'; header[3] = (byte)'F';
        // FileSize: 36 + data size (we'll use 0 data)
        header[4] = 36; header[5] = 0; header[6] = 0; header[7] = 0;
        // WAVE format: "WAVE"
        header[8] = (byte)'W'; header[9] = (byte)'A'; header[10] = (byte)'V'; header[11] = (byte)'E';
        // Format chunk: "fmt "
        header[12] = (byte)'f'; header[13] = (byte)'m'; header[14] = (byte)'t'; header[15] = (byte)' ';
        // FormatChunkSize: 16
        header[16] = 16; header[17] = 0; header[18] = 0; header[19] = 0;
        // AudioFormat: 1 (PCM)
        header[20] = 1; header[21] = 0;
        // NumberOfChannels: 2 (stereo)
        header[22] = 2; header[23] = 0;
        // SampleRate: 44100
        header[24] = 0x44; header[25] = 0xAC; header[26] = 0; header[27] = 0;
        // BytesPerSecond: 176400 (44100 * 2 * 16/8)
        header[28] = 0x10; header[29] = 0xB1; header[30] = 0x02; header[31] = 0;
        // BytesPerSampleFrame: 4 (2 channels * 16 bits / 8)
        header[32] = 4; header[33] = 0;
        // BitsPerSample: 16
        header[34] = 16; header[35] = 0;
        // Data chunk: "data"
        header[36] = (byte)'d'; header[37] = (byte)'a'; header[38] = (byte)'t'; header[39] = (byte)'a';
        // DataChunkSize: 0
        header[40] = 0; header[41] = 0; header[42] = 0; header[43] = 0;

        return header;
    }

    [TestMethod]
    public void Constructor_should_parse_valid_header()
    {
        var header = new WavFileHeader(CreateValidHeader());

        Assert.AreEqual((short)1, header.AudioFormat);
        Assert.AreEqual((short)16, header.BitsPerSample);
        Assert.AreEqual((short)4, header.BytesPerSampleFrame);
        Assert.AreEqual(176400, header.BytesPerSecond);
        Assert.AreEqual("data", header.DataChunkId);
        Assert.AreEqual(0, header.DataChunkSize);
        Assert.AreEqual(36, header.FileSize);
        Assert.AreEqual("fmt ", header.FormatChunkId);
        Assert.AreEqual(16, header.FormatChunkSize);
        Assert.AreEqual((short)2, header.NumberOfChannels);
        Assert.AreEqual("RIFF", header.RiffChunkId);
        Assert.AreEqual(44100, header.SampleRate);
        Assert.AreEqual("WAVE", header.WaveFormat);
    }

    [TestMethod]
    public void Constructor_should_throw_for_wrong_length_header()
    {
        Assert.ThrowsException(typeof(ArgumentException), () =>
        {
            new WavFileHeader(new byte[10]);
        });
    }

    [TestMethod]
    public void Constructor_should_throw_for_null_header()
    {
        Assert.ThrowsException(typeof(ArgumentException), () =>
        {
            // ReSharper disable once ObjectCreationAsStatement
            new WavFileHeader(null!);
        });
    }
}
