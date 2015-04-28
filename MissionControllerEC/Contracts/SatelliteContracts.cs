using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using System.Text;
using KSPAchievements;
namespace MissionControllerEC
{
    public class SatelliteContracts : Contract
    {
        Settings st = new Settings("Config.cfg");
        MissionControllerEC mc = new MissionControllerEC();
        CelestialBody targetBody = Planetarium.fetch.Home;
        public double GMaxecc = 0;
        public double GMinecc = 0;
        public double MaxApA = 0;
        public double MinApA = 0;
        public double MinPeA = 0;
        public double MaxPeA = 0;
        public double MaxAltitude = 0;
        public int crewCount = 0;
        public List<TechList> techlist = new List<TechList>();
        public bool techUnlocked = false;
        public double MinInc = 0;
        public double MaxInc = 0;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public string sciPartname = "Communotron 16";
        public int scipartamount = 1;
        public int scipartcount = 1;
        public int scipartFinalcount;
        public double timeOnStation;
        public string TOSName = "We need this amount of time to conduct our studies\n ";      
        public double maxorbital = 0;
        public double minorbital = 0;
        public int totalContracts;
        public int TotalFinished;

        public string satType = "None";
        public int satTypeNumber = 0;
        public string satStoryDef = "none";
        public string satNotesString = "none";
        public string satTitlestring = "none";
        public string satSynopsysString = "none";

        public void loadscienceparts()
        {
            foreach (AvailablePart ap in PartLoader.LoadedPartsList)
            {
                if (ap.category == PartCategories.Science)
                {
                    if (ResearchAndDevelopment.GetTechnologyState(ap.TechRequired) == RDTech.State.Available)
                    {
                        techlist.Add(new TechList(ap.title));
                    }
                }
            }
        }
        public void SatTypeValue()
        {
            satTypeNumber = Tools.RandomNumber(0, 3);
            switch (satTypeNumber)
            {
                case 0:
                    satType = "Communications";
                    TOSName = "Communications link up will need this amount of time to Be established\n ";
                    break;
                case 1:
                    satType = "Weather";
                    TOSName = "To study our target weather patterns we need this amount of Obital time\n ";
                    break;
                case 2:
                    satType = "Navigational";
                    TOSName = "To establish Naviagational aids we need this amount of time\n ";
                    break;
                case 3:
                    satType = "Applications";
                    TOSName = "Our research test will take about this amount of time to complete\n ";
                    break;
            }
            Debug.Log("MCE satType Mission Is " + satType);
        }

        public void satDefValue()
        {
            switch (satTypeNumber)
            {
                case 0:
                    satStoryDef = "Communications satellites provide a worldwide linkup of radio, telephone, and television. The first (Earth) communications satellite was Echo 1 ; launched in 1960, it was a large metallized " +
                        "balloon that reflected radio signals striking it. This passive mode of operation quickly gave way to the active or repeater mode, in which complex electronic equipment aboard the satellite " +
                        "receives a signal from the earth, amplifies it, and transmits it to another point on the earth, in this case Kerbin.";
                    break;
                case 1:
                    satStoryDef = "Weather satellites, or meteorological satellites, provide kerbin scientist continuous, up-to-date information about large-scale atmospheric conditions such as cloud cover and temperature profiles. " +
                        "Tiros 1, the first such (Earth) satellite, was launched in 1960; it transmitted infrared television pictures of the earth's cloud cover and was able to detect the development of hurricanes and to chart " +
                        "their paths.";
                    break;
                case 2:
                    satStoryDef = "Navigation satellites were developed primarily to satisfy the need for a navigation system that nuclear submarines could use to update their inertial navigation system. This led " +
                        "the (Earth) U.S. navy to establish the Transit program in 1958; the system was declared operational in 1962 after the launch of Transit 5A. Transit satellites provided a constant signal by which " +
                        "aircraft and ships could determine their positions with great accuracy.\n\n"+
                        "In kerbin society these satellites help with the day to day needs of most travel options for kerbin Land, Sea, Air Based navigation.";
                    break;
                case 3:
                    satStoryDef = "Applications satellites are designed to test ways of improving satellite technology itself. Areas of concern include structure, instrumentation, controls, power supplies, and " +
                        "telemetry for future communications, meteorological, and navigation satellites.";
                    break;
            }
            Debug.Log("Story type: " + satType + " Chosen for satellite contract");
        }

