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
            totalContracts = ContractSystem.Instance.GetCurrentContracts<DeliverSatellite>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<DeliverSatellite>().Count();

            //Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1 && !SaveInfo.NoSatelliteContracts)
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
                maxTon = UnityEngine.Random.Range(st.contracSatelliteMinAMassTrivial, st.contracSatelliteMaxAMassTrivial);
                capResources = UnityEngine.Random.Range(2, 5) * 100;
                GMaxApA = UnityEngine.Random.Range((int)st.contracSatelliteMaxApATrivial, (int)st.contracSatelliteMaxTotalHeightTrivial + (int)st.contracSatelliteMaxApATrivial);
                GMinApA = GMaxApA - st.contracSatelliteBetweenDifference;
                GMaxPeA = GMaxApA;
                GMinPeA = GMinApA;
                base.SetFunds(35000f * st.ContractPaymentMultiplier, 56000f * st.ContractPaymentMultiplier, 35000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(15f, 35f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Significant)
            {
                maxTon = UnityEngine.Random.Range(st.contracSatelliteMinMassSignificant, st.contracSatelliteMaxMassSignificant);
                capResources = UnityEngine.Random.Range(3, 9) * 100;
                if (targetBody.flightGlobalsIndex == 2 || targetBody.flightGlobalsIndex == 3)
                {
                    GMaxApA = UnityEngine.Random.Range(75000,150000);
                }
                else
                {
                    GMaxApA = UnityEngine.Random.Range((int)st.contracSatelliteMaxApASignificant, (int)st.contracSatelliteMaxTotalHeightSignificant + (int)st.contracSatelliteMaxApASignificant);
                }
                GMinApA = GMaxApA - st.contracSatelliteBetweenDifference;
                GMaxPeA = GMaxApA;
                GMinPeA = GMinApA;
                base.SetFunds(45000f * st.ContractPaymentMultiplier, 78000f * st.ContractPaymentMultiplier, 45000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(25f, 45f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Exceptional)
            {
                maxTon = UnityEngine.Random.Range(st.contracSatelliteMinMassExcept, st.contracSatelliteMaxMassExcept);
                capResources = UnityEngine.Random.Range(5, 13) * 100;
                if (targetBody.flightGlobalsIndex == 2 || targetBody.flightGlobalsIndex == 3)
                {
                    GMaxApA = UnityEngine.Random.Range(35000, 120000);
                }
                else
                {
                    GMaxApA = UnityEngine.Random.Range((int)st.contracSatelliteMaxApAExcept, (int)st.contracSatelliteMaxTotalHeightExcept + (int)st.contracSatelliteMaxApAExcept);
                }
                GMinApA = GMaxApA - st.contracSatelliteBetweenDifference;
                GMaxPeA = GMaxApA;
                GMinPeA = GMinApA;
                base.SetFunds(55000f * st.ContractPaymentMultiplier, 95000f * st.ContractPaymentMultiplier, 55000f * st.ContractPaymentMultiplier, targetBody);
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
            this.satellite2 = this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA), null);
            satellite2.SetFunds(2000, targetBody);
            if (ifInclination && targetBody.flightGlobalsIndex == 1)
            {
               this.satellite3 = this.AddParameter(new Inclination(MinInc, MaxInc));
               satellite3.SetFunds(8000, targetBody);
               satellite3.SetReputation(10, targetBody);
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
            totalContracts = ContractSystem.Instance.GetCurrentContracts<DeliverSatOrbitalPeriod>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<DeliverSatOrbitalPeriod>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            //Debug.Log("Orbital Period Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1 && !SaveInfo.NoOrbitalPeriodcontracts)
            {
                //Debug.Log("contract is generated right now terminating Orbital Period Satellite Mission");
                //Debug.Log("count is " + totalContracts);
                return false;                 
            }

            targetBody = Planetarium.fetch.Home;
            MinInc = MaxInc - 10;
            MinOrb = st.contractOrbitalPeriodMinInSeconds;
            MaxOrb = st.contractOrbitalPeriodMaxInSeconds;
            bool ifInclination = false;

            if (test >= 75)
                ifInclination = true;
            else
                ifInclination = false;

            if (this.prestige == ContractPrestige.Trivial)
            {
                maxTon = UnityEngine.Random.Range(st.contracSatelliteMinAMassTrivial, st.contracSatelliteMaxAMassTrivial);
                capResources = UnityEngine.Random.Range(2, 5) * 100;
                base.SetFunds(39000f * st.ContractPaymentMultiplier, 68000f * st.ContractPaymentMultiplier, 39000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(15f, 35f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Significant)
            {
                maxTon = UnityEngine.Random.Range(st.contracSatelliteMinMassSignificant, st.contracSatelliteMaxMassSignificant);
                capResources = UnityEngine.Random.Range(3, 6) * 100;
                base.SetFunds(49000f * st.ContractPaymentMultiplier, 89000f * st.ContractPaymentMultiplier, 49000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(25f, 45f, targetBody);
            }
            else if (this.prestige == ContractPrestige.Exceptional)
            {
                maxTon = UnityEngine.Random.Range(st.contracSatelliteMinMassExcept, st.contracSatelliteMaxMassSignificant);
                capResources = UnityEngine.Random.Range(4, 9) * 100;
                base.SetFunds(59000f * st.ContractPaymentMultiplier, 108000f * st.ContractPaymentMultiplier, 59000f * st.ContractPaymentMultiplier, targetBody);
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
        string titleName = "Repair Vessel";

        double maxApA;
        string repairParts = "repairParts";
        double RPamount = 1;

        ContractParameter repairgoal1;
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
            if (totalContracts >= 1 && !SaveInfo.NoRepairContracts)
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
                base.SetFunds(45000f * st.ContractPaymentMultiplier, 71000f * st.ContractPaymentMultiplier, 51000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
                base.SetScience(3.0f, targetBody);
            }
            if (maxApA > 120000 && maxApA <= 1000000)
            {
                base.SetFunds(70000f * st.ContractPaymentMultiplier, 95000f * st.ContractPaymentMultiplier, 70000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
                base.SetScience(10.0f, targetBody);
            }
            if (maxApA > 1000001)
            {
                base.SetFunds(120000f * st.ContractPaymentMultiplier, 130000f * st.ContractPaymentMultiplier, 120000f * st.ContractPaymentMultiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
                base.SetScience(20.0f, targetBody);
            }

            this.repairgoal1 = this.AddParameter(new TargetDockingGoal(vesselID,vesselName), null);
            repairgoal1.SetFunds(5000, targetBody);
            repairgoal1.SetReputation(5, targetBody);
            this.repairgoal2 = this.AddParameter(new RepairPanelPartCheck(titleName), null);
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
            totalContracts = ContractSystem.Instance.GetCurrentContracts<OrbitalScanContract>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<OrbitalScanContract>().Count();
            //Debug.Log("Orbital Research Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1 && !SaveInfo.NoOrbitalResearchContracts)
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
            base.SetFunds(34000f * st.ContractPaymentMultiplier, 53000f * st.ContractPaymentMultiplier, 34000f * st.ContractPaymentMultiplier, targetBody);

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
            return "Conduct Unmanned Orbital Research around " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", MissionSeed);
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

            totalContracts = ContractSystem.Instance.GetCurrentContracts<LanderResearchScan>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<LanderResearchScan>().Count();
            //Debug.Log("Land Research Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (totalContracts >= 1 && !SaveInfo.NoLanderResearchContracts)
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
            base.SetFunds(37000f * st.ContractPaymentMultiplier, 66000f * st.ContractPaymentMultiplier, 77000f * st.ContractPaymentMultiplier, targetBody);

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
            return "Conduct Ground research on " + targetBody.theName;
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
            StartNetwork = settings.StartBuilding;
            if (!StartNetwork)
            {
                //Debug.Log("ComSat Network is shut off, and set to false");
                return false;
            }
            targetBody = FlightGlobals.Bodies[settings.bodyNumber];
            ContractPlayerName = settings.contractName;
            MinOrb = settings.minOrbP;
            MaxOrb = settings.maxOrbP;

            this.AddParameter(new OrbitalPeriod(MinOrb, MaxOrb), null);
            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, partAmount), null);
            }
            this.AddParameter(new PreLaunch(), null);
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetExpiry(1f, 10f);
            base.SetScience(5f, targetBody);
            base.SetDeadlineYears(.3f, targetBody);
            base.SetReputation(25f, 40f, targetBody);
            base.SetFunds(39000f * settings.ContractPaymentMultiplier, 52000f * settings.ContractPaymentMultiplier, 37000f * settings.ContractPaymentMultiplier, targetBody);

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
    # region Agena Contract 1
    public class AgenaTargetPracticeContract : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double GMaxApA = 0;
        public double GMinApA = 0;
        public double GMaxPeA = 0;
        public double GMinPeA = 0;
        public int crewCount = 0;
        public string partName = "ModuleDockingNode";
        public string ModuleTitle = "Any Docking Port";

        public int totalContracts;
        public int TotalFinished;
        public int Agena1Done;

        ContractParameter AgenaParameter;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AgenaTargetPracticeContract>().Count();
            Agena1Done = ContractSystem.Instance.GetCompletedContracts<AgenaTargetPracticeContract>().Count();

            //Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (Agena1Done == 1 || SaveInfo.Agena1Done)
            {
                return false;
            }           
            if (totalContracts >= 1)
            {
                //Debug.Log("contract is generated right now terminating Normal Satellite Mission");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            targetBody = Planetarium.fetch.Home;
            GMaxApA = UnityEngine.Random.Range((int)st.contracSatelliteMaxApATrivial, (int)st.contracSatelliteMaxTotalHeightTrivial + (int)st.contracSatelliteMaxApATrivial);
            GMinApA = GMaxApA - st.contracSatelliteBetweenDifference;
            GMaxPeA = GMaxApA;
            GMinPeA = GMinApA;
          
            AgenaParameter = this.AddParameter(new AgenaInOrbit(targetBody), null);
            AgenaParameter.SetFunds(2000.0f, targetBody);
            AgenaParameter.SetReputation(20f, targetBody); 
            this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA), null);
            this.AddParameter(new ModuleGoal(partName, ModuleTitle), null);            
            this.AddParameter(new GetCrewCount(crewCount), null);
           
            base.SetExpiry(15f, 35f);
            base.SetScience(25f, targetBody);
            base.SetDeadlineDays(19f, targetBody);
            base.SetReputation(35f, 35f, targetBody);
            base.SetFunds(28000f * st.ContractPaymentMultiplier, 54000f * st.ContractPaymentMultiplier, 39000f * st.ContractPaymentMultiplier, targetBody);

            return true;
        }

        protected override void OnAccepted()
        {
            string AgenaMessage = "Please Take Note when you finish the Agena Contract that vessel will be recorded as the Agena Vessel for next mission!\n\n"+
                "if the wrong vessel is recorded you can change the vessel by using the Debug Tools in settings for Agena Contract";
            MessageSystem.Message m = new MessageSystem.Message("Important Agena Target Contract Information", AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW,MessageSystemButton.ButtonIcons.MESSAGE);             
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
        
        protected override string GetHashString()
        {
            return targetBody.bodyName + GMaxApA.ToString() + GMinApA.ToString();
        }
        protected override string GetTitle()
        {
            return "Agena Target Vehicle Orbital Test Around Kerbin - Launch Agena Vehicle";
        }
        protected override string GetDescription()
        {

            return "The Agena Target Vehicle (ATV) was an unmanned spacecraft used by NASA during its Gemini program to develop and practice orbital space rendezvous and docking techniques and\n" +
                "to perform large orbital changes, in preparation for the Apollo program lunar missions.\n\n" +
                "Your first task is to launch an Agena Type vehicle into orbit\n\n" +
                "Please Take Note when you finish the Agena Contract that vessel will be recorded as the Agena Vessel for next mission!";
        }
        protected override string GetSynopsys()
        {
            return "Agena Test " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Agena1Done = true;
            SaveInfo.AgenaTargetVesselID = FlightGlobals.ActiveVessel.id.ToString();
            SaveInfo.AgenaTargetVesselName = FlightGlobals.ActiveVessel.vesselName.Replace("(unloaded)", ""); 
            return FlightGlobals.ActiveVessel.name + " Vessel ID is " + FlightGlobals.ActiveVessel.id + "\n\n" +
                "If this is not correct you can change this is Debug menu using select Agena vessel Debug Tool\n\n" +
                "Congradulations you have succesfully launched your Agena Target Vehicle, now you must get you Manned Orbital vehicle to Dock with the ATV\n\n" +
                "The Gemini-Agena Target Vehicle design was an adaptation of the basic Agena-D vehicle using the alternate Model 8247 rocket engine and additional program-peculiar equipment required for the Gemini mission.\n" +
                "This GATV was divided into:\n\n" +

"The program-peculiar forward auxiliary section. This section consisted of the auxiliary equipment rack, the McDonnell Aircraft Company-furnished docking-adapter module, and the clamshell nose shroud.\n" +
"The Agena-D forward and mid-body sections. The Agena-D forward section housed the main equipment bay, and the mid-body contained the main fuel and oxidizer tanks which supplied propellants through a feed and\n" +
"load system for the main engine. (3) the program-peculiar aft section. The Model 8247 multi-start main engine and the smaller Model 8250 maneuvering and ullage orientation engines were located in this section.\n" +
"Orbital length of the GATV was approximately 26 feet. Vehicle weight-on-orbit was approximately 7200 lb. This weight included propellants still remaining in the main tanks and available for Model 8247 engine operation\n" +
"after the Agena achieved orbit.\n\n" +
"The Gemini-ATV propulsion system consisted of the following:\n\n" +

"Model 8247 rocket engine, also known as XLR-81-BA-13, and its controls, mount, gimbals, and titanium nozzle extension\n" +
"Pyrotechnically operated helium-control valve (POHCV) and associated pressurization plumbing\n" +
"Fuel and oxidizer feed and load system, including propellant tanks, vents, and fill quick disconnects\n" +
"Propellant isolation valves (PIV's)\n" +
"All associated pyro devices and solid-propellant rockets.\n\n" +
"All Information For Agena was Gathered From www.astronautix.com";            
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

            ModuleTitle = node.GetValue("moduletitle");
            partName = node.GetValue("pName");
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

            
            
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("moduletitle", ModuleTitle);

            node.AddValue("crewcount", crewCount);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }


    }
#endregion
    # region Agena Contract 2
    public class AgenaTargetPracticeContractDocking : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;        
        public int crewCount = 1;
        public int partAmount = 1;
        public string partName = "Clamp-O-Tron Docking Port";

        public double GMaxApA = 0;
        public double GMinApA = 0;
        public double GMaxPeA = 0;
        public double GMinPeA = 0;

        public string vesselTestID = "none";
        public string vesselTestName = "none";

        public int totalContracts;
        public int Agena1Done;
        public int Agena2Done;

        ContractParameter AgenaDockParameter;


        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AgenaTargetPracticeContractDocking>().Count();
            Agena1Done = ContractSystem.Instance.GetCompletedContracts<AgenaTargetPracticeContract>().Count();
            Agena2Done = ContractSystem.Instance.GetCompletedContracts<AgenaTargetPracticeContractDocking>().Count();

            //Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished)
            if (Agena1Done != 1 || Agena2Done == 1 || SaveInfo.Agena2Done)
            {
                //Debug.Log("Agena 1 Is not Done Yet, rejecting Contract 2 Docking");
                return false;
            }
            if (totalContracts >= 1)
            {
                //Debug.Log("Agena 2 is already loaded.");
                return false;
            }
            targetBody = Planetarium.fetch.Home;
            GMaxApA = UnityEngine.Random.Range((int)st.contracSatelliteMaxApAExcept, (int)st.contracSatelliteMaxApAExcept + (int)st.contracSatelliteMaxTotalHeightExcept);
            GMinApA = GMaxApA - st.contracSatelliteBetweenDifference;
            GMaxPeA = GMaxApA;
            GMinPeA = GMinApA;

            vesselTestID = SaveInfo.AgenaTargetVesselID;
            vesselTestName = SaveInfo.AgenaTargetVesselName;
            AgenaDockParameter = this.AddParameter(new TargetDockingGoal(vesselTestID, vesselTestName), null);
            AgenaDockParameter.SetFunds(3000.0f, targetBody);
            AgenaDockParameter.SetReputation(30f, targetBody);

            this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA), null);  

            this.AddParameter(new PartGoal(partName, partAmount), null);
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(15f, 35f);
            base.SetScience(25f, targetBody);
            base.SetDeadlineDays(20f, targetBody);
            base.SetReputation(50f, 35f, targetBody);
            base.SetFunds(29000f * st.ContractPaymentMultiplier, 48000f * st.ContractPaymentMultiplier, 42000f * st.ContractPaymentMultiplier, targetBody);

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
            return targetBody.bodyName + vesselTestName;
        }
        protected override string GetTitle()
        {
            return "Agena Target Vehicle Orbital Test Around Kerbin - Dock With ATV " + vesselTestName;
        }
        protected override string GetDescription()
        {

            return "Project Gemini was the second human spaceflight program of NASA, the civilian space agency of the United States government. Project Gemini was conducted between projects Mercury\n" +
                " and Apollo, with ten manned flights occurring in 1965 and 1966.\n\n"+

                 "Its objective was to develop space travel techniques in support of Apollo, which had the goal of landing men on the Moon. Gemini achieved missions long enough for a trip to the Moon\n" +
                 "and back, perfected extra-vehicular activity (working outside a spacecraft), and orbital maneuvers necessary to achieve rendezvous and docking. All Gemini flights were launched from \n" + 
                 " Cape Canaveral, Florida using the Titan II Gemini launch vehicle\n\n" +
                 "Info For Gemini From Wikipedia.org\n\n" +
                "Your Second Task Is To Dock your Manned Orbital Pod with Agena Target Vehicle.  Then you are required to change Altitude to the selected ApA and PeA";
        }
        protected override string GetSynopsys()
        {
            return "Agena Test " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Agena2Done = true;
            return "You have been succesful with Launching an Agena Type Craft, Docking with it, and changing your Orbital Altitude.  Congradulations!\n\n" +                
                "The first GATV was launched on October 25, 1965 while the Gemini 6 astronauts were waiting on the pad. While the Atlas performed normally,\n" +
                "the Agena's engine exploded during orbital injection. Since the rendezvous and docking was the primary objective, the Gemini 6 mission was scrubbed,\n" +
                "and replaced with the alternate mission Gemini 6A, which rendezvoused (but could not dock) with Gemini 7 in December.\n\n" +
                "Was not until Gemini 10 That all objectives of Launching, Docking, and boosting Gemini 10 to 412-nautical-mile change succeded.";
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

            int pcount = int.Parse(node.GetValue("pCount"));
            partAmount = pcount;
            partName = (node.GetValue("pName"));
            crewCount = int.Parse(node.GetValue("crewcount"));

            vesselTestID = node.GetValue("vesselid");
            vesselTestName = node.GetValue("vesselname");

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


            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);

            node.AddValue("crewcount", crewCount);

            node.AddValue("vesselid", vesselTestID);
            node.AddValue("vesselname", vesselTestName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
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
}
