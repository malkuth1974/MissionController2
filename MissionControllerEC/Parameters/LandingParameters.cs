using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{
    class LandingParameters : ContractParameter
    {
        private CelestialBody targetBody;       
        private bool AllowLandedWet = true;

        public LandingParameters()
        {
        }

        public LandingParameters(CelestialBody target, bool wetLanding)
        {
            this.targetBody = target;
            this.AllowLandedWet = wetLanding;
        }
        protected override string GetHashString()
        {
            return "Land on " + targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            if (AllowLandedWet)
                return "Land Your Vessel on " + targetBody.theName;
            else
                return "Land Your Vessel on " + targetBody.theName + ".  You must also only land on dry land, landing Wet will not count!";
        }
       
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (AllowLandedWet)
                {
                    if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                        }
                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && FlightGlobals.ActiveVessel.situation != Vessel.Situations.SPLASHED)
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                        }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");          
            Tools.ContractLoadCheck(node, ref AllowLandedWet, true, AllowLandedWet , "wetland");           
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("wetland", AllowLandedWet);         
        }

        public void Landing(Vessel vessel)
        {
            if (vessel.mainBody.referenceBody.Equals(targetBody))
            {
                if (vessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    if (vessel.launchTime > this.Root.DateAccepted)
                        base.SetComplete();
                }
            }
        }       
    }
}
