
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using MissionControllerEC;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using KSP;

namespace MissionControllerEC
{  
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class MCE_ScenarioStartup : MonoBehaviour
    {
        public static AssemblyName assemblyName;
        public static String versionCode;
        public static String mainWindowTitle;

        public static Rect PopUpWindowPosition3;
        public static bool ShowPopUpWindow3;

        public static Rect MainWindowPosition;
        public static bool ShowMainWindow = false;

        public static Rect SettingsWindowPostion;
        public static bool ShowSettingsWindow = false;

        public static Rect FinanceWindowPosition;
        public static bool ShowfinanaceWindow = false;

        public static Rect EditorWindowPosition;
        public static bool ShowEditorWindow = false;
        public static Vector2 scrollPosition = new Vector2(0, 0);

        public static Rect CustomWindowPostion;
        public static bool ShowCustomWindow;

        public static GUIStyle StyleWhite, StyleBold, styleBoxWhite, styleBlue, styleBlueBold, styleGreenBold;

        public static bool RevertHalt = false;

        // Special thanks to Magico13 Of Kerbal Construction Time for showing me how to Get Scenario Persistance.

        void Start()
        {
            
            ProtoScenarioModule scenario = HighLogic.CurrentGame.scenarios.Find(s => s.moduleName == typeof(MissionControllerData).Name);                        
            
            if (scenario == null)
            {
                try
                {
                    HighLogic.CurrentGame.AddProtoScenarioModule(typeof(MissionControllerData), new GameScenes[] { GameScenes.FLIGHT, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.TRACKSTATION });
                    Debug.LogWarning("[MCE] Adding InternalModule scenario to game '" + HighLogic.CurrentGame.Title + "'");
                    // the game will add this scenario to the appropriate persistent file on save from now on
                }
                catch (ArgumentException ae)
                {
                    Debug.LogException(ae);
                }
                catch
                {
                    Debug.LogWarning("[MCE] Unknown failure while adding scenario.");
                }
            }
            else
            {
                //Debug.LogWarning("[MCE] Scenario is not null.");
                if (!scenario.targetScenes.Contains(GameScenes.SPACECENTER))
                    scenario.targetScenes.Add(GameScenes.SPACECENTER);
                if (!scenario.targetScenes.Contains(GameScenes.FLIGHT))
                    scenario.targetScenes.Add(GameScenes.FLIGHT);
                if (!scenario.targetScenes.Contains(GameScenes.EDITOR))
                    scenario.targetScenes.Add(GameScenes.EDITOR);               
                if (!scenario.targetScenes.Contains(GameScenes.TRACKSTATION))
                    scenario.targetScenes.Add(GameScenes.TRACKSTATION);

            }                   
        }
        void Awake()
        {
            assemblyName = Assembly.GetExecutingAssembly().GetName();
            versionCode = assemblyName.Version.Major.ToString() + "." + assemblyName.Version.Minor.ToString() + "." + assemblyName.Version.Build.ToString();
            mainWindowTitle = "Mission Controller 2 ";
        }
        public static void loadStyles()
        {
            StyleWhite = new GUIStyle(GUI.skin.label);
            StyleWhite.normal.textColor = Color.white;
            StyleWhite.fontStyle = FontStyle.Normal;
            StyleWhite.alignment = TextAnchor.MiddleLeft;
            StyleWhite.wordWrap = true;

            styleBlue = new GUIStyle(GUI.skin.label);
            styleBlue.normal.textColor = Color.cyan;
            styleBlue.fontStyle = FontStyle.Normal;
            styleBlue.alignment = TextAnchor.MiddleLeft;
            styleBlue.wordWrap = true;

            styleBoxWhite = new GUIStyle(GUI.skin.box);
            styleBoxWhite.normal.textColor = Color.grey;
            styleBoxWhite.fontStyle = FontStyle.Normal;
            styleBoxWhite.alignment = TextAnchor.MiddleLeft;
            styleBoxWhite.wordWrap = true;

            StyleBold = new GUIStyle(GUI.skin.box);
            StyleBold.normal.textColor = Color.white;
            StyleBold.fontStyle = FontStyle.Bold;
            StyleBold.alignment = TextAnchor.MiddleLeft;
            StyleBold.wordWrap = true;

            styleBlueBold = new GUIStyle(GUI.skin.box);
            styleBlueBold.normal.textColor = Color.cyan;
            styleBlueBold.fontStyle = FontStyle.Bold;
            styleBlueBold.alignment = TextAnchor.MiddleLeft;
            styleBlueBold.wordWrap = true;

            styleGreenBold = new GUIStyle(GUI.skin.box);
            styleGreenBold.normal.textColor = Color.green;
            styleGreenBold.fontStyle = FontStyle.Bold;
            styleGreenBold.alignment = TextAnchor.MiddleLeft;
            styleGreenBold.wordWrap = true;

        }
    }

