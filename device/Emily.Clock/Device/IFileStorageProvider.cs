namespace Emily.Clock.Device;

public interface IFileStorageProvider
{
    bool IsMounted { get; }
    string Root { get; }

    bool Initialize();
}
