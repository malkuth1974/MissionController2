using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;
using KSP.Localization;

namespace MissionControllerEC.MCEParameters
{
    #region Get Seat Count
    public class GetSeatCount : ContractParameter
    {
        private int seatCount = 0;
        private string title = "none";
        private bool updated = false;

        public GetSeatCount()
        {
        }

        public GetSeatCount(int crewnumber, string titledesc)
        {
            this.seatCount = crewnumber;
            this.title = titledesc;
        }
        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000192") + " " + seatCount + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000192 = Amount crew 
        }
        protected override string GetTitle()
        {
            return title + " " + seatCount + " " + Localizer.Format("#autoLOC_MissionController2_1000193");		// #autoLOC_MissionController2_1000193 =  Plus Crew.
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
            Tools.ContractLoadCheck(node, ref seatCount, 71000, seatCount, "crewcount");
            Tools.ContractLoadCheck(node, ref title, "Defualts Loaded Error", title, "title");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", seatCount);
            node.AddValue("title", title);
        }

        public void CheckCrewValues(Vessel vessel)
        {
            int currentseats = FlightGlobals.ActiveVessel.GetCrewCapacity();
            int crewedseats = FlightGlobals.ActiveVessel.GetCrewCount();

            int seatavailable = currentseats - crewedseats;

            if (seatavailable >= seatCount)
            {
                base.SetComplete();
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
    #region Max Seat Count
    public class MaxSeatCount : ContractParameter
    {
        private int seatCount = 0;
        private string title = "";
        private bool updated = false;

        public MaxSeatCount()
        {
        }

        public MaxSeatCount(int MaxSeatCount)
        {
            this.seatCount = MaxSeatCount;
        }
        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000194") + " " + this.Root.MissionSeed;		// #autoLOC_MissionController2_1000194 = Amount crew 
        }
        protected override string GetTitle()
        {
            return title + Localizer.Format("#autoLOC_MissionController2_1000195") + " " + seatCount;		// #autoLOC_MissionController2_1000195 = Must Have This Many Seats: 
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
                CheckMaxSeatCount(FlightGlobals.ActiveVessel);

        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref seatCount, 71000, seatCount, "crewcount");
            Tools.ContractLoadCheck(node, ref title, "Defualts Loaded Error", title, "title");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", seatCount);
            node.AddValue("title", title);
        }

        public void CheckMaxSeatCount(Vessel vessel)
        {
            int currentseats = FlightGlobals.ActiveVessel.GetCrewCapacity();

            if (currentseats >= seatCount)
            {
                base.SetComplete();
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
                //Debug.LogError("Current crew is " + currentcrew + " crew can't be over " + crewCount);
                if (currentcrew >= crewCount && currentcrew != 0)
                {
                    base.SetComplete();
                    //Debug.Log("Passed Crew Check");
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
