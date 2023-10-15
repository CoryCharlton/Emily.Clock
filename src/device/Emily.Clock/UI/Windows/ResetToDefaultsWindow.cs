using Emily.Clock.Device;
using Emily.Clock.Device.Gpio;
using Emily.Clock.Mediator.Events;
using MakoIoT.Device.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.UI.Windows
{
    public class ResetToDefaultsWindow : Window, IEventHandler
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IMediator _mediator;

        // TODO: Add a timer to automatically reboot if no response. Timer should tick down and display in the status bar
        public ResetToDefaultsWindow(IDeviceManager deviceManager, IDisplayManager displayManager, ILogger logger, IMediator mediator) : base(displayManager, logger)
        {
            _deviceManager = deviceManager;
            _mediator = mediator;
        }

        private void Draw()
        {
            var screen = GetBitmap();
            screen.Clear();

            Controls.DrawTitle(screen, "Reset to Defaults");
            Controls.DrawContent(screen, "Are you sure?");
            Controls.DrawLogo(screen, Resources.BitmapResources.Warning_48);

            Controls.DrawButton(screen, Button.One, Resources.BitmapResources.Check_22);
            Controls.DrawButton(screen, Button.Three, Resources.BitmapResources.Close_22);

            screen.Flush();
        }

        public void Handle(IEvent mediatorEvent)
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
                    _deviceManager.ResetToDefaults();
                    break;
                case Button.Three:
                    _deviceManager.Reboot();
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
