// Using https://github.com/markwhitaker/MimeTypes.NET for reference

using Emily.Clock.IO;

namespace Emily.Clock.Http
{
    public static class MimeType
    {
        public static class Application
        {
            public const string Json = "application/json";
            public const string Octet = "application/octet-stream";
            public const string Xml = "application/xml";
            public const string Zip = "application/zip";
        }

        public static class Image
        {
            public const string Gif = "image/gif";
            public const string Jpeg = "image/jpeg";
            public const string Png = "image/Png";
        }

        public static class Text
        {
            public const string Html = "text/html";
            public const string JavaScript = "text/javascript";
            public const string Plain = "text/plain";
        }

        public static string GetMimeTypeFromFileName(string fileName)
        {
            var extension = FileUtils.GetFileExtension(fileName);

            return extension switch
            {
                ".gif" => Image.Gif,
                ".html" => Text.Html,
                ".jpeg" => Image.Jpeg,
                ".jpg" => Image.Jpeg,
                ".js" => Text.JavaScript,
                ".json" => Application.Json,
                ".png" => Image.Png,
                ".txt" => Text.Plain,
                ".xml" => Application.Xml,
                ".zip" => Application.Zip,
                _ => Application.Octet
            };
        }
    }
}
