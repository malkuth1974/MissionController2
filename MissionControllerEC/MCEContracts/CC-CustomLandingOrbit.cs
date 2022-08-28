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
    #region Custom Landing/Orbit Contract
    public class CustomLandingOrbit : Contract
    {
        CelestialBody targetBody = null;
        public string ContractPlayerName;
        public int crewAmount, totalContracts, TotalFinished;
        public string CTitle = Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_Title_Set" + " ");
        public bool StartOrbitLand = false;
        ContractParameter Orbit1, Land2, crew1;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<CustomLandingOrbit>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<CustomLandingOrbit>().Count();
            bool parttechUnlock = ResearchAndDevelopment.GetTechnologyState("advConstruction") == RDTech.State.Available;

            if (totalContracts >= 1)
            {
                return false;
            }
            StartOrbitLand = SaveInfo.OrbitLandingOn;
            if (!StartOrbitLand)
            {
                return false;
            }

            targetBody = FlightGlobals.Bodies[SaveInfo.LandingOrbitIDX];
            if (targetBody == null)
            {
                Log.Error("Could not find TargetBody for Custom Landing Orbit contract!!");
                return false;
            }

            ContractPlayerName = SaveInfo.LandingOrbitName;
            crewAmount = SaveInfo.LandingOrbitCrew;
            if (SaveInfo.IsOrbitOrLanding)
            {
                this.Orbit1 = this.AddParameter(new Contracts.Parameters.EnterOrbit(targetBody));
                Orbit1.SetFunds(2000 * crewAmount, 2000, targetBody);
                Orbit1.SetReputation(3 * crewAmount, targetBody);
            }

            else
            {
                this.Land2 = this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody));
                Land2.SetFunds(1200 * crewAmount, targetBody);
                Land2.SetReputation(1 * crewAmount, targetBody);
            }
            crew1 = this.AddParameter(new GetCrewCount(crewAmount), null);
            crew1.SetFunds(1000 * crewAmount, targetBody);
            crew1.SetReputation(1 * crewAmount, targetBody);
            this.AddParameter(new Contracts.Parameters.KerbalDeaths(0));

            if (SaveInfo.OrbitAllowCivs == true)
            {
                SaveInfo.TourisNames.Clear();
                Tools.CivilianName();
                int test = 1;

                if (SaveInfo.IsOrbitOrLanding == true)
                {
                    foreach (string name in SaveInfo.TourisNames)
                    {
                        if (test <= SaveInfo.LandingOrbitCivilians)
                        {

                            FinePrint.Contracts.Parameters.KerbalTourParameter Kerbaltour = new FinePrint.Contracts.Parameters.KerbalTourParameter(name, ProtoCrewMember.Gender.Male);
                            this.AddParameter(Kerbaltour);
                            Kerbaltour.AddParameter(new FinePrint.Contracts.Parameters.KerbalDestinationParameter(targetBody, FlightLog.EntryType.Orbit, name));
                            Kerbaltour.SetFunds(Tools.FloatRandomNumber(7000, 17000), targetBody);
                            Kerbaltour.AddParameter(new LandOnBody(Planetarium.fetch.Home));
                            test++;
                        }
                    }
                }
                else
                {
                    foreach (string name in SaveInfo.TourisNames)
                    {
                        if (test <= SaveInfo.LandingOrbitCivilians)
                        {

                            FinePrint.Contracts.Parameters.KerbalTourParameter Kerbaltour = new FinePrint.Contracts.Parameters.KerbalTourParameter(name, ProtoCrewMember.Gender.Male);
                            this.AddParameter(Kerbaltour);
                            Kerbaltour.AddParameter(new FinePrint.Contracts.Parameters.KerbalDestinationParameter(targetBody, FlightLog.EntryType.Land, name));
                            Kerbaltour.SetFunds(Tools.FloatRandomNumber(8000, 18500), targetBody);
                            Kerbaltour.AddParameter(new LandOnBody(Planetarium.fetch.Home));
                            test++;
                        }
                    }
                }


            }

            base.SetExpiry(15f, 40f);
            base.SetDeadlineYears(700, targetBody);
            base.SetReputation(25f, 50f, targetBody);
            base.SetFunds(12000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 100000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 100000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            Tools.SpawnCivilianKerbMCE(SaveInfo.LandingOrbitCivilians);
        }
        protected override void OnCancelled()
        {
            Tools.CivilianGoHome();
        }

        protected override void OnFailed()
        {
            Tools.CivilianGoHome();
        }

        protected override void OnOfferExpired()
        {
            Tools.CivilianGoHome();
        }

        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000265") + " " + crewAmount + " " + Localizer.Format("#autoLOC_MissionController2_1000266") + targetBody.bodyName + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000265 = Land Or Orbit 		// #autoLOC_MissionController2_1000266 =  Over 
        }
        protected override string GetTitle()
        {
            return ContractPlayerName;
        }
        protected override string GetDescription()
        {
            return SaveInfo.LandingOrbitDesc;
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_GetNotes_Set");
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_ContractDescription_Set");
        }
        protected override void OnOffered()
        {
            MessageSystem.Message m = new MessageSystem.Message("Contract Accepted", "Hello your contract *" + SaveInfo.LandingOrbitName + "* Has been accepted, Please check mission control for Information.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
            MessageSystem.Instance.AddMessage(m);
        }
        protected override string MessageCompleted()
        {
            Tools.CivilianGoHome();
            SaveInfo.OrbitAllowCivs = false;
            SaveInfo.IsOrbitOrLanding = false;
            return Localizer.Format("#autoLOC_MCE2_Custom_Land_Orbit_Contract_ContractCompleted_Victory");
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref ContractPlayerName, "Woops Defaults Loaded Error", ContractPlayerName, "contractplayername");
            Tools.ContractLoadCheck(node, ref crewAmount, 1, crewAmount, "crew");
            Tools.ContractLoadCheck(node, ref CTitle, "Defaults Loaded Error", CTitle, "ctitle");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            Debug.LogWarning("Custom Land Orbit Saved as " + bodyID);
            node.AddValue("targetBody", bodyID);
            node.AddValue("contractplayername", ContractPlayerName);
            node.AddValue("crew", crewAmount);
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
