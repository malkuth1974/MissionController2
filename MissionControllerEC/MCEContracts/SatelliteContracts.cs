using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using System.Text;
using KSPAchievements;
using MissionControllerEC.MCEParameters;
using KSP.Localization;
using static MissionControllerEC.RegisterToolbar;

namespace MissionControllerEC.MCEContracts
{
    public class MCE_Satellite_Contracts : Contract
    {
        #region fields
        Settings st = new Settings("Config.cfg");
        //MissionControllerEC mc; // = new MissionControllerEC();
        CelestialBody targetBody = Planetarium.fetch.Home;            
        public int crewCount = 0, partAmount = 1, scipartamount = 1, scipartcount = 1, trackStationNumber = 0, PolarStationNumber = 0;
        public bool techUnlocked = false;
        public float frequency = 0, moduletype = 0;
        public string partName = "Repair Panel";
        public string SatTypeName = "Communications";
        public string TOSName = Localizer.Format("#autoLOC_MissionController2_1000095");              		// #autoLOC_MissionController2_1000095 = We need this amount of time to conduct our studies\n 
        public int totalContracts, TotalFinished;
        public string StationName = "None";
        public string satType = "None";
        public string satStoryDef = "none";
        public string contractNotes = "none";
        public string satTitlestring = "none";
        public string contractSynops = "none";
        #endregion

