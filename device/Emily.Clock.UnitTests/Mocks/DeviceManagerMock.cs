using System;
using Emily.Clock.Device;

namespace Emily.Clock.UnitTests.Mocks
{
    public class DeviceManagerMock: IDeviceManager
    {
        public uint FreeMemory => 1;
        public string SerialNumber => "SerialNumber";
        public DateTime StartedAt { get; set; } = DateTime.MinValue;
        public TimeSpan RunningFor => DateTime.UtcNow.Subtract(StartedAt);

        public bool StartedAtSet => StartedAt > DateTime.MinValue;

        public void Reboot()
        {
            RebootCalled = true;
        }

        public bool RebootCalled { get; set; }

        public void ResetToDefaults()
        {
            ResetToDefaultsCalled = true;
        }

        public bool ResetToDefaultsCalled { get; set; }
    }
}
