namespace Emily.Clock.Device.Gpio
{
    public interface IButtonManager
    {
        bool Initialize();
        bool IsPressed(Button button);
    }
}
