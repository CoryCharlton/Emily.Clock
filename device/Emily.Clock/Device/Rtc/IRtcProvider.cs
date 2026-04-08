using System;

namespace Emily.Clock.Device.Rtc;

public interface IRtcProvider
{
    DateTime DateTime { get; set; }
    bool IsInitialized { get; }

    bool Initialize();
}
