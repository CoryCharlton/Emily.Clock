using System;

namespace Emily.Clock.Device;

// TODO: Should we expose device features here?
public interface IDeviceManager
{
    uint FreeMemory { get; }
    TimeSpan RunningFor { get; }
    string SerialNumber { get; }
    
    /// <summary>
    /// Time the application started. Should be set after wifi connects with a valid date/
    /// </summary>
    DateTime StartedAt { get; set; }

    void Reboot();
    void ResetToDefaults();
}