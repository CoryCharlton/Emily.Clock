using System.Drawing;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Display;
using Emily.Clock.Mediator.Events;
using Microsoft.Extensions.Logging;
using nanoFramework.UI;

namespace Emily.Clock.UI
{
    public interface IStatusService
    {
        bool SuppressEvents { get; set; }
    }

    public class StatusService : IStatusService, IMediatorEventHandler
    {
        private readonly IDisplayManager _displayManager;
        private readonly ILogger _logger;

        public StatusService(IDisplayManager displayManager, ILogger logger)
        {
            _displayManager = displayManager;
            _logger = logger;
        }

        public bool SuppressEvents { get; set; }

        public void HandleEvent(IMediatorEvent mediatorEvent)
        {
            if (mediatorEvent is not StatusEvent statusMessageEvent)
            {
                return;
            }

            if (SuppressEvents)
            {
                _logger.LogDebug(statusMessageEvent.Message);

                return;
            }

            if (!_displayManager.IsInitialized)
            {
                return;
            }

            var font = Theme.SmallFont;
            var padding = new Padding(5, 5);

            var source = new Bitmap(_displayManager.Width, font.Height + padding.Vertical);
            source.DrawText(statusMessageEvent.Message, font, Theme.SecondaryText, ContentAlignment.BottomLeft, padding);

            var screen = _displayManager.GetBitmap();
            screen.DrawImage(source, ContentAlignment.BottomLeft);
            screen.Flush();

            // TODO: Handle timeout
        }
    }
}
