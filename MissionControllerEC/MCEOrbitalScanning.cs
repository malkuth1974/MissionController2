using System;

namespace MissionControllerEC
{
    class MCEOrbitalScanning : PartModule
    {
        [KSPField(isPersistant = true)]
        public static bool doOrbitResearch = false;      
       
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

        [KSPAction("Start Scanning")]
        public void ToggleAction(KSPActionParam param)
        {
            StartResearchMCE();
        }

        public void checkVesselResearch()
        {
            if (probeOrbitResearch == true)
            {
                doOrbitResearch = true;
                ScreenMessages.PostScreenMessage("Starting Orbital Research, Please Stand By...");
            }
            else
            {
                doOrbitResearch = false;
                scanStart = false;
                ScreenMessages.PostScreenMessage("Vessel Needs to be In Orbit to Conduct Scans");
            }
        }

        public override void OnUpdate()
        {
            if (doOrbitResearch == true)
            {
                scanStart = true;
            }
            else { scanStart = false; }

            if(FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                probeOrbitResearch = true;
            }
            else 
                probeOrbitResearch = false;
        }
    }
    
}
