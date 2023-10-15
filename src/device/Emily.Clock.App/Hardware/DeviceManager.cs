using System;
using System.Net.NetworkInformation;
using System.Text;
using Emily.Clock.Device;
using Emily.Clock.Device.NeoPixel;
using Emily.Clock.Mediator.Events;
using Emily.Clock.Networking;
using Emily.Clock.UI;
using MakoIoT.Device.Services.Mediator;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Runtime.Native;
using GC = nanoFramework.Runtime.Native.GC;

namespace Emily.Clock.App.Hardware
{
    public class DeviceManager: IDeviceManager
    {
        private readonly IDisplayManager _displayManager;
        private readonly IMediator _mediator;
        private readonly INeoPixelManager _neoPixelManager;
        private readonly INetworkInterfaceProvider _networkInterfaceProvider;
        private string _serialNumber;

        public DeviceManager(IDisplayManager displayManager, IMediator mediator, INeoPixelManager neoPixelManager, INetworkInterfaceProvider networkInterfaceProvider)
        {
            _displayManager = displayManager;
            _mediator = mediator;
            _neoPixelManager = neoPixelManager;
            _networkInterfaceProvider = networkInterfaceProvider;
        }

        public uint FreeMemory
        {
            get
            {
                return GC.Run(false);

                // TODO: This reports less than GC.Run(false) and not sure why
                NativeMemory.GetMemoryInfo(NativeMemory.MemoryType.All, out _, out var freeMemory, out _);
                return freeMemory;
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

            if (_displayManager.IsInitialized)
            {
                _displayManager.Clear();
                _displayManager.SetBackLight(false);
            }

            if (_neoPixelManager.IsInitialized)
            {
                _neoPixelManager.Clear();
            }

            Power.RebootDevice();
        }
    }
}
