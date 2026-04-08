namespace Emily.Clock.Testing.Mocks;

public class AlarmServiceMock: IAlarmService
{
    public bool Enabled { get; private set; }
    public bool IsAlarming { get; private set; }
        
    public bool Initialize()
    {
        return true;
    }

    public void StartAlarm()
    {
            
    }

    public void Stop()
    {
            
    }

    public void Toggle()
    {
        Enabled = !Enabled;
    }

    public void Dispose()
    {
    }
}