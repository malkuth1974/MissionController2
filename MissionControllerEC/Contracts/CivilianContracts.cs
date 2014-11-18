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

            this.civ4 = this.AddParameter(new EccentricGoal(targetBody, eccmin, eccmax), null);
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node, ref civiliansAmount, 2, civiliansAmount, "civilians");
            Tools.ContractLoadCheck(node, ref name1, "George Error", name1, "name1");
            Tools.ContractLoadCheck(node, ref name2, "Sam Error", name2, "name2");
            Tools.ContractLoadCheck(node, ref name3, "Dave Error", name3, "name3");
            Tools.ContractLoadCheck(node, ref name4, "Fracking Error", name4, "name4");
            Tools.ContractLoadCheck(node, ref altitudeGoal, 71000, altitudeGoal, "altitude");
            Tools.ContractLoadCheck(node, ref eccmax, .5, eccmax, "eccmax");
            Tools.ContractLoadCheck(node, ref eccmin, 0, eccmin, "eccmin");
            Tools.ContractLoadCheck(node, ref TripTime, 10000, TripTime, "time");
            Tools.ContractLoadCheck(node, ref TripText, "Woops Loaded Default", TripText, "triptext");
            Tools.ContractLoadCheck(node, ref civdestination, "Loaded Error Draults Loaded", civdestination, "civd");

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
            civplanetnumber = Tools.RandomNumber(0, 100);
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
            return "Civilian Contract.  Bring Civilians on a landing Expedition of " + targetBody.theName;
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node, ref civiliansAmount, 2, civiliansAmount, "civilians");
            Tools.ContractLoadCheck(node, ref name1, "George Error", name1, "name1");
            Tools.ContractLoadCheck(node, ref name2, "Sam Error", name2, "name2");
            Tools.ContractLoadCheck(node, ref name3, "Dave Error", name3, "name3");
            Tools.ContractLoadCheck(node, ref name4, "Fracking Error", name4, "name4");            
            Tools.ContractLoadCheck(node, ref TripTime, 10000, TripTime, "time");
            Tools.ContractLoadCheck(node, ref TripText, "Woops Loaded Default", TripText, "triptext");
            Tools.ContractLoadCheck(node, ref civdestination, "Loaded Error Draults Loaded", civdestination, "civd");

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
            return "Civilian expedition to Space Station " + vesselName + " " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            return "The civilians thank you for bringing them home alive and showing them the wonders of your Space Station, they have learned a lot of iformation while staying at " + vesselName;
        }

        protected override void OnLoad(ConfigNode node)
        {          
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node, ref civiliansAmount, 2, civiliansAmount, "civilians");
            Tools.ContractLoadCheck(node, ref name1, "George Error", name1, "name1");
            Tools.ContractLoadCheck(node, ref name2, "Sam Error", name2, "name2");
            Tools.ContractLoadCheck(node, ref name3, "Dave Error", name3, "name3");
            Tools.ContractLoadCheck(node, ref name4, "Fracking Error", name4, "name4");           
            Tools.ContractLoadCheck(node, ref TripTime, 10000, TripTime, "time");
            Tools.ContractLoadCheck(node, ref TripText, "Woops Loaded Default", TripText, "triptext");
            Tools.ContractLoadCheck(node, ref civdestination, "Loaded Error Draults Loaded", civdestination, "civd");
            Tools.ContractLoadCheck(node, ref vesselID, "Defualt Loaded", vesselID, "vesselid");
            Tools.ContractLoadCheck(node, ref vesselName, "Error Defaults Loaded", vesselName, "vesselname");
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
}
