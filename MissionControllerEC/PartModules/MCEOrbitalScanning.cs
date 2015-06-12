using System;

namespace MissionControllerEC.PartModules
{
    class MCEOrbitalScanning : PartModule
    {
        [KSPField(isPersistant = true)]
        public static bool doOrbitResearch = false;      
       
        Vessel vs = new Vessel();

        [KSPField(isPersistant = false, guiActive = true, guiName = "Ionization chamber ready to scan:")]
        private bool probeOrbitResearch = false;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Starting Scan:")]
        private bool scanStart = false;

        [KSPEvent(guiActive = true, guiName = "Ionization Scan(Contract)", active = true)]
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
                ScreenMessages.PostScreenMessage("Ionization Chamber Filling, Please Stand By...");
            }
            else
            {
                doOrbitResearch = false;
                scanStart = false;
                ScreenMessages.PostScreenMessage("Vessel Needs to be In Orbit to Conduct Ionization Scan");
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
