using Emily.Clock.Device.Gpio;
using Emily.Clock.Mediator.Events;
using Emily.Clock.Networking;
using MakoIoT.Device.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.UI.Windows
{
    public class ConfigurationWindow: Window, IEventHandler
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
            Controls.DrawContent(screen, $"SSID: {_networkManager.Ssid}", $"Address: http://{_networkManager.IpAddress}");
            Controls.DrawLogo(screen, Resources.BitmapResources.Wireless_48);

            Controls.DrawButton(screen, Button.One, Resources.BitmapResources.Restart_22);

            screen.Flush();
        }

        public void Handle(IEvent mediatorEvent)
        {
            if (mediatorEvent is not ButtonEvent buttonEvent)
            {
                return;
            }

            if (ButtonEventType.Press == buttonEvent.Type && Button.One == buttonEvent.Button)
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
