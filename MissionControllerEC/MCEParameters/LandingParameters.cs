using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC.MCEParameters
{
    class LandingParameters : ContractParameter
    {
        private CelestialBody targetBody;       
        private bool AllowLandedWet = true;
        private bool updated = false;
        private bool LaunchTrue = false;

        public LandingParameters()
        {
        }

        public LandingParameters(CelestialBody target, bool wetLanding)
        {
            this.targetBody = target;
            this.AllowLandedWet = wetLanding;
        }
        protected override string GetHashString()
        {
            return "Land Vessel" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            if (AllowLandedWet)
                return "Land Your Vessel on " + targetBody.theName;
            else
                return "Land Your Vessel on " + targetBody.theName + ".  You must also only land on dry land, landing Wet will not count!";
        }

        protected override void OnRegister()
        {

            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onLaunch.Add(onLaunch);
                updated = true;
            }

        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onLaunch.Remove(onLaunch);
            }

        }
            
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active && LaunchTrue)
            {
                if (AllowLandedWet)
                {
                    if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("Wet and dry landing accepted for contract");
                        }
                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && FlightGlobals.ActiveVessel.situation != Vessel.Situations.SPLASHED)
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("only dry landing accepted for this contract");
                        }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");          
            Tools.ContractLoadCheck(node, ref AllowLandedWet, true, AllowLandedWet , "wetland"); 
            Tools.ContractLoadCheck(node, ref LaunchTrue,true,LaunchTrue,"onlaunch");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("wetland", AllowLandedWet);
            node.AddValue("onlaunch", LaunchTrue);
        }

        public void Landing(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && LaunchTrue)
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    base.SetComplete();
                }                                   
            }           
        }
        public void onLaunch(EventReport er)
        {
            if (FlightGlobals.ActiveVessel.launchTime > this.Root.DateAccepted)
            {
                LaunchTrue = true;
                Debug.LogWarning("Onlaunch event fired for landing parameter Landing is now HOT and can be applied.  This message is good, means you launched your vessel");
                // strange that the eventReport comes up null when launching a vessel, I can't check against this event launch.  Always comes up NUll?
            }
            else
                Debug.LogError("Vessel is not classified as new vessel and was launched before current contract was accepted. " + " Vessel Name: " + FlightGlobals.ActiveVessel.name + " Launch Date: " + FlightGlobals.ActiveVessel.launchTime);
        }
    }
    class BiomLandingParameters : ContractParameter
    {
        private CelestialBody targetBody;
        private bool AllowLandedWet = true;
        private bool updated = false;
        private bool LaunchTrue = false;
        private string BiomeName = "Test";
        private string currentBiome = "Current Test";

        public BiomLandingParameters()
        {
        }

        public BiomLandingParameters(CelestialBody target, bool wetLanding, string biomename)
        {
            this.targetBody = target;
            this.AllowLandedWet = wetLanding;
            this.BiomeName = biomename;
        }
        protected override string GetHashString()
        {
            return "Land Vessel on biome " + BiomeName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {            
         return "Land Your Vessel on " + targetBody.theName + ". The biome name is: " + BiomeName;          
        }

        protected override string GetNotes()
        {
             return "You can use MCE ToolHelpers or other mods like MechJeb and Kerbal Engineer to get your current Biome Location!";                      
        }

        protected override void OnRegister()
        {

            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onLaunch.Add(onLaunch);
                updated = true;
            }

        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onLaunch.Remove(onLaunch);
            }

        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active && LaunchTrue)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    currentBiome = FlightGlobals.ActiveVessel.mainBody.BiomeMap.GetAtt(FlightGlobals.ActiveVessel.latitude * Math.PI / 180d, FlightGlobals.ActiveVessel.longitude * Math.PI / 180d).name;
                    GetNotes();
                }
                if (AllowLandedWet)
                {
                    if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("Wet and dry landing accepted for contract");
                        }
                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && FlightGlobals.ActiveVessel.situation != Vessel.Situations.SPLASHED)
                        if (this.state == ParameterState.Incomplete)
                        {
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("only dry landing accepted for this contract");
                        }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref AllowLandedWet, true, AllowLandedWet, "wetland");
            Tools.ContractLoadCheck(node, ref LaunchTrue, true, LaunchTrue, "onlaunch");
            Tools.ContractLoadCheck(node, ref BiomeName, "Test", BiomeName, "biomename");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("wetland", AllowLandedWet);
            node.AddValue("onlaunch", LaunchTrue);
            node.AddValue("biomename", BiomeName);
        }

        public void Landing(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && LaunchTrue)
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                {

                    if (currentBiome == BiomeName)
                    {
                        base.SetComplete();
                    }
                }
            }
        }
        public void onLaunch(EventReport er)
        {
            if (FlightGlobals.ActiveVessel.launchTime > this.Root.DateAccepted)
            {
                LaunchTrue = true;
                Debug.LogWarning("Onlaunch event fired for landing parameter Landing is now HOT and can be applied.  This message is good, means you launched your vessel");
                // strange that the eventReport comes up null when launching a vessel, I can't check against this event launch.  Always comes up NUll?
            }
            else
                Debug.LogError("Vessel is not classified as new vessel and was launched before current contract was accepted. " + " Vessel Name: " + FlightGlobals.ActiveVessel.name + " Launch Date: " + FlightGlobals.ActiveVessel.launchTime);
        }
    }

    class CheckLandingLonAndLat : ContractParameter
    {
        private CelestialBody targetBody;
        private bool AllowLandedWet = true;
        private double currentLon = 0;
        private double currentLat = 0;
        private double savedLon = 0;
        private double savedLat = 0;
        private bool hasToBeNewVessel = true;
        private string title = "Land at specific target area";
        
        public CheckLandingLonAndLat()
        {
        }

        public CheckLandingLonAndLat(CelestialBody target, bool WetDryLanding,double saveLon,double saveLat,string title,bool vesselHasBeNew)
        {
            this.targetBody = target;
            this.AllowLandedWet = WetDryLanding;
            this.savedLon = saveLon;
            this.savedLat = saveLat;
            this.title = title;
            this.hasToBeNewVessel = vesselHasBeNew;
        }
        protected override string GetHashString()
        {
            return "" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return ""+ title;
        }

        protected override string GetNotes()
        {
            return "this value is based off a Longitude and Latitude recorded when Vessel Landed originally";
        }
                     
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    currentLat = FlightGlobals.ActiveVessel.latitude;
                    currentLon = FlightGlobals.ActiveVessel.longitude;
                    GetNotes();
                }
                if (AllowLandedWet)
                {
                    if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
                        if (this.state == ParameterState.Incomplete)
                        {
                            currentLat = FlightGlobals.ActiveVessel.latitude;
                            currentLon = FlightGlobals.ActiveVessel.longitude;
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("Wet and dry landing accepted for contract");
                        }
                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && FlightGlobals.ActiveVessel.situation != Vessel.Situations.SPLASHED)
                        if (this.state == ParameterState.Incomplete)
                        {
                            currentLat = FlightGlobals.ActiveVessel.latitude;
                            currentLon = FlightGlobals.ActiveVessel.longitude;
                            Landing(FlightGlobals.ActiveVessel);
                            //Debug.Log("only dry landing accepted for this contract");
                        }
                }
            }
        }

        public void Landing(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && hasToBeNewVessel)
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH && vessel.launchTime > this.Root.DateAccepted)
                {
                    double latMin = savedLat - 3000;
                    double latMax = savedLat + 3000;
                    double lonMin = savedLon - 3000;
                    double lonMax = savedLon + 3000;

                    if (currentLat >= latMin && currentLat <= latMax && currentLon >= lonMin && currentLon <= lonMax)
                    {
                        base.SetComplete();
                    }
                }
            }
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && !hasToBeNewVessel)
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    double latMin = savedLat - 3000;
                    double latMax = savedLat + 3000;
                    double lonMin = savedLon - 3000;
                    double lonMax = savedLon + 3000;

                    if (currentLat >= latMin && currentLat <= latMax && currentLon >= lonMin && currentLon <= lonMax)
                    {
                        base.SetComplete();
                    }
                }
            }
        }        

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref AllowLandedWet, true, AllowLandedWet, "wetland");
            Tools.ContractLoadCheck(node, ref currentLat, 0, currentLat, "currentlat");
            Tools.ContractLoadCheck(node, ref currentLon, 0, currentLon, "currentlon");
            Tools.ContractLoadCheck(node, ref savedLat, 0, savedLat, "savedlat");
            Tools.ContractLoadCheck(node, ref savedLon, 0, savedLon, "savedlon");
            Tools.ContractLoadCheck(node, ref title, "Land at target area", title, "title");
            Tools.ContractLoadCheck(node, ref hasToBeNewVessel, true, hasToBeNewVessel, "newvessel");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("wetland", AllowLandedWet);
            node.AddValue("currentlat", currentLat);
            node.AddValue("currentlon", currentLon);
            node.AddValue("savedlat", savedLat);
            node.AddValue("savedlon", savedLon);
            node.AddValue("title", title);
            node.AddValue("newvessel", hasToBeNewVessel);
        }        
    }
}
