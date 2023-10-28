using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Emily.Clock.IO;
using Microsoft.Extensions.Logging;
using nanoFramework.Hardware.Esp32;
using nanoFramework.System.IO.FileSystem;

namespace Emily.Clock.App.Hardware
{
    internal class FileStorageProvider: IFileStorageProvider, IDisposable
    {
        private const string Root = @"D:";

        private bool _disposed;
        private readonly ILogger _logger;
        private SDCard _sdCard;
        private readonly object _syncLock = new();

        public FileStorageProvider(ILogger logger)
        {
            _logger = logger;
        }

        ~FileStorageProvider() => Dispose(false);

        public bool IsMounted => _sdCard is not null && _sdCard.IsMounted;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            lock (_syncLock)
            {
                if (_disposed)
                {
                    return;
                }

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Unmount();

                _sdCard?.Dispose();
                _sdCard = null;
            }

            _disposed = true;
        }

        public bool FileExists(string path) => File.Exists(NormalizePath(path));

        public string[] GetDirectories(string path) => Directory.GetDirectories(NormalizePath(path));

        public string[] GetFiles(string path) => Directory.GetFiles(NormalizePath(path));

        public bool Initialize()
        {
            SetPinFunction(2, DeviceFunction.SPI2_MISO);
            SetPinFunction(15, DeviceFunction.SPI2_MOSI);
            SetPinFunction(14, DeviceFunction.SPI2_CLOCK);

            _sdCard ??= new SDCard(new SDCard.SDCardSpiParameters { spiBus = 2, chipSelectPin = 13, enableCardDetectPin = false });

            return Mount();
        }

        private bool Mount()
        {
            if (IsMounted)
            {
                return true;
            }

            try
            {
                _sdCard.Mount();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to mount file storage", e);
            }

            return IsMounted;
        }

        // TODO: Optimize this (can we implement a SpanString?)
        // This isn't very efficient
        private string NormalizePath(string path)
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

            var normalizedPathString = normalizedPath.ToString();
            Debug.WriteLine(normalizedPathString);
            return normalizedPathString;
        }


        public StreamReader OpenText(string path) =>
            new(new FileStream(NormalizePath(path), FileMode.Open, FileAccess.Read));

        public string ReadAllText(string path)
        {
            using var streamReader = OpenText(path);
            return streamReader.ReadToEnd();
        }

        private static void SetPinFunction(int pin, DeviceFunction function)
        {
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(pin, function);
        }



        private void Unmount()
        {
            if (!IsMounted)
            {
                return;
            }

            try
            {
                _sdCard.Unmount();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to unmount file storage", e);
            }
        }
    }
}
