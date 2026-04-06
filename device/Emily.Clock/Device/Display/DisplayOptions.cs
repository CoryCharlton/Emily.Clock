using nanoFramework.UI;

namespace Emily.Clock.Device.Display;

public abstract class DisplayOptions
{
    public int BacklightPin { get; set; }
    public ushort Height { get; set; }
    public DisplayOrientation Orientation { get; set; }
    public DevicePreInitializeDelegate? PreInitialize { get; set; }
    public ushort Width { get; set; }
}
