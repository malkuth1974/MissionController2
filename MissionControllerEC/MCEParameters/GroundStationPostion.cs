using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

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
            Debug.LogError("frequency is now set to " + frequency + " from " + coreFreq);
            this.PolarRegionLock = polarLock;
            wp = new FinePrint.Waypoint();
        }

        protected override string GetHashString()
        {
            return "Must be in Line of sight of " + stationName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Have line of sight of " + stationName + ". With Frequency: " + frequency;
        }

        protected override void OnRegister()
        {
            if (PolarRegionLock)
            {
                this.disableOnStateChange = true;
            }
            else
            this.DisableOnStateChange = false;

            if (Root.ContractState == Contract.State.Active && freqPass)
            {
                GameEvents.onFlightReady.Add(FlightReady);
                GameEvents.onVesselChange.Add(VesselChange);
                GameEvents.onTimeWarpRateChanged.Add(FlightReady);
                eventsAdded = true;
            }
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
                FinePrint.WaypointManager.RemoveWaypoint(wp);
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
                        bool AngleCheck = (GroundToVesselAngle <= 45 && GroundToVesselAngle >= 0);
                        if (this.State == ParameterState.Incomplete && freqPass)
                        {
                            if (AngleCheck)
                            {
                                base.SetComplete();
                                //Debug.Log("SetComplete Logitude is " + longitude + " Latitude is " + latitude);
                            }
                        }

                        if (this.State == ParameterState.Complete && freqPass)
                        {
                            if (!AngleCheck)
                                base.SetIncomplete();
                        }
                    }
                }
                
            }           
        }
        protected virtual void OnSatelliteChanged()
        {
            if (SatelliteTest != null)
            {
                SatelliteTest();
            }
            else
            {
                //Debug.Log("Event fired for OnSatelliteChanged! In GroundStation Parameter");
            }
        }

        public GroundStationPostion(float a)
        {
            SetGroundStationCheck(a);
            //Debug.Log("GroundStation Event Fired with float " + a);
        }
        public void SetGroundStationCheck(float Freq)
        {
            checkFrequency = Freq;
            OnSatelliteChanged();
            //Debug.Log("GroundStation Frequency set to " + checkFrequency);
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
            //Debug.Log("Angle: " + angle);
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
                    wp.celestialName = targetBody.theName;
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
                    Debug.LogError(r.Message + " " + r.Source);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + " " + e.Source);
                }
            }
            if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                if (this.Root.ContractState != Contract.State.Completed)
                {
                    try
                    {
                        wp.celestialName = targetBody.theName;
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
                        Debug.LogError(r.Message + " " + r.Source);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message + " " + e.Source);
                    }
                }
            }

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
}
