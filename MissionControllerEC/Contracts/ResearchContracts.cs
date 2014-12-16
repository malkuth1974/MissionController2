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
            orbitresearch1.SetScience(2, targetBody);
            this.orbitresearch2 = this.AddParameter(new OrbialResearchPartCheck(targetBody, missionTime), null);
            orbitresearch2.SetFunds(9000, targetBody);
            orbitresearch2.SetReputation(5, targetBody);
            orbitresearch2.SetScience(2, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(5f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(5f, 3f, targetBody);
            base.SetFunds(34000f * st.Contract_Payment_Multiplier, 53000f * st.Contract_Payment_Multiplier, 130000f * st.Contract_Payment_Multiplier, targetBody);

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
        protected override string GetNotes()
        {
            return "Vessel must be a new vessel launched after accepting this contract!";
        }
        protected override string GetSynopsys()
        {
            return "Scientific orbital research mission of  " + targetBody.theName + " with " + partName + ".";
        }
        protected override string MessageCompleted()
        {
            MCEOrbitalScanning.doOrbitResearch = false;
            return "You have reached the target body " + targetBody.theName + ", and conducted orbital research.  We have learned a lot of new information about the composition " +
                "of this planetary body in preparation for a possible landing in the future by our manned program or Robotic Legions.";
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody,Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref partName, "Orbital Research Scanner", partName, "partname");
            Tools.ContractLoadCheck(node, ref partNumber, 1, partNumber, "maxcount");
            Tools.ContractLoadCheck(node, ref missionTime, 10000, missionTime, "missiontime");

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
            this.landerscan2 = this.AddParameter(new LandingParameters(targetBody, true), null);
            landerscan2.SetFunds(8500, targetBody);
            landerscan2.SetReputation(5, targetBody);
            landerscan2.SetScience(3, targetBody);
            this.landerscan3 = this.AddParameter(new LanderResearchPartCheck(targetBody, amountTime), null);
            landerscan3.SetFunds(10000, targetBody);
            landerscan3.SetReputation(8, targetBody);
            landerscan3.SetScience(4, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(15f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(35f, 11f, targetBody);
            base.SetFunds(37000f * st.Contract_Payment_Multiplier, 66000f * st.Contract_Payment_Multiplier, 150000f * st.Contract_Payment_Multiplier, targetBody);

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
        protected override string GetNotes()
        {
            return "Vessel must be a new vessel launched after accepting this contract!";
        }
        protected override string GetSynopsys()
        {
            return "Land an unmanned vessel on " + targetBody.theName + " and conduct research for our company.";
        }
        protected override string MessageCompleted()
        {
            MCELanderResearch.doLanderResearch = false;
            return "You have successfully landed on and conducted researched on " + targetBody.theName + ".  After landing we have discovered many fascinating secrets about what makes up the composition of the landing site.\n\n" +

            "Further research missions, both manned and robotic, will be needed in the future to unlock the secrets of " + targetBody.theName + ".";
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref partName, "Orbital Research Scanner", partName, "partname");
            Tools.ContractLoadCheck(node, ref partNumber, 1, partNumber, "maxcount");
            Tools.ContractLoadCheck(node, ref amountTime, 10000, amountTime, "amountTime");
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
   
    public class TechList
    {
        public string techName = "";

        public TechList(string name)
        {
            this.techName = name;
        }
    }
}
