namespace Emily.Clock.Device.FileStorage;

public interface IFileStorageProvider
{
    bool IsMounted { get; }
    string Root { get; }

    bool Initialize();
}
