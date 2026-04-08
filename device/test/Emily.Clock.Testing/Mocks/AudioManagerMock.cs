using System.Threading;
using Emily.Clock.Audio;
using Emily.Clock.Device.Audio;

namespace Emily.Clock.Testing.Mocks;

public class AudioManagerMock : IAudioManager
{
    public bool PlayCalled { get; private set; }
    public bool PlayResult { get; set; } = true;
    public WavFile? LastPlayedFile { get; private set; }

    public IAudioDevice? Prepare(WavFile wavFile) => null;

    public bool Play(WavFile wavFile, WaitHandle? stopEvent = null)
    {
        LastPlayedFile = wavFile;
        PlayCalled = true;

        return PlayResult;
    }
}
