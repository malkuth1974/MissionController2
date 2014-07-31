using System;
using System.IO;
using System.Collections;
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
        CelestialBody targetBody = null;
        public double GMaxApA = UnityEngine.Random.Range(75000, 300000);
        public double GMinApA = 0;
        public double GMaxPeA = 0;
        public double GMinPeA = 0;
        public int crewCount = 0;
        public bool techUnlocked = false;
        public double MinInc = 0;
        public int partAmount = 1;
        public string partName = "repairPanel";
        public double MaxInc = UnityEngine.Random.Range(10, 90);
        public int test = UnityEngine.Random.Range(0, 100);

        public int totalContracts;
        public int TotalFinished;

       
        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<DeliverSatellite>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<DeliverSatellite>().Count();

            Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                Debug.Log("contract is generated right now terminating Normal Satellite Mission");
                Debug.Log("count is " + totalContracts);
                return false;                 
            }            

            targetBody = Planetarium.fetch.Home;
            GMinApA = GMaxApA - 5000;
            GMaxPeA = GMaxApA;
            GMinPeA = GMinApA;
            MinInc = MaxInc - 10;
            bool ifInclination = false;

            if (test >= 50)
                ifInclination = true;
            else
                ifInclination = false;
            this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA), null);
            if (ifInclination)
            {
                this.AddParameter(new Inclination(MinInc, MaxInc));
            }
            this.AddParameter(new PartGoal(partName, partAmount), null);
            this.AddParameter(new GetCrewCount(crewCount), null);
            
            base.SetExpiry(3f,10f);
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(15f, 35f, targetBody);
            if (!ifInclination)
                base.SetFunds(6000f, 22000f, 7000f, targetBody);
            else
                base.SetFunds(8000f, 24000f, 9000f, targetBody);

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

        protected override string GetHashString()
        {
            return targetBody.bodyName + GMaxApA.ToString() + GMinApA.ToString();
        }
        protected override string GetTitle()
        {
            return "Launch Company Satellite Into Orbit around " + targetBody.theName + ": " + TotalFinished;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", new System.Random().Next());
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
            if (techUnlock)
                return true;
            else
                return false;
        }

        public double AeAID { get; set; }
    }
    #endregion
    #region Deliver Satellite to Orbital Period
    public class DeliverSatOrbitalPeriod : Contract
    {
        CelestialBody targetBody = null;

        public double MinOrb = 21480;
        public double MaxOrb = 21660;
        public double MinInc = 0;
        public double MaxInc = UnityEngine.Random.Range(10, 90);
        public int test = UnityEngine.Random.Range(0, 100);
        public bool testThis = false;
        public int crewCount = 0;

        public int partAmount = 1;
        public string partName = "repairPanel";

        public int totalContracts;
        public int TotalFinished;


        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<DeliverSatOrbitalPeriod>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<DeliverSatOrbitalPeriod>().Count();

            Debug.Log("Orbital Period Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                Debug.Log("contract is generated right now terminating Orbital Period Satellite Mission");
                Debug.Log("count is " + totalContracts);
                return false;                 
            }

            targetBody = Planetarium.fetch.Home;
            MinInc = MaxInc - 10;
            bool ifInclination = false;

            if (test >= 50)
                ifInclination = true;
            else
                ifInclination = false;
            this.AddParameter(new OrbitalPeriod(MinOrb, MaxOrb), null);
            if (ifInclination)
            {
                this.AddParameter(new Inclination(MinInc, MaxInc));
            }
            this.AddParameter(new PartGoal(partName, partAmount), null);
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetExpiry(3f, 10f);
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(15f, 35f, targetBody);
            if (!ifInclination)
                base.SetFunds(8000f, 27000f, 10000f, targetBody);
            else
                base.SetFunds(10000f, 28000f, 11000f, targetBody);

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
            return targetBody.bodyName + MaxOrb + MinOrb;
        }
        protected override string GetTitle()
        {
            return "Deliver a Satellite To an Orbital Period of Six Hours " + targetBody.theName + ": " + TotalFinished;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", new System.Random().Next());
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
    #region Repair Goal Contract
    public class RepairGoal : Contract
    {
        CelestialBody targetBody = null;
        string vesselID;
        string vesselName;
        bool NoVessel = false;
        string titleName = "Repair Vessel";

        int randomWeightSystem = UnityEngine.Random.Range(1, 100);

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
                            repairvesselList.Add(new RepairVesselsList(vs.name,vs.id.ToString()));
                            Debug.Log("MCE***" + vs.name + " Loaded Vessels With RepairStation Parts");
                        }
                    }
                }
            }
        }

        public void chooseVesselRepairFromList()
        {            
            System.Random rnd = new System.Random();
            if (repairvesselList.Count > 0)
            {
                RepairVesselsList random = repairvesselList[rnd.Next(repairvesselList.Count)];
                vesselID = random.vesselId.ToString();
                vesselName = random.vesselName;
                Debug.LogWarning("Random Repair Vessel Selected " + random.vesselName);
                NoVessel = true;
            }
            else { Debug.LogError(" Vessel Selection Null, skiped process"); NoVessel = false; }
        }

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<RepairGoal>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<RepairGoal>().Count();
            Debug.Log(" Repair Contract Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            findVeselWithRepairPart();
            chooseVesselRepairFromList();
            if (totalContracts >= 1 || NoVessel == false || randomWeightSystem >= 60)
            {
                if (totalContracts >= 1)
                {
                    Debug.Log("contract is generated right now terminating Repair Vessel");
                }
                if (!NoVessel)
                {
                    Debug.Log("no vessel found current vessel is: " + vesselName);
                }
                if (randomWeightSystem >= 60)
                {
                    Debug.Log("Failed random weight system weight is set at: " + randomWeightSystem);
                }
                Debug.Log("count is " + totalContracts);
                return false;  
            }
            Debug.Log("Random Weight System set at: " + randomWeightSystem + " System passed by being greater than 60");
                                                                   
            targetBody = Planetarium.fetch.Home;
            this.AddParameter(new TargetDockingGoal(vesselID,vesselName), null);
            this.AddParameter(new RepairPanelPartCheck(titleName), null);
            base.SetExpiry(3f, 10f);
            base.SetScience(3.0f, targetBody);
            base.SetDeadlineYears(.1f, targetBody);
            base.SetReputation(20f, 25f, targetBody);
            base.SetFunds(12000f, 31000f, 15000f, targetBody);

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
            return "Repair Vessel " + vesselName + " by docking with it, then repairing it";
        }
        protected override string GetTitle()
        {
            return "Repair Contract for Satellite: " + " " + vesselName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Hello, we would like you to launch a Manned Vessel to our satellite.  Dock with it, transfer some repair Parts to the RepairPanel.  After this you will have to go EVA and open the door and conduct Repairs.";
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
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            vesselID = node.GetValue("VesselID");
            vesselName = node.GetValue("VesselName");
            titleName = node.GetValue("titlename");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("VesselID", vesselID);
            node.AddValue("VesselName", vesselName);
            node.AddValue("titlename", titleName);
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
        CelestialBody targetBody = null;
        int crewCount = 0;
        public double testpos = 0;
        string partName = "orbitalResearch";
        int partNumber = 1;

        double missionTime = Tools.RandomNumber(200, 1500);

        
        

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<OrbitalScanContract>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<OrbitalScanContract>().Count();
            Debug.Log("Orbital Research Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                Debug.Log("Orbital Research contract is generated right now terminating Mission");
                Debug.Log("count is " + totalContracts);
                return false;
            }
            System.Random generator = new System.Random(this.MissionSeed);
            int bodynumbers = FlightGlobals.Bodies.Count;
            int bodyChoice = Tools.RandomNumber(0, bodynumbers);
            Debug.Log("BodyChoice " + bodyChoice);
            if (targetBody == null)
                targetBody = FlightGlobals.Bodies[bodyChoice];
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new OrbialResearchPartCheck(targetBody,missionTime), null);
            this.AddParameter(new PartGoal(partName, partNumber), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(15f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(5f, 3f, targetBody);
            base.SetFunds(24000f, 30000f, 21000f, targetBody);

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
            return "Conduct Unmanned Orbital Research around " + targetBody.theName + ": " + TotalFinished;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", new System.Random().Next());
        }
        protected override string GetSynopsys()
        {
            return "Conduct Orbital Reasearch around " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            MCEOrbitalScanning.doOrbitResearch = false;
            return "You have successfully researched the planet body " + targetBody.theName;
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
           
            node.AddValue("crewcount",crewCount);
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
    }
    #endregion
    #region Lander Scanning Contract
    public class LanderResearchScan : Contract
    {
        CelestialBody targetBody = null;
        int crewCount = 0;
        public double testpos = 0;
        string partName = "LanderResearch";
        int partNumber = 1;

        double amountTime = Tools.RandomNumber(200, 1500);

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<LanderResearchScan>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<LanderResearchScan>().Count();
            Debug.Log("Land Research Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1)
            {
                Debug.LogWarning("Land Research contract is generated right now terminating Mission");
                Debug.Log("count is " + totalContracts);
                return false;
            }
            System.Random generator = new System.Random(this.MissionSeed);
            int bodynumbers = FlightGlobals.Bodies.Count;
            int bodyChoice = Tools.RandomNumber(2, bodynumbers);
            Debug.Log("BodyChoice " + bodyChoice);
            if (targetBody == null)
                targetBody = FlightGlobals.Bodies[bodyChoice]; 
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new LandOnBody(targetBody), null);
            this.AddParameter(new LanderResearchPartCheck(targetBody,amountTime), null);
            this.AddParameter(new PartGoal(partName, partNumber), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(35f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(12f, 11f , targetBody);
            base.SetFunds(27000f, 35000f, 24000f, targetBody);

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
            return "Conduct Ground research on " + targetBody.theName + ": " + TotalFinished;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", new System.Random().Next());
        }
        protected override string GetSynopsys()
        {
            return "Land a Unmanned Vessel on " + targetBody.theName + " and conduct research for our company";
        }
        protected override string MessageCompleted()
        {
            MCELanderResearch.doLanderResearch = false;
            return "You have successfully landed and researched on " + targetBody.theName;
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
    }
    #endregion
}
