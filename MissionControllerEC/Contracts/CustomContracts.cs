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

            this.AddParameter(new OrbitalPeriod(targetBody, MinOrb, MaxOrb), null);
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

            this.suppy1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
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
            return "Supply your base or station (" + vesselName + ") with supplies. Location is " + targetBody + " - Total Done: " + TotalFinished;
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

            this.ctrans1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            ctrans1.SetFunds(2000, 2000, targetBody);
            ctrans1.SetReputation(3, targetBody);
            this.ctrans2 = this.AddParameter(new TimeCountdownDocking(targetBody, crewTime, "Crew will Stay For This Amount Of Time ", vesselId), null);
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
            return "Transfer " + crewAmount + " Crew To (" + vesselName + ") for " + Tools.formatTime(crewTime) + " Over " + targetBody.theName;
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " (" + vesselName + ")";
        }
        protected override string GetDescription()
        {

            return "This is a custom Crew Transfer mission.  Use these contracts to supply your land bases and Orbital stations with crews and select the Time Spent in station or base. You must dock with the Vessel you selected to finish contract! " +
                "You can edit this contract by going to SpaceCenter screen and selecting Mission Controller Icon.  In the GUI choose the Custom Contract Button to start editing this contract. \n\n" +
                "All Crew Transfer contract information is stored in you Persistent Save File. The location of the Station or Base you will Transfer crew is " + targetBody.theName + "." + " Payments are adjusted for Travel Time To Body";
        }
        protected override string GetSynopsys()
        {
            return "Transfer crew to Station/Base over " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "You have Delivered your Crew to " + vesselName + "And spent " + Tools.formatTime(crewTime) + " at your Station/Base\n\n" +
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
