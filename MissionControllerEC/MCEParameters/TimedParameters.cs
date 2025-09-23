using Contracts;
using Contracts.Predicates;
using KSP;
using KSP.Localization;
using KSPAchievements;
using MCE_KacWrapper;
using MissionControllerEC;
using MissionControllerEC.MCEParameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MissionControllerEC.RegisterToolbar;

namespace MissionControllerEC.MCEParameters
{
    
    #region Time Countdown Orbits
    public class TimeCountdownOrbits : ContractParameter
    {
        public CelestialBody targetBody;        
        private double diff = 0;
        private double savedTime;
        private double missionTime;
        private string contractTimeTitle = "Reach Orbit and stay for amount of Time Specified: ";
        private string vesselID = "none";
        private bool kacCheck = false;
        private Contract contractRoot;

        private bool PreFlightCheck = false;

        private bool setTime = true;
        private bool timebool = false;

        private bool AllChildOff = false;       
      
        public TimeCountdownOrbits()
        {
        }

        public TimeCountdownOrbits(CelestialBody target, double Mtime, bool childOff)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.AllChildOff = childOff;
        }
       
        public TimeCountdownOrbits(CelestialBody target, double Mtime, string title, bool childOff)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.contractTimeTitle = title;
            this.AllChildOff = true;          
        }
               
        protected override string GetHashString()
        {
            return "Orbit " + targetBody.bodyName + " and conduct research." + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }

        protected override void OnRegister()
        {
            base.OnRegister();
            contractRoot = this.Root;
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                // Look for your orbit goal parameters without LINQ
                ApAOrbitGoal apAParam = null;
                PeAOrbitGoal peAParam = null;

                foreach (ContractParameter param in contractRoot.AllParameters)
                {
                    if (param is ApAOrbitGoal a) apAParam = a;
                    if (param is PeAOrbitGoal p) peAParam = p;
                }

                if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                    {
                        CheckIfOrbit(FlightGlobals.ActiveVessel);
                    }
                    else { }
                }
                if (apAParam != null && peAParam != null && apAParam.State == ParameterState.Complete && peAParam.State == ParameterState.Complete)
                {
                    timeCountDown();
                }
                else { }
            }
            else { }

        }
        private IEnumerable<T> GetChildren<T>(ContractParameter parent) where T : ContractParameter
        {
            var contractParent = parent as ContractParameter;
            if (contractParent == null) yield break;

            foreach (ContractParameter child in contractParent.GetType().GetProperty("Children",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.FlattenHierarchy)
            .GetValue(contractParent) as IEnumerable<ContractParameter>)
            {
                if (child is T tParam)
                    yield return tParam;

                foreach (var nested in GetChildren<T>(child))
                    yield return nested;
            }
        }
        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref savedTime, 0, savedTime, "savedtime");
            Tools.ContractLoadCheck(node, ref missionTime, 1000, missionTime, "missiontime");
            Tools.ContractLoadCheck(node, ref diff, 0, diff, "diff");
            Tools.ContractLoadCheck(node, ref setTime, false, setTime, "settime");
            Tools.ContractLoadCheck(node, ref timebool, false, timebool, "timebool");
            Tools.ContractLoadCheck(node, ref vesselID, "Defaults Loaded", vesselID, "vesid");
            Tools.ContractLoadCheck(node, ref PreFlightCheck, false, PreFlightCheck, "preflightcheck");
            Tools.ContractLoadCheck(node, ref AllChildOff, false, AllChildOff, "AllChildOff");
            Tools.ContractLoadCheck(node, ref kacCheck, false, kacCheck, "kacCheck");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("kacCheck", kacCheck);
            node.AddValue("savedtime", savedTime);
            node.AddValue("missiontime", missionTime);
            node.AddValue("diff", diff);

            node.AddValue("settime", setTime);
            node.AddValue("timebool", timebool);
            node.AddValue("vesid", vesselID);

            node.AddValue("preflightcheck", PreFlightCheck);
            node.AddValue("AllChildOff", AllChildOff);

        }

        private void CheckIfOrbit(Vessel vessel)
        {

            if (vessel.launchTime > this.Root.DateAccepted && setTime && !vessel.isEVA)
            {
                contractSetTime();
                vesselID = vessel.id.ToString();
            }
            else { }

        }
        public void timeCountDown()
        {
            if (!KACWrapper.APIReady)
            {
                Log.Warning("MCE tried to load the KAC Alarm but failed");
                if (!setTime)
                {
                    diff = Planetarium.GetUniversalTime() - savedTime;
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.id.ToString() == vesselID)
                    {
                        ScreenMessages.PostScreenMessage(
                        "Time Left To Complete: " + Tools.formatTime(missionTime - diff),
                        .001f
                        );
                    }

                    if (diff > missionTime)
                        base.SetComplete();
                }
            }
            else
            {
                
                    
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.id.ToString() == vesselID && kacCheck == false)
                    {
                        diff = Planetarium.GetUniversalTime() + missionTime;
                        KACHelper.CreateAlarmMC2(contractTimeTitle, diff);
                        kacCheck = true;
                        Log.Info("KacAlarm Loade" + contractTimeTitle + "  " + missionTime + " " + kacCheck);                 
                    }
                if (Planetarium.GetUniversalTime() > diff)
                base.SetComplete();
                
            }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;

        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        } 
    } 
     #endregion             
    #region Time Countdown Landing
    public class TimeCountdownLanding : ContractParameter
    {
        public CelestialBody targetBody;

        private double diff;
        private double savedTime;
        private double missionTime;
        private string contractTimeTitle = Localizer.Format("#autoLOC_MissionController2_1000235");		// #autoLOC_MissionController2_1000235 = Land Vessel and stay for amount of Time Specified: 
        private string vesselID = "none";
        private bool hasToBeNewVessel = true;

        private bool setTime = true;
        private bool timebool = false;

        public TimeCountdownLanding()
        {
        }

        public TimeCountdownLanding(CelestialBody target, double Mtime)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
        }

        public TimeCountdownLanding(CelestialBody target, double Mtime, string title,bool newVessel)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.contractTimeTitle = title;
            this.hasToBeNewVessel = newVessel;
        }

        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000236") + " " + targetBody.bodyName + Localizer.Format("#autoLOC_MissionController2_1000237") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000236 = Orbit 		// #autoLOC_MissionController2_1000237 =  and conduct research.
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }
      
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) &&
                    (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
                {
                    timebool = true;
                }
                if (timebool)
                {
                    CheckIfLanded(FlightGlobals.ActiveVessel);
                }
                else { }
            }
            else { }
        }
        protected override void OnLoad(ConfigNode node)
        {          
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref savedTime, 0, savedTime, "savedtime");
            Tools.ContractLoadCheck(node, ref missionTime, 1000, missionTime, "missiontime");
            Tools.ContractLoadCheck(node, ref diff, 0, diff, "diff");
            Tools.ContractLoadCheck(node, ref setTime, false, setTime, "settime");
            Tools.ContractLoadCheck(node, ref timebool, false, timebool, "timebool");
            Tools.ContractLoadCheck(node, ref vesselID, "Defaults Loaded", vesselID, "vesid");          
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("savedtime", savedTime);
            node.AddValue("missiontime", missionTime);
            node.AddValue("diff", diff);

            node.AddValue("settime", setTime);
            node.AddValue("timebool", timebool);
            node.AddValue("vesid", vesselID);
        }

        private void CheckIfLanded(Vessel vessel)
        {
            if (!KACWrapper.APIReady)
            {
                if (!setTime)
                {
                    diff = Planetarium.GetUniversalTime() - savedTime;
                    if (HighLogic.LoadedSceneIsFlight && vessel.id.ToString() == vesselID)
                    {
                        ScreenMessages.PostScreenMessage(
                        "Time Left To Complete: " + Tools.formatTime(missionTime - diff),
                        .001f
                        );
                    }

                    if (diff > missionTime)
                        base.SetComplete();
                }
                else if (hasToBeNewVessel && setTime)
                {
                    if (HighLogic.LoadedSceneIsFlight && vessel.launchTime > this.Root.DateAccepted)
                    {
                        contractSetTime();
                        vesselID = vessel.id.ToString();
                    }
                }
            }
            else
            {
                if (setTime)
                {
                    KACHelper.CreateAlarmMC2(contractTimeTitle, missionTime);
                    contractSetTime();
                }
            }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        } 
    }
    #endregion             
    #region Time Countdown Docking
    public class TimeCountdownDocking : ContractParameter
    {
        public CelestialBody targetBody;

        private double diff;
        private double savedTime;
        private double missionTime;
        private string contractTimeTitle = "Dock With Station To Start Timer and wait ";
        private string vesselID = "none";
        private string vesselName = "none";
        private Contract contractRoot;

        private bool PreFlightCheck = false;
        private bool kacCheck = false;

        private bool setTime = true;
        private bool timebool = false;
        private bool updated = false;


        public TimeCountdownDocking()
        {
        }

        public TimeCountdownDocking(CelestialBody target, double Mtime, string vesId)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.vesselID = vesId;
        }
        
        public TimeCountdownDocking(CelestialBody target, double Mtime, string title, string vesId)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.contractTimeTitle = title;
            this.vesselID = vesId;
        }
        public TimeCountdownDocking(CelestialBody target, double Mtime, string title, string vesId, string vesName)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.contractTimeTitle = title;
            this.vesselID = vesId;
            this.vesselName = vesName;
        }

        protected override string GetHashString()
        {
            return "Dock To Target Vessel To Start Countdown" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }

        protected override void OnRegister()
        {
            contractRoot = this.Root;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onPartCouple.Add(onPartCouple);
                updated = true;
            }
            else { }
        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onPartCouple.Remove(onPartCouple);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState != Contract.State.Active)
                return;
            // Look for your orbit goal parameters without LINQ
            TargetDockingGoal targetDockingGoal = null;
           
            foreach (ContractParameter param in contractRoot.AllParameters)
            {
                if (param is TargetDockingGoal p) targetDockingGoal = p;
            }
            // all the extra code just to make this behave and launch correctly.
            if (targetDockingGoal != null && targetDockingGoal.State == ParameterState.Complete)
            {
                timeCountDown();
            }
            else { }
        }
        private IEnumerable<T> GetChildren<T>(ContractParameter parent) where T : ContractParameter
        {
            //had to dig deep to get to the children for I can see if parameters are complete.  Allows to control when OnTime launches
            var contractParent = parent as ContractParameter;
            if (contractParent == null) yield break;

            foreach (ContractParameter child in contractParent.GetType().GetProperty("Children",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.FlattenHierarchy)
            .GetValue(contractParent) as IEnumerable<ContractParameter>)
            {
                if (child is T tParam)
                    yield return tParam;

                foreach (var nested in GetChildren<T>(child))
                    yield return nested;
            }
        }
        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref savedTime, 0, savedTime, "savedtime");
            Tools.ContractLoadCheck(node, ref missionTime, 1000, missionTime, "missiontime");
            Tools.ContractLoadCheck(node, ref diff, 0, diff, "diff");
            Tools.ContractLoadCheck(node, ref setTime, false, setTime, "settime");
            Tools.ContractLoadCheck(node, ref timebool, false, timebool, "timebool");
            Tools.ContractLoadCheck(node, ref vesselID, "Defaults Loaded", vesselID, "vesid");
            Tools.ContractLoadCheck(node, ref vesselName, "None", vesselName, "name2");
            Tools.ContractLoadCheck(node, ref PreFlightCheck, false, PreFlightCheck, "preflightcheck");
            Tools.ContractLoadCheck(node, ref kacCheck, false, kacCheck, "kacCheck");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("kacCheck", kacCheck);
            node.AddValue("savedtime", savedTime);
            node.AddValue("missiontime", missionTime);
            node.AddValue("diff", diff);

            node.AddValue("settime", setTime);
            node.AddValue("timebool", timebool);
            node.AddValue("vesid", vesselID);
            node.AddValue("name2", vesselName);

            node.AddValue("preflightcheck", PreFlightCheck);
        }
       
        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if ( FlightGlobals.ActiveVessel.launchTime > this.Root.DateAccepted)
            {
                Log.Info("Time Vessel Id Matches?: " + action.from.vessel.id.ToString() + " " + vesselID);
                Log.Info("Time Vessel Id Matches?: " + action.to.vessel.id.ToString() + " " + vesselID);
                Log.Info("Time Vessel Name Matches?: " + action.from.vessel.vesselName + " " + vesselName);
                Log.Info("Time Vessel Name Matches?: " + action.to.vessel.vesselName + " " + vesselName);
                if (vesselID == action.from.vessel.id.ToString() || vesselID == action.to.vessel.id.ToString() || vesselName == action.from.vessel.vesselName || vesselName == action.to.vessel.vesselName)
                {
                    ScreenMessages.PostScreenMessage("Vessel docked, time started");
                    contractSetTime();
                }
                else
                {
                    ScreenMessages.PostScreenMessage("Time not started not correct docking vessel, Try Again");                    
                }              
            }
            else { }
        }
        public void timeCountDown()
        {
            if (!KACWrapper.APIReady)
            {
                if (!setTime)
                {
                    // Old code in case KAC is not installed on players game
                    diff = Planetarium.GetUniversalTime() - savedTime;
                    if (HighLogic.LoadedSceneIsFlight &&
                    (FlightGlobals.ActiveVessel.id.ToString() == vesselID || FlightGlobals.ActiveVessel.vesselName == vesselName))
                    {
                        ScreenMessages.PostScreenMessage(
                        "Time Left To Complete: " + Tools.formatTime(missionTime - diff),
                        .001f
                        );
                    }

                    if (diff > missionTime)
                        base.SetComplete();
                }
            }
            else
            {
                // new code that uses KAC to do the countdown, much cleaner than the old code, doesn't flood screen.
                if (HighLogic.LoadedSceneIsFlight && kacCheck == false)
                {
                    diff = Planetarium.GetUniversalTime() + missionTime;
                    Log.Info("Mission Time Set By MCE IS " + missionTime);
                    KACHelper.CreateAlarmMC2(contractTimeTitle, diff);
                    kacCheck = true;
                    Log.Info("KacAlarm Loade" + contractTimeTitle + "  " + missionTime + " " + kacCheck);
                }
                // Final Check if Kac Countdown is done to end contract para.
                if (Planetarium.GetUniversalTime() > diff)
                    base.SetComplete();
            }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion         
}
