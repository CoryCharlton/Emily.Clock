namespace Emily.Clock;

public interface IAlarmService
{
    bool Enabled { get; set; }
}

public class AlarmService : IAlarmService
{
    public bool Enabled { get; set; }
}

public static class AlarmManagerExtensions
{
    internal static Resources.BitmapResources GetEnabledBitmapId(this IAlarmService alarmService)
    {
        return alarmService.Enabled
            ? Resources.BitmapResources.Alarm_22
            : Resources.BitmapResources.Alarm_22_Outline;
    }
}