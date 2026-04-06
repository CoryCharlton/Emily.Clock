using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Display;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Mediator.Events;
using Emily.Clock.Networking;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.UI.Windows
{
    public class ConfigurationWindow: Window, IMediatorEventHandler
    {
        private readonly IMediator _mediator;
        private readonly IWirelessNetworkManager _networkManager;

        public ConfigurationWindow(IDisplayManager displayManager, ILogger logger, IMediator mediator, IWirelessNetworkManager networkManager) : base(displayManager, logger)
        {
            _mediator = mediator;
            _networkManager = networkManager;
        }

        private void Draw()
        {
            var screen = GetBitmap();
            screen.Clear();

            Controls.DrawTitle(screen, "Configuration Mode");
            Controls.DrawContent(screen, $"SSID: {_networkManager.Ssid}", $"Address: {_networkManager.IpAddress}");
            Controls.DrawLogo(screen, Resources.BitmapResources.Wireless_48);

            Controls.DrawButton(screen, Button.One, Resources.BitmapResources.Restart_22);

            screen.Flush();
        }

        public void HandleEvent(IMediatorEvent mediatorEvent)
        {
            if (mediatorEvent is not ButtonEvent buttonEvent)
            {
                return;
            }

            if (buttonEvent is { Type: ButtonEventType.Press, Button: Button.One })
            {
                _networkManager.SetMode(WirelessMode.Client);
            }
        }

        protected override void OnStart()
        {
            _mediator.Subscribe(typeof(ButtonEvent), this);

            Draw();
        }

        protected override void OnStop()
        {
            _mediator.Unsubscribe(typeof(ButtonEvent), this);
        }
    }
}
