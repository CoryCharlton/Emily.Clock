using MakoIoT.Device.Services.Server.WebServer;
using Emily.Clock.Device;

namespace Emily.Clock.Controllers
{
    public class DeviceController: ControllerBase
    {
        private readonly IDeviceManager _deviceManager;

        public DeviceController(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        [Route("device")]
        [Method("GET")]
        public void GetDeviceInfo(WebServerEventArgs e)
        {
            Ok(e.Context.Response, _deviceManager);
        }

        [Route("device/reboot")]
        [Method("POST")]
        public void RebootDevice(WebServerEventArgs e)
        {
            Ok(e.Context.Response);
         
            _deviceManager.Reboot();
        }
    }
}
