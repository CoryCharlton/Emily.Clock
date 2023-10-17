using System;

namespace Emily.Clock.IO
{
    internal class FileUtils
    {
        private const uint Kilobyte = 1024;
        private const uint Megabyte = 1024 * 1024;
        private const uint Gigabyte = 1024 * 1024 * 1024;

        public static string GetFileExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException();
            }

            return fileName.Substring(fileName.LastIndexOf('.')).ToLower();
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
}
