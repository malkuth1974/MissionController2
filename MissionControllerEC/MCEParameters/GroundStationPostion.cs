using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;
using KSP.Localization;
using static MissionControllerEC.RegisterToolbar;

namespace MissionControllerEC.MCEParameters
{
    class GroundStationPostion : ContractParameter
    {
        private FinePrint.Waypoint wp;
        private bool submittedWaypoint;
        private CelestialBody targetBody = Planetarium.fetch.Home;
        private double longitude = 0;
        private double latitude = 0;
        private string stationName = "none";
        private float frequency = 0;
        bool eventsAdded;
        bool freqPass = false;
        private bool PolarRegionLock = false;
        bool angleCheck = false;

        public static float checkFrequency;
        public delegate void OnGroundStationCall();
        public event OnGroundStationCall SatelliteTest;

        public GroundStationPostion()
        {
            wp = new FinePrint.Waypoint();
        }

        public GroundStationPostion(string NameStation, double longitudeValue,double latitudeValue,float coreFreq,bool polarLock)
        {
            this.stationName = NameStation;
            this.longitude = longitudeValue;
            this.latitude = latitudeValue;
            this.frequency = coreFreq;
            //Log.Error("frequency is now set to " + frequency + " from " + coreFreq);
            this.PolarRegionLock = polarLock;
            wp = new FinePrint.Waypoint();
        }

        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000206") + " " + stationName + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000206 = Must be in Line of sight of 
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000207") + " " + stationName + " " + Localizer.Format("#autoLOC_MissionController2_1000208") + " " + frequency;		// #autoLOC_MissionController2_1000207 = Have line of sight of 		// #autoLOC_MissionController2_1000208 = . With Frequency: 
        }

        protected override void OnRegister()
        {
            if (PolarRegionLock)
            {
                this.disableOnStateChange = true;
            }
            else
            {
                this.DisableOnStateChange = false;
            }

            if (Root.ContractState == Contract.State.Active && freqPass)
            {
                GameEvents.onFlightReady.Add(FlightReady);
                GameEvents.onVesselChange.Add(VesselChange);
                GameEvents.onTimeWarpRateChanged.Add(FlightReady);
                eventsAdded = true;
            }
            else { }
        }

        protected override void OnUnregister()
        {
            if (eventsAdded)
            {
                GameEvents.onFlightReady.Remove(FlightReady);
                GameEvents.onVesselChange.Remove(VesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(FlightReady);
            }
            if (submittedWaypoint)
            {
                FinePrint.WaypointManager.RemoveWaypoint(wp);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                    {                                                                 
                        if (frequency == checkFrequency)
                        {
                            freqPass = true;
                        }
                        if (frequency != checkFrequency)
                        {
                            freqPass = false;
                        }
                        float GroundToVesselAngle = CheckVectorAngle2Objects(FlightGlobals.ActiveVessel,longitude,latitude);
                        if (FlightGlobals.ActiveVessel.orbit.altitude > 1000000)
                        {
                            angleCheck = (GroundToVesselAngle <= 45 && GroundToVesselAngle >= 0);
                        }
                        if (FlightGlobals.ActiveVessel.orbit.altitude <= 1000000 && FlightGlobals.ActiveVessel.orbit.altitude >= 500000)
                        {
                            angleCheck = (GroundToVesselAngle <= 65 && GroundToVesselAngle >= 0);
                        }
                        if (FlightGlobals.ActiveVessel.orbit.altitude < 500000)
                        {
                            angleCheck = (GroundToVesselAngle <= 85 && GroundToVesselAngle >= 0);
                        }

                        if (this.State == ParameterState.Incomplete && freqPass)
                        {
                            if (angleCheck)
                            {
                                base.SetComplete();
                                MCESatelliteCore.GroundStationLockedText = "Locked";
                                //Log.Info("SetComplete Logitude is " + longitude + " Latitude is " + latitude);
                            }
                            else { MCESatelliteCore.GroundStationLockedText = "NoLock"; }
                        }

                        if (this.State == ParameterState.Complete && freqPass)
                        {
                            if (!angleCheck)
                                base.SetIncomplete();
                            else { }
                        }
                        else { }
                    }
                    else { }
                }
                else { }

            }
            else { }
        }
        protected virtual void OnSatelliteChanged()
        {
            if (SatelliteTest != null)
            {
                SatelliteTest();
            }
            else
            {
                //Log.Info("Event fired for OnSatelliteChanged! In GroundStation Parameter");
            }
        }

