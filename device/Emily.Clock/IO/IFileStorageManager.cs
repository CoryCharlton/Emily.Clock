using System;
using System.Text;

namespace Emily.Clock.IO
{
    public interface IFileStorageManager
    {
        bool IsMounted { get; }
        string Root => @"D:";

        bool Initialize();

        // TODO: Optimize this (can we implement a SpanString?)
        // This isn't very efficient
        public string NormalizePath(string path)
        {
            if (path.StartsWith(Root))
            {
                return path.Contains("/") ? path.Replace("/", @"\") : path;
            }

            var colonIndex = path.IndexOf(':');
            var normalizedPath = new StringBuilder(colonIndex == -1 ? path : path.Substring(colonIndex + 1, path.Length - 1 - colonIndex));

            normalizedPath.Insert(0, Root + @"\", 1);
            normalizedPath.Replace("/", @"\");
            normalizedPath.Replace(@"\\", @"\");

            return normalizedPath.ToString();
        }
    }
}
