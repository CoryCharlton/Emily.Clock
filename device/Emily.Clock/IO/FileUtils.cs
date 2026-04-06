using System;
using System.Text;

namespace Emily.Clock.IO;

internal class FileUtils
{
    private const uint Kilobyte = 1024;
    private const uint Megabyte = 1024 * 1024;
    private const uint Gigabyte = 1024 * 1024 * 1024;

    // TODO: Optimize this (can we implement a SpanString?)
    // This isn't very efficient
    public static string NormalizePath(string root, string path)
    {
        if (path.StartsWith(root))
        {
            return path.Contains("/") ? path.Replace("/", @"\") : path;
        }

        var colonIndex = path.IndexOf(':');
        var normalizedPath = new StringBuilder(colonIndex == -1 ? path : path.Substring(colonIndex + 1, path.Length - 1 - colonIndex));

        normalizedPath.Insert(0, root + @"\", 1);
        normalizedPath.Replace("/", @"\");
        normalizedPath.Replace(@"\\", @"\");

        return normalizedPath.ToString();
    }

    // ReSharper disable SimplifyStringInterpolation
    public static string ToSizeString(uint sizeInByes)
    {
        switch (sizeInByes)
        {
            case > Gigabyte:
            {
                var scaledSize = sizeInByes / (double) Gigabyte;
                return $"{scaledSize.ToString("F")} GB";
            }
            case > Megabyte:
            {
                var scaledSize = sizeInByes / (double) Megabyte;
                return $"{scaledSize.ToString("F")} MB";
            }
            case > Kilobyte:
            {
                var scaledSize = sizeInByes / (double) Kilobyte;
                return $"{scaledSize.ToString("F")} KB";
            }
            default:
                return $"{sizeInByes.ToString("F")} B";
        }
    }
    // ReSharper enable SimplifyStringInterpolation
}