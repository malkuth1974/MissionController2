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

namespace MissionControllerEC.MCEContracts
{
    public class MCE_Satellite_Contracts : Contract
    {
        Settings st = new Settings("Config.cfg");
        MissionControllerEC mc = new MissionControllerEC();
        CelestialBody targetBody = Planetarium.fetch.Home;      
        public double contractAMA = 0;
        public double contractINC = 0;
        public double contractAOP = 0;
        public int crewCount = 0;
        public bool techUnlocked = false;
        public float frequency = 0;
        public float moduletype = 0;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public string SatTypeName = "Communications";
        public int scipartamount = 1;
        public int scipartcount = 1;
        public string TOSName = Localizer.Format("#autoLOC_MissionController2_1000095");              		// #autoLOC_MissionController2_1000095 = We need this amount of time to conduct our studies\n 
        public int totalContracts;
        public int TotalFinished;
        private int trackStationNumber = 0;
        private int PolarStationNumber = 0;
        public string StationName = "None";
        public string satType = "None";
        public string satStoryDef = "none";
        public string contractNotes = "none";
        public string satTitlestring = "none";
        public string contractSynops = "none";
              
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
            //Debug.Log("MCE satType Mission Is " + satType);
        }
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
            System.Random rnd = new System.Random();
            int stationNumber = rnd.Next(1,4);
            SetTrackStationNumber(stationNumber);
            mc.CheckRandomSatelliteContractTypes();
            float OrbitRandomMult = Tools.RandomNumber(1, 5);
                                                  
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
                                             
            SatTypeValue();
            int frequencyTest = rnd.Next(12, 40);
            frequency = (float)frequencyTest - .5f;
            moduletype = rnd.Next(1, 4);
            int AtmoDepth = Tools.RandomNumber(1, 8);
            contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, (float)AtmoDepth);
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                //contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 25000) *10 ;
                //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                contractINC = 0;
                contractAOP = Tools.getAOPCalc(contractAMA, 2.1);
                SatTypeName = "Communication";               

                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                if (randomPolar > 75)
                {
                    stationNumber = Tools.RandomNumber(5, 6);
                    SetTrackStationNumber(stationNumber);                    
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, 90, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency,true), null);
                    base.prestige = ContractPrestige.Exceptional;
                }
                else
                {                  
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, contractINC, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 4), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);                
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                SatTypeName = "Weather";
                //contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 10000) * 10;
                //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                contractINC = Tools.RandomNumber(-8, 8);
                contractAOP = Tools.getAOPCalc(contractAMA, 2.1);
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 70)
                {
                    //contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 10000) * 10;
                    //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                    contractINC = 90;                  
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, 90, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
                    this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                }
                else
                {                  
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, contractINC, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 6), null);
                    this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                }
            }

            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                SatTypeName = "Navigation";
                /*contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 17000) * 10*/;
                //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                contractINC = 0;
                contractAOP = Tools.getAOPCalc(contractAMA, 2.1);
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 75)
                {
                    stationNumber = Tools.RandomNumber(5, 6);
                    SetTrackStationNumber(stationNumber);                  
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, 90, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);                    
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, true), null);
                    base.prestige = ContractPrestige.Exceptional;
                }
                else
                {                  
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, contractINC, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                
            }

            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                SatTypeName = "Research";
                //contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 17000) * 10;
                //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                contractINC = Tools.RandomNumber(-10, 10);
                contractAOP = Tools.getAOPCalc(contractAMA, 2.3);
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 90)
                {
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, 90, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
                }
                else
                {
                    this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, contractINC, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
            }
            else if (SaveInfo.SatelliteTypeChoice == 4)
            {
                //contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 120000) * 10; ;  //Needs Fixing
                //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                contractINC = 0;
                contractAOP = Tools.getAOPCalc(contractAMA, 2);
                SatTypeName = "Communication";
                this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, contractINC, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
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
            else if (SaveInfo.SatelliteTypeChoice == 5)
            {
                //contractAMA = Tools.RandomNumber((int)AtmoDepth, (int)AtmoDepth + 120000) * 10; ;  //Needs Fixing
                //contractAMA = FinePrint.Utilities.CelestialUtilities.GetMinimumOrbitalDistance(targetBody, 2f);
                contractINC = 0;
                contractAOP = Tools.getAOPCalc(contractAMA, 2);
                SatTypeName = "Navigation";
                this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, contractINC, .01, contractAMA, 99, contractAOP, 1, 0, targetBody, 3), null);
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
            //this.AddParameter(new GetCrewCount(crewCount), null);
            //this.AddParameter(new FinePrint.Contracts.Parameters.VesselSystemsParameter(), null);
            if (HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().VesselMustSurvive == true)
            {
                this.OnDestroy = this.AddParameter(new VesselMustSurvive(), null);
                this.OnDestroy.DisableOnStateChange = false;
            }

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(2500 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 40000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 40000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);
            base.SetReputation(15, 30, targetBody);
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
            //Debug.Log("values for satellites have been reset");
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
            Tools.ContractLoadCheck(node, ref contractAMA, 71000, contractAMA, "ApA");           
            Tools.ContractLoadCheck(node, ref contractINC, 70500, contractINC, "PeA");
            Tools.ContractLoadCheck(node, ref satType, "Communication", satType, "sattype");
            Tools.ContractLoadCheck(node, ref satStoryDef, "None Loaded", satStoryDef, "satstory");
            Tools.ContractLoadCheck(node, ref contractNotes, "No load", contractNotes, "satnote");
            Tools.ContractLoadCheck(node, ref satTitlestring, "No Title Loaded", satTitlestring, "sattitle");
            Tools.ContractLoadCheck(node, ref contractSynops, "No Synopsys Loaded", contractSynops, "satsynop");
            Tools.ContractLoadCheck(node, ref SatTypeName, "CommunicationsCore", SatTypeName, "sattypename");
            Tools.ContractLoadCheck(node, ref trackStationNumber, 106, trackStationNumber, "tracknumber");
            Tools.ContractLoadCheck(node, ref PolarStationNumber, 0, PolarStationNumber, "polarstation");
            Tools.ContractLoadCheck(node, ref StationName, "Did Not Load", StationName, "stationname");
            Tools.ContractLoadCheck(node, ref contractAOP, 99, contractAOP, "contractAOP");

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
            node.AddValue("ApA",contractAMA);          
            node.AddValue("PeA",contractINC);
            node.AddValue("sattype", satType);
            node.AddValue("satstory", satStoryDef);
            node.AddValue("satnote", contractNotes);
            node.AddValue("sattitle", satTitlestring);
            node.AddValue("satsynop", contractSynops);
            node.AddValue("tracknumber", trackStationNumber);
            node.AddValue("polarstation", PolarStationNumber);
            node.AddValue("stationname", StationName);
            node.AddValue("contractAOP", contractAOP);
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
