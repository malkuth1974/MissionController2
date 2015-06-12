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
namespace MissionControllerEC.MCEContracts
{
    public class SatelliteContracts : Contract
    {
        Settings st = new Settings("Config.cfg");
        MissionControllerEC mc = new MissionControllerEC();
        CelestialBody targetBody = Planetarium.fetch.Home;      
        public double contractApA = 0;
        public double contractPeA = 0;
        public int crewCount = 0;
        public bool techUnlocked = false;
        public float frequency = 0;
        public float moduletype = 0;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public string SatTypeName = "Communications";
        public int scipartamount = 1;
        public int scipartcount = 1;
        public string TOSName = "We need this amount of time to conduct our studies\n ";              
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
                    TOSName = "Communications link up will need this amount of time to Be established\n ";
                    satStoryDef = "Communications satellites provide a worldwide linkup of radio, telephone, and television. The first (Earth) communications satellite was Echo 1 ; launched in 1960, it was a large metallized " +
                              "balloon that reflected radio signals striking it. This passive mode of operation quickly gave way to the active or repeater mode, in which complex electronic equipment aboard the satellite " +
                              "receives a signal from the earth, amplifies it, and transmits it to another point on the earth, in this case Kerbin.";
                    contractNotes ="You can set both the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n";
                    contractSynops ="You must bring the satellite to the specified orbit with Module type And Frequency. Set your satellite values in the Editor before taking Off, "+ 
                               "Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to " + 
                               "launch a new vessel";
                    break;
                case 1:
                    satType = "Weather";
                    TOSName = "To study our target weather patterns we need this amount of Obital time\n ";
                    satStoryDef = "Weather satellites, or meteorological satellites, provide kerbin scientist continuous, up-to-date information about large-scale atmospheric conditions such as cloud cover and temperature profiles. " +
                              "Tiros 1, the first such (Earth) satellite, was launched in 1960; it transmitted infrared television pictures of the earth's cloud cover and was able to detect the development of hurricanes and to chart " +
                              "their paths.";
                    contractNotes = "You can set both the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n";
                    contractSynops = "You must bring the  satellite to the specified orbit with Module type And Frequency. Set your satellite values in the Editor before taking Off, " +
                               "Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to " +
                               "launch a new vessel";
                    break;
                case 2:
                    satType = "Navigational";
                    TOSName = "To establish Naviagational aids we need this amount of time\n ";
                    satStoryDef = "Navigation satellites were developed primarily to satisfy the need for a navigation system that nuclear submarines could use to update their inertial navigation system. This led " +
                                "the (Earth) U.S. navy to establish the Transit program in 1958; the system was declared operational in 1962 after the launch of Transit 5A. Transit satellites provided a constant signal by which " +
                                "aircraft and ships could determine their positions with great accuracy.\n\n" +
                                "In kerbin society these satellites help with the day to day needs of most travel options for kerbin Land, Sea, Air Based navigation.";
                    contractNotes = "You can set both the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n";
                    contractSynops = "You must bring the satellite to the specified orbit with Module type And Frequency. Set your satellite values in the Editor before taking Off, " +
                               "Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to " +
                               "launch a new vessel";
                    break;
                case 3:
                    satType = "Research";
                    TOSName = "Our research, test will take about this amount of time to complete\n ";
                    satStoryDef = "Research satellites are designed to test different scientific studies while in the freedom of space. Away for the problems of Kerbin Ground studies";
                    contractNotes = "You can set both the frequency and module type in the editor with Tweak Sliders. Set these values before takeoff, They cannot be changed after the vessel is launched!\n\n";
                    contractSynops = "You must bring the satellite to the specified orbit with Module type And Frequency. Set your satellite values in the Editor before taking Off, " +
                               "Once you get to your assigned orbital height transmit the Data packet to customers to complete the objectives. Be warned, you only have 1 shot to send the packet. If it's incorrect, you will have to " +
                               "launch a new vessel";
                    break;
                case 4:
                    satType = "Network Communications";
                    TOSName = "Establish communications network around Kerbin";
                    satStoryDef = "Launch 6 Communication satellites to orbit and build a satellite network that connects all the Ground stations around Kerbin. All 6 satellites have to be pointed at a certain Ground station to be counted " +
                        "as part of the network.\n\n It's possible to launch multiple satellites with one vessel. If you do this, be sure to not use Symmetry mode when adding the Satellite cores. If you use Symmetry mode the frequencies will all be set to the same thing when adjusting them!";
                    contractNotes ="You have to launch 6 different satellites.  All 6 satellites have different frequencies.  But all other settings in the Satellite Cores are the same.  The Ground stations are connected by \n" +
                                "individual frequencies, and have been placed in order for easy access and completion\n\n"+
                                "PLEASE NOTE: There is no such thing as a KeoStationary Orbit around the polar regions, you won't be able to Keep  the lines of sight at all times with polar stations.  You can still connect to them \n" +
                                "to finish the polar objectives.  Those objectives will remain locked and won't disconnect. Like the Equatorial Ground stations will.\n\n";
                    contractSynops = "Set up all satellite cores with right frequencies.  All cores have a different set of frequencies to adjust in each vessel.  Failure to do this will result in having to launch more vessels.";
                    break;
                case 5:
                    satType = "Network Navigation System";
                    TOSName = "Establish navigation network around Kerbin";
                    satStoryDef = "Launch 6 Communication satellites to orbit and build a satellite network that connects all the Ground stations around Kerbin. All 6 satellites have to be pointed at a certain Ground station to be counted " +
                        "as part of the network.\n\n It's possible to launch multiple satellites with one vessel. If you do this, be sure to not use Symmetry mode when adding the Satellite cores. If you use Symmetry mode the frequencies will all be set to the same thing when adjusting them!";
                    contractNotes = "You have to launch 6 different satellites.  All 6 satellites have different frequencies.  But all other settings in the Satellite Cores are the same.  The Ground stations are connected by \n" +
                                "individual frequencies, and have been placed in order for easy access and completion\n\n" +
                                "PLEASE NOTE: There is no such thing as a KeoStationary Orbit around the polar regions, you won't be able to Keep  the lines of sight at all times with polar stations.  You can still connect to them \n" +
                                "to finish the polar objectives.  Those objectives will remain locked and won't disconnect. Like the Equatorial Ground stations will.\n\n";
                    contractSynops = "Set up all satellite cores with right frequencies.  All cores have a different set of frequencies to adjust in each vessel.  Failure to do this will result in having to launch more vessels.";
                    break;
            }
            Debug.Log("MCE satType Mission Is " + satType);
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
            if (SaveInfo.NoSatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SatelliteContracts>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<SatelliteContracts>().Count();

            if (totalContracts >= st.Satellite_Contract_Per_Cycle)
            {
                return false;
            }
            System.Random rnd = new System.Random();
            int stationNumber = rnd.Next(1,4);
            SetTrackStationNumber(stationNumber);
            mc.CheckRandomSatelliteContractTypes();
                                                  
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
                                             
            SatTypeValue();
            int frequencyTest = rnd.Next(1, 50);
            frequency = (float)frequencyTest - .5f;
            moduletype = rnd.Next(1, 4);

            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                contractApA = 2868000;
                contractPeA = 2868000;
                SatTypeName = "Communication";
                
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 75)
                {
                    stationNumber = Tools.RandomNumber(5, 6);
                    SetTrackStationNumber(stationNumber);
                    this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "Polar"), null);
                    this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "Polar"), null);
                    this.AddParameter(new Inclination(targetBody,90), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency,true), null);
                    base.prestige = ContractPrestige.Exceptional;
                }
                else
                {
                    this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "KeoStationary"), null);
                    this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "KeoStationary"), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);                
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                SatTypeName = "Weather";
                contractApA = 2868000;
                contractPeA = 2868000;
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 70)
                {
                    contractApA = 850000;
                    contractPeA = 850000;
                    this.AddParameter(new Inclination(targetBody,90), null);
                    this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "Polar Orbit"), null);
                    this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "Polar Orbit"), null);
                    this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                }
                else
                {
                    this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "KeoStationary Orbit"), null);
                    this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "KeoStationary Orbit"), null);
                    this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                }
            }

            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                SatTypeName = "Navigation";
                contractApA = 2868000;
                contractPeA = 2868000;              
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 75)
                {
                    stationNumber = Tools.RandomNumber(5, 6);
                    SetTrackStationNumber(stationNumber);
                    this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "Polar"), null);
                    this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "Polar"), null);
                    this.AddParameter(new Inclination(targetBody,90), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, true), null);
                    base.prestige = ContractPrestige.Exceptional;
                }
                else
                {
                    this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "KeoStationary Orbit"), null);
                    this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "KeoStationary Orbit"), null);
                    this.AddParameter(new GroundStationPostion(StationName, trackStationNumber, PolarStationNumber, frequency, false), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                
            }

            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                SatTypeName = "Research";              
                contractApA = Tools.RandomNumber(75000,2868001);              
                contractPeA = contractApA - 1000;
                this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "Orbit"), null);
                this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "Orbit"), null);
                int randomPolar;
                randomPolar = Tools.RandomNumber(1, 100);
                if (randomPolar > 90)
                {
                    this.AddParameter(new Inclination(targetBody,90), null);
                }
                this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
            }
            else if (SaveInfo.SatelliteTypeChoice == 4)
            {
                contractApA = 2868000;
                contractPeA = 2868000;
                SatTypeName = "Communication";
                this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "KeoStationary/Polar Orbit"), null);
                this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "KeoStationary/Polar Orbit"), null);
                if (frequency >= 50)
                {
                    frequency = -10;
                }
                ContractParameter Network1 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                Network1.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("Kerbal Space Center", -74, 0, frequency, false), null);
                ContractParameter Network2 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 1, moduletype, targetBody), null);
                Network2.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("East Shore Station", 16, 0, frequency + 1, false), null);
                ContractParameter Network3 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 2, moduletype, targetBody), null);
                Network3.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("Heart Station", 106, 0, frequency + 2, false), null);
                ContractParameter Network4 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 3, moduletype, targetBody), null);
                Network4.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("Crator Station", -164, 0, frequency + 3, false), null);
                ContractParameter Network5 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 4, moduletype, targetBody), null);
                Network5.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("North Pole Station", 0, 89, frequency + 4, true), null);
                ContractParameter Network6 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 5, moduletype, targetBody), null);
                Network6.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("South Pole Station", 0, -89, frequency + 5, true), null);
            }
            else if (SaveInfo.SatelliteTypeChoice == 5)
            {
                contractApA = 2868000;
                contractPeA = 2868000;
                SatTypeName = "Navigation";
                this.AddParameter(new SatApAOrbitGoal(targetBody, contractApA, "KeoStationary/Polar Orbit"), null);
                this.AddParameter(new SatPeAOrbitGoal(targetBody, contractPeA, "KeoStationary/Polar Orbit"), null);
                if (frequency >= 50)
                {
                    frequency = -10;
                }
                ContractParameter Network1 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency, moduletype, targetBody), null);
                Network1.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("Kerbal Space Center", -74, 0, frequency, false), null);
                ContractParameter Network2 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 1, moduletype, targetBody), null);
                Network2.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("East Shore Station", 16, 0, frequency + 1, false), null);
                ContractParameter Network3 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 2, moduletype, targetBody), null);
                Network3.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("Heart Station", 106, 0, frequency + 2, false), null);
                ContractParameter Network4 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 3, moduletype, targetBody), null);
                Network4.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("Crator Station", -164, 0, frequency + 3,false), null);
                ContractParameter Network5 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 4, moduletype, targetBody), null);
                Network5.SetFunds(90000, 100000, targetBody);
                this.AddParameter(new GroundStationPostion("North Pole Station", 0, 89, frequency + 4, true), null);
                ContractParameter Network6 = this.AddParameter(new satelliteCoreCheck(SatTypeName, frequency + 5, moduletype, targetBody), null);
                Network6.SetFunds(90000, 100000, targetBody);
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
            this.AddParameter(new FinePrint.Contracts.Parameters.ProbeSystemsParameter(), null);
            if (SaveInfo.Hardcore_Vessel_Must_Survive == true)
            {
                this.OnDestroy = this.AddParameter(new VesselMustSurvive(), null);
                this.OnDestroy.DisableOnStateChange = false;
            }

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(10000, 145000, 160000, targetBody);
            base.SetReputation(25, 50, targetBody);
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
            return targetBody.bodyName + " For specified Orbit " + " - Total Done: " + TotalFinished + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Launch new " + satType + " Satellite " + targetBody.theName;
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
            Debug.Log("values for satellites have been reset");
            return "You have successfully delivered our " + satType + " satellite to orbit around " + targetBody.theName;
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
            Tools.ContractLoadCheck(node, ref contractApA, 71000, contractApA, "ApA");           
            Tools.ContractLoadCheck(node, ref contractPeA, 70500, contractPeA, "PeA");
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
            node.AddValue("ApA",contractApA);          
            node.AddValue("PeA",contractPeA);
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
            return "Launch and land on Biomes of " + targetBody + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Land on specific Biome on " + targetBody.theName + ", Then collect science";
        }
        protected override string GetDescription()
        {
            return "Land a vessel on " + targetBody.theName + " in the biome called " + BiomeName + ". Then collect science to complete the contract. You must have at least " + crew + " crew member on your vessel";
        }
        protected override string GetSynopsys()
        {
            return "Land at specific Body and Biome we Request " + targetBody.theName + ". " + BiomeName + " Then collect science.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Luna16Done = true;
            return "Great job landing at the biome we specified.  The science we have gathered will help us in the future to bring kerbals deeper into our solar system!";
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
