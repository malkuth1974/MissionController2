using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;
using KSP.Localization;

namespace MissionControllerEC.MCEParameters
{
   
    #region Get Crew Count
    public class GetCrewCount : ContractParameter
    {
        private int crewCount = 0;
        private bool updated = false;

        public GetCrewCount()
        {
        }

        public GetCrewCount(int crewnumber)
        {
            this.crewCount = crewnumber;
        }
        protected override string GetHashString()
        {
            if (crewCount > 0)
                return Localizer.Format("#autoLOC_MissionController2_1000196") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000196 = Amount crew 
            else
                return Localizer.Format("#autoLOC_MissionController2_1000197") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000197 = Vessel is automated: (nocrew)
        }
        protected override string GetTitle()
        {
            if (crewCount > 0)
                return Localizer.Format("#autoLOC_MissionController2_1000198") + " " + crewCount;		// #autoLOC_MissionController2_1000198 = Vessel Must Have This Amount Of crew 
            else
                return Localizer.Format("#autoLOC_MissionController2_1000199");		// #autoLOC_MissionController2_1000199 = Vessel is automated: (nocrew)
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
            else { }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
                CheckCrewValues(FlightGlobals.ActiveVessel);
            else { }
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", crewCount);
        }

        public void CheckCrewValues(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                int currentcrew = FlightGlobals.ActiveVessel.GetCrewCount();
                //Log.Error("Current crew is " + currentcrew + " crew can't be over " + crewCount);
                if (currentcrew >= crewCount && currentcrew != 0)
                {
                    base.SetComplete();
                    //Log.Info("Passed Crew Check");
                }
                else { }
                if (crewCount == 0 && currentcrew == 0 )
                {
                    base.SetComplete();
                }
                else { }
            }
            else { }
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
    #region EVA Goal
    public class EvaGoal : ContractParameter
    {
        CelestialBody targetBody;

        public EvaGoal()
        {
        }

        public EvaGoal(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000200") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000200 = EVA outside of your vessel
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000201");		// #autoLOC_MissionController2_1000201 = Exit your vessel and conduct an EVA
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                isEVA(FlightGlobals.ActiveVessel);
            else { }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
        }

        public void isEVA(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.isEVA)
                base.SetComplete();
            else { }
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
