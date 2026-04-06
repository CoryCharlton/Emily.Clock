using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device.Display;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Mediator.Events;
using Emily.Clock.Networking;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.UI.Windows
{
    public class NetworkFailureWindow : Window, IMediatorEventHandler
    {
        private readonly IMediator _mediator;
        private readonly IWirelessNetworkManager _networkManager;

        // TODO: Add a timer to automatically reboot in client mode if no response. Timer should tick down and display in the status bar
        // TODO: If RTC exists and has valid time the reboot is not necessary
        public NetworkFailureWindow(IDisplayManager displayManager, ILogger logger, IMediator mediator, IWirelessNetworkManager networkManager) : base(displayManager, logger)
        {
            _mediator = mediator;
            _networkManager = networkManager;
        }

        private void Draw()
        {
            var screen = GetBitmap();
            screen.Clear();

            Controls.DrawTitle(screen, "Connection Failed");
            Controls.DrawContent(screen, "Reboot or Setup");
            Controls.DrawLogo(screen, Resources.BitmapResources.Wireless_48);

            Controls.DrawButton(screen, Button.One, Resources.BitmapResources.Restart_22);
            Controls.DrawButton(screen, Button.Three, Resources.BitmapResources.Settings_22);

            screen.Flush();
        }

        public void HandleEvent(IMediatorEvent mediatorEvent)
        {
            if (mediatorEvent is not ButtonEvent buttonEvent)
            {
                return;
            }

            if (ButtonEventType.Press != buttonEvent.Type)
            {
                return;
            }

            switch (buttonEvent.Button)
            {
                case Button.One:
                    _networkManager.SetMode(WirelessMode.Client);
                    break;
                case Button.Three:
                    _networkManager.SetMode(WirelessMode.AccessPoint);
                    break;
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
