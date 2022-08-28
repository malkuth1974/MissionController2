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
    #region Build ComSat Network
    public class BuildComNetwork : Contract
    {
        CelestialBody targetBody = null;
        Settings settings = new Settings("Config.cfg");
        public double MinOrb, MaxOrb, contractAOP;
        public string ContractPlayerName;
        public string partName = "Repair Panel";
        public bool StartNetwork;
        public int totalContracts, TotalFinished, crewCount = 0, partAmount = 1;


        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<BuildComNetwork>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<BuildComNetwork>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;
            if (totalContracts >= 1)
            {

                return false;
            }
            settings.Load();
            StartNetwork = SaveInfo.ComSatContractOn;
            if (!StartNetwork)
            {
                return false;
            }
            targetBody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            ContractPlayerName = SaveInfo.ComSatContractName;
            MinOrb = SaveInfo.comSatminOrbital;
            double minorb2 = SaveInfo.comSatmaxOrbital - 1000;
            MaxOrb = SaveInfo.comSatmaxOrbital;

            this.AddParameter(new ApAOrbitGoal(targetBody, MaxOrb, "Equatorial"), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, minorb2, "Equatorail"), null);
            this.AddParameter(new Inclination(targetBody, MinOrb), null);

            if (parttechUnlock)
            {
                this.AddParameter(new PartGoal(partName, "Small Repair Panel", partAmount, true), null);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetExpiry(3f, 15f);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(25f, 40f, targetBody);
            base.SetFunds(12000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 75000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 75000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            return targetBody.bodyName + MaxOrb + MinOrb + " - Total Done: " + TotalFinished + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " " + targetBody.bodyName;
        }
        protected override string GetDescription()
        {

            return Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_ContractDescription_Set");		// #autoLOC_MissionController2_1000267 = This is a ComSat Network contract, you have control over when these missions are available and what their orbital periods are, you can set this up in the MCE Infomation icon tab located top right \n corner of the screen while in Space Center View.  Please note that settings will not take effect until at least 1 cycle of contracts has passed.  If you don't see your settings cancel out the offered contract! \n\n All ComSat Information is stored inside the Mission Controller Config File and will pass on to other save files
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000268");		// #autoLOC_MissionController2_1000268 = Vessel must be a new vessel launched after accepting this contract!
        }
        protected override string GetSynopsys()
        {
            return SaveInfo.SatelliteConDesc;
        }
        protected override void OnOffered()
        {
            MessageSystem.Message m = new MessageSystem.Message("Contract Accepted", "Hello your contract *" + SaveInfo.ComSatContractName + "* Has been accepted, Please check mission control for Information.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
            MessageSystem.Instance.AddMessage(m);
        }

        protected override void OnAccepted()
        {
            SaveInfo.ComSatContractOn = false;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.ComSatContractOn = false;
            return Localizer.Format("#autoLOC_MissionController2_1000270") + " " + targetBody.bodyName + " " + Localizer.Format("#autoLOC_MissionController2_1000271");		// #autoLOC_MissionController2_1000270 = You have delivered your ComSat to its assigned height around 		// #autoLOC_MissionController2_1000271 =  Continue to build you network.  When you're done you can turn off ComSat Contracts in the MCE Information Window.  Please note it will take a few contract cycles for them to disappear! 
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref MaxOrb, 71000, MaxOrb, "aPa");
            Tools.ContractLoadCheck(node, ref MinOrb, 70500, MinOrb, "pEa");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref partAmount, 1, partAmount, "partamount");
            Tools.ContractLoadCheck(node, ref partName, "Repair Panel", partName, "partname");
            Tools.ContractLoadCheck(node, ref ContractPlayerName, "Woops Defaults Loaded Error", ContractPlayerName, "contractplayername");
            Tools.ContractLoadCheck(node, ref contractAOP, 50, contractAOP, "contractAOP");
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
            node.AddValue("contractAOP", contractAOP);
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
