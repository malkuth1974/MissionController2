using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
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
            return "Orbit " + targetBody.theName + " and conduct research.";
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

            if (vessel.launchTime > this.Root.DateAccepted && setTime)
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

        public TimeCountdownLanding(CelestialBody target, double Mtime, string title)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.contractTimeTitle = title;
        }

        protected override string GetHashString()
        {
            return "Orbit " + targetBody.theName + " and conduct research.";
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }
      
        protected override void OnUpdate()
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
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (HighLogic.LoadedSceneIsFlight && setTime)
                {
                    contractSetTime();
                    vesselID = vessel.id.ToString();
                }             
            }
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

        private bool PreFlightCheck = false;

        private bool setTime = true;
        private bool timebool = false;

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

        protected override string GetHashString()
        {
            return "Orbit " + targetBody.theName + " and conduct research.";
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active && HighLogic.LoadedSceneIsFlight)
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

            node.AddValue("preflightcheck", PreFlightCheck);
        }
       
        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if ( FlightGlobals.ActiveVessel.launchTime > this.Root.DateAccepted)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
                {

                    if (vesselID == action.from.vessel.id.ToString() || vesselID == action.to.vessel.id.ToString())
                    {
                        ScreenMessages.PostScreenMessage("You have docked to the Target Vessel, time started");
                        contractSetTime();
                        action.from.vessel.vesselName = action.from.vessel.vesselName.Replace("(Repair)", "");
                        action.to.vessel.vesselName = action.to.vessel.vesselName.Replace("(Repair)", "");
                    }
                    else
                        ScreenMessages.PostScreenMessage("Did not connect to the correct target ID vessel, Try Again");
                }
            }
        }
        public void timeCountDown()
        {
            if (!setTime)
            {
                diff = Planetarium.GetUniversalTime() - savedTime;
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.id.ToString() == vesselID)
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
