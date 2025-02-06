using System.IO;
using CCSWE.nanoFramework.FileStorage;
using CCSWE.nanoFramework.WebServer.StaticFiles;
using Emily.Clock.IO;

namespace Emily.Clock.StaticFiles
{
    internal class FileProvider: IFileProvider
    {
        private const string Root = @"\www";

        private readonly IFileStorage _fileStorage;
        private readonly IFileStorageManager _fileStorageManager;

        public FileProvider(IFileStorage fileStorage, IFileStorageManager fileStorageManager)
        {
            _fileStorage = fileStorage;
            _fileStorageManager = fileStorageManager;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath) || subpath.Equals("/"))
            {
                // TODO: Switch this based on wireless mode?
                subpath = "/index.html";
            }

            var path = _fileStorageManager.NormalizePath(Path.Combine(Root, subpath));

            return new FileInfo(path, _fileStorage);
        }
    }
}
