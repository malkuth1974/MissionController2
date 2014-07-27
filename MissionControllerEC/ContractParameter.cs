using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{
    #region ApA OrbitGaol
    public class ApAOrbitGoal : ContractParameter
    {
        public CelestialBody targetBody;
        public double maxApA = 0.0;
        public double minApA = 0.0;

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
            return "Enter Orbit Around ApA Goal: " + targetBody.theName + "  MaxApA: " + maxApA + "  MinApA: " + minApA;
        }

        protected override void OnUpdate()
        {
            Orbits(FlightGlobals.ActiveVessel);
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
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
            {
                if (vessel.orbit.ApA >= minApA && vessel.orbit.ApA <= maxApA)
                {
                    base.SetComplete();
                    Debug.Log("Min And Max ApA has been Completed");
                }
            }
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
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Enter Orbit Around Goal: " + targetBody.theName;
        }

        protected override void OnRegister()
        {
            GameEvents.onVesselOrbitClosed.Add(InOrbit);
        }
        protected override void OnUnregister()
        {
            GameEvents.onVesselOrbitClosed.Remove(InOrbit);
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
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
            {
                if (FlightGlobals.ActiveVessel.orbit.referenceBody.bodyName.Equals(targetBody))
                {
                    base.SetComplete();
                    ScreenMessages.PostScreenMessage("You Have achieved Orbit of Target Body: " + targetBody + " Contract Complete");
                }
            }
        }
    }
    #endregion
    #region PeA OrbitGoal
    public class PeAOrbitGoal : ContractParameter
    {
        public CelestialBody targetBody;
        public double maxPeA = 0.0;
        public double minPeA = 0.0;

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

        protected override void OnUpdate()
        {
            Orbits(FlightGlobals.ActiveVessel);
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
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
            {
                if (vessel.orbit.PeA >= minPeA && vessel.orbit.PeA <= maxPeA)
                {
                    base.SetComplete();
                    Debug.Log("Min And Max PeA has been Completed");
                }
            }
        }
    }
    #endregion
    #region inclinationGoal

    public class Inclination : ContractParameter
    {
        public double minInclination = 0.0;
        public double maxInclination = 0.0;

        public Inclination()
        {
        }

        public Inclination(double minInc, double maxInc)
        {
            this.minInclination = minInc;
            this.maxInclination = maxInc;
        }

        protected override string GetHashString()
        {
            return "Launch to Inclination" + maxInclination + minInclination;
        }
        protected override string GetTitle()
        {
            return "Reach Max Inclination Between: " + maxInclination + " and: " + minInclination;
        }
       
        protected override void OnUpdate()
        {
            CheckInclination(FlightGlobals.ActiveVessel);        
        }

        protected override void OnLoad(ConfigNode node)
        {
                       
            double maxincID = double.Parse(node.GetValue("maxincID"));
            maxInclination = maxincID;
            double minincID = double.Parse(node.GetValue("minincID"));
            minInclination = minincID;
        }
        protected override void OnSave(ConfigNode node)
        {
            double maxincID = maxInclination;
            node.AddValue("maxincID", maxInclination);
            double minincID = minInclination;
            node.AddValue("minincID", minInclination);
        }

        public void CheckInclination(Vessel vessel)
        {     
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                if (vessel.orbit.inclination <= maxInclination && vessel.orbit.inclination >= minInclination)
                    base.SetComplete();
            }
        }
    }
        #endregion
    #region OrbiatlPeriod Goal
    public class OrbitalPeriod : ContractParameter
    {
        public double minOrbitalPeriod = 0.0;
        public double maxOrbitalPeriod = 0.0;

        public OrbitalPeriod()
        {
        }

        public OrbitalPeriod(double minOrb, double maxOrb)
        {
            this.minOrbitalPeriod = minOrb;
            this.maxOrbitalPeriod = maxOrb;
        }

        protected override string GetHashString()
        {
            return "Launch to Orbital Period" + maxOrbitalPeriod + minOrbitalPeriod;
        }
        protected override string GetTitle()
        {
            return "Reach Orbital Period Between: " + Tools.formatTime(maxOrbitalPeriod) + " and: " + Tools.formatTime(minOrbitalPeriod);
        }
        
        protected override void OnUpdate()
        {
            CheckOrbitalPeriod(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            double maxOrbID = double.Parse(node.GetValue("maxOrbID"));
            maxOrbitalPeriod = maxOrbID;
            double minOrbID = double.Parse(node.GetValue("minOrbID"));
            minOrbitalPeriod = minOrbID;
        }
        protected override void OnSave(ConfigNode node)
        {
            double maxOrbID = maxOrbitalPeriod;
            node.AddValue("maxOrbID", maxOrbitalPeriod);
            double minOrbID = minOrbitalPeriod;
            node.AddValue("minOrbID", minOrbitalPeriod);
        }

        public void CheckOrbitalPeriod(Vessel vessel)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                if (vessel.orbit.period <= maxOrbitalPeriod && vessel.orbit.period >= minOrbitalPeriod)
                    base.SetComplete();
            }
        }
    }
    #endregion
    #region PartGoal
    public class PartGoal : ContractParameter
    {
        public String partName = "";
        public int partCount = 0;
        public int maxPartCount = 0;

        public PartGoal()
        {
        }

        public PartGoal(string name, int maxCount)
        {
            this.partName = name;
            this.maxPartCount = maxCount;
                
        }

        protected override string GetHashString()
        {
            return "You Must Have " + maxPartCount + " Part Type " + partName + "On your vessel"; 
        }
        protected override string GetTitle()
        {
            return "Have part type " + partName;
        }

        protected override void OnUpdate()
        {
            CheckPartGoal(FlightGlobals.ActiveVessel);
        }
        protected override void OnRegister()
        {
            GameEvents.onVesselLoaded.Add(CheckPartGoal);
        }
        protected override void OnUnregister()
        {
            GameEvents.onVesselLoaded.Remove(CheckPartGoal);
        }

        protected override void OnLoad(ConfigNode node)
        {

            string partname = (node.GetValue("partname"));
            partName = partname;
            int maxcount = int.Parse(node.GetValue("maxcount"));
            maxPartCount = maxcount;
        }
        protected override void OnSave(ConfigNode node)
        {
            string partname = partName;
            node.AddValue("partname", partName);
            int maxcount = maxPartCount;
            node.AddValue("maxcount", maxPartCount);
        }

        public void CheckPartGoal(Vessel vessel)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {               
                if (vessel != null)
                {
                    foreach (Part p in vessel.Parts)
                    {
                        if (p.partInfo.name.Equals(partName))
                        {
                            ++partCount;
                        }
                    }
                }
                if (partCount > 0)
                {
                    if (partCount >= maxPartCount)
                    {
                        base.SetComplete();
                    }
                }
                                
                
            }
        }
    }
    #endregion
    #region DockingGoal
    public class DockingGoal : ContractParameter
    {
        
        public DockingGoal()
        {
        }
       
        protected override string GetHashString()
        {
            return "Dock with another Vessel";
        }
        protected override string GetTitle()
        {
            return "Dock with another Vessel";
        }
        
        protected override void OnRegister()
        {
            GameEvents.onPartCouple.Add(onPartCouple);
        }
        protected override void OnUnregister()
        {
            GameEvents.onPartCouple.Remove(onPartCouple);
        }

        protected override void OnLoad(ConfigNode node)
        {
          
        }
        protected override void OnSave(ConfigNode node)
        {
            
        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                Debug.LogError ("Docked FROM: " + action.from.vessel.vesselName);
                Debug.LogError ("Docked TO: " + action.to.vessel.vesselName);

                Debug.LogError("Docked TO Type Vessel: " + action.to.vessel.vesselType);

                Debug.LogError ("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError ("Docked TO ID: " + action.to.vessel.id.ToString());
                base.SetComplete();
            }
        }
    }
