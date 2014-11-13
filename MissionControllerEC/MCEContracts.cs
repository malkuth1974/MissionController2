using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using KSPAchievements;


namespace MissionControllerEC
{
    #region Contract DeliverSatellit
    public class DeliverSatellite : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;        
        public double GMaxApA = 0;
        public double GMinApA = 0;
        public double GMaxPeA = 0;
        public double GMinPeA = 0;
        public float maxTon = 0;
        public double capResources;
        public string ResourceName = "ElectricCharge";
        public int crewCount = 0;
        public bool techUnlocked = false;
        public double MinInc = 0;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public double MaxInc = UnityEngine.Random.Range(10, 90);
        public int test = UnityEngine.Random.Range(0, 100);
        public int totalContracts;
        public int TotalFinished;
        ContractParameter satellite1;
        ContractParameter satellite2;
        ContractParameter satellite3;
            
        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.NoSatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<DeliverSatellite>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<DeliverSatellite>().Count();

            //Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.Log("contract is generated right now terminating Normal Satellite Mission");
                //Debug.Log("count is " + totalContracts);
                return false;                 
            }
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            int bodyrandom = UnityEngine.Random.Range(1, 3);
            int bodyrandomchance = UnityEngine.Random.Range(0, 100);
            if (bodyrandomchance > 80)
            {
                targetBody = FlightGlobals.Bodies[bodyrandom];
            }
            else
            {
                targetBody = Planetarium.fetch.Home;
            }                     
            MinInc = MaxInc - 10;
            if (this.prestige == ContractPrestige.Trivial)
            {
                maxTon = UnityEngine.Random.Range(st.contrac_Satellite_Min_Mass_Trivial, st.contrac_Satellite_Max_Mass_Trivial);
                capResources = UnityEngine.Random.Range(2, 5) * 100;
                GMaxApA = UnityEngine.Random.Range((int)st.contrac_Satellite_Max_ApA_Trivial, (int)st.contrac_Satellite_Max_Total_Height_Trivial + (int)st.contrac_Satellite_Max_ApA_Trivial);
                GMinApA = GMaxApA - st.contrac_Satellite_Between_Difference;
                GMaxPeA = GMaxApA;
                GMinPeA = GMinApA;
                base.SetFunds(35000f * st.Contract_Payment_Multiplier, 56000f * st.Contract_Payment_Multiplier, 35000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(15f, 35f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Significant)
            {
                maxTon = UnityEngine.Random.Range(st.contrac_Satellite_Min_Mass_Significant, st.contrac_Satellite_Max_Mass_Significant);
                capResources = UnityEngine.Random.Range(3, 9) * 100;
                if (targetBody.flightGlobalsIndex == 2 || targetBody.flightGlobalsIndex == 3)
                {
                    GMaxApA = UnityEngine.Random.Range(75000,150000);
                }
                else
                {
                    GMaxApA = UnityEngine.Random.Range((int)st.contrac_Satellite_Max_ApA_Significant, (int)st.contrac_Satellite_Max_Total_Height_Significant + (int)st.contrac_Satellite_Max_ApA_Significant);
                }
                GMinApA = GMaxApA - st.contrac_Satellite_Between_Difference;
                GMaxPeA = GMaxApA;
                GMinPeA = GMinApA;
                base.SetFunds(45000f * st.Contract_Payment_Multiplier, 78000f * st.Contract_Payment_Multiplier, 45000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(25f, 45f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Exceptional)
            {
                maxTon = UnityEngine.Random.Range(st.contrac_Satellite_Min_Mass_Except, st.contrac_Satellite_Max_Mass_Except);
                capResources = UnityEngine.Random.Range(5, 13) * 100;
                if (targetBody.flightGlobalsIndex == 2 || targetBody.flightGlobalsIndex == 3)
                {
                    GMaxApA = UnityEngine.Random.Range(35000, 120000);
                }
                else
                {
                    GMaxApA = UnityEngine.Random.Range((int)st.contrac_Satellite_Max_ApA_Except, (int)st.contrac_Satellite_Max_Total_Height_Except + (int)st.contrac_Satellite_Max_ApA_Except);
                }
                GMinApA = GMaxApA - st.contrac_Satellite_Between_Difference;
                GMaxPeA = GMaxApA;
                GMinPeA = GMinApA;
                base.SetFunds(55000f * st.Contract_Payment_Multiplier, 95000f * st.Contract_Payment_Multiplier, 55000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(35f, 345f, targetBody);
            }
            bool ifInclination = false;

            if (test >= 50)
                ifInclination = true;
            else
                ifInclination = false;            
            this.AddParameter(new PreLaunch(), null);

            this.satellite1 = this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA), null);
            satellite1.SetFunds(2000, targetBody);
            satellite1.DisableOnStateChange = false;

            this.satellite2 = this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA), null);
            satellite2.SetFunds(2000, targetBody);
            satellite2.DisableOnStateChange = false;

            if (ifInclination && targetBody.flightGlobalsIndex == 1)
            {
               this.satellite3 = this.AddParameter(new Inclination(MinInc, MaxInc));
               satellite3.SetFunds(8000, targetBody);
               satellite3.SetReputation(10, targetBody);
               satellite3.DisableOnStateChange = false;
            }

            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }

            this.AddParameter(new TotalMasGoal(targetBody, maxTon), null);
            this.AddParameter(new ResourceGoalCap(ResourceName, capResources), null);           
            this.AddParameter(new GetCrewCount(crewCount), null);
            
            base.SetExpiry(3f,10f);
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);          
            Debug.Log(contractsInExistance + ContractState);
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
            return "Both the Mass and ElectricCharge Objectives are meant for the Final Satellite stage, without Lifter.\n\n";
        }
        
        protected override string GetHashString()
        {
            return targetBody.bodyName + GMaxApA.ToString() + GMinApA.ToString() + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Launch Company Satellite Into Orbit around " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "We would like you to deliver our Satellite to orbit, its important to the future of our company\n\n"+"Contract Goals\n\n " + "1. Build a satellite (suggest placing docking port for future Repair contracts) \n 2. launch satellite to Contract Orbit Specified";
        }
        protected override string GetSynopsys()
        {
            return "Bring satellite to orbit " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our satellite to orbit " + targetBody.theName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double maxApa = double.Parse(node.GetValue("maxaPa"));
            GMaxApA = maxApa;
            double minApa = double.Parse(node.GetValue("minaPa"));
            GMinApA = minApa;

            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            GMaxPeA = masxPpaID;
            double minPeAID = double.Parse(node.GetValue("minpEa"));
            GMinPeA = minPeAID;

            double maxincID = double.Parse(node.GetValue("maxincID"));
            MaxInc = maxincID;
            double minincID = double.Parse(node.GetValue("minincID"));
            MinInc = minincID; 

            maxTon = float.Parse(node.GetValue("maxtons"));

            capResources = double.Parse(node.GetValue("minresources"));
            ResourceName = node.GetValue("resourcename");
           
            int pcount = int.Parse(node.GetValue("pCount"));
            partAmount = pcount;
            partName = (node.GetValue("pName"));
            crewCount = int.Parse(node.GetValue("crewcount"));
            
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            double maxApa = GMaxApA;
            node.AddValue("maxaPa", GMaxApA);
            double minApa = GMinApA;
            node.AddValue("minaPa", GMinApA);

            double maxPpAID = GMaxPeA;
            node.AddValue("maxpEa", GMaxPeA);
            double MinPeAID = GMinPeA;
            node.AddValue("minpEa", GMinPeA);

            double maxincID = MaxInc;
            node.AddValue("maxincID", MaxInc);
            double minincID = MinInc;
            node.AddValue("minincID", MinInc);

            node.AddValue("maxtons", maxTon);

            node.AddValue("minresources", capResources);
            node.AddValue("resourcename", ResourceName);

            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);

            node.AddValue("crewcount", crewCount);
        }
        
        //for testing purposes
        public override bool MeetRequirements()
        {           
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
            bool techBlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            if (techBlock)
            { return false; }
            if (techUnlock && techUnlock2)
                return true;
            else
                return false;
        }

        
    }
    #endregion
    #region Deliver Satellite to Orbital Period
    public class DeliverSatOrbitalPeriod : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double MinOrb = 21480;
        public double MaxOrb = 21660;
        public double MinInc = 0;
        public double MaxInc = UnityEngine.Random.Range(10, 90);
        public int test = UnityEngine.Random.Range(0, 100);
        public bool testThis = false;
        public int crewCount = 0;
        public float maxTon = 0;
        public double capResources;
        public string ResourceName = "ElectricCharge";
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public int totalContracts;
        public int TotalFinished;
        ContractParameter satellite1;
        ContractParameter satellite2;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.NoOrbitalPeriodcontracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<DeliverSatOrbitalPeriod>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<DeliverSatOrbitalPeriod>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            //Debug.Log("Orbital Period Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.Log("contract is generated right now terminating Orbital Period Satellite Mission");
                //Debug.Log("count is " + totalContracts);
                return false;                 
            }

            targetBody = Planetarium.fetch.Home;
            MinInc = MaxInc - 10;
            MinOrb = st.contract_Orbital_Period_Min_InSeconds;
            MaxOrb = st.contract_Orbital_Period_Max_InSeconds;
            bool ifInclination = false;

            if (test >= 75)
                ifInclination = true;
            else
                ifInclination = false;

            if (this.prestige == ContractPrestige.Trivial)
            {
                maxTon = UnityEngine.Random.Range(st.contrac_Satellite_Min_Mass_Trivial, st.contrac_Satellite_Max_Mass_Trivial);
                capResources = UnityEngine.Random.Range(2, 5) * 100;
                base.SetFunds(39000f * st.Contract_Payment_Multiplier, 68000f * st.Contract_Payment_Multiplier, 39000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(15f, 35f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Significant)
            {
                maxTon = UnityEngine.Random.Range(st.contrac_Satellite_Min_Mass_Significant, st.contrac_Satellite_Max_Mass_Significant);
                capResources = UnityEngine.Random.Range(3, 6) * 100;
                base.SetFunds(49000f * st.Contract_Payment_Multiplier, 89000f * st.Contract_Payment_Multiplier, 49000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(25f, 45f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Exceptional)
            {
                maxTon = UnityEngine.Random.Range(st.contrac_Satellite_Min_Mass_Except, st.contrac_Satellite_Max_Mass_Significant);
                capResources = UnityEngine.Random.Range(4, 9) * 100;
                base.SetFunds(59000f * st.Contract_Payment_Multiplier, 108000f * st.Contract_Payment_Multiplier, 59000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(35f, 345f, targetBody);
            }

            this.AddParameter(new PreLaunch(), null);
            this.satellite1 = this.AddParameter(new OrbitalPeriod(MinOrb, MaxOrb), null);
            satellite1.SetFunds(8000, targetBody);
            satellite1.SetReputation(10, targetBody);
            if (ifInclination)
            {
               this.satellite2 = this.AddParameter(new Inclination(MinInc, MaxInc));
               satellite2.SetFunds(10000, targetBody);
               satellite2.SetReputation(10, targetBody);
            }
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }
            this.AddParameter(new TotalMasGoal(targetBody, maxTon), null);
            this.AddParameter(new ResourceGoalCap(ResourceName, capResources), null);
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetExpiry(3f, 10f);
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);            
            
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
            return "Both the Mass and ElectricCharge Objectives are meant for the Final Satellite stage, without Lifter.\n\n";
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + MaxOrb + MinOrb + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Deliver a Satellite To an Orbital Period of Six Hours " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Our current contract and satellite needs require a very stable Orbit that is to target a certain population center on kerbin at all times, This satellite will point at this population center " +
                " and bring that area 24 hour coverage.\n\n" + "Contract Goals\n\n " +
                "1. Build a satellite (suggest placing docking port for future Repair contracts) \n 2. launch satellite to Contract Orbit Specified";
        }
        protected override string GetSynopsys()
        {
            return "Bring satellite to orbital Period of 6 Hours " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our satellite to orbit " + targetBody.theName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double ApaID = double.Parse(node.GetValue("aPa"));
            MaxOrb = ApaID;
            double PeAID = double.Parse(node.GetValue("pEa"));
            MinOrb = PeAID;
            double maxincID = double.Parse(node.GetValue("maxincID"));
            MaxInc = maxincID;
            double minincID = double.Parse(node.GetValue("minincID"));
            MinInc = minincID;
            crewCount = int.Parse(node.GetValue("crewcount"));
            partAmount = int.Parse(node.GetValue("partamount"));
            partName = node.GetValue("partname");

            maxTon = float.Parse(node.GetValue("maxtons"));

            capResources = double.Parse(node.GetValue("minresources"));
            ResourceName = node.GetValue("resourcename");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = MaxOrb;
            node.AddValue("aPa", ApAID);
            double PeAID = MinOrb;
            node.AddValue("pEa", PeAID);
            double maxincID = MaxInc;
            node.AddValue("maxincID", MaxInc);
            double minincID = MinInc;
            node.AddValue("minincID", MinInc);
            node.AddValue("crewcount",crewCount);
            node.AddValue("partamount", partAmount);
            node.AddValue("partname", partName);

            node.AddValue("maxtons", maxTon);

            node.AddValue("minresources", capResources);
            node.AddValue("resourcename", ResourceName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {            
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("FlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
            bool techBlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            if (techBlock)
            { return false; }
            if (techUnlock && techUnlock2)
                return true;
            else
                return false;
        }
    }
#endregion
    #region Contract Advanced Satellite
    public class AdvSatellite : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = Planetarium.fetch.Home;
        public double GMaxecc = 0;
        public double GMinecc = 0;
        public double MaxAltitude = 0;               
        public int crewCount = 0;
        public List<TechList> techlist = new List<TechList>();
        public bool techUnlocked = false;
        public double MinInc = 0;
        public int partAmount = 1;
        public string partName = "Repair Panel";        
        public string sciPartname = "Communotron 16";
        public int scipartamount = 1;
        public int scipartcount;
        public int scipartFinalcount;
        public double timeOnStation;
        public string TOSName = "We need this amount of time to conduct our studies\n ";
        public bool RandomCheck = false;
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

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.NoSatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AdvSatellite>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<AdvSatellite>().Count();

            if (totalContracts >= 1)
            {              
                return false;
            }           
            techlist.Clear();
            loadscienceparts();
            scipartcount = techlist.Count();
            scipartFinalcount = Tools.RandomNumber(0, scipartcount);
            sciPartname = techlist[scipartFinalcount].techName;

            RandomCheck = Tools.RandomBool(40);

            GMinecc = UnityEngine.Random.Range(0f, .9f);
            GMaxecc = GMinecc + .09f;

            MaxAltitude = Tools.RandomNumber(70000, 400000);

            timeOnStation = Tools.RandomNumber(102400, 900000);

            minorbital = Tools.RandomNumber((int)st.contract_Random_Orbital_Period_MinInSeconds, (int)st.contract_Random_Orbital_Period_MaxInSeconds);
            maxorbital = minorbital + st.contract_Random_Orbital_Period_Difference;
            
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            int bodyrandom = Tools.RandomNumber(1, 3);
            int bodyrandomchance = Tools.RandomNumber(0, 100);
            if (bodyrandomchance > 70 && RandomCheck)
            {
                targetBody = FlightGlobals.Bodies[bodyrandom];
            }
            else
            {
                targetBody = Planetarium.fetch.Home;
            }                       
            this.AddParameter(new PreLaunch(), null);
            if (RandomCheck)
            {
                this.satellite1 = this.AddParameter(new AltitudeGoal(targetBody, MaxAltitude), null);
                satellite1.SetFunds(5000, 5000, targetBody);
                this.satellite2 = this.AddParameter(new EccentricGoal(GMinecc, GMaxecc), null);
                satellite2.SetFunds(8000, 8000, targetBody);
                satellite2.DisableOnStateChange = false;
            }
            else
            {
                this.satellite5 = this.AddParameter(new OrbitalPeriod(minorbital, maxorbital), null);
                if (maxorbital > 300000)
                {
                    satellite5.SetFunds(37000, 37000, targetBody);
                }
                else
                {
                    satellite5.SetFunds(18000, 18000, targetBody);
                }
            }
            this.satellite4 = this.AddParameter(new TimeCountdownOrbits(targetBody, timeOnStation,TOSName), null);
            satellite4.SetFunds(20000,20000, targetBody);          
            this.satellite3 = this.AddParameter(new PartGoal(sciPartname, scipartamount), null);
            satellite3.SetFunds(2000, 2000, targetBody);
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(10000,145000,145000,targetBody);
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
            return "Send our satellite to orbit with specific parts and for amount of time we require";
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + " For specified Orbit " + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Bring advanced Satellite to orbit around " + targetBody.theName + " for amount time specified.";
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
            return "Bring Advanced satellite to orbit " + targetBody.theName;
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
            RandomCheck = bool.Parse(node.GetValue("randomcheck"));

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
            node.AddValue("randomcheck", RandomCheck);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {            
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
            if (techUnlock && techUnlock2)
                return true;
            else
                return false;
        }


    }
    #endregion
    #region Contract Advanced Satellite Type 2
    public class AdvSatellite2 : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double GMaxecc = 0;
        public double GMinecc = 0;
        public double MaxAltitude = 0;
        public double MaxInc = UnityEngine.Random.Range(10, 90);
        public double MinInc;
        public int crewCount = 0;
        public List<TechList> techlist = new List<TechList>();
        public bool techUnlocked = false;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public string sciPartname = "Communotron 16";
        public int scipartamount = 1;
        public int scipartcount;
        public int scipartFinalcount;
        public string sciPartname2 = "Communotron 16";
        public int scipartamount2 = 1;
        public int scipartFinalcount2;
        public double timeOnStation;
        public string TOSName = "We need this amount of time to conduct our studies\n ";
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

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.NoSatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AdvSatellite2>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<AdvSatellite2>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            techlist.Clear();
            loadscienceparts();
            scipartcount = techlist.Count();
            scipartFinalcount = UnityEngine.Random.Range(0, scipartcount);
            sciPartname = techlist[scipartFinalcount].techName;
           
            scipartFinalcount2 = UnityEngine.Random.Range(0, scipartcount);
            if (scipartFinalcount2 == scipartFinalcount)
            {
                scipartFinalcount2 ++;
                if (scipartFinalcount2 > scipartcount)
                {
                    scipartFinalcount2 = scipartFinalcount2 - 2;
                }
            }
            sciPartname2 = techlist[scipartFinalcount2].techName;

            GMinecc = UnityEngine.Random.Range(0f, .2f);
            GMaxecc = GMinecc + .03f;

            MaxAltitude = UnityEngine.Random.Range(150000, 2000000);

            timeOnStation = UnityEngine.Random.Range(102400, 900000);

            MinInc = MaxInc - 5;

            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            int bodyrandom = UnityEngine.Random.Range(1, 3);
            int bodyrandomchance = UnityEngine.Random.Range(0, 100);
            if (bodyrandomchance > 60)
            {
                targetBody = FlightGlobals.Bodies[bodyrandom];
            }
            else
            {
                targetBody = Planetarium.fetch.Home;
            }
            this.AddParameter(new PreLaunch(), null);
            this.satellite1 = this.AddParameter(new AltitudeGoal(targetBody, MaxAltitude), null);
            satellite1.SetFunds(5000, 5000, targetBody);
            this.satellite2 = this.AddParameter(new EccentricGoal(GMinecc, GMaxecc), null);
            satellite2.SetFunds(8000, 8000, targetBody);
            satellite2.DisableOnStateChange = false;
            this.satellite6 = this.AddParameter(new Inclination(MinInc, MaxInc), null);
            satellite6.SetFunds(5000, 5000, targetBody);
            this.satellite4 = this.AddParameter(new TimeCountdownOrbits(targetBody, timeOnStation, TOSName), null);
            satellite4.SetFunds(20000, 20000, targetBody);
            this.satellite3 = this.AddParameter(new PartGoal(sciPartname, scipartamount), null);
            satellite3.SetFunds(2000, 2000, targetBody);
            this.satellite5 = this.AddParameter(new PartGoal(sciPartname2, scipartamount2), null);
            satellite5.SetFunds(2000, 2000, targetBody);
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(12000, 175000, 175000, targetBody);
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
            return "Send our satellite to orbit with specific parts and for amount of time we require";
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + GMaxecc.ToString() + GMinecc.ToString() + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Bring advanced Satellite to orbit around " + targetBody.theName + " With Specified Inclination Change.";
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "We would like you to deliver our Satellite to orbit, We have specific scientific parts we want added to this satellite. Please include a " + sciPartname + "and " + sciPartname2 +
                "\n\n" + "Contract Goals\n\n " + "1. Build a satellite (suggest placing docking port for future Repair contracts)\n 2. Include a " + sciPartname + " In the construction." +
                "\n 3. launch satellite to Contract Orbit Specified With the Inclination change specified!";
        }
        protected override string GetSynopsys()
        {
            return "Bring Advanced satellite to orbit " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our satellite to orbit around " + targetBody.theName + " with the specialized part " + sciPartname + "and " + sciPartname2 +
                ".  We gained many scientific achievments with this mission and would like to thank you for your help, the inclination change in this contract helped us come to a greater understanding of how kerbin works.";
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

            MaxInc = double.Parse(node.GetValue("maxinc"));
            MinInc = double.Parse(node.GetValue("mininc"));

            sciPartname = node.GetValue("sciname");
            scipartamount = int.Parse(node.GetValue("sciamount"));
            
            timeOnStation = double.Parse(node.GetValue("timestation"));
            TOSName = node.GetValue("tosname");

            sciPartname2 = node.GetValue("sciname2");
            scipartamount2 = int.Parse(node.GetValue("sciamount2"));
                        
            GMaxecc = double.Parse(node.GetValue("maxecc"));
            GMinecc = double.Parse(node.GetValue("minecc"));
            MaxAltitude = double.Parse(node.GetValue("altitude"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("maxecc", GMaxecc);
            node.AddValue("minecc", GMinecc);
            node.AddValue("altitude", MaxAltitude);

            node.AddValue("maxinc",MaxInc);
            node.AddValue("mininc", MinInc);

            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("crewcount", crewCount);

            node.AddValue("sciname", sciPartname);
            node.AddValue("sciamount", scipartamount);

            node.AddValue("sciname2", sciPartname2);
            node.AddValue("sciamount2", scipartamount2);

            node.AddValue("timestation", timeOnStation);
            node.AddValue("tosname", TOSName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
            if (techUnlock && techUnlock2)
                return true;
            else
                return false;
        }


    }
    #endregion
    #region Contract Advanced Satellite Type 3
    public class AdvSatellite3 : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double GMaxOrbital;
        public double GMinOrbital;
        public int crewCount = 0;
        public List<TechList> techlist = new List<TechList>();
        public bool techUnlocked = false;
        public double MinInc = 0;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public string sciPartname = "Communotron 16";
        public int scipartamount = 1;
        public int scipartcount;
        public int scipartFinalcount;
        public string sciPartname2 = "Communotron 16";
        public int scipartamount2 = 1;
        public int scipartFinalcount2;        
        public double timeOnStation;
        public string TOSName = "We need this amount of time to conduct our studies\n ";
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
        public void loadCommandPart()
        {
            foreach (AvailablePart ap in PartLoader.LoadedPartsList)
            {
                if (ap.category == PartCategories.Pods)
                {
                    if (ResearchAndDevelopment.GetTechnologyState(ap.TechRequired) == RDTech.State.Available)
                    {
                        Part p = new Part();
                        if (p.name == ap.name)
                        {
                            if (p.CrewCapacity == 0)
                            {
                                techlist.Add(new TechList(ap.title));
                            }
                        }                       
                    }
                }
            }
        }

        ContractParameter satellite2;
        ContractParameter satellite3;
        ContractParameter satellite4;
        ContractParameter satellite5;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.NoSatelliteContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AdvSatellite3>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<AdvSatellite3>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            techlist.Clear();
            loadscienceparts();
            scipartcount = techlist.Count();
            scipartFinalcount = UnityEngine.Random.Range(0, scipartcount);
            sciPartname = techlist[scipartFinalcount].techName;

            scipartFinalcount2 = UnityEngine.Random.Range(0, scipartcount);
            if (scipartFinalcount2 == scipartFinalcount)
            {
                scipartFinalcount2++;
                if (scipartFinalcount2 > scipartcount)
                {
                    scipartFinalcount2 = scipartFinalcount2 - 2;
                }
            }
            sciPartname2 = techlist[scipartFinalcount2].techName;
                     
            timeOnStation = UnityEngine.Random.Range(102400, 900000);

            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
                      
            targetBody = Planetarium.fetch.Home;

            GMaxOrbital = st.contract_Orbital_Period_Max_InSeconds;
            GMinOrbital = st.contract_Orbital_Period_Min_InSeconds;
            
            this.AddParameter(new PreLaunch(), null);           
            this.satellite2 = this.AddParameter(new OrbitalPeriod(GMinOrbital, GMaxOrbital), null);
            satellite2.SetFunds(8000, 8000, targetBody);
            satellite2.DisableOnStateChange = false;
            this.satellite4 = this.AddParameter(new TimeCountdownOrbits(targetBody, timeOnStation, TOSName), null);
            satellite4.SetFunds(20000, 20000, targetBody);
            this.satellite3 = this.AddParameter(new PartGoal(sciPartname, scipartamount), null);
            satellite3.SetFunds(2000, 2000, targetBody);
            this.satellite5 = this.AddParameter(new PartGoal(sciPartname2, scipartamount2), null);
            satellite5.SetFunds(2000, 2000, targetBody);           
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(1400, 200000, 200000, targetBody);
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
            return "Send our satellite to an Orbital Period of 6 Hours with specific parts and for amount of time we require";
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + GMaxOrbital.ToString() + GMinOrbital.ToString() + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Bring advanced Satellite to orbit around " + targetBody.theName + " Into a KeoSync Orbit.";
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "We would like you to deliver our Satellite to orbit, We have specific scientific parts we want added to this satellite. Please include a " + sciPartname + "and " + sciPartname2 + 
                "\n\n" + "Contract Goals\n\n " + "1. Build a satellite (suggest placing docking port for future Repair contracts)\n 2. Include a " + sciPartname + "and " + sciPartname2 + "and " + " In the construction." +
                "\n 3. launch satellite to Contract Orbital Peroid of 6 hours";
        }
        protected override string GetSynopsys()
        {
            return "Bring Advanced satellite to orbit " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have successfully delivered our satellite to orbit around " + targetBody.theName + " with the specialized part " + sciPartname + "and " + sciPartname2 + "and " +
                ".  We gained many scientific achievments with this mission and would like to thank you for your help.  We were able to study the specific side of Kerbin that we needed to, thanks for bringing" +
                "The satellite to a KeoSync orbit for us.";
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

            sciPartname2 = node.GetValue("sciname2");
            scipartamount2 = int.Parse(node.GetValue("sciamount2"));
                     
            GMaxOrbital = double.Parse(node.GetValue("maxecc"));
            GMinOrbital = double.Parse(node.GetValue("minecc"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("maxecc", GMaxOrbital);
            node.AddValue("minecc", GMinOrbital);

            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("crewcount", crewCount);

            node.AddValue("sciname", sciPartname);
            node.AddValue("sciamount", scipartamount);

            node.AddValue("sciname2", sciPartname2);
            node.AddValue("sciamount2", scipartamount2);
           
            node.AddValue("timestation", timeOnStation);
            node.AddValue("tosname", TOSName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("scienceTech") == RDTech.State.Available;
            if (techUnlock && techUnlock2)
                return true;
            else
                return false;
        }


    }
    #endregion
    #region Repair Goal Contract
    public class RepairGoal : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        int bodyIDX = 1;
        string vesselID;
        string vesselName;
        bool NoVessel = false;
        string titleName = "Repair Vessel ";
        double maxApA;
        string repairParts = "SpareParts";
        double RPamount = 1;
        ContractParameter repairgoal2;
        string Ctitle = "To Repair Vessel You must have at Least ";
        public List<RepairVesselsList> repairvesselList = new List<RepairVesselsList>();

        public void findVeselWithRepairPart()
        {           
            foreach (Vessel vs in FlightGlobals.Vessels)
            {
                foreach (ProtoPartSnapshot p in vs.protoVessel.protoPartSnapshots)
                {
                    foreach (ProtoPartModuleSnapshot m in p.modules)
                    {
                        if (m.moduleName.Equals("RepairPanel"))
                        {                           
                           repairvesselList.Add(new RepairVesselsList(vs.vesselName,vs.id.ToString(),vs.orbit.ApA,vs.mainBody.flightGlobalsIndex));
                        }
                    }
                }
            }
           
        }
        public void changeNameRepairVes()
        {
            string originalName;
            foreach (Vessel vs in FlightGlobals.Vessels)
            {
                if (vs.id.ToString() == vesselID)
                {
                    originalName = vs.vesselName;
                    vs.vesselName = vs.vesselName.Replace(originalName,originalName + "(Repair)");
                    Debug.Log("vessel original name is " + originalName + "new name " + vs.vesselName);
                }
            }
        }

        public void NameBackOriginal()
        {
            foreach (Vessel vs in FlightGlobals.Vessels)
            {
                if (vs.id.ToString() == vesselID)
                {
                    vs.vesselName = vs.vesselName.Replace("(Repair)", "");
                }                
                else
                    Debug.Log("could not find vessel name to change back.  Have to manually change it back.  Possible when docked name can't be changed since vessel doesn't exist while docked");
            }
        }

        public void chooseVesselRepairFromList()
        {            
            System.Random rnd = new System.Random();
            if (repairvesselList.Count > 0)
            {
                RepairVesselsList random = repairvesselList[rnd.Next(repairvesselList.Count)];
                vesselID = random.vesselId.ToString();
                vesselName = random.vesselName.Replace("(unloaded)", "");
                maxApA = random.MaxApA;
                bodyIDX = random.bodyidx;
                //Debug.LogWarning("(repair)bodyIDX is: " + bodyIDX + "   " + random.bodyidx);
                //Debug.LogWarning("Random Repair Vessel Selected " + random.vesselName + "  " + random.vesselId + "  " + random.MaxApA);
                NoVessel = true;
            }
            //else { Debug.LogError(" Vessel Selection Null, skiped process"); NoVessel = false; }
        }

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {            
            if (HighLogic.LoadedSceneIsFlight)
            {
                return false;
            }           
            totalContracts = ContractSystem.Instance.GetCurrentContracts<RepairGoal>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<RepairGoal>().Count();
            //Debug.Log(" Repair Contract Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);           
            if (totalContracts >= 1 || SaveInfo.NoRepairContracts)
            {               
               //Debug.Log("contract is generated right now terminating Repair Vessel");
                                            
                return false;  
            }
            if (!SaveInfo.RepairContractOn)
            {
                Debug.LogWarning("(Repair) contract random is false, contract not generated");
                return false;               
            }
            findVeselWithRepairPart();
            chooseVesselRepairFromList();
            if (!NoVessel)
            {
                return false;
            }
            targetBody = FlightGlobals.Bodies[bodyIDX];
            if (targetBody = null)
            {
                targetBody = Planetarium.fetch.Home;
            }
            
            if (maxApA <= 120000)
            {
                base.SetFunds(45000f * st.Contract_Payment_Multiplier, 71000f * st.Contract_Payment_Multiplier, 51000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
                base.SetScience(3.0f, targetBody);
            }
            if (maxApA > 120000 && maxApA <= 1000000)
            {
                base.SetFunds(70000f * st.Contract_Payment_Multiplier, 95000f * st.Contract_Payment_Multiplier, 70000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
                base.SetScience(10.0f, targetBody);
            }
            if (maxApA > 1000001)
            {
                base.SetFunds(120000f * st.Contract_Payment_Multiplier, 130000f * st.Contract_Payment_Multiplier, 120000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
                base.SetScience(20.0f, targetBody);
            }
           
            this.repairgoal2 = this.AddParameter(new RepairPanelPartCheck(titleName,vesselID,vesselName), null);
            repairgoal2.SetFunds(8000, targetBody);
            repairgoal2.SetReputation(10, targetBody);
            this.AddParameter(new ResourceSupplyGoal(repairParts, RPamount,Ctitle), null);
            base.SetExpiry(1f, 3f);            
            base.SetDeadlineYears(1f, targetBody);                        

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

        protected override void OnAccepted()
        {
            changeNameRepairVes();
        }

        protected override void OnCancelled()
        {
            NameBackOriginal();
        }
        protected override void OnCompleted()
        {
            NameBackOriginal();
        }

        protected override string GetHashString()
        {
            return "Repair Vessel " + vesselName + " by docking with it, then repairing it";
        }
        protected override string GetTitle()
        {
            return "Repair Contract for Satellite: " + " " + vesselName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Hello, we would like you to launch a Manned Vessel to our satellite.  DOCK WITH IT, transfer some repair Parts to the RepairPanel.  After this you will have to go EVA and open the door and conduct Repairs.\n\n"+
                "Don't forget to bring at least 1 (Resource) repair Parts with you to conduct the repairs with.  Contact payout are adjusted for travel time, and what Celestrial Body you are going to. \n\n"+
                "If no docking port is on the Satellite you can use the claw to attach to the satellite, or any number of Mods that add attachment type systems.  As long as they count as DOCKED you will be good.";
        }
        protected override string GetSynopsys()
        {
            return "Repair Vessel: " + vesselName;
        }
        protected override string MessageCompleted()
        {
            NoVessel = false;
            RepairPanel.repair = false;
            return "You have successfully repair our Satellite " + vesselName + " thank you very much!";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            try
            {
                targetBody = FlightGlobals.Bodies[bodyID];
                //Debug.LogWarning("(repair)Target Body is: " + targetBody.theName + " BodyID was: " + bodyID);
            }
            catch
            {
                targetBody = Planetarium.fetch.Home;
                //Debug.LogWarning("(repair)On load defaulted to homebody");
            }
            
            vesselID = node.GetValue("VesselID");
            vesselName = node.GetValue("VesselName");
            titleName = node.GetValue("titlename");
            maxApA = double.Parse(node.GetValue("maxapa"));
            repairParts = node.GetValue("repairparts");
            RPamount = double.Parse(node.GetValue("rpamount"));
            Ctitle = node.GetValue("ctitle");
            bodyIDX = int.Parse(node.GetValue("bodyidx"));

        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("targetBody", bodyIDX);
            //Debug.LogWarning("(repair)OnSave targetBody saved as: " + bodyIDX);

            node.AddValue("VesselID", vesselID);
            node.AddValue("VesselName", vesselName);
            node.AddValue("titlename", titleName);
            node.AddValue("maxapa", maxApA);
            node.AddValue("repairparts", repairParts);
            node.AddValue("rpamount", RPamount);
            node.AddValue("ctitle",Ctitle);
            node.AddValue("bodyidx", bodyIDX);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }
#endregion
    #region OrbitalScan Contract
    public class OrbitalScanContract : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        int crewCount = 0;
        public double testpos = 0;
        string partName = "Orbital Research Scanner";
        int partNumber = 1;
        double missionTime = 0;
        public int totalContracts = 0;
        public int TotalFinished = 0;
        ContractParameter orbitresearch1;
        ContractParameter orbitresearch2;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            targetBody = GetUnreachedTargets();
            if (targetBody != null)
            {
                //Debug.LogWarning(" Bodies Not Visited is: " + targetBody.theName);
            }
            else
            {
                targetBody = Planetarium.fetch.Home;
                //Debug.LogWarning("Target Body was Null set to kerbin");
            }
            if (SaveInfo.NoOrbitalResearchContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<OrbitalScanContract>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<OrbitalScanContract>().Count();
            //Debug.Log("Orbital Research Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.Log("Orbital Research contract is generated right now terminating Mission");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            missionTime = Tools.RandomNumber(200, 1500);
            this.AddParameter(new PreLaunch(), null);
            this.orbitresearch1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            orbitresearch1.SetFunds(8000, targetBody);
            orbitresearch1.SetReputation(3, targetBody);
            orbitresearch1.SetScience(4, targetBody);
            this.orbitresearch2 = this.AddParameter(new OrbialResearchPartCheck(targetBody, missionTime), null);
            orbitresearch2.SetFunds(9000, targetBody);
            orbitresearch2.SetReputation(5, targetBody);
            orbitresearch2.SetScience(8, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(30f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(5f, 3f, targetBody);
            base.SetFunds(34000f * st.Contract_Payment_Multiplier, 53000f * st.Contract_Payment_Multiplier, 34000f * st.Contract_Payment_Multiplier, targetBody);

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
            return targetBody.bodyName + " " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Scientific orbital research mission of  " + targetBody.theName + " with " + partName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", MissionSeed);
        }
        protected override string GetSynopsys()
        {
            return "Scientific orbital research mission of  " + targetBody.theName + " with " + partName;
        }
        protected override string MessageCompleted()
        {
            MCEOrbitalScanning.doOrbitResearch = false;
            return "You have reached the target body " + targetBody.theName + ", and conducted orbital research.  We have learned a lot of new information about the composition " +
                "of this planetary body in preparation for a possible landing in the future by our manned program or Robotic Legions.";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            crewCount = int.Parse(node.GetValue("crewcount"));
            partName = (node.GetValue("partname"));
            partNumber = int.Parse(node.GetValue("maxcount"));
            missionTime = double.Parse(node.GetValue("missiontime"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewcount", crewCount);
            node.AddValue("partname", partName);
            node.AddValue("maxcount", partNumber);
            node.AddValue("missiontime", missionTime);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {           
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }

        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(true, true);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
            }
            return null;
        }
    }
    #endregion
    #region Lander Scanning Contract
    public class LanderResearchScan : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        int crewCount = 0;
        public double testpos = 0;
        string partName = "Ground Based Research Scanner";
        int partNumber = 1;
        double amountTime = Tools.RandomNumber(200, 1500);
        public int totalContracts;
        public int TotalFinished;
        ContractParameter landerscan1;
        ContractParameter landerscan2;
        ContractParameter landerscan3;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            targetBody = GetUnreachedTargets();

            if (targetBody != null)
            {
                //Debug.LogWarning(" Bodies Not Visited is: " + targetBody.theName);
            }
            else
            {
                //Debug.LogWarning("Target Body was Null For Landing Contract Contract Cancelled");
                return false;
            }
            if (SaveInfo.NoLanderResearchContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<LanderResearchScan>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<LanderResearchScan>().Count();
            //Debug.Log("Land Research Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.LogWarning("Land Research contract is generated right now terminating Mission");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            if (targetBody.flightGlobalsIndex == 8)
            {
                Debug.LogWarning("Landing Goal Body set to: " + targetBody.theName + " Contract Generate cancelled");
                return false;
            }
            this.AddParameter(new PreLaunch(), null);
            this.landerscan1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            landerscan1.SetFunds(8000, targetBody);
            landerscan1.SetReputation(4, targetBody);
            landerscan1.SetScience(2, targetBody);
            this.landerscan2 = this.AddParameter(new LandOnBody(targetBody), null);
            landerscan2.SetFunds(8500, targetBody);
            landerscan2.SetReputation(5, targetBody);
            landerscan2.SetScience(4, targetBody);
            this.landerscan3 = this.AddParameter(new LanderResearchPartCheck(targetBody, amountTime), null);
            landerscan3.SetFunds(10000, targetBody);
            landerscan3.SetReputation(8, targetBody);
            landerscan3.SetScience(7, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(55f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(35f, 11f, targetBody);
            base.SetFunds(37000f * st.Contract_Payment_Multiplier, 66000f * st.Contract_Payment_Multiplier, 77000f * st.Contract_Payment_Multiplier, targetBody);

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
            return targetBody.bodyName + " " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return "Scientific and research landing contract of  " + targetBody.theName + " with " + partName;;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", MissionSeed);
        }
        protected override string GetSynopsys()
        {
            return "Land a Unmanned Vessel on " + targetBody.theName + " and conduct research for our company";
        }
        protected override string MessageCompleted()
        {
            MCELanderResearch.doLanderResearch = false;
            return "You have successfully landed and researched on " + targetBody.theName + " After landing we have found out many fascinating secrets about what makes up the composition of the landing site.\n\n" +

            "Further research missions manned and robotic will be needed in the future to unlock the secrets of " + targetBody.theName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            crewCount = int.Parse(node.GetValue("crewcount"));
            partName = (node.GetValue("partname"));
            partNumber = int.Parse(node.GetValue("maxcount"));

            amountTime = double.Parse(node.GetValue("amountTime"));
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewcount", crewCount);
            node.AddValue("partname", partName);
            node.AddValue("maxcount", partNumber);

            node.AddValue("amountTime", amountTime);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {           
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advElectrics") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(false, false);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
            }
            return null;
        }
    }
    #endregion
    #region Build ComSat Network
    public class BuildComNetwork : Contract
    {
        CelestialBody targetBody = null;
        Settings settings = new Settings("Config.cfg");
        public double MinOrb;
        public double MaxOrb;
        public int crewCount = 0;
        public string ContractPlayerName;
        public int partAmount = 1;
        public string partName = "Repair Panel";
        public bool StartNetwork;
        public int totalContracts;
        public int TotalFinished;


        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<BuildComNetwork>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<BuildComNetwork>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            //Debug.Log("COMSAT Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.Log("Contract Deliver ComSat Network Rejected");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            settings.Load();
            StartNetwork = SaveInfo.ComSateContractOn;
            if (!StartNetwork)
            {
                //Debug.Log("ComSat Network is shut off, and set to false");
                return false;
            }
            targetBody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            ContractPlayerName = SaveInfo.ComSatContractName;
            MinOrb = SaveInfo.comSatminOrbital;
            MaxOrb = SaveInfo.comSatmaxOrbital;

            this.AddParameter(new OrbitalPeriod(MinOrb, MaxOrb), null);
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }
            this.AddParameter(new PreLaunch(), null);
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetExpiry(3f, 15f);
            base.SetScience(5f, targetBody);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(25f, 40f, targetBody);
            base.SetFunds(39000f * settings.Contract_Payment_Multiplier, 52000f * settings.Contract_Payment_Multiplier, 37000f * settings.Contract_Payment_Multiplier, targetBody);

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
            return targetBody.bodyName + MaxOrb + MinOrb + " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " " + targetBody.theName;
        }
        protected override string GetDescription()
        {

            return "This is a ComSat Network Contract, you have control over when these missions are available and what their Orbital Periods are, you can set this up in the MCE Infomation Icon Tab located top Right \n" +
                "Corner Of screen while in SpaceCenter View.  Please note that settings will not take effect until at least 1 cycle of contracts has passed.  If you don't see your settings cancel out the Offered contract! \n\n" +
                "All ComSat Information is stored inside the Mission Controller Config File and will pass on to other save files";
        }
        protected override string GetSynopsys()
        {
            return "Launch Your ComSat Network " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have Delivered your ComSat to its assigned height around " + targetBody.theName + " Continue to build you network.  When your done you can turn off ComSat Contracts in the MCE Information Window.  Please note it will take a few Contract cycles for them to Disapear! ";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double ApaID = double.Parse(node.GetValue("aPa"));
            MaxOrb = ApaID;
            double PeAID = double.Parse(node.GetValue("pEa"));
            MinOrb = PeAID;
            crewCount = int.Parse(node.GetValue("crewcount"));
            partAmount = int.Parse(node.GetValue("partamount"));
            partName = node.GetValue("partname");
            ContractPlayerName = node.GetValue("contractplayername");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = MaxOrb;
            node.AddValue("aPa", ApAID);
            double PeAID = MinOrb;
            node.AddValue("pEa", PeAID);
            node.AddValue("crewcount", crewCount);
            node.AddValue("partamount", partAmount);
            node.AddValue("partname", partName);
            node.AddValue("contractplayername", ContractPlayerName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }
#endregion
    #region Custom Supply Contract
    public class CustomSupply : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;      
        public string vesselName;
        public string vesselId;
        public string ResourceName;
        public string ContractPlayerName;
        public bool StartSupply;
        public double resourcesAmount;
        public int totalContracts;
        public int TotalFinished;
        public string CTitle = "Supply your Station Or Base with ";
        ContractParameter suppy1;
        ContractParameter suppy2;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CustomSupply>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CustomSupply>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            //Debug.Log("COMSAT Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.Log("Contract Deliver ComSat Network Rejected");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            StartSupply = SaveInfo.supplyContractOn;
            if (!StartSupply)
            {
                //Debug.Log("supply contract is shut off, and set to false");
                return false;
            }
            targetBody = FlightGlobals.Bodies[SaveInfo.SupplyBodyIDX];
            if (targetBody == null)
            {
                targetBody = Planetarium.fetch.Home;
                //Debug.Log("Did not find Body for Supply Mission defaulting to kerbin");
            }
            vesselName = SaveInfo.SupplyVesName;
            vesselId = SaveInfo.SupplyVesId;
            ResourceName = SaveInfo.ResourceName;
            resourcesAmount = SaveInfo.supplyAmount;
            
            ContractPlayerName = SaveInfo.SupplyContractName;

            this.AddParameter(new PreLaunch(), null);
            this.suppy1 = this.AddParameter(new TargetDockingGoal(vesselId,vesselName),null);
            suppy1.SetFunds(1000, 2000, targetBody);
            suppy1.SetReputation(5, 10, targetBody);
            this.suppy2 = this.AddParameter(new ResourceSupplyGoal(ResourceName, resourcesAmount, CTitle), null);
            suppy2.SetFunds(1000, 2000, targetBody);
            suppy2.SetReputation(5, 10, targetBody);
            base.SetExpiry(1f, 10f);
            base.SetScience(1f, targetBody);
            base.SetDeadlineYears(.3f, targetBody);
            base.SetReputation(20f, 40f, targetBody);
            base.SetFunds(20000, 90000, 150000, targetBody);

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
            return "Supply your base or station (" + vesselName + ") with supplies. Location is " + targetBody+ " - Total Done: " + TotalFinished;
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " (" + vesselName + ")";
        }
        protected override string GetDescription()
        {

            return "This is a custom supply mission.  Use these contracts to supply your land bases and Orbital stations with whatever supplies you choose. You must dock with the Vessel you selected to finish contract! \n" +
                "You can edit this contract by going to SpaceCenter screen and selecting Mission Controller Icon.  In the GUI choose the Custom Contract Button to start editing this contract. \n\n" +
                "All supply contract information is stored in you Persistent Save File. The location of the Station or Base you will resupply is " + targetBody.theName + " Payments are adjusted for Travel Time To Body";
        }
        protected override string GetSynopsys()
        {
            return "Launch Your ComSat Network " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have Delivered your supplies to " + vesselName + " If your done, you can turn off Supply Contracts in the MCE Information Window.  Please note it will take a few Contract cycles for them to Disapear! ";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                Debug.LogWarning("(supply) loaded targetbody is " + targetBody);
            }
            vesselName = node.GetValue("vesselname");
            vesselId = node.GetValue("vesselid");                     
            ContractPlayerName = node.GetValue("contractplayername");
            ResourceName = node.GetValue("supplies");
            resourcesAmount = double.Parse(node.GetValue("resourceamount"));
            CTitle = node.GetValue("ctitle");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            Debug.LogWarning("(supply) target body saved as " + bodyID);
            node.AddValue("targetBody", bodyID);
            node.AddValue("vesselname", vesselName);
            node.AddValue("vesselid", vesselId);          
            node.AddValue("contractplayername", ContractPlayerName);
            node.AddValue("supplies", ResourceName);
            node.AddValue("resourceamount", resourcesAmount);
            node.AddValue("ctitle", CTitle);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }
    #endregion
    #region Custom Crew Transfer Contract
    public class CustomCrewTransfer : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public string vesselName;
        public string vesselId;
        public bool Startcrewtrans;
        public int crewAmount;
        public string ContractPlayerName;
        public double crewTime;
        public int totalContracts;
        public int TotalFinished;
        public string CTitle = "Supply your Station Or Base with ";
        ContractParameter ctrans1;
        ContractParameter ctrans2;
        ContractParameter ctrans3;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CustomCrewTransfer>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CustomCrewTransfer>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            //Debug.Log("COMSAT Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                //Debug.Log("Contract Deliver ComSat Network Rejected");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            Startcrewtrans = SaveInfo.crewContractOn;
            if (!Startcrewtrans)
            {
                //Debug.Log("supply contract is shut off, and set to false");
                return false;
            }
            targetBody = FlightGlobals.Bodies[SaveInfo.crewBodyIDX];
            if (targetBody == null)
            {
                targetBody = Planetarium.fetch.Home;
                //Debug.Log("Did not find Body for Supply Mission defaulting to kerbin");
            }
            vesselName = SaveInfo.crewVesName;
            vesselId = SaveInfo.crewVesid;
            crewAmount = SaveInfo.crewAmount;
            crewTime = SaveInfo.crewTime;

            ContractPlayerName = SaveInfo.crewTransferName;

            this.AddParameter(new PreLaunch(), null);
            this.ctrans1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            ctrans1.SetFunds(2000, 2000, targetBody);
            ctrans1.SetReputation(3, targetBody);
            this.ctrans2 = this.AddParameter(new TimeCountdownOrbits(targetBody, crewTime), null);
            ctrans2.SetFunds(2000, 2000, targetBody);
            ctrans2.SetReputation(3, targetBody);
            this.ctrans3 = this.AddParameter(new LandOnBody(Planetarium.fetch.Home), null);
            ctrans3.SetFunds(2000, 2000, targetBody);
            ctrans3.SetReputation(3, targetBody);
            this.AddParameter(new GetCrewCount(crewAmount), null);
            base.SetExpiry(15f, 40f);
            base.SetScience(1f, targetBody);
            base.SetDeadlineYears(700, targetBody);
            base.SetReputation(25f, 50f, targetBody);
            base.SetFunds(15000, 100000, 110000, targetBody);

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
            return "Transfer " + crewAmount +" Crew To (" + vesselName + ") for " + Tools.formatTime(crewTime) + " Over " + targetBody.theName;
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " (" + vesselName + ")";
        }
        protected override string GetDescription()
        {

            return "This is a custom Crew Transfer mission.  Use these contracts to supply your land bases and Orbital stations with crews and select the Time Spent in station or base. You must dock with the Vessel you selected to finish contract! " +
                "You can edit this contract by going to SpaceCenter screen and selecting Mission Controller Icon.  In the GUI choose the Custom Contract Button to start editing this contract. \n\n" +
                "All Crew Transfer contract information is stored in you Persistent Save File. The location of the Station or Base you will Transfer crew is " + targetBody.theName +"." + " Payments are adjusted for Travel Time To Body";
        }
        protected override string GetSynopsys()
        {
            return "Transfer crew to Station/Base over " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have Delivered your Crew to " + vesselName + "And spent " + Tools.formatTime(crewTime) + " at your Station/Base\n\n"+
                " If your done, you can turn off Crew Contracts in the MCE Information Window.  Please note it will take a few Contract cycles for them to Disapear! ";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                Debug.LogWarning("(supply) loaded targetbody is " + targetBody);
            }
            vesselName = node.GetValue("vesselname");
            vesselId = node.GetValue("vesselid");
            ContractPlayerName = node.GetValue("contractplayername");
            crewAmount = int.Parse(node.GetValue("crew"));
            crewTime = double.Parse(node.GetValue("time"));
            CTitle = node.GetValue("ctitle");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            Debug.LogWarning("(supply) target body saved as " + bodyID);
            node.AddValue("targetBody", bodyID);
            node.AddValue("vesselname", vesselName);
            node.AddValue("vesselid", vesselId);
            node.AddValue("contractplayername", ContractPlayerName);
            node.AddValue("crew", crewAmount);
            node.AddValue("time", crewTime);
            node.AddValue("ctitle", CTitle);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }
    #endregion
    #region Civilian Low Orbit Contract
    public class CivilianLowOrbit : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double altitudeGoal;
        public double eccmax;
        public double eccmin;
        public int civiliansAmount = 0;
        public string civdestination = " Kerbin Civilian Tour";
        public string crewSeatTitle = "You must have these many open seats for Civilians";
        public string name1 = "Civilian Randall";
        public string name2 = "Civilian Lisa";
        public string name3 = "Civilian Roberts";
        public string name4 = "Civilian Johnsons";
        public double TripTime;
        public string TripText = "The civilians have contracted to spend this amount of time in orbit\n";        
        public int totalContracts;
        public int TotalFinished;
        private int choice1;
        private int choice2;
        private int choice3;
        private int choice4;
        ContractParameter civ1;
        ContractParameter civ2;
        ContractParameter civ3;
        ContractParameter civ4;
        ContractParameter civ5;
        ContractParameter civ6;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CivilianLowOrbit>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CivilianLowOrbit>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }

            if (!SaveInfo.CivilianLowOrbit)
            {
                return false;
            }
            targetBody = Planetarium.fetch.Home;

            civiliansAmount = UnityEngine.Random.Range(2, 4);
            eccmin = UnityEngine.Random.Range(0f, .4f);
            eccmax = eccmin + .10f;
            altitudeGoal = UnityEngine.Random.Range(70000, 225000);
            TripTime = UnityEngine.Random.Range(14000, 150000);

            this.civ1 = this.AddParameter(new PreLaunch(), null);
            civ1.SetFunds(5000, 5000, targetBody);
            civ1.SetReputation(5, 10, targetBody);

            MissionControllerEC.CivName.Clear();
            MissionControllerEC.civNamesListAdd();

            choice1 = UnityEngine.Random.Range(0, 7);
            name1 = MissionControllerEC.CivName[choice1];
            choice2 = UnityEngine.Random.Range(8, 12);
            name2 = MissionControllerEC.CivName[choice2];
            choice3 = UnityEngine.Random.Range(13, 17);
            name3 = MissionControllerEC.CivName[choice3];
            choice4 = UnityEngine.Random.Range(18, 23);
            name4 = MissionControllerEC.CivName[choice4];

            if (civiliansAmount == 2)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, civdestination), null);
                civ2.SetFunds(50000, 5000, targetBody);
                civ2.SetReputation(20, 40, targetBody);
                civ2.DisableOnStateChange = false;
            }
            if (civiliansAmount == 3)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, name3, civdestination), null);
                civ2.SetFunds(75000, 5000, targetBody);
                civ2.SetReputation(30, 60, targetBody);
                civ2.DisableOnStateChange = false;
            }
            if (civiliansAmount == 4)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, name3, name4, civdestination), null);
                civ2.SetFunds(100000, 5000, targetBody);
                civ2.SetReputation(40, 80, targetBody);
                civ2.DisableOnStateChange = false;
            }

            this.civ3 = this.AddParameter(new AltitudeGoal(targetBody, altitudeGoal), null);
            civ3.SetFunds(10000, 10000, targetBody);
            civ3.SetReputation(5, 10, targetBody);
            
            this.civ4 = this.AddParameter(new EccentricGoal(eccmin, eccmax), null);
            civ4.SetFunds(10000, 10000, targetBody);
            civ4.SetReputation(5, 10, targetBody);

            this.civ5 = this.AddParameter(new LandOnBody(targetBody), null);
            civ5.SetFunds(20000, 20000, targetBody);
            civ5.SetReputation(20, 40, targetBody);
                        
            this.civ6 = this.AddParameter(new TimeCountdownOrbits(targetBody, TripTime, TripText), null);
            civ1.SetFunds(50000, 50000, targetBody);
            civ1.SetReputation(15, 30, targetBody);

            this.AddParameter(new GetSeatCount(civiliansAmount, crewSeatTitle), null);
                                       
            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(15000, 150000, 350000, targetBody);
            base.SetReputation(50, 150, targetBody);
            return true;
        }

        protected override void OnAccepted()
        {

            string AgenaMessage = "The civilians that are assigned to your vessel for the Contract Tour are represented in game by seats.  They do not show up as Individual Kerbals in " +
                "the game! Make no mistake though they are on your vessel.  If you fill the seats they need, then you cannot finish the contract.\n" +

                "Even if the objective is Green Check marked,  If you try to cheat and Fill the seat later on the objective will GO BACK to Not Finished!";

            MessageSystem.Message m = new MessageSystem.Message("About the Passengers", AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);
            MessageSystem.Instance.AddMessage(m);
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
            return "Civilian Low Orbit Tour";
        }

        protected override string GetHashString()
        {
            return "Bring Civilians on a Low Kerbib Orbit Tour of Kerbin";
        }
        protected override string GetTitle()
        {
            return "Civilian Contract.  Bring us to Low Kerbin Orbit";
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return civiliansAmount + " Civilian kerbals have signed a contracted with us to bring them to Low Kerbin Orbit for a set amount of time.\n The vessel must have room for the amount of civilians specified in the contract. " +
                "Failure to have the space available will cause the contract to be null and void.\n\n" +
                "It’s also very important that nothing bad happens to our guest while in our care.  If anything tragic happens the financial burdens on the Space Agency could be the end of us!\n\n" +
                "Please take note civilians are not allowed to take part in operations of KSC Personal duties, they are on the vessel as passengers only.  For this reason you as player cannot use them as an in game asset.  \n" +
                "But do not take up their seats or you will lose the contract!"
;
        }
        protected override string GetSynopsys()
        {
            return "Low Kerbin Orbit Tour with Passengers " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "The civilians thank you for bringing them home alive and showing them the wonders of space, and whats its like to be a true Kerbal Space Astronaught!";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            civiliansAmount = int.Parse(node.GetValue("civilians"));
            name1 = node.GetValue("name1");
            name2 = node.GetValue("name2");
            name3 = node.GetValue("name3");
            name4 = node.GetValue("name4");
            altitudeGoal = double.Parse(node.GetValue("altitude"));
            eccmax = double.Parse(node.GetValue("eccmax"));
            eccmin = double.Parse(node.GetValue("eccmin"));
            TripTime = double.Parse(node.GetValue("time"));
            TripText = node.GetValue("triptext");
            civdestination = node.GetValue("civd");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("civilians", civiliansAmount);
            node.AddValue("name1", name1);
            node.AddValue("name2", name2);
            node.AddValue("name3", name3);
            node.AddValue("name4", name4);
            node.AddValue("altitude", altitudeGoal);
            node.AddValue("eccmax", eccmax);
            node.AddValue("eccmin", eccmin);
            node.AddValue("time", TripTime);
            node.AddValue("triptext", TripText);
            node.AddValue("civd", civdestination);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (!techUnlock || !techUnlock2 || st.Civilian_Contracts_Off == true)
                return false;
            else
                return true;
        }
    }
    #endregion
    #region Civilian Land on Body Contract
    public class CivilianLanding : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;     
        public int civiliansAmount = 0;
        public string civdestination = " Kerbin Civilian Landing Expedition";
        public string crewSeatTitle = "You must have these many open seats for Civilians";
        public string name1 = "Civilian Randall";
        public string name2 = "Civilian Lisa";
        public string name3 = "Civilian Roberts";
        public string name4 = "Civilian Johnsons";
        public int civplanetnumber = 2;
        public double TripTime;
        public string TripText = "You must stay landed on this Expedition for  \n";
        public int totalContracts;
        public int TotalFinished;
        private int choice1;
        private int choice2;
        private int choice3;
        private int choice4;
        ContractParameter civ1;
        ContractParameter civ2;
        ContractParameter civ3;
        ContractParameter civ4;
        ContractParameter civ5;
        ContractParameter civ6;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CivilianLanding>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CivilianLanding>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }

            if (!SaveInfo.CivilianLanding)
            {
                return false;
            }
            civplanetnumber = Tools.RandomNumber(0,100);
            if (civplanetnumber < 50)
                targetBody = FlightGlobals.Bodies[2];
            else
                targetBody = FlightGlobals.Bodies[3];

            Debug.LogError("Civilian Landing is set to Body " + targetBody.theName);

            civiliansAmount = UnityEngine.Random.Range(2, 4);

            TripTime = Tools.RandomNumber(78000, 350000);

            this.civ1 = this.AddParameter(new PreLaunch(), null);
            civ1.SetFunds(5000, 5000, targetBody);
            civ1.SetReputation(5, 10, targetBody);

            MissionControllerEC.CivName.Clear();
            MissionControllerEC.civNamesListAdd();

            choice1 = UnityEngine.Random.Range(0, 7);
            name1 = MissionControllerEC.CivName[choice1];
            choice2 = UnityEngine.Random.Range(8, 12);
            name2 = MissionControllerEC.CivName[choice2];
            choice3 = UnityEngine.Random.Range(13, 17);
            name3 = MissionControllerEC.CivName[choice3];
            choice4 = UnityEngine.Random.Range(18, 23);
            name4 = MissionControllerEC.CivName[choice4];

            if (civiliansAmount == 2)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, civdestination), null);
                civ2.SetFunds(5000, 5000, targetBody);
                civ2.SetReputation(20, 40, targetBody);
                civ2.DisableOnStateChange = false;
            }
            if (civiliansAmount == 3)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, name3, civdestination), null);
                civ2.SetFunds(7500, 5000, targetBody);
                civ2.SetReputation(30, 60, targetBody);
                civ2.DisableOnStateChange = false;
            }
            if (civiliansAmount == 4)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, name3, name4, civdestination), null);
                civ2.SetFunds(10000, 5000, targetBody);
                civ2.SetReputation(40, 80, targetBody);
                civ2.DisableOnStateChange = false;
            }

            this.civ3 = this.AddParameter(new InOrbitGoal(targetBody), null);
            civ3.SetFunds(10000, 10000, targetBody);
            civ3.SetReputation(5, 10, targetBody);

            this.civ4 = this.AddParameter(new LandOnBody(targetBody), null);
            civ4.SetFunds(35000, 35000, targetBody);
            civ4.SetReputation(5, 10, targetBody);

            this.civ6 = this.AddParameter(new TimeCountdownLanding(targetBody, TripTime, TripText), null);
            civ1.SetFunds(50000, 50000, targetBody);
            civ1.SetReputation(15, 30, targetBody);

            this.civ5 = this.AddParameter(new LandOnBody(Planetarium.fetch.Home), null);
            civ5.SetFunds(25000, 25000, targetBody);
            civ5.SetReputation(20, 40, targetBody);
         
            this.AddParameter(new GetSeatCount(civiliansAmount, crewSeatTitle), null);

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(25000, 300000, 300000, targetBody);
            base.SetReputation(50, 150, targetBody);
            return true;
        }

        protected override void OnAccepted()
        {

            string civLandingText = "The civilians that are assigned to your vessel for the Contract Tour are represented in game by seats.  They do not show up as Individual Kerbals in " +
                "the game! Make no mistake though they are on your vessel.  If you fill the seats they need, then you cannot finish the contract.\n" +

                "Even if the objective is Green Check marked,  If you try to cheat and Fill the seat later on the objective will GO BACK to Not Finished!";

            MessageSystem.Message m = new MessageSystem.Message("About the Passengers", civLandingText.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);
            MessageSystem.Instance.AddMessage(m);
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
            return "Civilian Landng Expedition";
        }

        protected override string GetHashString()
        {
            return "Bring Civilians on a Landing Expedition of " + targetBody.theName;
        }
        protected override string GetTitle()
        {
            return "Civilian Contract.  Bring Civilians on a landing Expedition of "+ targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return civiliansAmount + " Civilian kerbals have signed a contracted with us to bring them on a landing expedition of the " + targetBody.theName + ".\n\n" + 
                "The vessel must have room for the amount of civilians specified in the contract. " +
                "Failure to have the space available will cause the contract to be null and void.\n\n" +
                "It’s also very important that nothing bad happens to our guest while in our care.  If anything tragic happens the financial burdens on the Space Agency could be the end of us!\n\n" +
                "Please take note civilians are not allowed to take part in operations of KSC Personal duties, they are on the vessel as passengers only.  For this reason you as player cannot use them as an in game asset.  \n" +
                "But do not take up their seats or you will lose the contract!"
;
        }
        protected override string GetSynopsys()
        {
            return "Landing Expedition with Passengers " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "The civilians thank you for bringing them home alive and showing them the wonders of space, and whats its like to be a true Kerbal Space Astronaught! This expedition to " + targetBody.theName +
                "Has brought many discovies for us while we stayed on " + targetBody.theName + ".  We thank you very much for bringing us home safely.";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            civiliansAmount = int.Parse(node.GetValue("civilians"));
            name1 = node.GetValue("name1");
            name2 = node.GetValue("name2");
            name3 = node.GetValue("name3");
            name4 = node.GetValue("name4");           
            TripTime = double.Parse(node.GetValue("time"));
            TripText = node.GetValue("triptext");
            civdestination = node.GetValue("civd");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("civilians", civiliansAmount);
            node.AddValue("name1", name1);
            node.AddValue("name2", name2);
            node.AddValue("name3", name3);
            node.AddValue("name4", name4);           
            node.AddValue("time", TripTime);
            node.AddValue("triptext", TripText);
            node.AddValue("civd", civdestination);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("advLanding") == RDTech.State.Available;
            if (!techUnlock && !techUnlock2 || st.Civilian_Contracts_Off == true)
                return false;
            else
                return true;
        }
    }
    #endregion
    #region Civilian Station Expedition
    public class CivilianStationExpedition : Contract
    {
        MissionControllerEC mc = new MissionControllerEC();
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
               
        public int civiliansAmount = 0;
        public string civdestination = " Civilian Station Expedition";
        public string crewSeatTitle = "You must have these many open seats for Civilians";
        public string name1 = "Civilian Randall";
        public string name2 = "Civilian Lisa";
        public string name3 = "Civilian Roberts";
        public string name4 = "Civilian Johnsons";
        public double TripTime;
        public string TripText = "The civilians have contracted to spend this amount of time in The station\n";
        public int totalContracts;
        public int TotalFinished;
        private int choice1;
        private int choice2;
        private int choice3;
        private int choice4;
        public string vesselID;
        public string vesselName;
        ContractParameter civ1;
        ContractParameter civ2;
        ContractParameter civ3;
        ContractParameter civ5;
        ContractParameter civ6;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CivilianStationExpedition>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CivilianStationExpedition>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }

            if (!SaveInfo.CivilianStationExpedition)
            {
                return false;
            }

            

            mc.getSupplyList(true);
            if (MissionControllerEC.SupVes.Count == 0)
            {
                Debug.LogWarning("No stations in orbit cannot Generate Civilian Expedition Station Contracts");
                return false;              
            }
            Debug.LogWarning("Found stations in orbit Generating Civilian Expedition Station Contracts");
            int randomStation;
            int randomStationCount;
            int randomStationBodyID;
            randomStationCount = MissionControllerEC.SupVes.Count;            
            randomStation = Tools.RandomNumber(0, randomStationCount);

            vesselID = MissionControllerEC.SupVes[randomStation].vesselId.ToString();
            vesselName = MissionControllerEC.SupVes[randomStation].vesselName;
            randomStationBodyID = MissionControllerEC.SupVes[randomStation].body.flightGlobalsIndex;

            targetBody = FlightGlobals.Bodies[randomStationBodyID];

            civiliansAmount = UnityEngine.Random.Range(2, 4);          
            TripTime = UnityEngine.Random.Range(210000, 970000);

            this.civ1 = this.AddParameter(new PreLaunch(), null);
            civ1.SetFunds(5000, 5000, targetBody);
            civ1.SetReputation(5, 10, targetBody);

            this.civ6 = this.AddParameter(new TimeCountdownOrbits(targetBody, TripTime, TripText), null);
            civ1.SetFunds(50000, 50000, targetBody);
            civ1.SetReputation(15, 30, targetBody);         

            this.AddParameter(new PreLaunch(), null);
            this.civ3 = this.AddParameter(new TargetDockingGoal(vesselID, vesselName), null);

            MissionControllerEC.CivName.Clear();
            MissionControllerEC.civNamesListAdd();

            choice1 = UnityEngine.Random.Range(0, 7);
            name1 = MissionControllerEC.CivName[choice1];
            choice2 = UnityEngine.Random.Range(8, 12);
            name2 = MissionControllerEC.CivName[choice2];
            choice3 = UnityEngine.Random.Range(13, 17);
            name3 = MissionControllerEC.CivName[choice3];
            choice4 = UnityEngine.Random.Range(18, 23);
            name4 = MissionControllerEC.CivName[choice4];

            if (civiliansAmount == 2)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, civdestination), null);
                civ2.SetFunds(50000, 5000, targetBody);
                civ2.SetReputation(20, 40, targetBody);
                civ2.DisableOnStateChange = false;
            }
            if (civiliansAmount == 3)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, name3, civdestination), null);
                civ2.SetFunds(75000, 5000, targetBody);
                civ2.SetReputation(30, 60, targetBody);
                civ2.DisableOnStateChange = false;
            }
            if (civiliansAmount == 4)
            {
                this.civ2 = this.AddParameter(new CivilianModule(targetBody, civiliansAmount, name1, name2, name3, name4, civdestination), null);
                civ2.SetFunds(100000, 5000, targetBody);
                civ2.SetReputation(40, 80, targetBody);
                civ2.DisableOnStateChange = false;
            }
           
            this.civ5 = this.AddParameter(new LandOnBody(targetBody), null);
            civ5.SetFunds(20000, 20000, targetBody);
            civ5.SetReputation(20, 40, targetBody);            

            this.AddParameter(new GetSeatCount(civiliansAmount, crewSeatTitle), null);

            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(15000, 225000, 350000, targetBody);
            base.SetReputation(50, 150, targetBody);
            return true;
        }

        protected override void OnAccepted()
        {

            string AgenaMessage = "The civilians that are assigned to your vessel for the Contract Tour are represented in game by seats.  They do not show up as Individual Kerbals in " +
                "the game! Make no mistake though they are on your vessel.  If you fill the seats they need, then you cannot finish the contract.\n" +

                "Even if the objective is Green Check marked,  If you try to cheat and Fill the seat later on the objective will GO BACK to Not Finished!";

            MessageSystem.Message m = new MessageSystem.Message("About the Passengers", AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);
            MessageSystem.Instance.AddMessage(m);
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
            return "Civilian Station Expedition";
        }

        protected override string GetHashString()
        {
            return "Bring Civilians on an Expedition of your station named " + vesselName;
        }
        protected override string GetTitle()
        {
            return "Bring Civilians on an Expedition of your station named " + vesselName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return civiliansAmount + " Civilian kerbals have signed a contracted with us to bring them to your orbital station named " + vesselName + " for a set amount of time.\n The vessel must have room for the amount of civilians specified in the contract. " +
                "Failure to have the space available will cause the contract to be null and void.\n\n" +
                "It’s also very important that nothing bad happens to our guest while in our care.  If anything tragic happens the financial burdens on the Space Agency could be the end of us!\n\n" +
                "Please take note civilians are not allowed to take part in operations of KSC Personal duties, they are on the vessel as passengers only.  For this reason you as player cannot use them as an in game asset.  \n" +
                "But do not take up their seats or you will lose the contract!"
;
        }
        protected override string GetSynopsys()
        {
            return "Civilian expedition to Space Station " + vesselName  + " " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "The civilians thank you for bringing them home alive and showing them the wonders of your Space Station, they have learned a lot of iformation while staying at " + vesselName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            civiliansAmount = int.Parse(node.GetValue("civilians"));
            name1 = node.GetValue("name1");
            name2 = node.GetValue("name2");
            name3 = node.GetValue("name3");
            name4 = node.GetValue("name4");           
            TripTime = double.Parse(node.GetValue("time"));
            TripText = node.GetValue("triptext");
            civdestination = node.GetValue("civd");
            vesselID = node.GetValue("vesselid");
            vesselName = node.GetValue("vesselname");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("civilians", civiliansAmount);
            node.AddValue("name1", name1);
            node.AddValue("name2", name2);
            node.AddValue("name3", name3);
            node.AddValue("name4", name4);            
            node.AddValue("time", TripTime);
            node.AddValue("triptext", TripText);
            node.AddValue("civd", civdestination);
            node.AddValue("vesselid", vesselID);
            node.AddValue("vesselname", vesselName);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (!techUnlock || !techUnlock2 || st.Civilian_Contracts_Off == true)
                return false;
            else
                return true;
        }
    }
    #endregion

    public class TechList
    {
        public string techName = "";

        public TechList(string name)
        {
            this.techName = name;
        }
    }
}
