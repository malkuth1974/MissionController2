using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using System;
using KSP.UI.Screens;

namespace MissionControllerEC
{
    public class MCE_IntergratedSettings : GameParameters.CustomParameterNode
    {
        
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.CAREER; } }
        public override string Title { get { return "Mission Controller Contracts"; } }
        public override string Section { get { return "MissionControllerEC"; } }
        public override string DisplaySection { get { return "MissionControllerEC"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("MCE Satellite Contracts Enabled?", toolTip = "Do you want to play MCE Satellite Missions These use the MCE Part Cores.")]
        public bool SatelliteContracts = true;
        [GameParameters.CustomParameterUI("MCE Orbital Science Contracts Enabled?", toolTip = "These are contracts that use the MCE part Ionization Chamber.")]
        public bool OrbitalScienceContracts = true;
        [GameParameters.CustomParameterUI("MCE Ground Science Contracts Enabled?", toolTip = "These are contracts that use the MCE part Mass Spectrometry Tube.")]
        public bool GroundScienceContracts = true;
        [GameParameters.CustomParameterUI("MCE Repair Contracts Enabled?", toolTip = "These are the MCE repair contracts that use the Repair Panel and are generated using Satellite Contract Data.")]
        public bool RepairContracts = true;
        [GameParameters.CustomParameterUI("MCE Early Mission Enabled?", toolTip = "These are missions that come early in career and are biome based around kerbin.")]
        public bool EarlyMCEContracts = true;
        [GameParameters.CustomParameterUI("MCE Rover Contracts Eanbled?", toolTip = "These are rover random rover missions that MCE will send you on.")]
        public bool MCERoverContracts = true; 

        
    }
    public class MCE_IntergratedSettings2 : GameParameters.CustomParameterNode
    {
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.CAREER; } }
        public override string Title { get { return "Stock Game Contracts"; } }
        public override string Section { get { return "MissionControllerEC"; } }
        public override string DisplaySection { get { return "MissionControllerEC"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Rescue Kerbal Contracts Enabled?", toolTip = "Do you want to play the default rescue kerbal contracts?")]
        public bool RescueKerbalContracts = true;
        [GameParameters.CustomParameterUI("Part Test Contracts Enabled?", toolTip = "Do you want to play the default part test contracts?")]
        public bool PartTestContracts = true;
        [GameParameters.CustomParameterUI("Fine Print Satellite Contracts", toolTip = "Do you want to play the default satellite contracts?")]
        public bool FPSatelliteContracts = true;
        [GameParameters.CustomParameterUI("Fine Print Survey Contracts", toolTip = "Do you want to play the default survey contracts?")]
        public bool FPSurveyContracts = true;
        [GameParameters.CustomParameterUI("Fine Print Station Contracts", toolTip = "Do you want to play the default Station Build contracts?")]
        public bool FPStationContracts = true;
        [GameParameters.CustomParameterUI("Fine Print Base Contracts", toolTip = "Do you want to play the default Base Build contracts?")]
        public bool FPBaseContracts = true;
        [GameParameters.CustomParameterUI("Fine Print ISRU Contracts", toolTip = "Do you want to play the default ISRU contracts?")]
        public bool FPISRUContracts = true;
        [GameParameters.CustomParameterUI("Fine Print Tourism Contracts", toolTip = "Do you want to play the default Tourism Ridealong contracts?")]
        public bool FPTouricsmContracts = true;
        [GameParameters.CustomParameterUI("Grand Tour Contracts", toolTip = "Do you want to play the default Grand Tour contracts?")]
        public bool FPGrandTourContracts = true;


    }
    public class MCE_IntergratedSettings3 : GameParameters.CustomParameterNode
    {
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.CAREER; } }
        public override string Title { get { return "MCE Diffuculty Options"; } }        
        public override string Section { get { return "MissionControllerEC"; } }
        public override string DisplaySection { get { return "MissionControllerEC"; } }
        public override int SectionOrder { get { return 3; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("MCE Revert Cost Enabled?", toolTip = "Do you want to be charged when using revert?")]
        public bool MCERevertAllow = true;
        [GameParameters.CustomIntParameterUI("Revert Cost",maxValue =100)]
        public int MCERevertCost = 10;
        [GameParameters.CustomParameterUI("MCE Vessel Must Surviv Enabled?", toolTip = "If vessel is destroyed in MCE contract you will fail?")]
        public bool VesselMustSurvive = true;       
        [GameParameters.CustomParameterUI("MCE Debug Mode Enabled?", toolTip = "Debug mode becomes available in MCE Menu Icon (Cheats)?")]
        public bool MCEDebugMode = false;
        [GameParameters.CustomIntParameterUI("Max Number Satellite Contracts At Time", maxValue = 5)]
        public int SatelliteContractNumbers = 2;
        [GameParameters.CustomIntParameterUI("Max Number Science Contracts At Time", maxValue = 5)]
        public int ScienceContractNumbers= 1;
        [GameParameters.CustomFloatParameterUI("MCE Contract Payout Multiplier", maxValue = 3)]
        public float MCEContractPayoutMult = 1;
        [GameParameters.CustomIntParameterUI("Orbit Margin Error Contracts", maxValue = 10000)]
        public int MCEErrorOrbits = 5000;
        [GameParameters.CustomIntParameterUI("Inclination Margin Error Contracts", maxValue = 8)]
        public int MCEErrorInclintation = 2;
        [GameParameters.CustomFloatParameterUI("Eccentric Margin Error Contracts",minValue = 0,maxValue = 2)]
        public float MCEErrorEccentric = .5f;       
    }
}
