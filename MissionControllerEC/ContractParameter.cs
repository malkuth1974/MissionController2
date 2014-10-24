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
            return "Enter Orbit Around: " + targetBody.theName + "  MaxApA: " + maxApA + "  MinApA: " + minApA;
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
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

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            InOrbit(FlightGlobals.ActiveVessel);
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
            if (FlightGlobals.ActiveVessel)
            {
                base.SetComplete();
                ScreenMessages.PostScreenMessage("You Have achieved Orbit of Target Body: " + targetBody.theName);
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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                Orbits(FlightGlobals.ActiveVessel);

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
    #endregion
    #region inclinationGoal

    public class Inclination : ContractParameter
    {
        Settings settings = new Settings("Config.cfg");
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
            if (HighLogic.LoadedSceneIsFlight)
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
            if (FlightGlobals.ActiveVessel)
            {               
                if (vessel.orbit.inclination <= maxInclination && vessel.orbit.inclination >= minInclination)
                    base.SetComplete();
            }
        }
    }
        #endregion
    #region EccentricGoal

    public class EccentricGoal : ContractParameter
    {
        public double mineccn = 0.0;
        public double maxeccn = 0.0;

        public EccentricGoal()
        {
        }

        public EccentricGoal(double minEcc, double maxEcc)
        {
            this.mineccn = minEcc;
            this.maxeccn = maxEcc;
        }

        protected override string GetHashString()
        {
            return "Bring vessel into target orbital eccentricity";
        }
        protected override string GetTitle()
        {
            return "Bring vessel to Target Eccentricity between " + mineccn.ToString("F2") + " " + maxeccn.ToString("F2");
        }

        protected override void OnUpdate()
        {
            if (this.Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
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
            mineccn = double.Parse(node.GetValue("mineccn"));
            maxeccn = double.Parse(node.GetValue("maxeccn"));
            
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("mineccn", mineccn);
            node.AddValue("maxeccn", maxeccn);
        }

        public void CheckEccentricity(Vessel vessel)
        {           
            if (vessel.situation == Vessel.Situations.ORBITING && vessel.orbit.eccentricity <= maxeccn && vessel.orbit.eccentricity >= mineccn)
                    base.SetComplete();
        }
        public void ReCheckEccentricity(Vessel vessel)
        {          
            if (vessel.orbit.eccentricity > maxeccn && vessel.orbit.eccentricity < mineccn)
                base.SetIncomplete();
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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            CheckOrbitalPeriod(FlightGlobals.ActiveVessel);
            if (HighLogic.LoadedSceneIsFlight && SaveInfo.MessageHelpers == true)
            {
                Tools.ObitalPeriodHelper(FlightGlobals.ActiveVessel);
            }
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
            if (vessel.isActiveVessel)
            {               
                if (vessel.orbit.period <= maxOrbitalPeriod && vessel.orbit.period >= minOrbitalPeriod)
                    base.SetComplete();
            }
        }      
    }
    #endregion
    #region AltitudeGoal
    public class AltitudeGoal : ContractParameter
    {
        public CelestialBody targetBody;
        public double minAlt = 0.0;

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

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
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

        /// <summary>
        /// Used to get the Partname of this ContractParameter
        /// </summary>
        /// <param name="cp">Instance of this parameter</param>
        /// <returns>returns the actual name of part</returns>
        public static string iPartName(ContractParameter cp)
        {
            PartGoal instance = (PartGoal)cp;
            return instance.partName;
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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            CheckPartGoal(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            partName = (node.GetValue("partname"));
            maxPartCount = int.Parse(node.GetValue("maxcount"));
        }
        protected override void OnSave(ConfigNode node)
        {
            
            node.AddValue("partname", partName);            
            node.AddValue("maxcount", maxPartCount);
        }

        public void CheckPartGoal(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {               
                if (vessel != null)
                {
                    foreach (Part p in vessel.Parts)
                    {
                        if (p.partInfo.title.Equals(partName))
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
        public string targetDockingName;

        public TargetDockingGoal()
        {
        }

        /// <summary>
        /// used return the name of vessel for this docking goal
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string ItargetDockingName(ContractParameter cp)
        {
            TargetDockingGoal instance = (TargetDockingGoal)cp;
            return instance.targetDockingName;
        }
        /// <summary>
        /// used to return the Vessel ID of the To DOCK TO vessel
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string ItargetDockingID(ContractParameter cp)
        {
            TargetDockingGoal instance = (TargetDockingGoal)cp;
            return instance.targetDockingID;
        }

        public TargetDockingGoal(string targetID,string targetName)
        {
            this.targetDockingID = targetID;
            this.targetDockingName = targetName;
        }

        protected override string GetHashString()
        {
            return "Dock with Vessel:\n " + targetDockingName;
        }
        protected override string GetTitle()
        {
            return "Dock with Vessel: \n" + targetDockingName;
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
            targetDockingID = node.GetValue("VesselID");
            targetDockingName = node.GetValue("VesselName");

        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("VesselID", targetDockingID);
            node.AddValue("VesselName", targetDockingName);
        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {                
                          
                Debug.LogError("Does: " + targetDockingID + " = " + action.from.vessel.id.ToString());
                Debug.LogError("Docked FROM: " + action.from.vessel.vesselName);
                Debug.LogError("Docked TO: " + action.to.vessel.vesselName);

                Debug.LogError("Docked TO Type Vessel: " + action.to.vessel.vesselType);

                Debug.LogError("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError("Docked TO ID: " + action.to.vessel.id.ToString());
                if (targetDockingID == action.from.vessel.id.ToString() || targetDockingID == action.to.vessel.id.ToString())
                {
                    ScreenMessages.PostScreenMessage("You have docked to the Target Vessel, Goal Complete");
                    base.SetComplete();
                    action.from.vessel.vesselName = action.from.vessel.vesselName.Replace("(Repair)", "");
                    action.to.vessel.vesselName = action.to.vessel.vesselName.Replace("(Repair)", "");
                }
                else
                    ScreenMessages.PostScreenMessage("Did not connect to the correct target ID vessel, Try Again");
            }
        }
    }
    #endregion
    #region Orbital Research Part Check
    public class OrbialResearchPartCheck : ContractParameter
    {
        public CelestialBody targetBody;

        public double diff;
        public double savedTime;
        public double missionTime;

        public bool setTime = true;

        public OrbialResearchPartCheck()
        {
        }

        public OrbialResearchPartCheck(CelestialBody target, double Mtime)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
        }
       
        protected override string GetHashString()
        {
            return "Orbit " + targetBody.theName + " and conduct research.";
        }
        protected override string GetTitle()
        {
            return "Conduct Orbital Research. Time For Completion: " + Tools.formatTime(missionTime);
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                CheckIfOrbit(FlightGlobals.ActiveVessel);
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

            savedTime = double.Parse(node.GetValue("savedtime"));
            missionTime = double.Parse(node.GetValue("missiontime"));
            diff = double.Parse(node.GetValue("diff"));

            setTime = bool.Parse(node.GetValue("settime"));
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("savedtime", savedTime);
            node.AddValue("missiontime", missionTime);
            node.AddValue("diff", diff);

            node.AddValue("settime", setTime);
        }

        private void CheckIfOrbit(Vessel vessel)
        {

            if (vessel.isActiveVessel)
            {

                if (MCEOrbitalScanning.doOrbitResearch)
                {
                    if (HighLogic.LoadedSceneIsFlight && setTime)
                    {
                        contractSetTime();
                    }

                    if (!setTime)
                    {
                        diff = Planetarium.GetUniversalTime() - savedTime;

                        ScreenMessages.PostScreenMessage("Time Left To Complete: " + Tools.formatTime(missionTime - diff), .001f);

                        if (diff > missionTime)
                        {
                            base.SetComplete();
                            Debug.Log("Time Completed");
                        }
                    }

                }
            }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
            Debug.Log("Time countdown has been set");
        }

        
    }
#endregion
    #region Repair Panel Part Check
    public class RepairPanelPartCheck : ContractParameter
    {
        string TitleName;
        //bool must be static and global for check.

        public RepairPanelPartCheck()
        {
        }

        public RepairPanelPartCheck(string title)
        {
            this.TitleName = title;
        }

        protected override string GetHashString()
        {
            return TitleName;
        }
        protected override string GetTitle()
        {
            return TitleName;
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            CheckIfRepaired(FlightGlobals.ActiveVessel);
        }
        protected override void OnLoad(ConfigNode node)
        {
            TitleName = node.GetValue("titlename");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("titlename", TitleName);
        }

        private void CheckIfRepaired(Vessel name)
        {

            if (name.isActiveVessel && RepairPanel.repair)
            {
                base.SetComplete();
            }
        }
    }
    #endregion
    #region Get Crew Count
    public class GetCrewCount : ContractParameter
    {
        public int crewCount = 0;

        public GetCrewCount()
        {
        }

        public GetCrewCount(int crewnumber)
        {
            this.crewCount = crewnumber;
        }
        protected override string GetHashString()
        {
            if (crewCount > 0)
                return "Amount crew " + crewCount;
            else
                return "Vessel is automated: (nocrew)";
        }
        protected override string GetTitle()
        {
            if (crewCount > 0)
                return "Vessel Must Have This Amount Of crew " + crewCount;
            else
                return "Vessel is automated: (nocrew)";
        }

        protected override void OnUpdate()
        {
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
            CheckCrewValues(FlightGlobals.ActiveVessel); 
        }
     
        protected override void OnLoad(ConfigNode node)
        {            
            crewCount = int.Parse(node.GetValue("crewcount"));
        }
        protected override void OnSave(ConfigNode node)
        {            
            node.AddValue("crewcount", crewCount);
        }

        public void CheckCrewValues(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                int currentcrew = FlightGlobals.ActiveVessel.GetCrewCount();
                //Debug.LogError("Current crew is " + currentcrew + " crew can't be over " + crewCount);
                if (currentcrew <= crewCount)
                {
                    base.SetComplete();
                    //Debug.Log("Passed Crew Check");
                }
            }
        }
    }
#endregion
    #region Get Seat Count
    public class GetSeatCount : ContractParameter
    {
        public int seatCount = 0;
        public string title = "none";

        public GetSeatCount()
        {
        }

        public GetSeatCount(int crewnumber, string titledesc)
        {
            this.seatCount = crewnumber;
            this.title = titledesc;
        }
        protected override string GetHashString()
        {
            return "Amount crew " + seatCount;
        }
        protected override string GetTitle()
        {
            return title + " " + seatCount + " Plus Crew.";
        }

        protected override void OnUpdate()
        {
            
         if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
                 CheckCrewValues(FlightGlobals.ActiveVessel);
            
        }

        protected override void OnLoad(ConfigNode node)
        {
            seatCount = int.Parse(node.GetValue("crewcount"));
            title = node.GetValue("title");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", seatCount);
            node.AddValue("title", title);
        }

        public void CheckCrewValues(Vessel vessel)
        {
            int currentseats = FlightGlobals.ActiveVessel.GetCrewCapacity();
            int crewedseats = FlightGlobals.ActiveVessel.GetCrewCount();

            int seatavailable = currentseats - crewedseats;

            if (seatavailable >= seatCount)
            {
                base.SetComplete();
            }
        }
    }
    #endregion
    #region Lander Research Part Check
    public class LanderResearchPartCheck : ContractParameter
    {
        public CelestialBody targetBody;

        public double diff;
        public double savedTime;
        public double missionTime;

        public bool setTime = true;

        public LanderResearchPartCheck()
        {
        }

        public LanderResearchPartCheck(CelestialBody target, double Mtime)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
        }

        protected override string GetHashString()
        {
            return "Land Vessel And Conduct Research";
        }
        protected override string GetTitle()
        {
            return "Conduct Research. Time For Completion: " + Tools.formatTime(missionTime);
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                CheckIflanded(FlightGlobals.ActiveVessel);
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

            savedTime = double.Parse(node.GetValue("savedtime"));
            missionTime = double.Parse(node.GetValue("missiontime"));
            diff = double.Parse(node.GetValue("diff"));

            setTime = bool.Parse(node.GetValue("settime"));
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("savedtime", savedTime);
            node.AddValue("missiontime", missionTime);
            node.AddValue("diff", diff);

            node.AddValue("settime", setTime);
        }

        private void CheckIflanded(Vessel vessel)
        {

            if (vessel.isActiveVessel && (FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED))
            {
                if (MCELanderResearch.doLanderResearch)
                {
                    if (HighLogic.LoadedSceneIsFlight && setTime)
                    {
                        contractSetTime();
                    }
                    if (!setTime)
                    {
                        diff = Planetarium.GetUniversalTime() - savedTime;

                        ScreenMessages.PostScreenMessage("Time Left To Complete: " + Tools.formatTime(missionTime - diff), .001f);

                        if (diff > missionTime)
                        {
                            base.SetComplete();
                            Debug.Log("Time Completed");
                        }
                    }

                }
            }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
            Debug.Log("Time countdown has been set");
        }
    }
    #endregion    
    #region Agena In Orbit Goal
    public class AgenaInOrbit : ContractParameter
    {
        public CelestialBody targetBody;
        public string vesselID = "none";

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
            return "Build and Launch Agena Target Vehicle\n"+
                "Your current active vehicle at launch will be\n" +
                "Recorded as the Agena Vehicle";
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
    }
    #endregion
    #region Module Goal
    public class ModuleGoal : ContractParameter
    {
        public String moduleName;
        public String ModuleGoalname;
        //public int partCount = 0;
        //public int maxPartCount = 0;

        public ModuleGoal()
        {
        }

        public ModuleGoal(string Modulename, string TitleName)
        {
            this.moduleName = Modulename;
            this.ModuleGoalname = TitleName;

        }

        protected override string GetHashString()
        {
            return "Must Have  " + ModuleGoalname +  " On your vessel";
        }
        protected override string GetTitle()
        {
            return "Must Have  " + ModuleGoalname + " On your vessel";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                CheckPartGoal(FlightGlobals.ActiveVessel);
            }
        }

        protected override void OnLoad(ConfigNode node)
        {

            moduleName = (node.GetValue("partname"));
            ModuleGoalname = node.GetValue("modulegoalname");
        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("partname", moduleName);
            node.AddValue("modulegoalname", ModuleGoalname);
           
        }

        public void CheckPartGoal(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH)
            {                
                    foreach (Part p in vessel.parts)
                    {
                        foreach (PartModule pm in p.Modules)
                        {
                            if (pm.moduleName.Equals(moduleName))
                            {
                                base.SetComplete();
                            }
                        }
                    }
            }            

        }
    }
    
    #endregion
    #region Prelaunch Goal
    public class PreLaunch : ContractParameter
    {
        
        public PreLaunch()
        {
        }
       
        protected override string GetHashString()
        {
            return "Launch Vessel";
        }
        protected override string GetTitle()
        {
            return "Ready To Launch Your Vessel";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH)
            {
                base.SetComplete();
            }
        }        
    }
#endregion
    #region TotalMassGoal
    public class TotalMasGoal : ContractParameter
    {
        public CelestialBody targetBody;
        public float maxweight = 0.0f;

        public TotalMasGoal()
        {
        }

        public TotalMasGoal(CelestialBody target, float maxWeight)
        {
            this.targetBody = target;
            this.maxweight = maxWeight;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Satellite Mass Must Not Exceed: " + maxweight.ToString("F2") + " Tons. (InOrbit)";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            MassCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            maxweight = float.Parse(node.GetValue("maxtons"));
            

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("maxtons", maxweight);
            
        }

        public void MassCheck(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                if (vessel.GetTotalMass() <= maxweight)
                {
                    base.SetComplete();
                }               
            }
        }       
    }
    #endregion
    #region Resource Goal Check
    public class ResourceGoal : ContractParameter
    {
        public string targetName;
        public double maxAmountt = 0.0f;
        public double minAmount = 0.0f;

        /// <summary>
        /// Returns the name of the Resource goal for this parameter
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string iTargetName(ContractParameter cp)
        {
            ResourceGoal instance = (ResourceGoal)cp;
            return instance.targetName;
        }

        public ResourceGoal()
        {
        }

        public ResourceGoal(string target, double maxAmt, double minAmt)
        {
            this.targetName = target;
            this.maxAmountt = maxAmt;
            this.minAmount = minAmt;
        }
        protected override string GetHashString()
        {
            return targetName;
        }
        protected override string GetTitle()
        {
            return "Must Have " + targetName + " Between "+ minAmount + " and " + maxAmountt + " (InOrbit)";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                ResourceCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            targetName = node.GetValue("targetname");
            maxAmountt = float.Parse(node.GetValue("maxtons"));
            minAmount = float.Parse(node.GetValue("mintons"));


        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("targetname", targetName);
            node.AddValue("maxtons", maxAmountt);
            node.AddValue("mintons", minAmount);

        }

        public void ResourceCheck(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                double resources = 0;

                if (vessel != null)
                {
                    foreach (Part p in vessel.parts)
                    {
                        if (p.Resources[targetName] != null)
                        {
                            resources += p.Resources[targetName].amount;
                        }
                    }
                    if (resources > 0)
                    {
                        if (resources >= minAmount && resources <= maxAmountt)
                        {
                            base.SetComplete();
                        }
                    }
                }

                
            }
        }
    }
    #endregion
    #region Resource Goal Cap Check
    public class ResourceGoalCap : ContractParameter
    {
        public string targetName;        
        public double RsAmount = 0.0f;

        /// <summary>
        /// Returns the name of the Resource goal for this parameter
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string iTargetName(ContractParameter cp)
        {
            ResourceGoal instance = (ResourceGoal)cp;
            return instance.targetName;
        }

        public ResourceGoalCap()
        {
        }

        public ResourceGoalCap(string target, double rsAmount)
        {
            this.targetName = target;
            this.RsAmount = rsAmount;
        }
        protected override string GetHashString()
        {
            return targetName;
        }
        protected override string GetTitle()
        {
            return "Must Have " + targetName + " Greater Than " + RsAmount + " (InOrbit)";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                ResourceCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            targetName = node.GetValue("targetname");
            RsAmount = float.Parse(node.GetValue("mintons"));


        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("targetname", targetName);
            node.AddValue("mintons", RsAmount);

        }

        public void ResourceCheck(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                double resources = 0;

                if (vessel != null)
                {
                    foreach (Part p in vessel.parts)
                    {
                        if (p.Resources[targetName] != null)
                        {
                            resources += p.Resources[targetName].amount;
                        }
                    }
                    if (resources > 0)
                    {
                        if (resources >= RsAmount)
                        {
                            base.SetComplete();
                        }
                    }
                }


            }
        }
    }
    #endregion
    #region Resource Supply Goal Check
    public class ResourceSupplyGoal : ContractParameter
    {
        public string targetName;
        public double ResourceAmount = 0.0f;
        public string contractTitle;

        public ResourceSupplyGoal()
        {
        }

        public ResourceSupplyGoal(string target, double RsAmount,string Ctitle)
        {
            this.targetName = target;
            this.ResourceAmount = RsAmount;
            this.contractTitle = Ctitle;
        }
        protected override string GetHashString()
        {
            return targetName;
        }
        protected override string GetTitle()
        {
            return contractTitle + " "  + ResourceAmount + " " + targetName;
        }

        protected override void OnRegister()
        {
            GameEvents.onPartCouple.Add(OnResourceCheck);
        }
        protected override void OnUnregister()
        {
            GameEvents.onPartCouple.Remove(OnResourceCheck);
        }

        protected override void OnLoad(ConfigNode node)
        {

            targetName = node.GetValue("targetname");
            ResourceAmount = float.Parse(node.GetValue("resourceamount"));            
        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("targetname", targetName);
            node.AddValue("resourceamount", ResourceAmount);            
        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                Debug.LogError("Docked FROM: " + action.from.vessel.vesselName);
                Debug.LogError("Docked TO: " + action.to.vessel.vesselName);

                Debug.LogError("Docked TO Type Vessel: " + action.to.vessel.vesselType);

                Debug.LogError("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError("Docked TO ID: " + action.to.vessel.id.ToString());
                base.SetComplete();
            }
        }

        private void OnResourceCheck(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                double resources = 0;

                if (FlightGlobals.ActiveVessel != null)
                {
                    foreach (Part p in FlightGlobals.ActiveVessel.parts)
                    {
                        if (p.Resources[targetName] != null)
                        {
                            resources += p.Resources[targetName].amount;
                        }
                    }
                    if (resources > 0)
                    {
                        if (resources >= ResourceAmount)
                        {
                            base.SetComplete();
                        }
                    }
                }


            }
        }
    }
    #endregion
    #region Time Countdown 
    public class TimeCountdownOrbits : ContractParameter
    {
        public CelestialBody targetBody;

        public double diff;
        public double savedTime;
        public double missionTime;
        public string contractTimeTitle = "Reach Orbit and stay for amount of Time Specified: ";
        public string vesselID = "none";

        public bool setTime = true;
        public bool timebool = false;

        public TimeCountdownOrbits()
        {
        }

        public TimeCountdownOrbits(CelestialBody target, double Mtime)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
        }

        public TimeCountdownOrbits(CelestialBody target, double Mtime, string title)
        {
            this.targetBody = target;
            this.missionTime = Mtime;
            this.contractTimeTitle = title;
        }

        protected override string GetHashString()
        {
            return "Orbit " + targetBody.theName + " and conduct research.";
        }
        protected override string GetTitle()
        {
            return contractTimeTitle + Tools.formatTime(missionTime);
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                timebool = true;
            }
            if (timebool)
            {
                CheckIfOrbit(FlightGlobals.ActiveVessel);
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

            savedTime = double.Parse(node.GetValue("savedtime"));
            missionTime = double.Parse(node.GetValue("missiontime"));
            diff = double.Parse(node.GetValue("diff"));

            setTime = bool.Parse(node.GetValue("settime"));
            timebool = bool.Parse(node.GetValue("timebool"));
            vesselID = node.GetValue("vesid");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("savedtime", savedTime);
            node.AddValue("missiontime", missionTime);
            node.AddValue("diff", diff);

            node.AddValue("settime", setTime);
            node.AddValue("timebool", timebool);
            node.AddValue("vesid", vesselID);
        }

        private void CheckIfOrbit(Vessel vessel)
        {
            if (HighLogic.LoadedSceneIsFlight && setTime)
            {
                contractSetTime();
                vesselID = vessel.id.ToString();
            }

            if (!setTime)
            {
                diff = Planetarium.GetUniversalTime() - savedTime;
                if (HighLogic.LoadedSceneIsFlight && vessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("Time Left To Complete: " + Tools.formatTime(missionTime - diff), .001f);
                }

                if (diff > missionTime)
                {
                    base.SetComplete();
                }
            }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
        }
    } 
     #endregion             
    #region EVA Goal
    public class EvaGoal : ContractParameter
    {
        public CelestialBody targetBody;

        public EvaGoal()
        {
        }

        public EvaGoal(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Exit your vessel and conduct an EVA";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                isEVA(FlightGlobals.ActiveVessel);
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

        public void isEVA(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.isEVA)
                base.SetComplete();
        }


    }
    #endregion
    #region Crash Goal
    public class CrashGoal : ContractParameter
    {
        public CelestialBody targetBody;

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

        protected override void OnRegister()
        {
            if (HighLogic.LoadedSceneIsFlight && (FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody) || FlightGlobals.ship_orbit.referenceBody.Equals(targetBody)))
            GameEvents.onCrash.Add(crashGoal);
        }
        protected override void OnUnregister()
        {
            GameEvents.onCrash.Remove(crashGoal);
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

        public void crashGoal(EventReport ev)
        {           
                base.SetComplete();
        }


    }
    #endregion
    #region Civilian Module
    public class CivilianModule : ContractParameter
    {
        public int crewSpace = 0;
        public int civilianSpace = 0;
        public int FreeSpace = 0;
        public int UsedSpace = 0;

        public string name1 = "none";
        public string name2 = "none";
        public string name3 = "none";
        public string name4 = "none";

        public string vesselID;

        public string destination = "none";       

        public CelestialBody targetBody;

        public CivilianModule()
        {
        }

        public CivilianModule(CelestialBody body ,int civspace, string name1, string name2, string name3, string name4, string destination)
        {
            this.targetBody = body;
            this.civilianSpace = civspace;
            this.name1 = name1;
            this.name2 = name2;
            this.name3 = name3;
            this.name4 = name4;
            this.destination = destination;
        }
        public CivilianModule(CelestialBody body, int civspace, string name1, string name2, string name3, string destination)
        {
            this.targetBody = body;
            this.civilianSpace = civspace;
            this.name1 = name1;
            this.name2 = name2;
            this.name3 = name3;
            this.destination = destination;
        }
        public CivilianModule(CelestialBody body, int civspace, string name1, string name2, string destination)
        {
            this.targetBody = body;
            this.civilianSpace = civspace;
            this.name1 = name1;
            this.name2 = name2;
            this.destination = destination;
        }
        protected override string GetHashString()
        {

            return "Bring civilians on space tour";
           
        }
        protected override string GetTitle()
        {
            if (civilianSpace == 2 || civilianSpace == 1)
            {
                return "Have Room in vessel for following 2 Civilians for the " + destination + "\n\n" + "-" + name1 + "\n" + "-" + name2 + "\n";
            }
            if (civilianSpace == 3)
            {
                return "Have Room in vessel for following 3 Civilians for the " + destination + "\n\n" + "-" + name1 + "\n" + "-" + name2 + "\n" + "-" + name3 + "\n";
            }
            else 
            {
                return "Have Room in vessel for following 4 Civilians for the " + destination + "\n\n" + "-" + name1 + "\n" + "-" + name2 + "\n" + "-" + name3 + "\n" + "-" + name4 + "\n";
            }
        }
        
        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        CivilianChecks(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete)
                    {
                        civilianReCheck(FlightGlobals.ActiveVessel);
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
   
            crewSpace = int.Parse(node.GetValue("crewspace"));
            civilianSpace = int.Parse(node.GetValue("civspace"));
            FreeSpace = int.Parse(node.GetValue("freespace"));
            UsedSpace = int.Parse(node.GetValue("usedspace"));
            destination = node.GetValue("destination");
            name1 = node.GetValue("name1");
            name2 = node.GetValue("name2");
            name3 = node.GetValue("name3");
            name4 = node.GetValue("name4");
            vesselID = node.GetValue("vesselid");
          
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewspace", crewSpace);
            node.AddValue("civspace", civilianSpace);
            node.AddValue("freespace", FreeSpace);
            node.AddValue("usedspace", UsedSpace);
            node.AddValue("destination", destination);
            node.AddValue("name1", name1);
            node.AddValue("name2", name2);
            node.AddValue("name3", name3);
            node.AddValue("name4", name4);
            node.AddValue("vesselid", vesselID);
        }

        public void CivilianChecks(Vessel vessel)
        {
            crewSpace = FlightGlobals.ActiveVessel.GetCrewCount();
            FreeSpace = FlightGlobals.ActiveVessel.GetCrewCapacity();

            UsedSpace = FreeSpace - crewSpace;

            if (UsedSpace >= civilianSpace)
            {
                base.SetComplete();
                vesselID = FlightGlobals.ActiveVessel.id.ToString();
            }
            else
            {
                if (FlightGlobals.ActiveVessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("No space for Civilians on this vessel, Make some room!");
                }
            }            
        }
        public void civilianReCheck(Vessel vessel)
        {
            crewSpace = FlightGlobals.ActiveVessel.GetCrewCount();
            FreeSpace = FlightGlobals.ActiveVessel.GetCrewCapacity();

            UsedSpace = FreeSpace - crewSpace;

            if (UsedSpace < civilianSpace)
            {
                base.SetIncomplete();
                if (FlightGlobals.ActiveVessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("You have taken up the space that is required for your Civilian Passengers.  Please make room for them again!");
                }
            }             
        }       
    }
    #endregion
}
   