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

        //protected override void OnRegister()
        //{
        //    GameEvents.onVesselWasModified.Add(Orbits);
        //}
        //protected override void OnUnregister()
        //{
        //    GameEvents.onVesselWasModified.Remove(Orbits);
        //}

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

    public class StageEvent : ContractParameter
    {
        public string StEvent = "";
        public string sTitle = "";

        public StageEvent()
        {
        }

        public StageEvent(string sevent, string title)
        {
            this.StEvent = sevent;
            this.sTitle = title;
        }

        protected override string GetHashString()
        {
            return StEvent;
        }
        protected override string GetTitle()
        {
            return sTitle;
        }

        protected override void OnRegister()
        {
            GameEvents.onVesselWasModified.Add(stageEvent);
        }
        protected override void OnUnregister()
        {
            GameEvents.onVesselWasModified.Remove(stageEvent);
        }

        protected override void OnLoad(ConfigNode node)
        {
            
            string stageID = (node.GetValue("stageID"));
            StEvent = stageID;
            string titleID = (node.GetValue("titleID"));
            sTitle = titleID;

        }
        protected override void OnSave(ConfigNode node)
        {
            string stageID = StEvent;
            node.AddValue("stageID", stageID);
            string titleID = sTitle;
            node.AddValue("titleID", titleID);
        }

        public void stageEvent(Vessel vessel)
        {     
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                base.SetComplete();
            }
        }

        
    }
}
