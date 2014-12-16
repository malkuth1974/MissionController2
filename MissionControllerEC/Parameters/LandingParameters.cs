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
        private bool updated = false;
        private bool OnLaunch = false;

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
            return "Land Vessel";
        }
        protected override string GetTitle()
        {
            if (AllowLandedWet)
                return "Land Your Vessel on " + targetBody.theName;
            else
                return "Land Your Vessel on " + targetBody.theName + ".  You must also only land on dry land, landing Wet will not count!";
        }

        protected override void OnRegister()
        {

            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onLaunch.Add(onLaunch);
                updated = true;
            }

        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onLaunch.Remove(onLaunch);
            }

        }
            
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active && OnLaunch)
            {
                if (AllowLandedWet)
                {
                    if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("Wet and dry landing accepted for contract");
                        }
                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && FlightGlobals.ActiveVessel.situation != Vessel.Situations.SPLASHED)
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("only dry landing accepted for this contract");
                        }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");          
            Tools.ContractLoadCheck(node, ref AllowLandedWet, true, AllowLandedWet , "wetland"); 
            Tools.ContractLoadCheck(node, ref OnLaunch,true,OnLaunch,"onlaunch");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("wetland", AllowLandedWet);
            node.AddValue("onlaunch", OnLaunch);
        }

        public void Landing(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                if (vessel.launchTime > this.Root.DateAccepted && FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    base.SetComplete();
                }                                   
            }           
        }
        public void onLaunch(EventReport er)
        {
            OnLaunch = true;
            Debug.LogWarning("Onlaunch event fired for landing parameter \n" + er.msg + "\n" + er.eventType + "\n" + "Landing is now HOT and can be applied.  This message is good, means you launched your vessel");
        }
    }
}
