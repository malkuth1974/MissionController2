using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;
using MissionControllerEC.PartModules;

namespace MissionControllerEC.MCEParameters
{
    #region PartGoal
    public class PartGoal : ContractParameter
    {
        private String partName = "";
        private String partName2 = "";
        private int partCount = 0;
        private int maxPartCount = 0;
        private bool twoPartsTrue = false;
        private bool updated = false;
        private bool disableRecheck = false;

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

        public PartGoal(string name, string name2, int maxCount, bool partTrue)
        {
            this.partName = name;
            this.partName2 = name2;
            this.maxPartCount = maxCount;
            this.twoPartsTrue = partTrue;

        }

        public PartGoal(string name, int maxCount, bool disableRecheck)
        {
            this.partName = name;
            this.maxPartCount = maxCount;
            this.disableRecheck = disableRecheck;

        }

        protected override string GetHashString()
        {
            if (twoPartsTrue)
                return "You Must Have " + maxPartCount + " Part Type " + partName + " On your vessel"+ this.Root.MissionSeed.ToString();
            else
                return "You Must Have " + maxPartCount + " Part Type " + partName + " or " + partName2 + " On your vessel" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Have part type " + partName;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active && !disableRecheck)
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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
                CheckPartGoal(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {          
            Tools.ContractLoadCheck(node, ref partName, "Error Defaults Loaded", partName, "partname");
            Tools.ContractLoadCheck(node, ref partName2, "Error Defaults Loaded", partName2, "partname2");
            Tools.ContractLoadCheck(node, ref maxPartCount, 1, maxPartCount, "maxcount");
        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("partname", partName);
            node.AddValue("partname2", partName2);
            node.AddValue("maxcount", maxPartCount);
        }

        public void CheckPartGoal(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    if (vessel != null)
                    {
                        foreach (Part p in vessel.Parts)
                        {
                            if (p.partInfo.title.Equals(partName) || p.partInfo.title.Equals(partName2))
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
    #region Repair Panel Part Check
    public class RepairPanelPartCheck : ContractParameter
    {
        private string TitleName;
        private string ShipVesselID;
        private string ShipName;
        private string SavedOriginalID = "NONE";
        private bool updated = false;
        //bool must be static and global for check.

        public RepairPanelPartCheck()
        {
        }

        public RepairPanelPartCheck(string title, string vesselId, string vesselName)
        {
            this.TitleName = title;
            this.ShipVesselID = vesselId;
            this.ShipName = vesselName;
        }

        protected override string GetHashString()
        {
            return TitleName + " " + ShipName + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return TitleName + " " + ShipName;
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
                CheckIfRepaired(FlightGlobals.ActiveVessel);
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                GameEvents.onPartCouple.Add(onPartCouple);
                updated = true;
            }

        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onPartCouple.Remove(onPartCouple);
            }

        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref TitleName, "Error Defaults Loaded",TitleName, "titlename");
            Tools.ContractLoadCheck(node, ref ShipVesselID, "Defaults Loaded", ShipVesselID, "vesselid");
            Tools.ContractLoadCheck(node, ref ShipName, "Error Defaults Loaded", ShipName, "shipname");
            Tools.ContractLoadCheck(node, ref SavedOriginalID, "Error Defaults Loaded", SavedOriginalID, "savedid");          
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("titlename", TitleName);
            node.AddValue("vesselid", ShipVesselID);
            node.AddValue("shipname", ShipName);
            node.AddValue("savedid", SavedOriginalID);
        }

        private void CheckIfRepaired(Vessel name)
        {
            if (SavedOriginalID == "NONE")
            {
                SavedOriginalID = ShipVesselID;
            }
            //Debug.LogError("Does ID's Match? " + ShipVesselID+ " and " + RepairPanel.vesselId + " Backup " + SavedOriginalID);
            if (ShipVesselID == RepairPanel.vesselId || SavedOriginalID == RepairPanel.vesselId)
            {
                if (RepairPanel.repair)
                {
                    base.SetComplete();
                }
            }
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

                if (action.from.vessel.id.ToString() == ShipVesselID || action.to.vessel.id.ToString() == ShipVesselID)
                {
                    Debug.Log("saved vessel Id Changed to new docking ID for vessel " + ShipName);
                    ShipVesselID = action.from.vessel.id.ToString();
                    SavedOriginalID = action.to.vessel.id.ToString();
                }
                else Debug.Log("Docked to vessel was not repair vessel, no ID change needed");
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

    #region Satellite Core Check
    public class satelliteCoreCheck : ContractParameter
    {
     
        private string satType = "none";
        private float frequency = 1;
        private float Moduletype = 1;
        private CelestialBody targetBody;
        public static bool SatelliteCalled = false;
        public static bool APReady = false;
        public static bool PEReady = false;

        //Should not have to Save Or Load these Below Values, since they are changed on spot by MCECommunicationsCore rather not use static but for now they work.
        public static string testSatType = "test";
        public static float testModuleType = 0;
        public static float testFreq = 0;
        //Below Values set up Delegte and the two events that communicate between Parameter and PartModules.
        public delegate void OnSatelliteCall();
        public event OnSatelliteCall ChangeBool;
        public event OnSatelliteCall SatelliteTest;

        public satelliteCoreCheck()
        {
        }

        public satelliteCoreCheck(string typeSatellite,float satFrequency,float SatModule,CelestialBody body)
        {
            this.satType = typeSatellite;
            this.frequency = satFrequency;
            this.Moduletype = SatModule;
            this.targetBody = body;
        }
        
        protected override string GetHashString()
        {
            return "Set module frequency and Module types and complete contract" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Send Data Stream and Connect Satellite to Customers";
        }
        protected override string GetNotes()
        {
            return "Set Module type In editor to: " + Moduletype + "\nFrequency to: " + frequency + ".\nUse Core Type: " + satType + ".\n\n";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                if (APReady && PEReady)
                {
                    checkPartDataPacket();
                }
            }  
        }
       

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref satType, "Communication", satType, "typeofsat");
            Tools.ContractLoadCheck(node, ref frequency, 0, frequency, "frequency");
            Tools.ContractLoadCheck(node, ref Moduletype, 0, Moduletype, "moduletype");
            Tools.ContractLoadCheck(node, ref SatelliteCalled, false, SatelliteCalled, "satcalled");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("typeofsat", satType);
            node.AddValue("frequency", frequency);
            node.AddValue("moduletype", Moduletype);
            node.AddValue("satcalled", SatelliteCalled);
        }

        public void checkPartDataPacket()
        {
            if (SatelliteCalled == true)
            {
                Debug.Log("Satellite Called Checking If satellite has modules");
                if (testSatType == satType)
                {
                    Debug.Log("Satellite Type matches " + satType);
                    if (testModuleType == Moduletype)
                    {
                        Debug.Log("Satellite Module Matches " + Moduletype);
                        if (testFreq == frequency)
                        {
                            Debug.Log("Satellite Frequency matches " + frequency);
                            base.SetComplete();
                            SatelliteCalled = false;
                            APReady = false;
                            PEReady = false;
                        }
                        else 
                        {
                            Debug.Log("Satellite Frequency Failed " + frequency); 
                        }
                    }
                    else 
                    {
                        Debug.Log("Satellite Module Failed " + Moduletype); 
                    }
                }
                else 
                {
                    Debug.Log("Satellite Type Failed " + satType);
                }
            }
            else
            {              
            }
        }

        protected virtual void OnBoolChanged()
        {
            if (ChangeBool != null)
            {
                ChangeBool();
            }
            else
            {
                //Debug.Log("Event fired for OnBoolCharged! In SatelliteCoreCheck Parameter");
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
                //Debug.Log("Event fired for OnSatelliteChanged! In SatelliteCoreCheck Parameter");
            }
        }

        public satelliteCoreCheck(bool n) 
        {
            //Debug.Log("EventTest Fired with Bool Value " + n + " In SatelliteCoreCheck Parameter");
            SetBoolSatelliteCoreValue(n);
        }        
        public satelliteCoreCheck(string a, float b, float c)
        {
            //Debug.Log("EventTest Fired with string " + a + "Float " + b + "float " + c + "In SatelliteCoreCheck Parameter");
            SetSatelliteCoreCheck(a, b, c);
        }
        // SetBoolSatelliteCoreValue Communicates Between Parameter and Part Module.
        public void SetBoolSatelliteCoreValue(bool n)
        {
            SatelliteCalled = n;
            OnBoolChanged();
            //Debug.Log("Called Bool is now " + n + " In SatelliteCoreCheck Parameter");
        }
        //SetSatelliteCoreCheck Communicates Between Parameter and Part Module.
        public void SetSatelliteCoreCheck(string satType, float ModuleType, float Freq)
        {
            testSatType = satType;
            testModuleType = ModuleType;
            testFreq = Freq;
            OnSatelliteChanged();
            //Debug.Log("Satellite Values In Parameters Now Set to string " + satType + "Float " + ModuleType + "float " + Freq + "In SatelliteCoreCheck Parameter");
        }
        public void SetApaBoolSatelliteCoreValue(bool n)
        {
            APReady = n;
            OnBoolChanged();
            //Debug.Log("ApA Bool is now " + n + " In SatelliteCoreCheck Parameter");
        }
        public void SetPeABoolSatelliteCoreValue(bool n)
        {
            PEReady = n;
            OnBoolChanged();
            //Debug.Log("PeA Bool is now " + n + " In SatelliteCoreCheck Parameter");
        }
          
    }
    #endregion

    #region Orbital Research Part Check
    public class OrbialResearchPartCheck : ContractParameter
    {
        private CelestialBody targetBody;

        private double diff;
        private double savedTime;
        private double missionTime;

        private bool setTime = true;
        private bool updated = false;

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
            return "Orbit " + targetBody.theName + " and conduct Ionization Scan." + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Conduct Ionization Scan. Time For Completion: " + Tools.formatTime(missionTime);
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
            if (HighLogic.LoadedSceneIsFlight)
            {
                CheckIfOrbit(FlightGlobals.ActiveVessel);
            }
        }
        protected override void OnLoad(ConfigNode node)
        {         
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref savedTime, 0, savedTime, "savedtime");
            Tools.ContractLoadCheck(node, ref missionTime, 1000, missionTime, "missiontime");
            Tools.ContractLoadCheck(node, ref diff, 0, diff, "diff");
            Tools.ContractLoadCheck(node, ref setTime, false, setTime, "settime");
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
            if (vessel.launchTime > this.Root.DateAccepted)
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
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
            Debug.Log("Time countdown has been set");
        }


    }
    #endregion
    #region Lander Research Part Check
    public class LanderResearchPartCheck : ContractParameter
    {
        private CelestialBody targetBody;