        public  MCE_Satellite_Contracts()
        {
#if false
            mc = MissionControllerEC.Instance;
            if (mc == null)
            {
                Log.Info("MCE_Satellite_Contracts allocating MissionControllerED");
                mc = new MissionControllerEC();
            }
#endif
        }
#region switch 1      
        public void SatTypeValue()
        {
       
            switch (SaveInfo.SatelliteTypeChoice)
            {
                case 0:
                    satType = "Communications";
                    TOSName = Localizer.Format("#autoLOC_MissionController2_1000096");		// #autoLOC_MissionController2_1000096 = Communications link up will need this amount of time to Be established\n 
                    satStoryDef = Localizer.Format("#autoLOC_MissionController2_1000097") +		// #autoLOC_MissionController2_1000097 = Communications satellites provide a worldwide linkup of radio, telephone, and television. The first (Earth) communications satellite was Echo 1 ; launched in 1960, it was a large metallized 
                              Localizer.Format("#autoLOC_MissionController2_1000098") +		// #autoLOC_MissionController2_1000098 = balloon that reflected radio signals striking it. This passive mode of operation quickly gave way to the active or repeater mode, in which complex electronic equipment aboard the satellite 
                              Localizer.Format("#autoLOC_MissionController2_1000099");		// #autoLOC_MissionController2_1000099 = receives a signal from the earth, amplifies it, and transmits it to another point on the earth, in this case Kerbin.
                    contractNotes = Localizer.Format("#autoLOC_MissionController2_1000100");		// #autoLOC_MissionController2_1000100 = You can set Satellite Type the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n
                    contractSynops = Localizer.Format("#autoLOC_MissionController2_1000101") + 		// #autoLOC_MissionController2_1000101 = You must bring the satellite to the specified orbit(Below The ApA, And Above The PeA) with Module type And Frequency. Set your satellite values in the Editor before taking Off, 
                               Localizer.Format("#autoLOC_MissionController2_1000102") + 		// #autoLOC_MissionController2_1000102 = Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to 
                               Localizer.Format("#autoLOC_MissionController2_1000103");		// #autoLOC_MissionController2_1000103 = launch a new vessel
                    break;
                case 1:
                    satType = "Weather";
                    TOSName = Localizer.Format("#autoLOC_MissionController2_1000104");		// #autoLOC_MissionController2_1000104 = To study our target weather patterns we need this amount of Obital time\n 
                    satStoryDef = Localizer.Format("#autoLOC_MissionController2_1000105") +		// #autoLOC_MissionController2_1000105 = Weather satellites, or meteorological satellites, provide kerbin scientist continuous, up-to-date information about large-scale atmospheric conditions such as cloud cover and temperature profiles. 
                              Localizer.Format("#autoLOC_MissionController2_1000106") +		// #autoLOC_MissionController2_1000106 = Tiros 1, the first such (Earth) satellite, was launched in 1960; it transmitted infrared television pictures of the earth's cloud cover and was able to detect the development of hurricanes and to chart 
                              "their paths.";
                    contractNotes = Localizer.Format("#autoLOC_MissionController2_1000107");		// #autoLOC_MissionController2_1000107 = You can set set Satellite Type the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n
                    contractSynops = Localizer.Format("#autoLOC_MissionController2_1000108") +		// #autoLOC_MissionController2_1000108 = You must bring the  satellite to the specified orbit (Below The ApA, And Above The PeA) with Module type And Frequency. Set your satellite values in the Editor before taking Off, 
                               Localizer.Format("#autoLOC_MissionController2_1000109") +		// #autoLOC_MissionController2_1000109 = Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to 
                               Localizer.Format("#autoLOC_MissionController2_1000110");		// #autoLOC_MissionController2_1000110 = launch a new vessel
                    break;
                case 2:
                    satType = "Navigational";
                    TOSName = Localizer.Format("#autoLOC_MissionController2_1000111");		// #autoLOC_MissionController2_1000111 = To establish Naviagational aids we need this amount of time\n 
                    satStoryDef = Localizer.Format("#autoLOC_MissionController2_1000112") +		// #autoLOC_MissionController2_1000112 = Navigation satellites were developed primarily to satisfy the need for a navigation system that nuclear submarines could use to update their inertial navigation system. This led 
                                Localizer.Format("#autoLOC_MissionController2_1000113") +		// #autoLOC_MissionController2_1000113 = the (Earth) U.S. navy to establish the Transit program in 1958; the system was declared operational in 1962 after the launch of Transit 5A. Transit satellites provided a constant signal by which 
                                Localizer.Format("#autoLOC_MissionController2_1000114") +		// #autoLOC_MissionController2_1000114 = aircraft and ships could determine their positions with great accuracy.\n\n
                                Localizer.Format("#autoLOC_MissionController2_1000115");		// #autoLOC_MissionController2_1000115 = In kerbin society these satellites help with the day to day needs of most travel options for kerbin Land, Sea, Air Based navigation.
                    contractNotes = Localizer.Format("#autoLOC_MissionController2_1000117");		// #autoLOC_MissionController2_1000117 = You can set set Satellite Type the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n
                    contractSynops = Localizer.Format("#autoLOC_MissionController2_1000118") +		// #autoLOC_MissionController2_1000118 = You must bring the satellite to the specified orbit (Below The ApA, And Above The PeA) with Module type And Frequency. Set your satellite values in the Editor before taking Off, 
                               Localizer.Format("#autoLOC_MissionController2_1000119") +		// #autoLOC_MissionController2_1000119 = Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to 
                               Localizer.Format("#autoLOC_MissionController2_1000120");		// #autoLOC_MissionController2_1000120 = launch a new vessel
                    break;
                case 3:
                    satType = "Research";
                    TOSName = Localizer.Format("#autoLOC_MissionController2_1000121");		// #autoLOC_MissionController2_1000121 = Our research, test will take about this amount of time to complete\n 
                    satStoryDef = Localizer.Format("#autoLOC_MissionController2_1000122");		// #autoLOC_MissionController2_1000122 = Research satellites are designed to test different scientific studies while in the freedom of space. Away for the problems of Kerbin Ground studies
                    contractNotes = Localizer.Format("#autoLOC_MissionController2_1000123");		// #autoLOC_MissionController2_1000123 = You can set set Satellite Type the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n
                    contractSynops = Localizer.Format("#autoLOC_MissionController2_1000124") +		// #autoLOC_MissionController2_1000124 = You must bring the satellite to the specified orbit (Below The ApA, And Above The PeA) with Module type And Frequency. Set your satellite values in the Editor before taking Off, 
                               Localizer.Format("#autoLOC_MissionController2_1000125") +		// #autoLOC_MissionController2_1000125 = Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to 
                               Localizer.Format("#autoLOC_MissionController2_1000126");		// #autoLOC_MissionController2_1000126 = launch a new vessel
                    break;
                case 4:
                    satType = "Network Communications";
                    TOSName = Localizer.Format("#autoLOC_MissionController2_1000127");		// #autoLOC_MissionController2_1000127 = Establish communications network around Kerbin
                    satStoryDef = Localizer.Format("#autoLOC_MissionController2_1000128") +		// #autoLOC_MissionController2_1000128 = Launch 6 Communication satellites to orbit and build a satellite network that connects all the Ground stations around Kerbin. All 6 satellites have to be pointed at a certain Ground station to be counted 
                        Localizer.Format("#autoLOC_MissionController2_1000129");		// #autoLOC_MissionController2_1000129 = as part of the network.\n\n It's possible to launch multiple satellites with one vessel. If you do this, be sure to not use Symmetry mode when adding the Satellite cores. If you use Symmetry mode the frequencies will all be set to the same thing when adjusting them!
                    contractNotes = Localizer.Format("#autoLOC_MissionController2_1000130") +		// #autoLOC_MissionController2_1000130 = You have to launch 6 different satellites.  All 6 satellites have different frequencies.  But all other settings in the Satellite Cores are the same.  The Ground stations are connected by \n
                                Localizer.Format("#autoLOC_MissionController2_1000131") +		// #autoLOC_MissionController2_1000131 = individual frequencies, and have been placed in order for easy access and completion\n\n
                                Localizer.Format("#autoLOC_MissionController2_1000132") +		// #autoLOC_MissionController2_1000132 = PLEASE NOTE: There is no such thing as a KeoStationary Orbit around the polar regions, you won't be able to Keep  the lines of sight at all times with polar stations.  You can still connect to them \n
                                Localizer.Format("#autoLOC_MissionController2_1000133");		// #autoLOC_MissionController2_1000133 = to finish the polar objectives.  Those objectives will remain locked and won't disconnect. Like the Equatorial Ground stations will.\n\n
                    contractSynops = Localizer.Format("#autoLOC_MissionController2_1000134");		// #autoLOC_MissionController2_1000134 = Set up all satellite cores with right frequencies.  All cores have a different set of frequencies to adjust in each vessel.  Failure to do this will result in having to launch more vessels.
                    break;
                case 5:
                    satType = "Network Navigation System";
                    TOSName = Localizer.Format("#autoLOC_MissionController2_1000135");		// #autoLOC_MissionController2_1000135 = Establish navigation network around Kerbin
                    satStoryDef = Localizer.Format("#autoLOC_MissionController2_1000136") +		// #autoLOC_MissionController2_1000136 = Launch 6 Communication satellites to orbit and build a satellite network that connects all the Ground stations around Kerbin. All 6 satellites have to be pointed at a certain Ground station to be counted 
                        Localizer.Format("#autoLOC_MissionController2_1000137");		// #autoLOC_MissionController2_1000137 = as part of the network.\n\n It's possible to launch multiple satellites with one vessel. If you do this, be sure to not use Symmetry mode when adding the Satellite cores. If you use Symmetry mode the frequencies will all be set to the same thing when adjusting them!
                    contractNotes = Localizer.Format("#autoLOC_MissionController2_1000138") +		// #autoLOC_MissionController2_1000138 = You have to launch 6 different satellites.  All 6 satellites have different frequencies.  But all other settings in the Satellite Cores are the same.  The Ground stations are connected by \n
                                Localizer.Format("#autoLOC_MissionController2_1000139") +		// #autoLOC_MissionController2_1000139 = individual frequencies, and have been placed in order for easy access and completion\n\n
                                Localizer.Format("#autoLOC_MissionController2_1000140") +		// #autoLOC_MissionController2_1000140 = PLEASE NOTE: There is no such thing as a KeoStationary Orbit around the polar regions, you won't be able to Keep  the lines of sight at all times with polar stations.  You can still connect to them \n
                                Localizer.Format("#autoLOC_MissionController2_1000141");		// #autoLOC_MissionController2_1000141 = to finish the polar objectives.  Those objectives will remain locked and won't disconnect. Like the Equatorial Ground stations will.\n\n
                    contractSynops = Localizer.Format("#autoLOC_MissionController2_1000142");		// #autoLOC_MissionController2_1000142 = Set up all satellite cores with right frequencies.  All cores have a different set of frequencies to adjust in each vessel.  Failure to do this will result in having to launch more vessels.
                    break;
            }
            
        }
#endregion
#region Switch 2
        public void SetTrackStationNumber(int value)
        {
            switch (value)
            {
                case 1:
                    trackStationNumber = -74;
                    PolarStationNumber = 0;
                    StationName = "Kerbal Space Center";
                    break;
                case 2:
                    trackStationNumber = 16;
                    PolarStationNumber = 0;
                    StationName = "East Shore Station";
                    break;
                case 3:
                    trackStationNumber = 106;
                    PolarStationNumber = 0;
                    StationName = "Heart Station";
                    break;
                case 4:
                    trackStationNumber = -164;
                    PolarStationNumber = 0;
                    StationName = "Crator Station";
                    break;
                case 5:
                    trackStationNumber = 0;
                    PolarStationNumber = 89;
                    StationName = "North Pole Station";
                    break;
                case 6:
                    trackStationNumber = 0;
                    PolarStationNumber = -89;
                    StationName = "South Pole Station";
                    break;
                default:
                    trackStationNumber = 106;
                    break;
            }
        }
#endregion
#region ContractGenerate
        ContractParameter OnDestroy;
        
        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().SatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<MCE_Satellite_Contracts>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<MCE_Satellite_Contracts>().Count();

