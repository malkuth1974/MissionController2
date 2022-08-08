using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;
using Contracts;
using KSP.UI.Screens;
using Contracts.Parameters;

using ToolbarControl_NS;
using static MissionControllerEC.RegisterToolbar;


namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        #region Variables
        public float resourceCost;
        public float vesselPartCost;
        public float vesseltons;
        public bool showFuel = false;
        public bool showTons = false;
        public bool showMiniTons = false;
        public float vesselResourceTons;
        public int currentContractType = 0;
        public int kerbalNumbers;
        public float kerbCost, KerbalFlatrate, kerbalMultiplier, RevertTotal, RevertAltitude,RevertOrbit,RevertPlanet;      
        public Vessel vessel;


        public static int supplyCount;

        public static List<SupplyVesselList> SupVes = new List<SupplyVesselList>();

        public static List<string> CivName = new List<string>();

        Tools.MC2RandomWieghtSystem.Item<int>[] RandomSatelliteContractsCheck;
        #endregion
#region Textures Main Buttons Handling
#if false
        public void loadTextures()
        {
            if (texture == null)
            {
                texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCEStockToolbar.png")));
                //Log.Error("MCE Textures Loaded");
            }
            if (texture2 == null)
            {
                texture2 = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture2.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCERevert.png")));
                //Log.Error("MCE Textures2 Loaded");
            }           
            else { /*Log.Info("MCE Textures Already Loaded"); */}
           
        }
#endif

        public void loadFiles()
        {           
            if (settings.FileExists) { settings.Load(); settings.Save(); }
            else { settings.Save(); settings.Load(); }
            //Log.Error("MCE Settings Loaded");
        }       

        public void MceCreateButtons()
        {
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER && toolbarControl_MCEButton == null)
            {
#if false
                MceDestroyButtons();
                    MCEButton = ApplicationLauncher.Instance.AddModApplication(
                    this.MCEOn,
                    this.MCEOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.SPACECENTER,
                    texture
                    );
#endif
                toolbarControl_MCEButton = gameObject.AddComponent<ToolbarControl>();
                toolbarControl_MCEButton.AddToAllToolbars(this.MCEOn,
                    this.MCEOff,
                    ApplicationLauncher.AppScenes.SPACECENTER,
                    MODID,
                     "MCESpacecenter_Button",
                     "Missioncontroller/PluginData/MCEStockToolbar",
                     "Missioncontroller/PluginData/MCEStockToolbar-24",
                    MODNAME);

                //Log.Error("Creating MCEButton Buttons");
            }
            else { /*Log.Error("MCE2 MCE Button Already Loaded");*/ }

            if (HighLogic.LoadedScene == GameScenes.FLIGHT && toolbarControl_MCERevert == null && HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCERevertAllow)
            {
#if false
                MceDestroyButtons();
                    MCERevert = ApplicationLauncher.Instance.AddModApplication(
                    this.RevertPress,
                    this.KillMCePopups,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    texture2
                    );
#endif
                toolbarControl_MCERevert = gameObject.AddComponent<ToolbarControl>();
                toolbarControl_MCERevert.AddToAllToolbars(this.RevertPress,
                    this.KillMCePopups,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    MODID,
                     "MCEFlight_Button",
                     Path.Combine("Missioncontroller/PluginData/MCERevert"),
                     Path.Combine("Missioncontroller/PluginData/MCERevert-24"),
                    MODNAME);

                //Log.Error("creating MCERevert Buttons");
            }
            else { /*Log.Error("MCE2 MCERevert Already Loaded");*/ }
        }
        private void MCEOn()
        {
            SaveInfo.GUIEnabled = true;
        }

        private void MCEOff()
        {
            SaveInfo.GUIEnabled = false;
        }

#if false
        public void MceDestroyButtons()
        {
            if (MCEButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(MCEButton);
                //Log.Info("MCEButton Deleted With MCEDestroyButtons");
            }           
            if (MCERevert != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(MCERevert);
                //Log.Info("MCERevert Deleted With MCEDestroyButtons");
            }
            else { /*Log.Info("MCE destroy buttons failed"); */}
        }
#endif
        public void GuiDestroy(Vector2 value, PopupDialog popupinfo)
        {            
            value = new Vector2(
                ((Screen.width / 2) + popupinfo.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + popupinfo.RTrf.position.y) / Screen.height);
            //Log.Error("GuiDestroy Info Save as is:  " + value.ToString());
           
        }