        private double diff;
        private double savedTime;
        private double missionTime;

        private bool setTime = true;
        private bool updated = false;

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
            return "Land Vessel And Conduct Research" + this.Root.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return "Conduct Research. Time For Completion: " + Tools.formatTime(missionTime);
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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                CheckIflanded(FlightGlobals.ActiveVessel);
            }
        }
        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref savedTime, 0, savedTime, "savedtime");
            Tools.ContractLoadCheck(node, ref missionTime, 1000, missionTime, "missiontime");
            Tools.ContractLoadCheck(node, ref diff, 0, diff, "diff");
            Tools.ContractLoadCheck(node, ref setTime, false, setTime, "settime");
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
            if (vessel.launchTime > this.Root.DateAccepted)
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
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
            Debug.Log("Time countdown has been set");
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
    #region Module Goal
    public class ModuleGoal : ContractParameter
    {
        private String moduleName;
        private String ModuleGoalname;
       
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
            return "Must Have  " + ModuleGoalname + " On your vessel" + this.Root.MissionSeed.ToString();
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
            Tools.ContractLoadCheck(node, ref moduleName, "Error Defaults Loaded", moduleName, "partname");
            Tools.ContractLoadCheck(node, ref ModuleGoalname, "Error Defaults Loaded", ModuleGoalname, "modulegoalname");
        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("partname", moduleName);
            node.AddValue("modulegoalname", ModuleGoalname);

        }

        public void CheckPartGoal(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
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