    public class MissionControllerData : ScenarioModule
    {
        // used TacLife Support by Taranis Elsu way of loading Components for the MissionControllerEC Component 
        private readonly List<Component> mcechildren = new List<Component>();

        public override void OnAwake()
        {
            //Debug.Log("OnAwake in " + HighLogic.LoadedScene);


            if (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.EDITOR)
            {
                //Debug.Log("Adding MissionController Child");
                var c = gameObject.AddComponent<MissionControllerEC>();
                mcechildren.Add(c);
            }
        }

        public override void OnSave(ConfigNode node)
        {
            //Debug.Log("[MCE] Writing to persistence.");
            base.OnSave(node);
            MCE_DataStorage mceData = new MCE_DataStorage();
            node.AddNode(mceData.AsConfigNode());
        }
        public override void OnLoad(ConfigNode node)
        {
            //Debug.Log("[MCE] Loading from persistence.");
            base.OnLoad(node);
            MCE_DataStorage mceData = new MCE_DataStorage();
            ConfigNode CN = node.GetNode(mceData.GetType().Name);
            if (CN != null)
                ConfigNode.LoadObjectFromConfig(mceData, CN);
        }

        void OnDestroy()
        {
            //Debug.Log("MCE ScenarioModule OnDestroy");
            foreach (Component c in mcechildren)
            {
                Destroy(c);
                //MCELoaded = false;
            }
            mcechildren.Clear();
        }
    }
    public partial class MissionControllerEC : MonoBehaviour
    {
        private static Texture2D texture;
        private static Texture2D texture2;
        private ApplicationLauncherButton MCEButton;
        private ApplicationLauncherButton MCERevert;
        private string tirosNumber;
        private string marinerNumber;
        private string apolloNumber;
        
        Settings settings = new Settings("Config.cfg");        

        public void Awake()
        {
            DontDestroyOnLoad(this);
            loadTextures();
            loadFiles();
            CreateButtons();          
            GameEvents.Contract.onContractsLoaded.Add(this.onContractLoaded);
            GameEvents.onGameSceneLoadRequested.Add(this.CheckRandomContractTypes);
            //Debug.Log("MCE Awake");
            getSupplyList(false);
        }    
                
        public void Start()
        {                 
            DictCount = settings.SupplyResourceList.Count();           
        }            
                      
        void OnDestroy()
        {
            DestroyButtons();
            //Debug.Log("MCE OnDestroy");
            GameEvents.Contract.onContractsLoaded.Remove(this.onContractLoaded);
            GameEvents.onGameSceneLoadRequested.Remove(this.CheckRandomContractTypes);
            //Debug.Log("Game All values removed for MCE");
        }                
      
