using System;
using UnityEngine;
using MCE_KacWrapper;
using System.Linq; // Must match wrapper namespace

namespace MissionControllerEC
{
    public static class KACHelper
    {
        //public static  bool KACInstalled()
        //{
            //Checking to see if KAC is even installed on players Game. If not won't use KAC alarms.
           // return AssemblyLoader.loadedAssemblies.Any(a => a.assembly.GetType("KACWrapper.KACAPI") != null);
       // }
        public static void CreateAlarmMC2(string title, double duration)
        {
            if (KACWrapper.APIReady)
            {
                //Create a raw alarm 15 mins from now game time and get the id back
                String aID = KACWrapper.KAC.CreateAlarm(KACWrapper.KACAPI.AlarmTypeEnum.Raw, title, duration);

                if (aID != "")
                {
                    //if the alarm was made get the object so we can update it
                    KACWrapper.KACAPI.KACAlarm a = KACWrapper.KAC.Alarms.First(z => z.ID == aID);

                    //Now update some of the other properties
                    a.Notes = "Mission Controller Alarm For Missions";
                    a.AlarmAction = KACWrapper.KACAPI.AlarmActionEnum.DoNothingDeleteWhenPassed;
                }
    
            }
        }
    }
}