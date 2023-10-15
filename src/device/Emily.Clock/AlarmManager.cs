using System;
using System.Text;

namespace Emily.Clock
{
    public interface IAlarmManager
    {
        bool Enabled { get; set; }
    }

    public class AlarmManager : IAlarmManager
    {
        public bool Enabled { get; set; }
    }

    public static class AlarmManagerExtensions
    {
        internal static Resources.BitmapResources GetEnabledBitmapId(this IAlarmManager alarmManager)
        {
            return alarmManager.Enabled
                ? Resources.BitmapResources.Alarm_22
                : Resources.BitmapResources.Alarm_22_Outline;
        }
    }
}