        public void OnGUI()
        {
            MCE_ScenarioStartup.loadStyles();
            
            if (MCE_ScenarioStartup.ShowMainWindow)
            {
                MCE_ScenarioStartup.MainWindowPosition = GUILayout.Window(971974, MCE_ScenarioStartup.MainWindowPosition, DrawMainWindow, "Maine MCE Window", GUILayout.MaxHeight(600), GUILayout.MaxWidth(400), GUILayout.MinHeight(300), GUILayout.MinWidth(200));
                MCE_ScenarioStartup.MainWindowPosition.x = Mathf.Clamp(MCE_ScenarioStartup.MainWindowPosition.x, 0, Screen.width - MCE_ScenarioStartup.MainWindowPosition.width);
                MCE_ScenarioStartup.MainWindowPosition.y = Mathf.Clamp(MCE_ScenarioStartup.MainWindowPosition.y, 0, Screen.height - MCE_ScenarioStartup.MainWindowPosition.height);
            }
            if (MCE_ScenarioStartup.ShowfinanaceWindow)
            {
                MCE_ScenarioStartup.FinanceWindowPosition = GUILayout.Window(981974, MCE_ScenarioStartup.FinanceWindowPosition, drawFinanceWind, "MCE Finances", GUILayout.MaxHeight(350), GUILayout.MaxWidth(400), GUILayout.MinHeight(250), GUILayout.MinWidth(390));
                MCE_ScenarioStartup.FinanceWindowPosition.x = Mathf.Clamp(MCE_ScenarioStartup.FinanceWindowPosition.x, 0, Screen.width - MCE_ScenarioStartup.FinanceWindowPosition.width);
                MCE_ScenarioStartup.FinanceWindowPosition.y = Mathf.Clamp(MCE_ScenarioStartup.FinanceWindowPosition.y, 0, Screen.height - MCE_ScenarioStartup.FinanceWindowPosition.height);
            }
            if (MCE_ScenarioStartup.ShowCustomWindow)
            {
                MCE_ScenarioStartup.CustomWindowPostion = GUILayout.Window(1091974, MCE_ScenarioStartup.CustomWindowPostion, drawCustomGUI, "Custom Contracts", GUILayout.MaxHeight(800), GUILayout.MaxWidth(400), GUILayout.MinHeight(250), GUILayout.MinWidth(390));
                MCE_ScenarioStartup.CustomWindowPostion.x = Mathf.Clamp(MCE_ScenarioStartup.CustomWindowPostion.x, 0, Screen.width - MCE_ScenarioStartup.CustomWindowPostion.width);
                MCE_ScenarioStartup.CustomWindowPostion.y = Mathf.Clamp(MCE_ScenarioStartup.CustomWindowPostion.y, 0, Screen.height - MCE_ScenarioStartup.CustomWindowPostion.height);
            }
            if (MCE_ScenarioStartup.ShowEditorWindow)
            {
                MCE_ScenarioStartup.EditorWindowPosition = GUILayout.Window(1031974, MCE_ScenarioStartup.EditorWindowPosition, drawEditorwindow, "Ship Cost BreakDown", GUILayout.MaxHeight(800), GUILayout.MaxWidth(410), GUILayout.MinHeight(750), GUILayout.MinWidth(410));
                MCE_ScenarioStartup.EditorWindowPosition.x = Mathf.Clamp(MCE_ScenarioStartup.EditorWindowPosition.x, 0, Screen.width - MCE_ScenarioStartup.EditorWindowPosition.width);
                MCE_ScenarioStartup.EditorWindowPosition.y = Mathf.Clamp(MCE_ScenarioStartup.EditorWindowPosition.y, 0, Screen.height - MCE_ScenarioStartup.EditorWindowPosition.height);
            }
            
            if (MCE_ScenarioStartup.ShowSettingsWindow)
            {
                MCE_ScenarioStartup.SettingsWindowPostion = GUILayout.Window(1061974, MCE_ScenarioStartup.SettingsWindowPostion, drawSettings, "MCE Settings", GUILayout.MaxHeight(500), GUILayout.MaxWidth(400), GUILayout.MinHeight(250), GUILayout.MinWidth(400));
                MCE_ScenarioStartup.SettingsWindowPostion.x = Mathf.Clamp(MCE_ScenarioStartup.SettingsWindowPostion.x, 0, Screen.width - MCE_ScenarioStartup.SettingsWindowPostion.width);
                MCE_ScenarioStartup.SettingsWindowPostion.y = Mathf.Clamp(MCE_ScenarioStartup.SettingsWindowPostion.y, 0, Screen.height - MCE_ScenarioStartup.SettingsWindowPostion.height);
            }
            if (MCE_ScenarioStartup.ShowPopUpWindow3)
            {
                MCE_ScenarioStartup.PopUpWindowPosition3 = GUILayout.Window(1011974, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 450, 150), drawPopUpWindow3, "MCE Information Window");
            }
        }

        private void DrawMainWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
           
            GUILayout.Label("Current Funds: " + Funding.Instance.Funds);

            if (GUILayout.Button("Add Money"))
            {
                Funding.Instance.AddFunds(500000,TransactionReasons.Cheating);
            }

            GUILayout.Label("Current Science: " + ResearchAndDevelopment.Instance.Science);
            if (GUILayout.Button("Add Science"))
            {
                ResearchAndDevelopment.Instance.AddScience(1000,TransactionReasons.Cheating);
            }
           
            if (GUILayout.Button("Set Agena Current Selected Vessel (Must be in flight)"))
            {
                SaveInfo.AgenaTargetVesselName = FlightGlobals.ActiveVessel.vesselName;
                SaveInfo.AgenaTargetVesselID = FlightGlobals.ActiveVessel.id.ToString();
            }