            if (totalContracts >= HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().SatelliteContractNumbers)
            {
                return false;
            }
            
            //New random Planet the contract will select.. It checks first to see if player has visited.. If not cannont get a Sat contract to planet until you do.
            int randomTargetBody;
            randomTargetBody = Tools.RandomNumber(1, 100);
            if (randomTargetBody > HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)  // Higher the number the Lower chance to get a planet other than kerbin
            {
                targetBody = GetUnreachedTargets();
                if (targetBody == null)
                {
                    //Debug.LogWarning("Orbital Research Has No Valid Target bodies contract rejected");
                    return false;  //Just incase something bad happens and doesn't match contract is terminated and won't generate.
                }
            }
            //Setting up the ground stations (random)
            int stationNumber = SaveInfo.rnd.Next(1,4);
            SetTrackStationNumber(stationNumber);
            MissionControllerEC.CheckRandomSatelliteContractTypes();
            
            //Just checking to make sure the player has the Tech to do these contracts.
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            
            // Setting up the Satellite types/Freq/Modules
            SatTypeValue();
            int frequencyTest = SaveInfo.rnd.Next(12, 40);
            frequency = (float)frequencyTest - .5f;
            moduletype = SaveInfo.rnd.Next(1, 4);

            //Just the random seed number used for Orbits in fineprint.. Effects Random Nubmers for Altitudes...
            //Random Diffuculty for alltitude.