        public void satExpValues()
        {
            switch (SaveInfo.SatelliteTypeChoice)
            {
                case 0:
                    satNotesString ="Launch our satellite into an eccentric orbit with the specific parts and for the amount of time we require.\n\nVessel must be a new vessel launched after accepting the contract.";
                    satSynopsysString="You must bring the satellite to the indicated orbital altitude and then adjust to the eccentricity that we have specified.";
                    satTitlestring ="Launch new " + satType + " Satellite " + targetBody.theName;
                    break;
                case 1:
                    satNotesString ="Launch our satellite into the specified orbital period with the specific parts and for the amount of time we require.\n\nVessel must be a new vessel launched after accepting the contract.";
                    satSynopsysString="For this contract bring the satellite to the indicated maximum and minimum orbital periods specified in the contract.";
                    satTitlestring ="Launch new  "+ satType + " Satellite";
                    break;
                case 2:
                    if (MinInc > 90)
                        {
                        satNotesString = "Launch our satellite into a retrograde inclined orbit of " + MinInc + " and " + MaxInc + " with the desired apogee and perigee.\n\nVessel must be a new vessel launched after accepting the contract.";
                        }
                    else
                        {
                        satNotesString ="Launch our satellite on a prograde inclined orbit of " + MinInc + " and " + MaxInc + " with desired apogee and perigee.\n\nVessel must be a new vessel launched after accepting the contract.";
                        }
                    satSynopsysString="For this contract, launch the satellite into the correct inclination specified, and also bring the satellite to the specified apoapsis and periapsis";
                    satTitlestring ="Launch new "  + satType + " satellite";
                    break;
                case 3:
                    satNotesString ="Launch our satellite into a Keosynchronous orbit with the specified parts and for the amount of time we require.\n\nVessel must be a new vessel launched after accepting the contract.";
                    satSynopsysString="A Keosynchronous orbit is an orbit around Kerbin with an orbital period of one sidereal day, intentionally matching Kerbin's " +
                    "sidereal rotation period (approximately 6 hours).";
                    satTitlestring ="Launch new " + satType + " satellite";
                    break;
            }
            Debug.Log("MCE contract completion text loaded type: " + SaveInfo.SatelliteTypeChoice);
        }
        
        ContractParameter satellite1;
        ContractParameter satellite2;
        ContractParameter satellite3;
        ContractParameter satellite4;
        ContractParameter satellite5;
        ContractParameter satellite6;
        ContractParameter satellite7;
        ContractParameter satellite8;
        ContractParameter satellite9;
        ContractParameter OnDestroy;