#endregion
    #region Target Docking Goal
    public class TargetDockingGoal : ContractParameter
    {
        public string targetDockingID;

        public TargetDockingGoal()
        {
        }

        public TargetDockingGoal(string targetID)
        {

        }

        protected override string GetHashString()
        {
            return "Dock with another Vessel";
        }
        protected override string GetTitle()
        {
            return "Dock with another Vessel";
        }

        protected override void OnRegister()
        {
            GameEvents.onPartCouple.Add(onPartCouple);
        }
        protected override void OnUnregister()
        {
            GameEvents.onPartCouple.Remove(onPartCouple);
        }

        protected override void OnLoad(ConfigNode node)
        {
            string tagID = (node.GetValue("tagID"));
            targetDockingID = tagID;
        }
        protected override void OnSave(ConfigNode node)
        {
            string tagID = targetDockingID;
            node.AddValue("tagID", targetDockingID);
        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {                                             
                Debug.LogError("Does: " + targetDockingID + " = " + action.from.vessel.id.ToString());
                Debug.LogError("Docked TO ID: " + action.to.vessel.id.ToString());
                if (targetDockingID == action.from.vessel.vesselName)
                {
                    ScreenMessages.PostScreenMessage("You have docked to the Target Vessel, Goal Complete");
                    base.SetComplete();
                }
                else
                    ScreenMessages.PostScreenMessage("Did not connect to the correct target ID vessel, Try Again");
            }
        }
    }
#endregion
}
