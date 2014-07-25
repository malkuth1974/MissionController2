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
    
    public class OrbitGoal : ContractParameter
    {
        public CelestialBody targetBody;       
        public double maxApA = 0.0;
        public double minPeA = 0.0;

        public OrbitGoal()
        {
        }
        
        public OrbitGoal(CelestialBody target, double maxApA, double minPeA)
        {
            this.targetBody = target;           
            this.maxApA = maxApA;
            this.minPeA = minPeA;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Enter Orbit Around: " + targetBody.theName + "  MaxApA: " + maxApA + "  MinPeA: " + minPeA;
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
            minPeA = PeAID;

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = maxApA;
            node.AddValue("aPa", ApAID);
            double PeAID = minPeA;
            node.AddValue("pEa", PeAID);
        }
        
        public void Orbits(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
            {
                if (vessel.orbit.PeA >= minPeA && vessel.orbit.ApA <= maxApA)
                {
                    base.SetComplete();
                    Debug.Log("Contract Orbital has been completed");
                }
            }
        }


    }
    
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
    public class PartGoal : ContractParameter
    {
        public String partName = "";
        public int partCount = 0;
        public int maxPartCount = 0;

        public PartGoal()
        {
        }

        public PartGoal(string name, int Pcount, int maxCount)
        {
            this.partName = name;
            this.partCount = Pcount;
            this.maxPartCount = maxCount;
                
        }

        protected override string GetHashString()
        {
            return "You Must Have " + maxPartCount + " Part Type " + partName + "On your vessel"; 
        }
        protected override string GetTitle()
        {
            return "Have partType " + partName;
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
}
