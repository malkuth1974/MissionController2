using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP.Localization;

namespace MissionControllerEC.PartModules
{
    class MCELanderResearch : PartModule
    {
        [KSPField(isPersistant = true)]
        public static bool doLanderResearch = false;      
       
        Vessel vs = new Vessel();

        [KSPField(isPersistant = false, guiActive = true, guiName = "Rover Landed:")]
        public bool roverlanded = false;
       
        [KSPField(isPersistant = false, guiActive = true, guiName = "Rover Landed Wet:")]
        public bool roverlandedWet = false;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Starting Mass Spectrometry:")]
        public bool scanStart = false;

        [KSPEvent(guiActive = true, guiName = "Mass Spectrometry(Contract)", active = true)]
        public void StartResearchMCE()
        {
            checkVesselResearch();
        }

        [KSPAction("Start Mass Spectrometry Analysis")]
        public void ToggleAction(KSPActionParam param)
        {            
            StartResearchMCE();            
        }

        public void checkVesselResearch()
        {
            if (roverlanded != false || roverlandedWet != false)
            {
                doLanderResearch = true;
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000246"));		// #autoLOC_MissionController2_1000246 = Starting Mass spectrometry Analysis of Ground Level, Please Stand By...
            }
            else
            {
                doLanderResearch = false;
                scanStart = false;
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000247"));		// #autoLOC_MissionController2_1000247 = Vessel needs to be landed to start Mass spectrometry Analysis
            }
        }

        public override void OnStart(PartModule.StartState state)
        {
            this.part.force_activate();
        }

        public override void OnUpdate()
        {

            if (FlightGlobals.fetch.activeVessel.situation.Equals(Vessel.Situations.LANDED))
            {
                roverlanded = true;
            }
            else { roverlanded = false; }

            if (FlightGlobals.fetch.activeVessel.situation.Equals(Vessel.Situations.SPLASHED))
            {
                roverlandedWet = true;
            }
            else { roverlandedWet = false; }

            if (doLanderResearch == true)
            {
                scanStart = true;
            }
            else { scanStart = false; }
        }
    }
}
