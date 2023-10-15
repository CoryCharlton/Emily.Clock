namespace Emily.Clock.IO
{
    internal class FileUtils
    {
        private const uint Kilobyte = 1024;
        private const uint Megabyte = 1024 * 1024;
        private const uint Gigabyte = 1024 * 1024 * 1024;

        // ReSharper disable SimplifyStringInterpolation
        public static string ToSizeString(uint sizeInByes)
        {
            string sizeString;

            if (sizeInByes > Gigabyte)
            {
                var scaledSize = sizeInByes / (double) Gigabyte;
                sizeString = $"{scaledSize.ToString("F")} GB";
            }
            else if (sizeInByes > Megabyte)
            {
                var scaledSize = sizeInByes / (double) Megabyte;
                sizeString = $"{scaledSize.ToString("F")} MB";
            }
            else if (sizeInByes > Kilobyte)
            {
                var scaledSize = sizeInByes / (double) Kilobyte;
                sizeString = $"{scaledSize.ToString("F")} KB";
            }
            else
            {
                sizeString = $"{sizeInByes.ToString("F")} B";
            }

            return sizeString;
        }
        // ReSharper enable SimplifyStringInterpolation

    }
}