            //Finding the Min Orbit Height for the Contract payout later.
            double minSMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);  
            
            //Build New orbits Using KSP Build Orbits.. Simple inclanations becuase of ground stations           
            Orbit o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.EQUATORIAL,.1, 0, 0);
            //Using Fineprint to double check its own calculations.
            FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.EQUATORIAL, .1, 0);
            //Log.Info("MCE Orbit Values for satellite Contracts: " + " APA " + o.ApA + " PEA " + o.PeA + " Seed Number " + MissionSeed.ToString());

            if (SaveInfo.SatelliteTypeChoice == 0)  // Using the switch to check which type of contract to load (Random) Repeated for all Satellite types.
            {

                SatTypeName = "Communication";               

                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 75)
                {
                    //Have To Rebuild Orbit for Polar.. Still pretty simple no real inclination changes yet because of ground stations.
                    o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.POLAR, .4, 1, 1);
                    FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.POLAR, .1, 0);

                    stationNumber = Tools.RandomNumber(5, 6);
                    SetTrackStationNumber(stationNumber);
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.POLAR,o.inclination,o.eccentricity,o.semiMajorAxis,o.LAN,o.argumentOfPeriapsis,o.meanAnomalyAtEpoch,o.epoch, targetBody, 3), null);
                    if (randomTargetBody < HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)  // We don't want a Ground station Parameter for anything other than Kerbin.  
                    {
                        this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, true), null);
                    }
                    base.prestige = ContractPrestige.Exceptional;
                }
                else
                {
                    //This is default Orbit that is Equatorial in nature. Uses default orbit and does not have to be rebuilt.  Repeats this pattern for all Satellite Types
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);
                    if (randomTargetBody < HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)
                    {
                        this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                    }
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);     //The satellite core check make sure all match.           
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                SatTypeName = "Weather";
                
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 70)
                {
                    o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.POLAR, .2, 1, 1);
                    FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.POLAR, .1, 0);

                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.POLAR, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 3), null);
                    this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                }
                else
                {
                    o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.POLAR, .2, 1, 1);
                    FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.POLAR, .1, 0);
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.RANDOM, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);
                    this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                }
            }

            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                SatTypeName = "Navigation";
               
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 75)
                {
                    o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.POLAR, .1, 0, 0);
                    FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.POLAR, .1, 0);
                    stationNumber = Tools.RandomNumber(5, 6);
                    SetTrackStationNumber(stationNumber);
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.POLAR, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 3), null);
                    if (randomTargetBody < HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)
                    {
                        this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, true), null);
                    }
                    base.prestige = ContractPrestige.Exceptional;
                }
                else
                {
                    
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);
                    if (randomTargetBody < HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)
                    {
                        this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                    }
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                
            }

            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                SatTypeName = "Research";

                int randomPolar;
                int randomResearchPlanet;
                randomPolar = Tools.RandomNumber(1, 100);
                randomResearchPlanet = Tools.RandomNumber(1, 100);
                if (randomPolar > 90)
                {
                    o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.POLAR, .1, 2, 2);
                    FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.POLAR, .1, 0);
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.POLAR, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 3), null);
                }
                else
                {
                    o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.RANDOM, .1, 2, 2);
                    FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.POLAR, .1, 0);
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.RANDOM, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
            }
            else if (SaveInfo.SatelliteTypeChoice == 4 && randomTargetBody < HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)
            {
               
                SatTypeName = "Communication";              
                this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);
                if (frequency >= 50)
                {
                    frequency = -10;
                }
                ContractParameter Network1 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                Network1.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("Kerbal Space Center", -74, 0, frequency, false), null);
                ContractParameter Network2 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 1, moduletype, targetBody), null);
                Network2.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("East Shore Station", 16, 0, frequency + 1, false), null);
                ContractParameter Network3 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 2, moduletype, targetBody), null);
                Network3.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("Heart Station", 106, 0, frequency + 2, false), null);
                ContractParameter Network4 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 3, moduletype, targetBody), null);
                Network4.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("Crator Station", -164, 0, frequency + 3, false), null);
                ContractParameter Network5 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 4, moduletype, targetBody), null);
                Network5.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("North Pole Station", 0, 89, frequency + 4, true), null);
                ContractParameter Network6 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 5, moduletype, targetBody), null);
                Network6.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("South Pole Station", 0, -89, frequency + 5, true), null);
            }
            else if (SaveInfo.SatelliteTypeChoice == 5 && randomTargetBody < HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCESatOwPercentChance)
            {
              
                SatTypeName = "Navigation";
                this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);
                if (frequency >= 50)
                {
                    frequency = -10;
                }
                ContractParameter Network1 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                Network1.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("Kerbal Space Center", -74, 0, frequency, false), null);
                ContractParameter Network2 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 1, moduletype, targetBody), null);
                Network2.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("East Shore Station", 16, 0, frequency + 1, false), null);
                ContractParameter Network3 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 2, moduletype, targetBody), null);
                Network3.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("Heart Station", 106, 0, frequency + 2, false), null);
                ContractParameter Network4 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 3, moduletype, targetBody), null);
                Network4.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("Crator Station", -164, 0, frequency + 3,false), null);
                ContractParameter Network5 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 4, moduletype, targetBody), null);
                Network5.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("North Pole Station", 0, 89, frequency + 4, true), null);
                ContractParameter Network6 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 5, moduletype, targetBody), null);
                Network6.SetFunds(32000, 32000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                this.AddParameter(new GroundStationPostion("South Pole Station", 0, -89, frequency + 5,true), null);
            }

            else
            {
                Debug.LogWarning("Failed to load satellite contracts on Generation");
                return false;
            }            
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, "Small Repair Panel", partAmount, true), null);
            }           
            if (HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().VesselMustSurvive == true)
            {
                this.OnDestroy = this.AddParameter(new VesselMustSurvive(), null);
                this.OnDestroy.DisableOnStateChange = false;
            }

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);

            // Simple Height check for Contract Payouts.  Sure I could do it better.. But it seems to work.
            if (o.ApA < minSMA + 500000)
            {
                base.SetFunds(2500 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 30000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 30000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                Log.Info("Satellite Contract Base Pay Less than Type 1of4");
            }
            if (o.ApA > minSMA + 500000 && o.ApA < minSMA + 800000)
            {
                base.SetFunds(5500 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 65000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 65000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                Log.Info("Satellite Contract Base Pay Less than Type 2of4");
            }
            if (o.ApA > minSMA + 800000 && o.ApA < minSMA + 2000000)
            {
                base.SetFunds(10500 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 75000* HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 75000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                Log.Info("Satellite Contract Base Pay Less than Type 3of4");
            }
            else
            {               
                 base.SetFunds(12500 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 95000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 95000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
                Log.Info("Satellite Contract Base Pay Less than Type 4of4");
            }

            base.SetReputation(15, 30, targetBody);
            return true;
        }
#endregion
#region ContractOverrides
        public override bool CanBeCancelled()
        {
            return true;
        }
        public override bool CanBeDeclined()
        {
            return true;
        }

        protected override string GetNotes()
        {
            return contractNotes;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + Localizer.Format("#autoLOC_MissionController2_1000143") + " " + Localizer.Format("#autoLOC_MissionController2_1000144") + TotalFinished + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000143 =  For specified Orbit 		// #autoLOC_MissionController2_1000144 =  - Total Done: 
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000145") + " " + satType + " " + Localizer.Format("#autoLOC_MissionController2_1000146") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000145 = Launch new 		// #autoLOC_MissionController2_1000146 =  Satellite 
        }
        protected override string GetDescription()
        {
            return satStoryDef;
        }
        protected override string GetSynopsys()
        {
            return contractSynops;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.SatelliteContractType = "None";
            SaveInfo.SatelliteContractModule = 0;
            SaveInfo.SatelliteContractFrequency = -1;
            //Log.Info("values for satellites have been reset");
            return Localizer.Format("#autoLOC_MissionController2_1000147") + " " + satType + " " + Localizer.Format("#autoLOC_MissionController2_1000148") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000147 = You have successfully delivered our 		// #autoLOC_MissionController2_1000148 =  satellite to orbit around 
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");           
            Tools.ContractLoadCheck(node, ref partAmount, 1, partAmount, "pCount");            
            Tools.ContractLoadCheck(node, ref partName, "Default", partName, "pName");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");      
            Tools.ContractLoadCheck(node, ref scipartamount, 1, scipartamount, "sciamount");           
            Tools.ContractLoadCheck(node, ref TOSName, "Default", TOSName, "tosname");                     
            Tools.ContractLoadCheck(node, ref moduletype, 30, moduletype, "moduletype");
            Tools.ContractLoadCheck(node, ref frequency, 1, frequency, "frequency");           
            Tools.ContractLoadCheck(node, ref satType, "Communication", satType, "sattype");
            Tools.ContractLoadCheck(node, ref satStoryDef, "None Loaded", satStoryDef, "satstory");
            Tools.ContractLoadCheck(node, ref contractNotes, "No load", contractNotes, "satnote");
            Tools.ContractLoadCheck(node, ref satTitlestring, "No Title Loaded", satTitlestring, "sattitle");
            Tools.ContractLoadCheck(node, ref contractSynops, "No Synopsys Loaded", contractSynops, "satsynop");
            Tools.ContractLoadCheck(node, ref SatTypeName, "CommunicationsCore", SatTypeName, "sattypename");
            Tools.ContractLoadCheck(node, ref trackStationNumber, 106, trackStationNumber, "tracknumber");
            Tools.ContractLoadCheck(node, ref PolarStationNumber, 0, PolarStationNumber, "polarstation");
            Tools.ContractLoadCheck(node, ref StationName, "Did Not Load", StationName, "stationname");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);           
            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("crewcount", crewCount);
            node.AddValue("sattypename", SatTypeName);
            node.AddValue("sciamount", scipartamount);
            node.AddValue("tosname", TOSName);           
            node.AddValue("moduletype", moduletype);
            node.AddValue("frequency", frequency);            
            node.AddValue("sattype", satType);
            node.AddValue("satstory", satStoryDef);
            node.AddValue("satnote", contractNotes);
            node.AddValue("sattitle", satTitlestring);
            node.AddValue("satsynop", contractSynops);
            node.AddValue("tracknumber", trackStationNumber);
            node.AddValue("polarstation", PolarStationNumber);
            node.AddValue("stationname", StationName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("basicScience") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            if (techUnlock && techUnlock2)
                return true;
            else
                return false;
        }
        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(true, false);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
            }
            else
            {
                return null;
            }
            return null;
        }
#endregion

    }

    public class EarlyContracts : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 1;
        CelestialBody targetBody; 
        public string BiomeName;
        public int BiomeNumber = 0;
        public float PaymentMultipllier = 1f;
        public int totalContracts;
        public int TotalFinished;
        
        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<EarlyContracts>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<EarlyContracts>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().EarlyMCEContracts)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            BiomeNumber = Tools.RandomNumber(0, 8);
            switch (BiomeNumber)
            {
                case 0:
                    BiomeName = "Grasslands";
                    PaymentMultipllier = 1f;
                    break;
                case 1:
                    BiomeName = "Highlands";
                    PaymentMultipllier = 1.1f;
                    break;
                case 2:
                    BiomeName = "Mountains";
                    PaymentMultipllier = 1.1f;
                    break;
                case 3:
                    BiomeName = "Deserts";
                    PaymentMultipllier = 1.3f;
                    break;
                case 4:
                    BiomeName = "Badlands";
                    PaymentMultipllier = 1.4f;
                    break;
                case 5:
                    BiomeName = "Tundra";
                    PaymentMultipllier = 1.4f;
                    break;
                case 6:
                    BiomeName = "Ice Caps";
                    PaymentMultipllier = 1.8f;
                    break;
                case 7:
                    BiomeName = "Water";
                    PaymentMultipllier = .6f;
                    break;
                case 8:
                    BiomeName = "Shores";
                    PaymentMultipllier = .2f;
                    break;
                default:
                    BiomeName = "Shores";
                    PaymentMultipllier = .2f;
                    break;
            }
            targetBody = Planetarium.fetch.Home;

            this.AddParameter(new BiomLandingParameters(Planetarium.fetch.Home, true, BiomeName), null);
            this.AddParameter(new CollectScience(targetBody, BodyLocation.Surface), null);

            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(5000f * PaymentMultipllier, 30000f * PaymentMultipllier, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(5f * PaymentMultipllier, targetBody);
            base.SetScience(1f * PaymentMultipllier, targetBody);
            return true;
        }

        public override bool CanBeCancelled()
        {
            return true;
        }
        public override bool CanBeDeclined()
        {
            return true;
        }
      
        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000149") + " " + targetBody + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000149 = Launch and land on Biomes of 
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000150") + " " + targetBody.bodyName + Localizer.Format("#autoLOC_MissionController2_1000151");		// #autoLOC_MissionController2_1000150 = Land on specific Biome on 		// #autoLOC_MissionController2_1000151 = , Then collect science
        }
        protected override string GetDescription()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000152") + " " + targetBody.bodyName + Localizer.Format("#autoLOC_MissionController2_1000153") + " " + BiomeName + Localizer.Format("#autoLOC_MissionController2_1000154") + " " + crew + " " + Localizer.Format("#autoLOC_MissionController2_1000155");		// #autoLOC_MissionController2_1000152 = Land a vessel on 		// #autoLOC_MissionController2_1000153 =  in the biome called 		// #autoLOC_MissionController2_1000154 = . Then collect science to complete the contract. You must have at least 		// #autoLOC_MissionController2_1000155 =  crew member on your vessel
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000156") + " " + targetBody.bodyName + ". " + BiomeName + " " + Localizer.Format("#autoLOC_MissionController2_1000157");		// #autoLOC_MissionController2_1000156 = Land at specific Body and Biome we Request 		// #autoLOC_MissionController2_1000157 =  Then collect science.
        }
        protected override string MessageCompleted()
        {          
            return Localizer.Format("#autoLOC_MissionController2_1000158");		// #autoLOC_MissionController2_1000158 = Great job landing at the biome we specified.  The science we have gathered will help us in the future to bring kerbals deeper into our solar system!
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref crew, 1, crew, "crewcount");
            Tools.ContractLoadCheck(node, ref BiomeName, "Test", BiomeName, "biomename");
            Tools.ContractLoadCheck(node, ref PaymentMultipllier, 1.0f, PaymentMultipllier, "paymentmult");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("crewcount", crew);
            node.AddValue("biomename", BiomeName);
            node.AddValue("paymentmult", PaymentMultipllier);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("heavyRocketry") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("basicScience") == RDTech.State.Available;
            bool techUnlock3 = ResearchAndDevelopment.GetTechnologyState("basicRocketry") == RDTech.State.Available;
            if (techUnlock && techUnlock2)
            {
                return false;
            }
            else if (techUnlock3)
            {
                return true;
            }
            else
                return false;
        }
    }
}
