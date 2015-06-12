using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC.MCEParameters
{
    #region DockingGoal
    public class DockingGoal : ContractParameter
    {
        private bool updated = false;

        public DockingGoal()
        {
        }

        protected override string GetHashString()
        {
            return "Dock with another Vessel" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Dock with another Vessel";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
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

        protected override void OnLoad(ConfigNode node)
        {

        }
        protected override void OnSave(ConfigNode node)
        {

        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                Debug.LogError("Docked FROM: " + action.from.vessel.vesselName);
                Debug.LogError("Docked TO: " + action.to.vessel.vesselName);

                Debug.LogError("Docked TO Type Vessel: " + action.to.vessel.vesselType);

                Debug.LogError("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError("Docked TO ID: " + action.to.vessel.id.ToString());
                base.SetComplete();
            }
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
    #region Target Docking Goal
    public class TargetDockingGoal : ContractParameter
    {
        private string targetDockingID;
        private string targetDockingName;
        private bool updated = false;
        private static bool DockedTrue = false;

        public TargetDockingGoal()
        {
        }


        public static string ItargetDockingName(ContractParameter cp)
        {
            TargetDockingGoal instance = (TargetDockingGoal)cp;
            return instance.targetDockingName;
        }

        public static string ItargetDockingID(ContractParameter cp)
        {
            TargetDockingGoal instance = (TargetDockingGoal)cp;
            return instance.targetDockingID;
        }

        internal static bool isDockedTrue
        {
            get { return DockedTrue; }
            private set { }
        }

        public TargetDockingGoal(string targetID, string targetName)
        {
            this.targetDockingID = targetID;
            this.targetDockingName = targetName;
        }

        protected override string GetHashString()
        {
            return "Dock with Vessel:\n " + targetDockingName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Dock with Vessel: \n" + targetDockingName;
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

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetDockingID, "defaults Loaded", targetDockingID, "VesselID");
            Tools.ContractLoadCheck(node, ref targetDockingName, "Error Defaults Loaded", targetDockingName, "VesselName");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("VesselID", targetDockingID);
            node.AddValue("VesselName", targetDockingName);
        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {

                Debug.LogError("Does: " + targetDockingID + " = " + action.from.vessel.id.ToString());
                Debug.LogError("Or Does: " + targetDockingID + " = " + action.to.vessel.id.ToString());
                Debug.LogError("Does: " + targetDockingName + " = " + action.from.vessel.vesselName);
                Debug.LogError("Or Does: " + targetDockingName + " = " + action.to.vessel.vesselName);
                
                Debug.LogError("Docked FROM: " + action.from.vessel.vesselName);
                Debug.LogError("Docked TO: " + action.to.vessel.vesselName);
                              
                Debug.LogError("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError("Docked TO ID: " + action.to.vessel.id.ToString());

                if (targetDockingID == action.from.vessel.id.ToString() || targetDockingID == action.to.vessel.id.ToString() || targetDockingName == action.from.vessel.vesselName || targetDockingName == action.to.vessel.vesselName)
                {
                    ScreenMessages.PostScreenMessage("You have docked to the Target Vessel, Goal Complete");
                    DockedTrue = true;
                    base.SetComplete();
                    action.from.vessel.vesselName = action.from.vessel.vesselName.Replace("(Repair)", "");
                    action.to.vessel.vesselName = action.to.vessel.vesselName.Replace("(Repair)", "");
                }
                else
                    ScreenMessages.PostScreenMessage("Did not connect to the correct target ID vessel, Try Again");
            }
        }        
    }
    #endregion
    
}
