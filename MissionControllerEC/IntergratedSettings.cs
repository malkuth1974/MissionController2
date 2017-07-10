using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using System;

namespace MissionControllerEC
{
    public class IntergratedSettings : GameParameters.CustomParameterNode
    {

        public override string Title { get { return "Mission Controller Contracts"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.CAREER; } }
        public override string Section { get { return "MissionControllerEC"; } }
        public override string DisplaySection { get { return "MissionControllerEC"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Historic Mission Enabled?", toolTip = "Do you want to play historical Missions?")]
        public bool HistoricalContracts = true;
        [GameParameters.CustomParameterUI("Apollo Extra Mission Enabled?", toolTip = "If playing apollo Historical Contracts Do you want Fictional Missions after 17?")]
        public bool ApolloExtraContent = true;
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
    public class IntergratedSettings2 : GameParameters.CustomParameterNode
    {

        public override string Title { get { return "Stock Game Contracts"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.CAREER; } }
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
    public class IntergratedSettings3 : GameParameters.CustomParameterNode
    {

        public override string Title { get { return "MCE Diffuculty Options"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.CAREER; } }
        public override string Section { get { return "MissionControllerEC"; } }
        public override string DisplaySection { get { return "MissionControllerEC"; } }
        public override int SectionOrder { get { return 3; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("MCE Revert Cost Enabled?", toolTip = "Do you want to be charged when using revert?")]
        public bool MCERevertAllow = true;
        [GameParameters.CustomIntParameterUI("Revert Cost",maxValue =50000)]
        public int MCERevertCost = 1000;
        [GameParameters.CustomParameterUI("MCE Vessel Must Surviv Enabled?", toolTip = "If vessel is destroyed in MCE contract you will fail?")]
        public bool VesselMustSurvive = true;
        [GameParameters.CustomParameterUI("MCE Flight Help Readout?", toolTip = "This is very simple APA,PEA, Readout, not needed if you have Another Mod that gives you these readouts?")]
        public bool MCEReadoutHelpAllow = true;
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


        //[GameParameters.CustomStringParameterUI("Test String UI", autoPersistance = true, lines = 2, title = "This is what should show Test string#1", toolTip = "test string tooltip")]
        //public string UIstring = "";

        //[GameParameters.CustomFloatParameterUI("My Float", displayFormat = "N0", minValue = 0.0f, maxValue = 50.0f)]
        //public double MyFloat = 1.0f;

        //[GameParameters.CustomIntParameterUI("My Integer", maxValue = 10)]
        //public int MyInt = 1;

        //[GameParameters.CustomIntParameterUI("My Non-Sandbox Integer", gameMode = GameParameters.GameMode.CAREER | GameParameters.GameMode.SCIENCE)]
        //public int MyCareerInt = 1;

        //[GameParameters.CustomIntParameterUI("My New Game Integer", newGameOnly = true)]
        //public int MyNewGameInt = 1;

        //[GameParameters.CustomFloatParameterUI("My Percentage", asPercentage = true)]
        //public double MyPercentage = 0.5f;

        //[GameParameters.CustomIntParameterUI("My Stepped Integer", maxValue = 500000, stepSize = 1000)]
        //public int MySteppedInt = 1000;

        //[GameParameters.CustomParameterUI("MCEDifficulty")]
        //public MCEDifficulty MyEnum = MCEDifficulty.MEDIUM;

        //[GameParameters.CustomIntParameterUI("My Int Property", minValue = 100, maxValue = 500)]
        //public int MyProperty { get { return myPropertyBackingField; } set { myPropertyBackingField = value; } }        

        //private int myPropertyBackingField = 100;
        //[GameParameters.CustomParameterUI("My IList Field", toolTip = "ILIST field values")]
        //public string MyIlist = "";

        //public override void SetDifficultyPreset(GameParameters.Preset preset)
        //{
        //    Debug.Log("Setting difficulty preset");
        //    switch (preset)
        //    {
        //        case GameParameters.Preset.Easy:
        //            MyInt = 1;
        //            break;

        //        case GameParameters.Preset.Normal:
        //            MyInt = 2;
        //            break;

        //        case GameParameters.Preset.Moderate:
        //            MyInt = 3;
        //            break;

        //        case GameParameters.Preset.Hard:
        //            MyInt = 4;
        //            break;
        //    }
        //}       

        //public override IList ValidValues(MemberInfo member)
        //{
        //    if (member.Name == "MyIlist")
        //    {
        //        List<string> myList = new List<string>();
        //        foreach (CelestialBody cb in FlightGlobals.Bodies)
        //        {
        //            myList.Add(cb.name);
        //        }
        //        IList myIlist = myList;
        //        return myIlist;
        //    }
        //    else
        //    {
        //        return null;
        //    }
    }
}
