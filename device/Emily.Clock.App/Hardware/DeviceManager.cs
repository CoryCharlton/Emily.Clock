using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using CCSWE.nanoFramework.Configuration;
using CCSWE.nanoFramework.Mediator;
using Emily.Clock.Device;
using Emily.Clock.Mediator.Events;
using Emily.Clock.Networking;
using Microsoft.Extensions.Logging;
using nanoFramework.Runtime.Native;
using GC = nanoFramework.Runtime.Native.GC;

namespace Emily.Clock.App.Hardware
{
    // TODO: Consider moving most (all?) of this to a base class
    public class DeviceManager: IDeviceManager
    {
        private readonly IConfigurationManager _configurationService;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly INetworkInterfaceProvider _networkInterfaceProvider;
        private string? _serialNumber;

        public DeviceManager(IConfigurationManager configurationService, ILogger logger, IMediator mediator, INetworkInterfaceProvider networkInterfaceProvider)
        {
            _configurationService = configurationService;
            _logger = logger;
            _mediator = mediator;
            _networkInterfaceProvider = networkInterfaceProvider;
        }

        public uint FreeMemory
        {
            get
            {
                // ReSharper disable once ArrangeAccessorOwnerBody
                return GC.Run(false);

                // TODO: This reports less than GC.Run(false) and not sure why
                //NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.All, out _, out var freeMemory, out _);
                //return freeMemory;
            }
        }

        public TimeSpan RunningFor => DateTime.UtcNow.Subtract(StartedAt);

        public string SerialNumber
        {
            get
            {
                if (string.IsNullOrEmpty(_serialNumber))
                {
                    var macAddress = _networkInterfaceProvider.RequireInterface(NetworkInterfaceType.Wireless80211).PhysicalAddress;
                    var stringBuilder = new StringBuilder(6);

                    for (var i = macAddress.Length - 3; i < macAddress.Length; i++)
                    {
                        stringBuilder.Append(BitConverter.ToString(macAddress, i, 1));
                    }

                    _serialNumber = stringBuilder.ToString();
                }

                return _serialNumber;
            }
        }

        public DateTime StartedAt { get; set; } = DateTime.MinValue;

        public void Reboot()
        {
            _mediator.Publish(new StatusEvent("Rebooting..."));

            if (Debugger.IsAttached)
            {
                _logger.LogWarning("Device will not reboot while debugger attached");
                _logger.LogWarning("Please power cycle device");

                _mediator.Publish(new StatusEvent("Please power cycle device"));
            }
            else
            {
                Power.RebootDevice();
            }
        }

        public void ResetToDefaults()
        {
            _configurationService.Clear();

            Reboot();
        }
    }
}
