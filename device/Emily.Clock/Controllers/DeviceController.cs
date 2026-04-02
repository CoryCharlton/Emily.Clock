using CCSWE.nanoFramework.WebServer;
using CCSWE.nanoFramework.WebServer.Authorization;
using Emily.Clock.Device;
using Emily.Clock.Networking;

namespace Emily.Clock.Controllers
{
    [Route("/api/device")]
    [AllowAnonymous]
    public class DeviceController: ControllerBase
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IWirelessNetworkManager _wirelessNetworkManager;

        public DeviceController(IDeviceManager deviceManager, IWirelessNetworkManager wirelessNetworkManager)
        {
            _deviceManager = deviceManager;
            _wirelessNetworkManager = wirelessNetworkManager;
        }

        [HttpGet]
        public void GetDeviceInfo()
        {
            Ok(_deviceManager);
        }

        [HttpGet("network")]
        public void GetNetworkInfo()
        {
            Ok(_wirelessNetworkManager);
        }

        [HttpGet("ping")]
        public void PingDevice()
        {
            Ok("Pong!");
        }

        [HttpPost("reboot")]
        public void RebootDevice()
        {
            Ok();
         
            _deviceManager.Reboot();
        }
    }
}
