
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using MissionControllerEC;
using System.Collections.Generic;
using KSP.Localization;
using System.Reflection;
using KSP.UI.Screens;
using System.Text;
using System.IO;
using KSP;

namespace MissionControllerEC
{    
   
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public partial class MissionControllerEC : MonoBehaviour
    {
        private static Texture2D texture;
        private static Texture2D texture2;
        private ApplicationLauncherButton MCEButton;
        private ApplicationLauncherButton MCERevert;
        private string tirosNumber;
        private string marinerNumber;
        private string apolloNumber;
        private int id = new System.Random().Next(int.MaxValue);
        public static bool RevertHalt = false;        
       
        // Special thanks to Magico13 Of Kerbal Construction Time for showing me how to Get Scenario Persistance.
        // Some of New GUI Elements learned from Kerbal Forum and piezPiedPy - KSP Trajectories https://github.com/PiezPiedPy/KSPTrajectories/blob/NewGui-Test/Plugin/MainGUI.cs
        // constants
        private const float width = 210;
        private const float height = 110;
        private const float button_width = 200.0f;
        private const float button_height = 25.0f;

        private const float Contract_Button_Large_W = 300;
        private const float Contract_Button_Large_H = 30;
        private const float Contract_Button_Med_W = 150;
        private const float Contract_Button_Med_H = 30;
        private const float Contract_Button_Small_W = 70;
        private const float Contract_Button_Small_H = 30;

        public static AssemblyName assemblyName;
        public static String versionCode;

        public static GUIStyle StyleBold, styleBlueBold, styleGreenBold;

        private static MissionControllerEC instance = null;

        private static bool visible = false;

        private static MultiOptionDialog Mainmulti_dialog;
        private static MultiOptionDialog CSatmulti_dialog;
        private static MultiOptionDialog CSupplyMulti_Dialog;
        private static MultiOptionDialog CCrewMulti_Dialog;
        private static MultiOptionDialog DebugMulti_Dialg;
        private static PopupDialog popup_dialog;
        private static PopupDialog customSatPop_dialg;
        private static PopupDialog customSupPop_dialg;
        private static PopupDialog customCrewPop_Dialg;
        private static PopupDialog customDebug_Dialg;


        private static DialogGUIBase Debug_button;
        private static DialogGUIBase CustomCrew_button;
        private static DialogGUIBase CustomSat_button;
        private static DialogGUIBase CustomSupply_button;

        private static DialogGUIBase Custom_Contract_Button1;
        private static DialogGUIBase Custom_Contract_Button2;
        private static DialogGUIBase Custom_Contract_Button3;
        private static DialogGUIBase Custom_Contract_Button4;
        private static DialogGUIBase Custom_Contract_Button5;
        private static DialogGUIBase Custom_Contract_Button6;
        private static DialogGUIBase Custom_Contract_Button7;
        private static DialogGUIBase Custom_Contract_Button8;
        private static DialogGUIBase Custom_Contract_Button9;
        private static DialogGUIBase Custom_Contract_Button10;
        private static DialogGUIBase Custom_Contract_Button11;

        private static DialogGUIToggleButton Custom_Contract_Toggle1;
        private static DialogGUIToggleButton Custom_Contract_Toggle2;
        private static DialogGUIToggleButton Custom_Contract_Toggle3;
        private static DialogGUIToggleButton Custom_Contract_Toggle4;
        private static DialogGUIToggleButton Custom_Contract_Toggle5;
        private static DialogGUIToggleButton Custom_Contract_Toggle6;
        private static DialogGUIToggleButton Custom_Contract_Toggle7;
        private static DialogGUIToggleButton Custom_Contract_Toggle8;       

        private static DialogGUIBox Custom_Contract_GuiBox1;
        private static DialogGUIBox Custom_Contract_GuiBox2;
        private static DialogGUIBox Custom_Contract_GuiBox3;
        private static DialogGUIBox Custom_Contract_GuiBox4;
        private static DialogGUIBox Custom_Contract_GuiBox5;
        private static DialogGUIBox Custom_Contract_GuiBox6;
        private static DialogGUIBox Custom_Contract_GuiBox7;
        private static DialogGUIBox Custom_Contract_GuiBox8;
        private static DialogGUIBox Custom_Contract_GuiBox9;
        private static DialogGUIBox Custom_Contract_GuiBox10;

        private static DialogGUIBase Custom_Contract_Input;      

        Settings settings = new Settings("Config.cfg");

        public static MissionControllerEC Instance
        {
            get
            {
                return Instance;
            }
        }

        public MissionControllerEC()
        {           
            instance = this;
            Allocate();
        }

    void Start()
    {

            ProtoScenarioModule scenario = HighLogic.CurrentGame.scenarios.Find(s => s.moduleName == typeof(MissionControllerData).Name);
            DictCount = settings.SupplyResourceList.Count();
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
        
        public void Awake()
        {
            assemblyName = Assembly.GetExecutingAssembly().GetName();
            versionCode = assemblyName.Version.Major.ToString() + "." + assemblyName.Version.Minor.ToString() + "." + assemblyName.Version.Build.ToString();
            DontDestroyOnLoad(this);
            loadTextures();
            loadFiles();
            CreateButtons();          
            GameEvents.Contract.onContractsLoaded.Add(this.onContractLoaded);
            GameEvents.onGameSceneLoadRequested.Add(this.CheckRepairContractTypes);
            GameEvents.OnVesselRollout.Add(this.onvesselRoll);      
            //Debug.Log("MCE Awake");
            getSupplyList(false);
            // create popup dialog and hide it
            popup_dialog = PopupDialog.SpawnPopupDialog(Mainmulti_dialog, true, HighLogic.UISkin, false, "");
            Hide();
        }    
                           
        void OnDestroy()
        {
            DestroyButtons();
            //Debug.Log("MCE OnDestroy");
            GameEvents.Contract.onContractsLoaded.Remove(this.onContractLoaded);
            GameEvents.onGameSceneLoadRequested.Remove(this.CheckRepairContractTypes);
            //Debug.Log("Game All values removed for MCE");
            instance = null;

            SaveInfo.MainGUIWindowPos = new Vector2(
            ((Screen.width / 2) + popup_dialog.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + popup_dialog.RTrf.position.y) / Screen.height);

            SaveInfo.CustomSatWindowPos = new Vector2(
            ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);          

            popup_dialog.Dismiss();
            customSatPop_dialg.Dismiss();
            customCrewPop_Dialg.Dismiss();
            popup_dialog = null;
            customSatPop_dialg = null;
            customCrewPop_Dialg = null;
        }
        private void Update()
        {
            if (!SaveInfo.GUIEnabled)
            {
                Hide();
                return;
            }
            else if (SaveInfo.GUIEnabled && !visible)
            {
                Show();
            }           
        }
        
        private void Allocate()
        {
            if (SaveInfo.MainGUIWindowPos.x <= 0 || SaveInfo.MainGUIWindowPos.y <= 0)
                SaveInfo.MainGUIWindowPos = new Vector2(0.5f, 0.5f);

            Debug_button = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_DebugButton"), delegate
            {
                DebugMenuMce();
                SaveInfo.GUIEnabled = false;
            }, button_width, button_height, false);

            CustomCrew_button = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_CustomCrew"), delegate
            {
                CrewTransferContract();
                SaveInfo.GUIEnabled = false;
            }, button_width, button_height, false);

            CustomSat_button = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_CustomComSat"), delegate
            {
                ComSatContract();
                SaveInfo.GUIEnabled = false;
            }, button_width, button_height, false);
            CustomSupply_button = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_CustomSupply"), delegate
            {
                TransferContract();
                SaveInfo.GUIEnabled = false;
              
            }, button_width, button_height, false);

            Mainmulti_dialog = new MultiOptionDialog(
               "MissionControllerMain",
               "",
               Localizer.Format("#autoLOC_MCE_MCETitle"),
               HighLogic.UISkin,
               new Rect(SaveInfo.MainGUIWindowPos.x, SaveInfo.MainGUIWindowPos.y, width, height),
               new DialogGUIBase[]
               {
                   new DialogGUIVerticalLayout(Debug_button, CustomCrew_button, CustomSat_button, CustomSupply_button),
               });
        }

        public void Show()
        {
            if (popup_dialog != null)
            {
                visible = true;
                popup_dialog.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (popup_dialog != null)
            {
                visible = false;
                popup_dialog.gameObject.SetActive(false);
            }
        }
                 
    }
    public class MissionControllerData : ScenarioModule
    {
        // used TacLife Support by Taranis Elsu way of loading Components for the MissionControllerEC Component 
        private readonly List<Component> mcechildren = new List<Component>();

        public override void OnAwake()
        {

            if (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.EDITOR)
            {
                var c = gameObject.AddComponent<MissionControllerEC>();
                mcechildren.Add(c);
            }
            else { }
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
            MCE_DataStorage mceData = new MCE_DataStorage();
            node.AddNode(mceData.AsConfigNode());
        }
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            MCE_DataStorage mceData = new MCE_DataStorage();
            ConfigNode CN = node.GetNode(mceData.GetType().Name);
            if (CN != null)
                ConfigNode.LoadObjectFromConfig(mceData, CN);
        }

        void OnDestroy()
        {
            foreach (Component c in mcechildren)
            {
                Destroy(c);
            }
            mcechildren.Clear();
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
        [Persistent]internal double savedroverLat = 0;
        [Persistent]internal double savedroverlong = 0;
        [Persistent]internal bool roverislanded = false;
        [Persistent]internal string roversName = "Rover Name";
        [Persistent]internal int roverBody = 6;

        public override void OnDecodeFromConfigNode()
        {           
            SaveInfo.apolloDunaStation = apolloStationStatus;
            SaveInfo.apolloLandingLat = apolldunLat;
            SaveInfo.apolloLandingLon = apolldunLon;
            SaveInfo.tirosCurrentNumber = tirosNumber;
            SaveInfo.marinerCurrentNumber = marinerNumber;
            SaveInfo.apolloCurrentNumber = apolloNumber;
            SaveInfo.apolloDunaCurrentNumber = apolloDunaNumber;          
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
                     
            SaveInfo.SavedRoverLat = savedroverLat;
            SaveInfo.savedRoverLong = savedroverlong;
            SaveInfo.RoverLanded = roverislanded;
            SaveInfo.RoverName = roversName;
            SaveInfo.RoverBody = roverBody;                
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
                    
            savedroverLat = SaveInfo.SavedRoverLat;
            savedroverlong = SaveInfo.savedRoverLong;
            roverislanded = SaveInfo.RoverLanded;
            roversName = SaveInfo.RoverName;
            roverBody = SaveInfo.RoverBody;            
        }
    

    }
}
