using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP.UI.Screens;
using KSP;
using System.Text;
using KSPAchievements;
using MissionControllerEC.MCEParameters;
using KSP.Localization;
using static MissionControllerEC.RegisterToolbar;

namespace MissionControllerEC.MCEContracts
{
    #region Custom Supply Contract
    public class CustomSupply : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public string vesselName, vesselId, ResourceName, ContractPlayerName;
        public bool StartSupply;
        public double resourcesAmount;
        public int totalContracts, TotalFinished;
        public string CTitle = Localizer.Format("#autoLOC_MCE_Supply_your_Station_Or_Base_with");
        ContractParameter suppy1, suppy2;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CustomSupply>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CustomSupply>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;

            if (totalContracts >= 1)
            {
                return false;
            }
            StartSupply = SaveInfo.supplyContractOn;
            if (!StartSupply)
            {
                return false;
            }
            targetBody = FlightGlobals.Bodies[SaveInfo.SupplyBodyIDX];
            if (targetBody == null)
            {
                targetBody = Planetarium.fetch.Home;
            }
            vesselName = SaveInfo.SupplyVesName;
            vesselId = SaveInfo.SupplyVesId;
            ResourceName = SaveInfo.ResourceName;
            resourcesAmount = SaveInfo.supplyAmount;

            ContractPlayerName = SaveInfo.SupplyContractName;

            this.suppy1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            suppy1.SetFunds(1000, 2000, targetBody);
            suppy1.SetReputation(5, 10, targetBody);
            this.suppy2 = this.AddParameter(new ResourceSupplyGoal(ResourceName, resourcesAmount, CTitle, true), null);
            suppy2.SetFunds(1000, 2000, targetBody);
            suppy2.SetReputation(5, 10, targetBody);
            base.SetExpiry(1f, 10f);
            base.SetDeadlineYears(.3f, targetBody);
            base.SetReputation(20f, 40f, targetBody);
            base.SetFunds(8000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 55000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 55000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            return Localizer.Format("#autoLOC_MissionController2_1000272") + " " + vesselName + " " + Localizer.Format("#autoLOC_MissionController2_1000273") + " " + targetBody + " " + Localizer.Format("#autoLOC_MissionController2_1000274") + TotalFinished + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000272 = Supply your base or station (		// #autoLOC_MissionController2_1000273 = ) with supplies. Location is 		// #autoLOC_MissionController2_1000274 =  - Total Done: 
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " (" + vesselName + ")";
        }
        protected override string GetDescription()
        {

            return Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_ContractDescription_Set") + " " + targetBody.name;
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MCE_Custom_Supply_Contract_GetNotes");
        }
        protected override string GetSynopsys()
        {
            return SaveInfo.ResourceTransferConDesc;
        }
        protected override void OnOffered()
        {
            MessageSystem.Message m = new MessageSystem.Message("Contract Accepted", "Hello your contract *" + SaveInfo.SupplyContractName + "* Has been accepted, Please check mission control for Information.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
            MessageSystem.Instance.AddMessage(m);
        }
        protected override string MessageCompleted()
        {
            SaveInfo.supplyContractOn = false;
            return Localizer.Format("#autoLOC_MCE_Custom_Supply_Contract_FinishedContract");
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref vesselName, "defaults Loaded Error", vesselName, "vesselname");
            Tools.ContractLoadCheck(node, ref vesselId, "defaults", vesselId, "vesselid");
            Tools.ContractLoadCheck(node, ref ContractPlayerName, "Woops Defaults Loaded Error", ContractPlayerName, "contractplayername");
            Tools.ContractLoadCheck(node, ref ResourceName, "Woops Defaults Loaded Error", ResourceName, "supplies");
            Tools.ContractLoadCheck(node, ref resourcesAmount, 1, resourcesAmount, "resourceamount");
            Tools.ContractLoadCheck(node, ref CTitle, "Defaults Loaded Error", CTitle, "ctitle");
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
