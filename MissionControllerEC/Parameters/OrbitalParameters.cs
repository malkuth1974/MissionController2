using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{

    #region ApA OrbitGaol
    public class ApAOrbitGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double maxApA = 0.0;
        private double minApA = 0.0;
        private bool updated = false;

        public ApAOrbitGoal()
        {
        }

        public ApAOrbitGoal(CelestialBody target, double maxapA, double minapA)
        {
            this.targetBody = target;
            this.maxApA = maxapA;
            this.minApA = minapA;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Enter Orbit Around: " + targetBody.theName + "  MaxApA: " + maxApA + "  MinApA: " + minApA;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                    if (this.state == ParameterState.Incomplete)
                    {
                        Orbits(FlightGlobals.ActiveVessel);
                    }
                if (this.state == ParameterState.Complete)
                    {
                    OffOrbits(FlightGlobals.ActiveVessel);
                    }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double ApaID = double.Parse(node.GetValue("aPa"));
            maxApA = ApaID;
            double PeAID = double.Parse(node.GetValue("pEa"));
            minApA = PeAID;

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = maxApA;
            node.AddValue("aPa", ApAID);
            double PeAID = minApA;
            node.AddValue("pEa", PeAID);
        }

        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    if (vessel.orbit.ApA >= minApA && vessel.orbit.ApA <= maxApA)
                    {
                        base.SetComplete();
                    }
                }
                else
                    base.SetIncomplete();
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    if (vessel.orbit.ApA <= minApA && vessel.orbit.ApA >= maxApA)
                    {
                        base.SetIncomplete();
                    }
                }
                else
                    base.SetComplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region InOrbit Goal
    public class InOrbitGoal : ContractParameter
    {
        public CelestialBody targetBody;
        private bool updated = false;

        public InOrbitGoal()
        {
        }

        public InOrbitGoal(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Enter Orbit Around Goal: " + targetBody.theName;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {

                    InOrbit(FlightGlobals.ActiveVessel);
                }
            }
                
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
        }

        public void InOrbit(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (FlightGlobals.ActiveVessel)
                {
                    base.SetComplete();
                    ScreenMessages.PostScreenMessage("You Have achieved Orbit of Target Body: " + targetBody.theName);
                }
            }
        }

        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region PeA OrbitGoal
    public class PeAOrbitGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double maxPeA = 0.0;
        private double minPeA = 0.0;
        private bool updated = false;

        public PeAOrbitGoal()
        {
        }

        public PeAOrbitGoal(CelestialBody target, double maxpeA, double minpeA)
        {
            this.targetBody = target;
            this.maxPeA = maxpeA;
            this.minPeA = minpeA;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Enter Orbit Around PeA Goal: " + targetBody.theName + "  MaxPeA: " + maxPeA + "  MinPeA: " + minPeA;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                {
                    if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                    {
                        if (this.state == ParameterState.Incomplete)
                        {
                            Orbits(FlightGlobals.ActiveVessel);
                        }
                        if (this.state == ParameterState.Complete)
                        {
                            OffOrbits(FlightGlobals.ActiveVessel);
                        }
                    }
                }
            }

            if (HighLogic.LoadedSceneIsFlight && SaveInfo.MessageHelpers == true)
            {
                Tools.ObitalPeriodHelper(FlightGlobals.ActiveVessel);
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double masxPpaID = double.Parse(node.GetValue("maxpEa"));
            maxPeA = masxPpaID;
            double minPeAID = double.Parse(node.GetValue("minpEa"));
            minPeA = minPeAID;

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double maxPpAID = maxPeA;
            node.AddValue("maxpEa", maxPpAID);
            double MinPeAID = minPeA;
            node.AddValue("minpEa", MinPeAID);
        }

        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {

                    if (vessel.orbit.PeA >= minPeA && vessel.orbit.PeA <= maxPeA)
                    {
                        base.SetComplete();
                    }
                }
                else
                    base.SetIncomplete();
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {

                    if (vessel.orbit.PeA <= minPeA && vessel.orbit.PeA >= maxPeA)
                    {
                        base.SetIncomplete();
                    }
                }
                else
                    base.SetComplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region inclinationGoal

    public class Inclination : ContractParameter
    {
        Settings settings = new Settings("Config.cfg");
        CelestialBody targetBody;
        private double minInclination = 0.0;
        private double maxInclination = 0.0;
        private bool updated = false;

        public Inclination()
        {
        }

        public Inclination(CelestialBody body ,double minInc, double maxInc)
        {
            this.minInclination = minInc;
            this.maxInclination = maxInc;
            this.targetBody = body;
        }

        protected override string GetHashString()
        {
            return "Launch to Inclination" + maxInclination + minInclination;
        }
        protected override string GetTitle()
        {
            return "Reach Max Inclination Between: " + maxInclination + " and: " + minInclination;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        CheckInclination(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete)
                    {
                        OffCheckInclination(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            double maxincID = double.Parse(node.GetValue("maxincID"));
            maxInclination = maxincID;
            double minincID = double.Parse(node.GetValue("minincID"));
            minInclination = minincID;
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double maxincID = maxInclination;
            node.AddValue("maxincID", maxInclination);
            double minincID = minInclination;
            node.AddValue("minincID", minInclination);
        }

        public void CheckInclination(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (FlightGlobals.ActiveVessel)
                {
                    if (vessel.orbit.inclination <= maxInclination && vessel.orbit.inclination >= minInclination)
                        base.SetComplete();
                }
            }
        }
        public void OffCheckInclination(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (FlightGlobals.ActiveVessel)
                {
                    if (vessel.orbit.inclination >= maxInclination && vessel.orbit.inclination <= minInclination)
                        base.SetIncomplete();
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region EccentricGoal

    public class EccentricGoal : ContractParameter
    {
        CelestialBody targetBody;
        private double mineccn = 0.0;
        private double maxeccn = 0.0;
        private bool updated = false;

        public EccentricGoal()
        {
        }

        public EccentricGoal(CelestialBody body,double minEcc, double maxEcc)
        {
            this.mineccn = minEcc;
            this.maxeccn = maxEcc;
            this.targetBody = body;
        }

        protected override string GetHashString()
        {
            return "Bring vessel into target orbital eccentricity";
        }
        protected override string GetTitle()
        {
            return "Bring vessel to Target Eccentricity between " + mineccn.ToString("F2") + " " + maxeccn.ToString("F2");
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (this.Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        CheckEccentricity(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete)
                    {
                        ReCheckEccentricity(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetbody");
            Tools.ContractLoadCheck(node,ref mineccn,0,mineccn,"mineccn");
            Tools.ContractLoadCheck(node,ref maxeccn,.2,mineccn,"maxeccn");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("mineccn", mineccn);
            node.AddValue("maxeccn", maxeccn);
        }

        public void CheckEccentricity(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.situation == Vessel.Situations.ORBITING && vessel.orbit.eccentricity <= maxeccn && vessel.orbit.eccentricity >= mineccn)
                    base.SetComplete();
            }
        }
        public void ReCheckEccentricity(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.situation == Vessel.Situations.ORBITING && vessel.orbit.eccentricity > maxeccn && vessel.orbit.eccentricity < mineccn)
                    base.SetIncomplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region OrbiatlPeriod Goal
    public class OrbitalPeriod : ContractParameter
    {
        CelestialBody targetBody;
        private double minOrbitalPeriod = 0.0;
        private double maxOrbitalPeriod = 0.0;
        private bool updated = false;

        public OrbitalPeriod()
        {
        }

        public OrbitalPeriod(CelestialBody body,double minOrb, double maxOrb)
        {
            this.minOrbitalPeriod = minOrb;
            this.maxOrbitalPeriod = maxOrb;
            this.targetBody = body;
        }

        protected override string GetHashString()
        {
            return "Launch to Orbital Period" + maxOrbitalPeriod + minOrbitalPeriod;
        }
        protected override string GetTitle()
        {
            return "Reach Orbital Period Between: " + Tools.ConvertMinsHours(maxOrbitalPeriod) + " and: " + Tools.ConvertMinsHours(minOrbitalPeriod);
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                    if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                    {
                        if (this.state == ParameterState.Incomplete)
                        {
                            CheckOrbitalPeriod(FlightGlobals.ActiveVessel);
                        }
                        if (this.state == ParameterState.Complete)
                        {
                            OffCheckOrbitalPeriod(FlightGlobals.ActiveVessel);
                        }
                        if (HighLogic.LoadedSceneIsFlight && SaveInfo.MessageHelpers == true)
                        {
                            Tools.ObitalPeriodHelper(FlightGlobals.ActiveVessel);
                        }
                    }             
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double maxOrbID = double.Parse(node.GetValue("maxOrbID"));
            maxOrbitalPeriod = maxOrbID;
            double minOrbID = double.Parse(node.GetValue("minOrbID"));
            minOrbitalPeriod = minOrbID;
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double maxOrbID = maxOrbitalPeriod;
            node.AddValue("maxOrbID", maxOrbitalPeriod);
            double minOrbID = minOrbitalPeriod;
            node.AddValue("minOrbID", minOrbitalPeriod);
        }

        public void CheckOrbitalPeriod(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    if (vessel.orbit.period <= maxOrbitalPeriod && vessel.orbit.period >= minOrbitalPeriod)
                        base.SetComplete();
                }
            }
        }
        public void OffCheckOrbitalPeriod(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    if (vessel.orbit.period >= maxOrbitalPeriod && vessel.orbit.period <= minOrbitalPeriod)
                        base.SetComplete();
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region AltitudeGoal
    public class AltitudeGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double minAlt = 0.0;
        private bool updated = false;

        public AltitudeGoal()
        {
        }

        public AltitudeGoal(CelestialBody target, double minapA)
        {
            this.targetBody = target;
            this.minAlt = minapA;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Achieve an altitude of at least: " + minAlt;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        Orbits(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete)
                    {
                        OffOrbits(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            minAlt = double.Parse(node.GetValue("alt"));
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("alt", minAlt);
        }

        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    if (vessel.orbit.altitude >= minAlt)
                    {
                        base.SetComplete();
                    }
                }
                else
                    base.SetIncomplete();
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    if (vessel.orbit.altitude <= minAlt)
                    {
                        base.SetComplete();
                    }
                }
                else
                    base.SetIncomplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }


    }
    #endregion
    #region Agena In Orbit Goal
    public class AgenaInOrbit : ContractParameter
    {
        private CelestialBody targetBody;
        private string vesselID = "none";
        private bool updated = false;

        public AgenaInOrbit()
        {
        }

        public AgenaInOrbit(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Build and Launch Agena Target Vehicle\n" +
                "Your current active vehicle at launch will be\n" +
                "Recorded as the Agena Vehicle";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH)
                launchAgena(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            vesselID = node.GetValue("vesselid");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("vesselid", vesselID);
        }

        public void launchAgena(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                base.SetComplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region Crash Goal
    public class CrashGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private bool ReadyToCrash = false;
        private bool updated = false;

        public CrashGoal()
        {
        }

        public CrashGoal(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Crash your vessel into " + targetBody;
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                ReadyToCrash = true;
            }
        }

        protected override void OnRegister()
        {
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onCrash.Add(crashGoal);
                updated = true;
            }

        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onCrash.Remove(crashGoal);
            }

        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            ReadyToCrash = bool.Parse(node.GetValue("readybool"));
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("readybool", ReadyToCrash);
        }

        public void crashGoal(EventReport ev)
        {
            if (ReadyToCrash)
                base.SetComplete();
        }

        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
}
