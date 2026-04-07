using Emily.Clock.Device;
using nanoFramework.System.IO.FileSystem;

namespace Emily.Clock.Device.FileStorage.SdCard;

public class SdCardOptions
{
    public SDCardMmcParameters? MmcParameters { get; set; }
    public DevicePreInitializeDelegate? PreInitialize { get; set; }
    public SDCardSpiParameters? SpiParameters { get; set; }
}