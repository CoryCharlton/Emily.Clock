using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using nanoFramework.UI;

namespace Emily.Clock.UI.Windows
{
    public interface IWindow : IDisposable
    {
        void Start();
        void Stop();
    }

    public abstract class Window : IWindow
    {
        private bool _disposed;

        protected readonly object SyncLock = new();

        protected Window(IDisplayManager displayManager, ILogger logger)
        {
            CancellationToken = CancellationTokenSource.Token;
            DisplayManager = displayManager;
            Logger = logger;
        }

        ~Window() => Dispose(false);

        protected CancellationToken CancellationToken { get; }

        protected CancellationTokenSource CancellationTokenSource { get; } = new();

        private IDisplayManager DisplayManager { get; }

        protected ILogger Logger { get; }

        protected void ClearDisplay(bool flush = false)
        {
            DisplayManager.Clear(flush);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            lock (SyncLock)
            {
                if (_disposed)
                {
                    return;
                }

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Stop();
            }

            OnDispose(disposing);

            _disposed = true;
        }

        protected Bitmap GetBitmap() => DisplayManager.GetBitmap();

        protected virtual void OnDispose(bool disposing)
        {

        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnStop()
        {

        }

        public void Start()
        {
            OnStart();
        }

        public void Stop()
        {
            CancellationTokenSource.Cancel();

            OnStop();
        }
    }
}