        public GroundStationPostion(float a)
        {
            SetGroundStationCheck(a);
            //Log.Info("GroundStation Event Fired with float " + a);
        }
        public void SetGroundStationCheck(float Freq)
        {
            checkFrequency = Freq;
            OnSatelliteChanged();
            //Log.Info("GroundStation Frequency set to " + checkFrequency);
        }
        private void FlightReady()
        {
            base.SetIncomplete();
        }

        private void VesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
        private float CheckVectorAngle2Objects(Vessel v, double LongT,double latT)
        {           
            Vector3 groundStation = new Vector3();
            groundStation = Planetarium.fetch.Home.GetWorldSurfacePosition(latT, LongT, 0);
            Vector3 groundStationUpVec = (groundStation - v.mainBody.position);
            Vector3 groundToVesselVec = (v.rootPart.transform.position - groundStation);
            float angle = Vector3.Angle(groundToVesselVec, groundStationUpVec);
            //Log.Info("MCE GroundStation Angle: " + angle + " Vessel Height is: " + v.heightFromSurface);
            return angle;          
        }
        
        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node, ref longitude, 0, longitude, "long");
            Tools.ContractLoadCheck(node, ref latitude, 0, latitude, "lati");
            Tools.ContractLoadCheck(node, ref stationName, "None", stationName, "station");
            Tools.ContractLoadCheck(node, ref frequency, 5, frequency, "freq");
            Tools.ContractLoadCheck(node, ref PolarRegionLock, false, PolarRegionLock, "polarlock");

            if (HighLogic.LoadedSceneIsFlight && this.Root.ContractState == Contract.State.Active)
            {
                try
                {
                    wp.celestialName = targetBody.name;
                    wp.latitude = latitude;
                    wp.longitude = longitude;
                    wp.seed = Root.MissionSeed;
                    wp.id = "dish";
                    wp.name = stationName;
                    wp.index = 1;
                    wp.altitude = 0;
                    wp.isOnSurface = true;
                    wp.isNavigatable = true;
                    FinePrint.WaypointManager.AddWaypoint(wp);
                    submittedWaypoint = true;
                }
                catch (ArgumentOutOfRangeException r)
                {
                    Log.Error(r.Message + " " + r.Source);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message + " " + e.Source);
                }
            }
            if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                if (this.Root.ContractState != Contract.State.Completed)
                {
                    if (this.Root.ContractState != Contract.State.Cancelled)
                    {
                        try
                        {
                            wp.celestialName = targetBody.name;
                            wp.latitude = latitude;
                            wp.longitude = longitude;
                            wp.seed = Root.MissionSeed;
                            wp.id = "dish";
                            wp.name = stationName;
                            wp.index = 1;
                            wp.altitude = 0;
                            wp.isOnSurface = true;
                            wp.isNavigatable = true;
                            FinePrint.WaypointManager.AddWaypoint(wp);
                            submittedWaypoint = true;
                        }
                        catch (ArgumentOutOfRangeException r)
                        {
                            Log.Error(r.Message + " " + r.Source);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message + " " + e.Source);
                        }
                    }
                    else { }
                }
                else { }
            }
            else { }

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetbody", bodyID);
            node.AddValue("long", longitude);
            node.AddValue("lati", latitude);
            node.AddValue("station", stationName);
            node.AddValue("freq", frequency);
            node.AddValue("polarlock", PolarRegionLock);
        }
    }
    class RoverLandingPositionCheck : ContractParameter
    {
        private FinePrint.Waypoint wp;
        private bool submittedWaypoint;
        private CelestialBody targetBody = Planetarium.fetch.Home;
        private double longitude = 0;
        private double latitude = 0;
        private double SavedLong;
        private double SavedLat;
        private string LandingName = "none";
        bool eventsAdded;
        private double MarginOfErrorInDegree = 10;
        private bool HasToBeNewVessel = false;
      
        public RoverLandingPositionCheck()
        {
            wp = new FinePrint.Waypoint();
        }

        public RoverLandingPositionCheck(CelestialBody targetBody,string landingSite, double longitudeValue, double latitudeValue,double margofErrInDegree,bool HasToBeNewVessel)
        {
            this.targetBody = targetBody;
            this.SavedLat = longitudeValue;
            this.SavedLong = latitudeValue;
            this.LandingName = landingSite;
            this.HasToBeNewVessel = HasToBeNewVessel;
            this.MarginOfErrorInDegree = margofErrInDegree;
            wp = new FinePrint.Waypoint();
        }

        protected override string GetHashString()
        {
            return "Land rover at " + SavedLong + " " + SavedLat;
        }
        protected override string GetTitle()
        {
            return LandingName + targetBody.name + " at specific location marked map";
        }

        protected override void OnRegister()
        {
                     

            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(FlightReady);
                GameEvents.onVesselChange.Add(VesselChange);
                eventsAdded = true;
            }
            else { }
        }

        protected override void OnUnregister()
        {
            if (eventsAdded)
            {
                GameEvents.onFlightReady.Remove(FlightReady);
                GameEvents.onVesselChange.Remove(VesselChange);
            }
            if (submittedWaypoint)
            {
                FinePrint.WaypointManager.RemoveWaypoint(wp);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    latitude = FlightGlobals.ActiveVessel.latitude;
                    longitude = FlightGlobals.ActiveVessel.longitude;
                    GetNotes();
                    if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && this.state == ParameterState.Incomplete)
                    {
                        latitude = FlightGlobals.ActiveVessel.latitude;
                        longitude = FlightGlobals.ActiveVessel.longitude;
                        Landing(FlightGlobals.ActiveVessel);
                    }
                    else { }
                }               
            }
            else { }
        }

        public void Landing(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && HasToBeNewVessel)
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH && vessel.launchTime > this.Root.DateAccepted)
                {
                    double latMin = SavedLat - MarginOfErrorInDegree;
                    double latMax = SavedLat + MarginOfErrorInDegree;
                    double lonMin = SavedLong - MarginOfErrorInDegree;
                    double lonMax = SavedLong + MarginOfErrorInDegree;

                    if (latitude >= latMin && latitude <= latMax && longitude >= lonMin && longitude <= lonMax)
                    {
                        base.SetComplete();
                    }
                    else { }
                }
                else { }
            }
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && !HasToBeNewVessel)
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    double latMin = SavedLat - 3000;
                    double latMax = SavedLat + 3000;
                    double lonMin = SavedLong - 3000;
                    double lonMax = SavedLong + 3000;

                    if (latitude >= latMin && latitude <= latMax && longitude >= lonMin && longitude <= lonMax)
                    {
                        base.SetComplete();
                    }
                    else { }
                }
                else { }
            }
            else { }
        }
                
        private void FlightReady()
        {
            base.SetIncomplete();
        }

        private void VesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
        
        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node, ref longitude, 0, longitude, "long");
            Tools.ContractLoadCheck(node, ref latitude, 0, latitude, "lati");
            Tools.ContractLoadCheck(node, ref LandingName, "None", LandingName, "landingName");
            Tools.ContractLoadCheck(node, ref SavedLat, 100, SavedLat, "savedLat");
            Tools.ContractLoadCheck(node, ref SavedLong, 100, SavedLong, "savedLong");
            Tools.ContractLoadCheck(node, ref HasToBeNewVessel, false, HasToBeNewVessel, "vesselNew");

            if (HighLogic.LoadedSceneIsFlight && this.Root.ContractState == Contract.State.Active)
            {
                try
                {
                    wp.celestialName = targetBody.name;
                    wp.latitude = SavedLat;
                    wp.longitude = SavedLong;
                    wp.seed = Root.MissionSeed;
                    wp.id = "dish";
                    wp.name = LandingName;
                    wp.index = 1;
                    wp.altitude = 0;
                    wp.isOnSurface = true;
                    wp.isNavigatable = true;
                    FinePrint.WaypointManager.AddWaypoint(wp);
                    submittedWaypoint = true;
                }
                catch (ArgumentOutOfRangeException r)
                {
                    Log.Error(r.Message + " " + r.Source);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message + " " + e.Source);
                }
            }
            if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                if (this.Root.ContractState != Contract.State.Completed)
                {
                    if (this.Root.ContractState != Contract.State.Cancelled)
                    {
                        try
                        {
                            wp.celestialName = targetBody.name;
                            wp.latitude = SavedLat;
                            wp.longitude = SavedLong;
                            wp.seed = Root.MissionSeed;
                            wp.id = "dish";
                            wp.name = LandingName;
                            wp.index = 1;
                            wp.altitude = 0;
                            wp.isOnSurface = true;
                            wp.isNavigatable = true;
                            FinePrint.WaypointManager.AddWaypoint(wp);
                            submittedWaypoint = true;
                        }
                        catch (ArgumentOutOfRangeException r)
                        {
                            Log.Error(r.Message + " " + r.Source);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message + " " + e.Source);
                        }
                    }
                    else { }
                }
                else { }
            }
            else { }

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetbody", bodyID);
            node.AddValue("long", longitude);
            node.AddValue("lati", latitude);
            node.AddValue("landingName", LandingName);
            node.AddValue("savedLong", SavedLong);
            node.AddValue("savedLat", SavedLat);
            node.AddValue("vesselNew", HasToBeNewVessel);
        }
    }
    class RoverGroundWaypointPara : ContractParameter
    {
        private FinePrint.Waypoint wp;
        private bool submittedWaypoint;
        private CelestialBody targetBody = Planetarium.fetch.Home;
        private double longitude = 0;
        private double latitude = 0;
        private double SavedLong;
        private double SavedLat;
        private string GroundWaypointName = "none";
        bool eventsAdded;
        private double MarginOfErrorInDegree = 10;
        private string RoverName = "None";

        public RoverGroundWaypointPara()
        {
            wp = new FinePrint.Waypoint();
        }

        public RoverGroundWaypointPara(CelestialBody targetBody, string groundWaypointName, double longitudeValue, double latitudeValue, double margofErrInDegree, string RoverName)
        {
            this.targetBody = targetBody;
            this.SavedLat = longitudeValue;
            this.SavedLong = latitudeValue;
            this.GroundWaypointName = groundWaypointName;
            this.MarginOfErrorInDegree = margofErrInDegree;
            this.RoverName = RoverName;
            wp = new FinePrint.Waypoint();
        }

        protected override string GetHashString()
        {
            return "Bring Rover To Waypoint At " + SavedLong + " " + SavedLat;
        }
        protected override string GetTitle()
        {
            return GroundWaypointName + " " + RoverName + "located On " + targetBody.name + " To specific location marked on map";
        }

        protected override void OnRegister()
        {


            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(FlightReady);
                GameEvents.onVesselChange.Add(VesselChange);
                eventsAdded = true;
            }
            else { }
        }

        protected override void OnUnregister()
        {
            if (eventsAdded)
            {
                GameEvents.onFlightReady.Remove(FlightReady);
                GameEvents.onVesselChange.Remove(VesselChange);
            }
            if (submittedWaypoint)
            {
                FinePrint.WaypointManager.RemoveWaypoint(wp);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    latitude = FlightGlobals.ActiveVessel.latitude;
                    longitude = FlightGlobals.ActiveVessel.longitude;
                    GetNotes();
                    if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED && this.state == ParameterState.Incomplete)
                    {
                        latitude = FlightGlobals.ActiveVessel.latitude;
                        longitude = FlightGlobals.ActiveVessel.longitude;
                        Landing(FlightGlobals.ActiveVessel);
                    }
                    else { }
                }
            }
            else { }
        }

        public void Landing(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    double latMin = SavedLat - MarginOfErrorInDegree;
                    double latMax = SavedLat + MarginOfErrorInDegree;
                    double lonMin = SavedLong - MarginOfErrorInDegree;
                    double lonMax = SavedLong + MarginOfErrorInDegree;

                    if (latitude >= latMin && latitude <= latMax && longitude >= lonMin && longitude <= lonMax)
                    {
                        base.SetComplete();
                    }
                    else { }
                }
                else { }
            }           
            else { }
        }

        private void FlightReady()
        {
            base.SetIncomplete();
        }

        private void VesselChange(Vessel v)
        {
            base.SetIncomplete();
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node, ref longitude, 0, longitude, "long");
            Tools.ContractLoadCheck(node, ref latitude, 0, latitude, "lati");
            Tools.ContractLoadCheck(node, ref GroundWaypointName, "None", GroundWaypointName, "landingName");
            Tools.ContractLoadCheck(node, ref SavedLat, 100, SavedLat, "savedLat");
            Tools.ContractLoadCheck(node, ref SavedLong, 100, SavedLong, "savedLong");
            Tools.ContractLoadCheck(node, ref RoverName, "Name Not Loaded", RoverName, "roverName");

            if (HighLogic.LoadedSceneIsFlight && this.Root.ContractState == Contract.State.Active)
            {
                try
                {
                    wp.celestialName = targetBody.name;
                    wp.latitude = SavedLat;
                    wp.longitude = SavedLong;
                    wp.seed = Root.MissionSeed;
                    wp.id = "dish";
                    wp.name = GroundWaypointName;
                    wp.index = 1;
                    wp.altitude = 0;
                    wp.isOnSurface = true;
                    wp.isNavigatable = true;
                    FinePrint.WaypointManager.AddWaypoint(wp);
                    submittedWaypoint = true;
                }
                catch (ArgumentOutOfRangeException r)
                {
                    Log.Error(r.Message + " " + r.Source);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message + " " + e.Source);
                }
            }
            if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                if (this.Root.ContractState != Contract.State.Completed)
                {
                    if (this.Root.ContractState != Contract.State.Cancelled)
                    {
                        try
                        {
                            wp.celestialName = targetBody.name;
                            wp.latitude = SavedLat;
                            wp.longitude = SavedLong;
                            wp.seed = Root.MissionSeed;
                            wp.id = "dish";
                            wp.name = GroundWaypointName;
                            wp.index = 1;
                            wp.altitude = 0;
                            wp.isOnSurface = true;
                            wp.isNavigatable = true;
                            FinePrint.WaypointManager.AddWaypoint(wp);
                            submittedWaypoint = true;
                        }
                        catch (ArgumentOutOfRangeException r)
                        {
                            Log.Error(r.Message + " " + r.Source);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message + " " + e.Source);
                        }
                    }
                    else { }
                }
                else { }
            }
            else { }

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetbody", bodyID);
            node.AddValue("long", longitude);
            node.AddValue("lati", latitude);
            node.AddValue("landingName", GroundWaypointName);
            node.AddValue("savedLong", SavedLong);
            node.AddValue("savedLat", SavedLat);
            node.AddValue("roverName", RoverName);
        }
    }
}
