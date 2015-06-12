using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC.MCEParameters
{
    #region Time Countdown Orbits
    public class TimeCountdownOrbits : ContractParameter
    {
        public CelestialBody targetBody;

        private double diff;
        private double savedTime;
        private double missionTime;
        private string contractTimeTitle = "Reach Orbit and stay for amount of Time Specified: ";
        private string vesselID = "none";

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
            return "Orbit " + targetBody.theName + " and conduct research." + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }      

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))                   
                {
                    if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                    {                     
                            CheckIfOrbit(FlightGlobals.ActiveVessel);                                               
                    }
                }
                timeCountDown(); 
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

        }
        public void timeCountDown()
        {
            if (!setTime)
            {
                diff = Planetarium.GetUniversalTime() - savedTime;
                if (HighLogic.LoadedSceneIsFlight &&  FlightGlobals.ActiveVessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("Time Left To Complete: " + Tools.formatTime(missionTime - diff), .001f);
                }

                if (diff > missionTime)
                {
                    base.SetComplete();
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
    #region Time Countdown Landing
    public class TimeCountdownLanding : ContractParameter
    {
        public CelestialBody targetBody;

        private double diff;
        private double savedTime;
        private double missionTime;
        private string contractTimeTitle = "Land Vessel and stay for amount of Time Specified: ";
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
            return "Orbit " + targetBody.theName + " and conduct research." + this.Root.MissionSeed.ToString();
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
            if (!setTime)
            {
                diff = Planetarium.GetUniversalTime() - savedTime;
                if (HighLogic.LoadedSceneIsFlight && vessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("Time Left To Complete: " + Tools.formatTime(missionTime - diff), .001f);
                }

                if (diff > missionTime)
                {
                    base.SetComplete();
                }
            }
            else if (hasToBeNewVessel && setTime)
            {
                if (HighLogic.LoadedSceneIsFlight && vessel.launchTime > this.Root.DateAccepted)
                {
                    contractSetTime();
                    vesselID = vessel.id.ToString();
                }             
            }
            else if (!hasToBeNewVessel && setTime)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    contractSetTime();
                    vesselID = vessel.id.ToString();
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
        private string contractTimeTitle = "Reach Orbit and stay for amount of Time Specified: ";
        private string vesselID = "none";
        private string vesselName = "none";

        private bool PreFlightCheck = false;

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

            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onPartCouple.Add(onPartCouple);
                updated = true;
            }

        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onPartCouple.Remove(onPartCouple);
            }

        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
                timeCountDown();
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
            node.AddValue("name2", vesselName);

            node.AddValue("preflightcheck", PreFlightCheck);
        }
       
        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if ( FlightGlobals.ActiveVessel.launchTime > this.Root.DateAccepted)
            {
                Debug.Log("Time Vessel Id Matches?: " + action.from.vessel.id.ToString() + " " + vesselID);
                Debug.Log("Time Vessel Id Matches?: " + action.to.vessel.id.ToString() + " " + vesselID);
                Debug.Log("Time Vessel Name Matches?: " + action.from.vessel.vesselName + " " + vesselName);
                Debug.Log("Time Vessel Name Matches?: " + action.to.vessel.vesselName + " " + vesselName);
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
        }
        public void timeCountDown()
        {
            if (!setTime)
            {
                diff = Planetarium.GetUniversalTime() - savedTime;
                if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.id.ToString() == vesselID || FlightGlobals.ActiveVessel.vesselName == vesselName))
                {
                    ScreenMessages.PostScreenMessage("Time Left To Complete: " + Tools.formatTime(missionTime - diff), .001f);
                }

                if (diff > missionTime)
                {
                    base.SetComplete();
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
}