            if (GUILayout.Button("Set SkyLab Current Selected Vessel(Must be in flight)"))
            {
                SaveInfo.skyLabName = FlightGlobals.ActiveVessel.vesselName;
                SaveInfo.skyLabVesID = FlightGlobals.ActiveVessel.id.ToString();
            }

            if (GUILayout.Button("Set Landing Site for Apollo Missions(Flight)"))
            {
                GetLatandLonDefault(FlightGlobals.ActiveVessel);
                Debug.Log("Apollo Landing Lat: " + SaveInfo.apolloLandingLat + " Apollo Landing Lon: " + SaveInfo.apolloLandingLon);
            }

            if (GUILayout.Button("Debug Landing Site For Apollo(Flight)"))
            {
                Debug.Log("Current Lat + Lon: " + FlightGlobals.ActiveVessel.latitude + " " + FlightGlobals.ActiveVessel.longitude + " Current Saved Values Lat + Lon " + SaveInfo.apolloLandingLat + " " + SaveInfo.apolloLandingLon);
            }
 
            if (GUILayout.Button("Set Agena1 Bool False"))
            {
                SaveInfo.Agena1Done = false;
            }

            if (GUILayout.Button("Set Agena2 Bool False"))
            {
                SaveInfo.Agena2Done = false;
            }

            if (GUILayout.Button("Set Voskov1 Bool false"))
            {
                SaveInfo.Vostok1Done = false;
            }
            if (GUILayout.Button("Set Voskov2 Bool false"))
            {
                SaveInfo.Vostok2Done = false;
            }
            if (GUILayout.Button("Set Voskhod2 Bool false"))
            {
                SaveInfo.Voskhod2Done = false;
            }
            if (GUILayout.Button("Set Luna2 Bool false"))
            {
                SaveInfo.Luna2Done = false;
            }
            if (GUILayout.Button("Set Luna3 Bool false"))
            {
                SaveInfo.Luna16Done = false;
            }
            if (GUILayout.Button("Turn On All Repair Mission At Once"))
            {
                SaveInfo.RepairContractOn = true; SaveInfo.RepairStationContract = true;
            }
            
