using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP.UI.Screens;
using MissionControllerEC.MCEParameters;
using System;
using KSP.Localization;

namespace MissionControllerEC.MCEContracts
{
    # region Vostok 1
    public class Vostok : Contract
    {
        Settings settings = new Settings("config.cfg");
        public float minHeight = 75000;
        public int totalContracts;
        public int TotalFinished;
        public int crew = 1;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Vostok>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Vostok>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            minHeight= Convert.ToSingle(targetBody.atmosphereDepth);         
            vostok1 = this.AddParameter(new Contracts.Parameters.ReachAltitudeEnvelope(minHeight, minHeight + HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEErrorOrbits), null);
            vostok1.SetFunds(1000f, targetBody);
            vostok1.SetReputation(2f, targetBody);
            vostok2 = this.AddParameter(new InOrbitGoal(targetBody), null);
            vostok2.SetFunds(2000f, targetBody);
            vostok2.SetReputation(3f, targetBody);
            vostok3 = this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody), null);
            vostok3.SetFunds(2500f, targetBody);
            vostok3.SetReputation(4f, targetBody);
            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(5000f, 45000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(5f, targetBody);
            base.SetScience(1.25f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000282") + " " + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000282 = Launch of Vostok 1 into space
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000283") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000283 = Launch of Vostok 1 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000284");		// #autoLOC_MissionController2_1000284 = Vostok 1 was the first spaceflight of the Vostok program and the first human spaceflight in history.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000285") + " " + targetBody.bodyName + ". " + " Orbit at least once!";		// #autoLOC_MissionController2_1000285 = Bring Vostok 1 to orbit around 
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Vostok1Done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000286") +		// #autoLOC_MissionController2_1000286 = Four decades after the flight, historian Asif Siddiqi wrote that Vostok 1 will undoubtedly remain one of the major milestones in not only the history of space exploration, 
                Localizer.Format("#autoLOC_MissionController2_1000287") +		// #autoLOC_MissionController2_1000287 = but also the history of the human race itself. The fact that this accomplishment was successfully carried out by the Soviet Union, a country completely devastated by war just 
                Localizer.Format("#autoLOC_MissionController2_1000288") +		// #autoLOC_MissionController2_1000288 = sixteen years prior, makes the achievement even more impressive. Unlike the United States, the USSR had to begin from a position of tremendous disadvantage. Its industrial 
                Localizer.Format("#autoLOC_MissionController2_1000289") +		// #autoLOC_MissionController2_1000289 = infrastructure had been ruined, and its technological capabilities were outdated at best. A good portion of its land had been devastated by war, and it had lost about 25 million 
                Localizer.Format("#autoLOC_MissionController2_1000290") +		// #autoLOC_MissionController2_1000290 = citizens... but it was the totalitarian state that overwhelmingly took the lead.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000291") +		// #autoLOC_MissionController2_1000291 = Commemorative monument, Vostok-1 landing site near Engels, Russia.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000292") +		// #autoLOC_MissionController2_1000292 = The landing site is now a monument park. The central feature in the park is a 25 meter tall monument that consists of a silver metallic rocket rising on a curved metallic 
                Localizer.Format("#autoLOC_MissionController2_1000293") +		// #autoLOC_MissionController2_1000293 = column of flame, from a wedge shaped, white stone base. In front of this is a 3 meter tall, white stone statue of Yuri Gagarin, wearing a spacesuit, with one arm raised in greeting 
                Localizer.Format("#autoLOC_MissionController2_1000294") +		// #autoLOC_MissionController2_1000294 = and the other holding a space helmet.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000295") +		// #autoLOC_MissionController2_1000295 = The Vostok 1 re-entry capsule is now on display at the RKK Energiya museum in Korolyov, near Moscow.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000296") +		// #autoLOC_MissionController2_1000296 = In 2011, documentary film maker Christopher Riley partnered with European Space Agency astronaut Paolo Nespoli to record a new film of what Gagarin would have seen of the 
                Localizer.Format("#autoLOC_MissionController2_1000297") +		// #autoLOC_MissionController2_1000297 = Earth from his spaceship, by matching historical audio recordings to video from the International Space Station following the ground path taken by Vostok 1. The resulting film,
                Localizer.Format("#autoLOC_MissionController2_1000298") +		// #autoLOC_MissionController2_1000298 = First Orbit, was released online to celebrate the 50th anniversary of human spaceflight.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000299");		// #autoLOC_MissionController2_1000299 = Information of Vostok 1 was gathered from Wikipedia
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            minHeight = float.Parse(node.GetValue("minheight"));
            crew = int.Parse(node.GetValue("crewcount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("minheight", minHeight);
            node.AddValue("crewcount", crew);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.Vostok1Done == true) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Vostok 2
    public class Vostok2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public float minHeight = 70000;
        public double missionTime = 21600;
        public int totalContracts;
        public int TotalFinished;
        public int crew = 1;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;
        ContractParameter vostok4;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Vostok2>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Vostok2>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }

            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            minHeight = Convert.ToSingle(targetBody.atmosphereDepth);
            vostok1 = this.AddParameter(new Contracts.Parameters.ReachAltitudeEnvelope(minHeight, minHeight + HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEErrorOrbits), null);
            vostok1.SetFunds(1000f, targetBody);
            vostok1.SetReputation(2f, targetBody);
            vostok2 = this.AddParameter(new InOrbitGoal(targetBody), null);
            vostok2.SetFunds(2000f, targetBody);
            vostok2.SetReputation(3f, targetBody);
            vostok4 = this.AddParameter(new TimeCountdownOrbits(targetBody, missionTime, true), null);
            vostok4.SetFunds(3000f, targetBody);
            vostok4.SetReputation(6f, targetBody);
            vostok4.SetScience(10f, targetBody);
            vostok3 = this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody), null);
            vostok3.SetFunds(3500f, targetBody);
            vostok3.SetReputation(7f, targetBody);
            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(8000f, 51000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(20f, targetBody);
            base.SetScience(15f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of Vostok 2 into space" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000300") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000300 = Launch of Vostok 2 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000301") +		// #autoLOC_MissionController2_1000301 = Vostok 2 was a Soviet space mission which carried cosmonaut Gherman Titov into orbit for a full day on August 6, 1961 
                Localizer.Format("#autoLOC_MissionController2_1000302") +		// #autoLOC_MissionController2_1000302 = to study the effects of a more prolonged period of weightlessness on the human body.  Titov orbited the Earth over 17 times, exceeding the single orbit of Yuri Gagarin on Vostok 1 
                Localizer.Format("#autoLOC_MissionController2_1000303") +		// #autoLOC_MissionController2_1000303 = − as well as the suborbital spaceflights of American astronauts Alan Shepard and Gus Grissom aboard their respective Mercury-Redstone 3 and 4 missions. Indeed, Titov's number of orbits 
                Localizer.Format("#autoLOC_MissionController2_1000304") +		// #autoLOC_MissionController2_1000304 = and flight time would not be surpassed by an American astronaut until Gordon Cooper's Mercury-Atlas 9 spaceflight in May 1963.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000305") +		// #autoLOC_MissionController2_1000305 = Information on Vostok 2 was gathered from Wikipedia\n\n

                Localizer.Format("#autoLOC_MissionController2_1000306");		// #autoLOC_MissionController2_1000306 = Objectives are to: \n\n1. Enter Space \n\n2. Stay in orbit for a total of 1 day (6 Kerbal Hours) \n3. Return Home.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000307") + " " + targetBody.bodyName + ". " + Localizer.Format("#autoLOC_MissionController2_1000308");		// #autoLOC_MissionController2_1000307 = Bring Vostok 2 to orbit around 		// #autoLOC_MissionController2_1000308 =  stay in orbit for 1 day.
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Vostok2Done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000309") +		// #autoLOC_MissionController2_1000309 = The flight was an almost complete success, marred only by a heater that had inadvertently been turned off prior to liftoff and that allowed the inside temperature 
                Localizer.Format("#autoLOC_MissionController2_1000310") +		// #autoLOC_MissionController2_1000310 = to drop to 50 °F (10 °C), a bout of space sickness, and a troublesome re-entry when the reentry module failed to separate cleanly from its service module. \n\n
            Localizer.Format("#autoLOC_MissionController2_1000311") +		// #autoLOC_MissionController2_1000311 = Unlike Yuri Gagarin on Vostok 1, Titov took manual control of the spacecraft for a short while. Another change came when the Soviets admitted that Titov did not land with 
            Localizer.Format("#autoLOC_MissionController2_1000312") +		// #autoLOC_MissionController2_1000312 = his spacecraft. Titov would claim in an interview that he ejected from his capsule as a test of an alternative landing system; it is now known that all Vostok program landings were performed this way.\n\n
            Localizer.Format("#autoLOC_MissionController2_1000313") +		// #autoLOC_MissionController2_1000313 = The re-entry capsule was destroyed during development of the Voskhod spacecraft.\n\n
            Localizer.Format("#autoLOC_MissionController2_1000314");		// #autoLOC_MissionController2_1000314 = As of 2013, Titov remains the youngest person to reach space. He was a month short of 26 years old at launch.
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            minHeight = float.Parse(node.GetValue("minheight"));
            crew = int.Parse(node.GetValue("crewcount"));
            missionTime = double.Parse(node.GetValue("missionTime"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("minheight", minHeight);
            node.AddValue("crewcount", crew);
            node.AddValue("missionTime", missionTime);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.Vostok2Done == true) { return false; }
            if (SaveInfo.Vostok1Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Voskhod 2
    public class Voskhod2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public float minHeight = 82000;
        public int totalContracts;
        public int TotalFinished;
        public int crew = 2;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;
        ContractParameter vostok4;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Voskhod2>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Voskhod2>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }         
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            minHeight = Convert.ToSingle(targetBody.atmosphereDepth);
            vostok1 = this.AddParameter(new Contracts.Parameters.ReachAltitudeEnvelope(minHeight, minHeight + HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEErrorOrbits), null);
            vostok1.SetFunds(2000f, targetBody);
            vostok1.SetReputation(2f, targetBody);
            vostok2 = this.AddParameter(new InOrbitGoal(targetBody), null);
            vostok2.SetFunds(3000f, targetBody);
            vostok2.SetReputation(3f, targetBody);
            vostok4 = this.AddParameter(new EvaGoal(targetBody), null);
            vostok4.SetFunds(4200f, targetBody);
            vostok4.SetReputation(6f, targetBody);
            vostok4.SetScience(6f, targetBody);
            vostok3 = this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody), null);
            vostok3.SetFunds(4800f, targetBody);
            vostok3.SetReputation(7f, targetBody);
            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(11000f, 63000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(27f, targetBody);
            base.SetScience(11f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of Voskhod 2 (2 Kerbals) into space" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000315") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000315 = Launch of Voskhod 2 (2 Kerbals) 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000316") +		// #autoLOC_MissionController2_1000316 = Voskhod 2 was a Soviet manned space mission in March 1965. Vostok-based Voskhod 3KD spacecraft with two crew members on board, Pavel Belyayev and Alexey Leonov,
                Localizer.Format("#autoLOC_MissionController2_1000317") +		// #autoLOC_MissionController2_1000317 = was equipped with an inflatable airlock. It established another milestone in space exploration when Alexey Leonov became the first person to leave the spacecraft in a specialized 
                Localizer.Format("#autoLOC_MissionController2_1000318") +		// #autoLOC_MissionController2_1000318 = spacesuit to conduct a 12 minute spacewalk.

                Localizer.Format("#autoLOC_MissionController2_1000319") +		// #autoLOC_MissionController2_1000319 = Information on Voskhod 2 was gathered from Wikipedia\n\n

                Localizer.Format("#autoLOC_MissionController2_1000320");		// #autoLOC_MissionController2_1000320 = Objectives are too \n\n1. Enter Space \n\n2. Bring vessel to Orbit And Specified height (2 Kerbals Total) \n3. EVA 1 Kerbal \n 4. Return to Kerbin safely.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000321") + " " + targetBody.bodyName + ". " + Localizer.Format("#autoLOC_MissionController2_1000322");		// #autoLOC_MissionController2_1000321 = Bring Voskhod 2 (2 Kerbals) to orbit around 		// #autoLOC_MissionController2_1000322 =  stay in orbit for 1 day.
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Voskhod2Done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000323") +		// #autoLOC_MissionController2_1000323 = A delay of 46 seconds caused the spacecraft to land 386 km from the intended landing zone, in the inhospitable forests of Upper Kama Upland, somewhere west of Solikamsk. 
            Localizer.Format("#autoLOC_MissionController2_1000327") +		// #autoLOC_MissionController2_1000327 = Although mission control had no idea where the spacecraft had landed or whether Leonov and Belyayev had survived, their families were told that they were resting after having been recovered. 
            Localizer.Format("#autoLOC_MissionController2_1000328") +		// #autoLOC_MissionController2_1000328 = The two men were both familiar with the harsh climate and knew that bears and wolves, made aggressive by mating season, lived in the taiga; the spacecraft carried a pistol and plenty of ammunition. 
            Localizer.Format("#autoLOC_MissionController2_1000329") +		// #autoLOC_MissionController2_1000329 = Although aircraft quickly located the cosmonauts, the area was so heavily forested that helicopters could not land. Night arrived, the temperature fell to below −30°C, and the spacecraft's hatch 
            Localizer.Format("#autoLOC_MissionController2_1000330") +		// #autoLOC_MissionController2_1000330 = had been blown open by explosive bolts. Leonov and Belyayev had to strip naked, wring out the sweat from their underwear, and re-don it and the inner linings of their spacesuits to stay warm. 
            Localizer.Format("#autoLOC_MissionController2_1000331") +		// #autoLOC_MissionController2_1000331 = A rescue party arrived on skis the next day with food and hot water, and chopped wood for a fire and a log cabin. After a more comfortable second night in the forest, the cosmonauts skied to a 
            Localizer.Format("#autoLOC_MissionController2_1000332") +		// #autoLOC_MissionController2_1000332 = waiting helicopter several kilometers away and flew to Perm, then Baikonur.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000333");		// #autoLOC_MissionController2_1000333 = Information on Voskhod 2 was gathered from Wikipedia
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            minHeight = float.Parse(node.GetValue("minheight"));
            crew = int.Parse(node.GetValue("crewcount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("minheight", minHeight);
            node.AddValue("crewcount", crew);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.Voskhod2Done == true) { return false; }
            if (SaveInfo.Vostok2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    public class Tiros : Contract
    {
        Settings settings = new Settings("config.cfg");
        CelestialBody targetBody = Planetarium.fetch.Home;
        private double minHeight = 0;
        private double inclination = 0;
        private double Eccentricity = 0;
        private double AmountDaysActive = 0;
        private int TirosTitleMissionNumber;
        private float contractMult = 1.0f;
        public int totalContracts;
        public int TotalFinished;
        private string tirosNotes = "none";
        private string tirosHash = "none";
        private string tirostitle = "none";
        private string tirosDescription = "none";
        private string tirosSynops = "none";
        private string tirosCompleteMessage = "none";

        public void tirosTextSelect()
        {
            switch (TirosTitleMissionNumber)
            {
                case 1:
                    tirosNotes = Localizer.Format("#autoLOC_MissionController2_1000334");		// #autoLOC_MissionController2_1000334 = Launch the first weather satellite to a Low Kerbin Orbit and observe weather patterns Use Commotron 16 as longAntenna
                    tirosHash ="Tiros 1";
                    tirostitle = Localizer.Format("#autoLOC_MissionController2_1000335");		// #autoLOC_MissionController2_1000335 = Tiros 1 (Television Infrared Observation Satellite)
                    tirosDescription = Localizer.Format("#autoLOC_MissionController2_1000336") +		// #autoLOC_MissionController2_1000336 = The TIROS Program (Television Infrared Observation Satellite) was NASA's first experimental step to determine if satellites could be useful in the study of the Earth.
                    Localizer.Format("#autoLOC_MissionController2_1000337") +		// #autoLOC_MissionController2_1000337 = At that time, the effectiveness of satellite observations was still unproven. Since satellites were a new technology, the TIROS Program also tested various design issues for 
                    Localizer.Format("#autoLOC_MissionController2_1000338") +		// #autoLOC_MissionController2_1000338 = spacecraft: instruments, data and operational parameters. The goal was to improve satellite applications for Earth-bound decisions, such as should we evacuate the coast because of the hurricane?\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000339") +		// #autoLOC_MissionController2_1000339 = The TIROS Program's first priority was the development of a meteorological satellite information system. Weather forecasting was deemed the most promising application of space-based observations.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000340") +		// #autoLOC_MissionController2_1000340 = TIROS proved extremely successful, providing the first accurate weather forecasts based on data gathered from space. TIROS began continuous coverage of the Earth's weather in 1962, and was used 
                    Localizer.Format("#autoLOC_MissionController2_1000341") +		// #autoLOC_MissionController2_1000341 = by meteorologists worldwide. The program's success with many instrument types and orbital configurations lead to the development of more sophisticated meteorological observation satellites.\n\n 
                    Localizer.Format("#autoLOC_MissionController2_1000342");		// #autoLOC_MissionController2_1000342 = All information for the Tiros Missions were gathered from NASA.com
                    tirosSynops = Localizer.Format("#autoLOC_MissionController2_1000343") +		// #autoLOC_MissionController2_1000343 = Objectives: To test experimental television techniques designed to develop a worldwide meteorological satellite information system. To test Sun angle and horizon sensor systems for spacecraft 
                    Localizer.Format("#autoLOC_MissionController2_1000344") +		// #autoLOC_MissionController2_1000344 = orientation.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000345") +		// #autoLOC_MissionController2_1000345 = Description: The spacecraft was 42 inches in diameter, 19 inches high and weighed 270 pounds. The craft was made of aluminum alloy and stainless steel which was then covered by 9200 solar cells.
                    Localizer.Format("#autoLOC_MissionController2_1000346");		// #autoLOC_MissionController2_1000346 = The solar cells served to charge the on-board batteries. Three pairs of solid-propellant spin rockets were mounted on the base plate.
                    tirosCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000347");                  		// #autoLOC_MissionController2_1000347 = Good Job you have finsished this contract\n\n
                    break;
                case 2:
                    tirosNotes = Localizer.Format("#autoLOC_MissionController2_1000348");		// #autoLOC_MissionController2_1000348 = Launch Tiros/NOOA with batteries and solar panels to a Kebin Polar Orbit Use Commotron 16 as longAntenna
                    tirosHash = Localizer.Format("#autoLOC_MissionController2_1000349");		// #autoLOC_MissionController2_1000349 = Tiros 7
                    tirostitle = Localizer.Format("#autoLOC_MissionController2_1000350");		// #autoLOC_MissionController2_1000350 = Tiros 7 (Television Infrared Observation Satellite)
                    tirosDescription = Localizer.Format("#autoLOC_MissionController2_1000351") +		// #autoLOC_MissionController2_1000351 = The TIROS Program (Television Infrared Observation Satellite) was NASA's first experimental step to determine if satellites could be useful in the study of the Earth.
                    Localizer.Format("#autoLOC_MissionController2_1000352") +		// #autoLOC_MissionController2_1000352 = At that time, the effectiveness of satellite observations was still unproven. Since satellites were a new technology, the TIROS Program also tested various design issues for 
                    Localizer.Format("#autoLOC_MissionController2_1000353") +		// #autoLOC_MissionController2_1000353 = spacecraft: instruments, data and operational parameters. The goal was to improve satellite applications for Earth-bound decisions, such as should we evacuate the coast because of the hurricane?\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000354") +		// #autoLOC_MissionController2_1000354 = The TIROS Program's first priority was the development of a meteorological satellite information system. Weather forecasting was deemed the most promising application of space-based observations.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000355") +		// #autoLOC_MissionController2_1000355 = TIROS proved extremely successful, providing the first accurate weather forecasts based on data gathered from space. TIROS began continuous coverage of the Earth's weather in 1962, and was used 
                    Localizer.Format("#autoLOC_MissionController2_1000356") +		// #autoLOC_MissionController2_1000356 = by meteorologists worldwide. The program's success with many instrument types and orbital configurations lead to the development of more sophisticated meteorological observation satellites.\n\n 
                    Localizer.Format("#autoLOC_MissionController2_1000357");		// #autoLOC_MissionController2_1000357 = All information for the Tiros Missions were gathered from NASA.com
                    tirosSynops = Localizer.Format("#autoLOC_MissionController2_1000358") +		// #autoLOC_MissionController2_1000358 = Objectives: Continue research and development of the meteorological satellite information system; obtain improved data for use in weather forecasting, especially during hurricane season.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000359") +		// #autoLOC_MissionController2_1000359 = Description: The spacecraft was 42 inches in diameter, 19 inches high and weighed 270 pounds. The craft was made of aluminum alloy and stainless steel then covered by 9200 solar cells. The solar
                    Localizer.Format("#autoLOC_MissionController2_1000360") +		// #autoLOC_MissionController2_1000360 = cells served to charge the nickel-cadmium (nicad) batteries. Three pairs of solid-propellant spin rockets were mounted on the base plate.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000361") +		// #autoLOC_MissionController2_1000361 = TIROS-7 was also designed to make infrared measurements of reflected solar and terrestrial radiation over selected spectrum ranges and gather data on electron density and temperature in space.
                    Localizer.Format("#autoLOC_MissionController2_1000362") +		// #autoLOC_MissionController2_1000362 = To accomplish this new expanded mission, TIROS-7 carried two wide-angle camera systems, a magnetic tape recorder, and infrared experimentation equipment. The electron density and temperature probes
                    Localizer.Format("#autoLOC_MissionController2_1000363") +		// #autoLOC_MissionController2_1000363 = were the same as the ones flown on board Explorer 17.
                    Localizer.Format("#autoLOC_MissionController2_1000364") +		// #autoLOC_MissionController2_1000364 = The spacecraft operating system still included the infrared horizon scanner, the north direction indicator, despin weights and spinup rockets, and the magnetic attitude control system. TIROS-7 was
                    Localizer.Format("#autoLOC_MissionController2_1000365");		// #autoLOC_MissionController2_1000365 = deactivated after furnishing over 30,000 cloud photographs; it lasted the longest of the TIROS series thus far, 1809 days.
                    tirosCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000366") +		// #autoLOC_MissionController2_1000366 = Good Job you have finsished this contract\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000367") +		// #autoLOC_MissionController2_1000367 = TIROS-7 STATS:\n
                    Localizer.Format("#autoLOC_MissionController2_1000368") +		// #autoLOC_MissionController2_1000368 = Launch Date:  June 19, 1963\n
                    Localizer.Format("#autoLOC_MissionController2_1000369") +		// #autoLOC_MissionController2_1000369 = Operational Period: 1809 days before being deactivated by NASA on June 3, 1968\n
                    Localizer.Format("#autoLOC_MissionController2_1000370") +		// #autoLOC_MissionController2_1000370 = Launch Vehicle:    Three-Stage Delta\n
                    Localizer.Format("#autoLOC_MissionController2_1000371") +		// #autoLOC_MissionController2_1000371 = Launch Site:    Cape Canaveral, FL\n
                    Localizer.Format("#autoLOC_MissionController2_1000372");		// #autoLOC_MissionController2_1000372 = Type:   Weather Satellite
                    break;
                case 3:
                    tirosNotes = Localizer.Format("#autoLOC_MissionController2_1000373");		// #autoLOC_MissionController2_1000373 = Launch Tiros with new technology batteries and solar panels to keep satellite in orbit longer
                    tirosHash = Localizer.Format("#autoLOC_MissionController2_1000374");		// #autoLOC_MissionController2_1000374 = Tiros/NOOA - N
                    tirostitle = Localizer.Format("#autoLOC_MissionController2_1000375");		// #autoLOC_MissionController2_1000375 = Tiros/NOAA - N
                    tirosDescription = Localizer.Format("#autoLOC_MissionController2_1000376") +		// #autoLOC_MissionController2_1000376 = The TIROS-N/NOAA Program (Television InfraRed Operational Satellite - Next-generation) was NASA's next step in improving the operational capability of the TIROS system first tried in 
                    Localizer.Format("#autoLOC_MissionController2_1000377") +		// #autoLOC_MissionController2_1000377 = the 1960's and the ITOS/NOAA system of the 1970's. Technological improvements integrated into the satellite system provided higher resolution imaging, and more day and night quantitative 
                    Localizer.Format("#autoLOC_MissionController2_1000378") +		// #autoLOC_MissionController2_1000378 = environmental data on local and global scales than seen with the two earlier generations of TIROS. Like earlier TIROS systems, NASA took responsibility for the satellite only until proven 
                    Localizer.Format("#autoLOC_MissionController2_1000379") +		// #autoLOC_MissionController2_1000379 = operational. Once operational the satellite's name was changed to 'NOAA' with day to day use under the direction of the National Oceanic and Atmospheric Administration.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000380") +		// #autoLOC_MissionController2_1000380 = The TIROS-N/NOAA satellite series carried the Advanced Very High Resolution Radiometer (AVHRR). The AVHRR provided day and night cloud-top and sea surface temperatures, as well as ice and snow 
                    Localizer.Format("#autoLOC_MissionController2_1000381") +		// #autoLOC_MissionController2_1000381 = conditions. The satellite also carried an atmospheric sounding system (TOVS - TIROS Operational Vertical Sounder) which provided vertical profiles of temperature and water vapor from the Earth's 
                    Localizer.Format("#autoLOC_MissionController2_1000382") +		// #autoLOC_MissionController2_1000382 = surface to the top of the atmosphere; and a solar proton monitor to detect the arrival of energetic particles for use in solar storm prediction. For the first time, this satellite carried a data 
                    Localizer.Format("#autoLOC_MissionController2_1000383");		// #autoLOC_MissionController2_1000383 = collection platform used to receive, process and store information from free floating balloons and buoys worldwide for transmission to one central processing facility.
                    tirosSynops = Localizer.Format("#autoLOC_MissionController2_1000384") +		// #autoLOC_MissionController2_1000384 = TIROS-N was an experimental satellite which carried an Advanced Very High Resolution Radiometer (AVHRR) to provide day and night cloud top and sea surface temperatures, as well as ice and 
                    Localizer.Format("#autoLOC_MissionController2_1000385") +		// #autoLOC_MissionController2_1000385 = snow conditions; an atmospheric sounding system (TOVS - TIROS Operational Vertical Sounder) to provide vertical profiles of temperature and water vapor from the Earth's surface to the top 
                    Localizer.Format("#autoLOC_MissionController2_1000386") +		// #autoLOC_MissionController2_1000386 = of the atmosphere; and a solar proton monitor to detect the arrival of energetic particles for use in solar storm prediction. For the first time, this satellite also carried a data collection 
                    Localizer.Format("#autoLOC_MissionController2_1000387") +		// #autoLOC_MissionController2_1000387 = platform used to receive, process and store information from free floating balloons and buoys worldwide for transmission to one central processing facility.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000388") +		// #autoLOC_MissionController2_1000388 = TIROS-N was placed in a near circular, (470nm) polar orbit. The craft and its systems operated successfully, providing high-resolution scanned images and vertical temperature and moisture profiles 
                    Localizer.Format("#autoLOC_MissionController2_1000389");		// #autoLOC_MissionController2_1000389 = to both operational meteorologists and private interests with APT and HRPT capability.
                    tirosCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000390") +		// #autoLOC_MissionController2_1000390 = Good Job you have finsished this contract\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000391") +		// #autoLOC_MissionController2_1000391 = TIROS-N Stats:\n
                    Localizer.Format("#autoLOC_MissionController2_1000392") +		// #autoLOC_MissionController2_1000392 = Launch Date:    October 13, 1978\n
                    Localizer.Format("#autoLOC_MissionController2_1000393") +		// #autoLOC_MissionController2_1000393 = Operational Period:    Operational for 868 days until deactivated by NOAA on February 27, 1981\n
                    Localizer.Format("#autoLOC_MissionController2_1000394") +		// #autoLOC_MissionController2_1000394 = Launch Vehicle:   Atlas E/F\n
                    Localizer.Format("#autoLOC_MissionController2_1000395") +		// #autoLOC_MissionController2_1000395 = Launch Site:    Vandenberg Air Force Base, CA\n
                    Localizer.Format("#autoLOC_MissionController2_1000396");		// #autoLOC_MissionController2_1000396 = Type:   Weather Satellite
                    break;
            }
        }

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Tiros>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Tiros>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }           
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            minHeight = Convert.ToSingle(targetBody.atmosphereDepth);
            Eccentricity = .01;
            AmountDaysActive = 10800;
            TirosTitleMissionNumber = SaveInfo.tirosCurrentNumber;
            if (TirosTitleMissionNumber == 2)
            {
                contractMult = 1.3f;
                AmountDaysActive += 10800;
            }
            else if (TirosTitleMissionNumber == 3)
            {
                contractMult = 1.5f;
                AmountDaysActive *= 3;
            }
            else
            {
                contractMult = 1.0f;
            }
            tirosTextSelect();
            this.AddParameter(new AltitudeGoal(targetBody, minHeight), null);
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new EccentricGoal(targetBody, Eccentricity, false), null);
            if (TirosTitleMissionNumber == 3)
            {
                this.AddParameter(new Inclination(targetBody,90), null);
            }
            this.AddParameter(new TimeCountdownOrbits(targetBody, AmountDaysActive, true), null);
            if (TirosTitleMissionNumber == 2)
            {
                this.AddParameter(new ModuleGoal("ModuleScienceExperiment", "Science Device"), null);
                this.AddParameter(new ModuleGoal("ModuleDataTransmitter", "ModuleRTAntenna", "Antenna Device"), null);
            }
            else if (TirosTitleMissionNumber == 3)
            {
                this.AddParameter(new ModuleGoal("ModuleScienceExperiment", "Science Device"), null);
                this.AddParameter(new ModuleGoal("ModuleScienceExperiment", "2nd Science Device"), null);
            }
            else
            {
                this.AddParameter(new ModuleGoal("ModuleDataTransmitter", "Antenna Device"), null);
            }
            if (TirosTitleMissionNumber == 2 || TirosTitleMissionNumber == 3)
            {
                this.AddParameter(new ModuleGoal("ModuleDeployableSolarPanel", "KopernicusSolarPanel", "Solar Panels"), null);
                this.AddParameter(new ResourceGoalCap("ElectricCharge", 600), null);
            }
            this.AddParameter(new GetCrewCount(0), null);

            base.SetFunds(6000f * contractMult, 30000f * contractMult, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(15f * contractMult, targetBody);
            base.SetScience(2f * contractMult, targetBody);
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

        protected override string GetNotes()
        {
            return tirosNotes;
        }

        protected override string GetHashString()
        {
            return tirosHash + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return tirostitle;
        }
        protected override string GetDescription()
        {
            return tirosDescription;
        }
        protected override string GetSynopsys()
        {
            return tirosSynops;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.tirosCurrentNumber++;
            return Localizer.Format("#autoLOC_MissionController2_1000397");		// #autoLOC_MissionController2_1000397 = Good job launching the tiros vessel
        }

        protected override void OnLoad(ConfigNode node)       
        {
            try {
                Tools.ContractLoadCheck(node, ref tirosNotes, "Notes Did Not Load", tirosNotes, "tnotes");
                Tools.ContractLoadCheck(node, ref tirosHash, "Hash Message Did Not Load", tirosHash, "thash");
                Tools.ContractLoadCheck(node, ref tirostitle, "Title did not load", tirostitle, "ttitle");
                Tools.ContractLoadCheck(node, ref tirosDescription, "Descripition Did Not Load", tirosDescription, "tdescript");
                Tools.ContractLoadCheck(node, ref tirosSynops, "synops Did Not Load", tirosSynops, "tsynop");
                Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
                Tools.ContractLoadCheck(node, ref inclination, 180, inclination, "inclination");
                Tools.ContractLoadCheck(node, ref Eccentricity, .01, Eccentricity, "eccentricity");
                Tools.ContractLoadCheck(node, ref AmountDaysActive, 5, AmountDaysActive, "amountdays");
                Tools.ContractLoadCheck(node, ref contractMult, 1.0f, contractMult, "mult");
                Tools.ContractLoadCheck(node, ref minHeight, 140000, minHeight, "minheight");
                Tools.ContractLoadCheck(node, ref TirosTitleMissionNumber, SaveInfo.tirosCurrentNumber, TirosTitleMissionNumber, "number");
            }
            catch(Exception ex)
            {
                Debug.LogError("OnLoad " + ex.Message);
                Debug.LogError("OnLoad " + ex.StackTrace);
            }
        }
        protected override void OnSave(ConfigNode node)
        {
            try {
                int bodyID = targetBody.flightGlobalsIndex;
                node.AddValue("targetBody", bodyID);
                node.AddValue("tnotes", tirosNotes);
                node.AddValue("thash", tirosHash);
                node.AddValue("ttitle", tirostitle);
                node.AddValue("tdescript", tirosDescription);
                node.AddValue("tsynop", tirosSynops);
                node.AddValue("minheight", minHeight);
                node.AddValue("inclination", inclination);
                node.AddValue("eccentricity", Eccentricity);
                node.AddValue("amountdays", AmountDaysActive);
                node.AddValue("mult", contractMult);
                node.AddValue("number", TirosTitleMissionNumber);
            }
            catch (Exception ex)
            {
                Debug.LogError("OnSave " + ex.Message);
                Debug.LogError("OnSave " + ex.StackTrace);
            }
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("spaceExploration") == RDTech.State.Available;
            bool techUnlock3 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            bool techUnlock4 = ResearchAndDevelopment.GetTechnologyState("advExploration") == RDTech.State.Available;
            if (SaveInfo.tirosCurrentNumber == 1 && techUnlock)
            {
                return true;
            }
            else if (SaveInfo.tirosCurrentNumber == 2 && techUnlock && techUnlock2 && techUnlock3)
            {
                return true;
            }
            else if (SaveInfo.tirosCurrentNumber == 3 && techUnlock && techUnlock2 && techUnlock3 && techUnlock4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class Mariner : Contract
    {
        Settings settings = new Settings("config.cfg");
        CelestialBody targetBody;
        private double PeA = 0;
        private int marinerNumber = 1;
        private float multiplier = 1.0f;
        private double extraBody2 = 0;
        private double extraBody3 = 0;
        private double extraBody4 = 0;
        private double extraBody5 = 0;
        private double extraBody6 = 0;
        public int totalContracts;
        public int TotalFinished;
        private string marinerTitle = "None";
        private string marinerDescription = "none";
        private string marinerSynops = "None";
        private string marinerCompleteMessage = "none";

        ContractParameter marinerOrbit;
        ContractParameter marinerOrbit2;
        ContractParameter marinerOrbit3;
        ContractParameter marinerOrbit4;
        ContractParameter marinerOrbit5;
        ContractParameter marinerOrbit6;

        private void marinerTextSelection()
        {
            switch (marinerNumber)
            {
                case 1:
                    marinerTitle = Localizer.Format("#autoLOC_MissionController2_1000398");		// #autoLOC_MissionController2_1000398 = Mariner 2 flyby of Eve
                    marinerDescription = Localizer.Format("#autoLOC_MissionController2_1000399") +		// #autoLOC_MissionController2_1000399 = As plans were getting under way to explore the Moon with the Rangers and Surveyors, JPL and NASA also turned their attention to the rest of the solar system. The Mariner series of missions 
                    Localizer.Format("#autoLOC_MissionController2_1000400") +		// #autoLOC_MissionController2_1000400 = were designed to be the first U.S. spacecraft to other planets, specifically Venus and Mars. Mariner 1 and 2 were nearly identical spacecraft developed to fly by Venus. The rocket carrying
                    Localizer.Format("#autoLOC_MissionController2_1000401") +		// #autoLOC_MissionController2_1000401 = Mariner 1 went off-course during launch on July 22, 1962, and was blown up by a range safety officer about 5 minutes into flight.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000402") +		// #autoLOC_MissionController2_1000402 = A month later, Mariner 2 was launched successfully on August 27, 1962, sending it on a 3-1/2-month flight to Venus. On the way it measured for the first time the solar wind, a constant stream of 
                    Localizer.Format("#autoLOC_MissionController2_1000403") +		// #autoLOC_MissionController2_1000403 = charged particles flowing outward from the Sun. It also measured interplanetary dust, which turned out to be more scarce than predicted. In addition, Mariner 2 detected high-energy charged particles 
                    Localizer.Format("#autoLOC_MissionController2_1000404") +		// #autoLOC_MissionController2_1000404 = coming from the Sun, including several brief solar flares, as well as cosmic rays from outside the solar system. As it flew by Venus on December 14, 1962, Mariner 2 scanned the planet with infrared and 
                    Localizer.Format("#autoLOC_MissionController2_1000405") +		// #autoLOC_MissionController2_1000405 = microwave radiometers, revealing that Venus has cool clouds and an extremely hot surface. (Because the bright, opaque clouds hide the planet’s surface, Mariner 2 was not outfitted with a camera.) 
                    Localizer.Format("#autoLOC_MissionController2_1000406") +		// #autoLOC_MissionController2_1000406 = Mariner 2's signal was tracked until January 3, 1963. The spacecraft remains in orbit around the Sun.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000407");		// #autoLOC_MissionController2_1000407 = All info for Mariner was collect from Nasa
                    marinerSynops = Localizer.Format("#autoLOC_MissionController2_1000408");		// #autoLOC_MissionController2_1000408 = Eve is the kerbal version of Venus, in this mission you have to do a flyby of Eve and collect science.
                    marinerCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000409");		// #autoLOC_MissionController2_1000409 = Eve is the kerbal version of Venus, you have finished the contract!
                    break;
                case 2:
                    marinerTitle = Localizer.Format("#autoLOC_MissionController2_1000410");		// #autoLOC_MissionController2_1000410 = Mariner 4 Flyby Of Duna
                    marinerDescription = Localizer.Format("#autoLOC_MissionController2_1000411") +		// #autoLOC_MissionController2_1000411 = Mariner 4 was the first spacecraft to get a close look at Mars. Flying as close as 9,846 kilometers (6,118 miles), Mariner 4 revealed Mars to have a cratered, rust-colored surface, with signs 
                    Localizer.Format("#autoLOC_MissionController2_1000412") +		// #autoLOC_MissionController2_1000412 = on some parts of the planet that liquid water had once etched its way into the soil. In addition to various field and particle sensors and detectors, the spacecraft had a television camera, which 
                    Localizer.Format("#autoLOC_MissionController2_1000413") +		// #autoLOC_MissionController2_1000413 = took 22 television pictures covering about 1% of the planet. Initially stored on a 4-track tape recorder, these pictures took four days to transmit to Earth.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000414");		// #autoLOC_MissionController2_1000414 = All Mariner info gathered from NASA
                    marinerSynops = Localizer.Format("#autoLOC_MissionController2_1000415");		// #autoLOC_MissionController2_1000415 = Duna is the Kerbal version of Mars, in this mission you have to do a flyby of Duna and collect science.
                    marinerCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000416");		// #autoLOC_MissionController2_1000416 = Duna is the Kerbal version of Mars, you have finished the contract!
                    break;
                case 3:
                    marinerTitle = Localizer.Format("#autoLOC_MissionController2_1000417");		// #autoLOC_MissionController2_1000417 = Mariner 10 Flyby Of Moho
                    marinerDescription = Localizer.Format("#autoLOC_MissionController2_1000418") +		// #autoLOC_MissionController2_1000418 = Mariner 10 was the seventh successful launch in the Mariner series, the first spacecraft to use the gravitational pull of one planet (Venus) to reach another (Mercury), and the first 
                    Localizer.Format("#autoLOC_MissionController2_1000419") +		// #autoLOC_MissionController2_1000419 = spacecraft mission to visit two planets. Mariner 10 was the first (and as of 2003 the only) spacecraft to visit Mercury. The spacecraft flew by Mercury three times in a retrograde heliocentric 
                    Localizer.Format("#autoLOC_MissionController2_1000420") +		// #autoLOC_MissionController2_1000420 = orbit and returned images and data on the planet. Mariner 10 returned the first-ever close-up images of Venus and Mercury. The primary scientific objectives of the mission were to measure Mercury's 
                    Localizer.Format("#autoLOC_MissionController2_1000421") +		// #autoLOC_MissionController2_1000421 = environment, atmosphere, surface, and body characteristics and to make similar investigations of Venus. Secondary objectives were to perform experiments in the interplanetary medium and to obtain 
                    Localizer.Format("#autoLOC_MissionController2_1000422") +		// #autoLOC_MissionController2_1000422 = experience with a dual-planet gravity-assist mission.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000423");		// #autoLOC_MissionController2_1000423 = All Mariner info gathered from NASA
                    marinerSynops = Localizer.Format("#autoLOC_MissionController2_1000424");		// #autoLOC_MissionController2_1000424 = Moho is the Kerbal version of Mecury, in this mission you have to do a flyby of both Eve (venus) and Moho(Mercury) and collect science.
                    marinerCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000425");		// #autoLOC_MissionController2_1000425 = Moho is the Kerbal version of Mecury, you have finished the contract!
                    break;
                case 4:
                    marinerTitle = Localizer.Format("#autoLOC_MissionController2_1000426");		// #autoLOC_MissionController2_1000426 = Voyager 1 Flyby Of Jool
                    marinerDescription = Localizer.Format("#autoLOC_MissionController2_1000427") +		// #autoLOC_MissionController2_1000427 = Originally, a Mariner 11 and Mariner 12 were planned as part of the Mariner program, however, due to congressional budget cuts, the mission was scaled back to be a flyby of Jupiter and Saturn, 
                    Localizer.Format("#autoLOC_MissionController2_1000428") +		// #autoLOC_MissionController2_1000428 = and renamed the Mariner Jupiter-Saturn probes. As the program progressed, the name was later changed to Voyager, as the probe designs began to differ greatly from previous Mariner missions.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000429") +		// #autoLOC_MissionController2_1000429 = The twin spacecraft Voyager 1 and Voyager 2 were launched by NASA in separate months in the summer of 1977 from Cape Canaveral, Florida. As originally designed, the Voyagers were to conduct closeup 
                    Localizer.Format("#autoLOC_MissionController2_1000430") +		// #autoLOC_MissionController2_1000430 = studies of Jupiter and Saturn, Saturn's rings, and the larger moons of the two planets.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000431") +		// #autoLOC_MissionController2_1000431 = To accomplish their two-planet mission, the spacecraft were built to last five years. But as the mission went on, and with the successful achievement of all its objectives, the additional flybys 
                    Localizer.Format("#autoLOC_MissionController2_1000432") +		// #autoLOC_MissionController2_1000432 = of the two outermost giant planets, Uranus and Neptune, proved possible -- and irresistible to mission scientists and engineers at the Voyagers' home at the Jet Propulsion Laboratory in 
                    Localizer.Format("#autoLOC_MissionController2_1000433") +		// #autoLOC_MissionController2_1000433 = Pasadena, California.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000434") +		// #autoLOC_MissionController2_1000434 = As the spacecraft flew across the solar system, remote-control reprogramming was used to endow the Voyagers with greater capabilities than they possessed when they left the Earth. Their two-planet 
                    Localizer.Format("#autoLOC_MissionController2_1000435") +		// #autoLOC_MissionController2_1000435 = mission became four. Their five-year lifetimes stretched to 12 and more.
                    Localizer.Format("#autoLOC_MissionController2_1000436") +		// #autoLOC_MissionController2_1000436 = Eventually, between them, Voyager 1 and 2 would explore all the giant outer planets of our solar system, 48 of their moons, and the unique systems of rings and magnetic fields those planets possess.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000437");		// #autoLOC_MissionController2_1000437 = Information on Voyager program gathered from NASA.
                    marinerSynops = Localizer.Format("#autoLOC_MissionController2_1000438");		// #autoLOC_MissionController2_1000438 = No real example of Saturn or Jupiter exist in Kerbal Space program, the only Gas Planet in KSP is Jool.  You are to explore Jool and its moon to complete this contract.
                    marinerCompleteMessage = Localizer.Format("#autoLOC_MissionController2_1000439");		// #autoLOC_MissionController2_1000439 = You have visited Jool and its moon, and your voyager craft is still going strong.. We hope.
                    break;
            }
        }

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Mariner>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Mariner>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }           
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            marinerNumber = SaveInfo.marinerCurrentNumber;
            if (marinerNumber == 1)
            {
                targetBody = FlightGlobals.Bodies[5];
                PeA = Tools.getBodyAltitude(targetBody);
            }
            else if (marinerNumber == 2)
            {
                targetBody = FlightGlobals.Bodies[6];
                PeA = Tools.getBodyAltitude(targetBody);
            }
            else if (marinerNumber == 3)
            {
                targetBody = FlightGlobals.Bodies[4];
                PeA = Tools.getBodyAltitude(targetBody);
                extraBody2 = Tools.getBodyAltitude(FlightGlobals.Bodies[5]);
                multiplier = 1.5f;
            }
            else if (marinerNumber == 4)
            {
                targetBody = FlightGlobals.Bodies[8];
                PeA = Tools.getBodyAltitude(targetBody);
                extraBody2 = Tools.getBodyAltitude(FlightGlobals.Bodies[14]);
                extraBody3 = Tools.getBodyAltitude(FlightGlobals.Bodies[12]);
                extraBody4 = Tools.getBodyAltitude(FlightGlobals.Bodies[11]);
                extraBody5 = Tools.getBodyAltitude(FlightGlobals.Bodies[10]);
                extraBody6 = Tools.getBodyAltitude(FlightGlobals.Bodies[9]);
                multiplier = 2.5f;
            }
            else
            {
                return false;
            }
            marinerTextSelection();
            if (marinerNumber == 3)
            {
                this.marinerOrbit2 = this.AddParameter(new FlyByCelestialBodyGoal(FlightGlobals.Bodies[5], extraBody2 + 500000, extraBody2), null);
                marinerOrbit2.SetFunds(1200 * multiplier, FlightGlobals.Bodies[5]);
                this.AddParameter(new CollectScience(FlightGlobals.Bodies[5], BodyLocation.Space), null);
            }
            if (marinerNumber <= 3)
            {
                this.marinerOrbit = this.AddParameter(new FlyByCelestialBodyGoal(targetBody, PeA + 5000, PeA), null);
                marinerOrbit.SetFunds(1200 * multiplier, targetBody);
                this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            }
            else
            {
                this.marinerOrbit = this.AddParameter(new FlyByCelestialBodyGoal(targetBody, PeA + 201000000, PeA), null);
                marinerOrbit.SetFunds(1200 * multiplier, targetBody);
                this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            }

            if (marinerNumber == 4)
            {
                this.marinerOrbit2 = this.AddParameter(new FlyByCelestialBodyGoal(FlightGlobals.Bodies[14], extraBody2 + 50000, extraBody2), null);
                marinerOrbit2.SetFunds(1200 * multiplier, FlightGlobals.Bodies[14]);
                this.AddParameter(new CollectScience(FlightGlobals.Bodies[14], BodyLocation.Space), null);

                this.marinerOrbit3 = this.AddParameter(new FlyByCelestialBodyGoal(FlightGlobals.Bodies[12], extraBody2 + 50000, extraBody2), null);
                marinerOrbit3.SetFunds(1200 * multiplier, FlightGlobals.Bodies[12]);
                this.AddParameter(new CollectScience(FlightGlobals.Bodies[12], BodyLocation.Space), null);

                this.marinerOrbit4 = this.AddParameter(new FlyByCelestialBodyGoal(FlightGlobals.Bodies[11], extraBody2 + 50000, extraBody2), null);
                marinerOrbit4.SetFunds(1200 * multiplier, FlightGlobals.Bodies[11]);
                this.AddParameter(new CollectScience(FlightGlobals.Bodies[11], BodyLocation.Space), null);

                this.marinerOrbit5 = this.AddParameter(new FlyByCelestialBodyGoal(FlightGlobals.Bodies[10], extraBody2 + 50000, extraBody2), null);
                marinerOrbit5.SetFunds(1200 * multiplier, FlightGlobals.Bodies[10]);
                this.AddParameter(new CollectScience(FlightGlobals.Bodies[10], BodyLocation.Space), null);

                this.marinerOrbit6 = this.AddParameter(new FlyByCelestialBodyGoal(FlightGlobals.Bodies[9], extraBody2 + 50000, extraBody2), null);
                marinerOrbit6.SetFunds(1200 * multiplier, FlightGlobals.Bodies[9]);
                this.AddParameter(new CollectScience(FlightGlobals.Bodies[9], BodyLocation.Space), null);
            }

            this.AddParameter(new GetCrewCount(0), null);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3000f, targetBody);
            base.SetFunds(3000 * multiplier, 32000 * multiplier, targetBody);
            base.SetScience(3 * multiplier, targetBody);
            base.SetReputation(25 * multiplier, targetBody);
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
            return Localizer.Format("#autoLOC_MissionController2_1000440") + " " + targetBody.bodyName + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000440 = Conduct Flyby of 
        }
        protected override string GetTitle()
        {
            return marinerTitle;
        }
        protected override string GetDescription()
        {
            return marinerDescription;
        }
        protected override string GetSynopsys()
        {
            return marinerSynops;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.marinerCurrentNumber++;
            return Localizer.Format("#autoLOC_MissionController2_1000441");		// #autoLOC_MissionController2_1000441 = Good job finishing the Mariner Mission
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, FlightGlobals.Bodies[4], targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref marinerTitle, "Title Did Not Load", marinerTitle, "mtitle");
            Tools.ContractLoadCheck(node, ref marinerDescription, "Description Did Not Load", marinerDescription, "mdescript");
            Tools.ContractLoadCheck(node, ref marinerSynops, "Synops Did Not Load", marinerSynops, "msynop");
            Tools.ContractLoadCheck(node, ref marinerCompleteMessage, "Message Complete Did Not Load", marinerCompleteMessage, "mcomplete");            
            Tools.ContractLoadCheck(node, ref PeA, 100000, PeA, "pea");
            Tools.ContractLoadCheck(node, ref marinerNumber, SaveInfo.marinerCurrentNumber, marinerNumber, "mn");
            Tools.ContractLoadCheck(node, ref multiplier, 1f, multiplier, "mult");
            Tools.ContractLoadCheck(node, ref extraBody2, 100000, extraBody2, "extra2");
            Tools.ContractLoadCheck(node, ref extraBody3, 100000, extraBody3, "extra3");
            Tools.ContractLoadCheck(node, ref extraBody4, 100000, extraBody4, "extra4");
            Tools.ContractLoadCheck(node, ref extraBody5, 100000, extraBody5, "extra5");
            Tools.ContractLoadCheck(node, ref extraBody6, 100000, extraBody6, "extra6");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("pea", PeA);
            node.AddValue("mn", marinerNumber);
            node.AddValue("mult", multiplier);
            node.AddValue("extra2", extraBody2);
            node.AddValue("extra3", extraBody3);
            node.AddValue("extra4", extraBody4);
            node.AddValue("extra5", extraBody5);
            node.AddValue("extra6", extraBody6);
            node.AddValue("mtitle", marinerTitle);
            node.AddValue("mdescript", marinerDescription);
            node.AddValue("msynop", marinerSynops);
            node.AddValue("mcomplete", marinerCompleteMessage);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock4 = ResearchAndDevelopment.GetTechnologyState("advExploration") == RDTech.State.Available;
            if (SaveInfo.marinerCurrentNumber <= 4 && SaveInfo.tirosCurrentNumber != 1 && techUnlock & techUnlock4) { return true; }
            else { return false; }

        }
    }
    # region Luna 2
    public class Luna2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 0;
        public int totalContracts;
        public int TotalFinished;
        CelestialBody targetBody = FlightGlobals.Bodies[2];

        ContractParameter luna1;
        ContractParameter luna2;
        ContractParameter luna3;
        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Luna2>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Luna2>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }          
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            luna1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            luna1.SetFunds(4000f, targetBody);
            luna1.SetReputation(8f, targetBody);

            luna2 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            luna2.SetFunds(3000f, targetBody);
            luna2.SetReputation(5f, targetBody);

            luna3 = this.AddParameter(new CrashGoal(targetBody), null);
            luna3.SetFunds(4000f, targetBody);
            luna3.SetReputation(3f, targetBody);
            luna3.SetScience(10f, targetBody);

            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(11000f, 85000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(25f, targetBody);
            base.SetScience(25f, targetBody);
            return true;

        }

        protected override void OnAccepted()
        {
            string AgenaMessage = Localizer.Format("#autoLOC_MissionController2_1000442") +		// #autoLOC_MissionController2_1000442 = For best results with the Luna 2 and its Crash Goal I have found it best to make sure that the Command Pod does not get destroyed first!!\n\n
                Localizer.Format("#autoLOC_MissionController2_1000443") +		// #autoLOC_MissionController2_1000443 = Had many reports of issue with this contract, and it seems that sometimes when the pod goes first then KSP doesn't have time to register the destruction of you personal Vessel.\n\n
                Localizer.Format("#autoLOC_MissionController2_1000444");		// #autoLOC_MissionController2_1000444 = If this contract does give you issue's with the crash goal feel free to ALT F-12, select contracts and used the COM with the Luna 2 contract to complete it, its not cheating its a bug
            MessageSystem.Message m = new MessageSystem.Message(Localizer.Format("#autoLOC_MissionController2_1000445"), AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);		// #autoLOC_MissionController2_1000445 = Important Luna 2 CrashGoal Contract Information
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
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of LUNA 2 To Mun" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000446") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000446 = Launch of Luna 2 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000447") +		// #autoLOC_MissionController2_1000447 = on September 12th, 1959 Luna 2 was launched. At just past midnight Moscow time on September 14th it crashed some 240,000 miles away on the Moon not far from 
                Localizer.Format("#autoLOC_MissionController2_1000448") +		// #autoLOC_MissionController2_1000448 = the Sea of Tranquillity (perhaps a not entirely appropriate location). Korolev and his people were listening as the signals coming back from the spacecraft suddenly stopped. 
                Localizer.Format("#autoLOC_MissionController2_1000449") +		// #autoLOC_MissionController2_1000449 = The total silence meant that Luna had hit its target and there was great jubilation in the control room.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000450") +		// #autoLOC_MissionController2_1000450 = Luna 2 (Luna is Russian for Moon) weighed 390 kilograms. It was spherical in shape with antennae sticking out of it and carried instruments for measuring radiation, magnetic fields 
                Localizer.Format("#autoLOC_MissionController2_1000451") +		// #autoLOC_MissionController2_1000451 = and meteorites. It also carried metal pendants which it scattered on the surface on impact, with the hammer and sickle of the USSR on one side and the launch date on the other. 
                Localizer.Format("#autoLOC_MissionController2_1000452") +		// #autoLOC_MissionController2_1000452 = It confirmed that the moon had only a tiny radiation field and, so far as could be observed, no radiation belts. The spacecraft had no propulsion system of its own and the third and 
                Localizer.Format("#autoLOC_MissionController2_1000453") +		// #autoLOC_MissionController2_1000453 = final stage of its propelling rocket crashed on the moon about half an hour after Luna 2 itself.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000454") +		// #autoLOC_MissionController2_1000454 = Information on Luna 2 was gathered from HistoryToday.com\n\n

                Localizer.Format("#autoLOC_MissionController2_1000455");		// #autoLOC_MissionController2_1000455 = Objectives are \n\n1. Launch Luna 2 \n2. Orbit the Mun \n3. Conduct Science at Mun \n 4. When finished crash vessel into the surface of Mun.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000456") + " " + targetBody.bodyName + " " + ". " + Localizer.Format("#autoLOC_MissionController2_1000457");		// #autoLOC_MissionController2_1000456 = Bring Luna 2 to orbit around 		// #autoLOC_MissionController2_1000457 =  Conduct Science, Then crash into surface of Mun.
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Luna2Done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000458") +		// #autoLOC_MissionController2_1000458 = The scientific results of Luna 2 were similar to those of Luna 1, but the psychological impact of Luna 2 was profound. The closest any American probe had come to the Moon at 
                Localizer.Format("#autoLOC_MissionController2_1000459") +		// #autoLOC_MissionController2_1000459 = that point was 37,000 miles. It seemed clear in the United States that the timing had been heavily influenced by the fact that the Soviet premier, Nikita Khruschev, was due to arrive 
            Localizer.Format("#autoLOC_MissionController2_1000460") +		// #autoLOC_MissionController2_1000460 = in the US immediately afterwards, to be welcomed by President Eisenhower. Luna 2’s success enabled him to appear beaming with rumbustious pride. He lectured Americans on the virtues of communism 
            Localizer.Format("#autoLOC_MissionController2_1000461") +		// #autoLOC_MissionController2_1000461 = and the immorality of scantily clothed chorus girls. The only way of annoying him seemed to be by refusing to let him into Disneyland.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000462") +		// #autoLOC_MissionController2_1000462 = Korolev had a clincher to come. Only three weeks later, Luna 3 was launched on October 4th, the second anniversary of Sputnik 1, to swing round the far side of the Moon and send back the 
            Localizer.Format("#autoLOC_MissionController2_1000463") +		// #autoLOC_MissionController2_1000463 = first fuzzy pictures of its dark side, which no one had seen before. It was an astonishing feat of navigation and it was now possible to draw a tentative map of the Moon’s hidden side.\n\n 

                Localizer.Format("#autoLOC_MissionController2_1000464");		// #autoLOC_MissionController2_1000464 = Information on Luna 2 was gathered from HistoryToday.com
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            crew = int.Parse(node.GetValue("crewcount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewcount", crew);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.Luna2Done == true) { return false; }
            if (SaveInfo.Voskhod2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Luna 16
    public class Luna16 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 0;
        public int totalContracts;
        public int TotalFinished;
        CelestialBody targetBody = FlightGlobals.Bodies[2];
        ContractParameter luna1;
        ContractParameter luna2;
        ContractParameter luna3;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<Luna16>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<Luna16>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }            
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            luna1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            luna1.SetFunds(8000f, targetBody);
            luna1.SetReputation(8f, targetBody);

            luna3 = this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody), null);
            luna3.SetFunds(8000f, targetBody);
            luna3.SetReputation(3f, targetBody);
            luna3.SetScience(25f, targetBody);

            luna2 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Surface), null);
            luna2.SetFunds(5000f, targetBody);
            luna2.SetReputation(5f, targetBody);

            this.AddParameter(new Contracts.Parameters.LandOnBody(Planetarium.fetch.Home), null);

            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(15000f, 150000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(75f, targetBody);
            base.SetScience(50f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of LUNA 16 To Mun" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000465") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000465 = Launch of Luna 16 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000466") +		// #autoLOC_MissionController2_1000466 = Luna 16 was an unmanned space mission, part of the Soviet Luna program.

            Localizer.Format("#autoLOC_MissionController2_1000467") +		// #autoLOC_MissionController2_1000467 = Luna 16 was the first robotic probe to land on the Moon and return a sample of lunar soil to Earth. The sample was returned from Mare Fecunditatis. It represented 
            Localizer.Format("#autoLOC_MissionController2_1000468") +		// #autoLOC_MissionController2_1000468 = the first lunar sample return mission by the Soviet Union and was the third lunar sample return mission overall, following the Apollo 11 and Apollo 12 missions.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000469") +		// #autoLOC_MissionController2_1000469 = The spacecraft consisted of two attached stages, an ascent stage mounted on top of a descent stage. The descent stage was a cylindrical body with four protruding landing legs, fuel tanks, 
            Localizer.Format("#autoLOC_MissionController2_1000470") +		// #autoLOC_MissionController2_1000470 = a landing radar, and a dual descent engine complex.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000471") +		// #autoLOC_MissionController2_1000471 = A main descent engine was used to slow the craft until it reached a cutoff point which was determined by the on-board computer based on altitude and velocity. After cutoff a bank of lower 
            "thrust jets was used for the final landing. The descent stage also acted as a launch pad for the ascent stage.\n\n" +

            Localizer.Format("#autoLOC_MissionController2_1000472") +		// #autoLOC_MissionController2_1000472 = The ascent stage was a smaller cylinder with a rounded top. It carried a cylindrical hermetically sealed soil sample container inside a re-entry capsule.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000473") +		// #autoLOC_MissionController2_1000473 = The spacecraft descent stage was equipped with a television camera, radiation and temperature monitors, telecommunications equipment, and an extendable arm with a drilling rig for 
            Localizer.Format("#autoLOC_MissionController2_1000474") +		// #autoLOC_MissionController2_1000474 = the collection of a lunar soil sample.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000475") +		// #autoLOC_MissionController2_1000475 = Information on Luna 2 was gathered from Wikipedia.org\n\n

                Localizer.Format("#autoLOC_MissionController2_1000476");		// #autoLOC_MissionController2_1000476 = Objectives are \n\n1. Launch Luna 16 \n2. Orbit the Mun \n3. Land Mun (Samples are automated) \n 4. Return to Kerbin with samples.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000477") + " " + targetBody.bodyName + ". " + Localizer.Format("#autoLOC_MissionController2_1000478");		// #autoLOC_MissionController2_1000477 = Bring Luna 16 to orbit around 		// #autoLOC_MissionController2_1000478 =  Land on Mun, Return To Kerbin with automated samples.
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Luna16Done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000479") +		// #autoLOC_MissionController2_1000479 = Three tiny samples (0.2 grams) of the Luna 16 soil were sold at Sotheby auction for $442,500 in 1993. The samples are the only lunar return material in private ownership during the 
                Localizer.Format("#autoLOC_MissionController2_1000480") +		// #autoLOC_MissionController2_1000480 = 20th century. Another source of privately possessed moon rock is lunar meteorites of varying quality and authenticity, and another is lost Apollo moon rocks, possible legal issues aside.

                Localizer.Format("#autoLOC_MissionController2_1000481") +		// #autoLOC_MissionController2_1000481 = A series of 10-kopeck stamps was issued in 1970 to commemorate the flight of Luna 16 lunar probe and depicted the main stages of the program: soft landing on Moon, launch of the lunar soil 
                Localizer.Format("#autoLOC_MissionController2_1000482") +		// #autoLOC_MissionController2_1000482 = sample return capsule, and parachute assisted landing back on Earth.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000483");		// #autoLOC_MissionController2_1000483 = Information on Luna 2 was gathered from Wikipedia.org
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            crew = int.Parse(node.GetValue("crewcount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewcount", crew);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.Luna16Done == true) { return false; }
            if (SaveInfo.Luna2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    public class ApolloProgram : Contract
    {
        CelestialBody targetBody = FlightGlobals.Bodies[2];
        CelestialBody targetBody2 = FlightGlobals.Bodies[1];
        CelestialBody targetBody3 = FlightGlobals.Bodies[3];
        private string ApolloBiome = "Midland Craters";
        private string ApolloBiome2 = " East Crater";
        private string ApolloBiome3 = "Great Flats";
        private string PartDockingModule = "ModuleDockingNode";
        private string ElectricPowerSource = "ElectricCharge";
        private string SolarPanelsModule = "ModuleDeployableSolarPanel";
        private string WheelModule = "ModuleWheelBase";
        private int ApolloMissionNumber = 1;

        private double MinHeight = 250000;
        private string dockingModuleDescription = "Docking Port";
        private double electricPowerDescription = 400;
        private string solarPanelDescription = "Solar Panels";
        private string wheelModuleDescription = "Wheels";
        private int crewCount = 3;
        public int totalContracts;

        public string ApNoteText = "none";
        public string apTitleText = "none";
        public string apSynopsText = "none";
        public string apDescriptText = "none";
        public string apCompletionText = "none";

        public void ApolloTextSelection()
        {
            switch (ApolloMissionNumber)
            {
                case 1:
                    ApNoteText = Localizer.Format("#autoLOC_MissionController2_1000692");		// #autoLOC_MissionController2_1000692 = Apollo 7
                    apTitleText = Localizer.Format("#autoLOC_MissionController2_1000690");		// #autoLOC_MissionController2_1000690 = Apollo 7: CrewPod Test Flight.
                    apSynopsText = Localizer.Format("#autoLOC_MissionController2_1000691");		// #autoLOC_MissionController2_1000691 = Apollo 7 objective is to build and test a new command module that can carry a 3 man crew to the mun.\n\n We will test launch and return the crew module to Kerbin Safely.
                    apDescriptText= Localizer.Format("#autoLOC_MissionController2_1000693") +		// #autoLOC_MissionController2_1000693 = Apollo 7 was the first crewed flight of the Apollo spacecraft, with astronauts Walter Schirra, Jr, Donn Eisele, and Walter Cunningham on board. The primary objectives of the Earth orbiting 
                    Localizer.Format("#autoLOC_MissionController2_1000694") +		// #autoLOC_MissionController2_1000694 = mission were to demonstrate Command and Service Module (CSM), crew, launch vehicle, and mission support facilities performance and to demonstrate CSM rendezvous capability. Two photographic 
                    Localizer.Format("#autoLOC_MissionController2_1000695") +		// #autoLOC_MissionController2_1000695 = experiments and three medical experiments were planned.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000696") +		// #autoLOC_MissionController2_1000696 = The command module (CM), a cone-shaped craft about 390 cm in diameter at the large end, served as a command, control, and communications center. Supplemented by the service module (SM), it provided 
                    Localizer.Format("#autoLOC_MissionController2_1000697") +		// #autoLOC_MissionController2_1000697 = all life support elements for the crew. The CM was capable of attitude control about three axes and some lateral lift translation. It also served as a buoyant vessel at sea. The SM provided the main 
                    Localizer.Format("#autoLOC_MissionController2_1000698") +		// #autoLOC_MissionController2_1000698 = propulsion and maneuvering capability. It was jettisoned just before CM reentry. The SM was a cylinder 390 cm in diameter and 670 cm long. The spacecraft mass of 14,781 kg is the mass of the CSM 
                    Localizer.Format("#autoLOC_MissionController2_1000699") +		// #autoLOC_MissionController2_1000699 = including propellants and expendables. There was no lunar module or boilerplate unit on this flight.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000700");		// #autoLOC_MissionController2_1000700 = All Apollo information was gathered from Nasa website.
                    apCompletionText = Localizer.Format("#autoLOC_MissionController2_1000701");		// #autoLOC_MissionController2_1000701 = Good Job, now that we have the command pod worked out we can start work on the lander!
                    break;
                case 2:
                    ApNoteText = Localizer.Format("#autoLOC_MissionController2_1000702");		// #autoLOC_MissionController2_1000702 = Apollo 9
                    apTitleText = Localizer.Format("#autoLOC_MissionController2_1000703");		// #autoLOC_MissionController2_1000703 = Apollo 9: CrewPod And Lunar Module Test Flight.
                    apSynopsText = Localizer.Format("#autoLOC_MissionController2_1000704");		// #autoLOC_MissionController2_1000704 = Apollo 9 main goal is to launch to Kerbin orbit and test the Command Module and Lunar landing docking procedures.  After completion jetison the Lunar Lander and bring the crew back to Kerbin.
                    apDescriptText= Localizer.Format("#autoLOC_MissionController2_1000705") +		// #autoLOC_MissionController2_1000705 = Apollo 9 was the third crewed Apollo flight and the first crewed flight to include the Lunar Module (LM). The crew was Commander James McDivitt, Command Module (CM) pilot David Scott, and LM 
                    Localizer.Format("#autoLOC_MissionController2_1000706") +		// #autoLOC_MissionController2_1000706 = pilot Russell Schweickart. The primary objective of the mission was to test all aspects of the Lunar Module in Earth orbit, including operation of the LM as an independent self-sufficient spacecraft 
                    Localizer.Format("#autoLOC_MissionController2_1000707") +		// #autoLOC_MissionController2_1000707 = and performance of docking and rendezvous manuevers. The goal was to simulate maneuvers which would be performed in actual lunar missions. Other concurrent objectives included overall checkout of 
                    Localizer.Format("#autoLOC_MissionController2_1000708") +		// #autoLOC_MissionController2_1000708 = launch vehicle and spacecraft systems, crew, and procedures. A multispectral photographic experiment was also performed. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000709") +		// #autoLOC_MissionController2_1000709 = Apollo 9 was composed of a command module, a command service module (CSM), a lunar module, and an instrument unit (IU), and was launched by a Saturn V rocket. The vehicle rocket had three 
                    Localizer.Format("#autoLOC_MissionController2_1000710") +		// #autoLOC_MissionController2_1000710 = stages, S-IC, S-II, and S-IVB. The CM, a cone-shaped craft about 390 cm in diameter at the large end, served as a command, control, and communications center. Supplemented by the SM, it provided 
                    Localizer.Format("#autoLOC_MissionController2_1000711") +		// #autoLOC_MissionController2_1000711 = all life support elements for the three crewmen. The spacecraft mass of 26,801 kg is the mass of the CSM including propellants and expendables. The CM was capable of attitude control about three 
                    Localizer.Format("#autoLOC_MissionController2_1000712") +		// #autoLOC_MissionController2_1000712 = axes and some lateral lift translation. It permitted LM attachment and CM/LM ingress and egress and served as a buoyant vessel at sea. The CSM provided the main propulsion and maneuvering capability. 
                    Localizer.Format("#autoLOC_MissionController2_1000713") +		// #autoLOC_MissionController2_1000713 = It was jettisoned just before CM reentry. The CSM was a cylinder 390 cm in diameter. The LM was a two-stage vehicle that accommodated two men and could transport them to the lunar surface. It had 
                    Localizer.Format("#autoLOC_MissionController2_1000714") +		// #autoLOC_MissionController2_1000714 = its own propulsion, communication, and life support systems.\n\n
                    "All Apollo information was gathered from Nasa website.";
                    apCompletionText= Localizer.Format("#autoLOC_MissionController2_1000715");		// #autoLOC_MissionController2_1000715 = Good Job putting the lander together and getting everything to work, lets set our eyes to the Mun!
                    break;
                case 3:
                    ApNoteText = Localizer.Format("#autoLOC_MissionController2_1000716");		// #autoLOC_MissionController2_1000716 = Apollo 10
                    apTitleText = Localizer.Format("#autoLOC_MissionController2_1000717");		// #autoLOC_MissionController2_1000717 = Apollo 10: Lunar Orbit and Return Only.
                    apSynopsText = Localizer.Format("#autoLOC_MissionController2_1000718") +		// #autoLOC_MissionController2_1000718 = The purpose of the mission was to confirm all aspects of the lunar landing mission exactly as it would be performed, except for the actual landing. Additional objectives included 
                    Localizer.Format("#autoLOC_MissionController2_1000719") +		// #autoLOC_MissionController2_1000719 = verification of lunar module systems in the lunar environment, evaluation of mission-support performance for the combined spacecraft at lunar distance, and further refinement of the lunar 
                    Localizer.Format("#autoLOC_MissionController2_1000720");		// #autoLOC_MissionController2_1000720 = gravitational potential.
                    apDescriptText= Localizer.Format("#autoLOC_MissionController2_1000721") +		// #autoLOC_MissionController2_1000721 = This spacecraft was the second Apollo mission to orbit the Moon, and the first to travel to the Moon with the full Apollo spacecraft, consisting of the Command and Service Module 
                    Localizer.Format("#autoLOC_MissionController2_1000722") +		// #autoLOC_MissionController2_1000722 = (CSM-106, Charlie Brown) and the Lunar Module (LM-4, Snoopy). The spacecraft mass of 28,834 kg is the mass of the CSM including propellants and expendables. The LM mass including propellants 
                    Localizer.Format("#autoLOC_MissionController2_1000723") +		// #autoLOC_MissionController2_1000723 = was 13,941 kg. The primary objectives of the mission were to demonstrate crew, space vehicle, and mission support facilities during a manned lunar mission and to evaluate LM performance in 
                    Localizer.Format("#autoLOC_MissionController2_1000724") +		// #autoLOC_MissionController2_1000724 = cislunar and lunar environment. The mission was a full dry run for the Apollo 11 mission, in which all operations except the actual lunar landing were performed. The flight carried a 
                    Localizer.Format("#autoLOC_MissionController2_1000725") +		// #autoLOC_MissionController2_1000725 = three man crew: Commander Thomas P. Stafford, Command Module (CM) Pilot John W. Young, and Lunar Module (LM) Pilot Eugene A. Cernan.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000726");		// #autoLOC_MissionController2_1000726 = All Apollo information gathered from Nasa Website.
                    apCompletionText= Localizer.Format("#autoLOC_MissionController2_1000727");		// #autoLOC_MissionController2_1000727 = Good Job, next time we will give you permission to actually land! Lets move!
                    break;
                case 4:
                    ApNoteText = Localizer.Format("#autoLOC_MissionController2_1000728");		// #autoLOC_MissionController2_1000728 = Apollo 11
                    apTitleText = Localizer.Format("#autoLOC_MissionController2_1000729") + ApolloBiome;                  		// #autoLOC_MissionController2_1000729 = Apollo 11: Lunar Landing On 
                    apSynopsText = Localizer.Format("#autoLOC_MissionController2_1000730") +		// #autoLOC_MissionController2_1000730 = The mission plan of Apollo 11 was to land two men on the lunar surface and return them safely to Earth. The launch took place at Kennedy Space Center Launch Complex 39A on July 16, 1969, at 08:32 a.m. 
                    Localizer.Format("#autoLOC_MissionController2_1000731") +		// #autoLOC_MissionController2_1000731 = EST. The spaccraft carried a crew of three: Mission Commander Neil Armstrong, Command Module Pilot Michael Collins, and Lunar Module Pilot Edwin E. Aldrin Jr. The mission evaluation concluded 
                    Localizer.Format("#autoLOC_MissionController2_1000732");		// #autoLOC_MissionController2_1000732 = that all mission tasks were completed satisfactorily.
                    apDescriptText= Localizer.Format("#autoLOC_MissionController2_1000733") +		// #autoLOC_MissionController2_1000733 = Apollo 11 was the first mission in which humans walked on the lunar surface and returned to Earth. On 20 July 1969 two astronauts (Apollo 11 Commander Neil A. Armstrong and LM pilot Edwin E. Buzz 
                    Localizer.Format("#autoLOC_MissionController2_1000734") +		// #autoLOC_MissionController2_1000734 = Aldrin Jr.) landed in Mare Tranquilitatis (the Sea of Tranquility) on the Moon in the Lunar Module (LM) while the Command and Service Module (CSM) (with CM pilot Michael Collins) continued 
                    Localizer.Format("#autoLOC_MissionController2_1000735") +		// #autoLOC_MissionController2_1000735 = in lunar orbit. During their stay on the Moon, the astronauts set up scientific experiments, took photographs, and collected lunar samples. The LM took off from the Moon on 21 July and the astronauts 
                    Localizer.Format("#autoLOC_MissionController2_1000736") +		// #autoLOC_MissionController2_1000736 = returned to Earth on 24 July.\n\n
                    "All Apollo information gathered from Nasa website.";
                    apCompletionText= Localizer.Format("#autoLOC_MissionController2_1000737");		// #autoLOC_MissionController2_1000737 = Good Job, we have found that we should invest in some type of rover for our landings can move around a lot more.
                    break;
                case 5:
                    ApNoteText = Localizer.Format("#autoLOC_MissionController2_1000738");		// #autoLOC_MissionController2_1000738 = Apollo 15
                    apTitleText = Localizer.Format("#autoLOC_MissionController2_1000739") + ApolloBiome2 + " With Lunar Rover";                   		// #autoLOC_MissionController2_1000739 = Apollo 15: Lunar Landing On 
                    apSynopsText = Localizer.Format("#autoLOC_MissionController2_1000740") +		// #autoLOC_MissionController2_1000740 = Apollo 15 was the first of the three J missions designed to conduct exploration of the Moon over longer periods, over greater ranges, and with more instruments for scientific data acquisition than 
                    Localizer.Format("#autoLOC_MissionController2_1000741") +		// #autoLOC_MissionController2_1000741 = on previous Apollo missions. Major modifications and augmentations to the basic Apollo hardware were made. The most significant change was the installation of a scientific instrument module in one 
                    Localizer.Format("#autoLOC_MissionController2_1000742") +		// #autoLOC_MissionController2_1000742 = of the service module bays for scientific investigations from lunar orbit. Other hardware changes consisted of lunar module modifications to accommodate a greater payload and a longer stay on the 
                    Localizer.Format("#autoLOC_MissionController2_1000743");		// #autoLOC_MissionController2_1000743 = lunar surface, and the provision of a lunar roving vehicle. The landing site chosen for the mission was an area near the foot of the Montes Apenniuns and adjacent to Hadley Rille.
                    apDescriptText= Localizer.Format("#autoLOC_MissionController2_1000744") +		// #autoLOC_MissionController2_1000744 = Apollo 15 was the fourth mission in which humans walked on the lunar surface and returned to Earth. On 30 July 1971 two astronauts (Apollo 15 Commander David R. Scott and LM pilot James B. Irwin) landed
                    Localizer.Format("#autoLOC_MissionController2_1000745") +		// #autoLOC_MissionController2_1000745 = in the Hadley Rille/Apennines region of the Moon in the Lunar Module (LM) while the Command and Service Module (CSM) (with CM pilot Alfred M. Worden) continued in lunar orbit. During their stay on the
                    Localizer.Format("#autoLOC_MissionController2_1000746") +		// #autoLOC_MissionController2_1000746 = Moon, the astronauts set up scientific experiments, took photographs, and collected lunar samples. The LM took off from the Moon on 2 August and the astronauts returned to Earth on 7 August.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000747") +		// #autoLOC_MissionController2_1000747 = Apollo 15 was also the first test of the Lunar Rover\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000748") +		// #autoLOC_MissionController2_1000748 = The Lunar Roving Vehicle (LRV) was an electric vehicle designed to operate in the low-gravity vacuum of the Moon and to be capable of traversing the lunar surface, allowing the Apollo astronauts 
                    Localizer.Format("#autoLOC_MissionController2_1000749") +		// #autoLOC_MissionController2_1000749 = to extend the range of their surface extravehicular activities. Three LRVs were driven on the Moon, one on Apollo 15 by astronauts David Scott and Jim Irwin, one on Apollo 16 by John Young and Charles 
                    Localizer.Format("#autoLOC_MissionController2_1000750") +		// #autoLOC_MissionController2_1000750 = Duke, and one on Apollo 17 by Gene Cernan and Harrison Schmitt. Each rover was used on three traverses, one per day over the three day course of each mission. On Apollo 15 the LRV was driven a total of 
                    Localizer.Format("#autoLOC_MissionController2_1000751") +		// #autoLOC_MissionController2_1000751 = 27.8 km in 3 hours, 2 minutes of driving time. The longest single traverse was 12.5 km and the maximum range from the LM was 5.0 km. On Apollo 16 the vehicle traversed 26.7 km in 3 hours 26 minutes of 
                    Localizer.Format("#autoLOC_MissionController2_1000752") +		// #autoLOC_MissionController2_1000752 = driving. The longest traverse was 11.6 km and the LRV reached a distance of 4.5 km from the LM. On Apollo 17 the rover went 35.9 km in 4 hours 26 minutes total drive time. The longest traverse was 
                    Localizer.Format("#autoLOC_MissionController2_1000753") +		// #autoLOC_MissionController2_1000753 = 20.1 km and the greatest range from the LM was 7.6 km. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000754");		// #autoLOC_MissionController2_1000754 = All Apollo information gathered from Nasa website.
                    apCompletionText= Localizer.Format("#autoLOC_MissionController2_1000755") +		// #autoLOC_MissionController2_1000755 = Good Job on that rover!  But we have a new task ahead for a future mission.  Minmus!\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000756");		// #autoLOC_MissionController2_1000756 = There is talk in the space center about an extension of the apollo program after Apollo 17.  Many are speculating that the higher ups are considering Duna as a possible target!
                    break;
                case 6:
                    ApNoteText = Localizer.Format("#autoLOC_MissionController2_1000757");                    		// #autoLOC_MissionController2_1000757 = Apollo 17
                    apTitleText = Localizer.Format("#autoLOC_MissionController2_1000758") + ApolloBiome3 + " With Minmus Rover";		// #autoLOC_MissionController2_1000758 = Apollo 17: Minmus Landing On 
                    apSynopsText = Localizer.Format("#autoLOC_MissionController2_1000759") +		// #autoLOC_MissionController2_1000759 = The mission plan of Apollo 17 is to land on Minmus in the Great Flats, Scientific objectives of the Apollo 17 mission included, geological surveying and sampling of materials and surface features
                    Localizer.Format("#autoLOC_MissionController2_1000760");		// #autoLOC_MissionController2_1000760 = in a preselected area of the Taurus-Littrow region; deploying and activating surface experiments; and conducting in-flight experiments and photographic tasks during lunar orbit and transearth coast
                    apDescriptText= Localizer.Format("#autoLOC_MissionController2_1000761") +		// #autoLOC_MissionController2_1000761 = The lunar landing site was the Taurus-Littrow highlands and valley area. This site was picked for Apollo 17 as a location where rocks both older and younger than those previously returned from other
                    Localizer.Format("#autoLOC_MissionController2_1000762") +		// #autoLOC_MissionController2_1000762 = Apollo missions, as well as from Luna 16 and 20 missions, might be found.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000763") +		// #autoLOC_MissionController2_1000763 = The mission was the final in a series of three J-type missions planned for the Apollo Program. These J-type missions can be distinguished from previous G- and H-series missions by extended hardware
                    Localizer.Format("#autoLOC_MissionController2_1000764");		// #autoLOC_MissionController2_1000764 = capability, larger scientific payload capacity and by the use of the battery-powered Lunar Roving Vehicle, or LRV.
                    apCompletionText= Localizer.Format("#autoLOC_MissionController2_1000765") +		// #autoLOC_MissionController2_1000765 = Great job, its a sad day that the Apollo missions have come to a close, but you have proven that we can achieve anything as a species!\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000766") +		// #autoLOC_MissionController2_1000766 = Alternate reality for kerbals.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000767") +		// #autoLOC_MissionController2_1000767 = In the human world the American Apollo program was ended after Apollo 17, the kerbals on the other hand had no such limitations.  They have decided to continue the Apollo program and work on 
                    Localizer.Format("#autoLOC_MissionController2_1000768") +		// #autoLOC_MissionController2_1000768 = getting to Duna.  This new series of Contracts for Mission Controller runs through about 9 new Contracts that help set up the ability to get to Duna and establish a small colony. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000769") +		// #autoLOC_MissionController2_1000769 = If you wish not to play this new set of contracts you can turn them off in the Mission Controller settings menu. Before these set of missions start, you need to play the Historical missions 
                    Localizer.Format("#autoLOC_MissionController2_1000770");		// #autoLOC_MissionController2_1000770 = SkyLab, this sets up the ability for long duration space Expeditions and will teach the kerbals how to survive the long journey to Duna.
                    break;
            }
        }

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<ApolloProgram>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }          
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            ApolloMissionNumber = SaveInfo.apolloCurrentNumber;
            ApolloTextSelection();
            MinHeight = Convert.ToSingle(targetBody.atmosphereDepth);
            if (ApolloMissionNumber == 6)
            {
                this.AddParameter(new InOrbitGoal(targetBody2), null);
                this.AddParameter(new InOrbitGoal(targetBody3), null);
                this.AddParameter(new BiomLandingParameters(targetBody3, false, ApolloBiome3), null);
                this.AddParameter(new ModuleGoal(WheelModule, wheelModuleDescription), null);
                base.SetFunds(12000f, 210000f, targetBody3);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(20f, targetBody3);
                base.SetScience(3f, targetBody3);
            }
            if (ApolloMissionNumber == 5)
            {
                this.AddParameter(new InOrbitGoal(targetBody2), null);
                this.AddParameter(new InOrbitGoal(targetBody), null);
                this.AddParameter(new BiomLandingParameters(targetBody, false, ApolloBiome2), null);
                this.AddParameter(new ModuleGoal(WheelModule, wheelModuleDescription), null);
                base.SetFunds(10000f, 140000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(20f, targetBody);
                base.SetScience(3f, targetBody);
            }
            if (ApolloMissionNumber == 4)
            {
                this.AddParameter(new InOrbitGoal(targetBody2), null);
                this.AddParameter(new InOrbitGoal(targetBody), null);
                this.AddParameter(new BiomLandingParameters(targetBody, false, ApolloBiome), null);
                base.SetFunds(10000f, 120000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(15f, targetBody);
                base.SetScience(5f, targetBody);
            }
            if (ApolloMissionNumber == 3)
            {
                this.AddParameter(new InOrbitGoal(targetBody2), null);
                this.AddParameter(new InOrbitGoal(targetBody), null);
                base.SetFunds(8000f, 100000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(7f, targetBody);
                base.SetScience(2f, targetBody);
            }
            if (ApolloMissionNumber == 2)
            {
                this.AddParameter(new AltitudeGoal(targetBody2, MinHeight, true), null);
                this.AddParameter(new InOrbitGoal(targetBody2), null);
                this.AddParameter(new DockingGoal(), null);
                base.SetFunds(5000f, 65000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(4f, targetBody);
                base.SetScience(1f, targetBody);
            }
            if (ApolloMissionNumber == 1)
            {
                this.AddParameter(new AltitudeGoal(targetBody2, MinHeight, true), null);
                this.AddParameter(new InOrbitGoal(targetBody2), null);
                base.SetFunds(4000f, 35000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(2f, targetBody);
                base.SetScience(1f, targetBody);
            }
            this.AddParameter(new GetCrewCount(crewCount), null);
            this.AddParameter(new ModuleGoal(PartDockingModule, dockingModuleDescription), null);
            this.AddParameter(new ModuleGoal(SolarPanelsModule, "KopernicusSolarPanel", solarPanelDescription), null);
            this.AddParameter(new ResourceGoalCap(ElectricPowerSource, electricPowerDescription), null);
            return true;

        }

        protected override void OnAccepted()
        {

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
            return ApNoteText + this.MissionSeed.ToString(); 
        }
        protected override string GetTitle()
        {
            return apTitleText;
        }
        protected override string GetDescription()
        {
            return apDescriptText;
        }
        protected override string GetSynopsys()
        {
            return apSynopsText;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.apolloCurrentNumber++;
            return apCompletionText;
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref ApNoteText, "Hash String Not loaded", ApNoteText, "apnote");
            Tools.ContractLoadCheck(node, ref apTitleText, "Title Not Loaded", apTitleText, "aptitle");
            Tools.ContractLoadCheck(node, ref apDescriptText, "Description Not Loaded", apDescriptText, "apdescript");
            Tools.ContractLoadCheck(node, ref apSynopsText, "Synops Not Loaded", apSynopsText, "apsynop");
            Tools.ContractLoadCheck(node, ref apCompletionText, "Message Complete Not Loaded", apCompletionText, "apcomplete");
            Tools.ContractLoadCheck(node, ref targetBody, FlightGlobals.Bodies[2], targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref targetBody2, FlightGlobals.Bodies[1], targetBody2, "targetBody2");
            Tools.ContractLoadCheck(node, ref targetBody3, FlightGlobals.Bodies[3], targetBody3, "targetBody3");
            Tools.ContractLoadCheck(node, ref ApolloBiome, "None", ApolloBiome, "apbiome");
            Tools.ContractLoadCheck(node, ref ApolloBiome2, "none", ApolloBiome2, "apbiome2");
            Tools.ContractLoadCheck(node, ref ApolloBiome3, "none", ApolloBiome3, "apbiome3");
            Tools.ContractLoadCheck(node, ref PartDockingModule, "ModuleDockingNode", PartDockingModule, "dock");
            Tools.ContractLoadCheck(node, ref ElectricPowerSource, "ElectricCharge", ElectricPowerSource, "electric");
            Tools.ContractLoadCheck(node, ref SolarPanelsModule, "ModuleDeployableSolarPanel", SolarPanelsModule, "solar");
            Tools.ContractLoadCheck(node, ref ApolloMissionNumber, SaveInfo.apolloCurrentNumber, ApolloMissionNumber, "apnumber");
            Tools.ContractLoadCheck(node, ref MinHeight, 200000, MinHeight, "minheight");
            Tools.ContractLoadCheck(node, ref dockingModuleDescription, "Must Have Docking Port", dockingModuleDescription, "dockdesc");
            Tools.ContractLoadCheck(node, ref electricPowerDescription, 1000, electricPowerDescription, "powernumber");
            Tools.ContractLoadCheck(node, ref solarPanelDescription, "Must Have Solar Panels", solarPanelDescription, "solardesc");
            Tools.ContractLoadCheck(node, ref crewCount, 3, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref WheelModule, "ModuleWheel", WheelModule, "wheel");
            Tools.ContractLoadCheck(node, ref wheelModuleDescription, "Wheels On Rover", wheelModuleDescription, "wheeldesc");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            int bodyID2 = targetBody2.flightGlobalsIndex;
            node.AddValue("targetBody2", bodyID2);

            int bodyID3 = targetBody3.flightGlobalsIndex;
            node.AddValue("targetBody3", bodyID3);

            node.AddValue("apnote", ApNoteText);
            node.AddValue("aptitle", apTitleText);
            node.AddValue("apdescript", apDescriptText);
            node.AddValue("apsynop", apSynopsText);
            node.AddValue("apcomplete",apCompletionText);
            node.AddValue("apbiome", ApolloBiome);
            node.AddValue("apbiome2", ApolloBiome2);
            node.AddValue("apbiome3", ApolloBiome3);
            node.AddValue("dock", PartDockingModule);
            node.AddValue("electric", ElectricPowerSource);
            node.AddValue("solar", SolarPanelsModule);
            node.AddValue("apnumber", ApolloMissionNumber);
            node.AddValue("minheight", MinHeight);
            node.AddValue("dockdesc", dockingModuleDescription);
            node.AddValue("powernumber", electricPowerDescription);
            node.AddValue("solardesc", solarPanelDescription);
            node.AddValue("crewcount", crewCount);
            node.AddValue("wheel", WheelModule);
            node.AddValue("wheeldesc", wheelModuleDescription);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("advLanding") == RDTech.State.Available;
            bool techUnlock3 = ResearchAndDevelopment.GetTechnologyState("heavierRocketry") == RDTech.State.Available;
            //if (SaveInfo.Luna16Done == false)
            //{
            //    return false;
            //}
            if (techUnlock && techUnlock2 && techUnlock3 && ApolloMissionNumber <= 6)
            {
                return true;
            }
            else return false;

        }
    }
    public class ApolloDunaProgram : Contract
    {
        CelestialBody targetBody;
        CelestialBody targetBody2;
        CelestialBody targetBody3;
        CelestialBody targetBody4;
        private string ApolloDunaBiome = "Farside Crater";       
        private int ApolloDunaMissionNumber = 1;        
        private int crewCount = 3;
        private int maxSeatCountShip = 4;
        private string landingTitle = "Land your vessel near your Colony Habitat";
        public int totalContracts;

        public string apdHash = "none";
        public string apTitle = "none";
        public string apDescription = "none";
        public string apsynops = "none";
        public string apCompletMessage = "none";

        public void GetLatandLon(Vessel vessel)
        {
            double LatValue;
            LatValue = vessel.latitude;
            double LonValue;
            LonValue = vessel.longitude;

            SaveInfo.apolloLandingLat = LatValue;

            SaveInfo.apolloLandingLon = LonValue;
        }

        public void apdMessageSelection()
        {
            switch (ApolloDunaMissionNumber)
            {
                case 1:
                    apdHash ="Duna 1";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000605");		// #autoLOC_MissionController2_1000605 = Apollo-Duna 1: Mun Test Colony Module.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000606") +		// #autoLOC_MissionController2_1000606 = After the successful Apollo Moon Missions Kerbal Space Center Officials planed a new Mission that could take Kerbal kind to the Planet Duna.  Dubbed Apollo-Duna Missions these missions would be the 
                    Localizer.Format("#autoLOC_MissionController2_1000607") +		// #autoLOC_MissionController2_1000607 = backbone of the new Kerbal Exploration and colonization system that would help spread kerbal kind throughout the kerbin system.\n
                    Localizer.Format("#autoLOC_MissionController2_1000608") +		// #autoLOC_MissionController2_1000608 = Duna 1 is the mission where Engineers and scientist built and delivered a experimental Colony Habitat to the surface of the mun.  This engineering test was slated for 4 Missions.  1. The delivery 
                    Localizer.Format("#autoLOC_MissionController2_1000609") +		// #autoLOC_MissionController2_1000609 = of colony Module.  2. The construction and landing of the New Duna Lander.  3. The delivery of the Colonies first crew.  And 4. The crew transfer Mission for the Year 2 Test Mission.  These missions helped 
                    Localizer.Format("#autoLOC_MissionController2_1000610") +		// #autoLOC_MissionController2_1000610 = develop the procedures and experience needed to bring kerbals to Duna with a Higher success rate. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000611") +		// #autoLOC_MissionController2_1000611 = This Unmanned test vehicle will be launched from Kerbin and land on the Farside Crater located on our own Mun.  This will be tested by two separate crews 
                    Localizer.Format("#autoLOC_MissionController2_1000612");		// #autoLOC_MissionController2_1000612 = that will each spend at least 1 month on the mun.  Use this time to plan out how to survive on Duna and work out any bugs that might happen with the new equipment.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000613") +		// #autoLOC_MissionController2_1000613 = We need you to construct and design a new Duna Colony Module that can hold 3 kerbals with enough supplies to last at least a year on Duna.  This module must also be able to survive 2 separate 
                    Localizer.Format("#autoLOC_MissionController2_1000614") +		// #autoLOC_MissionController2_1000614 = Crew rotations.  The plan is to send 2 Expeditions to Duna.  Expedition 1 will arrive first and stay for a year.  Expedition 2 will launch a year later, while Expedition 1 returns to Kerbin. 
                    Localizer.Format("#autoLOC_MissionController2_1000615") +		// #autoLOC_MissionController2_1000615 = After Expedition 2 arrives it also must survive for at least 1 year also!  So you must plan your supplies for these two missions and and support craft that you might need to keep these two 
                    Localizer.Format("#autoLOC_MissionController2_1000616") +		// #autoLOC_MissionController2_1000616 = missions going. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000617") +		// #autoLOC_MissionController2_1000617 = Duna 1 is a test bed for your new Colony module.  This test bed will be launched from Kerbin and land on the Farside Crater located on our own Mun.  This will be tested by two separate crews 
                    Localizer.Format("#autoLOC_MissionController2_1000618") +		// #autoLOC_MissionController2_1000618 = that will each spend at least 1 month on the mun.  Use this time to plan out how to survive on Duna and work out any bugs that might happen with the new equipment.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000619");		// #autoLOC_MissionController2_1000619 = Your first task is to build the Colony Module and land it on the Mun (Without Crew).
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000620") +		// #autoLOC_MissionController2_1000620 = Great job you have successfully landed the colony module on the Mun.  We are now ready to construct a new lander capable of landing on Duna.  
                    Localizer.Format("#autoLOC_MissionController2_1000621");		// #autoLOC_MissionController2_1000621 = We will first test it on the Mun and make sure everything works as planned. 
                    break;
                case 2:
                    apdHash ="Duna 2";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000622");		// #autoLOC_MissionController2_1000622 = Apollo-Duna 2: Duna Lander Development and Test Mun.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000623") +		// #autoLOC_MissionController2_1000623 = We need you to construct and design a new Duna Landing Module that can hold 3 kerbals with enough supplies to last at least a year on Duna. 
                    Localizer.Format("#autoLOC_MissionController2_1000624") +		// #autoLOC_MissionController2_1000624 =  The plan is to send 2 Expeditions to Duna.  Expedition 1 will arrive first and stay for a year. 
                    Localizer.Format("#autoLOC_MissionController2_1000625") +		// #autoLOC_MissionController2_1000625 = After Expedition 2 arrives it also must survive for at least 1 year also!  So you must plan your supplies for these two missions and and support craft that you might need to keep these two 
                    Localizer.Format("#autoLOC_MissionController2_1000626") +		// #autoLOC_MissionController2_1000626 = missions going. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000627") +		// #autoLOC_MissionController2_1000627 = Duna 2 is a test bed for your new Landing module.  . You need to land your test lander near the Colony module you landed on the Mun in Apollo-Duna 1.  This will help with your ability to land 
                    Localizer.Format("#autoLOC_MissionController2_1000628");		// #autoLOC_MissionController2_1000628 = at a specific spot without risk of death or injury to any Kerbals.  Good luck.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000629") +		// #autoLOC_MissionController2_1000629 = Construct and design a new lander that has the ability to land on Duna.  The use of parachutes and preliminary test flight in Kerbin atmosphere is highly suggest by Both engineering division, and Management. 
                    Localizer.Format("#autoLOC_MissionController2_1000630") +		// #autoLOC_MissionController2_1000630 = The first use of Drag Chutes might come in very handy for slowing down the lander on Decent to the Duna Surface. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000631") +		// #autoLOC_MissionController2_1000631 = The first practice run will be conducted on the Mun.  Get the lander to the Colony module located on the mun.  High precision intercept is very difficult in Duna atmosphere so deep thought and planning will be needed 
                    Localizer.Format("#autoLOC_MissionController2_1000632") +		// #autoLOC_MissionController2_1000632 = for this mission.  Use the practice missions to make it happen!\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000633") +		// #autoLOC_MissionController2_1000633 = This mission records your Colony Modules postion and requires you to land within a reasonable distance of the colony module.  This information was recorded on your previous mission Apollo-Duna 1.  If you have moved 
                    Localizer.Format("#autoLOC_MissionController2_1000634");		// #autoLOC_MissionController2_1000634 = your colony module since that mission, this will cause issues completing these missions!
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000635");		// #autoLOC_MissionController2_1000635 = Great job on that lander, work out any bugs or problems that you encourtered during this mission in preperation of the next mission.
                    break;
                case 3:
                    apdHash ="Duna 3";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000636");		// #autoLOC_MissionController2_1000636 = Apollo-Duna 3: Crew Test on Mun Colony Module.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000637") +		// #autoLOC_MissionController2_1000637 = Congratulations on your first Colony test on the Mun.  It is now time to rotate the crew and conduct a 2nd series of test.  A new crew will be delivered to the Mun Colony habital and stay for 
                    Localizer.Format("#autoLOC_MissionController2_1000638");		// #autoLOC_MissionController2_1000638 = another month.  This will conclude the testing phase of the Duna Colony module and should clear the vehicle for delivery to Duna.  Make sure any bugs are worked out in the Colony Habitat and Lander.  Good luck.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000639") +		// #autoLOC_MissionController2_1000639 = Launch you Lander to the mun.  Land near your Colony habitat.  Mission records have recorded where your Colony habitat is located.  Like the last unmanned mission we are checking to make sure 
                    Localizer.Format("#autoLOC_MissionController2_1000640") +		// #autoLOC_MissionController2_1000640 = that you land close enough to the Colony habitat.  If you don’t then Mission will be considered failed.  The habitat is important for survival of the crew on Duna.  If you land to far away 
                    Localizer.Format("#autoLOC_MissionController2_1000641") +		// #autoLOC_MissionController2_1000641 = they will not survive the yearlong mission on Duna!\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000642");		// #autoLOC_MissionController2_1000642 = When crew is set up conduct Scientific studies on the Mun.  Your mission will last 1 month for the test.  After a month return to kerbin safely!
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000643");		// #autoLOC_MissionController2_1000643 = Great job, the civilian population of kerbin are getting very excited about the prospect of a kerbal landing on Duna.  Lets make this happen.
                    break;
                case 4:
                    apdHash ="Duna 4";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000644");		// #autoLOC_MissionController2_1000644 = Apollo 4: Crew Transfer on Mun Colony Module. 
                    apDescription ="Live Crew Habitat Test:  Its time we conduct a live crew test of our test Lunar Colony module.  You will launch your new Test lander with crew to the Lunar Colony module.  When the crew arrive "+
                    Localizer.Format("#autoLOC_MissionController2_1000645") +		// #autoLOC_MissionController2_1000645 = they will conduct scientific test and stay in the module for 1 Months’ time.  \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000646") +		// #autoLOC_MissionController2_1000646 = When the test in complete they will launch back to Kerbin and the 2nd crew will launch and conduct a 2nd month test in the Lunar Colony module.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000647");		// #autoLOC_MissionController2_1000647 = These test are to practice any and all scientific activities you plan to conduct on the Actual Duna Colony module when it arrives at Duna.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000648") +		// #autoLOC_MissionController2_1000648 = Launch the 2nd crew to the Lunar Colony Module and conduct scientific test and studies for 1 months’ time.  After completion of these test return you crew safely to Kerbin.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000649");		// #autoLOC_MissionController2_1000649 = Objective are to test and fix any issues in the Lunar Colony module and the Lander.
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000650") +		// #autoLOC_MissionController2_1000650 = We are now much closer to our final goal of landing a kerbal on Duna.  It time to work out any final bugs that might of been found during these last two missions.  Once the Duna missions start we 
                    Localizer.Format("#autoLOC_MissionController2_1000651");		// #autoLOC_MissionController2_1000651 = will not be able to fix these issue!  So lets get it right the first time!
                    break;
                case 5:
                    apdHash ="Duna 5";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000652");		// #autoLOC_MissionController2_1000652 = Apollo-Duna 5: Duna Apollo Flyby With Crew
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000653") +		// #autoLOC_MissionController2_1000653 = Duna Flyby:  We have tested a new Colony Module, and new Lander for Duna.  But now we also must have a way to get to Duna.  Design a Vessel that can carry your crew and landers to Duna.  
                    Localizer.Format("#autoLOC_MissionController2_1000654") + 		// #autoLOC_MissionController2_1000654 = This is really your choice.  You can split up the vessels into many.  Or you can construct 1 large vessel the choice is yours. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000655") +		// #autoLOC_MissionController2_1000655 = This first mission is only a flyby and Orbit of Duna.  You can use this mission for many purposes.  Testing your ability of launching to Duna should be of top Priority.  Establishing a 
                    Localizer.Format("#autoLOC_MissionController2_1000656");		// #autoLOC_MissionController2_1000656 = network of satellites if need be is also a good idea.  The main point of the mission is to bring your crew of kerbals to Duna and survive.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000657");		// #autoLOC_MissionController2_1000657 = Construct a vessel to fly to Duna.  Bring 3 Kerbals with you on this trip and make sure they survive.
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000658");		// #autoLOC_MissionController2_1000658 = Nice job on bring our kerbals on first orbit of Duna.  These lucky kerbals are the first to see Duna in person.  They are proud of what the accomplished.
                    break;
                case 6:
                    apdHash ="Duna 6";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000659");		// #autoLOC_MissionController2_1000659 = Apollo-Duna 6: Land Colony Module (without crew) on Duna.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000660") +		// #autoLOC_MissionController2_1000660 = It’s time to start the main objective of the Apollo-Duna missions.  Our first priority is to get our new Colony Module on Duna safely.  After delivery of this module we can send the first crew 
                    Localizer.Format("#autoLOC_MissionController2_1000661") + 		// #autoLOC_MissionController2_1000661 = to Duna. This is a very important mission, without the colony module there is No Apollo-Duna mission.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000662");		// #autoLOC_MissionController2_1000662 = You can also use this mission to deliver any other type of module that you may need to keep your kerbals alive on Duna for a year.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000663");		// #autoLOC_MissionController2_1000663 = Deliver your colony module to Duna.  Any other type of support mission you have planned can be launched in this window also.
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000664") +		// #autoLOC_MissionController2_1000664 = Great job on delivery your Colony module to Duna.  We have recorded the landing site in our computers and all crew missions will be required to land in the area the Colony module is located.\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000665");		// #autoLOC_MissionController2_1000665 = Next mission we will launch our crew to Duna.  We just need to wait for a new launch window to open up.
                    break;
                case 7:
                    apdHash ="Duna 8";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000666");		// #autoLOC_MissionController2_1000666 = Apollo-Duna 8: Land Duna Expedition 1 On Duna.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000667") +		// #autoLOC_MissionController2_1000667 = It’s time to deliver the first crew to the Duna Colony module.  We will launch at the very next available launch window.  Any last minute adjustments that need to be made to lander 
                    Localizer.Format("#autoLOC_MissionController2_1000668");		// #autoLOC_MissionController2_1000668 = should be done now.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000669");		// #autoLOC_MissionController2_1000669 = Launch 3 crew members to Duna and land them at your Colony Module.  They will stay for at least 6 months on Planet.
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000670") +		// #autoLOC_MissionController2_1000670 = Great job, the World is celebrating the Successful return of our Astronaut’s.  These first pioneers on Duna have become Hero’s among the civilian population.  It’s time to concentrate on the 
                    Localizer.Format("#autoLOC_MissionController2_1000671"); 		// #autoLOC_MissionController2_1000671 = 2nd Rotation crew.
                    break;
                case 8:
                    apdHash ="Duna 9";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000672");		// #autoLOC_MissionController2_1000672 = Apollo-Duna 9: Land Duna Expedition 2 On Duna.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000673") +		// #autoLOC_MissionController2_1000673 = Talk of a replacement program for Apollo has spread throughout the Kerbal Space Center.  Apollo has become too expensive! We can do the same thing cheaper.  But for now Apollo-Duna 9 is still 
                    Localizer.Format("#autoLOC_MissionController2_1000674") +		// #autoLOC_MissionController2_1000674 = on the table.  This could be the last great mission for the Apollo program!\n\n
                    Localizer.Format("#autoLOC_MissionController2_1000675");		// #autoLOC_MissionController2_1000675 = We are tasked with bringing the 2nd Rotation crew to duna.  We have done this before, and can do it again.
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000676");		// #autoLOC_MissionController2_1000676 = Land the 2nd rotation crew for its stay on Duna.  Conduct more science experiments and then return home safely. 
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000677") +		// #autoLOC_MissionController2_1000677 = The official word has come in Apollo is to be retired.  But first Apollo will be used in one more important mission.  Eve.  Eve has a very similar delta V profile as Duna.  We want you to set 
                    Localizer.Format("#autoLOC_MissionController2_1000678");		// #autoLOC_MissionController2_1000678 = up a new mission for an Eve Orbit.  More information will be released in a few days.  For now good job on the last mission to Duna.  Apollo has come a long way!
                    break;
                case 9:
                    apdHash ="Eve Last Flight Apollo";
                    apTitle = Localizer.Format("#autoLOC_MissionController2_1000679");		// #autoLOC_MissionController2_1000679 = Apollo-Eve: The last Flight of Apollo.
                    apDescription = Localizer.Format("#autoLOC_MissionController2_1000680") +		// #autoLOC_MissionController2_1000680 = The last flight of Apollo:  Apollo will go down as one of the most successful space craft in Kerbal history.  But it’s time is at an end.  It has served us well, but talk of the new Constellation 
                    Localizer.Format("#autoLOC_MissionController2_1000681") +		// #autoLOC_MissionController2_1000681 = program has hit the Center.  But before that day comes we have one more milestone for Apollo.  Eve!
                    Localizer.Format("#autoLOC_MissionController2_1000682") +		// #autoLOC_MissionController2_1000682 = This is a standard Orbit and observe mission.  We are to bring a crew of 3 using the existing Apollo space craft to EVE.  Eve is way too much for a lander.  Takes more Delta-V to get off Eve then 
                    Localizer.Format("#autoLOC_MissionController2_1000683");		// #autoLOC_MissionController2_1000683 = Kerbin!  So for now we will orbit and take scientific measurements.  If you wish you may land on its moon.  But other than that don’t attempt to land of Eve with a Manned Vessel!
                    apsynops = Localizer.Format("#autoLOC_MissionController2_1000684") +		// #autoLOC_MissionController2_1000684 = Bring a 3 man capsule and any support craft you need into orbit of Eve.  Conduct scientific research of Eve and it’s moon.  The rest of Apollo Money has been applied to this mission.  
                    Localizer.Format("#autoLOC_MissionController2_1000685");		// #autoLOC_MissionController2_1000685 = It pays well.  We will leave the science up to you!  After completion bring the crew home safely.
                    apCompletMessage = Localizer.Format("#autoLOC_MissionController2_1000686") +		// #autoLOC_MissionController2_1000686 = The day has come, the last flight of Apollo.  You have proven that kerbals can do anything if they work hard and take risk.  The Apollo program was a huge risk.  From the first Mun landing to 
                    Localizer.Format("#autoLOC_MissionController2_1000687") +		// #autoLOC_MissionController2_1000687 = Duna, and now EVE.  Apollo has proven itself over and over, and will continue to aspire young kerbals for Generations!  You should be proud of your Astronaut’s, and your ships.  You have done well.\n\n 
                    Localizer.Format("#autoLOC_MissionController2_1000688") + 		// #autoLOC_MissionController2_1000688 = It’s time to take the gloves off and create your own story.  Your own hero’s… And continue the adventure, where ever that might take you. \n\n
                    Localizer.Format("#autoLOC_MissionController2_1000689");		// #autoLOC_MissionController2_1000689 = Good luck.
                    break;
            }
        }
        
        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<ApolloDunaProgram>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }            
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().ApolloExtraContent)
            {
                return false;
            }
            ApolloDunaMissionNumber = SaveInfo.apolloDunaCurrentNumber;
            apdMessageSelection();
            targetBody = FlightGlobals.Bodies[2];
            targetBody2 = FlightGlobals.Bodies[1];
            targetBody3 = FlightGlobals.Bodies[6];
            targetBody4 = FlightGlobals.Bodies[5];
            if (ApolloDunaMissionNumber == 9)
            {
                this.AddParameter(new InOrbitGoal(targetBody4), null);
                this.AddParameter(new CollectScience(targetBody4, BodyLocation.Surface), null);
                this.AddParameter(new GetCrewCount(crewCount), null);
                this.AddParameter(new LandingParameters(targetBody2, true), null);
                base.SetFunds(30000f, 350000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(20f, targetBody);
                base.SetScience(3f, targetBody);
            }

            if (ApolloDunaMissionNumber == 8)
            {
                this.AddParameter(new InOrbitGoal(targetBody3), null);
                this.AddParameter(new CheckLandingLonAndLat(targetBody3, false, SaveInfo.apolloLandingLon, SaveInfo.apolloLandingLat, landingTitle,false), null);
                this.AddParameter(new CollectScience(targetBody3, BodyLocation.Surface), null);
                this.AddParameter(new GetCrewCount(crewCount), null);
                this.AddParameter(new TimeCountdownLanding(targetBody3, 648000, "Crew must stay for this amount of time",false), null);               
                this.AddParameter(new LandingParameters(targetBody2, true), null);
                base.SetFunds(30000f, 400000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(4f, targetBody);
                base.SetScience(1f, targetBody);
            }
            if (ApolloDunaMissionNumber == 7)
            {
                this.AddParameter(new InOrbitGoal(targetBody3), null);
                this.AddParameter(new CheckLandingLonAndLat(targetBody3, false, SaveInfo.apolloLandingLon, SaveInfo.apolloLandingLat, landingTitle,true), null);
                this.AddParameter(new CollectScience(targetBody3, BodyLocation.Surface), null);
                this.AddParameter(new GetCrewCount(crewCount), null);
                this.AddParameter(new TimeCountdownLanding(targetBody3, 648000, "Crew must stay for this amount of time",true), null);               
                this.AddParameter(new LandingParameters(targetBody2, true), null);
                base.SetFunds(30000f, 400000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(4f, targetBody);
                base.SetScience(1f, targetBody);
            }
            
            if (ApolloDunaMissionNumber == 6)
            {
                this.AddParameter(new InOrbitGoal(targetBody3), null);
                this.AddParameter(new MaxSeatCount(maxSeatCountShip), null);
                this.AddParameter(new GetCrewCount(0), null);
                base.SetFunds(25000f, 300000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(25f, targetBody);
                base.SetScience(5f, targetBody);
            }
            if (ApolloDunaMissionNumber == 5)
            {
                this.AddParameter(new InOrbitGoal(targetBody3), null);               
                this.AddParameter(new CollectScience(targetBody3, BodyLocation.Space), null);
                this.AddParameter(new GetCrewCount(crewCount), null);
                this.AddParameter(new LandingParameters(targetBody2, true), null);
                base.SetFunds(30000f, 300000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(20f, targetBody);
                base.SetScience(3f, targetBody);
            }
            if (ApolloDunaMissionNumber == 4)
            {
                this.AddParameter(new InOrbitGoal(targetBody), null);
                this.AddParameter(new CheckLandingLonAndLat(targetBody, false, SaveInfo.apolloLandingLon, SaveInfo.apolloLandingLat, landingTitle,false), null);
                this.AddParameter(new CollectScience(targetBody, BodyLocation.Surface), null);
                this.AddParameter(new GetCrewCount(crewCount), null);
                this.AddParameter(new TimeCountdownLanding(targetBody, 648000, "Crew must stay for this amount of time",false), null);               
                this.AddParameter(new LandingParameters(targetBody2, true), null);
                base.SetFunds(30000f, 150000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(4f, targetBody);
                base.SetScience(1f, targetBody);
            }
            if (ApolloDunaMissionNumber == 3)
            {
                this.AddParameter(new InOrbitGoal(targetBody), null);
                this.AddParameter(new CheckLandingLonAndLat(targetBody, false, SaveInfo.apolloLandingLon, SaveInfo.apolloLandingLat, landingTitle,false), null);
                this.AddParameter(new CollectScience(targetBody, BodyLocation.Surface), null);
                this.AddParameter(new GetCrewCount(crewCount), null);
                this.AddParameter(new TimeCountdownLanding(targetBody, 648000, "Crew must stay for this amount of time",false), null);                
                this.AddParameter(new LandingParameters(targetBody2, true), null);
                base.SetFunds(30000f, 150000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(4f, targetBody);
                base.SetScience(1f, targetBody);
            }
            if (ApolloDunaMissionNumber == 2)
            {
                
                this.AddParameter(new InOrbitGoal(targetBody), null);
                this.AddParameter(new CheckLandingLonAndLat(targetBody, false, SaveInfo.apolloLandingLon, SaveInfo.apolloLandingLat, landingTitle,true), null);
                this.AddParameter(new GetCrewCount(0), null);                
                base.SetFunds(30000f, 190000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(4f, targetBody);
                base.SetScience(1f, targetBody);
            }
            if (ApolloDunaMissionNumber == 1)
            {
                this.AddParameter(new InOrbitGoal(targetBody), null);
                this.AddParameter(new BiomLandingParameters(targetBody, false, ApolloDunaBiome), null);
                this.AddParameter(new MaxSeatCount(maxSeatCountShip), null);
                this.AddParameter(new GetCrewCount(0), null);               
                base.SetFunds(25000f, 200000f, targetBody);
                base.SetExpiry(3f, 10f);
                base.SetDeadlineYears(6, targetBody);
                base.SetReputation(25f, targetBody);
                base.SetScience(5f, targetBody);
            }
            return true;

        }

        protected override void OnAccepted()
        {
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
            return apdHash + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return apTitle;
        }
        protected override string GetDescription()
        {
            return apDescription;
        }
        protected override string GetSynopsys()
        {
            return apsynops;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.apolloDunaCurrentNumber++;
            return apCompletMessage;
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref apdHash, "Hash String Not Loaded", apdHash, "apdhash");
            Tools.ContractLoadCheck(node, ref apTitle, "Title Not Loaded", apTitle, "apdtitle");
            Tools.ContractLoadCheck(node, ref apDescription, "Description Not Loaded", apDescription, "apddescript");
            Tools.ContractLoadCheck(node, ref apsynops, "Synops Not Loaded", apsynops, "apdsynop");
            Tools.ContractLoadCheck(node, ref apCompletMessage, "Message Complete Not Loaded", apCompletMessage, "apdmessagecomplete");
            Tools.ContractLoadCheck(node, ref targetBody, FlightGlobals.Bodies[2], targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref targetBody2, FlightGlobals.Bodies[1], targetBody2, "targetBody2");
            Tools.ContractLoadCheck(node, ref targetBody3, FlightGlobals.Bodies[3], targetBody3, "targetBody3");
            Tools.ContractLoadCheck(node, ref targetBody4, FlightGlobals.Bodies[5], targetBody4, "targetbody4");
            Tools.ContractLoadCheck(node, ref ApolloDunaBiome, "None", ApolloDunaBiome, "apbiome");            
            Tools.ContractLoadCheck(node, ref ApolloDunaMissionNumber, SaveInfo.apolloCurrentNumber, ApolloDunaMissionNumber, "apnumber");           
            Tools.ContractLoadCheck(node, ref crewCount, 3, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref maxSeatCountShip, 4, maxSeatCountShip, "maxseats");           
            Tools.ContractLoadCheck(node, ref landingTitle, "Land your vessel near the Colony Habitat", landingTitle, "landingtitle");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            int bodyID2 = targetBody2.flightGlobalsIndex;
            node.AddValue("targetBody2", bodyID2);
            int bodyID3 = targetBody3.flightGlobalsIndex;
            node.AddValue("targetBody3", bodyID3);
            int bodyID4 = targetBody4.flightGlobalsIndex;
            node.AddValue("targetbody4", bodyID4);
            node.AddValue("apbiome", ApolloDunaBiome);          
            node.AddValue("apnumber", ApolloDunaMissionNumber);          
            node.AddValue("crewcount", crewCount);
            node.AddValue("maxseats", maxSeatCountShip);            
            node.AddValue("landingtitle", landingTitle);
            
            node.AddValue("apdhash", apdHash);
            node.AddValue("apdtitle", apTitle);
            node.AddValue("apddescript", apDescription);
            node.AddValue("apdsynop", apsynops);
            node.AddValue("apdmessagecomplete", apCompletMessage);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("fieldScience") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("advLanding") == RDTech.State.Available;
            bool techUnlock3 = ResearchAndDevelopment.GetTechnologyState("heavierRocketry") == RDTech.State.Available;           
            if (techUnlock && techUnlock2 && techUnlock3 && ApolloDunaMissionNumber <= 5 && SaveInfo.apolloCurrentNumber > 6 && SaveInfo.skylab4done == true)
            {
                return true;
            }
            else if (techUnlock && techUnlock2 && techUnlock3 && ApolloDunaMissionNumber <= 8 && SaveInfo.apolloCurrentNumber > 6 && SaveInfo.skylab4done == true && SaveInfo.apolloDunaStation)
            {
                return true;
            }
            else return false;

        }
    }
    public class ApolloDunaStation : Contract
    {       
        public int crewCount = 0;

        CelestialBody targetBody = FlightGlobals.Bodies[6];

        public int totalContracts;
        public int TotalFinished;

        protected override bool Generate()
        {
            totalContracts = ContractSystem.Instance.GetCurrentContracts<ApolloDunaStation>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<ApolloDunaStation>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }           
            totalContracts = ContractSystem.Instance.GetCurrentContracts<ApolloDunaStation>().Count();
            if (totalContracts >= 1) { return false; }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().ApolloExtraContent)
            {
                return false;
            }

            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new GetCrewCount(0), null);
            base.SetFunds(30000f, 75000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(6, targetBody);
            base.SetReputation(20f, targetBody);
            base.SetScience(3f, targetBody);
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

        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000599");		// #autoLOC_MissionController2_1000599 = Establish a space station over Duna
        }

        protected override string GetHashString()
        {
            return "Duna 7" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000600");		// #autoLOC_MissionController2_1000600 = Apollo-Duna 7: Establish a resupply station above Duna
        }
        protected override string GetDescription()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000601") +		// #autoLOC_MissionController2_1000601 = Duna Supply Station:  Mission planners fear complications with the Duna missions.  So they have devised a plan to establish a small space station above Duna that will contain any 
                    Localizer.Format("#autoLOC_MissionController2_1000602");		// #autoLOC_MissionController2_1000602 = emergency supplies the Apollo-Duna mission might need.  This is a great opportunity to build a space station above Duna!
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000603");		// #autoLOC_MissionController2_1000603 = Construct a small Space Station that can be used as an emergency supply station for the Apollo-Duna Missions.  What you build on this station is up to you.
        }
        protected override string MessageCompleted()
        {
            SaveInfo.apolloDunaStation = true;
            return Localizer.Format("#autoLOC_MissionController2_1000604");		// #autoLOC_MissionController2_1000604 = Mission planners and Management are a little more confident now about the mission. With this new station over Duna, any emergencies that develop will be dealt with.
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

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewcount", crewCount);                 
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.apolloCurrentNumber > 6 && SaveInfo.apolloDunaCurrentNumber >= 5 && SaveInfo.apolloDunaStation == false)
            {
                return true;
            }
            else { return false; }
        }
    }
    # region Agena Contract 1
    public class AgenaTargetPracticeContract : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double ApA = 0;
        public double PeA = 0;
        public int crewCount = 0;
        public string partName = "ModuleDockingNode";
        public string ModuleTitle = "Any Docking Port";
        public int totalContracts;
        public int TotalFinished;
        public int Agena1Done;

        ContractParameter AgenaParameter;

        protected override bool Generate()
        {          
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AgenaTargetPracticeContract>().Count();
            Agena1Done = ContractSystem.Instance.GetCompletedContracts<AgenaTargetPracticeContract>().Count();

            //Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished);
            if (Agena1Done == 1 || SaveInfo.Agena1Done)
            {
                return false;
            }
            if (totalContracts >= 1)
            {
                //Debug.Log("contract is generated right now terminating Normal Satellite Mission");
                //Debug.Log("count is " + totalContracts);
                return false;
            }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            targetBody = Planetarium.fetch.Home;
            ApA = Tools.ReturnMinOrbit(targetBody, 1.2f);
            PeA = ApA - 1000;

            AgenaParameter = this.AddParameter(new AgenaInOrbit(targetBody), null);
            AgenaParameter.SetFunds(2000.0f, targetBody);
            AgenaParameter.SetReputation(20f, targetBody);
            this.AddParameter(new ApAOrbitGoal(targetBody, (double)ApA,"Orbit"), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)PeA,"Orbit"), null);
            this.AddParameter(new ModuleGoal(partName, ModuleTitle), null);
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(15f, 35f);
            base.SetScience(25f, targetBody);
            base.SetDeadlineDays(19f, targetBody);
            base.SetReputation(35f, 35f, targetBody);
            base.SetFunds(28000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 54000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 39000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

            return true;
        }

        protected override void OnAccepted()
        {
            string AgenaMessage = Localizer.Format("#autoLOC_MissionController2_1000484") +		// #autoLOC_MissionController2_1000484 = Please take note that when you finish the Agena contract, that vessel will be recorded as the Agena Vessel for the next mission!\n\n
                Localizer.Format("#autoLOC_MissionController2_1000486");		// #autoLOC_MissionController2_1000486 = If the wrong vessel is recorded, you can change the vessel by using the Debug Tools in settings for Agena Contract.
            MessageSystem.Message m = new MessageSystem.Message(Localizer.Format("#autoLOC_MissionController2_1000487"), AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);		// #autoLOC_MissionController2_1000487 = Important Agena Target Contract Information
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

        protected override string GetHashString()
        {
            return targetBody.bodyName + ApA.ToString() + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000488");		// #autoLOC_MissionController2_1000488 = Agena Target Vehicle Orbital Test Around Kerbin - Launch Agena Vehicle
        }
        protected override string GetDescription()
        {

            return Localizer.Format("#autoLOC_MissionController2_1000489") +		// #autoLOC_MissionController2_1000489 = The Agena Target Vehicle (ATV) was an unmanned spacecraft used by NASA during its Gemini program to develop and practice orbital space rendezvous and docking techniques and\n
                Localizer.Format("#autoLOC_MissionController2_1000490") +		// #autoLOC_MissionController2_1000490 = to perform large orbital changes, in preparation for the Apollo Program's lunar missions.\n\n
                Localizer.Format("#autoLOC_MissionController2_1000491") +		// #autoLOC_MissionController2_1000491 = Your first task is to launch an Agena Type vehicle into orbit.\n\n
                Localizer.Format("#autoLOC_MissionController2_1000492");		// #autoLOC_MissionController2_1000492 = Please take note that when you finish the Agena Contract, that vessel will be recorded as the Agena Vessel for the next mission!
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000493") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000493 = Agena Test 
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Agena1Done = true;
            SaveInfo.AgenaTargetVesselID = FlightGlobals.ActiveVessel.id.ToString();
            SaveInfo.AgenaTargetVesselName = FlightGlobals.ActiveVessel.vesselName.Replace("(unloaded)", "");
            return FlightGlobals.ActiveVessel.name + " Vessel ID is " + FlightGlobals.ActiveVessel.id + "\n\n" +
                Localizer.Format("#autoLOC_MissionController2_1000494") +		// #autoLOC_MissionController2_1000494 = If this is not correct you can change this is Debug menu using select Agena vessel Debug Tool\n\n
                Localizer.Format("#autoLOC_MissionController2_1000495") +		// #autoLOC_MissionController2_1000495 = Congradulations you have succesfully launched your Agena Target Vehicle, now you must get you Manned Orbital vehicle to Dock with the ATV\n\n
                Localizer.Format("#autoLOC_MissionController2_1000496") +		// #autoLOC_MissionController2_1000496 = The Gemini-Agena Target Vehicle design was an adaptation of the basic Agena-D vehicle using the alternate Model 8247 rocket engine and additional program-peculiar equipment required for the Gemini mission.\n\n
                Localizer.Format("#autoLOC_MissionController2_1000497") +		// #autoLOC_MissionController2_1000497 = This GATV was divided into:\n\n

Localizer.Format("#autoLOC_MissionController2_1000498") +		// #autoLOC_MissionController2_1000498 = The program-peculiar forward auxiliary section. This section consisted of the auxiliary equipment rack, the McDonnell Aircraft Company-furnished docking-adapter module, and the clamshell nose shroud.\n
Localizer.Format("#autoLOC_MissionController2_1000499") +		// #autoLOC_MissionController2_1000499 = The Agena-D forward and mid-body sections. The Agena-D forward section housed the main equipment bay, and the mid-body contained the main fuel and oxidizer tanks which supplied propellants through a feed and\n
Localizer.Format("#autoLOC_MissionController2_1000500") +		// #autoLOC_MissionController2_1000500 = load system for the main engine. (3) the program-peculiar aft section. The Model 8247 multi-start main engine and the smaller Model 8250 maneuvering and ullage orientation engines were located in this section.\n
Localizer.Format("#autoLOC_MissionController2_1000501") +		// #autoLOC_MissionController2_1000501 = Orbital length of the GATV was approximately 26 feet. Vehicle weight-on-orbit was approximately 7200 lb. This weight included propellants still remaining in the main tanks and available for Model 8247 engine operation\n
Localizer.Format("#autoLOC_MissionController2_1000502") +		// #autoLOC_MissionController2_1000502 = after the Agena achieved orbit.\n\n
Localizer.Format("#autoLOC_MissionController2_1000503") +		// #autoLOC_MissionController2_1000503 = The Gemini-ATV propulsion system consisted of the following:\n\n

Localizer.Format("#autoLOC_MissionController2_1000504") +		// #autoLOC_MissionController2_1000504 = Model 8247 rocket engine, also known as XLR-81-BA-13, and its controls, mount, gimbals, and titanium nozzle extension\n
Localizer.Format("#autoLOC_MissionController2_1000505") +		// #autoLOC_MissionController2_1000505 = Pyrotechnically operated helium-control valve (POHCV) and associated pressurization plumbing\n
Localizer.Format("#autoLOC_MissionController2_1000506") +		// #autoLOC_MissionController2_1000506 = Fuel and oxidizer feed and load system, including propellant tanks, vents, and fill quick disconnects\n
Localizer.Format("#autoLOC_MissionController2_1000507") +		// #autoLOC_MissionController2_1000507 = Propellant isolation valves (PIV's)\n
Localizer.Format("#autoLOC_MissionController2_1000508") +		// #autoLOC_MissionController2_1000508 = All associated pyro devices and solid-propellant rockets.\n\n
Localizer.Format("#autoLOC_MissionController2_1000509");		// #autoLOC_MissionController2_1000509 = All Information For Agena was Gathered From www.astronautix.com
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            double maxApa = double.Parse(node.GetValue("maxaPa"));
            ApA = maxApa;
            
            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            PeA = masxPpaID;
            
            ModuleTitle = node.GetValue("moduletitle");
            partName = node.GetValue("pName");
            crewCount = int.Parse(node.GetValue("crewcount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            double maxApa = ApA;
            node.AddValue("maxaPa", ApA);
            
            double maxPpAID = PeA;
            node.AddValue("maxpEa", PeA);
            
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("moduletitle", ModuleTitle);

            node.AddValue("crewcount", crewCount);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (!techUnlock)
            {
                return false;
            }
            else if (SaveInfo.Voskhod2Done == true) { return true; }
            else
                return false;
        }


    }
    #endregion
    # region Agena Contract 2
    public class AgenaTargetPracticeContractDocking : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public int crewCount = 1;
        public int partAmount = 1;
        public string partName = "ModuleDockingNode";
        public string ModuleTitle = "Any Docking Port";
        public double ApA = 0;
        public double PeA = 0;
        public string vesselTestID = "none";
        public string vesselTestName = "none";
        public int totalContracts;
        public int Agena1Done;
        public int Agena2Done;
        ContractParameter AgenaDockParameter;


        protected override bool Generate()
        {          
            totalContracts = ContractSystem.Instance.GetCurrentContracts<AgenaTargetPracticeContractDocking>().Count();
            Agena1Done = ContractSystem.Instance.GetCompletedContracts<AgenaTargetPracticeContract>().Count();
            Agena2Done = ContractSystem.Instance.GetCompletedContracts<AgenaTargetPracticeContractDocking>().Count();

            //Debug.Log("Satellite Delivery Totalcontracts " + totalContracts + " - " + " Total Finsihed " + TotalFinished)
            if (Agena1Done != 1 || Agena2Done == 1 || SaveInfo.Agena2Done)
            {
                //Debug.Log("Agena 1 Is not Done Yet, rejecting Contract 2 Docking");
                return false;
            }
            if (totalContracts >= 1)
            {
                //Debug.Log("Agena 2 is already loaded.");
                return false;
            }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            targetBody = Planetarium.fetch.Home;
            ApA = Tools.ReturnMinOrbit(targetBody, 1.6f);
            PeA = ApA - 1000;

            vesselTestID = SaveInfo.AgenaTargetVesselID;
            vesselTestName = SaveInfo.AgenaTargetVesselName;
            AgenaDockParameter = this.AddParameter(new TargetDockingGoal(vesselTestID, vesselTestName), null);
            AgenaDockParameter.SetFunds(3000.0f, targetBody);
            AgenaDockParameter.SetReputation(30f, targetBody);

            this.AddParameter(new ApAOrbitGoal(targetBody, (double)ApA,true, "Orbit"), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)PeA,true, "Orbit"), null);
            this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody), null);
            this.AddParameter(new ModuleGoal(partName, "KopernicusSolarPanel", ModuleTitle), null);
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(15f, 35f);
            base.SetScience(25f, targetBody);
            base.SetDeadlineDays(20f, targetBody);
            base.SetReputation(50f, 35f, targetBody);
            base.SetFunds(29000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 48000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 42000f * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            return targetBody.bodyName + vesselTestName + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000510") + " " + vesselTestName;		// #autoLOC_MissionController2_1000510 = Agena Target Vehicle Orbital Test Around Kerbin - Dock With ATV 
        }
        protected override string GetDescription()
        {

            return Localizer.Format("#autoLOC_MissionController2_1000511") +		// #autoLOC_MissionController2_1000511 = Project Gemini was the second human spaceflight program of NASA, the civilian space agency of the United States government. Project Gemini was conducted between projects Mercury\n
                Localizer.Format("#autoLOC_MissionController2_1000512") +		// #autoLOC_MissionController2_1000512 =  and Apollo, with ten manned flights occurring in 1965 and 1966.\n\n

                 Localizer.Format("#autoLOC_MissionController2_1000513") +		// #autoLOC_MissionController2_1000513 = Its objective was to develop space travel techniques in support of Apollo, which had the goal of landing men on the Moon. Gemini achieved missions long enough for a trip to the Moon\n
                 Localizer.Format("#autoLOC_MissionController2_1000514") +		// #autoLOC_MissionController2_1000514 = and back, perfected extra-vehicular activity (working outside a spacecraft), and orbital maneuvers necessary to achieve rendezvous and docking. All Gemini flights were launched from \n
                 Localizer.Format("#autoLOC_MissionController2_1000515") +		// #autoLOC_MissionController2_1000515 =  Cape Canaveral, Florida using the Titan II Gemini launch vehicle\n\n
                 Localizer.Format("#autoLOC_MissionController2_1000516") +		// #autoLOC_MissionController2_1000516 = Info For Gemini From Wikipedia.org\n\n
                Localizer.Format("#autoLOC_MissionController2_1000517");		// #autoLOC_MissionController2_1000517 = Your Second Task Is To Dock your Manned Orbital Pod with Agena Target Vehicle.  Then you are required to change Altitude to the selected ApA and PeA
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000518") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000518 = Agena Test 
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Agena2Done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000519") +		// #autoLOC_MissionController2_1000519 = You have been succesful with Launching an Agena Type craft, docking with it, and changing your Orbital Altitude.  Congradulations!\n\n
                Localizer.Format("#autoLOC_MissionController2_1000520") +		// #autoLOC_MissionController2_1000520 = The first GATV was launched on October 25, 1965 while the Gemini 6 astronauts were waiting on the pad. While the Atlas performed normally,\n
                Localizer.Format("#autoLOC_MissionController2_1000521") +		// #autoLOC_MissionController2_1000521 = the Agena's engine exploded during orbital injection. Since the rendezvous and docking was the primary objective, the Gemini 6 mission was scrubbed,\n
                Localizer.Format("#autoLOC_MissionController2_1000522") +		// #autoLOC_MissionController2_1000522 = and replaced with the alternate mission Gemini 6A, which rendezvoused (but could not dock) with Gemini 7 in December.\n\n
                Localizer.Format("#autoLOC_MissionController2_1000523");		// #autoLOC_MissionController2_1000523 = It was not until Gemini 10 that all objectives of Launching, Docking, and boosting Gemini 10 to 412-nautical-mile change succeded.
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            double maxApa = double.Parse(node.GetValue("maxaPa"));
            ApA = maxApa;
            
            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            PeA = masxPpaID;
            
            int pcount = int.Parse(node.GetValue("pCount"));
            partAmount = pcount;
            partName = (node.GetValue("pName"));
            ModuleTitle = node.GetValue("mtitle");
            crewCount = int.Parse(node.GetValue("crewcount"));

            vesselTestID = node.GetValue("vesselid");
            vesselTestName = node.GetValue("vesselname");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            double maxApa = ApA;
            node.AddValue("maxaPa", ApA);
           
            double maxPpAID = PeA;
            node.AddValue("maxpEa", PeA);
            
            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("mtitle", ModuleTitle);

            node.AddValue("crewcount", crewCount);

            node.AddValue("vesselid", vesselTestID);
            node.AddValue("vesselname", vesselTestName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (!techUnlock)
                return false;
            else
                return true;
        }


    }
    #endregion
    # region SkyLab 1
    public class SkyLab1 : Contract
    {

        Settings settings = new Settings("config.cfg");
        public double minHeight = 90000;

        public string part1goal = "Repair Panel";
        public string part2goal = "PPD-10 Hitchhiker Storage Container";
        public string part3goal = "OX-4L 1x6 Photovoltaic Panels";

        public int part1amount = 1;
        public int part2amount = 1;
        public int part3amount = 1;

        public int crewCount = 0;

        CelestialBody targetBody = Planetarium.fetch.Home;

        public void getVesselIDName(Vessel vs)
        {
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
            {
                SaveInfo.skyLabName = vs.vesselName.Replace("(unloaded)", "");
                SaveInfo.skyLabVesID = vs.id.ToString();
                ScreenMessages.PostScreenMessage("" + vs.name.Replace("(unloaded)", "") + " was added to skylab contract vessel.  It's ID is " + vs.id.ToString());
            }
            else
                Debug.Log("error in adding vessel id and name to save file for SkyLab 1 Contract");
        }
        public int totalContracts;

        protected override bool Generate()
        {           
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab1>().Count();
            if (totalContracts >= 1) { return false; }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            minHeight = Tools.ReturnMinOrbit(targetBody, 1.2f);
            this.AddParameter(new AltitudeGoal(targetBody, minHeight), null);
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new PartGoal(part1goal, "Small Repair Panel", part1amount, true), null);
            this.AddParameter(new PartGoal(part2goal, part2amount, false), null);
            this.AddParameter(new PartGoal(part3goal, part3amount, false), null);
            this.AddParameter(new GetCrewCount(crewCount), null);
            base.SetFunds(25000f, 150000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(100f, targetBody);
            base.SetReputation(35f, targetBody);
            base.SetScience(10f, targetBody);
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

        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000524");		// #autoLOC_MissionController2_1000524 = Our task is to launch Skylab and deploy it in orbit.  Later we will send a crew to man the station.
        }

        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000525") + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000525 = Launch and deploy skylab into orbit
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000526") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000526 = Launch and deployment of skylab over 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000527") +		// #autoLOC_MissionController2_1000527 = Skylab was a space station launched and operated by NASA and was the United States' first space station. Skylab orbited the Earth from 1973 to 1979, and included a workshop, 
                Localizer.Format("#autoLOC_MissionController2_1000528") +		// #autoLOC_MissionController2_1000528 = a solar observatory, and other systems. It was launched unmanned by a modified Saturn V rocket, with a weight of 169,950 pounds (77 t).  Three manned missions to the station, 
                Localizer.Format("#autoLOC_MissionController2_1000529") +		// #autoLOC_MissionController2_1000529 = conducted between 1973 and 1974 using the Apollo Command/Service Module (CSM) atop the smaller Saturn IB, each delivered a three-astronaut crew. On the last two manned missions, 
                Localizer.Format("#autoLOC_MissionController2_1000530") +		// #autoLOC_MissionController2_1000530 = an additional Apollo / Saturn IB stood by ready to rescue the crew in orbit if it was needed.\n\n
                Localizer.Format("#autoLOC_MissionController2_1000531") +		// #autoLOC_MissionController2_1000531 = Our Goals \n\n 1. Launch skylab to space with specified equipment \n2. deploy skylab in desired orbit and await the next mission crew launch \n\n
                Localizer.Format("#autoLOC_MissionController2_1000532") +		// #autoLOC_MissionController2_1000532 = Please note that at the end of contract your active vessel will be recorded as the SkyLab Station for all the next SkyLab Contracts!\n\n
                Localizer.Format("#autoLOC_MissionController2_1000533");		// #autoLOC_MissionController2_1000533 = Information on SkyLab was obtained from Wikipidia.
        }
        protected override string GetSynopsys()
        {
            return "";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab1done = true;
            getVesselIDName(FlightGlobals.ActiveVessel);
            return Localizer.Format("#autoLOC_MissionController2_1000534") +		// #autoLOC_MissionController2_1000534 = The station was damaged during launch when the micrometeoroid shield separated from the workshop and tore away, taking one of two main solar panel arrays with it and jamming 
                Localizer.Format("#autoLOC_MissionController2_1000535") +		// #autoLOC_MissionController2_1000535 = the other one so that it could not deploy. This deprived Skylab of most of its electrical power, and also removed protection from intense solar heating, threatening to make it unusable. \n\n
                Localizer.Format("#autoLOC_MissionController2_1000536") +		// #autoLOC_MissionController2_1000536 = Our first mission to skylab will be to repair the station.  After we will have the crew conduct scientific studies as planned\n\n
                Localizer.Format("#autoLOC_MissionController2_1000537") + " " + FlightGlobals.ActiveVessel.name + "\n" +		// #autoLOC_MissionController2_1000537 = Current vessel selected as SkyLab Base is 
                FlightGlobals.ActiveVessel.vesselName.Replace("(unloaded)", "") + Localizer.Format("#autoLOC_MissionController2_1000538") + " " + FlightGlobals.ActiveVessel.id + "\n\n" +		// #autoLOC_MissionController2_1000538 =  Vessel ID is 
                Localizer.Format("#autoLOC_MissionController2_1000539");		// #autoLOC_MissionController2_1000539 = If this is not correct you can change this is Debug menu using select SkyLab vessel Debug Tool
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            minHeight = double.Parse(node.GetValue("minheight"));
            crewCount = int.Parse(node.GetValue("crewcount"));

            part1amount = int.Parse(node.GetValue("part1amount"));
            part2amount = int.Parse(node.GetValue("part2amount"));
            part3amount = int.Parse(node.GetValue("part3amount"));

            part1goal = node.GetValue("part1goal");
            part2goal = node.GetValue("part2goal");
            part3goal = node.GetValue("part3goal");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("minheight", minHeight);
            node.AddValue("crewcount", crewCount);

            node.AddValue("part1amount", part1amount);
            node.AddValue("part2amount", part2amount);
            node.AddValue("part3amount", part3amount);

            node.AddValue("part1goal", part1goal);
            node.AddValue("part2goal", part2goal);
            node.AddValue("part3goal", part3goal);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("largeElectrics") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;

            if (SaveInfo.skylab1done == true || SaveInfo.Agena2Done == false) { return false; }
            if (SaveInfo.apolloCurrentNumber <= 6) { return false;}
            if (!techUnlock && !techUnlock2) { return false; }   
            else { return true; }
        }
    }
    #endregion
    # region SkyLab 2
    public class SkyLab2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 3;
        public int contractTime = 605448;
        public string contractName = "Launch And Repair " + SaveInfo.skyLabName;
        public string contTimeTitle = " Dock and stay in orbit ";
        string Ctitle = "To Repair Station You must have at Least ";
        double RPamount = 1;
        string vesselId = "none";
        string vesselName = "none";
        CelestialBody targetBody = Planetarium.fetch.Home;
        ContractParameter skylab1;
        ContractParameter skylab2;
        ContractParameter skylab3;
        ContractParameter skylab4;
        ContractParameter skylab5;

        public int totalContracts;

        protected override bool Generate()
        {        
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab2>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }

            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }

            vesselId = SaveInfo.skyLabVesID;
            vesselName = SaveInfo.skyLabName;
            this.skylab1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            skylab1.SetFunds(7000, targetBody);
            skylab1.SetReputation(5, targetBody);

            this.skylab2 = this.AddParameter(new RepairPanelPartCheck(contractName, SaveInfo.skyLabVesID, SaveInfo.skyLabName), null);
            skylab2.SetFunds(8000, targetBody);
            skylab2.SetReputation(7, targetBody);
            skylab2.SetScience(10, targetBody);

            this.skylab3 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            skylab3.SetFunds(8500, targetBody);
            skylab3.SetReputation(10, targetBody);
            skylab3.SetScience(10, targetBody);

            this.skylab4 = this.AddParameter(new TimeCountdownDocking(targetBody, contractTime, contTimeTitle, vesselId, vesselName), null);
            skylab4.SetFunds(20000, targetBody);
            skylab4.SetReputation(25, targetBody);
            skylab4.SetScience(25, targetBody);

            this.skylab5 = this.AddParameter(new Contracts.Parameters.LandOnBody(targetBody), null);
            skylab5.SetFunds(5000, 75000, targetBody);
            skylab5.SetReputation(20, targetBody);
            skylab5.SetScience(3, targetBody);

            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(25000f, 150000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(700f, targetBody);
            base.SetReputation(45f, targetBody);
            base.SetScience(75f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of SL-2 and Repair Mission" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000540") + " " + SaveInfo.skyLabName;		// #autoLOC_MissionController2_1000540 = Launch SL-2 To (SkyLab) 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000541") + " " + SaveInfo.skyLabName + " " + Localizer.Format("#autoLOC_MissionController2_1000542") +		// #autoLOC_MissionController2_1000541 = The station 		// #autoLOC_MissionController2_1000542 =  was damaged during launch when the micrometeoroid shield separated from the workshop and tore away, taking one of two main solar panel arrays with it and jamming the other
                Localizer.Format("#autoLOC_MissionController2_1000543") + SaveInfo.skyLabName + " of most of its electrical power, and also removed protection from intense solar heating, threatening to make it unusable.\n\n" +		// #autoLOC_MissionController2_1000543 = one so that it could not deploy. This deprived (Skylab) 
                Localizer.Format("#autoLOC_MissionController2_1000544") + SaveInfo.skyLabName + " will have to repair the Station before they can conduct any Science and stay on the station! So our objectives are:\n\n" +		// #autoLOC_MissionController2_1000544 = Our first crew we will send to the (SkyLab) 
                Localizer.Format("#autoLOC_MissionController2_1000545") +		// #autoLOC_MissionController2_1000545 = 1. Launch 3 Man Vessel to the station and Dock with it.\n 2. Repair the station using the Repair Panel placed on the vessel during Mission 1.\n 3. After repairs stay on station and conduct 
                Localizer.Format("#autoLOC_MissionController2_1000546") + Tools.formatTime(contractTime) + "\n\n" +		// #autoLOC_MissionController2_1000546 = scientific studies in orbit around Kerbin.\n 4. Keep crew in station and orbit for 
                Localizer.Format("#autoLOC_MissionController2_1000547") +		// #autoLOC_MissionController2_1000547 = Mission Controller will save the TimeCountdown to you persistent file. There's no need to actually stay in flight once countdown starts, so you are free to do other task while the contract counts down\n\n
                Localizer.Format("#autoLOC_MissionController2_1000548") +		// #autoLOC_MissionController2_1000548 = How To Repair SkyLab\n\n 1. Dock with skylab\n2.Transfer RepairParts from your orbitor to the Repair Panel on SkyLab.\n3.While still in vessel right click Repair Panel and Select Check If System is Ready 
                Localizer.Format("#autoLOC_MissionController2_1000549");		// #autoLOC_MissionController2_1000549 = to repair.\n4. Go EVA and approach the Repair Panel on SkyLab.\n5. Open the Repair Panel door and select Repair.  All done!
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000550") + " " + SaveInfo.skyLabName + " " + Localizer.Format("#autoLOC_MissionController2_1000551");		// #autoLOC_MissionController2_1000550 = Launch 3 man vessel to (SkyLab) 		// #autoLOC_MissionController2_1000551 =  and conduct repair, science, and long duration mission
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab2done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000552") +		// #autoLOC_MissionController2_1000552 = Skylab 2 was the first manned mission to Skylab, the first U.S. orbital space station. The mission was launched on a Saturn IB rocket and carried a three-person 
                Localizer.Format("#autoLOC_MissionController2_1000553") +		// #autoLOC_MissionController2_1000553 = crew to the station. The name Skylab 2 also refers to the vehicle used for that mission. The Skylab 2 mission established a record for human spaceflight duration. Furthermore, its crew were the 
                Localizer.Format("#autoLOC_MissionController2_1000554") +		// #autoLOC_MissionController2_1000554 = first space station occupants ever to return safely to Earth – the only other space station occupants, the crew of the 1971 Soyuz 11 mission that had manned the Salyut 1 station, were killed during reentry.\n\n 

                Localizer.Format("#autoLOC_MissionController2_1000555") +		// #autoLOC_MissionController2_1000555 = The manned Skylab missions were officially designated Skylab 2, 3, and 4. Miscommunication about the numbering resulted in the mission emblems reading Skylab I, Skylab II, and Skylab 3 respectively.\n\n 

                Localizer.Format("#autoLOC_MissionController2_1000556");		// #autoLOC_MissionController2_1000556 = Information of the SkyLab 2 Mission was gathered from Wikipedia
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            crew = int.Parse(node.GetValue("crewcount"));
            contractTime = int.Parse(node.GetValue("ctime"));
            contractName = node.GetValue("cname");
            contTimeTitle = node.GetValue("ctitle");

            Ctitle = node.GetValue("ctitle");
            RPamount = double.Parse(node.GetValue("rpamount"));

            Tools.ContractLoadCheck(node, ref vesselId, "none", vesselId, "vesid");
            Tools.ContractLoadCheck(node, ref vesselName, "none", vesselName, "vname");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("ctime", contractTime);
            node.AddValue("crewcount", crew);
            node.AddValue("cname", contractName);
            node.AddValue("ctitle", contTimeTitle);

            node.AddValue("ctitle", Ctitle);
            node.AddValue("rpamount", RPamount);

            node.AddValue("vesid", vesselId);
            node.AddValue("vname", vesselName);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.skylab2done == true) { return false; }
            if (SaveInfo.skylab1done == false) { return false; }
            if (SaveInfo.Agena2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region SkyLab 3
    public class SkyLab3 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 3;
        public int contractTime = 1284336;
        public string contTimeTitle = " Dock and stay in orbit ";
        string vesselId = "none";
        string vesselName = "none";

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter skylab1;
        ContractParameter skylab2;
        ContractParameter skylab3;
        ContractParameter skylab4;
        ContractParameter skylab5;

        public int totalContracts;

        protected override bool Generate()
        {          
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab3>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }

            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }
            vesselId = SaveInfo.skyLabVesID;
            vesselName = SaveInfo.skyLabName;

            this.skylab1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            skylab1.SetFunds(5000, targetBody);
            skylab1.SetReputation(10, targetBody);
            this.skylab3 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            skylab3.SetFunds(5000, targetBody);
            skylab3.SetReputation(10, targetBody);
            skylab3.SetScience(25, targetBody);

            this.skylab2 = this.AddParameter(new EvaGoal(targetBody), null);
            skylab2.SetFunds(2000, targetBody);
            skylab2.SetReputation(3, targetBody);
            skylab2.SetScience(5, targetBody);

            this.skylab4 = this.AddParameter(new TimeCountdownDocking(targetBody, contractTime, contTimeTitle, vesselId, vesselName), null);
            skylab4.SetFunds(10000, targetBody);
            skylab4.SetReputation(15, targetBody);
            skylab4.SetScience(15, targetBody);

            this.skylab5 = this.AddParameter(new LandOnBody(targetBody), null);
            skylab5.SetFunds(5000, 75000, targetBody);
            skylab5.SetReputation(15, targetBody);
            skylab5.SetScience(5, targetBody);

            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(25000f, 130000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(700f, targetBody);
            base.SetReputation(35f, targetBody);
            base.SetScience(100f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of SL-3 & conduct science while on station" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000557") + " " + SaveInfo.skyLabName;		// #autoLOC_MissionController2_1000557 = Launch SL-3 To (SkyLab) 
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return Localizer.Format("#autoLOC_MissionController2_1000558") +		// #autoLOC_MissionController2_1000558 = Skylab 3 was the second manned mission to the first American space station, Skylab. The mission began July 28, 1973, with the launch of three astronauts on the Saturn IB rocket, 
                Localizer.Format("#autoLOC_MissionController2_1000559") +		// #autoLOC_MissionController2_1000559 = and lasted 59 days, 11 hours and 9 minutes. A total of 1,084.7 astronaut-utilization hours were tallied by the Skylab 3 crew performing scientific experiments in the areas of medical activities, 
                Localizer.Format("#autoLOC_MissionController2_1000560");		// #autoLOC_MissionController2_1000560 = solar observations, Earth resources, and other experiments.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000561") + " " + SaveInfo.skyLabName + " " + Localizer.Format("#autoLOC_MissionController2_1000562");		// #autoLOC_MissionController2_1000561 = Launch 3 man vessel to (SkyLab) 		// #autoLOC_MissionController2_1000562 =  and conduct science, and long duration mission
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab3done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000563") +		// #autoLOC_MissionController2_1000563 = During the approach phase, a propellant leak developed in one of the Apollo Service Module's reaction control system thruster quads. The crew was able to safely dock with Skylab, but 
                Localizer.Format("#autoLOC_MissionController2_1000564") +		// #autoLOC_MissionController2_1000564 = troubleshooting continued with the problem. Six days later, another thruster quad developed a leak, creating concern amongst Mission Control. For the first time, an Apollo spacecraft would be rolled 
            Localizer.Format("#autoLOC_MissionController2_1000565") +		// #autoLOC_MissionController2_1000565 = out to Launch Complex 39 for a rescue mission, made possible by the ability for the station to have two Apollo CSMs docked at the same time. It was eventually determined that the CSM could be safely maneuvered 
            Localizer.Format("#autoLOC_MissionController2_1000566") +		// #autoLOC_MissionController2_1000566 = using only two working thruster quads, and the rescue mission was never launched.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000567") +		// #autoLOC_MissionController2_1000567 = The crew, during their first EVA, installed the twin-pole sunshade, one of the two solutions for the destruction of the micrometeoroid shield during Skylab's launch to keep the space station cool. 
            Localizer.Format("#autoLOC_MissionController2_1000568") +		// #autoLOC_MissionController2_1000568 = It was installed over the parasol, which was originally deployed through a porthole airlock during Skylab 2. Both were brought to the station by Skylab 2.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000569") +		// #autoLOC_MissionController2_1000569 = Skylab 3 continued a comprehensive medical research program that extended the data on human physiological adaptation and readaptation to space flight collected on the previous Skylab 2 mission. 
            Localizer.Format("#autoLOC_MissionController2_1000570") +		// #autoLOC_MissionController2_1000570 = In addition, Skylab 3 extended the astronauts' stay in space from approximately one month to two months. Therefore, the effects of flight duration on physiological adaptation and 
            Localizer.Format("#autoLOC_MissionController2_1000571") +		// #autoLOC_MissionController2_1000571 = readaptation could be examined.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000572");		// #autoLOC_MissionController2_1000572 = Information of the SkyLab 2 Mission was gathered from Wikipedia
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            crew = int.Parse(node.GetValue("crewcount"));
            contractTime = int.Parse(node.GetValue("ctime"));
            contTimeTitle = node.GetValue("ctitle");
            Tools.ContractLoadCheck(node, ref vesselId, "none", vesselId, "vesid");
            Tools.ContractLoadCheck(node, ref vesselName, "none", vesselName, "vname");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("ctime", contractTime);
            node.AddValue("crewcount", crew);
            node.AddValue("ctitle", contTimeTitle);
            node.AddValue("vesid", vesselId);
            node.AddValue("vname", vesselName);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.skylab3done == true) { return false; }
            if (SaveInfo.skylab2done == false) { return false; }
            if (SaveInfo.Agena2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region SkyLab 4
    public class SkyLab4 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 3;
        public int contractTime = 1815264;
        public string contTimeTitle = " Dock and stay in orbit ";
        string vesselId = "none";
        string vesselName = "none";
        public double ApA = 120000;
        public double PeA = 100000;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter skylab1;
        ContractParameter skylab2;
        ContractParameter skylab3;
        ContractParameter skylab4;
        ContractParameter skylab5;
        ContractParameter skylab6;
        ContractParameter skylab7;

        public int totalContracts;

        protected override bool Generate()
        {          
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab4>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }

            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().HistoricalContracts)
            {
                return false;
            }

            ApA = Tools.ReturnMinOrbit(targetBody, 1.8f); ;
            PeA = ApA;

            vesselId = SaveInfo.skyLabVesID;
            vesselName = SaveInfo.skyLabName;
            this.skylab1 = this.AddParameter(new TargetDockingGoal(vesselId, vesselName), null);
            skylab1.SetFunds(5000, targetBody);
            skylab1.SetReputation(10, targetBody);

            this.skylab3 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            skylab3.SetFunds(5000, targetBody);
            skylab3.SetReputation(10, targetBody);
            skylab3.SetScience(25, targetBody);

            this.skylab2 = this.AddParameter(new EvaGoal(targetBody), null);
            skylab2.SetFunds(2000, targetBody);
            skylab2.SetReputation(3, targetBody);
            skylab2.SetScience(5, targetBody);

            skylab6 = this.AddParameter(new ApAOrbitGoal(targetBody, (double)ApA, true, "Orbit",false), null);
            skylab6.SetFunds(1000, targetBody);

            skylab7 = this.AddParameter(new PeAOrbitGoal(targetBody, (double)PeA, true, "Orbit",false), null);
            skylab7.SetFunds(1000, targetBody);

            this.skylab4 = this.AddParameter(new TimeCountdownDocking(targetBody, contractTime, contTimeTitle, vesselId, vesselName), null);
            skylab4.SetFunds(10000, targetBody);
            skylab4.SetReputation(15, targetBody);
            skylab4.SetScience(15, targetBody);

            this.skylab5 = this.AddParameter(new LandOnBody(targetBody), null);
            skylab5.SetFunds(5000, 75000, targetBody);
            skylab5.SetReputation(15, targetBody);
            skylab5.SetScience(5, targetBody);

            this.AddParameter(new GetCrewCount(crew), null);

            base.SetFunds(25000f, 130000f, targetBody);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineDays(700f, targetBody);
            base.SetReputation(35f, targetBody);
            base.SetScience(100f, targetBody);
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

        protected override string GetNotes()
        {
            return "";
        }

        protected override string GetHashString()
        {
            return "Launch of SL-4 & conduct science while on station" + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000573") + " " + SaveInfo.skyLabName;		// #autoLOC_MissionController2_1000573 = Launch SL-4 To (SkyLab) 
        }
        protected override string GetDescription()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000574") +		// #autoLOC_MissionController2_1000574 = Skylab 4 was the third manned Skylab mission and placed the third and final crew aboard the first American space station. The mission started on November 16, 1973 with the launch of three astronauts 
                Localizer.Format("#autoLOC_MissionController2_1000575") +		// #autoLOC_MissionController2_1000575 = on a Saturn IB rocket from the Kennedy Space Center, Florida and lasted 84 days, one hour and 16 minutes. A total of 6,051 astronaut-utilization hours were tallied by Skylab 4 astronauts performing 
                Localizer.Format("#autoLOC_MissionController2_1000576");		// #autoLOC_MissionController2_1000576 = scientific experiments in the areas of medical activities, solar observations, Earth resources, observation of the Comet Kohoutek and other experiments.
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000577") + SaveInfo.skyLabName + " and conduct science, change Station altitude, and long duration mission";		// #autoLOC_MissionController2_1000577 = Launch 3 man vessel to (SkyLab) 
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab4done = true;
            return Localizer.Format("#autoLOC_MissionController2_1000578") +		// #autoLOC_MissionController2_1000578 = Skylab 4 was the last Skylab mission. The crew arrived aboard Skylab to find that they had company – three figures dressed in flight suits. Upon closer inspection, they found their companions were three 
                Localizer.Format("#autoLOC_MissionController2_1000579") +		// #autoLOC_MissionController2_1000579 = dummies, complete with Skylab 4 mission emblems and name tags which had been left there by Al Bean, Jack Lousma, and Owen Garriott at the end of Skylab 3.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000580") +		// #autoLOC_MissionController2_1000580 = The all-rookie astronaut crew had problems adjusting to the same workload level as their predecessors when activating the workshop. Things got off to a bad start after the crew attempted to hide Pogue's 
            Localizer.Format("#autoLOC_MissionController2_1000581") +		// #autoLOC_MissionController2_1000581 = early space sickness from flight surgeons, a fact discovered by mission controllers after downloading onboard voice recordings. The crew's initial task of unloading and stowing the thousands of items needed 
            Localizer.Format("#autoLOC_MissionController2_1000582") +		// #autoLOC_MissionController2_1000582 = for their lengthy mission also proved to be overwhelming. The schedule for the activation sequence dictated lengthy work periods with a large variety of tasks to be performed, and the crew soon found themselves 
            Localizer.Format("#autoLOC_MissionController2_1000583") +		// #autoLOC_MissionController2_1000583 = tired and behind schedule.

            Localizer.Format("#autoLOC_MissionController2_1000584") +		// #autoLOC_MissionController2_1000584 = as the activation of Skylab progressed, the astronauts complained of being pushed too hard. Ground crews disagreed; they felt that the astronauts were not working long enough or hard enough. During the course
            Localizer.Format("#autoLOC_MissionController2_1000585") +		// #autoLOC_MissionController2_1000585 = of the mission, this culminated in a radio conference to air frustrations. Following this, the workload schedule was modified, and by the end of their mission the crew had completed even more work than had been
            Localizer.Format("#autoLOC_MissionController2_1000586") +		// #autoLOC_MissionController2_1000586 = planned before launch. The experiences of the crew and ground controllers provided important lessons in planning subsequent manned spaceflight work schedules.

            Localizer.Format("#autoLOC_MissionController2_1000587") +		// #autoLOC_MissionController2_1000587 = On Thanksgiving Day, Gibson and Pogue accomplished a 6 1⁄2 hour spacewalk. The first part of their spacewalk was spent replacing film in the solar observatory. The remainder of the time was used to repair
            Localizer.Format("#autoLOC_MissionController2_1000588") +		// #autoLOC_MissionController2_1000588 = a malfunctioning antenna.

            Localizer.Format("#autoLOC_MissionController2_1000589") +		// #autoLOC_MissionController2_1000589 = The crew reported that the food was good, but slightly bland. The crew would have preferred to use more condiments to enhance the taste of the food. The amount of salt they could use was restricted for medical
            Localizer.Format("#autoLOC_MissionController2_1000590") +		// #autoLOC_MissionController2_1000590 = purposes. The quantity and type of food consumed was rigidly controlled because of their strict diet.

            Localizer.Format("#autoLOC_MissionController2_1000591") +		// #autoLOC_MissionController2_1000591 = Seven days into their mission, a problem developed in the Skylab attitude control gyroscope system, which threatened to bring an early end to the mission. Skylab depended upon three large gyroscopes, sized
            Localizer.Format("#autoLOC_MissionController2_1000592") +		// #autoLOC_MissionController2_1000592 = so that any two of them could provide sufficient control and maneuver Skylab as desired. The third acted as a backup in the event of failure of one of the others. The gyroscope failure was attributed 
            Localizer.Format("#autoLOC_MissionController2_1000593") +		// #autoLOC_MissionController2_1000593 = to insufficient lubrication. Later in the mission, a second gyroscope showed similar problems, but special temperature control and load reduction procedures kept the second one operating, and no further 
            Localizer.Format("#autoLOC_MissionController2_1000594") +		// #autoLOC_MissionController2_1000594 = problems occurred.

            Localizer.Format("#autoLOC_MissionController2_1000595") +		// #autoLOC_MissionController2_1000595 = SL-4 Goals\n\n
            Localizer.Format("#autoLOC_MissionController2_1000596") +		// #autoLOC_MissionController2_1000596 = 1. An important aspect of the SL-4 Mission is to bring the station to a higher orbital level, so once you get into the station prepare to perform orbital maneuvers.\n
            Localizer.Format("#autoLOC_MissionController2_1000597") +		// #autoLOC_MissionController2_1000597 = 2. Conduct an EVA at some point during mission.\n3. Conduct science inside the station to further Kerbal understanding of space and technology.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000598");		// #autoLOC_MissionController2_1000598 = Information of the SkyLab 2 Mission was gathered from Wikipedia
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
                else { }
            }
            crew = int.Parse(node.GetValue("crewcount"));
            contractTime = int.Parse(node.GetValue("ctime"));
            contTimeTitle = node.GetValue("ctitle");

            double maxApa = double.Parse(node.GetValue("maxaPa"));
            ApA = maxApa;
           
            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            PeA = masxPpaID;
            
            Tools.ContractLoadCheck(node, ref vesselId, "none", vesselId, "vesid");
            Tools.ContractLoadCheck(node, ref vesselName, "none", vesselName, "vname");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("ctime", contractTime);
            node.AddValue("crewcount", crew);
            node.AddValue("ctitle", contTimeTitle);

            double maxApa = ApA;
            node.AddValue("maxaPa", ApA);
            
            double maxPpAID = PeA;
            node.AddValue("maxpEa", PeA);          
            node.AddValue("vesid", vesselId);
            node.AddValue("vname", vesselName);

        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.skylab4done == true) { return false; }
            if (SaveInfo.skylab3done == false) { return false; }
            if (SaveInfo.Agena2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
}
