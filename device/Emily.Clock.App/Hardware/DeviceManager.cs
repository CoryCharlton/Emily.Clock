using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device;
using Emily.Clock.Networking;
using Microsoft.Extensions.Logging;

namespace Emily.Clock.App.Hardware;

public class DeviceManager: DeviceManagerBase
{
    public DeviceManager(IConfigurationManager configurationService, ILogger logger, IMediator mediator, INetworkInterfaceProvider networkInterfaceProvider): base(configurationService, logger, mediator, networkInterfaceProvider)
    {
    }
}