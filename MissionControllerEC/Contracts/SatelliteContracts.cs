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

            MaxAltitude = Tools.RandomNumber((int)st.contract_Satellite_MIn_Height, (int)st.contract_Satellite_Max_Height);

            timeOnStation = Tools.RandomNumber(102400, 900000);

            MaxApA = Tools.RandomNumber((int)st.contract_Satellite_MIn_Height, (int)st.contract_Satellite_Max_Height);
            MinApA = MaxApA - st.contract_Satellite_Between_Difference;
            MaxPeA = MaxApA - st.contract_Satellite_Between_Difference;
            MinPeA = MaxPeA - st.contract_Satellite_Between_Difference;           

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
                MinInc = Tools.RandomNumber(0, 180);
                MaxInc = MinInc + 5;
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
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                return "Launch our satellite into an eccentric orbit with the specific parts and for the amount of time we require.\n\nVessel must be a new vessel launched after accepting the contract.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                return "Launch our satellite into the specified orbital period with the specific parts and for the amount of time we require.\n\nVessel must be a new vessel launched after accepting the contract.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                if (MinInc > 90)
                {
                    return ("Launch our satellite into a retrograde inclined orbit of " + MinInc + " and " + MaxInc + " with the desired apogee and perigee.\n\nVessel must be a new vessel launched after accepting the contract.");
                }
                else
                {
                    return ("Launch our satellite on a prograde inclined orbit of " + MinInc + " and " + MaxInc + " with desired apogee and perigee.\n\nVessel must be a new vessel launched after accepting the contract.");
                }
            }
            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                return "Launch our satellite into a Keosynchronous orbit with the specified parts and for the amount of time we require.\n\nVessel must be a new vessel launched after accepting the contract.";
            }
            else
                return "Launch our satellite into orbit.\n\nVessel must be a new vessel launched after accepting the contract.";
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + " For specified Orbit " + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                return "Launch new satellite to eccentric orbit for specified amount of time over " + targetBody.theName;
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                return "Launch new satellite to specified orbital period for amount of time indicated.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                return "Launch new satellite into an Inclined orbit for amount of time indicated.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                return "Launch new satellite into Keosynchronous orbit with specified parts and for amount of time we require";
            }
            else
                return "Launch a new satellite into orbit";
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "We would like you to deliver our satellite to orbit, We have specific scientific parts we want added to this satellite. Please include a " + sciPartname + "."
                "\n\n" + "Contract Goals\n\n " + "1. Build a satellite (we recommend you place a docking port for future Repair contracts)\n 2. Include a " + sciPartname + " on the satellite." +
                "\n 3. Launch satellite into the orbit specified ";
        }
        protected override string GetSynopsys()
        {
            if (SaveInfo.SatelliteTypeChoice == 0)
            {
                return "You must bring the satellite to the indicated orbital altitude and then adjust to the eccentricity that we have specified.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 1)
            {
                return "For this contract bring the satellite to the indicated maximum and minimum orbital periods specified in the contract.";
            }
            else if (SaveInfo.SatelliteTypeChoice == 2)
            {
                return "For this contract, launch the satellite into the correct inclination specified, and also bring the satellite to the specified apoapsis and periapsis";
            }
            else if (SaveInfo.SatelliteTypeChoice == 3)
            {
                return "A Keosynchronous orbit is an orbit around Kerbin with an orbital period of one sidereal day, intentionally matching Kerbin's " +
                    "sidereal rotation period (approximately 6 hours).";
            }
            else
                return "Send satellite to orbit";
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our satellite to orbit around " + targetBody.theName + " with the required " + sciPartname +
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
