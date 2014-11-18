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
        public bool SatelliteType1 = false;
        public bool SatelliteType2 = false;
        public bool SatelliteType3 = false;
        public bool SatelliteType4 = false;
        public double maxorbital = 0;
        public double minorbital = 0;
        public int totalContracts;
        public int TotalFinished;

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

        ContractParameter satellite1;
        ContractParameter satellite2;
        ContractParameter satellite3;
        ContractParameter satellite4;
        ContractParameter satellite5;
        ContractParameter satellite6;
        ContractParameter satellite7;
        ContractParameter satellite8;
        ContractParameter satellite9;       

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

            MaxAltitude = Tools.RandomNumber(70000, 400000);

            timeOnStation = Tools.RandomNumber(102400, 900000);

            MaxApA = Tools.RandomNumber(71000, 2500000);
            MinApA = MaxApA - 1000;
            MaxPeA = MaxApA - 1000;
            MinPeA = MinPeA - 1000;           

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

            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                this.satellite1 = this.AddParameter(new AltitudeGoal(targetBody, MaxAltitude), null);
                satellite1.SetFunds(5000, 5000, targetBody);
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
                Debug.Log("Loaded random orbital period satellite contract");
            }

            else if (SaveInfo.SatelliteTypeChoice == 2)
            {              
                this.satellite4 = this.AddParameter(new ApAOrbitGoal(targetBody,MaxApA,MinApA),null);
                satellite4.SetFunds(5000, 5000, targetBody);
                this.satellite5 = this.AddParameter(new PeAOrbitGoal(targetBody,MaxPeA,MinPeA),null);
                satellite5.SetFunds(8000, 8000, targetBody);
                this.satellite6= this.AddParameter(new Inclination(targetBody,MinInc, MaxInc), null);
                satellite6.SetFunds(10000, 5000, targetBody);
                Debug.Log("Loaded Inclination ApA and PeA Satellite contract");
            }

            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                maxorbital = st.contract_Orbital_Period_Max_InSeconds;
                minorbital = st.contract_Orbital_Period_Min_InSeconds;
                this.satellite7 = this.AddParameter(new OrbitalPeriod(targetBody,minorbital, maxorbital), null);
                satellite7.SetFunds(37000, 37000, targetBody);
                Debug.Log("Loaded KeoSync Orbit Satellite Contract");
            }

            else
            {
                Debug.LogWarning("Failed to load satellite contracts on Generation");
                return false;
            }

            this.satellite8 = this.AddParameter(new TimeCountdownOrbits(targetBody, timeOnStation, TOSName, false), null);
            satellite8.SetFunds(20000, 20000, targetBody);
            this.satellite9 = this.AddParameter(new PartGoal(sciPartname, scipartamount), null);
            satellite9.SetFunds(2000, 2000, targetBody);
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, "Small Repair Panel", partAmount, true), null);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(10000, 145000, 145000, targetBody);
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
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                return "Send our satellite to Eccentric orbit with specific parts and for amount of time we require";
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                return "Send our satellite to Specified Orbital Period with specific parts and for amount of time we require";
            }
            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                return "Send our satellite to Inclined orbit with specific parts and for amount of time we require";
            }
            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                return "Send our satellite to KeoSync orbit with specific parts and for amount of time we require";
            }
            else
                return "Send satellite to orbit";
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + " For specified Orbit " + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                return "Launch New Satellite to Eccentric Orbit For Specified Amount Of Time over " + targetBody.theName;
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                return "Launch New Satellite to Specified Orbital Period for Amount Of Time Indicated.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                return "Launch New Satellite to an Inclined Orbit for Amount Of Time Indicated.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                return "Launch New satellite to Keosynchronous orbit with specific parts and for amount of time we require";
            }
            else
                return "Launch A New satellite to orbit";
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "We would like you to deliver our Satellite to orbit, We have specific scientific parts we want added to this satellite. Please include a " + sciPartname +
                "\n\n" + "Contract Goals\n\n " + "1. Build a satellite (suggest placing docking port for future Repair contracts)\n 2. Include a " + sciPartname + " In the construction." +
                "\n 3. launch satellite to Contract Orbit Specified";
        }
        protected override string GetSynopsys()
        {
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                return "You must bring the satellite to the Indicated Orbital Height and then adjust to the Eccentricity that we have specified.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                return "For this contract bring satellite to the Indicated Maximum and Minumum orbital periods specified in the contract.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                return "For this contract launch the satellite into the correct Inclination specified and also Bring the satellite to the Max And Min Apoapsis and Periapsis";
            }
            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                return "A Keosynchronous Orbit is an orbit around Kerbin with an orbital period of one sidereal day, intentionally matching Kerbin's " +
                    "sidereal rotation period (approximately 6).";
            }
            else
                return "Send satellite to orbit";
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our satellite to orbit around " + targetBody.theName + " with the specialized part " + sciPartname +
                ".  We gained many scientific achievments with this mission and would like to thank you for your help";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            int pcount = int.Parse(node.GetValue("pCount"));
            partAmount = pcount;
            partName = (node.GetValue("pName"));
            crewCount = int.Parse(node.GetValue("crewcount"));

            sciPartname = node.GetValue("sciname");
            scipartamount = int.Parse(node.GetValue("sciamount"));
            timeOnStation = double.Parse(node.GetValue("timestation"));
            TOSName = node.GetValue("tosname");

            GMaxecc = double.Parse(node.GetValue("maxecc"));
            GMinecc = double.Parse(node.GetValue("minecc"));
            MaxAltitude = double.Parse(node.GetValue("altitude"));

            maxorbital = double.Parse(node.GetValue("maxorbital"));
            minorbital = double.Parse(node.GetValue("minorbital"));
            SatelliteType1 = bool.Parse(node.GetValue("randomcheck"));

            MaxInc = int.Parse(node.GetValue("maxinclination"));
            MinInc = int.Parse(node.GetValue("mininclination"));

            MaxApA = int.Parse(node.GetValue("maxApA"));
            MinApA = int.Parse(node.GetValue("minApA"));
            MaxPeA = int.Parse(node.GetValue("maxPeA"));
            MinPeA = int.Parse(node.GetValue("minPeA"));

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
            node.AddValue("randomcheck", SatelliteType1);

            node.AddValue("maxinclination", MaxInc);
            node.AddValue("mininclination", MinInc);

            node.AddValue("maxApA",MaxApA);
            node.AddValue("minApA",MinApA);
            node.AddValue("maxPeA",MaxPeA);
            node.AddValue("minPeA",MinPeA);
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
}
