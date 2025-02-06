using CCSWE.nanoFramework.WebServer;
using Emily.Clock.Device;

namespace Emily.Clock.Controllers
{
    [Route("/api/device")]
    public class DeviceController: ControllerBase
    {
        private readonly IDeviceManager _deviceManager;

        public DeviceController(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        [HttpGet]
        public void GetDeviceInfo()
        {
            Ok(_deviceManager);
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