            if (GUILayout.Button("Test EVA Type Kerbal"))
            {
                GetEvaTypeKerbal();
            }
            if (GUILayout.Button("Set Vessel Current Launch Time"))
            {
                SetVesselLaunchCurrentTime();
            }
            GUILayout.BeginHorizontal();
            GUILayout.Box("Tiros Number 1-3", MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            tirosNumber = SaveInfo.tirosCurrentNumber.ToString();
            tirosNumber= Regex.Replace(GUILayout.TextField(tirosNumber), "[^.0-9]", "");
            SaveInfo.tirosCurrentNumber = int.Parse(tirosNumber);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Mariner Number 1-4", MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            marinerNumber = SaveInfo.marinerCurrentNumber.ToString();
            marinerNumber = Regex.Replace(GUILayout.TextField(marinerNumber), "[^.0-9]", "");
            SaveInfo.marinerCurrentNumber = int.Parse(marinerNumber);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Apollo Number 1-6", MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            apolloNumber = SaveInfo.apolloCurrentNumber.ToString();
            apolloNumber = Regex.Replace(GUILayout.TextField(apolloNumber), "[^.0-9]", "");
            SaveInfo.apolloCurrentNumber = int.Parse(apolloNumber);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box("Apollo-Duna Number 1-9", MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            apolloNumber = SaveInfo.apolloDunaCurrentNumber.ToString();
            apolloNumber = Regex.Replace(GUILayout.TextField(apolloNumber), "[^.0-9]", "");
            SaveInfo.apolloDunaCurrentNumber = int.Parse(apolloNumber);
            GUILayout.EndHorizontal();
                          
            GUILayout.EndVertical();
            if (GUILayout.Button("Exit Save Settings"))
            {
                MCE_ScenarioStartup.ShowMainWindow = false;
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }      
    
    }
               
    public class MCE_DataStorage : ConfigNodeStorage
    {        
        [Persistent]public bool ComSatOn = false;
        [Persistent]public string agenaTargetVesselID = "none";
        [Persistent]public string agenaTargetVesselName = "none";
        [Persistent]public bool agena1done = false;
        [Persistent]public bool agena2done = false;
        [Persistent]public bool vostok1done = false;
        [Persistent]public bool vostok2done = false;
        [Persistent]public bool voskhod2done = false;
        [Persistent]public bool luna2done = false;
        [Persistent]public bool luna16done = false;
        [Persistent]public bool skylab1done = false;
        [Persistent]public bool skylab2done = false;
        [Persistent]public bool skylab3done = false;
        [Persistent]public bool skylab4done = false;
        [Persistent]public bool messagehelpers = false;

        [Persistent]public string skylabname = "none";
        [Persistent]public string skylabID = "none";

        
        [Persistent]public string supplyVesselName = "None";
        [Persistent]public string supplyVesselId = "none";
        [Persistent]public string supplyResource = "none";
        [Persistent]public string supplyContractName = "none";
        [Persistent]public int supplybodyIDX;
        [Persistent]public bool supplyContractOn = false;
        [Persistent]public double supplyResAmount = 0;

        [Persistent]public string crewtransfername = "crew transfer";
        [Persistent]public string crewvesname = "none";
        [Persistent]public string crewvesId = "none";
        [Persistent]public int crewamount = 0;
        [Persistent]public double crewtime = 0;
        [Persistent]public int crewbodyIDX = 0;
        [Persistent]public bool crewcontracton = false;

        [Persistent]public bool apCivilianPod = false;
        [Persistent]public string apCivilianName = "none";

        [Persistent]public bool noOrbitalContract = false;
        [Persistent]public bool noLandingContract = false;
        [Persistent]public bool noSatelliteContract = false;
        [Persistent]public bool noRepairContract = false;
        [Persistent]public bool noOrbitalPeriodContract = false;
        [Persistent]public bool noHistoricContracts = false;
        [Persistent]public bool noCivilianContracts = false;

        [Persistent]internal bool com_Sat_Start_Building = false;
        [Persistent]internal double com_Sat_maxOrbP = 10860;
        [Persistent]internal double com_Sat_minOrbP = 10680;
        [Persistent]internal string com_Sat_contractName = "Deliever COMSAT Satellite";
        [Persistent]internal int com_Sat_bodyNumber = 1;
        [Persistent]internal bool hardcoreOn = true;

        [Persistent]internal int tirosNumber = 1;
        [Persistent]internal int marinerNumber = 1;
        [Persistent]internal int apolloNumber = 1;
        [Persistent]internal int apolloDunaNumber = 1;
        [Persistent]internal bool apolloStationStatus = false;

        [Persistent]internal double apolldunLat = 1;
        [Persistent]internal double apolldunLon = 1;
      
        public override void OnDecodeFromConfigNode()
        {           
            SaveInfo.apolloDunaStation = apolloStationStatus;
            SaveInfo.apolloLandingLat = apolldunLat;
            SaveInfo.apolloLandingLon = apolldunLon;
            SaveInfo.tirosCurrentNumber = tirosNumber;
            SaveInfo.marinerCurrentNumber = marinerNumber;
            SaveInfo.apolloCurrentNumber = apolloNumber;
            SaveInfo.apolloDunaCurrentNumber = apolloDunaNumber;
            SaveInfo.Hardcore_Vessel_Must_Survive = hardcoreOn;            
            SaveInfo.SatContractReady = ComSatOn;
            SaveInfo.AgenaTargetVesselID = agenaTargetVesselID;
            SaveInfo.AgenaTargetVesselName = agenaTargetVesselName;
            SaveInfo.Agena1Done = agena1done;
            SaveInfo.Agena2Done = agena2done;
            SaveInfo.Vostok1Done = vostok1done;
            SaveInfo.Vostok2Done = vostok2done;
            SaveInfo.Voskhod2Done = voskhod2done;
            SaveInfo.Luna2Done = luna2done;
            SaveInfo.Luna16Done = luna16done;
            SaveInfo.skylab1done = skylab1done;
            SaveInfo.skylab2done = skylab2done;
            SaveInfo.skylab3done = skylab3done;
            SaveInfo.skylab4done = skylab4done;
            SaveInfo.MessageHelpers = messagehelpers;

            SaveInfo.ComSateContractOn = com_Sat_Start_Building;
            SaveInfo.comSatmaxOrbital = com_Sat_maxOrbP;
            SaveInfo.comSatminOrbital = com_Sat_minOrbP;
            SaveInfo.ComSatContractName = com_Sat_contractName;
            SaveInfo.comSatBodyName = com_Sat_bodyNumber;

            SaveInfo.crewContractOn = crewcontracton;
            SaveInfo.crewAmount = crewamount;
            SaveInfo.crewBodyIDX = crewbodyIDX;
            SaveInfo.crewTime = crewtime;
            SaveInfo.crewTransferName = crewtransfername;
            SaveInfo.crewVesid = crewvesId;
            SaveInfo.crewVesName = crewvesname;

            SaveInfo.skyLabName = skylabname;
            SaveInfo.skyLabVesID = skylabID;

            SaveInfo.SupplyVesName = supplyVesselName;
            SaveInfo.SupplyVesId = supplyVesselId;
            SaveInfo.ResourceName = supplyResource;
            SaveInfo.SupplyContractName = supplyContractName;
            SaveInfo.SupplyBodyIDX = supplybodyIDX;
            SaveInfo.supplyContractOn = supplyContractOn;
            SaveInfo.supplyAmount = supplyResAmount;
           
            SaveInfo.NoOrbitalResearchContracts = noOrbitalContract;
            SaveInfo.NoLanderResearchContracts = noLandingContract;
            SaveInfo.NoSatelliteContracts = noSatelliteContract;
            SaveInfo.NoRepairContracts = noRepairContract;
            SaveInfo.NoOrbitalPeriodcontracts = noOrbitalPeriodContract;
            SaveInfo.all_Historical_Contracts_Off = noHistoricContracts;           

        }

        public override void OnEncodeToConfigNode()
        {         
            apolloStationStatus = SaveInfo.apolloDunaStation;
            apolldunLat = SaveInfo.apolloLandingLat;
            apolldunLon = SaveInfo.apolloLandingLon;
            tirosNumber = SaveInfo.tirosCurrentNumber;
            marinerNumber = SaveInfo.marinerCurrentNumber;
            apolloNumber = SaveInfo.apolloCurrentNumber;
            apolloDunaNumber = SaveInfo.apolloDunaCurrentNumber;
            hardcoreOn = SaveInfo.Hardcore_Vessel_Must_Survive;           
            ComSatOn = SaveInfo.SatContractReady;
            agenaTargetVesselID = SaveInfo.AgenaTargetVesselID;
            agenaTargetVesselName = SaveInfo.AgenaTargetVesselName;
            agena1done = SaveInfo.Agena1Done;
            agena2done = SaveInfo.Agena2Done;
            vostok1done = SaveInfo.Vostok1Done;
            vostok2done = SaveInfo.Vostok2Done;
            voskhod2done = SaveInfo.Voskhod2Done;
            luna2done = SaveInfo.Luna2Done;
            luna16done = SaveInfo.Luna16Done;
            skylab1done = SaveInfo.skylab1done;
            skylab2done = SaveInfo.skylab2done;
            skylab3done = SaveInfo.skylab3done;
            skylab4done = SaveInfo.skylab4done;
            messagehelpers = SaveInfo.MessageHelpers;

            com_Sat_Start_Building = SaveInfo.ComSateContractOn;
            com_Sat_maxOrbP = SaveInfo.comSatmaxOrbital;
            com_Sat_minOrbP = SaveInfo.comSatminOrbital;
            com_Sat_contractName = SaveInfo.ComSatContractName;
            com_Sat_bodyNumber = SaveInfo.comSatBodyName;

            crewcontracton = SaveInfo.crewContractOn;
            crewamount = SaveInfo.crewAmount;
            crewbodyIDX = SaveInfo.crewBodyIDX;
            crewtime = SaveInfo.crewTime;
            crewtransfername = SaveInfo.crewTransferName;
            crewvesId = SaveInfo.crewVesid;
            crewvesname = SaveInfo.crewVesName;

            skylabname = SaveInfo.skyLabName;
            skylabID = SaveInfo.skyLabVesID;

            supplyVesselName = SaveInfo.SupplyVesName;
            supplyVesselId = SaveInfo.SupplyVesId;
            supplyResource = SaveInfo.ResourceName;
            supplyContractName = SaveInfo.SupplyContractName;
            supplybodyIDX = SaveInfo.SupplyBodyIDX;
            supplyContractOn = SaveInfo.supplyContractOn;
            supplyResAmount = SaveInfo.supplyAmount;
          
            noOrbitalContract = SaveInfo.NoOrbitalResearchContracts;
            noLandingContract = SaveInfo.NoLanderResearchContracts;
            noSatelliteContract = SaveInfo.NoSatelliteContracts;
            noRepairContract = SaveInfo.NoRepairContracts;
            noOrbitalPeriodContract = SaveInfo.NoOrbitalPeriodcontracts;
            noHistoricContracts = SaveInfo.all_Historical_Contracts_Off;            
        }
    

    }
}
