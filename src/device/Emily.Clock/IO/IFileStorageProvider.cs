using System;
using System.Text;

namespace Emily.Clock.IO
{
    public interface IFileStorageProvider
    {
        bool IsMounted { get; }

        string[] GetDirectories(string path);
        string[] GetFiles(string path);
        bool Initialize();
    }
}
