namespace Emily.Clock.IO;

public interface IFileStorageManager
{
    bool IsMounted { get; }
    string Root { get; }

    bool Initialize();

    string NormalizePath(string path);
}