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
    #region Repair Goal Contract
    public class RepairGoal : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public int planetIDX = 1;
        public string vesselID;
        public string vesselName;
        public bool NoVessel = false;
        public string titleName = "Repair Vessel ";
        public double maxApA;
        public string repairParts = "SpareParts";
        public double RPamount = 1;
        ContractParameter repairgoal2;
        public string Ctitle = "To Repair Vessel You must have at Least ";
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
                            if (vs.vesselType == VesselType.Probe)
                            {
                                repairvesselList.Add(new RepairVesselsList(vs.vesselName, vs.id.ToString(), vs.orbit.ApA, vs.mainBody.flightGlobalsIndex));
                            }
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
                planetIDX = random.bodyidx;
                Debug.LogWarning("(repair)bodyIDX is: " + "   " + random.bodyidx);
                Debug.LogWarning("Random Repair Vessel Selected " + random.vesselName + "  " + random.vesselId + "  " + random.MaxApA);
                NoVessel = true;
            }
            else { Debug.LogError(" Vessel Selection Null for repair Contract, skiped process"); NoVessel = false; }
        }

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
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
            targetBody = FlightGlobals.Bodies[planetIDX];
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
            return "An issue has arised from one of our satellites in orbit, we would like to contract your agency out to launch and fix this issue\n\n" +
                "How To Conduct Repairs\n\n" +
                "1. You need Spare Parts To conduct Repairs.\n 2. Launch and intercept the target satellite (no need to dock unless you want to). \n" +
                "3. When you go EVA Grab some SpareParts while EVA using KERT, right click part that has Spare Parts and use the KERT GUI to transfer the Spare Parts to your EVA Kerbal\n" +
                "4. Go to the nearest Repair Panel and Transfer the Spare Parts to The Repair Panel using KERT GUI Again.\n5. Open the Repair Panel and Select Test System.\n6. Once system is tested and passes select Repair. All Done!";
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
            planetIDX = int.Parse(node.GetValue("planetIDX"));
            vesselID = node.GetValue("VesselID");
            vesselName = node.GetValue("VesselName");
            titleName = node.GetValue("titlename");
            maxApA = double.Parse(node.GetValue("maxapa"));
            repairParts = node.GetValue("repairparts");
            RPamount = double.Parse(node.GetValue("rpamount"));
            Ctitle = node.GetValue("ctitle");
            NoVessel = bool.Parse(node.GetValue("novessel"));
            targetBody = FlightGlobals.Bodies[planetIDX];
        }
        protected override void OnSave(ConfigNode node)
        {            
            node.AddValue("planetIDX", planetIDX);
            node.AddValue("VesselID", vesselID);
            node.AddValue("VesselName", vesselName);
            node.AddValue("titlename", titleName);
            node.AddValue("maxapa", maxApA);
            node.AddValue("repairparts", repairParts);
            node.AddValue("rpamount", RPamount);
            node.AddValue("ctitle",Ctitle);
            node.AddValue("novessel", NoVessel);
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
    #region Repair Station Contract
    public class RepairStation : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public int planetIDX = 1;
        public string vesselID;
        public string vesselName;
        public bool NoVessel = false;
        public string titleName = "Test";
        public string repairParts = "SpareParts";
        public double RPamount = 1;
        ContractParameter repairgoal2;
        public string Ctitle = "To Repair Station You must have at Least ";
        public List<RepairVesselsList> repairvesselListStations = new List<RepairVesselsList>();

        public int randomString = 0;
                
        public void findVeselWithRepairPartAndStation()
        {
            foreach (Vessel vs in FlightGlobals.Vessels)
            {
                foreach (ProtoPartSnapshot p in vs.protoVessel.protoPartSnapshots)
                {
                    foreach (ProtoPartModuleSnapshot m in p.modules)
                    {
                        if (m.moduleName.Equals("RepairPanel"))
                        {
                            if (vs.vesselType == VesselType.Station)
                            {
                                repairvesselListStations.Add(new RepairVesselsList(vs.vesselName, vs.id.ToString(), vs.orbit.ApA, vs.mainBody.flightGlobalsIndex));
                            }
                        }
                    }
                }
            }

        }
             
        public void chooseVesselRepairStationFromList()
        {
            System.Random rnd = new System.Random();
            if (repairvesselListStations.Count > 0)
            {
                RepairVesselsList random = repairvesselListStations[rnd.Next(repairvesselListStations.Count)];
                vesselID = random.vesselId.ToString();
                vesselName = random.vesselName.Replace("(unloaded)", "");
                planetIDX = random.bodyidx;
                Debug.LogWarning("(repair station)bodyIDX is: " + random.bodyidx);
                Debug.LogWarning("Random Repair Orbital Station Selected " + random.vesselName + "  " + random.vesselId + "  " + random.MaxApA);
                NoVessel = true;
            }
            //else { Debug.LogError(" Vessel Selection Null, skiped process"); NoVessel = false; }
        }

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<RepairStation>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<RepairStation>().Count();          
            if (totalContracts >= 1 || SaveInfo.NoRepairContracts)
            {
                return false;
            }
            if (!SaveInfo.RepairStationContract)
            {
                Debug.LogWarning("(Repair Station) contract random is false, contract not generated");
                return false;
            }
            titleName = "Find the Repair Panel on " + vesselName + " And start conducting repairs.";
            findVeselWithRepairPartAndStation();
            chooseVesselRepairStationFromList();
            randomString = Tools.RandomNumber(0, 4);
            targetBody = FlightGlobals.Bodies[planetIDX];
            if (targetBody = null)
            {
                targetBody = Planetarium.fetch.Home;
            }
            if (!NoVessel)
            {
                return false;
            }

            this.AddParameter(new EvaGoal(FlightGlobals.Bodies[planetIDX]), null);
            this.repairgoal2 = this.AddParameter(new RepairPanelPartCheck(titleName, vesselID, vesselName), null);
            repairgoal2.SetFunds(2000, targetBody);
            repairgoal2.SetReputation(10, targetBody);
            //this.AddParameter(new ResourceSupplyGoal(repairParts, RPamount, Ctitle), null);
            base.SetExpiry(1f, 3f);
            base.SetDeadlineYears(1f, targetBody);
            base.SetFunds(5000f * st.Contract_Payment_Multiplier, 30000f * st.Contract_Payment_Multiplier, 35000f * st.Contract_Payment_Multiplier, targetBody);
            base.SetReputation(75f, 125f, targetBody);
            base.SetScience(1.0f, targetBody);

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
            return "Eva And Repair Space Station ";
        }
        protected override string GetTitle()
        {
            return "EVA and Repair Contract for Space Station: " + " " + vesselName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Space Station " + vesselName + " has some issue and needs attention.  You are to EVA and head for the Repair panel and start repairs on the issues.\n\n"+
                "How To Conduct Repairs\n\n"+
                "1. You need Spare Parts To conduct Repairs.\n2. When you go EVA Grab some SpareParts while EVA using KERT, right click part that has Spare Parts and use the KERT GUI to transfer the Spare Parts to your EVA Kerbal\n" +
                "3. Go to the nearest Repair Panel and Transfer the Spare Parts to The Repair Panel using KERT GUI Again.\n4. Open the Repair Panel and Select Test System.\n5. Once system is tested and passes select Repair. All Done!";
        }
        protected override string GetSynopsys()
        {
            if (randomString == 0)
            {
                return "A solar panel section overloaded last night, we shut a small part of the panel down, but you need to reprogram the system to compensate for the new energy output.";
            }
            if (randomString == 1)
            {
                return "The four Main Bus Switching Units (MBSUs), control the routing of power from the the solar array wings to the rest of the Station " + vesselName + "." +
                " Yesterday MBSU-1, while still routing power correctly, ceased responding.  We need you to reset the system from the Repair Panel.";
            }
            if (randomString == 2)
            {
                return "An incorrect command sequence caused the Current altitude maintenance rocket control system to misfire during an altitude re-boost manoeuvre, fix the issue by using the Repair Panel Outside.";
            }
            if (randomString == 3)
            {
                return " A failure in cooling Loop A (starboard side), one of two external cooling loops failed, leaving the station with only half of its normal cooling capacity and zero redundancy in some systems. The repair panel can be used to adjust for this issue and get the system back online.";
            }
            else
            {
                return "A Fuel leak has been detected, we closed off the system form Mission Control but the system needs a reset.  Go to the Repair Panel and fix the issue.";
            }
        }
        protected override string MessageCompleted()
        {
            NoVessel = false;
            RepairPanel.repair = false;
            return "You have successfully repair the Space Station " + vesselName + " hopefully that will be the last of these issue's!";
        }

        protected override void OnLoad(ConfigNode node)
        {
            planetIDX = int.Parse(node.GetValue("planetIDX"));
            Debug.LogWarning("planetIDX loaded");
            vesselID = node.GetValue("VesselID");
            Debug.LogWarning("VesselID loaded");
            vesselName = node.GetValue("VesselName");
            Debug.LogWarning("VesselName loaded");
            titleName = node.GetValue("titlename");
            Debug.LogWarning("TitleName loaded");
            repairParts = node.GetValue("repairparts");
            Debug.LogWarning("RepairParts loaded");
            RPamount = double.Parse(node.GetValue("rpamount"));
            Debug.LogWarning("RpAmount loaded");
            Ctitle = node.GetValue("ctitle");
            Debug.LogWarning("CTitle loaded");
            randomString = int.Parse(node.GetValue("randomstring"));
            Debug.LogWarning("RandomString loaded");
            NoVessel = bool.Parse(node.GetValue("novessel"));
            Debug.LogWarning("NoVessel loaded");
            targetBody = FlightGlobals.Bodies[planetIDX];
            Debug.LogWarning("target body loaded as: " + targetBody.theName);
        }
        protected override void OnSave(ConfigNode node)
        {
            Debug.LogWarning("Starting Save Process for OnSave Repair Station Contracts");

            node.AddValue("planetIDX", planetIDX);
            Debug.LogWarning("PlanetIDX Saved");
            node.AddValue("VesselID", vesselID);
            Debug.LogWarning("targetbody saved");
            node.AddValue("VesselName", vesselName);
            Debug.LogWarning("VesselName saved");
            node.AddValue("titlename", titleName);
            Debug.LogWarning("TitleName saved");
            node.AddValue("repairparts", repairParts);
            Debug.LogWarning("RepairParts saved");
            node.AddValue("rpamount", RPamount);
            Debug.LogWarning("RpAmount saved");
            node.AddValue("ctitle", Ctitle);
            Debug.LogWarning("CTitle saved");
            node.AddValue("randomstring", randomString);
            Debug.LogWarning("RandomString saved");
            node.AddValue("novessel", NoVessel);
            Debug.LogWarning("NoVessel saved");
        }

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
            if (prestige != ContractPrestige.Trivial)
            {
                return false;
            }
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
            if (prestige != ContractPrestige.Trivial)
            {
                return false;
            }
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

            this.ctrans1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName),null);
            ctrans1.SetFunds(2000, 2000, targetBody);
            ctrans1.SetReputation(3, targetBody);
            this.ctrans2 = this.AddParameter(new TimeCountdownDocking(targetBody, crewTime, "Crew will Stay For This Amount Of Time ",vesselId), null);
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
        ContractParameter civ2;
        ContractParameter civ3;
        ContractParameter civ4;
        ContractParameter civ5;
        ContractParameter civ6;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CivilianLowOrbit>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CivilianLowOrbit>().Count();
            if (prestige != ContractPrestige.Significant || prestige != ContractPrestige.Exceptional)
            {
                return false;
            }

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
            
            this.civ4 = this.AddParameter(new EccentricGoal(targetBody,eccmin, eccmax), null);
            civ4.SetFunds(10000, 10000, targetBody);
            civ4.SetReputation(5, 10, targetBody);

            this.civ5 = this.AddParameter(new LandOnBody(targetBody), null);
            civ5.SetFunds(20000, 20000, targetBody);
            civ5.SetReputation(20, 40, targetBody);
                        
            this.civ6 = this.AddParameter(new TimeCountdownOrbits(targetBody, TripTime, TripText, true), null);
            civ6.SetFunds(50000, 50000, targetBody);
            civ6.SetReputation(15, 30, targetBody);

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
        ContractParameter civ2;
        ContractParameter civ3;
        ContractParameter civ4;
        ContractParameter civ5;
        ContractParameter civ6;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
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
            civ6.SetFunds(50000, 50000, targetBody);
            civ6.SetReputation(15, 30, targetBody);

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
        ContractParameter civ2;
        ContractParameter civ3;
        ContractParameter civ5;
        ContractParameter civ6;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
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

            this.civ6 = this.AddParameter(new TimeCountdownDocking(targetBody, TripTime, TripText, vesselID), null);
            civ6.SetFunds(50000, 50000, targetBody);
            civ6.SetReputation(15, 30, targetBody); 
                            
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
