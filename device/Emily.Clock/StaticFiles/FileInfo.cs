using System.IO;
using CCSWE.nanoFramework.FileStorage;
using CCSWE.nanoFramework.WebServer.StaticFiles;

namespace Emily.Clock.StaticFiles
{
    internal class FileInfo: IFileInfo
    {
        private readonly IFileStorage _fileStorage;
        private readonly string _path;

        public FileInfo(string path, IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
            _path = path;

            Exists = fileStorage.Exists(_path);
            Name = Path.GetFileName(_path);
        }

        public Stream CreateReadStream()
        {
            return _fileStorage.OpenRead(_path);
        }

        public bool Exists { get; }
        //TODO: Remove...
        public long Length { get; } = 0;
        public string Name { get; }
    }
}
