using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;
using KSP.Localization;
using static MissionControllerEC.RegisterToolbar;

namespace MissionControllerEC.MCEParameters
{
   
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
            return Localizer.Format("#autoLOC_MissionController2_1000204") + " " + targetDockingName + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000204 = Dock with Vessel:\n 
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000205") + " " + targetDockingName;		// #autoLOC_MissionController2_1000205 = Dock with Vessel: \n
        }

        protected override void OnRegister()
        {

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

                Log.Error("Does: " + targetDockingID + " = " + action.from.vessel.id.ToString());
                Log.Error("Or Does: " + targetDockingID + " = " + action.to.vessel.id.ToString());
                Log.Error("Does: " + targetDockingName + " = " + action.from.vessel.vesselName);
                Log.Error("Or Does: " + targetDockingName + " = " + action.to.vessel.vesselName);
                
                Log.Error("Docked FROM: " + action.from.vessel.vesselName);
                Log.Error("Docked TO: " + action.to.vessel.vesselName);
                              
                Log.Error("Docked FROM ID: " + action.from.vessel.id.ToString());
                Log.Error("Docked TO ID: " + action.to.vessel.id.ToString());

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
            else { }
        }        
    }
    #endregion
    
}
