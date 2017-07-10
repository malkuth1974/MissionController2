using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using System.Text;
using KSPAchievements;
using MissionControllerEC.MCEParameters;

namespace MissionControllerEC.MCEContracts
{
    public class RoverContracts : Contract
    {
        private double RcLatitude = 0;
        private double RcLongitude = 0;
        public int totalContracts;
        public int TotalFinished;
        private string WheelModule = "ModuleWheelBase";
        CelestialBody targetBody;

        public void roverBodyNum(int bodyNum)
        {
            switch(bodyNum)
            {
                case 1:
                    targetBody = FlightGlobals.Bodies[2];
                    break;
                case 2:
                    targetBody = FlightGlobals.Bodies[6];
                    break;
                case 3:
                    targetBody = FlightGlobals.Bodies[7];
                    break;
                default:
                    targetBody = FlightGlobals.Bodies[6];
                    break;
            }
        }

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<RoverContracts>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<RoverContracts>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<IntergratedSettings>().MCERoverContracts)
            {
                return false;
            }
            roverBodyNum(Tools.RandomNumber(1, 3));
            
            if (targetBody == null)
            {
                return false;
            }
            RcLatitude = Tools.GetRandomLongOrLat(0, 180);
            RcLongitude = Tools.GetRandomLongOrLat(0, 180);
            if (RcLatitude == 0 || RcLongitude == 0)
            {
                return false;
            }
            this.AddParameter(new RoverLandingPositionCheck(targetBody, "Land a rover on ", RcLongitude, RcLatitude, 125, false), null);
            this.AddParameter(new ModuleGoal(WheelModule, "Wheels"), null);
            this.AddParameter(new GetCrewCount(0), null);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(3f, targetBody);
            base.SetFunds(5000, 70000, 90000, targetBody);
            base.SetReputation(25, 50, targetBody);
            base.SetScience(5, targetBody);
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
            return "Your Landing Zone is Longitude " + RcLongitude + " Latitude " + RcLatitude + " You will see a landing marker on the Map for " + targetBody.name;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + " Land rover " + " - Total Done: " + TotalFinished + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Launch Rover To " + targetBody.bodyName + " And Land";
        }
        protected override string GetDescription()
        {
            return "Land on duna with a rover";
        }
        protected override string GetSynopsys()
        {
            return "You must land on " + targetBody.name + " with a rover. If you check " + targetBody.name + " on the map screen you will see the landing site represented by a waypoint marker.  You must get as close " +
                " to this landing site as possible for the mission to be a success.  After you land we will periodically send you new contracts for this rover.  These contracts will require you to travel with " +
                " the rover to the new waypoint, or possibly multiple waypoints to conduct science.  Good luck.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.RoverLanded = true;
            SaveInfo.RoverName = FlightGlobals.ActiveVessel.vesselName.Replace("(unloaded)", "");
            SaveInfo.SavedRoverLat = RcLatitude;
            SaveInfo.savedRoverLong = RcLongitude;
            SaveInfo.RoverBody = targetBody.flightGlobalsIndex;
            return "Good job landing on, we will be sending you some more information.  Our scientist on the ground have found a few spots we want you to check out with rover, please check mission control." + targetBody.bodyName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "DunaTarget");
            Tools.ContractLoadCheck(node, ref RcLatitude, 0, RcLatitude, "RcLatitude");
            Tools.ContractLoadCheck(node, ref RcLongitude, 0, RcLongitude, "RcLongitude");
            Tools.ContractLoadCheck(node, ref WheelModule, "ModuleWheel", WheelModule, "WheelMod");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("DunaTarget", bodyID);
            node.AddValue("RcLatitude", RcLatitude);
            node.AddValue("RcLongitude", RcLongitude);
            node.AddValue("WheelMod", WheelModule);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("fieldScience") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            if (techUnlock && techUnlock2 && SaveInfo.RoverLanded == false)
                return true;
            else
                return false;
        }
    }
    public class RoverContractsDrive : Contract
    {
        private double RcLatitude = 0;
        private double RcLongitude = 0;
        public int totalContracts;
        public int TotalFinished;
        private string WheelModule = "ModuleWheelBase";
        private string RoverName = "None";
        CelestialBody targetBody;

        protected override bool Generate()
        {
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<RoverContractsDrive>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<RoverContractsDrive>().Count();

            if (totalContracts >= 1)
            {
                return false;
            }
            targetBody = FlightGlobals.Bodies[SaveInfo.RoverBody];
            if (targetBody == null)
            {
                return false;
            }
            RcLatitude = Tools.GetRandomLongOrLat(SaveInfo.SavedRoverLat, 5);
            RcLongitude = Tools.GetRandomLongOrLat(SaveInfo.savedRoverLong, 8);
            RoverName = SaveInfo.RoverName;
            if (RcLatitude == 0 || RcLongitude == 0)
            {
                return false;
            }
            this.AddParameter(new RoverGroundWaypointPara(targetBody, "Drive Rover", RcLongitude, RcLatitude, 2, RoverName), null);
            this.AddParameter(new CollectScience(targetBody, BodyLocation.Surface), null);
            this.AddParameter(new ModuleGoal(WheelModule, "Wheels"), null);
            this.AddParameter(new GetCrewCount(0), null);
            base.SetExpiry(3f, 10f);
            base.SetDeadlineYears(1f, targetBody);
            base.SetFunds(500, 5000, 3000, targetBody);
            base.SetReputation(5, 10, targetBody);
            base.SetScience(9, targetBody);
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
            return "Your waypoint is located at Longitude: " + RcLongitude + " Latitude: " + RcLatitude + " You will see a waypoint marker on the Map for " + targetBody.name;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + " drive rover " + " - Total Done: " + TotalFinished + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Drive Rover " + RoverName + " To Wapoint And Conduct Science.";
        }
        protected override string GetDescription()
        {
            return "Drive Rover to Destination waypoint and conduct science";
        }
        protected override string GetSynopsys()
        {
            return "You must drive your rover " + RoverName + "located on " + targetBody.name + " to the waypoint and conduct science experiments. If you check " + targetBody.name + " on the map screen you will see the ground waypoint represented by a waypoint marker.  You must get as close " +
                " to this waypoint as possible for the mission to be a success.";
        }
        protected override string MessageCompleted()
        {
            SaveInfo.RoverLanded = false;
            return "Good job driving to the waypoint, we will be sending you some more information. If we find any more point of interest to explore we will send you more information via the contracts system.";
        }
        protected override string MessageCancelled()
        {
            SaveInfo.RoverLanded = false;
            return "Rover Contract has been cancled resseting Rover Contracts";
        }
        protected override string MessageFailed()
        {
            SaveInfo.RoverLanded = false;
            return "You failed the rover drive contracts, we are pulling out all support of this mission.  Resseting contracts";
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "DunaTarget");
            Tools.ContractLoadCheck(node, ref RcLatitude, 0, RcLatitude, "RcLatitude");
            Tools.ContractLoadCheck(node, ref RcLongitude, 0, RcLongitude, "RcLongitude");
            Tools.ContractLoadCheck(node, ref WheelModule, "ModuleWheel", WheelModule, "WheelMod");
            Tools.ContractLoadCheck(node, ref RoverName, "Nothing Loaded", RoverName, "RoverName");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("DunaTarget", bodyID);
            node.AddValue("RcLatitude", RcLatitude);
            node.AddValue("RcLongitude", RcLongitude);
            node.AddValue("WheelMod", WheelModule);
            node.AddValue("RoverName", RoverName);
        }

        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("fieldScience") == RDTech.State.Available;
            bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            if (techUnlock && techUnlock2 && SaveInfo.RoverLanded == true)
                return true;
            else
                return false;
        }
    }
}