        protected override bool Generate()
        {                      
            //if (prestige == ContractPrestige.Trivial)
            //{
            //    return false;
            //}
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.NoSatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SatelliteContracts>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<SatelliteContracts>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            techlist.Clear();
            loadscienceparts();
            scipartcount = techlist.Count();
            scipartFinalcount = Tools.RandomNumber(0, scipartcount);
            sciPartname = techlist[scipartFinalcount].techName;

            mc.CheckRandomSatelliteContractTypes();

            GMinecc = UnityEngine.Random.Range(0f, .9f);
            GMaxecc = GMinecc + .09f;

            MaxAltitude = Tools.RandomNumber((int)Tools.getBodyAltitude(targetBody) + 1000, (int)Tools.getBodyAltitude(targetBody) + 300000);

            timeOnStation = Tools.RandomNumber(4800, 23700);

            MaxApA = Tools.RandomNumber((int)Tools.getBodyAltitude(targetBody) + 3000, (int)Tools.getBodyAltitude(targetBody) + 300000);
            MinApA = MaxApA - 2000;
            MaxPeA = MaxApA;
            MinPeA = MaxPeA - 2000;         

            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            int bodyrandom = Tools.RandomNumber(1, 3);
            int bodyrandomchance = Tools.RandomNumber(0, 100);
            if (bodyrandomchance > 70 && SaveInfo.SatelliteTypeChoice == 0)
            {
                targetBody = FlightGlobals.Bodies[bodyrandom];
            }
            else
            {
                targetBody = Planetarium.fetch.Home;
            }
            SatTypeValue();
            satDefValue();
            satExpValues();

            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                this.satellite1 = this.AddParameter(new AltitudeGoal(targetBody, MaxAltitude), null);
                satellite1.SetFunds(5000, 5000, targetBody);
                this.satellite1.DisableOnStateChange = false;
                this.satellite2 = this.AddParameter(new EccentricGoal(targetBody,GMinecc, GMaxecc), null);
                satellite2.SetFunds(8000, 8000, targetBody);
                satellite2.DisableOnStateChange = false;
                Debug.Log("Loaded Eccentric Satellite contract");
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                minorbital = Tools.RandomNumber((int)st.contract_Random_Orbital_Period_MinInSeconds, (int)st.contract_Random_Orbital_Period_MaxInSeconds);
                maxorbital = minorbital + st.contract_Random_Orbital_Period_Difference;
                this.satellite3 = this.AddParameter(new OrbitalPeriod(targetBody,minorbital, maxorbital), null);
                if (maxorbital > 300000)
                {
                    satellite3.SetFunds(37000, 37000, targetBody);
                }
                else
                {
                    satellite3.SetFunds(18000, 18000, targetBody);
                }
                this.satellite3.DisableOnStateChange = false;
                Debug.Log("Loaded random orbital period satellite contract");
            }

            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                MinInc = Tools.RandomNumber(0, 180);
                MaxInc = MinInc + 5;
                this.satellite4 = this.AddParameter(new ApAOrbitGoal(targetBody,MaxApA,MinApA),null);
                satellite4.SetFunds(5000, 5000, targetBody);
                this.satellite4.DisableOnStateChange = false;
                this.satellite5 = this.AddParameter(new PeAOrbitGoal(targetBody,MaxPeA,MinPeA),null);
                satellite5.SetFunds(8000, 8000, targetBody);
                this.satellite5.DisableOnStateChange = false;
                this.satellite6= this.AddParameter(new Inclination(targetBody,MinInc, MaxInc), null);
                satellite6.SetFunds(10000, 5000, targetBody);
                this.satellite6.DisableOnStateChange = false;
                Debug.Log("Loaded Inclination ApA and PeA Satellite contract");
            }

            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                maxorbital = st.contract_Orbital_Period_Max_InSeconds;
                minorbital = st.contract_Orbital_Period_Min_InSeconds;
                this.satellite7 = this.AddParameter(new OrbitalPeriod(targetBody,minorbital, maxorbital), null);
                satellite7.SetFunds(37000, 37000, targetBody);
                this.satellite7.DisableOnStateChange = false;
                Debug.Log("Loaded KeoSync Orbit Satellite Contract");
            }

            else
            {
                Debug.LogWarning("Failed to load satellite contracts on Generation");
                return false;
            }

            this.satellite8 = this.AddParameter(new TimeCountdownOrbits(targetBody, timeOnStation, TOSName, false), null);
            satellite8.SetFunds(20000, 20000, targetBody);
            this.satellite9 = this.AddParameter(new PartGoal(sciPartname, "Small Repair Panel", scipartamount,true), null);
            satellite9.SetFunds(2000, 2000, targetBody);
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, "Small Repair Panel", partAmount, true), null);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);
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
            return satNotesString;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + " For specified Orbit " + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return satTitlestring;
        }
        protected override string GetDescription()
        {
            return satStoryDef;
        }
        protected override string GetSynopsys()
        {
            return satSynopsysString;
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our " + satType + " satellite to orbit around " + targetBody.theName + " with the required " + sciPartname +
                ".  We've accomplished many scientific achievements with this mission and would like to thank you for your help.";
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");           
            Tools.ContractLoadCheck(node, ref partAmount, 1, partAmount, "pCount");            
            Tools.ContractLoadCheck(node, ref partName, "Default", partName, "pName");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");      
            Tools.ContractLoadCheck(node, ref scipartamount, 1, scipartamount, "sciamount");           
            Tools.ContractLoadCheck(node, ref timeOnStation, 25000, timeOnStation, "timestation");
            Tools.ContractLoadCheck(node, ref TOSName, "Default", TOSName, "tosname");
            Tools.ContractLoadCheck(node, ref GMaxecc, 71000, GMaxecc, "maxecc");
            Tools.ContractLoadCheck(node, ref GMinecc, 70500, GMinecc, "minecc");
            Tools.ContractLoadCheck(node, ref MaxAltitude, 71500, MaxAltitude, "altitude");
            Tools.ContractLoadCheck(node, ref maxorbital, 71000, maxorbital, "maxorbital");
            Tools.ContractLoadCheck(node, ref minorbital, 70500, minorbital, "minorbital");            
            Tools.ContractLoadCheck(node, ref MaxInc, 30, MaxInc, "maxinclination");
            Tools.ContractLoadCheck(node, ref MinInc, 1, MinInc, "mininclination");
            Tools.ContractLoadCheck(node, ref MaxApA, 71000, MaxApA, "maxApA");
            Tools.ContractLoadCheck(node, ref MinApA, 75000, MinApA, "minApA");
            Tools.ContractLoadCheck(node, ref MaxPeA, 71000, MaxPeA, "maxPeA");
            Tools.ContractLoadCheck(node, ref MinPeA, 70500, MinPeA, "minPeA");
            Tools.ContractLoadCheck(node, ref satType, "Communications", satType, "sattype");
            Tools.ContractLoadCheck(node, ref satStoryDef, "None Loaded", satStoryDef, "satstory");
            Tools.ContractLoadCheck(node, ref satTypeNumber, 0, satTypeNumber, "sattypenumber");
            Tools.ContractLoadCheck(node, ref satNotesString, "No load", satNotesString, "satnote");
            Tools.ContractLoadCheck(node, ref satTitlestring, "No Title Loaded", satTitlestring, "sattitle");
            Tools.ContractLoadCheck(node, ref satSynopsysString, "No Synopsys Loaded", satSynopsysString, "satsynop");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("maxecc", GMaxecc);
            node.AddValue("minecc", GMinecc);
            node.AddValue("altitude", MaxAltitude);
            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("crewcount", crewCount);
            node.AddValue("sciname", sciPartname);
            node.AddValue("sciamount", scipartamount);
            node.AddValue("timestation", timeOnStation);
            node.AddValue("tosname", TOSName);
            node.AddValue("maxorbital", maxorbital);
            node.AddValue("minorbital", minorbital);
            node.AddValue("maxinclination", MaxInc);
            node.AddValue("mininclination", MinInc);
            node.AddValue("maxApA",MaxApA);
            node.AddValue("minApA",MinApA);
            node.AddValue("maxPeA",MaxPeA);
            node.AddValue("minPeA",MinPeA);
            node.AddValue("sattype", satType);
            node.AddValue("sattypenumber", satTypeNumber);
            node.AddValue("satstory", satStoryDef);
            node.AddValue("satnote", satNotesString);
            node.AddValue("sattitle", satTitlestring);
            node.AddValue("satsynop", satSynopsysString);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
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
            return "Launch and land on Biomes of " + targetBody;
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
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
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
