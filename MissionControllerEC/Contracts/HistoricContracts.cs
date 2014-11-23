using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{
    # region Vostok 1
    public class Vostok : Contract
    {
        Settings settings = new Settings("config.cfg");
        public double minHeight = 70000;
        
        public int crew = 1;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;

        protected override bool Generate()
        {           
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            minHeight = settings.vostok12height;  
            vostok1 = this.AddParameter(new AltitudeGoal(targetBody,minHeight,true),null);
            vostok1.SetFunds(1000f, targetBody);
            vostok1.SetReputation(2f, targetBody);
            vostok2 = this.AddParameter(new InOrbitGoal(targetBody),null);
            vostok2.SetFunds(2000f, targetBody);
            vostok2.SetReputation(3f, targetBody);
            vostok3 = this.AddParameter(new LandingParameters(targetBody,true), null);
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
            return "Launch of Vostok 1 into space";
        }
        protected override string GetTitle()
        {
            return "Launch of Vostok 1 " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Vostok 1 was the first spaceflight of the Vostok program and the first human spaceflight in history. The Vostok 3KA spacecraft was " +
                "launched on April 12, 1961 with Soviet cosmonaut Yuri Gagarin, making him the first human to cross into outer space.\n\n" +
                "The spaceflight consisted of one orbit around Earth, the shortest manned orbital flight to date. According to official records, the spaceflight took 108 " +
                "minutes from launch to landing. As planned, Gagarin parachuted to the ground separately from his spacecraft after ejecting at 7 km (23,000 ft) altitude. Due to the secrecy " +
                "surrounding the Soviet space program at the time, many details of the spaceflight only came to light years later, and several details in the original press releases turned out to be false.\n\n"+
                
                "Information on Vostok 1 was gathered from wikipedia\n\n" +
                
                "Objectives are too \n\n1. Enter Space \n2. Conduct at least one orbit of Kebin \n3. Return safely to Kerbin";
        }
        protected override string GetSynopsys()
        {
            return "Bring Vostok 1 to orbit around " + targetBody.theName + ". " + " Orbit at least once!";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Vostok1Done = true;
            return "Four decades after the flight, historian Asif Siddiqi wrote that Vostok 1 will undoubtedly remain one of the major milestones in not only the history of space exploration, "+
                "but also the history of the human race itself. The fact that this accomplishment was successfully carried out by the Soviet Union, a country completely devastated by war just " +
                "sixteen years prior, makes the achievement even more impressive. Unlike the United States, the USSR had to begin from a position of tremendous disadvantage. Its industrial " +
                "infrastructure had been ruined, and its technological capabilities were outdated at best. A good portion of its land had been devastated by war, and it had lost about 25 million "+ 
                "citizens ... but it was the totalitarian state that overwhelmingly took the lead.\n\n"+

                "Commemorative monument, Vostok-1 landing site near Engels, Russia.\n\n"+

                "The landing site is now a monument park. The central feature in the park is a 25 meter tall monument that consists of a silver metallic rocketship rising on a curved metallic "+
                "column of flame, from a wedge shaped, white stone base. In front of this is a 3 meter tall, white stone statue of Yuri Gagarin, wearing a spacesuit, with one arm raised in greeting "+
                "and the other holding a space helmet.\n\n"+

                "The Vostok 1 re-entry capsule is now on display at the RKK Energiya museum in Korolyov, near Moscow.\n\n"+

                "In 2011, documentary film maker Christopher Riley partnered with European Space Agency astronaut Paolo Nespoli to record a new film of what Gagarin would have seen of the "+
                "Earth from his spaceship, by matching historical audio recordings to video from the International Space Station following the ground path taken by Vostok 1. The resulting film,"+
                "First Orbit, was released online to celebrate the 50th anniversary of human spaceflight.\n\n"+
                
                "Information of Vostok 1 was gathered from Wikipedia";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            minHeight = double.Parse(node.GetValue("minheight"));
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
            if (SaveInfo.Vostok1Done == true || settings.all_Historical_Contracts_Off == true) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Vostok 2
    public class Vostok2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public double minHeight = 70000;
        public double missionTime = 43200;

        public int crew = 1;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;
        ContractParameter vostok4;

        protected override bool Generate()
        {
            
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            minHeight = settings.vostok12height;
            vostok1 = this.AddParameter(new AltitudeGoal(targetBody, minHeight,true), null);
            vostok1.SetFunds(1000f, targetBody);
            vostok1.SetReputation(2f, targetBody);
            vostok2 = this.AddParameter(new InOrbitGoal(targetBody), null);
            vostok2.SetFunds(2000f, targetBody);
            vostok2.SetReputation(3f, targetBody);
            vostok4 = this.AddParameter(new TimeCountdownOrbits(targetBody, missionTime, true), null);
            vostok4.SetFunds(3000f, targetBody);
            vostok4.SetReputation(6f, targetBody);
            vostok4.SetScience(10f, targetBody);
            vostok3 = this.AddParameter(new LandingParameters(targetBody,true), null);
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
            return "Launch of Vostok 2 into space";
        }
        protected override string GetTitle()
        {
            return "Launch of Vostok 2 " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Vostok 2 was a Soviet space mission which carried cosmonaut Gherman Titov into orbit for a full day on August 6, 1961 "+
                "to study the effects of a more prolonged period of weightlessness on the human body.[1] Titov orbited the Earth over 17 times, exceeding the single orbit of Yuri Gagarin on Vostok 1 "+
                "− as well as the suborbital spaceflights of American astronauts Alan Shepard and Gus Grissom aboard their respective Mercury-Redstone 3 and 4 missions. Indeed, Titov's number of orbits "+
                "and flight time would not be surpassed by an American astronaut until Gordon Cooper's Mercury-Atlas 9 spaceflight in May 1963.\n\n"+

                "Information on Vostok 1 was gathered from wikipedia\n\n" +

                "Objectives are too \n\n1. Enter Space \n\n2. Stay in orbit for a total of 1 day (12 Kerbal Hours) \n3. Return Home.";
        }
        protected override string GetSynopsys()
        {
            return "Bring Vostok 2 to orbit around " + targetBody.theName + ". " + " stay in orbit for 1 day.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Vostok2Done = true;
            return "The flight was an almost complete success, marred only by a heater that had inadvertently been turned off prior to liftoff and that allowed the inside temperature "+
                "to drop to 50 °F (10 °C), a bout of space sickness, and a troublesome re-entry when the reentry module failed to separate cleanly from its service module. \n\n"+
            "Unlike Yuri Gagarin on Vostok 1, Titov took manual control of the spacecraft for a short while. Another change came when the Soviets admitted that Titov did not land with "+
            "his spacecraft. Titov would claim in an interview that he ejected from his capsule as a test of an alternative landing system; it is now known that all Vostok program landings were performed this way.\n\n"+
            "The re-entry capsule was destroyed during development of the Voskhod spacecraft.\n\n"+
            "As of 2013, Titov remains the youngest person to reach space. He was a month short of 26 years old at launch.";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            minHeight = double.Parse(node.GetValue("minheight"));
            crew = int.Parse(node.GetValue("crewcount"));
            missionTime = double.Parse(node.GetValue("missionTime"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("minheight", minHeight);
            node.AddValue("crewcount", crew);
            node.AddValue("missionTime",missionTime);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.Vostok2Done == true || settings.all_Historical_Contracts_Off == true) { return false; }
            if (SaveInfo.Vostok1Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Voskhod 2
    public class Voskhod2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public double minHeight = 82000;

        public int crew = 2;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;
        ContractParameter vostok4;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            minHeight = settings.voshodheight;
            vostok1 = this.AddParameter(new AltitudeGoal(targetBody, minHeight,true), null);
            vostok1.SetFunds(2000f, targetBody);
            vostok1.SetReputation(2f, targetBody);
            vostok2 = this.AddParameter(new InOrbitGoal(targetBody), null);
            vostok2.SetFunds(3000f, targetBody);
            vostok2.SetReputation(3f, targetBody);
            vostok4 = this.AddParameter(new EvaGoal(targetBody), null);
            vostok4.SetFunds(4200f, targetBody);
            vostok4.SetReputation(6f, targetBody);
            vostok4.SetScience(6f, targetBody);
            vostok3 = this.AddParameter(new LandingParameters(targetBody, true), null);
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
            return "Launch of Voskhod 2 (2 Kerbals) into space";
        }
        protected override string GetTitle()
        {
            return "Launch of Voskhod 2 (2 Kerbals) " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Voskhod 2 was a Soviet manned space mission in March 1965. Vostok-based Voskhod 3KD spacecraft with two crew members on board, Pavel Belyayev and Alexey Leonov,"+
                "was equipped with an inflatable airlock. It established another milestone in space exploration when Alexey Leonov became the first person to leave the spacecraft in a specialized "+
                "spacesuit to conduct a 12 minute spacewalk." +

                "Information on Voskhod 2 was gathered from wikipedia\n\n" +

                "Objectives are too \n\n1. Enter Space \n\n2. Bring vessel to Orbit Height at least 82000m (2 Kerbals Total) \n3. EVA 1 Kerbal \n 4. Return to Kerbin safely.";
        }
        protected override string GetSynopsys()
        {
            return "Bring Voskhod 2 (2 Kerbals) to orbit around " + targetBody.theName + ". " + " stay in orbit for 1 day.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Voskhod2Done = true;
            return "A delay of 46 seconds caused the spacecraft to land 386 km from the intended landing zone, in the inhospitable forests of Upper Kama Upland, somewhere west of Solikamsk. " +
            "Although mission control had no idea where the spacecraft had landed or whether Leonov and Belyayev had survived, their families were told that they were resting after having been recovered. " +
            "The two men were both familiar with the harsh climate and knew that bears and wolves, made aggressive by mating season, lived in the taiga; the spacecraft carried a pistol and plenty of ammunition. " +
            "Although aircraft quickly located the cosmonauts, the area was so heavily forested that helicopters could not land. Night arrived, the temperature fell to below −30°C, and the spacecraft's hatch " +
            "had been blown open by explosive bolts. Leonov and Belyayev had to strip naked, wring out the sweat from their underwear, and re-don it and the inner linings of their spacesuits to stay warm. " +
            "A rescue party arrived on skis the next day with food and hot water, and chopped wood for a fire and a log cabin. After a more comfortable second night in the forest, the cosmonauts skied to a " +
            "waiting helicopter several kilometers away and flew to Perm, then Baikonur.\n\n" +

            "Information on Voskhod 2 was gathered from wikipedia";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            minHeight = double.Parse(node.GetValue("minheight"));
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
            if (SaveInfo.Voskhod2Done == true || settings.all_Historical_Contracts_Off == true) { return false; }
            if (SaveInfo.Vostok2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Luna 2
    public class Luna2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public int crew = 0;

        CelestialBody targetBody = FlightGlobals.Bodies[2];

        ContractParameter luna1;
        ContractParameter luna2;
        ContractParameter luna3;
        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
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
            string AgenaMessage = "For best results with the Luna 2 and its Crash Goal I have found it best to make sure that the Command Pod does not get destroyed first!!\n\n" +
                "Had many reports of issue with this contract, and it seems that sometimes when the pod goes first then KSP doesn't have time to register the destruction of you personal Vessel.\n\n" +
                "If this contract does give you issue's with the crash goal feel free to ALT F-12, select contracts and used the COM with the Luna 2 contract to complete it, its not cheating its a bug";
            MessageSystem.Message m = new MessageSystem.Message("Important Luna 2 CrashGoal Contract Information", AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);
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
            return "Launch of LUNA 2 To Mun";
        }
        protected override string GetTitle()
        {
            return "Launch of Luna 2 " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "on September 12th, 1959 Luna 2 was launched. At just past midnight Moscow time on September 14th it crashed some 240,000 miles away on the Moon not far from "+
                "the Sea of Tranquillity (perhaps a not entirely appropriate location). Korolev and his people were listening as the signals coming back from the spacecraft suddenly stopped. "+
                "The total silence meant that Luna had hit its target and there was great jubilation in the control room.\n\n"+

                "Luna 2 (Luna is Russian for Moon) weighed 390 kilograms. It was spherical in shape with antennae sticking out of it and carried instruments for measuring radiation, magnetic fields "+
                "and meteorites. It also carried metal pendants which it scattered on the surface on impact, with the hammer and sickle of the USSR on one side and the launch date on the other. "+
                "It confirmed that the moon had only a tiny radiation field and, so far as could be observed, no radiation belts. The spacecraft had no propulsion system of its own and the third and "+
                "final stage of its propelling rocket crashed on the moon about half an hour after Luna 2 itself.\n\n"+

                "Information on Luna 2 was gathered from HistoryToday.com\n\n" +

                "Objectives are \n\n1. Launch Luna 2 \n2. Orbit the Mun \n3. Conduct Science at Mun \n 4. When finished crash vessel into surface mun.";
        }
        protected override string GetSynopsys()
        {
            return "Bring Luna 2 to orbit around " + targetBody.theName + ". " + " Conduct Science, Then crash into surface of Mun.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Luna2Done = true;
            return "The scientific results of Luna 2 were similar to those of Luna 1, but the psychological impact of Luna 2 was profound. The closest any American probe had come to the Moon at "+
                "that point was 37,000 miles. It seemed clear in the United States that the timing had been heavily influenced by the fact that the Soviet premier, Nikita Khruschev, was due to arrive "+
            "in the US immediately afterwards, to be welcomed by President Eisenhower. Luna 2’s success enabled him to appear beaming with rumbustious pride. He lectured Americans on the virtues of communism "+
            "and the immorality of scantily clothed chorus girls. The only way of annoying him seemed to be by refusing to let him into Disneyland.\n\n"+  

            "Korolev had a clincher to come. Only three weeks later, Luna 3 was launched on October 4th, the second anniversary of Sputnik 1, to swing round the far side of the Moon and send back the "+
            "first fuzzy pictures of its dark side, which no one had seen before. It was an astonishing feat of navigation and it was now possible to draw a tentative map of the Moon’s hidden side.\n\n "+

                "Information on Luna 2 was gathered from HistoryToday.com";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
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
            if (SaveInfo.Luna2Done == true || settings.all_Historical_Contracts_Off == true) { return false; }
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
        CelestialBody targetBody = FlightGlobals.Bodies[2];
        ContractParameter luna1;
        ContractParameter luna2;
        ContractParameter luna3;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            luna1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            luna1.SetFunds(8000f, targetBody);
            luna1.SetReputation(8f, targetBody);

            luna3 = this.AddParameter(new LandingParameters(targetBody, true), null);
            luna3.SetFunds(8000f, targetBody);
            luna3.SetReputation(3f, targetBody);
            luna3.SetScience(25f, targetBody);

            luna2 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Surface), null);
            luna2.SetFunds(5000f, targetBody);
            luna2.SetReputation(5f, targetBody);

            this.AddParameter(new LandingParameters(Planetarium.fetch.Home,true), null);

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
            return "Launch of LUNA 16 To Mun";
        }
        protected override string GetTitle()
        {
            return "Launch of Luna 16 " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Luna 16 was an unmanned space mission, part of the Soviet Luna program."+

            "Luna 16 was the first robotic probe to land on the Moon and return a sample of lunar soil to Earth.[1] The sample was returned from Mare Fecunditatis. It represented "+
            "the first lunar sample return mission by the Soviet Union and was the third lunar sample return mission overall, following the Apollo 11 and Apollo 12 missions.\n\n"+

            "The spacecraft consisted of two attached stages, an ascent stage mounted on top of a descent stage. The descent stage was a cylindrical body with four protruding landing legs, fuel tanks, "+
            "a landing radar, and a dual descent engine complex.\n\n"+

            "A main descent engine was used to slow the craft until it reached a cutoff point which was determined by the on-board computer based on altitude and velocity. After cutoff a bank of lower "+
            "thrust jets was used for the final landing. The descent stage also acted as a launch pad for the ascent stage.\n\n"+

            "The ascent stage was a smaller cylinder with a rounded top. It carried a cylindrical hermetically sealed soil sample container inside a re-entry capsule.\n\n"+

            "The spacecraft descent stage was equipped with a television camera, radiation and temperature monitors, telecommunications equipment, and an extendable arm with a drilling rig for "+
            "the collection of a lunar soil sample.\n\n" +

                "Information on Luna 2 was gathered from wikipedia.org\n\n" +

                "Objectives are \n\n1. Launch Luna 16 \n2. Orbit the Mun \n3. Land Mun (Samples are automated) \n 4. Return to Kerbin with samples.";
        }
        protected override string GetSynopsys()
        {
            return "Bring Luna 16 to orbit around " + targetBody.theName + ". " + " Land on Mun, Return To Kerbin with automated samples.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Luna16Done = true;
            return "Three tiny samples (0.2 grams) of the Luna 16 soil were sold at Sotheby auction for $442,500 in 1993.[2] The samples are the only lunar return material in private ownership during the "+
                "20th century.[2] Another source of privately possessed moon rock is lunar meteorites of varying quality and authenticity, and another is lost Apollo moon rocks, possible legal issues aside."+

                "A series of 10-kopeck stamps was issued in 1970 to commemorate the flight of Luna 16 lunar probe and depicted the main stages of the programme: soft landing on Moon, launch of the lunar soil "+
                "sample return capsule, and parachute assisted landing back on Earth.\n\n"+

                "Information on Luna 2 was gathered from wikipedia.org";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
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
            if (SaveInfo.Luna16Done == true || settings.all_Historical_Contracts_Off == true) { return false; }
            if (SaveInfo.Luna2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Agena Contract 1
    public class AgenaTargetPracticeContract : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double GMaxApA = 0;
        public double GMinApA = 0;
        public double GMaxPeA = 0;
        public double GMinPeA = 0;
        public int crewCount = 0;
        public string partName = "ModuleDockingNode";
        public string ModuleTitle = "Any Docking Port";
        public int totalContracts;
        public int TotalFinished;
        public int Agena1Done;

        ContractParameter AgenaParameter;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
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
            targetBody = Planetarium.fetch.Home;
            GMaxApA = UnityEngine.Random.Range((int)st.contract_Satellite_MIn_Height, (int)st.contract_Satellite_Max_Height);
            GMinApA = GMaxApA - st.contract_Satellite_Between_Difference;
            GMaxPeA = GMaxApA;
            GMinPeA = GMinApA;

            AgenaParameter = this.AddParameter(new AgenaInOrbit(targetBody), null);
            AgenaParameter.SetFunds(2000.0f, targetBody);
            AgenaParameter.SetReputation(20f, targetBody);
            this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA), null);
            this.AddParameter(new ModuleGoal(partName, ModuleTitle), null);
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(15f, 35f);
            base.SetScience(25f, targetBody);
            base.SetDeadlineDays(19f, targetBody);
            base.SetReputation(35f, 35f, targetBody);
            base.SetFunds(28000f * st.Contract_Payment_Multiplier, 54000f * st.Contract_Payment_Multiplier, 39000f * st.Contract_Payment_Multiplier, targetBody);

            return true;
        }

        protected override void OnAccepted()
        {
            string AgenaMessage = "Please Take Note when you finish the Agena Contract that vessel will be recorded as the Agena Vessel for next mission!\n\n" +
                "if the wrong vessel is recorded you can change the vessel by using the Debug Tools in settings for Agena Contract";
            MessageSystem.Message m = new MessageSystem.Message("Important Agena Target Contract Information", AgenaMessage.ToString(), MessageSystemButton.MessageButtonColor.YELLOW, MessageSystemButton.ButtonIcons.MESSAGE);
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
            return targetBody.bodyName + GMaxApA.ToString() + GMinApA.ToString();
        }
        protected override string GetTitle()
        {
            return "Agena Target Vehicle Orbital Test Around Kerbin - Launch Agena Vehicle";
        }
        protected override string GetDescription()
        {

            return "The Agena Target Vehicle (ATV) was an unmanned spacecraft used by NASA during its Gemini program to develop and practice orbital space rendezvous and docking techniques and\n" +
                "to perform large orbital changes, in preparation for the Apollo program lunar missions.\n\n" +
                "Your first task is to launch an Agena Type vehicle into orbit\n\n" +
                "Please Take Note when you finish the Agena Contract that vessel will be recorded as the Agena Vessel for next mission!";
        }
        protected override string GetSynopsys()
        {
            return "Agena Test " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Agena1Done = true;
            SaveInfo.AgenaTargetVesselID = FlightGlobals.ActiveVessel.id.ToString();
            SaveInfo.AgenaTargetVesselName = FlightGlobals.ActiveVessel.vesselName.Replace("(unloaded)", "");
            return FlightGlobals.ActiveVessel.name + " Vessel ID is " + FlightGlobals.ActiveVessel.id + "\n\n" +
                "If this is not correct you can change this is Debug menu using select Agena vessel Debug Tool\n\n" +
                "Congradulations you have succesfully launched your Agena Target Vehicle, now you must get you Manned Orbital vehicle to Dock with the ATV\n\n" +
                "The Gemini-Agena Target Vehicle design was an adaptation of the basic Agena-D vehicle using the alternate Model 8247 rocket engine and additional program-peculiar equipment required for the Gemini mission.\n" +
                "This GATV was divided into:\n\n" +

"The program-peculiar forward auxiliary section. This section consisted of the auxiliary equipment rack, the McDonnell Aircraft Company-furnished docking-adapter module, and the clamshell nose shroud.\n" +
"The Agena-D forward and mid-body sections. The Agena-D forward section housed the main equipment bay, and the mid-body contained the main fuel and oxidizer tanks which supplied propellants through a feed and\n" +
"load system for the main engine. (3) the program-peculiar aft section. The Model 8247 multi-start main engine and the smaller Model 8250 maneuvering and ullage orientation engines were located in this section.\n" +
"Orbital length of the GATV was approximately 26 feet. Vehicle weight-on-orbit was approximately 7200 lb. This weight included propellants still remaining in the main tanks and available for Model 8247 engine operation\n" +
"after the Agena achieved orbit.\n\n" +
"The Gemini-ATV propulsion system consisted of the following:\n\n" +

"Model 8247 rocket engine, also known as XLR-81-BA-13, and its controls, mount, gimbals, and titanium nozzle extension\n" +
"Pyrotechnically operated helium-control valve (POHCV) and associated pressurization plumbing\n" +
"Fuel and oxidizer feed and load system, including propellant tanks, vents, and fill quick disconnects\n" +
"Propellant isolation valves (PIV's)\n" +
"All associated pyro devices and solid-propellant rockets.\n\n" +
"All Information For Agena was Gathered From www.astronautix.com";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double maxApa = double.Parse(node.GetValue("maxaPa"));
            GMaxApA = maxApa;
            double minApa = double.Parse(node.GetValue("minaPa"));
            GMinApA = minApa;

            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            GMaxPeA = masxPpaID;
            double minPeAID = double.Parse(node.GetValue("minpEa"));
            GMinPeA = minPeAID;

            ModuleTitle = node.GetValue("moduletitle");
            partName = node.GetValue("pName");
            crewCount = int.Parse(node.GetValue("crewcount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            double maxApa = GMaxApA;
            node.AddValue("maxaPa", GMaxApA);
            double minApa = GMinApA;
            node.AddValue("minaPa", GMinApA);

            double maxPpAID = GMaxPeA;
            node.AddValue("maxpEa", GMaxPeA);
            double MinPeAID = GMinPeA;
            node.AddValue("minpEa", GMinPeA);



            string pname = partName;
            node.AddValue("pName", partName);
            node.AddValue("moduletitle", ModuleTitle);

            node.AddValue("crewcount", crewCount);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (!techUnlock || st.all_Historical_Contracts_Off == true)
                return false;
            else
                return true;
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
        public string partName = "Clamp-O-Tron Docking Port";
        public double GMaxApA = 0;
        public double GMinApA = 0;
        public double GMaxPeA = 0;
        public double GMinPeA = 0;
        public string vesselTestID = "none";
        public string vesselTestName = "none";
        public int totalContracts;
        public int Agena1Done;
        public int Agena2Done;
        ContractParameter AgenaDockParameter;


        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
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
            targetBody = Planetarium.fetch.Home;
            GMaxApA = UnityEngine.Random.Range((int)st.contract_Satellite_MIn_Height, (int)st.contract_Satellite_Max_Height);
            GMinApA = GMaxApA - st.contract_Satellite_Between_Difference;
            GMaxPeA = GMaxApA;
            GMinPeA = GMinApA;

            vesselTestID = SaveInfo.AgenaTargetVesselID;
            vesselTestName = SaveInfo.AgenaTargetVesselName;
            AgenaDockParameter = this.AddParameter(new TargetDockingGoal(vesselTestID, vesselTestName), null);
            AgenaDockParameter.SetFunds(3000.0f, targetBody);
            AgenaDockParameter.SetReputation(30f, targetBody);

            this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA,true), null);
            this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA,true), null);
            this.AddParameter(new LandingParameters(targetBody, true), null);
            this.AddParameter(new PartGoal(partName, partAmount), null);
            this.AddParameter(new GetCrewCount(crewCount), null);

            base.SetExpiry(15f, 35f);
            base.SetScience(25f, targetBody);
            base.SetDeadlineDays(20f, targetBody);
            base.SetReputation(50f, 35f, targetBody);
            base.SetFunds(29000f * st.Contract_Payment_Multiplier, 48000f * st.Contract_Payment_Multiplier, 42000f * st.Contract_Payment_Multiplier, targetBody);

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
            return targetBody.bodyName + vesselTestName;
        }
        protected override string GetTitle()
        {
            return "Agena Target Vehicle Orbital Test Around Kerbin - Dock With ATV " + vesselTestName;
        }
        protected override string GetDescription()
        {

            return "Project Gemini was the second human spaceflight program of NASA, the civilian space agency of the United States government. Project Gemini was conducted between projects Mercury\n" +
                " and Apollo, with ten manned flights occurring in 1965 and 1966.\n\n" +

                 "Its objective was to develop space travel techniques in support of Apollo, which had the goal of landing men on the Moon. Gemini achieved missions long enough for a trip to the Moon\n" +
                 "and back, perfected extra-vehicular activity (working outside a spacecraft), and orbital maneuvers necessary to achieve rendezvous and docking. All Gemini flights were launched from \n" +
                 " Cape Canaveral, Florida using the Titan II Gemini launch vehicle\n\n" +
                 "Info For Gemini From Wikipedia.org\n\n" +
                "Your Second Task Is To Dock your Manned Orbital Pod with Agena Target Vehicle.  Then you are required to change Altitude to the selected ApA and PeA";
        }
        protected override string GetSynopsys()
        {
            return "Agena Test " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Agena2Done = true;
            return "You have been succesful with Launching an Agena Type Craft, Docking with it, and changing your Orbital Altitude.  Congradulations!\n\n" +
                "The first GATV was launched on October 25, 1965 while the Gemini 6 astronauts were waiting on the pad. While the Atlas performed normally,\n" +
                "the Agena's engine exploded during orbital injection. Since the rendezvous and docking was the primary objective, the Gemini 6 mission was scrubbed,\n" +
                "and replaced with the alternate mission Gemini 6A, which rendezvoused (but could not dock) with Gemini 7 in December.\n\n" +
                "Was not until Gemini 10 That all objectives of Launching, Docking, and boosting Gemini 10 to 412-nautical-mile change succeded.";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double maxApa = double.Parse(node.GetValue("maxaPa"));
            GMaxApA = maxApa;
            double minApa = double.Parse(node.GetValue("minaPa"));
            GMinApA = minApa;

            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            GMaxPeA = masxPpaID;
            double minPeAID = double.Parse(node.GetValue("minpEa"));
            GMinPeA = minPeAID;

            int pcount = int.Parse(node.GetValue("pCount"));
            partAmount = pcount;
            partName = (node.GetValue("pName"));
            crewCount = int.Parse(node.GetValue("crewcount"));

            vesselTestID = node.GetValue("vesselid");
            vesselTestName = node.GetValue("vesselname");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            double maxApa = GMaxApA;
            node.AddValue("maxaPa", GMaxApA);
            double minApa = GMinApA;
            node.AddValue("minaPa", GMinApA);

            double maxPpAID = GMaxPeA;
            node.AddValue("maxpEa", GMaxPeA);
            double MinPeAID = GMinPeA;
            node.AddValue("minpEa", GMinPeA);


            int pcount = partAmount;
            node.AddValue("pCount", partAmount);
            string pname = partName;
            node.AddValue("pName", partName);

            node.AddValue("crewcount", crewCount);

            node.AddValue("vesselid", vesselTestID);
            node.AddValue("vesselname", vesselTestName);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("specializedConstruction") == RDTech.State.Available;
            if (!techUnlock || st.all_Historical_Contracts_Off == true)
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
                SaveInfo.skyLabName = vs.name.Replace("(unloaded)", "");
                SaveInfo.skyLabVesID = vs.id.ToString();
                ScreenMessages.PostScreenMessage("" + vs.name.Replace("(unloaded)", "") + " was added to skylab contract vessel.  It's ID is " + vs.id.ToString());
            }
            else
                Debug.Log("error in adding vessel id and name to save file for SkyLab 1 Contract");
        }
        public int totalContracts;
    
        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab1>().Count();
            if (totalContracts >= 1) { return false; }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            minHeight = settings.skyLabheight;
            this.AddParameter(new AltitudeGoal(targetBody,minHeight), null);
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new PartGoal(part1goal, part1amount), null);
            this.AddParameter(new PartGoal(part2goal, part2amount), null);
            this.AddParameter(new PartGoal(part3goal, part3amount), null);
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
            return "Our task is to launch Skylab and deploy it in orbit.  Later we will send a crew to man the station.";
        }

        protected override string GetHashString()
        {
            return "Launch and deploy skylab into orbit";
        }
        protected override string GetTitle()
        {
            return "Launch and deployment of skylab over " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Skylab was a space station launched and operated by NASA and was the United States' first space station. Skylab orbited the Earth from 1973 to 1979, and included a workshop, " +
                "a solar observatory, and other systems. It was launched unmanned by a modified Saturn V rocket, with a weight of 169,950 pounds (77 t).[1] Three manned missions to the station, " +
                "conducted between 1973 and 1974 using the Apollo Command/Service Module (CSM) atop the smaller Saturn IB, each delivered a three-astronaut crew. On the last two manned missions, " +
                "an additional Apollo / Saturn IB stood by ready to rescue the crew in orbit if it was needed.\n\n" +
                "Our Goals \n\n 1. Launch skylab to space with specified equipment \n2. deploy skylab in desired orbit and await the next mission crew launch \n\n" +
                "Please note at the end of contract your active vessel will be recored as the SkyLab Station for all the next SkyLab Contracts!\n\n" +
                "Information on SkyLab was obtained from WikipidiA.";
        }
        protected override string GetSynopsys()
        {
            return "";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab1done = true;
            getVesselIDName(FlightGlobals.ActiveVessel);
            return "The station was damaged during launch when the micrometeoroid shield separated from the workshop and tore away, taking one of two main solar panel arrays with it and jamming " +
                "the other one so that it could not deploy. This deprived Skylab of most of its electrical power, and also removed protection from intense solar heating, threatening to make it unusable. \n\n" +
                "Our first mission to skylab will be to repair the station.  After we will have the crew conduct scientific studies as planned\n\n" +
                "Current vessel selected as SkyLab Base is " + FlightGlobals.ActiveVessel.name + "\n" +
                FlightGlobals.ActiveVessel.vesselName.Replace("(unloaded)", "") + " Vessel ID is " + FlightGlobals.ActiveVessel.id + "\n\n" +
                "If this is not correct you can change this is Debug menu using select SkyLab vessel Debug Tool";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
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

            if (SaveInfo.skylab1done == true || SaveInfo.Agena2Done == false || settings.all_Historical_Contracts_Off == true) { return false; }
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
        public string contTimeTitle = " Must keep crew in orbit for ";
        string repairParts = "repairParts";
        string Ctitle = "To Repair Station You must have at Least ";
        double RPamount = 1;
        CelestialBody targetBody = Planetarium.fetch.Home;
        ContractParameter skylab1;
        ContractParameter skylab2;
        ContractParameter skylab3;
        ContractParameter skylab4;
        ContractParameter skylab5;

        public int totalContracts;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab2>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }
            
            this.skylab1 = this.AddParameter(new TargetDockingGoal(SaveInfo.skyLabVesID, SaveInfo.skyLabName), null);
            skylab1.SetFunds(7000, targetBody);
            skylab1.SetReputation(5, targetBody);

            this.skylab2 = this.AddParameter(new RepairPanelPartCheck(contractName,SaveInfo.skyLabVesID,SaveInfo.skyLabName), null);
            skylab2.SetFunds(8000, targetBody);
            skylab2.SetReputation(7, targetBody);
            skylab2.SetScience(10, targetBody);
            
            this.skylab3 = this.AddParameter(new CollectScience(targetBody, BodyLocation.Space), null);
            skylab3.SetFunds(8500, targetBody);
            skylab3.SetReputation(10, targetBody);
            skylab3.SetScience(10, targetBody);
            
            this.skylab4 = this.AddParameter(new TimeCountdownDocking(targetBody, contractTime, contTimeTitle,SaveInfo.skyLabVesID), null);
            skylab4.SetFunds(20000, targetBody);
            skylab4.SetReputation(25, targetBody);
            skylab4.SetScience(25, targetBody);

            this.skylab5 = this.AddParameter(new LandingParameters(targetBody, true), null);
            skylab5.SetFunds(5000,75000,targetBody);
            skylab5.SetReputation(20, targetBody);
            skylab5.SetScience(3, targetBody);

            this.AddParameter(new ResourceSupplyGoal(repairParts, RPamount, Ctitle), null);

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
            return "Launch of SL-2 and Repair Mission";
        }
        protected override string GetTitle()
        {
            return "Launch SL-2 To (SkyLab) " + SaveInfo.skyLabName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "The station " + SaveInfo.skyLabName + " was damaged during launch when the micrometeoroid shield separated from the workshop and tore away, taking one of two main solar panel arrays with it and jamming the other" +
                "one so that it could not deploy. This deprived (Skylab) " + SaveInfo.skyLabName + " of most of its electrical power, and also removed protection from intense solar heating, threatening to make it unusable.\n\n" +
                "Our first crew we will send to the (SkyLab) " + SaveInfo.skyLabName + " will have to repair the Station before they can conduct any Science and stay on the station! So our objectives are:\n\n" +
                "1. Launch 3 Man Vessel to the station and Dock with it.\n 2. Repair the station using the Repair Panel placed on the vessel during Mission 1.\n 3. After repairs stay on station and conduct " +
                "scientific studies in orbit around Kerbin.\n 4. Keep crew in station and orbit for " + Tools.formatTime(contractTime) + "\n\n" +
                "Mission Controller will save the TimeCountdown to you persistent file, no need to actually stay in flight once countdown starts, you are free to do other task while contract countsdown\n\n"+
                "How To Repair SkyLab\n\n 1. Dock with skylab\n2.Transfer RepairParts from your orbitor to the Repair Panel on SkyLab.\n3.While still in vessel right click Repair Panel and Select Check If System is Ready "+
                "to repair.\n4. Go EVA and approach the Repair Panel on sky Lab.\n5. Open the repair panel door and select repair!  All done.";
        }
        protected override string GetSynopsys()
        {
            return "Launch 3 man vessel to (SkyLab) " + SaveInfo.skyLabName + " and conduct repair, science, and long duration mission";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab2done = true;
            return "Skylab 2 was the first manned mission to Skylab, the first U.S. orbital space station. The mission was launched on a Saturn IB rocket and carried a three-person "+
                "crew to the station. The name Skylab 2 also refers to the vehicle used for that mission. The Skylab 2 mission established a record for human spaceflight duration. Furthermore, its crew were the "+
                "first space station occupants ever to return safely to Earth – the only other space station occupants, the crew of the 1971 Soyuz 11 mission that had manned the Salyut 1 station, were killed during reentry.\n\n "+

                "The manned Skylab missions were officially designated Skylab 2, 3, and 4. Miscommunication about the numbering resulted in the mission emblems reading Skylab I, Skylab II, and Skylab 3 respectively.\n\n " +

                "Information of the SkyLab 2 Mission was gathered from Wikipedia";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            crew = int.Parse(node.GetValue("crewcount"));
            contractTime = int.Parse(node.GetValue("ctime"));
            contractName = node.GetValue("cname");
            contTimeTitle = node.GetValue("ctitle");

            repairParts = node.GetValue("rparts");
            Ctitle = node.GetValue("ctitle");
            RPamount = double.Parse(node.GetValue("rpamount"));

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("ctime", contractTime);
            node.AddValue("crewcount", crew);
            node.AddValue("cname", contractName);
            node.AddValue("ctitle", contTimeTitle);

            node.AddValue("rparts", repairParts);
            node.AddValue("ctitle", Ctitle);
            node.AddValue("rpamount", RPamount);
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.skylab2done == true || settings.all_Historical_Contracts_Off == true) { return false; }
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
        public string contTimeTitle = " Must keep crew in orbit for ";
      
        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter skylab1;
        ContractParameter skylab2;
        ContractParameter skylab3;
        ContractParameter skylab4;
        ContractParameter skylab5;

        public int totalContracts;

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (settings.all_Historical_Contracts_Off)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab3>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }

            this.skylab1 = this.AddParameter(new TargetDockingGoal(SaveInfo.skyLabVesID, SaveInfo.skyLabName), null);
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

            this.skylab4 = this.AddParameter(new TimeCountdownDocking(targetBody, contractTime, contTimeTitle, SaveInfo.skyLabVesID), null);
            skylab4.SetFunds(10000, targetBody);
            skylab4.SetReputation(15, targetBody);
            skylab4.SetScience(15, targetBody);

            this.skylab5 = this.AddParameter(new LandingParameters(targetBody, true), null);
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
            return "Launch of SL-3 Conduct Science while on station";
        }
        protected override string GetTitle()
        {
            return "Launch SL-3 To (SkyLab) " + SaveInfo.skyLabName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return "Skylab 3 was the second manned mission to the first American space station, Skylab. The mission began July 28, 1973, with the launch of three astronauts on the Saturn IB rocket, " +
                "and lasted 59 days, 11 hours and 9 minutes. A total of 1,084.7 astronaut-utilization hours were tallied by the Skylab 3 crew performing scientific experiments in the areas of medical activities, " +
                "solar observations, Earth resources, and other experiments.";
        }
        protected override string GetSynopsys()
        {
            return "Launch 3 man vessel to (SkyLab) " + SaveInfo.skyLabName + " and conduct science, and long duration mission";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab3done = true;
            return "During the approach phase, a propellant leak developed in one of the Apollo Service Module's reaction control system thruster quads. The crew was able to safely dock with Skylab, but "+
                "troubleshooting continued with the problem. Six days later, another thruster quad developed a leak, creating concern amongst Mission Control. For the first time, an Apollo spacecraft would be rolled "+
            "out to Launch Complex 39 for a rescue mission, made possible by the ability for the station to have two Apollo CSMs docked at the same time. It was eventually determined that the CSM could be safely maneuvered "+
            "using only two working thruster quads, and the rescue mission was never launched.\n\n"+

            "The crew, during their first EVA, installed the twin-pole sunshade, one of the two solutions for the destruction of the micrometeoroid shield during Skylab's launch to keep the space station cool. "+
            "It was installed over the parasol, which was originally deployed through a porthole airlock during Skylab 2. Both were brought to the station by Skylab 2.\n\n"+

            "Skylab 3 continued a comprehensive medical research program that extended the data on human physiological adaptation and readaptation to space flight collected on the previous Skylab 2 mission. "+
            "In addition, Skylab 3 extended the astronauts' stay in space from approximately one month to two months. Therefore, the effects of flight duration on physiological adaptation and "+
            "readaptation could be examined.\n\n"+

                "Information of the SkyLab 2 Mission was gathered from Wikipedia";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            crew = int.Parse(node.GetValue("crewcount"));
            contractTime = int.Parse(node.GetValue("ctime"));            
            contTimeTitle = node.GetValue("ctitle");            
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("ctime", contractTime);
            node.AddValue("crewcount", crew);
            node.AddValue("ctitle", contTimeTitle);          
        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.skylab3done == true || settings.all_Historical_Contracts_Off == true) { return false; }
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
        public string contTimeTitle = " Must keep crew in orbit for ";

        public double GMaxApA = 120000;
        public double GMinApA = 115000;
        public double GMaxPeA = 120000;
        public double GMinPeA = 115000;

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
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab4>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }

            GMaxApA = settings.skyLab4MaxApA;
            GMaxPeA = settings.skyLab4MaxPeA;
            GMinApA = GMaxApA - 5000;
            GMinPeA = GMaxPeA - 5000;

            this.skylab1 = this.AddParameter(new TargetDockingGoal(SaveInfo.skyLabVesID, SaveInfo.skyLabName), null);
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

            skylab6 = this.AddParameter(new ApAOrbitGoal(targetBody, (double)GMaxApA, (double)GMinApA,true), null);
            skylab6.SetFunds(1000, targetBody);

            skylab7 = this.AddParameter(new PeAOrbitGoal(targetBody, (double)GMaxPeA, (double)GMinPeA,true), null);
            skylab7.SetFunds(1000, targetBody);

            this.skylab4 = this.AddParameter(new TimeCountdownDocking(targetBody, contractTime, contTimeTitle, SaveInfo.skyLabVesID), null);
            skylab4.SetFunds(10000, targetBody);
            skylab4.SetReputation(15, targetBody);
            skylab4.SetScience(15, targetBody);

            this.skylab5 = this.AddParameter(new LandingParameters(targetBody, true), null);
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
            return "Launch of SL-4 Conduct Science while on station";
        }
        protected override string GetTitle()
        {
            return "Launch SL-4 To (SkyLab) " + SaveInfo.skyLabName;
        }
        protected override string GetDescription()
        {
           return "Skylab 4 was the third manned Skylab mission and placed the third and final crew aboard the first American space station. The mission started on November 16, 1973 with the launch of three astronauts "+
               "on a Saturn IB rocket from the Kennedy Space Center, Florida and lasted 84 days, one hour and 16 minutes. A total of 6,051 astronaut-utilization hours were tallied by Skylab 4 astronauts performing " +
               "scientific experiments in the areas of medical activities, solar observations, Earth resources, observation of the Comet Kohoutek and other experiments.";
        }
        protected override string GetSynopsys()
        {
            return "Launch 3 man vessel to (SkyLab) " + SaveInfo.skyLabName + " and conduct science, change Station altitude, and long duration mission";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.skylab4done = true;
            return "Skylab 4 was the last Skylab mission. The crew arrived aboard Skylab to find that they had company – three figures dressed in flight suits. Upon closer inspection, they found their companions were three "+
                "dummies, complete with Skylab 4 mission emblems and name tags which had been left there by Al Bean, Jack Lousma, and Owen Garriott at the end of Skylab 3.\n\n"+
                
            "The all-rookie astronaut crew had problems adjusting to the same workload level as their predecessors when activating the workshop. Things got off to a bad start after the crew attempted to hide Pogue's "+
            "early space sickness from flight surgeons, a fact discovered by mission controllers after downloading onboard voice recordings. The crew's initial task of unloading and stowing the thousands of items needed "+
            "for their lengthy mission also proved to be overwhelming. The schedule for the activation sequence dictated lengthy work periods with a large variety of tasks to be performed, and the crew soon found themselves "+
            "tired and behind schedule."+

            "as the activation of Skylab progressed, the astronauts complained of being pushed too hard. Ground crews disagreed; they felt that the astronauts were not working long enough or hard enough. During the course"+
            "of the mission, this culminated in a radio conference to air frustrations. Following this, the workload schedule was modified, and by the end of their mission the crew had completed even more work than had been"+
            "planned before launch. The experiences of the crew and ground controllers provided important lessons in planning subsequent manned spaceflight work schedules."+

            "On Thanksgiving Day, Gibson and Pogue accomplished a 61⁄2 hour spacewalk. The first part of their spacewalk was spent replacing film in the solar observatory. The remainder of the time was used to repair"+
            "a malfunctioning antenna."+

            "The crew reported that the food was good, but slightly bland. The crew would have preferred to use more condiments to enhance the taste of the food. The amount of salt they could use was restricted for medical"+
            "purposes. The quantity and type of food consumed was rigidly controlled because of their strict diet."+

            "Seven days into their mission, a problem developed in the Skylab attitude control gyroscope system, which threatened to bring an early end to the mission. Skylab depended upon three large gyroscopes, sized"+
            "so that any two of them could provide sufficient control and maneuver Skylab as desired. The third acted as a backup in the event of failure of one of the others. The gyroscope failure was attributed "+
            "to insufficient lubrication. Later in the mission, a second gyroscope showed similar problems, but special temperature control and load reduction procedures kept the second one operating, and no further "+
            "problems occurred." +

            "SL-4 Goals\n\n"+
            "1. Important Aspect of SL-4 Mission is to bring the station to a higher orbital level, so once you get into the station prepare to bring the station higher into orbit\n"+
            "2. Conduct an EVA at some point during mission.\n3. Conduct science inside the station to further kerbal understanding of Space and technology.\n\n"+

                "Information of the SkyLab 2 Mission was gathered from Wikipedia";
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            crew = int.Parse(node.GetValue("crewcount"));
            contractTime = int.Parse(node.GetValue("ctime"));
            contTimeTitle = node.GetValue("ctitle");

            double maxApa = double.Parse(node.GetValue("maxaPa"));
            GMaxApA = maxApa;
            double minApa = double.Parse(node.GetValue("minaPa"));
            GMinApA = minApa;

            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            GMaxPeA = masxPpaID;
            double minPeAID = double.Parse(node.GetValue("minpEa"));
            GMinPeA = minPeAID;
          
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("ctime", contractTime);
            node.AddValue("crewcount", crew);
            node.AddValue("ctitle", contTimeTitle);

            double maxApa = GMaxApA;
            node.AddValue("maxaPa", GMaxApA);
            double minApa = GMinApA;
            node.AddValue("minaPa", GMinApA);

            double maxPpAID = GMaxPeA;
            node.AddValue("maxpEa", GMaxPeA);
            double MinPeAID = GMinPeA;
            node.AddValue("minpEa", GMinPeA);

        }

        public override bool MeetRequirements()
        {
            if (SaveInfo.skylab4done == true || settings.all_Historical_Contracts_Off == true) { return false; }
            if (SaveInfo.skylab3done == false) { return false; }
            if (SaveInfo.Agena2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
}
