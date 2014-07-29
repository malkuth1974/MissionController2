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

        public int ContractCount = ContractSystem.Instance.GetCurrentContracts<DeliverSatellite>().Count();
       
        protected override bool Generate()
        {
            Debug.Log("ContractCount is: " + ContractCount);
            if (ContractCount >= 1)
            {
                Debug.Log("contract is generated right now terminating DeliverSat");
                return false;
            }
            Debug.Log("contracts exist" + contractsInExistance);
            Debug.Log("Contract State" + ContractState);

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
            
            base.SetExpiry();
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
            return "Launch Satellite Into Orbit. " + targetBody.theName;
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

        public int ContractCount = ContractSystem.Instance.GetCurrentContracts<DeliverSatOrbitalPeriod>().Count();
       
        protected override bool Generate()
        {            
            if (ContractCount >= 1)
            {
                Debug.Log("contract is generated right now terminating OrbitalPeriod");
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
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetExpiry();
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
            return "Launch Satellite Into a 6 Hour Orbital Period. " + targetBody.theName;
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
        bool repairCheck = RepairPanel.repair;

        public List<RepairVesselsList> repairvesselList = new List<RepairVesselsList>();
               
        public int ContractCount = ContractSystem.Instance.GetCurrentContracts<DeliverSatOrbitalPeriod>().Count();

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

        protected override bool Generate()
        {
            findVeselWithRepairPart();
            chooseVesselRepairFromList();
            if (ContractCount >= 1 || !NoVessel)
            {
                Debug.Log("contract is generated right now terminating OrbitalPeriod");
                return false;
            }
                       
            targetBody = Planetarium.fetch.Home;
            this.AddParameter(new TargetDockingGoal(vesselID,vesselName), null);
            this.AddParameter(new SimplePartCheck(titleName,repairCheck), null);
            base.SetExpiry();
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
            return "Repair Satellite: " + targetBody.theName + " " + vesselName;
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
            repairCheck = bool.Parse(node.GetValue("value1"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("VesselID", vesselID);
            node.AddValue("VesselName", vesselName);
            node.AddValue("titlename", titleName);
            node.AddValue("value1", repairCheck);
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
        string titleName;
        bool isScanned = MCEOrbitalScanning.doResearch;
        int crewCount = 0;
        public double testpos = 0;
        public static float mult = 0;

        public int ContractCount = ContractSystem.Instance.GetCurrentContracts<DeliverSatOrbitalPeriod>().Count();

        public static void Multiplier(int val)
        {
            switch (val)
            {
                case 0:
                     mult = 1;
                     break;
                case 1:
                     mult = (int)1.0;
                    break;
                case 2:
                    mult = (int)1.2;
                    break;
                case 3:
                    mult = (int)1.3;
                    break;
                case 4:
                    mult = (int)1.4;
                    break;
                case 5:
                    mult = (int)1.6;
                    break;
                case 6:
                    mult = (int)1.8;
                    break;
                case 7:
                    mult = (int)1.9;
                    break;
                case 9:
                    mult = (int)2;
                    break;
                case 10:
                    mult = (int)2.2;
                    break;
                case 11:
                    mult = (int)2.5;
                    break;
                case 12:
                    mult = (int)2.8;
                    break;
                case 13:
                    mult = (int)3;
                    break;
                case 14:
                    mult = (int)3.3;
                    break;
                case 15:
                    mult = (int)3.6;
                    break;
                case 16:
                    mult = (int)3.9;
                    break;
                case 17:
                    mult = (int)4.2;
                    break;
                default:
                    mult = (int)1;
                    break;
            }
        }
       
        protected override bool Generate()
        {            
            if (ContractCount >= 1)
            {
                Debug.Log("contract is generated right now terminating OrbitalPeriod");
                return false;               
            }
            int bodynumbers = FlightGlobals.Bodies.Count;
            int bodyChoice = Tools.RandomNumber(0,bodynumbers);
            Debug.Log("BodyChoice " + bodyChoice);
            if (targetBody == null)
				targetBody = FlightGlobals.Bodies[bodyChoice];           
            targetBody.getPositionAtUT(testpos);
            titleName = "Conduct Orbital Research Around " + targetBody;           
            Multiplier(bodyChoice);
            Debug.Log("Multiplier is " + mult);
            this.AddParameter(new InOrbitGoal(targetBody),null);
            this.AddParameter(new SimplePartCheck(titleName,isScanned));
            base.SetExpiry();
            base.SetScience(15f * mult, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(5f * mult, 3f * mult, targetBody);
            base.SetFunds(21000f * mult, 29000f * mult, 19000f, targetBody);

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
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Conduct Orbital Research around. " + targetBody.theName;
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
            titleName = node.GetValue("titlename");
            isScanned = bool.Parse(node.GetValue("value1"));
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID); 
           
            node.AddValue("crewcount",crewCount);
            node.AddValue("titlename", titleName);
            node.AddValue("value1", isScanned);
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

}
