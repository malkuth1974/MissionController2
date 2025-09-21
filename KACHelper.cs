using KACWrapper;

namespace MissionControllerEC
{
    public static class KACHelper
    {
        public static void CreateAlarm(string title, double ut, double margin)
        {
            if (!KACWrapper.APIReady) return;

            var alarm = KACWrapper.KAC.CreateAlarm(
            KACWrapper.KACAPI.AlarmTypeEnum.Raw,
            title,
            ut
            );

            if (alarm != null)
            {
                alarm.AlarmMarginSecs = margin;
                alarm.Notes = "Created by Mission Controller 2";
            }
        }
    }
}