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
    #region Custom Crew Transfer Contract
    public class CustomCrewTransfer : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public string vesselName, vesselId, ContractPlayerName;
        public bool Startcrewtrans;
        public int crewAmount, totalContracts, TotalFinished;
        public double crewTime;
        public string CTitle = "Supply your Station Or Base with ";
        ContractParameter ctrans1, ctrans2, ctrans3;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CustomCrewTransfer>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CustomCrewTransfer>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;

            if (totalContracts >= 1)
            {
                return false;
            }
            Startcrewtrans = SaveInfo.crewContractOn;
            if (!Startcrewtrans)
            {
                return false;
            }

            targetBody = FlightGlobals.Bodies[SaveInfo.crewBodyIDX];
            if (targetBody == null)
            {
                targetBody = Planetarium.fetch.Home;
            }
            vesselName = SaveInfo.crewVesName;
            vesselId = SaveInfo.crewVesid;
            crewAmount = SaveInfo.crewAmount;
            crewTime = SaveInfo.crewTime;
            double timemultiplier = Tools.ConvertDays(crewTime);

            ContractPlayerName = SaveInfo.crewTransferName;

            this.ctrans1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            ctrans1.SetFunds(2000, 2000, targetBody);
            ctrans1.SetReputation(3, targetBody);
            this.ctrans2 = this.AddParameter(new TimeCountdownDocking(targetBody, crewTime, "Crew will Stay For This Amount Of Time ", vesselId, vesselName), null);
            ctrans2.SetFunds(Tools.RandomNumber(600, 900) * (float)timemultiplier, targetBody);
            ctrans2.SetReputation(3, targetBody);
            this.AddParameter(new GetCrewCount(crewAmount), null);

            if (SaveInfo.transferTouristTrue == true)
            {
                SaveInfo.TourisNames2.Clear();
                Tools.CivilianName2();
                int test = 1;

                foreach (string name in SaveInfo.TourisNames2)
                {

                    if (test <= SaveInfo.transferTouristAmount)
                    {
                        FinePrint.Contracts.Parameters.KerbalTourParameter Kerbaltour2 = new FinePrint.Contracts.Parameters.KerbalTourParameter(name, ProtoCrewMember.Gender.Female);
                        this.AddParameter(Kerbaltour2);
                        Kerbaltour2.SetFunds(Tools.RandomNumber(800, 1900) * (float)timemultiplier, targetBody);
                        Kerbaltour2.AddParameter(new TargetDockingGoal(vesselId, vesselName));
                        Kerbaltour2.AddParameter(new LandOnBody(Planetarium.fetch.Home));
                        test++;
                    }

                }

            }

            this.ctrans3 = this.AddParameter(new LandingParameters(Planetarium.fetch.Home, true), null);
            ctrans3.SetFunds(2000, 2000, Planetarium.fetch.Home);
            ctrans3.SetReputation(3, Planetarium.fetch.Home);
            base.SetExpiry(15f, 40f);
            base.SetDeadlineYears(700, targetBody);
            base.SetReputation(25f, 50f, targetBody);
            base.SetFunds(12000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 85000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 85000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            Tools.SpawnCivilianKerbMCE2(SaveInfo.transferTouristAmount);
        }
        protected override void OnCancelled()
        {
            Tools.CivilianGoHome2();
        }

        protected override void OnFailed()
        {
            Tools.CivilianGoHome2();
        }

        protected override void OnOfferExpired()
        {
            Tools.CivilianGoHome2();
        }
        protected override void OnCompleted()
        {
            Tools.CivilianGoHome2();
        }
        protected override string GetHashString()
        {
            return "Transfer " + crewAmount + " Crew To (" + vesselName + ") for " + Tools.formatTime(crewTime) + " Over " + targetBody.bodyName + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return ContractPlayerName + " (" + vesselName + ")";
        }
        protected override string GetDescription()
        {

            return Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_ContractDescription_Set ");
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000276");		// #autoLOC_MissionController2_1000276 = Vessel must be a new vessel launched after accepting this contract!
        }
        protected override string GetSynopsys()
        {
            return SaveInfo.TransferCrewDesc;
        }
        protected override void OnOffered()
        {
            MessageSystem.Message m = new MessageSystem.Message("Contract Accepted", "Hello your contract *" + SaveInfo.crewTransferName + "* Has been accepted, Please check mission control for Information.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
            MessageSystem.Instance.AddMessage(m);
        }
        protected override string MessageCompleted()
        {
            Tools.CivilianGoHome2();
            SaveInfo.crewContractOn = false;
            return Localizer.Format("#autoLOC_MissionController2_1000278") + " " + vesselName + " " + Localizer.Format("#autoLOC_MissionController2_1000279") + " " + Tools.formatTime(crewTime) + " " + Localizer.Format("#autoLOC_MissionController2_1000280") +		// #autoLOC_MissionController2_1000278 = You have Delivered your Crew to 		// #autoLOC_MissionController2_1000279 = And spent 		// #autoLOC_MissionController2_1000280 =  at your Station/Base\n\n
                Localizer.Format("#autoLOC_MissionController2_1000281");		// #autoLOC_MissionController2_1000281 =  If you're done, you can turn off Crew Contracts in the MCE Information Window.  Please note it will take a few contract cycles for them to disappear! 
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref vesselName, "defaults Loaded Error", vesselName, "vesselname");
            Tools.ContractLoadCheck(node, ref vesselId, "defaults", vesselId, "vesselid");
            Tools.ContractLoadCheck(node, ref ContractPlayerName, "Woops Defaults Loaded Error", ContractPlayerName, "contractplayername");
            Tools.ContractLoadCheck(node, ref crewAmount, 1, crewAmount, "crew");
            Tools.ContractLoadCheck(node, ref crewTime, 10000, crewTime, "time");
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
