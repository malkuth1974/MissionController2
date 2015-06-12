using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC.MCEParameters
{

    #region ApA OrbitGaol
    public class ApAOrbitGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double ApA = 0.0;
        private bool updated = false;
        private bool LockOut = false;
        private string OrbitType = "Orbit";

        public ApAOrbitGoal()
        {
        }

        public ApAOrbitGoal(CelestialBody target, double maxapA,string typeOfOrbit)
        {
            this.targetBody = target;
            this.ApA = maxapA;
            this.LockOut = false;
            this.OrbitType = typeOfOrbit;
        }

        public ApAOrbitGoal(CelestialBody target, double maxapA, bool lockout,string typeOfOrbit)
        {
            this.targetBody = target;
            this.ApA = maxapA;
            this.LockOut = lockout;
            this.OrbitType = typeOfOrbit;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Enter " + OrbitType + " Around: " + targetBody.theName + "  With APA Of: " + ApA;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !LockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onTimeWarpRateChanged.Add(flightReady);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(flightReady);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        Orbits(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete && !LockOut)
                    {
                        OffOrbits(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");          
            Tools.ContractLoadCheck(node, ref ApA, 71000, ApA , "aPa");           
            Tools.ContractLoadCheck(node, ref LockOut, true, LockOut, "lockout");
            Tools.ContractLoadCheck(node,ref OrbitType,"Orbit",OrbitType,"typeorbit");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = ApA;
            node.AddValue("aPa", ApAID);           
            node.AddValue("lockout", LockOut);
            node.AddValue("typeorbit", OrbitType);
        }

        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    double minApA = ApA - 1000;
                    double maxApA = ApA + 1000;
                    if (vessel.orbit.ApA >= minApA && vessel.orbit.ApA <= maxApA)
                    {
                        base.SetComplete();
                    }
                }
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    double minApA = ApA - 1000;
                    double maxApA = ApA + 1000;
                    if (vessel.orbit.ApA <= minApA && vessel.orbit.ApA >= maxApA)
                    {
                        base.SetIncomplete();
                    }
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
    #region  Sat ApA OrbitGaol
    public class SatApAOrbitGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double ApA = 0.0;
        private bool updated = false;
        private bool LockOut = false;
        private string OrbitType = "Orbit";

        public SatApAOrbitGoal()
        {
        }

        public SatApAOrbitGoal(CelestialBody target, double mapA,string typeOfOrbit)
        {
            this.targetBody = target;
            this.ApA = mapA;
            this.LockOut = false;
            this.OrbitType = typeOfOrbit;
        }

        public SatApAOrbitGoal(CelestialBody target, double mapA, bool lockout,string typeOfOrbit)
        {
            this.targetBody = target;
            this.ApA = mapA;
            this.LockOut = lockout;
            this.OrbitType = typeOfOrbit;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Enter "+ OrbitType + " Around: " + targetBody.theName + " With ApA of At Least: " + ApA;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !LockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onTimeWarpRateChanged.Add(flightReady);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(flightReady);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        Orbits(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete && !LockOut && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                    {
                        OffOrbits(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");          
            Tools.ContractLoadCheck(node, ref ApA, 71000, ApA , "aPa");           
            Tools.ContractLoadCheck(node, ref LockOut, true, LockOut, "lockout");
            Tools.ContractLoadCheck(node,ref OrbitType,"Orbit",OrbitType,"typeorbit");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("aPa", ApA);
            node.AddValue("lockout", LockOut);
            node.AddValue("typeorbit", OrbitType);
        }
        satelliteCoreCheck sc = new satelliteCoreCheck();
        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    double minApA = ApA - 1000;
                    double maxApA = ApA + 1000;
                    if (vessel.orbit.ApA >= minApA && vessel.orbit.ApA <= maxApA)
                    {
                        base.SetComplete();
                        sc.SetApaBoolSatelliteCoreValue(true);
                    }
                }             
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                {
                    double minApA = ApA - 1000;
                    double maxApA = ApA + 1000;
                    if (vessel.orbit.ApA <= minApA && vessel.orbit.ApA >= maxApA)
                    {
                        base.SetIncomplete();
                    }
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
    #region InOrbit Goal
    public class InOrbitGoal : ContractParameter
    {
        public CelestialBody targetBody;

        public InOrbitGoal()
        {
        }

        public InOrbitGoal(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Enter Orbit Around Goal: " + targetBody.theName;
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");          
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
                    //ScreenMessages.PostScreenMessage("You Have achieved Orbit of Target Body: " + targetBody.theName);
                }
            }
        }             
    }
    #endregion
    #region PeA OrbitGoal
    public class PeAOrbitGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double PeA = 0.0;
        private bool updated = false;
        private bool lockOut = false;
        private string orbitType = "Orbit";

        public PeAOrbitGoal()
        {
        }

        public PeAOrbitGoal(CelestialBody target, double maxpeA,string typeOrbit)
        {
            this.targetBody = target;
            this.PeA = maxpeA;
            this.lockOut = false;
            this.orbitType = typeOrbit;
        }

        public PeAOrbitGoal(CelestialBody target, double maxpeA, bool lockout,string typeOrbit)
        {
            this.targetBody = target;
            this.PeA = maxpeA;
            this.lockOut = lockout;
            this.orbitType = typeOrbit;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Enter " + orbitType + " Around " + targetBody.theName + "  With PeA Of: " + PeA;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !lockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onTimeWarpRateChanged.Add(flightReady);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(flightReady);
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
                        if (this.state == ParameterState.Complete && !lockOut)
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref PeA, 71000, PeA, "pea");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");
            Tools.ContractLoadCheck(node, ref orbitType, "orbit", orbitType, "orbittype");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("pea", PeA);
            node.AddValue("lockout", lockOut);
            node.AddValue("orbittype", orbitType);
        }

        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    double minPeA = PeA - 1000;
                    double maxPeA = PeA + 1000;
                    if (vessel.orbit.PeA >= minPeA && vessel.orbit.PeA <= PeA)
                    {
                        base.SetComplete();
                    }
                }
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    double minPeA = PeA - 1000;
                    double maxPeA = PeA + 1000;
                    if (vessel.orbit.PeA <= minPeA && vessel.orbit.PeA >= PeA)
                    {
                        base.SetIncomplete();
                    }
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
    #region Sat PeA OrbitGoal
    public class SatPeAOrbitGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double PeA = 0.0;
        private bool updated = false;
        private bool lockOut = false;
        private string orbitType = "Orbit";

        public SatPeAOrbitGoal()
        {
        }

        public SatPeAOrbitGoal(CelestialBody target, double maxpeA, string typeOrbit)
        {
            this.targetBody = target;
            this.PeA = maxpeA;
            this.lockOut = false;
            this.orbitType = typeOrbit;
        }

        public SatPeAOrbitGoal(CelestialBody target, double maxpeA, bool lockout, string typeOrbit)
        {
            this.targetBody = target;
            this.PeA = maxpeA;
            this.lockOut = lockout;
            this.orbitType = typeOrbit;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Enter " + orbitType + " Around " + targetBody.theName + " With PeA of At Least: " + PeA;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !lockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onTimeWarpRateChanged.Add(flightReady);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(flightReady);
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
                        if (this.state == ParameterState.Complete && !lockOut)
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref PeA, 71000, PeA, "maxpEa");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");
            Tools.ContractLoadCheck(node, ref orbitType, "orbit", orbitType, "orbittype");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("maxpEa", PeA);
            node.AddValue("lockout", lockOut);
            node.AddValue("orbittype", orbitType);
        }
        satelliteCoreCheck sc = new satelliteCoreCheck();
        public void Orbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    double minPeA = PeA - 1000;
                    double maxPeA = PeA + 1000;
                    if (vessel.orbit.PeA >= minPeA && vessel.orbit.PeA <= PeA)
                    {
                        base.SetComplete();
                        sc.SetPeABoolSatelliteCoreValue(true);
                    }
                }
            }
        }
        public void OffOrbits(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    double minPeA = PeA - 1000;
                    double maxPeA = PeA + 1000;
                    if (vessel.orbit.PeA <= minPeA && vessel.orbit.PeA >= PeA)
                    {
                        base.SetIncomplete();
                    }
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
    #region inclinationGoal

    public class Inclination : ContractParameter
    {
        Settings settings = new Settings("Config.cfg");
        CelestialBody targetBody;
        private double InclinationValue = 0.0;
        private bool updated = false;
        private bool lockOut = false;

        public Inclination()
        {
        }

        public Inclination(CelestialBody body ,double minInc)
        {
            this.InclinationValue = minInc;
            this.targetBody = body;
            this.lockOut = false;
        }

        public Inclination(CelestialBody body, double minInc, bool lockout)
        {
            this.InclinationValue = minInc;
            this.targetBody = body;
            this.lockOut = lockout;
        }

        protected override string GetHashString()
        {
            return "Launch to Inclination" + InclinationValue + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Reach an inclination of: " + InclinationValue;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !lockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onTimeWarpRateChanged.Add(flightReady);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(flightReady);
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
                    if (this.state == ParameterState.Complete && !lockOut)
                    {
                        OffCheckInclination(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref InclinationValue, 70500, InclinationValue, "minincID");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);           
            double minincID = InclinationValue;
            node.AddValue("minincID", InclinationValue);
            node.AddValue("lockout", lockOut);
        }

        public void CheckInclination(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (FlightGlobals.ActiveVessel)
                {
                    double maxInclination = InclinationValue + 3;
                    double minInclination = InclinationValue - 3;
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
                    double maxInclination = InclinationValue + 3;
                    double minInclination = InclinationValue - 3;
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
        private double eccentricity = 0.0;
        private bool updated = false;
        private bool lockOut = false;

        public EccentricGoal()
        {
        }

        public EccentricGoal(CelestialBody body,double minEcc)
        {
            this.eccentricity = minEcc;
            this.targetBody = body;
            this.lockOut = false;
        }

        public EccentricGoal(CelestialBody body, double minEcc, bool lockout)
        {
            this.eccentricity = minEcc;
            this.targetBody = body;
            this.lockOut = lockout;
        }

        protected override string GetHashString()
        {
            return "Bring vessel into target orbital eccentricity" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Bring vessel to Target Eccentricity of " + eccentricity.ToString("F2");
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !lockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onTimeWarpRateChanged.Add(flightReady);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated && !lockOut)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onTimeWarpRateChanged.Remove(flightReady);
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
                    if (this.state == ParameterState.Complete && !lockOut)
                    {
                        ReCheckEccentricity(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node,ref eccentricity,0,eccentricity,"mineccn");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("mineccn", eccentricity);
            node.AddValue("lockout", lockOut);
        }

        public void CheckEccentricity(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                double maxeccn = eccentricity + .5;
                double mineccn = eccentricity - .5;
                if (vessel.situation == Vessel.Situations.ORBITING && vessel.orbit.eccentricity <= maxeccn && vessel.orbit.eccentricity >= mineccn)
                    base.SetComplete();
            }
        }
        public void ReCheckEccentricity(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                double maxeccn = eccentricity + .5;
                double mineccn = eccentricity - .5;
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
        private bool lockOut = false;

        public OrbitalPeriod()
        {
        }

        public OrbitalPeriod(CelestialBody body,double minOrb, double maxOrb)
        {
            this.minOrbitalPeriod = minOrb;
            this.maxOrbitalPeriod = maxOrb;
            this.targetBody = body;
            this.lockOut = false;
        }

        public OrbitalPeriod(CelestialBody body, double minOrb, double maxOrb, bool lockout)
        {
            this.minOrbitalPeriod = minOrb;
            this.maxOrbitalPeriod = maxOrb;
            this.targetBody = body;
            this.lockOut = lockout;
        }

        protected override string GetHashString()
        {
            return "Launch to Orbital Period" + maxOrbitalPeriod + minOrbitalPeriod + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Reach Orbital Period Between: " + Tools.ConvertMinsHours(minOrbitalPeriod) + " and: " + Tools.ConvertMinsHours(maxOrbitalPeriod);
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !lockOut)
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
                        if (this.state == ParameterState.Complete && !lockOut)
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref maxOrbitalPeriod, 10000, maxOrbitalPeriod, "maxOrbID");
            Tools.ContractLoadCheck(node, ref minOrbitalPeriod, 95000, minOrbitalPeriod, "minOrbID");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double maxOrbID = maxOrbitalPeriod;
            node.AddValue("maxOrbID", maxOrbitalPeriod);
            double minOrbID = minOrbitalPeriod;
            node.AddValue("minOrbID", minOrbitalPeriod);
            node.AddValue("lockout", lockOut);
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
    #region AltitudeGoal
    public class AltitudeGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double minAlt = 0.0;
        private bool updated = false;
        private bool lockOut = false;

        public AltitudeGoal()
        {
        }

        public AltitudeGoal(CelestialBody target, double minapA)
        {
            this.targetBody = target;
            this.minAlt = minapA;
            this.lockOut = false;
        }

        public AltitudeGoal(CelestialBody target, double minapA, bool lockout)
        {
            this.targetBody = target;
            this.minAlt = minapA;
            this.lockOut = lockout;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Achieve an altitude of at least: " + minAlt;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !lockOut)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated && !lockOut)
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
                    if (this.state == ParameterState.Complete && !lockOut)
                    {
                        OffOrbits(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref minAlt, 71000, minAlt, "alt");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("alt", minAlt);
            node.AddValue("lockout", lockOut);
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
                        base.SetIncomplete();
                    }
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
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref vesselID, "DefaultLoaded", vesselID, "vesselid");
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
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref ReadyToCrash, false, ReadyToCrash, "readybool");
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
    #region Flyby Goal
    public class FlyByCelestialBodyGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private double maxheight = 0.0;
        private double minheight = 0.0;
        private bool lockOut = false;

        public FlyByCelestialBodyGoal()
        {
        }

        public FlyByCelestialBodyGoal(CelestialBody target, double maxpeA, double minpeA)
        {
            this.targetBody = target;
            this.maxheight = maxpeA;
            this.minheight = minpeA;
            this.lockOut = false;
        }

        public FlyByCelestialBodyGoal(CelestialBody target, double maxpeA, double minpeA, bool lockout)
        {
            this.targetBody = target;
            this.maxheight = maxpeA;
            this.minheight = minpeA;           
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "FlyBy: " + targetBody.theName + "  Between an altitude of: " + maxheight + "  And: " + minheight;
        }       

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
                    {
                        if (this.state == ParameterState.Incomplete)
                        {
                            flyby(FlightGlobals.ActiveVessel);
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
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref maxheight, 71000, maxheight, "maxpEa");
            Tools.ContractLoadCheck(node, ref minheight, 70500, minheight, "minpEa");
            Tools.ContractLoadCheck(node, ref lockOut, true, lockOut, "lockout");

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double maxPpAID = maxheight;
            node.AddValue("maxpEa", maxPpAID);
            double MinPeAID = minheight;
            node.AddValue("minpEa", MinPeAID);
            node.AddValue("lockout", lockOut);
        }

        public void flyby(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {

                    if (vessel.orbit.altitude >= minheight && vessel.orbit.altitude <= maxheight)
                    {
                        base.SetComplete();
                    }
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
}
