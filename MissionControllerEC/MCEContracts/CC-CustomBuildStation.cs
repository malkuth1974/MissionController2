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
    #region Custom Build Space Station
    public class CustomBuildStation : Contract
    {
        CelestialBody targetBody = null;
        public string ContractPlayerName;
        public int EmptycrewAmount, totalContracts, TotalFinished;
        public string CTitle = Localizer.Format("Build Space Station");
        public bool StartSpaceBuild = false;
        ContractParameter Orbit1, crew1;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CustomBuildStation>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CustomBuildStation>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;

            if (totalContracts >= 1)
            {
                return false;
            }
            StartSpaceBuild = SaveInfo.BuildSpaceStationOn;
            if (!StartSpaceBuild)
            {
                return false;
            }

            targetBody = FlightGlobals.Bodies[SaveInfo.BuildSpaceStationIDX];
            if (targetBody == null)
            {
                Log.Error("Could not find TargetBody for Build Station contract!!");
                return false;
            }

            ContractPlayerName = SaveInfo.BuildSpaceStationName;
            EmptycrewAmount = SaveInfo.BuildSpaceStationCrewAmount;
            
                this.Orbit1 = this.AddParameter(new Contracts.Parameters.EnterOrbit(targetBody));
                Orbit1.SetFunds(2000 * EmptycrewAmount, 2000, targetBody);
                Orbit1.SetReputation(3 * EmptycrewAmount, targetBody);

            crew1 = this.AddParameter(new FinePrint.Contracts.Parameters.CrewCapacityParameter(EmptycrewAmount), null);
            crew1.SetFunds(10000 * EmptycrewAmount, targetBody);
            crew1.SetReputation(2* EmptycrewAmount, targetBody);           

            base.SetExpiry(15f, 40f);
            base.SetDeadlineYears(700, targetBody);
            base.SetReputation(25f, 50f, targetBody);
            base.SetFunds(25000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 120000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 120000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            return Localizer.Format("Build Space Station In Orbit Of " + targetBody.bodyDisplayName);
        }
        protected override string GetTitle()
        {
            return ContractPlayerName;
        }
        protected override string GetDescription()
        {
            return SaveInfo.BuildSpaceStationDesc;
        }
        protected override string GetNotes()
        {
            return Localizer.Format("Build Space Station In Orbit Of " + targetBody.bodyDisplayName + " That can support " + EmptycrewAmount + " crew amount!");
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("We have accepted your contract to build a space station that can support " + EmptycrewAmount + " In Orbit Of " + targetBody.bodyDisplayName);
        }
        protected override void OnOffered()
        {
            MessageSystem.Message m = new MessageSystem.Message("Contract Accepted", "Hello your contract *" + SaveInfo.BuildSpaceStationName + "* Has been accepted, Please check mission control for Information.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
            MessageSystem.Instance.AddMessage(m);
        }
        protected override string MessageCompleted()
        {     
            return Localizer.Format("We are impressed with the station you have built.  Good Job!");
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref ContractPlayerName, "Woops Defaults Loaded Error", ContractPlayerName, "contractplayername");
            Tools.ContractLoadCheck(node, ref EmptycrewAmount, 1, EmptycrewAmount, "crew");
            Tools.ContractLoadCheck(node, ref CTitle, "Defaults Loaded Error", CTitle, "ctitle");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            Debug.LogWarning("Custom Land Orbit Saved as " + bodyID);
            node.AddValue("targetBody", bodyID);
            node.AddValue("contractplayername", ContractPlayerName);
            node.AddValue("crew", EmptycrewAmount);
            node.AddValue("ctitle", CTitle);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("start") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }
    #endregion
}
