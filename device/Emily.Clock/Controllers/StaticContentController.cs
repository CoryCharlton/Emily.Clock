using MakoIoT.Device.Services.Server.WebServer;
using System;
using System.Net;
using Emily.Clock.IO;

namespace Emily.Clock.Controllers
{
    // TODO: I don't like that I need to register a route for every static file here.
    // TODO: Look into adding a StaticContentDelegate to IServer that will check the file system when a route isn't matched
    public class StaticContentController: ControllerBase
    {
        private const string Root = @"\www\";
        private readonly IFileStorageProvider _fileStorageProvider;

        public StaticContentController(IFileStorageProvider fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;
        }

        [Route("")]
        [Route("index.html")]
        [Method("GET")]
        public void GetIndex(WebServerEventArgs e)
        {
            RespondWithFileContent(e.Context.Response, "index.html");
        }

        /*
        [Method("GET")]
        public void GetRoot(WebServerEventArgs e)
        {
            RespondWithFileContent(e.Context.Response, "index.html");
        }
        */

        private string ReadFileContent(string path)
        {
            var fileStoragePath = Root + path;
            if (!_fileStorageProvider.IsMounted)
            {
                return null;
            }

            /* At this point it's more efficient just to catch any exception...
            if (!_fileStorageProvider.FileExists(fileStoragePath))
            {
                return null;
            }
            */

            try
            {
                return _fileStorageProvider.ReadAllText(fileStoragePath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void RespondWithFileContent(HttpListenerResponse response, string path)
        {
            var content = ReadFileContent(path);
            if (string.IsNullOrEmpty(content))
            {
                NotFound(response);
            }
            else
            {
                // TODO: Need to handle mime type
                Ok(response, content);
            }
        }
    }
}
