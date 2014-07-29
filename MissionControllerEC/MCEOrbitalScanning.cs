using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControllerEC
{
    class MCEOrbitalScanning : PartModule
    {
        [KSPField(isPersistant = false)]
        public static bool doResearch = false;

        [KSPField(isPersistant = true)]
        private bool IsEnabled = false;

        Vessel vs = new Vessel();

        [KSPField(isPersistant = false, guiActive = true, guiName = "Probe Ready To Scan:")]
        private bool probeOrbitResearch = false;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Starting Scan:")]
        private bool scanStart = false;

        [KSPEvent(guiActive = true, guiName = "Start MCE Orbital Research", active = true)]
        public void StartResearchMCE()
        {
            checkVesselResearch();
        }

        public void checkVesselResearch()
        {
            if (probeOrbitResearch == true)
            {
                doResearch = true;
                ScreenMessages.PostScreenMessage("Starting Orbital Research, Please Stand By...");
            }
            else
            {
                doResearch = false;
                scanStart = false;
                ScreenMessages.PostScreenMessage("Vessel Needs to be In Orbit to Conduct Scans");
            }
        }

        public override void OnFixedUpdate()
        {
            if (doResearch == true)
            {
                scanStart = true;
            }
            else { scanStart = false; }

            if(FlightGlobals.ActiveVessel.situation.Equals(Vessel.Situations.ORBITING))
            {
                probeOrbitResearch = true;
            }
            else 
                probeOrbitResearch = false;
        }
    }
    
}
