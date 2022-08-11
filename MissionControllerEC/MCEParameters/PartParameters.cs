using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;
using MissionControllerEC.PartModules;
using KSP.Localization;
using static MissionControllerEC.RegisterToolbar;

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
                return Localizer.Format("#autoLOC_MissionController2_1000227") + " " + maxPartCount + " " + Localizer.Format("#autoLOC_MissionController2_1000228") + " " + partName + " " + Localizer.Format("#autoLOC_MissionController2_1000229") + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000227 = You Must Have 		// #autoLOC_MissionController2_1000228 =  Part Type 		// #autoLOC_MissionController2_1000229 =  On your vessel
            else
                return Localizer.Format("#autoLOC_MissionController2_1000230") + " " + maxPartCount + " " + Localizer.Format("#autoLOC_MissionController2_1000231") + " " + partName + " " + Localizer.Format("#autoLOC_MissionController2_1000232") + " " + partName2 + " " + Localizer.Format("#autoLOC_MissionController2_1000233") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000230 = You Must Have 		// #autoLOC_MissionController2_1000231 =  Part Type 		// #autoLOC_MissionController2_1000232 =  or 		// #autoLOC_MissionController2_1000233 =  On your vessel
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000234") + " " + partName;		// #autoLOC_MissionController2_1000234 = Have part type 
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
            else { }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
                CheckPartGoal(FlightGlobals.ActiveVessel);
            else { }
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
                                partCount = 1;
                            }
                            else { }
                        }
                    }
                    if (partCount > 0)
                    {
                        if (partCount >= maxPartCount)
                        {
                            base.SetComplete();
                        }
                        else { }
                    }
                    else { }
                }
                else { }
            }
            else { }
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
            else { }
        }
        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
                GameEvents.onPartCouple.Remove(onPartCouple);
            }
            else { }
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
            //Log.Error("Does ID's Match? " + ShipVesselID+ " and " + RepairPanel.vesselId + " Backup " + SavedOriginalID);
            if (ShipVesselID == RepairPanel.vesselId || SavedOriginalID == RepairPanel.vesselId)
            {
                if (RepairPanel.repair)
                {
                    base.SetComplete();
                }
                else { }
            }
            else { }
        }

        private void onPartCouple(GameEvents.FromToAction<Part, Part> action)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                Log.Info("Docked FROM: " + action.from.vessel.vesselName);
                Log.Info("Docked TO: " + action.to.vessel.vesselName);

                Log.Info("Docked TO Type Vessel: " + action.to.vessel.vesselType);

                Log.Info("Docked FROM ID: " + action.from.vessel.id.ToString());
                Log.Info("Docked TO ID: " + action.to.vessel.id.ToString());

                if (action.from.vessel.id.ToString() == ShipVesselID || action.to.vessel.id.ToString() == ShipVesselID)
                {
                    Log.Info("saved vessel Id Changed to new docking ID for vessel " + ShipName);
                    ShipVesselID = action.from.vessel.id.ToString();
                    SavedOriginalID = action.to.vessel.id.ToString();
                }
                else Log.Info("Docked to vessel was not repair vessel, no ID change needed");
            }
            else { }
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
        public static bool APReady = true;
        public static bool PEReady = true;

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
            return Localizer.Format("#autoLOC_MissionController2_1000222") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000222 = Set module frequency and Module types and complete contract
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000223");		// #autoLOC_MissionController2_1000223 = Send Data Stream and Connect Satellite to Customers
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000224") + " " + Moduletype + " " + Localizer.Format("#autoLOC_MissionController2_1000225") + " " + frequency + " " + Localizer.Format("#autoLOC_MissionController2_1000226") + " " + satType + ".\n\n";		// #autoLOC_MissionController2_1000224 = Set Module type In editor to: 		// #autoLOC_MissionController2_1000225 = \n Frequency to: 		// #autoLOC_MissionController2_1000226 = .\n Use Core Type: 
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.orbit.referenceBody.Equals(targetBody))
            {
                if (APReady && PEReady)
                {
                    checkPartDataPacket();
                }
                else { }
            }
            else { }
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
                Log.Info("Satellite Called Checking If satellite has modules");
                if (testSatType == satType)
                {
                    Log.Info("Satellite Type matches " + satType);
                    if (testModuleType == Moduletype)
                    {
                        Log.Info("Satellite Module Matches " + Moduletype);
                        if (testFreq == frequency)
                        {
                            Log.Info("Satellite Frequency matches " + frequency);
                            base.SetComplete();
                            SatelliteCalled = false;
                            APReady = true;
                            PEReady = true;
                        }
                        else 
                        {
                            Log.Info("Satellite Frequency Failed " + frequency); 
                        }
                    }
                    else 
                    {
                        Log.Info("Satellite Module Failed " + Moduletype); 
                    }
                }
                else 
                {
                    Log.Info("Satellite Type Failed " + satType);
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
                //Log.Info("Event fired for OnBoolCharged! In SatelliteCoreCheck Parameter");
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
                //Log.Info("Event fired for OnSatelliteChanged! In SatelliteCoreCheck Parameter");
            }
        }

        public satelliteCoreCheck(bool n) 
        {
            //Log.Info("EventTest Fired with Bool Value " + n + " In SatelliteCoreCheck Parameter");
            SetBoolSatelliteCoreValue(n);
        }        
        public satelliteCoreCheck(string a, float b, float c)
        {
            //Log.Info("EventTest Fired with string " + a + "Float " + b + "float " + c + "In SatelliteCoreCheck Parameter");
            SetSatelliteCoreCheck(a, b, c);
        }
        // SetBoolSatelliteCoreValue Communicates Between Parameter and Part Module.
        public void SetBoolSatelliteCoreValue(bool n)
        {
            SatelliteCalled = n;
            OnBoolChanged();
            //Log.Info("Called Bool is now " + n + " In SatelliteCoreCheck Parameter");
        }
        //SetSatelliteCoreCheck Communicates Between Parameter and Part Module.
        public void SetSatelliteCoreCheck(string satType, float ModuleType, float Freq)
        {
            testSatType = satType;
            testModuleType = ModuleType;
            testFreq = Freq;
            OnSatelliteChanged();
            //Log.Info("Satellite Values In Parameters Now Set to string " + satType + "Float " + ModuleType + "float " + Freq + "In SatelliteCoreCheck Parameter");
        }
        public void SetApaBoolSatelliteCoreValue(bool n)
        {
            APReady = n;
            OnBoolChanged();
            //Log.Info("ApA Bool is now " + n + " In SatelliteCoreCheck Parameter");
        }
        public void SetPeABoolSatelliteCoreValue(bool n)
        {
            PEReady = n;
            OnBoolChanged();
            //Log.Info("PeA Bool is now " + n + " In SatelliteCoreCheck Parameter");
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
            return Localizer.Format("#autoLOC_MissionController2_1000219") + " " + targetBody.bodyName + Localizer.Format("#autoLOC_MissionController2_1000220") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000219 = Orbit 		// #autoLOC_MissionController2_1000220 =  and conduct Ionization Scan.
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000221") + " " + Tools.formatTime(missionTime);		// #autoLOC_MissionController2_1000221 = Conduct Ionization Scan. Time For Completion: 
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
            else { }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
            else { }
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                CheckIfOrbit(FlightGlobals.ActiveVessel);
            }
            else { }
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
                                Log.Info("Time Completed");
                            }
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
            Log.Info("Time countdown has been set");
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
            return Localizer.Format("#autoLOC_MissionController2_1000217") + " " + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000217 = Land Vessel And Conduct Research
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000218") + " " + Tools.formatTime(missionTime);		// #autoLOC_MissionController2_1000218 = Conduct Research. Time For Completion: 
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
                                Log.Info("Time Completed");
                            }
                            else { }
                        }
                        else { }

                    }
                    else { }
                }
            }
            else { }
        }
        public void contractSetTime()
        {
            savedTime = Planetarium.GetUniversalTime();
            setTime = false;
            Log.Info("Time countdown has been set");
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
        private string moduleName2 = "NoneTest";
        private String ModuleGoalname;
       
        public ModuleGoal()
        {
        }

        public ModuleGoal(string Modulename, string TitleName)
        {
            this.moduleName = Modulename;
            this.ModuleGoalname = TitleName;

        }
        public ModuleGoal(string Modulename, String Modulename2, string TitleName)
        {
            this.moduleName = Modulename;
            this.ModuleGoalname = TitleName;
            this.moduleName2 = Modulename2;
        }

        protected override string GetHashString()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000213") + " " + ModuleGoalname + " " + Localizer.Format("#autoLOC_MissionController2_1000214") + this.Root.MissionSeed.ToString();		// #autoLOC_MissionController2_1000213 = Must Have  		// #autoLOC_MissionController2_1000214 =  On your vessel
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000215") + " " + ModuleGoalname + " " + Localizer.Format("#autoLOC_MissionController2_1000216");		// #autoLOC_MissionController2_1000215 = Must Have  		// #autoLOC_MissionController2_1000216 =  On your vessel
        }

        
        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
            {
                CheckPartGoal(FlightGlobals.ActiveVessel);
            }
            else { }
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref moduleName, "Error Defaults Loaded", moduleName, "partname");
            Tools.ContractLoadCheck(node, ref moduleName2, "Error Defaults Loaded", moduleName2, "partname2");
            Tools.ContractLoadCheck(node, ref ModuleGoalname, "Error Defaults Loaded", ModuleGoalname, "modulegoalname");
        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("partname", moduleName);
            node.AddValue("partname2", moduleName2);
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
                            if (pm.moduleName.Equals(moduleName) || pm.moduleName.Equals(moduleName2))
                            {
                                base.SetComplete();
                            }
                            else { }
                        }
                    }
                }
                else { }
            }
            else { }
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
