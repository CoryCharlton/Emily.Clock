using System;
using Emily.Clock.Device;
using MakoIoT.Device.Services.Interface;

namespace Emily.Clock.Networking
{
    public interface IWirelessNetworkManager
    {
        string IpAddress { get; }
        string Ssid { get; }

        WirelessMode GetMode();
        bool Initialize();
        void SetMode(WirelessMode mode);
    }

    // TODO: Replace IStorageService with something else more generic
    public class WirelessNetworkManager : IWirelessNetworkManager
    {
        private const string WirelessModeFileName = "wireless_mode.sys";
        //private const string WirelessModeFileName = @"I:\wireless_mode.sys";

        private readonly IDeviceManager _deviceManager;
        private readonly IStorageService _storageService;
        private readonly IWirelessAccessPointManager _wirelessAccessPointManager;
        private readonly IWirelessClientManager _wirelessClientManager;

        public WirelessNetworkManager(IDeviceManager deviceManager, IStorageService storageService, IWirelessAccessPointManager wirelessAccessPointManager, IWirelessClientManager wirelessClientManager)
        {
            _deviceManager = deviceManager;
            _storageService = storageService;
            _wirelessAccessPointManager = wirelessAccessPointManager;
            _wirelessClientManager = wirelessClientManager;
        }

        public string IpAddress => GetMode() == WirelessMode.AccessPoint ? _wirelessAccessPointManager.IpAddress : _wirelessClientManager.IpAddress;

        public string Ssid => GetMode() == WirelessMode.AccessPoint ? _wirelessAccessPointManager.Ssid : _wirelessClientManager.Ssid;

        public WirelessMode GetMode()
        {
            if (!_storageService.FileExists(WirelessModeFileName))
            {
                return WirelessMode.Client;
            }

            var modeString = _storageService.ReadFile(WirelessModeFileName);
            if (string.IsNullOrEmpty(modeString))
            {
                return WirelessMode.Client;
            }

            if (!int.TryParse(modeString, out var mode))
            {
                return WirelessMode.Client;
            }
            
            return (WirelessMode) mode;
        }

        public bool Initialize()
        {
            return GetMode() switch
            {
                WirelessMode.AccessPoint => InitializeAccessPoint(),
                WirelessMode.Client => InitializeClient(),
                _ => throw new NotSupportedException()
            };
        }

        private bool InitializeAccessPoint()
        {
            if (!_wirelessAccessPointManager.IsEnabled)
            {
                SetMode(WirelessMode.AccessPoint);

                return false;
            }

            return _wirelessAccessPointManager.Start();
        }

        private bool InitializeClient()
        {
            if (!_wirelessClientManager.IsConfigured)
            {
                SetMode(WirelessMode.AccessPoint);
                
                return false;
            }

            _wirelessAccessPointManager.Disable();

            if (!_wirelessClientManager.IsEnabled)
            {
                _wirelessClientManager.Enable();
            }

            return _wirelessClientManager.Connect();
        }

        public void SetMode(WirelessMode mode)
        {
            /*
            if (GetMode() == mode)
            {
                return;
            }
            */

            switch (mode)
            {
                case WirelessMode.AccessPoint:
                    _wirelessClientManager.Disconnect();
                    _wirelessClientManager.Disable();

                    _wirelessAccessPointManager.Enable();
                    break;
                case WirelessMode.Client:
                    _wirelessAccessPointManager.Stop();
                    _wirelessAccessPointManager.Disable();

                    _wirelessClientManager.Enable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }

            _storageService.WriteToFile(WirelessModeFileName, mode.ToString());
            _deviceManager.Reboot();
        }
    }
}
