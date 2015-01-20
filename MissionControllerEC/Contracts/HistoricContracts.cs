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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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
                
                "Information on Vostok 1 was gathered from Wikipedia\n\n" +
                
                "Objectives are to: \n\n1. Enter Space \n2. Conduct at least one orbit of Kebin \n3. Return safely to Kerbin";
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
                "citizens... but it was the totalitarian state that overwhelmingly took the lead.\n\n"+

                "Commemorative monument, Vostok-1 landing site near Engels, Russia.\n\n"+

                "The landing site is now a monument park. The central feature in the park is a 25 meter tall monument that consists of a silver metallic rocket rising on a curved metallic "+
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
            if (SaveInfo.Vostok1Done == true) { return false; }
            else { return true; }
        }
    }
    #endregion
    # region Vostok 2
    public class Vostok2 : Contract
    {
        Settings settings = new Settings("config.cfg");
        public double minHeight = 70000;
        public double missionTime = 21600;

        public int crew = 1;

        CelestialBody targetBody = Planetarium.fetch.Home;

        ContractParameter vostok1;
        ContractParameter vostok2;
        ContractParameter vostok3;
        ContractParameter vostok4;

        protected override bool Generate()
        {
            
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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
                "to study the effects of a more prolonged period of weightlessness on the human body.  Titov orbited the Earth over 17 times, exceeding the single orbit of Yuri Gagarin on Vostok 1 "+
                "− as well as the suborbital spaceflights of American astronauts Alan Shepard and Gus Grissom aboard their respective Mercury-Redstone 3 and 4 missions. Indeed, Titov's number of orbits "+
                "and flight time would not be surpassed by an American astronaut until Gordon Cooper's Mercury-Atlas 9 spaceflight in May 1963.\n\n"+

                "Information on Vostok 2 was gathered from Wikipedia\n\n" +

                "Objectives are to: \n\n1. Enter Space \n\n2. Stay in orbit for a total of 1 day (6 Kerbal Hours) \n3. Return Home.";
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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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

                "Information on Voskhod 2 was gathered from Wikipedia\n\n" +

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

            "Information on Voskhod 2 was gathered from Wikipedia";
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

        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
            minHeight = Tools.RandomNumber((int)settings.vostok12height, (int)settings.vostok12height + 15000);
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

            this.AddParameter(new AltitudeGoal(targetBody, minHeight), null);
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new EccentricGoal(targetBody,Eccentricity,(Eccentricity + .1),false),null);
            if (TirosTitleMissionNumber == 3)
            {
                this.AddParameter(new Inclination(targetBody, 170, 180), null);
            }
            this.AddParameter(new TimeCountdownOrbits(targetBody, AmountDaysActive, true), null);
            if (TirosTitleMissionNumber == 2)
            {
                this.AddParameter(new PartGoal("2HOT Thermometer", 1), null);
                this.AddParameter(new PartGoal("Communotron 16", 1), null);
            }
            else if (TirosTitleMissionNumber == 3)
            {
                this.AddParameter(new PartGoal("Communotron 88-88", 1), null);
                this.AddParameter(new PartGoal("PresMat Barometer", 1), null);
                this.AddParameter(new PartGoal("2HOT Thermometer", 1), null);
            }
            else
            {
                this.AddParameter(new PartGoal("Communotron 16", 1), null);
            }         
            if (TirosTitleMissionNumber == 2 || TirosTitleMissionNumber == 3)
            {
                this.AddParameter(new ModuleGoal("ModuleDeployableSolarPanel", "Solar Panels"), null);
                this.AddParameter(new ResourceGoalCap("Electric power", 600), null);
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
            if (TirosTitleMissionNumber == 2)
            {
                return "Launch Tiros with new technology batteries and solar panels to keep satellite in orbit longer";
            }
            else if (TirosTitleMissionNumber == 3)
            {
                return "Launch Tiros/NOOA with batteries and solar panels to a Kebin Polar Orbit";
            }
            else
            {
                return "Launch the first weather satellite to a Low Kerbin Orbit and observe weather patterns";
            }
        }

        protected override string GetHashString()
        {
            if (TirosTitleMissionNumber == 3)
            {
                return "Tiros/NOOA - N";
            }
            if (TirosTitleMissionNumber == 2)
            {
                return "Tiros Number";
            }
            else
            {
                return "Tiros 1";
            }
        }
        protected override string GetTitle()
        {
            if (TirosTitleMissionNumber == 3)
            {
                return "Tiros/NOAA - N";
            }
            else if (TirosTitleMissionNumber == 2)
            {
                return "Tiros 7 (Television Infrared Observation Satellite)";
            }

            else
            {
                return "Tiros 1 (Television Infrared Observation Satellite)";
            }  
        }
        protected override string GetDescription()
        {
                        
            {
                if (TirosTitleMissionNumber == 3)
                {
                    return "The TIROS-N/NOAA Program (Television InfraRed Operational Satellite - Next-generation) was NASA's next step in improving the operational capability of the TIROS system first tried in " +
                        "the 1960's and the ITOS/NOAA system of the 1970's. Technological improvements integrated into the satellite system provided higher resolution imaging, and more day and night quantitative " +
                        "environmental data on local and global scales than seen with the two earlier generations of TIROS. Like earlier TIROS systems, NASA took responsibility for the satellite only until proven " +
                        "operational. Once operational the satellite's name was changed to 'NOAA' with day to day use under the direction of the National Oceanic and Atmospheric Administration.\n\n" +

                    "The TIROS-N/NOAA satellite series carried the Advanced Very High Resolution Radiometer (AVHRR). The AVHRR provided day and night cloud-top and sea surface temperatures, as well as ice and snow " +
                    "conditions. The satellite also carried an atmospheric sounding system (TOVS - TIROS Operational Vertical Sounder) which provided vertical profiles of temperature and water vapor from the Earth's " +
                    "surface to the top of the atmosphere; and a solar proton monitor to detect the arrival of energetic particles for use in solar storm prediction. For the first time, this satellite carried a data " +
                    "collection platform used to receive, process and store information from free floating balloons and buoys worldwide for transmission to one central processing facility.";
                }
                else
                {
                    return "The TIROS Program (Television Infrared Observation Satellite) was NASA's first experimental step to determine if satellites could be useful in the study of the Earth." +
                        "At that time, the effectiveness of satellite observations was still unproven. Since satellites were a new technology, the TIROS Program also tested various design issues for " +
                        "spacecraft: instruments, data and operational parameters. The goal was to improve satellite applications for Earth-bound decisions, such as should we evacuate the coast because of the hurricane?\n\n" +

                        "The TIROS Program's first priority was the development of a meteorological satellite information system. Weather forecasting was deemed the most promising application of space-based observations.\n\n" +

                        "TIROS proved extremely successful, providing the first accurate weather forecasts based on data gathered from space. TIROS began continuous coverage of the Earth's weather in 1962, and was used " +
                        "by meteorologists worldwide. The program's success with many instrument types and orbital configurations lead to the development of more sophisticated meteorological observation satellites.\n\n " +
                        "All information for the Tiros Missions were gathered from NASA.com";
                }
            }
        }
        protected override string GetSynopsys()
        {
            if (TirosTitleMissionNumber == 1)
            {
                return "Objectives: To test experimental television techniques designed to develop a worldwide meteorological satellite information system. To test Sun angle and horizon sensor systems for spacecraft " +
                    "orientation.\n\n" +

                "Description: The spacecraft was 42 inches in diameter, 19 inches high and weighed 270 pounds. The craft was made of aluminum alloy and stainless steel which was then covered by 9200 solar cells." +
                    "The solar cells served to charge the on-board batteries. Three pairs of solid-propellant spin rockets were mounted on the base plate.";
            }
            else if (TirosTitleMissionNumber == 2)
            {
                return "Objectives: Continue research and development of the meteorological satellite information system; obtain improved data for use in weather forecasting, especially during hurricane season.\n\n" +

                "Description: The spacecraft was 42 inches in diameter, 19 inches high and weighed 270 pounds. The craft was made of aluminum alloy and stainless steel then covered by 9200 solar cells. The solar" +
                "cells served to charge the nickel-cadmium (nicad) batteries. Three pairs of solid-propellant spin rockets were mounted on the base plate.\n\n" +

                "TIROS-7 was also designed to make infrared measurements of reflected solar and terrestrial radiation over selected spectrum ranges and gather data on electron density and temperature in space." +
                "To accomplish this new expanded mission, TIROS-7 carried two wide-angle camera systems, a magnetic tape recorder, and infrared experimentation equipment. The electron density and temperature probes" +
                "were the same as the ones flown on board Explorer 17." +

                "The spacecraft operating system still included the infrared horizon scanner, the north direction indicator, despin weights and spinup rockets, and the magnetic attitude control system. TIROS-7 was" +
                "deactivated after furnishing over 30,000 cloud photographs; it lasted the longest of the TIROS series thus far, 1809 days.";
        
            }
            else if (TirosTitleMissionNumber == 3)
            {
                return "TIROS-N was an experimental satellite which carried an Advanced Very High Resolution Radiometer (AVHRR) to provide day and night cloud top and sea surface temperatures, as well as ice and "+
                    "snow conditions; an atmospheric sounding system (TOVS - TIROS Operational Vertical Sounder) to provide vertical profiles of temperature and water vapor from the Earth's surface to the top "+
                    "of the atmosphere; and a solar proton monitor to detect the arrival of energetic particles for use in solar storm prediction. For the first time, this satellite also carried a data collection "+
                    "platform used to receive, process and store information from free floating balloons and buoys worldwide for transmission to one central processing facility.\n\n"+

                "TIROS-N was placed in a near circular, (470nm) polar orbit. The craft and its systems operated successfully, providing high-resolution scanned images and vertical temperature and moisture profiles "+
                    "to both operational meteorologists and private interests with APT and HRPT capability.";

            }
            else
            {
                return "Tiros Contract";
            }

        }
        protected override string MessageCompleted()
        {
            SaveInfo.tirosCurrentNumber++;
            if (TirosTitleMissionNumber == 2)
            {
                return "Good Job you have finsished this contract\n\n" +
                        "TIROS-7 STATS:\n" +
                        "Launch Date:  June 19, 1963\n" +
                        "Operational Period: 1809 days before being deactivated by NASA on June 3, 1968\n" +
                        "Launch Vehicle:    Three-Stage Delta\n" +
                        "Launch Site:    Cape Canaveral, FL\n" +
                        "Type:   Weather Satellite";
            }
            else if (TirosTitleMissionNumber == 3)
            {
                return "Good Job you have finsished this contract\n\n" +
                    "TIROS-N Stats:\n" +
                    "Launch Date:    October 13, 1978\n" +
                    "Operational Period:    Operational for 868 days until deactivated by NOAA on February 27, 1981\n" +
                    "Launch Vehicle:   Atlas E/F\n" +
                    "Launch Site:    Vandenberg Air Force Base, CA\n" +
                    "Type:   Weather Satellite";
            }
            else
            {
                return "Good Job you have finsished this contract\n\n"+
                    "TIROS-1 STATS:\n"+
                    "Launch Date:    April 1, 1960\n"+
                    "Operational Period: 78 days\n"+
                    "Launch Vehicle:    Standard Thor-Able\n"+
                    "Launch Site:    Cape Canaveral, FL\n"+
                    "Type:    Weather Satellite";
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref minHeight, settings.vostok12height, minHeight, "minheight");
            Tools.ContractLoadCheck(node, ref inclination, 180, inclination, "inclination");
            Tools.ContractLoadCheck(node, ref Eccentricity, .01, Eccentricity, "eccentricity");
            Tools.ContractLoadCheck(node, ref AmountDaysActive, 5, AmountDaysActive, "amountdays");
            Tools.ContractLoadCheck(node, ref contractMult, 1.0f, contractMult, "mult");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("minheight", minHeight);
            node.AddValue("inclination", inclination);
            node.AddValue("eccentricity", Eccentricity);
            node.AddValue("amountdays", AmountDaysActive);
            node.AddValue("mult", contractMult);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("spaceExploration") == RDTech.State.Available;
            bool techUnlock3 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            bool techUnlock4 = ResearchAndDevelopment.GetTechnologyState("advExploration") == RDTech.State.Available;
            if (TirosTitleMissionNumber == 1 && techUnlock)
            {
                return true;
            }
            else if (TirosTitleMissionNumber == 2 && techUnlock && techUnlock2 && techUnlock3)
            {
                return true;
            }
            else if (TirosTitleMissionNumber == 2 && techUnlock && techUnlock2 && techUnlock3 && techUnlock4)
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

        ContractParameter marinerOrbit;
        ContractParameter marinerOrbit2;
        ContractParameter marinerOrbit3;
        ContractParameter marinerOrbit4;
        ContractParameter marinerOrbit5;
        ContractParameter marinerOrbit6;
        
        
        protected override bool Generate()
        {
            if (prestige == ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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
            base.SetDeadlineYears(3f, targetBody);
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
            return "Conduct Flyby of " + targetBody.theName;
        }
        protected override string GetTitle()
        {
            if (marinerNumber == 4)
            {
                return "Voyager 1 Flyby Of Jool";
            }
            else if (marinerNumber == 3)
            {
                return "Mariner 10 Flyby Of Moho";
            }
            else if (marinerNumber == 2)
            {
                return "Mariner 4 Flyby Of Duna";
            }
            else
            {
                return "Mariner 2 flyby of Eve";
            }
        }
        protected override string GetDescription()
        {
            if (marinerNumber == 4)
            {
                return "Originally, a Mariner 11 and Mariner 12 were planned as part of the Mariner program, however, due to congressional budget cuts, the mission was scaled back to be a flyby of Jupiter and Saturn, " +
                    "and renamed the Mariner Jupiter-Saturn probes. As the program progressed, the name was later changed to Voyager, as the probe designs began to differ greatly from previous Mariner missions.\n\n" +

                    "The twin spacecraft Voyager 1 and Voyager 2 were launched by NASA in separate months in the summer of 1977 from Cape Canaveral, Florida. As originally designed, the Voyagers were to conduct closeup " +
                    "studies of Jupiter and Saturn, Saturn's rings, and the larger moons of the two planets.\n\n" +

                    "To accomplish their two-planet mission, the spacecraft were built to last five years. But as the mission went on, and with the successful achievement of all its objectives, the additional flybys " +
                    "of the two outermost giant planets, Uranus and Neptune, proved possible -- and irresistible to mission scientists and engineers at the Voyagers' home at the Jet Propulsion Laboratory in " +
                    "Pasadena, California.\n\n" +

                    "As the spacecraft flew across the solar system, remote-control reprogramming was used to endow the Voyagers with greater capabilities than they possessed when they left the Earth. Their two-planet " +
                    "mission became four. Their five-year lifetimes stretched to 12 and more." +

                    "Eventually, between them, Voyager 1 and 2 would explore all the giant outer planets of our solar system, 48 of their moons, and the unique systems of rings and magnetic fields those planets possess.\n\n" +
                    "Information on Voyager program gathered from NASA.";

            }
            if (marinerNumber == 3)
            {
                return "Mariner 10 was the seventh successful launch in the Mariner series, the first spacecraft to use the gravitational pull of one planet (Venus) to reach another (Mercury), and the first " +
                    "spacecraft mission to visit two planets. Mariner 10 was the first (and as of 2003 the only) spacecraft to visit Mercury. The spacecraft flew by Mercury three times in a retrograde heliocentric " +
                    "orbit and returned images and data on the planet. Mariner 10 returned the first-ever close-up images of Venus and Mercury. The primary scientific objectives of the mission were to measure Mercury's " +
                    "environment, atmosphere, surface, and body characteristics and to make similar investigations of Venus. Secondary objectives were to perform experiments in the interplanetary medium and to obtain " +
                    "experience with a dual-planet gravity-assist mission.\n\n" +

                    "All Mariner info gathered from NASA";
            }
            else if (marinerNumber == 2)
            {
                return "Mariner 4 was the first spacecraft to get a close look at Mars. Flying as close as 9,846 kilometers (6,118 miles), Mariner 4 revealed Mars to have a cratered, rust-colored surface, with signs " +
                    "on some parts of the planet that liquid water had once etched its way into the soil. In addition to various field and particle sensors and detectors, the spacecraft had a television camera, which " +
                    "took 22 television pictures covering about 1% of the planet. Initially stored on a 4-track tape recorder, these pictures took four days to transmit to Earth.\n\n" +

                    "All Mariner info gathered from NASA";
            }
            else
            {
                return "As plans were getting under way to explore the Moon with the Rangers and Surveyors, JPL and NASA also turned their attention to the rest of the solar system. The Mariner series of missions " +
                    "were designed to be the first U.S. spacecraft to other planets, specifically Venus and Mars. Mariner 1 and 2 were nearly identical spacecraft developed to fly by Venus. The rocket carrying" +
                    "Mariner 1 went off-course during launch on July 22, 1962, and was blown up by a range safety officer about 5 minutes into flight.\n\n" +

                "A month later, Mariner 2 was launched successfully on August 27, 1962, sending it on a 3-1/2-month flight to Venus. On the way it measured for the first time the solar wind, a constant stream of " +
                "charged particles flowing outward from the Sun. It also measured interplanetary dust, which turned out to be more scarce than predicted. In addition, Mariner 2 detected high-energy charged particles " +
                "coming from the Sun, including several brief solar flares, as well as cosmic rays from outside the solar system. As it flew by Venus on December 14, 1962, Mariner 2 scanned the planet with infrared and " +
                "microwave radiometers, revealing that Venus has cool clouds and an extremely hot surface. (Because the bright, opaque clouds hide the planet’s surface, Mariner 2 was not outfitted with a camera.) " +
                "Mariner 2's signal was tracked until January 3, 1963. The spacecraft remains in orbit around the Sun.\n\n" +

                "All info for Mariner was collect from Nasa";
            }
           
        }
        protected override string GetSynopsys()
        {
            if (marinerNumber == 4)
            {
                return "No real example of Saturn or Jupiter exist in Kerbal Space program, the only Gas Planet in KSP is Jool.  You are to explore Jool and its moon to complete this contract.";
            }
            if (marinerNumber == 3)
            {
                return "Moho is the Kerbal version of Mecury, in this mission you have to do a flyby of both Eve (venus) and Moho(Mercury) and collect science.";
            }
            else if (marinerNumber == 2)
            {
                return "Duna is the Kerbal version of Mars, in this mission you have to do a flyby of Duna and collect science.";
            }
            else
            {
                return "Eve is the kerbal version of Venus, in this mission you have to do a flyby of Eve and collect science.";
            }
        }
        protected override string MessageCompleted()
        {
            SaveInfo.marinerCurrentNumber++;
            if (marinerNumber == 4)
            {
                return "You have visited Jool and its moon, and your voyager craft is still going strong.. We hope.";
            }
            if (marinerNumber == 3)
            {
                return "Moho is the Kerbal version of Mecury, you have finished the contract!";
            }
            else if (marinerNumber == 2)
            {
                return "Duna is the Kerbal version of Mars, you have finished the contract!";
            }
            else
            {
                return "Eve is the kerbal version of Venus, you have finished the contract!";
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref PeA, 100000, PeA, "pea");
            Tools.ContractLoadCheck(node, ref marinerNumber, 1, marinerNumber, "mn");
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
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("spaceExploration") == RDTech.State.Available;
            bool techUnlock3 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            bool techUnlock4 = ResearchAndDevelopment.GetTechnologyState("advExploration") == RDTech.State.Available;
            if (SaveInfo.marinerCurrentNumber <= 4) { return true; }
            else { return false; }
            
        }
    }
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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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

                "Objectives are \n\n1. Launch Luna 2 \n2. Orbit the Mun \n3. Conduct Science at Mun \n 4. When finished crash vessel into the surface of Mun.";
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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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

            "Luna 16 was the first robotic probe to land on the Moon and return a sample of lunar soil to Earth. The sample was returned from Mare Fecunditatis. It represented "+
            "the first lunar sample return mission by the Soviet Union and was the third lunar sample return mission overall, following the Apollo 11 and Apollo 12 missions.\n\n"+

            "The spacecraft consisted of two attached stages, an ascent stage mounted on top of a descent stage. The descent stage was a cylindrical body with four protruding landing legs, fuel tanks, "+
            "a landing radar, and a dual descent engine complex.\n\n"+

            "A main descent engine was used to slow the craft until it reached a cutoff point which was determined by the on-board computer based on altitude and velocity. After cutoff a bank of lower "+
            "thrust jets was used for the final landing. The descent stage also acted as a launch pad for the ascent stage.\n\n"+

            "The ascent stage was a smaller cylinder with a rounded top. It carried a cylindrical hermetically sealed soil sample container inside a re-entry capsule.\n\n"+

            "The spacecraft descent stage was equipped with a television camera, radiation and temperature monitors, telecommunications equipment, and an extendable arm with a drilling rig for "+
            "the collection of a lunar soil sample.\n\n" +

                "Information on Luna 2 was gathered from Wikipedia.org\n\n" +

                "Objectives are \n\n1. Launch Luna 16 \n2. Orbit the Mun \n3. Land Mun (Samples are automated) \n 4. Return to Kerbin with samples.";
        }
        protected override string GetSynopsys()
        {
            return "Bring Luna 16 to orbit around " + targetBody.theName + ". " + " Land on Mun, Return To Kerbin with automated samples.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.Luna16Done = true;
            return "Three tiny samples (0.2 grams) of the Luna 16 soil were sold at Sotheby auction for $442,500 in 1993. The samples are the only lunar return material in private ownership during the "+
                "20th century. Another source of privately possessed moon rock is lunar meteorites of varying quality and authenticity, and another is lost Apollo moon rocks, possible legal issues aside."+

                "A series of 10-kopeck stamps was issued in 1970 to commemorate the flight of Luna 16 lunar probe and depicted the main stages of the program: soft landing on Moon, launch of the lunar soil "+
                "sample return capsule, and parachute assisted landing back on Earth.\n\n"+

                "Information on Luna 2 was gathered from Wikipedia.org";
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
            if (SaveInfo.Luna16Done == true) { return false; }
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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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
            string AgenaMessage = "Please take note that when you finish the Agena contract, that vessel will be recorded as the Agena Vessel for the next mission!\n\n" +
                "If the wrong vessel is recorded, you can change the vessel by using the Debug Tools in settings for Agena Contract.";
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
                "to perform large orbital changes, in preparation for the Apollo Program's lunar missions.\n\n" +
                "Your first task is to launch an Agena Type vehicle into orbit.\n\n" +
                "Please take note that when you finish the Agena Contract, that vessel will be recorded as the Agena Vessel for the next mission!";
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
                "The Gemini-Agena Target Vehicle design was an adaptation of the basic Agena-D vehicle using the alternate Model 8247 rocket engine and additional program-peculiar equipment required for the Gemini mission.\n\n" +
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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
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
            this.AddParameter(new ModuleGoal(partName, ModuleTitle), null);
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
            return "You have been succesful with Launching an Agena Type craft, docking with it, and changing your Orbital Altitude.  Congradulations!\n\n" +
                "The first GATV was launched on October 25, 1965 while the Gemini 6 astronauts were waiting on the pad. While the Atlas performed normally,\n" +
                "the Agena's engine exploded during orbital injection. Since the rendezvous and docking was the primary objective, the Gemini 6 mission was scrubbed,\n" +
                "and replaced with the alternate mission Gemini 6A, which rendezvoused (but could not dock) with Gemini 7 in December.\n\n" +
                "It was not until Gemini 10 that all objectives of Launching, Docking, and boosting Gemini 10 to 412-nautical-mile change succeded.";
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
            ModuleTitle = node.GetValue("mtitle");
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
            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
            minHeight = settings.skyLabheight;
            this.AddParameter(new AltitudeGoal(targetBody,minHeight), null);
            this.AddParameter(new InOrbitGoal(targetBody), null);
            this.AddParameter(new PartGoal(part1goal, "Small Repair Panel", part1amount,true), null);
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
                "a solar observatory, and other systems. It was launched unmanned by a modified Saturn V rocket, with a weight of 169,950 pounds (77 t).  Three manned missions to the station, " +
                "conducted between 1973 and 1974 using the Apollo Command/Service Module (CSM) atop the smaller Saturn IB, each delivered a three-astronaut crew. On the last two manned missions, " +
                "an additional Apollo / Saturn IB stood by ready to rescue the crew in orbit if it was needed.\n\n" +
                "Our Goals \n\n 1. Launch skylab to space with specified equipment \n2. deploy skylab in desired orbit and await the next mission crew launch \n\n" +
                "Please note that at the end of contract your active vessel will be recorded as the SkyLab Station for all the next SkyLab Contracts!\n\n" +
                "Information on SkyLab was obtained from Wikipidia.";
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

            if (SaveInfo.skylab1done == true || SaveInfo.Agena2Done == false) { return false; }
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
        string repairParts = "SpareParts";
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

            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }
            
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
                "Mission Controller will save the TimeCountdown to you persistent file. There's no need to actually stay in flight once countdown starts, so you are free to do other task while the contract counts down\n\n"+
                "How To Repair SkyLab\n\n 1. Dock with skylab\n2.Transfer RepairParts from your orbitor to the Repair Panel on SkyLab.\n3.While still in vessel right click Repair Panel and Select Check If System is Ready "+
                "to repair.\n4. Go EVA and approach the Repair Panel on SkyLab.\n5. Open the Repair Panel door and select Repair.  All done!";
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
            totalContracts = ContractSystem.Instance.GetCurrentContracts<SkyLab3>().Count();
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            if (totalContracts >= 1) { return false; }

            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }

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
            return "Launch of SL-3 & conduct science while on station";
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

            if (SaveInfo.all_Historical_Contracts_Off == true) { return false; }

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
            return "Launch of SL-4 & conduct science while on station";
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

            "On Thanksgiving Day, Gibson and Pogue accomplished a 6 1⁄2 hour spacewalk. The first part of their spacewalk was spent replacing film in the solar observatory. The remainder of the time was used to repair"+
            "a malfunctioning antenna."+

            "The crew reported that the food was good, but slightly bland. The crew would have preferred to use more condiments to enhance the taste of the food. The amount of salt they could use was restricted for medical"+
            "purposes. The quantity and type of food consumed was rigidly controlled because of their strict diet."+

            "Seven days into their mission, a problem developed in the Skylab attitude control gyroscope system, which threatened to bring an early end to the mission. Skylab depended upon three large gyroscopes, sized"+
            "so that any two of them could provide sufficient control and maneuver Skylab as desired. The third acted as a backup in the event of failure of one of the others. The gyroscope failure was attributed "+
            "to insufficient lubrication. Later in the mission, a second gyroscope showed similar problems, but special temperature control and load reduction procedures kept the second one operating, and no further "+
            "problems occurred." +

            "SL-4 Goals\n\n"+
            "1. An important aspect of the SL-4 Mission is to bring the station to a higher orbital level, so once you get into the station prepare to perform orbital maneuvers.\n"+
            "2. Conduct an EVA at some point during mission.\n3. Conduct science inside the station to further Kerbal understanding of space and technology.\n\n"+

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
            if (SaveInfo.skylab4done == true) { return false; }
            if (SaveInfo.skylab3done == false) { return false; }
            if (SaveInfo.Agena2Done == false) { return false; }
            else { return true; }
        }
    }
    #endregion
}
