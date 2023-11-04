using System.IO;

namespace Emily.Clock.IO
{
    public interface IFileStorageProvider
    {
        bool IsMounted { get; }

        bool FileExists(string path);
        string[] GetDirectories(string path);
        string[] GetFiles(string path);
        bool Initialize();
        StreamReader OpenText(string path);
        string ReadAllText(string path);
    }
}
