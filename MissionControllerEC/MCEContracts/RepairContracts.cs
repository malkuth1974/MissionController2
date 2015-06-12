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
using MissionControllerEC.PartModules;
namespace MissionControllerEC.MCEContracts
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
        ContractParameter repairgoal2;
        public string Ctitle = "To Repair Vessel You must have at Least ";
        public int randomString = 0;
        public string repairSynopsysText = "None Loaded";
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
                    vs.vesselName = vs.vesselName.Replace(originalName, originalName + "(Repair)");
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

        public void repairTextSelection()
        {
            switch (randomString)
            {
                case 0:
                    repairSynopsysText ="A section of the satellite has started to leak a small amount of fuel.  You are to EVA to the satellites repair panel and conduct repairs.";
                    break;
                case 1:
                    repairSynopsysText ="A small bus switch that controls the Satellites heating unit has malfunctioned.  EVA to the Repair Panel and try to bypass the switch with its backup.";
                    break;
                case 2:
                    repairSynopsysText ="After launch of the satellite, ground control operators noticed a software glitch in the satellites operating system.  EVA to the Repair Panel to update the software.";
                    break;
                case 3:
                    repairSynopsysText =" A small fragment dislodged one of the solar panel housing units.  The solar panel is considered lost and needs to be disengaged from the satellite systems.  EVA to the Repair Panel to start repairs.";
                    break;
                case 4:
                    repairSynopsysText ="The port side ABS unit has malfunctioned on this satellite.  EVA to the Repair Panel and conduct repairs to replace the defective component.";
                    break;
                case 5:
                    repairSynopsysText ="Ground control operators noticed that the pitch control of the satellite sometimes fires off for no reason.  Satellite TV users are getting upset that they are losing satellite reception.  EVA to the Repair Panel to correct the issue.";
                    break;
                case 6:
                    repairSynopsysText = "Ground control operators noticed that the pitch control of the satellite sometimes fires off for no reason.  Satellite TV users are getting upset that they are losing satellite reception.  EVA to the Repair Panel to correct the issue.";
                    break;
            }
        }

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
            randomString = Tools.RandomNumber(0, 7);
            findVeselWithRepairPart();
            chooseVesselRepairFromList();
            if (!NoVessel)
            {
                return false;
            }
            repairTextSelection();
            targetBody = FlightGlobals.Bodies[planetIDX];
            if (targetBody = null)
            {
                targetBody = Planetarium.fetch.Home;
            }

            if (maxApA <= 120000)
            {
                base.SetFunds(45000f * st.Contract_Payment_Multiplier, 71000f * st.Contract_Payment_Multiplier, 150000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
            }
            if (maxApA > 120000 && maxApA <= 1000000)
            {
                base.SetFunds(70000f * st.Contract_Payment_Multiplier, 95000f * st.Contract_Payment_Multiplier, 210000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
            }
            if (maxApA > 1000001)
            {
                base.SetFunds(120000f * st.Contract_Payment_Multiplier, 130000f * st.Contract_Payment_Multiplier, 300000f * st.Contract_Payment_Multiplier, targetBody);
                base.SetReputation(20f, 25f, targetBody);
            }

            this.repairgoal2 = this.AddParameter(new RepairPanelPartCheck(titleName, vesselID, vesselName), null);
            repairgoal2.SetFunds(8000, targetBody);
            repairgoal2.SetReputation(10, targetBody);                    
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
            return "Repair Vessel " + vesselName + " by docking with it, then repairing it" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Repair Contract for Satellite: " + " " + vesselName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "An issue has arisen with one of our satellites in orbit, we would like to contract your agency to launch and fix this issue.\n\n" +
                "How To conduct repairs:\n\n" +
                "1. You need An Engineer Kerbal to conduct the repairs.\n 2. Launch and intercept the target satellite (no need to dock unless you want to). \n" +
                "3. Conduct an EVA with an enginerr kerbal.\n" +
                "4. Go to the nearest Repair Panel and Open the Repair Panel and select Test System.\n6. Once system is tested and passes, select Repair.  All done!";
        }
        protected override string GetNotes()
        {
            return "How To conduct repairs:\n\n" +
                "1. You need an Engineer kerbal.\n 2. Launch and intercept the target satellite (no need to dock unless you want to). \n" +
                "3. Conduct an EVA with a Kerbal.\n" +
                "4. Go to the nearest Repair Panel and Open the Repair Panel and select Test System.\n6. Once system is tested and passes, select Repair.  All done!";
        }
        protected override string GetSynopsys()
        {
            return repairSynopsysText;
        }
        protected override string MessageCompleted()
        {
            NoVessel = false;
            RepairPanel.repair = false;
            return "You have successfully repaired our satellite " + vesselName + ".  Thank you very much!";
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref planetIDX, 1, planetIDX, "planetIDX");
            Tools.ContractLoadCheck(node, ref vesselID, "Default", vesselID, "VesselID");
            Tools.ContractLoadCheck(node, ref vesselName, "Vessel Name Not Loaded", vesselName, "VesselName");
            Tools.ContractLoadCheck(node, ref titleName, "Title Name Not Loaded", titleName, "titlename");
            Tools.ContractLoadCheck(node, ref maxApA, 300000, maxApA, "maxapa");            
            Tools.ContractLoadCheck(node, ref Ctitle, "Title Not Loaded", Ctitle, "ctitle");
            Tools.ContractLoadCheck(node, ref randomString, 1, randomString, "randomstring");
            Tools.ContractLoadCheck(node, ref NoVessel, false, NoVessel, "novessel");
            Tools.ContractLoadCheck(node, ref repairSynopsysText, "Synopsys Not Loaded", repairSynopsysText, "repairsynop");
            targetBody = FlightGlobals.Bodies[planetIDX];
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("planetIDX", planetIDX);
            node.AddValue("VesselID", vesselID);
            node.AddValue("VesselName", vesselName);
            node.AddValue("titlename", titleName);
            node.AddValue("maxapa", maxApA);            
            node.AddValue("ctitle", Ctitle);
            node.AddValue("novessel", NoVessel);
            node.AddValue("randomstring", randomString);
            node.AddValue("repairsynop", repairSynopsysText);
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
        public string stationRepSynopse = "None";
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

        public void repairStationSelect()
        {
            switch (randomString)
            {
                case 0:
                    stationRepSynopse ="A solar panel section overloaded last night.  We shut down a small area of the panel, but you need to reprogram the system to compensate for the new energy output.";
                    break;
                case 1:
                    stationRepSynopse ="The four Main Bus Switching Units (MBSUs), control the routing of power from the the solar array wings to the rest of Station " + vesselName + "." +
                    " Yesterday MBSU-1, while still routing power correctly, ceased responding.  We need you to reset the system from the Repair Panel.";
                    break;
                case 2:
                    stationRepSynopse ="An incorrect command sequence caused the current altitude maintenance rocket control system to misfire during an altitude re-boost maneuver.  Fix the issue by using the Repair Panel outside.";
                    break;
                case 3:
                    stationRepSynopse ="There is a failure in Cooling Loop A (starboard side).  One of two external cooling loops failed, leaving the station with only half of its normal cooling capacity and zero redundancy in some systems.  The repair panel can be used to adjust for this issue and get the system back online.";
                    break;
                case 4:
                    stationRepSynopse ="A replacement sequential shunt unit (SSU) for a 3A power system needs to be powered up so it can start to help the station's power situation.  Conduct an EVA, locate the Repair Panel, and perform the operations.";
                    break;
                case 5:
                    stationRepSynopse =" You need to Replace a failed Space-to-Ground Transmitter Receiver Controller and the Mobile Base Camera Light Pan-Tilt Assembly near the Repair panel.  Conduct an EVA, locate the Repair Panel, and perform the repairs.";
                    break;
                case 6:
                    stationRepSynopse =" fuel leak has been detected!  We have isolated the leak from Mission Control but the system needs recalibrated to return to normal operation.  Conduct an EVA, locate the Repair Panel, and fix the issue.";
                    break;
            }
        }

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
            randomString = Tools.RandomNumber(0, 7);
            targetBody = FlightGlobals.Bodies[planetIDX];
            if (targetBody = null)
            {
                targetBody = Planetarium.fetch.Home;
            }
            if (!NoVessel)
            {
                return false;
            }
            repairStationSelect();

            this.AddParameter(new EvaGoal(FlightGlobals.Bodies[planetIDX]), null);
            this.repairgoal2 = this.AddParameter(new RepairPanelPartCheck(titleName, vesselID, vesselName), null);
            repairgoal2.SetFunds(2000, targetBody);
            repairgoal2.SetReputation(10, targetBody);
            //this.AddParameter(new ResourceSupplyGoal(repairParts, RPamount, Ctitle), null);
            base.SetExpiry(1f, 3f);
            base.SetDeadlineYears(1f, targetBody);
            base.SetFunds(5000f * st.Contract_Payment_Multiplier, 30000f * st.Contract_Payment_Multiplier, 85000f * st.Contract_Payment_Multiplier, targetBody);
            base.SetReputation(75f, 125f, targetBody);

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
            return "EVA and repair Space Station " + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Repair Contract for Space Station: " + " " + vesselName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Space Station " + vesselName + " is experiencing issues and needs attention.  You are to conduct an EVA, locate the Repair Panel, and perform repairs to resolve the issues.\n\n" +
                "How To Conduct Repairs:\n\n" +
                "1. You need an Engineer kerbal to conduct repairs.\n2. Conduct an EVA." +
                "3. Go to the nearest Repair Panel and Open the Repair Panel and Select Test System.\n5. Once system is tested and passes, select Repair. All done!";
        }
        protected override string GetNotes()
        {
            return "How To Conduct Repairs:\n\n" +
                "1. You need an Engineer to conduct repairs.\n2. Conduct an EVA.\n" +
                "3. Go to the nearest Repair Panel and Open the Repair Panel and Select Test System.\n5. Once system is tested and passes, select Repair. All done!";
        }
        protected override string GetSynopsys()
        {
            return stationRepSynopse;
        }
        protected override string MessageCompleted()
        {
            NoVessel = false;
            RepairPanel.repair = false;
            return "You have successfully repaired the Space Station " + vesselName + ".  Hopefully that will be the last of these issues!";
        }

        protected override void OnLoad(ConfigNode node)
        {                     
            Tools.ContractLoadCheck(node, ref planetIDX, 1, planetIDX, "planetIDX");
            Tools.ContractLoadCheck(node, ref vesselID, "Default", vesselID, "VesselID");
            Tools.ContractLoadCheck(node, ref vesselName, "Woops Default Loaded", vesselName, "VesselName");
            Tools.ContractLoadCheck(node, ref titleName, "Woops Default Loaded", titleName, "titlename");            
            Tools.ContractLoadCheck(node, ref Ctitle, "Woops Default Loaded", Ctitle, "ctitle");
            Tools.ContractLoadCheck(node, ref randomString, 1,randomString,"randomstring");
            Tools.ContractLoadCheck(node, ref NoVessel, false, NoVessel, "novessel");
            Tools.ContractLoadCheck(node, ref stationRepSynopse, "Synopse Not loaded", stationRepSynopse, "stationsynop");
            targetBody = FlightGlobals.Bodies[planetIDX];
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("planetIDX", planetIDX);
            node.AddValue("VesselID", vesselID);
            node.AddValue("VesselName", vesselName);
            node.AddValue("titlename", titleName);           
            node.AddValue("ctitle", Ctitle);
            node.AddValue("randomstring", randomString);
            node.AddValue("novessel", NoVessel);
            node.AddValue("stationsynop", stationRepSynopse);
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
    

}
