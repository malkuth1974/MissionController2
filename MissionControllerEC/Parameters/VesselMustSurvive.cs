using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{
    class VesselMustSurvive: ContractParameter
    {            
        private bool updated = false;
        private bool VesselAlive = true;      

        public VesselMustSurvive()
        {
        }       
        protected override string GetHashString()
        {
            return "vessel must survive or contract fail";
        }
        protected override string GetTitle()
        {
            return "Vessel Must Survive While Contract Is Active!";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onCrash.Add(vesselDestroyed);
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
                Debug.LogError("Events fired for Keep Vessel Alive Build");
            }
        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onCrash.Remove(vesselDestroyed);
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                Debug.LogError("Events fired for Keep Vessel Alive Debuild");
            }
        }
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (this.state == ParameterState.Incomplete && VesselAlive == true)
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.launchTime > this.Root.DateAccepted)
                    {
                        base.SetComplete();
                    }
                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight && HighLogic.LoadedSceneIsFlight && VesselAlive == false)
                    {
                        base.SetIncomplete();
                        base.SetFailed();
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            //Tools.ContractLoadCheck(node, ref vesselID, "None Recorded", vesselID, "vesselID");            
        }
        protected override void OnSave(ConfigNode node)
        {
            //node.AddValue("vesselID", vesselID);
        }

        public void vesselDestroyed(EventReport er)
        {
            if (FlightGlobals.ActiveVessel)
            {
                VesselAlive = false;
                Debug.LogError("Vessel Recorded as destroyed in contract vessleAlive = " + VesselAlive);
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
}
