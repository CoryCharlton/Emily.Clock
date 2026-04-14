using System;
using System.IO;
using CCSWE.nanoFramework.FileStorage;
using CCSWE.nanoFramework.WebServer.StaticFiles;
using Emily.Clock.Device.FileStorage;
using Emily.Clock.IO;

namespace Emily.Clock.StaticFiles;

internal class FileProvider : IFileProvider
{
    private const string Root = @"\www";

    private readonly IFileStorage _fileStorage;
    private readonly IFileStorageProvider? _fileStorageProvider;

    public FileProvider(IFileStorage fileStorage, IServiceProvider serviceProvider)
    {
        _fileStorage = fileStorage;
        _fileStorageProvider = (IFileStorageProvider)serviceProvider.GetService(typeof(IFileStorageProvider));
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        if (string.IsNullOrEmpty(subpath) || subpath.Equals("/"))
        {
            // TODO: Switch this based on wireless mode? After thinking about this the wireless configuration page needs to be available 
            // even if the device does not have an SD card or other file storage available. This is because the wireless configuration page
            // is used to configure the wireless settings. Maybe all the pages need to be available even if the device does not have an SD card
            // or other file storage available. This is because the device needs to be able to serve the pages even if the file storage is not
            // available. Maybe all the pages need to be embedded resources instead of being served from file storage?
            subpath = "/index.html";
        }

        if (subpath.StartsWith("/"))
        {
            subpath = subpath.Substring(1);
        }

        var storageRoot = _fileStorageProvider?.Root ?? string.Empty;
        var path = FileUtils.NormalizePath(storageRoot, Path.Combine(Root, subpath));

        return new FileInfo(path, _fileStorage);
    }
}