#endregion
#region Methods Etc
        public void CheckRandomSatelliteContractTypes()
        {
            randomSatelliteContractsCheck();
            SaveInfo.SatelliteTypeChoice = Tools.MC2RandomWieghtSystem.PickOne<int>(RandomSatelliteContractsCheck);
            //Debug.LogWarning("Satellite Type Chosen Is Number " + SaveInfo.SatelliteTypeChoice);         
        }
      
        public void randomSatelliteContractsCheck()
        {
            RandomSatelliteContractsCheck = new Tools.MC2RandomWieghtSystem.Item<int>[6];
            RandomSatelliteContractsCheck[0] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[0].weight = 35;
            RandomSatelliteContractsCheck[0].value = 0;

            RandomSatelliteContractsCheck[1] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[1].weight = 35;
            RandomSatelliteContractsCheck[1].value = 1;

            RandomSatelliteContractsCheck[2] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[2].weight = 35;
            RandomSatelliteContractsCheck[2].value = 2;

            RandomSatelliteContractsCheck[3] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[3].weight = 35;
            RandomSatelliteContractsCheck[3].value = 3;

            RandomSatelliteContractsCheck[4] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[4].weight = 5;
            RandomSatelliteContractsCheck[4].value = 4;

            RandomSatelliteContractsCheck[5] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[5].weight = 5;
            RandomSatelliteContractsCheck[5].value = 5;
        }

        public void onContractLoaded()
        {
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().RescueKerbalContracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.RecoverAsset)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.RecoverAsset));
                        //Log.Info("Removed RescueKerbal Type Contracts from Gererating");
                    }

                    catch { /*Log.Error("could not run NoRescueKerbalContracts Returned Null");*/ }
                }

                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPExplorationContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.ExplorationContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.ExplorationContract));
                        //Log.Info("Removed Exploration Type Contracts");
                    }

                    catch { Log.Error("could not run Exploration Contracts Returned Null"); }
                }

                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPCometGo && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.CometSampleContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.CometSampleContract));
                        //Log.Info("Removed Comet Type Contracts");
                    }

                    catch { Log.Error("could not run Comet Contracts Returned Null"); }
                }


                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPSatelliteContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.SatelliteContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.SatelliteContract));
                        //Log.Info("Removed FinePrint Satellite Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run FinePrint Satellite Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPBaseContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.BaseContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.BaseContract));
                        //Log.Info("Removed FinePrint Satellite Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run FinePrint Satellite Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPStationContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.StationContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.StationContract));
                        //Log.Info("Removed FinePrint Satellite Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run FinePrint Satellite Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPISRUContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.ISRUContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.ISRUContract));
                        //Log.Info("Removed FinePrint Satellite Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run FinePrint Satellite Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPTouricsmContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.TourismContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.TourismContract));
                        //Log.Info("Removed FinePrint Satellite Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run FinePrint Satellite Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPSurveyContracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.SurveyContract)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.SurveyContract));
                        //Log.Info("Removed FinePrint Survey Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run FinePrint Survey Contracts Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().PartTestContracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.PartTest)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.PartTest));
                        //Log.Info("Removed PartTest Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run NoPartTest Returned Null"); }
                }
                if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings2>().FPGrandTourContracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.GrandTour)))
                {
                    try
                    {
                        ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.GrandTour));
                        //Log.Info("Removed PartTest Type Contracts from Gererating");
                    }

                    catch { Log.Error("could not run NoPartTest Returned Null"); }
                }
                Log.Info("MCE Remove Contracts loaded");
            }
            else { Log.Info("MCE Debug Log, No Contracts Loaded at this time, need Game Save Loaded"); }

            
        }
        
        public void GetRefundCost()
        {            
            vessel = FlightGlobals.ActiveVessel;
            if (vessel == null || vessel.situation == Vessel.Situations.PRELAUNCH) { Log.Info("No Active vessel for Part Calculation"); }
            else
            {
                foreach (ProtoPartSnapshot pps in vessel.protoVessel.protoPartSnapshots)
                {
                    float dryCost, fuelCost;
                    ShipConstruction.GetPartCosts(pps, pps.partInfo, out dryCost, out fuelCost);
                    dryCost = dryCost < 0 ? 0 : dryCost;
                    fuelCost = fuelCost < 0 ? 0 : fuelCost;
                    RevertTotal += dryCost + fuelCost;                   
                }
                if (vessel.situation == Vessel.Situations.ORBITING)
                {
                    Math.Round(RevertOrbit = RevertTotal / 85);
                    Log.Info("Revert Orbit = " + RevertOrbit);
                }
                if (vessel.altitude <= 10000 & vessel.situation != Vessel.Situations.ORBITING)
                {
                    Math.Round(RevertAltitude = RevertTotal / 90);
                    Log.Info("Revert Altitude Below 10k = " + RevertAltitude);
                }
                if (vessel.altitude > 10001 & vessel.situation != Vessel.Situations.ORBITING)
                {
                    Math.Round(RevertAltitude = RevertTotal / 95);
                    Log.Info("Revert Altitude Above 10K = " + RevertAltitude);
                }
            }
            Log.Info("Revert Cost Of Vessel Is " + RevertTotal);
        }

        public void getSupplyList(bool stationOnly)
        {
            bool boolStation = stationOnly;
            supplyCount = 0;
            SupVes.Clear();

            if (!stationOnly)
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.vesselType == VesselType.Station || v.vesselType == VesselType.Base)
                    {
                        SupVes.Add(new SupplyVesselList(v.name.Replace("(unloaded)", ""), v.id.ToString(), v.mainBody));
                        supplyCount++;
                        //Log.Info("Found Vessel " + v.name + " " + v.vesselType + " Count is: " + supplyCount);
                    }
                }
            }
            else
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.vesselType == VesselType.Station)
                    {
                        SupVes.Add(new SupplyVesselList(v.name.Replace("(unloaded)", ""), v.id.ToString(), v.mainBody));
                        supplyCount++;
                        //Log.Info("Found Vessel " + v.name + " " + v.vesselType + " Count is: " + supplyCount);
                    }
                    else { }
                }
            }
        }

        public void SendRevertMessage()
        {
            MessageSystem.Message m = new MessageSystem.Message("Revert Charges", "Your Charges For Using Revert System is\n\n" + "Altitude Charge: " + RevertAltitude + "\n\n" + "Orbit Charge: " + RevertOrbit + "\n\n" + " Veseel simulation Cost: " + RevertTotal + "\n\n" + "Total Charges: " + Math.Round(RevertAltitude + RevertOrbit + RevertTotal), MessageSystemButton.MessageButtonColor.ORANGE, MessageSystemButton.ButtonIcons.MESSAGE);
            MessageSystem.Instance.AddMessage(m);

        }

        //public void GetEvaTypeKerbal()
        //{
        //    List<ProtoCrewMember> protoCrewMembers = FlightGlobals.ActiveVessel.GetVesselCrew();
        //    foreach (Experience.ExperienceEffect exp in protoCrewMembers[0].experienceTrait.Effects)
        //    {
        //        if (exp.ToString() == "Experience.Effects.RepairSkill")
        //        {
        //            Log.Info("Current kerbal is a Engineer you have passed");
        //        }
        //        else
        //        {
        //            Log.Info("Current kerbal is NOT an Engineer you don't pass... Bad boy!");
        //        }
        //    }
        //}
        //public void SetVesselLaunchCurrentTime()
        //{
        //    Log.Info("Old LaunchTime was: " + Tools.ConvertDays(FlightGlobals.ActiveVessel.launchTime));
        //    double currentTime = Planetarium.GetUniversalTime();
        //    FlightGlobals.ActiveVessel.launchTime = currentTime;
        //    Log.Info("New LaunchTime Is: " + Tools.ConvertDays(FlightGlobals.ActiveVessel.launchTime));
        //}
        public void GetContractList()
        {
            if (ContractSystem.Instance != null)
            {
                foreach (Contract cs in ContractSystem.Instance.ContractsFinished)
                {
                    Type test = cs.GetType();
                    if (test.Name == "WorldFirstContract")
                    {
                        Debug.LogWarning(cs.Title);
                    }                   
                }

            }
            else
            {
                //Log.Info("Contract Instance Not found, list not loading");
            }  
            if (ProgressTracking.Instance != null)
            {
                
            }         
        }
        //public void GetLongAndLat(Vessel v)
        //{
        //    double longitude;
        //    double latitude;
        //    longitude = v.longitude;
        //    latitude = v.latitude;
        //    Log.Error("Current longitude is " + longitude + " Current latitude is " + latitude);
        //}
        //public void setOrbitLandNodes()
        //{
        //    SaveInfo.OrbitNamesList.Add("Landing");
        //    SaveInfo.OrbitNamesList.Add("Orbit");
        //    SaveInfo.OrbitNamesList.Add("SubTraject");
        //    SaveInfo.OrbitNamesList.Add("Flyby");

        //}
#endregion
    }

    public class RepairVesselsList
    {
        public string vesselName;
        public string vesselId;
        public int bodyidx;
        public double MaxApA;
        public RepairVesselsList()
        {
        }
        public RepairVesselsList(string name, string id,double maxAP,int Vbody)
        {
            this.vesselName = name;
            this.vesselId = id;
            this.MaxApA = maxAP;
            this.bodyidx = Vbody;
        }


    }

    public class SupplyVesselList
    {
        public string vesselName;
        public string vesselId;
        public CelestialBody body;
        public SupplyVesselList()
        {
        }
        public SupplyVesselList(string name, string vId, CelestialBody bod)
        {
            this.vesselName = name;
            this.vesselId = vId;
            this.body = bod;
        }
    }
    public class McContractList
    {
        public string ContractName;
        public bool ContractDisabled = false;
        public McContractList() { }
        public McContractList(string ContName, bool ContDisabled)
        {
            this.ContractName = ContName;
            this.ContractDisabled = ContDisabled;
        }
        
    }   
}
