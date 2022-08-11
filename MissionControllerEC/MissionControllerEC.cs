
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using KSP.Localization;
using System.Reflection;
using KSP.UI.Screens;
using System.IO;

using ToolbarControl_NS;
using static MissionControllerEC.RegisterToolbar;


namespace MissionControllerEC
{

    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION)]
    public class MissionControllerData : ScenarioModule
    {
        #region ScenarioModule Loading and Saving
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
        #endregion
    }

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class MCEUILoader : MonoBehaviour
    {
        #region Prefab Loading and Handles
        private static GameObject panelPrefab;

        public static GameObject PanelPrefab
        {
            get { return panelPrefab; }
        }
        private static GameObject panelPrefab2;

        public static GameObject PanelPrefab2
        {
            get { return panelPrefab2; }
        }

        private void Awake()
        {
            // Mission Controller First use of Custom UI for Repair Panel.  Thanks to Fengist and Dmagic for their guides on how to do this.
            // I left most of Fengist words below intact to help others if they want to see how I did this.  Minor changes though, but should help.
            // Oh PS sorry for messy code.  Im like a below amatuer coder. :)
            //The way I was doing this which does seem to work.  But DMagic's method makes much more sense.
            //AssetBundle prefabs = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mycoolui.ksp"));
            //DMagic's method without the .ksp file extension            
            AssetBundle prefabs = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../PluginData/mceuipanels.dat"));
            panelPrefab = prefabs.LoadAsset("MCERepairPanel") as GameObject;

            AssetBundle prefabs2 = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../PluginData/mcesatelliteui.dat"));
            panelPrefab2 = prefabs2.LoadAsset("MCESatelliteHub") as GameObject;

            ;
        }
        #endregion
    }

    //[KSPAddon(KSPAddon.Startup.MainMenu, true)]
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public partial class MissionControllerEC : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Types + variables
        public static GameObject RepairUICanvas = null;
        public static GameObject RepairText = null;
        public static List<RepairPanel> RepairUIParts = new List<RepairPanel>();
        private static Vector2 Repairdragstart;
        private static Vector2 Repairaltstart;

        public static GameObject SatelliteUICanvas = null;
        public static GameObject SatFreqText = null;
        public static GameObject SatTypeText = null;
        public static GameObject SatModuleText = null;
        public static GameObject SatGroundLockText = null;

        public static List<MCESatelliteCore> SatelliteUIParts = new List<MCESatelliteCore>();
        private static Vector2 Satellitedragstart;
        private static Vector2 Satellitealtstart;

#if false
        private static Texture2D texture;
        private static Texture2D texture2;

        internal static ApplicationLauncherButton MCEButton;
        internal static ApplicationLauncherButton MCERevert;
#endif
        internal static ToolbarControl toolbarControl_MCEButton;
        internal static ToolbarControl toolbarControl_MCERevert;

        private int id = new System.Random().Next(int.MaxValue);
        public static bool RevertHalt = false;
        // Special thanks to Magico13 Of Kerbal Construction Time for showing me how to Get Scenario Persistance.
        // Some of New GUI Elements learned from Kerbal Forum and piezPiedPy - KSP Trajectories https://github.com/PiezPiedPy/KSPTrajectories/blob/NewGui-Test/Plugin/MainGUI.cs
        // constants        
        private const float width = 400f;
        private const float height = 350f;
        private const float button_width = 390.0f;
        private const float button_height = 50.0f;

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
        private static MultiOptionDialog LandOrbitMulti_Dialg;
        private static MultiOptionDialog BuildSpaceStation_Dialg;

        private static PopupDialog Main_popup_dialog;
        private static PopupDialog customSatPop_dialg;
        private static PopupDialog customSupPop_dialg;
        private static PopupDialog customCrewPop_Dialg;
        private static PopupDialog customLandOrbit_dialg;
        private static PopupDialog BuildSpaceStatPop_dialg;



        private static DialogGUIBase CustomCrew_button;
        private static DialogGUIBase CustomSat_button;
        private static DialogGUIBase BuildBase_button;
        private static DialogGUIBase BuildStation_button;
        private static DialogGUIBase CustomSupply_button;
        private static DialogGUIBase CustomLandOrbit_button;
        private static DialogGUIBase Main_Exit_button;

        private static DialogGUIBase Custom_Contract_Button1;
        private static DialogGUIBase Custom_Contract_Button2;
        private static DialogGUIBase Custom_Contract_Button3;
        private static DialogGUIBase Custom_Contract_Button3a;
        private static DialogGUIBase Custom_Contract_Button4;
        private static DialogGUIBase Custom_Contract_Button5;
        private static DialogGUIBase Custom_Contract_Button6;
        private static DialogGUIBase Custom_Contract_Button7;
        private static DialogGUIBase Custom_Contract_Button8;
        private static DialogGUIBase Custom_Contract_Button9;
        private static DialogGUIBase Custom_Contract_Button10;
        private static DialogGUIBase Custom_Contract_Button10a;
        private static DialogGUIBase Custom_Contract_Button11;
        private static DialogGUIBase Custom_Contract_Button12;
        private static DialogGUIBase Custom_Contract_Button13;

        private static DialogGUIToggleButton Custom_Contract_Toggle2;


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
        private static DialogGUIBox Custom_Contract_GuiBox11;
        private static DialogGUIBox Custom_Contract_GuiBox12;

        private static DialogGUIBase Custom_Contract_Input;
        private static DialogGUIBase Custom_Contract_Input2;

        Settings settings = new Settings("Config.cfg");
#endregion
#region Start/Awake/Instance Stuff
        public static MissionControllerEC Instance
        {
            get
            {
                return instance;
                //return Instance;
            }
        }

        public MissionControllerEC()
        {
            instance = this;
            Allocate();
        }

        void Start()
        {
            Log.Info("MissionControllerEC.Start()");

            if (HighLogic.CurrentGame == null)
                return;
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
            versionCode = assemblyName.Version.Major.ToString() + assemblyName.Version.Minor.ToString();
            loadFiles();
            GameEvents.Contract.onContractsLoaded.Add(this.onContractLoaded);
            getSupplyList(false);
            // create popup dialog and hide it
            if (Mainmulti_dialog == null)
                return;
            Main_popup_dialog = PopupDialog.SpawnPopupDialog(Mainmulti_dialog, true, HighLogic.UISkin, false, "");
            Hide();
            //loadTextures();
            //Debug.LogWarning("[MCE] Textrues called OnAwake");
            MceCreateButtons();
            //Debug.LogWarning("[MCE] Buttons called OnAwake");
            Log.Info("MCE MissionControllerEC Main Awake called");
            //this creates a callback so that whenever the scene is changed we can destroy the UI
            GameEvents.onGameSceneSwitchRequested.Add(OnRepairSceneChange);
            GameEvents.onGameSceneSwitchRequested.Add(OnSatelliteSceneChange);
        }
#endregion
#region GUI Events
        //If we don't get rid of the UI, it'll stay where it is indefinitely.  So, every time the scene is changed, we need to get rid of it.
        void OnRepairSceneChange(GameEvents.FromToAction<GameScenes, GameScenes> fromToScenes)
        {
            if (RepairUICanvas != null)
            {
                RepairUIDestroy();
            }
        }
        void OnSatelliteSceneChange(GameEvents.FromToAction<GameScenes, GameScenes> fromToScenes)
        {
            if (SatelliteUICanvas != null)
                SatelliteUIDestroy();
        }

        //This actually destroys the UI.  But it also goes through the partmodule's toggle buttons and turns them all off.
        public static void RepairUIDestroy()
        {
            RepairUICanvas.DestroyGameObject();
            RepairUICanvas = null;
            foreach (RepairPanel thisPart in RepairUIParts)
            {
                thisPart.openUI = false;
            }
        }
        public static void SatelliteUIDestroy()
        {
            SatelliteUICanvas.DestroyGameObject();
            SatelliteUICanvas = null;
            foreach (MCESatelliteCore thisPart in SatelliteUIParts)
            {
                thisPart.openSatUI = false;
            }
        }
#endregion
#region GuiShow Codes
        public static void RepairShowGUI()
        {
            if (RepairUICanvas != null)  //if the UI is already showing, don't show another one.
                return;

            //Load up the UI and show it
            RepairUICanvas = (GameObject)Instantiate(MCEUILoader.PanelPrefab);
            RepairUICanvas.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
            RepairUICanvas.AddComponent<MissionControllerEC>();

            //Find the game objects that we gave cool names to in Unity
            RepairText = (GameObject)GameObject.Find("RepairText");

            //This is a button so we need to create a callback for when it gets clicked on
            GameObject RepaircheckToggle = (GameObject)GameObject.Find("TestButton");
            Button RepairtoggleButton = RepaircheckToggle.GetComponent<Button>();
            RepairtoggleButton.onClick.AddListener(RepairOnToggleClicked);

            GameObject RepairButton2 = (GameObject)GameObject.Find("EnterButton");
            Button RepairToggleButton2 = RepairButton2.GetComponent<Button>();
            RepairToggleButton2.onClick.AddListener(RepairButton2Clicked);

            GameObject RepExitButton2 = (GameObject)GameObject.Find("SatExitButton");
            Button RepExitToggleButton2 = RepExitButton2.GetComponent<Button>();
            RepExitToggleButton2.onClick.AddListener(RepExitButton2Clicked);

            ConfigNode node = new ConfigNode();
            var thisnode = RepGetConfig("RepairUI");
            float xpos = float.Parse(thisnode.GetValue("x"));
            float ypos = float.Parse(thisnode.GetValue("y"));
            RepairUICanvas.transform.position = new Vector3(xpos, ypos, RepairUICanvas.transform.position.z);
        }

        public static void SatelliteShowGUI()
        {
            if (SatelliteUICanvas != null)  //if the UI is already showing, don't show another one.
                return;

            //Load up the UI and show it
            SatelliteUICanvas = (GameObject)Instantiate(MCEUILoader.PanelPrefab2);
            SatelliteUICanvas.transform.SetParent(MainCanvasUtil.MainCanvas.transform);
            SatelliteUICanvas.AddComponent<MissionControllerEC>();

            //Find the game objects that we gave cool names to in Unity
            //RepairText = (GameObject)GameObject.Find("RepairText");
            SatFreqText = (GameObject)GameObject.Find("FreqTextHead");
            SatTypeText = (GameObject)GameObject.Find("MceSatTypeText");
            SatModuleText = (GameObject)GameObject.Find("MceSatModulText");
            SatGroundLockText = (GameObject)GameObject.Find("MceGroundStationText");


            GameObject SatFreqButton1 = (GameObject)GameObject.Find("FreqFowardButton");
            Button SatToggleButton1 = SatFreqButton1.GetComponent<Button>();
            SatToggleButton1.onClick.AddListener(SatFreqButtonClicked);

            GameObject SatFreqButton2 = (GameObject)GameObject.Find("FreqFowardButton2");
            Button SatToggleButton2 = SatFreqButton2.GetComponent<Button>();
            SatToggleButton2.onClick.AddListener(SatFreqButtonClicked2);

            GameObject SatFreqButton3 = (GameObject)GameObject.Find("FreqBackButton");
            Button SatToggleButton3 = SatFreqButton3.GetComponent<Button>();
            SatToggleButton3.onClick.AddListener(SatFreqButtonClicked3);

            GameObject SatFreqButton4 = (GameObject)GameObject.Find("FreqBackButton2");
            Button SatToggleButton4 = SatFreqButton4.GetComponent<Button>();
            SatToggleButton4.onClick.AddListener(SatFreqButtonClicked4);

            GameObject SatTypeButton1 = (GameObject)GameObject.Find("SatTypeFowardButton");
            Button SatTypeToggleButton1 = SatTypeButton1.GetComponent<Button>();
            SatTypeToggleButton1.onClick.AddListener(SatTypeButtonClicked1);

            GameObject SatTypeButton2 = (GameObject)GameObject.Find("SatTypeBackButton");
            Button SatTypeToggleButton2 = SatTypeButton2.GetComponent<Button>();
            SatTypeToggleButton2.onClick.AddListener(SatTypeButtonClicked2);

            GameObject SatModButton1 = (GameObject)GameObject.Find("ModuleTypeFowardButton");
            Button SatModToggleButton1 = SatModButton1.GetComponent<Button>();
            SatModToggleButton1.onClick.AddListener(SatModButtonClicked1);

            GameObject SatModButton2 = (GameObject)GameObject.Find("ModuleTypeBackButton");
            Button SatModToggleButton2 = SatModButton2.GetComponent<Button>();
            SatModToggleButton2.onClick.AddListener(SatModButtonClicked2);

            GameObject SatTransmitButton2 = (GameObject)GameObject.Find("TransmitKeyButton");
            Button SatTransmitToggleButton2 = SatTransmitButton2.GetComponent<Button>();
            SatTransmitToggleButton2.onClick.AddListener(SattransmitButtonClicked);

            GameObject SatExitButton2 = (GameObject)GameObject.Find("SatExitButton");
            Button SatExitToggleButton2 = SatExitButton2.GetComponent<Button>();
            SatExitToggleButton2.onClick.AddListener(SatExitButton2Clicked);

            ConfigNode node = new ConfigNode();
            var thisnode = SatGetConfig("SatelliteUI");
            float xpos = float.Parse(thisnode.GetValue("x"));
            float ypos = float.Parse(thisnode.GetValue("y"));
            SatelliteUICanvas.transform.position = new Vector3(xpos, ypos, SatelliteUICanvas.transform.position.z);
        }
#endregion
#region Gui buttons
        //this is the callback we created for when the toggle button is clicked.
        static void RepairOnToggleClicked()
        {
            RepairPanel rp = new RepairPanel();
            rp.CheckSystems();
            ScreenMessages.PostScreenMessage("You Pressed The Test Button");
        }
        static void RepairButton2Clicked()
        {
            RepairPanel rp = new RepairPanel();
            rp.EnableRepair();
            ScreenMessages.PostScreenMessage("You Pressed The Enter Button");
        }
        static void SatFreqButtonClicked()
        {
            float a = MCESatelliteCore.frequencyModulation;
            float b = .50f;
            float final = a + b;
            MCESatelliteCore.frequencyModulation = final;
            if (MCESatelliteCore.frequencyModulation > 50) { MCESatelliteCore.frequencyModulation = 50; }
        }
        static void SatFreqButtonClicked2()
        {
            MCESatelliteCore.frequencyModulation++;
            if (MCESatelliteCore.frequencyModulation > 50) { MCESatelliteCore.frequencyModulation = 50; }
        }

        static void SatFreqButtonClicked3()
        {
            float a = MCESatelliteCore.frequencyModulation;
            float b = .50f;
            float final = a - b;
            MCESatelliteCore.frequencyModulation = final;
            if (MCESatelliteCore.frequencyModulation < 1) { MCESatelliteCore.frequencyModulation = 1; }
        }
        static void SatFreqButtonClicked4()
        {
            MCESatelliteCore.frequencyModulation--;
            if (MCESatelliteCore.frequencyModulation < 1) { MCESatelliteCore.frequencyModulation = 1; }
        }
        static void SatTypeButtonClicked1()
        {
            if (!MCESatelliteCore.ControlsLocked)
            {
                MCESatelliteCore.sattypenumber++;
                if (MCESatelliteCore.sattypenumber > 3) { MCESatelliteCore.sattypenumber = 3; }
            }
            else { ScreenMessages.PostScreenMessage("Vessel Has Launced, Controls For Satellite Type And Module Type Are Now Locked!", 15f); }
        }
        static void SatTypeButtonClicked2()
        {
            if (!MCESatelliteCore.ControlsLocked)
            {
                MCESatelliteCore.sattypenumber--;
                if (MCESatelliteCore.sattypenumber < 0) { MCESatelliteCore.sattypenumber = 0; }
            }
            else { ScreenMessages.PostScreenMessage("Vessel Has Launced, Controls For Satellite Type And Module Type Are Now Locked!", 15f); }
        }
        static void SatModButtonClicked1()
        {
            if (!MCESatelliteCore.ControlsLocked)
            {
                MCESatelliteCore.moduleTypeChange++;
                if (MCESatelliteCore.moduleTypeChange > 4) { MCESatelliteCore.moduleTypeChange = 4; }
            }
            else { ScreenMessages.PostScreenMessage("Vessel Has Launced, Controls For Satellite Type And Module Type Are Now Locked!", 15f); }
        }
        static void SatModButtonClicked2()
        {
            if (!MCESatelliteCore.ControlsLocked)
            {
                MCESatelliteCore.moduleTypeChange--;
                if (MCESatelliteCore.moduleTypeChange < 0) { MCESatelliteCore.moduleTypeChange = 0; }
            }
            else { ScreenMessages.PostScreenMessage("Vessel Has Launced, Controls For Satellite Type And Module Type Are Now Locked!", 15f); }
        }
        static void SattransmitButtonClicked()
        {
            Log.Info("Hey You Pressed Transmit!");
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                MCESatelliteCore.StartDataTransfer = true;
            }
            else
            { ScreenMessages.PostScreenMessage("Not in flight Or Orbit", 10f); Log.Info("You Pressed Transmit Not In Flight"); }
        }
        static void RepExitButton2Clicked()
        {
            RepairUIDestroy();
        }
        static void SatExitButton2Clicked()
        {
            SatelliteUIDestroy();
        }

        //this function is where we update the text component on the UI.
        public static void RepairUpdateText(string message)
        {
            RepairText.GetComponent<Text>().text = message;
        }
        public static void SatFreqUpdateText(string message)
        {
            SatFreqText.GetComponent<Text>().text = message;
        }
        public static void SatTypeUpdateText(string message)
        {
            SatTypeText.GetComponent<Text>().text = message;
        }
        public static void SatModTypeUpdateText(string message)
        {
            SatModuleText.GetComponent<Text>().text = message;
        }
        public static void SatGroundLockUpdateText(string message)
        {
            SatGroundLockText.GetComponent<Text>().text = message;
        }
#endregion
#region GUI Drag Values
        //this event fires when a drag event begins
        public void OnBeginDrag(PointerEventData data)
        {
            if (RepairUICanvas != null)
            {
                Repairdragstart = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
                Repairaltstart = RepairUICanvas.transform.position;
            }
            if (SatelliteUICanvas != null)
            {
                Satellitedragstart = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
                Satellitealtstart = SatelliteUICanvas.transform.position;
            }

        }

        //this event fires while we're dragging. It's constantly moving the UI to a new position
        public void OnDrag(PointerEventData data)
        {
            if (RepairUICanvas != null)
            {
                Vector2 dpos = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
                Vector2 dragdist = dpos - Repairdragstart;
                RepairUICanvas.transform.position = Repairaltstart + dragdist;
            }
            if (SatelliteUICanvas != null)
            {
                Vector2 satdpos = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
                Vector2 satdragdist = satdpos - Satellitedragstart;
                SatelliteUICanvas.transform.position = Satellitealtstart + satdragdist;
            }

        }

        //this event fires when we let go of the mouse and stop dragging
        public void OnEndDrag(PointerEventData data)
        {
            if (RepairUICanvas != null)
            {
                RepSetConfig(RepairUICanvas.transform.position.x, RepairUICanvas.transform.position.y, "RepairUI");
                Log.Info("Saving RepairUI Position");
            }

            if (SatelliteUICanvas != null)
            {
                SatSetConfig(SatelliteUICanvas.transform.position.x, SatelliteUICanvas.transform.position.y, "SatelliteUI");
                Log.Info("Saving SatelliteUI Position");
            }
        }

        //This function grabs the position of the UI slider
        public static float RepairSliderPosition()
        {
            GameObject slider = (GameObject)GameObject.Find("YouMoveMe");
            Slider thisSlider = slider.GetComponent<Slider>();
            return thisSlider.value;
        }
        public static float RepairCoolSliderPosition2()
        {
            GameObject slider = (GameObject)GameObject.Find("YouMoveMe2");
            Slider thisSlider = slider.GetComponent<Slider>();
            return thisSlider.value;
        }
        public static float RepairCoolSliderPosition3()
        {
            GameObject slider = (GameObject)GameObject.Find("YouMoveMe3");
            Slider thisSlider = slider.GetComponent<Slider>();
            return thisSlider.value;
        }

#endregion
#region Old GUI Code Working On Replacing
        void OnDestroy()
        {
            Log.Info("MissionControllerEC.OnDestroy");
            if (Main_popup_dialog != null)
            {
                SaveInfo.MainGUIWindowPos = new Vector2(
                ((Screen.width / 2) + Main_popup_dialog.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + Main_popup_dialog.RTrf.position.y) / Screen.height);
            }

#if false
            if (MCEButton != null && HighLogic.LoadedScene != GameScenes.SPACECENTER)
            {
                ApplicationLauncher.Instance.RemoveModApplication(MCEButton);
                Log.Error("[MCE] Button OnDestroyed called");
            }
            if (MCERevert != null && !HighLogic.LoadedSceneIsFlight)
            {
                ApplicationLauncher.Instance.RemoveModApplication(MCERevert);
                Log.Error("[MCE] Revert OnDestroyed called");
            }
#endif

            GameEvents.Contract.onContractsLoaded.Remove(this.onContractLoaded);
            instance = null;
            Log.Info("MCE MissioniControllerEC Main OnDestroy Called");

            try
            {
                Main_popup_dialog.Dismiss();
            }
            catch { Log.Info("MCE Main_Popup_dialog already loaded"); }

            Main_popup_dialog = null;
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

            CustomSat_button = new DialogGUIButton(Localizer.Format("Send Satellite To Space"), delegate
            {
                ComSatContract();
                SaveInfo.GUIEnabled = false;
            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);

            CustomLandOrbit_button = new DialogGUIButton(Localizer.Format("Send A Crew To Orbit Or Landing"), delegate
            {
                LandingOrbitCustomContract();
                SaveInfo.GUIEnabled = false;
            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);

            CustomCrew_button = new DialogGUIButton(Localizer.Format("Launch Crew To A Station"), delegate
            {
                CrewTransferContract();
                SaveInfo.GUIEnabled = false;
            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);

            CustomSupply_button = new DialogGUIButton(Localizer.Format("Resupply Resources To A Station"), delegate
            {
                TransferContract();
                SaveInfo.GUIEnabled = false;

            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);
            BuildStation_button = new DialogGUIButton(Localizer.Format("Build A Space Station"), delegate
            {
                BuildSpaceStation()
;
                SaveInfo.GUIEnabled = false;
            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);
            BuildBase_button = new DialogGUIButton(Localizer.Format("WithDraw Contract Offers (ALL) Clears Current Contracts"), delegate
            {
                SaveInfo.OrbitLandingOn = false;
                SaveInfo.BuildSpaceStationOn = false;
                SaveInfo.ComSateContractOn = false;
                SaveInfo.crewContractOn = false;
                SaveInfo.supplyContractOn = false;
                SaveInfo.GUIEnabled = false;
            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);

            Main_Exit_button = new DialogGUIButton(Localizer.Format("Exit Finance Contract Builder"), delegate
            {
                SaveInfo.GUIEnabled = false;
                //MCEButton.SetFalse();
                toolbarControl_MCEButton.SetFalse();
                onContractLoaded();
            }, delegate { return true; }, button_width, button_height, false, MCEGuiElements.ButtonMenuMainSyle);

            Mainmulti_dialog = new MultiOptionDialog(
               "MissionControllerMain",
               "",
               "Finance Contract Builder Menu",
               MCEGuiElements.MissionControllerSkin,
               new Rect(SaveInfo.MainGUIWindowPos.x, SaveInfo.MainGUIWindowPos.y, width, height),
               new DialogGUIBase[]
               {
                   new DialogGUIVerticalLayout(CustomSat_button,CustomLandOrbit_button ,CustomCrew_button ,CustomSupply_button,BuildStation_button,BuildBase_button, new DialogGUISpace(5),
                    Main_Exit_button), new DialogGUISpace(12)
               });
        }

        public void Show()
        {
            if (Main_popup_dialog != null)
            {
                visible = true;
                Main_popup_dialog.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (Main_popup_dialog != null)
            {
                visible = false;
                Main_popup_dialog.gameObject.SetActive(false);
            }
        }
#endregion
#region Gui Config Save/Load
        static ConfigNode RepGetConfig(string NodeText)
        {
            string CurrentNode = NodeText;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../PluginData/MceGUILocation.dat");
            ConfigNode result = ConfigNode.Load(filePath).GetNode(NodeText);
            return result;
        }
        static ConfigNode SatGetConfig(string NodeText)
        {
            string CurrentNode = NodeText;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../PluginData/MceGUILocation2.dat");
            ConfigNode result = ConfigNode.Load(filePath).GetNode(NodeText);
            return result;
        }

        private static void RepSetConfig(float x, float y, string NodeText)
        {
            string CurrentNode = NodeText;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../PluginData/MceGUILocation.dat");
            ConfigNode thiscfg = new ConfigNode();
            var thisnode = thiscfg.AddNode(CurrentNode);
            thisnode.AddValue("x", x);
            thisnode.AddValue("y", y);
            thiscfg.Save(filePath);
        }

        private static void SatSetConfig(float x, float y, string NodeText)
        {
            string CurrentNode = NodeText;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../PluginData/MceGUILocation2.dat");
            ConfigNode thiscfg = new ConfigNode();
            var thisnode = thiscfg.AddNode(CurrentNode);
            thisnode.AddValue("x", x);
            thisnode.AddValue("y", y);
            thiscfg.Save(filePath);
        }
#endregion
    }

    public class RepairPanel : PartModule
    {
#region variables
        [KSPField]
        string DoorAnimation = "mceanim";

        [KSPField(isPersistant = false)]
        public static bool repair = false;

        [KSPField(isPersistant = true)]
        public double currentRepair = 1;

        [KSPField(isPersistant = true)]
        public static string vesselId = "Test";

        [KSPField(isPersistant = true)]
        public static string vesselName = "TestName";

        [KSPField(isPersistant = false)]
        public double repairRate = 1;

        public bool startrepair = false;

        public bool openUI = false;

        private bool RepairGamewin = false, GameSlider1 = false, GameSlider2 = false, GameSlider3 = false;
        private double GameSwitchChange;
        public Animation GetDeployDoorAnim
        {
            get
            {
                return part.FindModelAnimators(DoorAnimation)[0];
            }
        }

        private void PlayOpenAnimation(int speed, float time)
        {
            print("Opening");
            GetDeployDoorAnim[DoorAnimation].speed = speed;
            GetDeployDoorAnim[DoorAnimation].normalizedTime = time;
            GetDeployDoorAnim.Play(DoorAnimation);
        }

        [KSPField(guiName = "Battery Coolant", guiActiveEditor = false, guiActive = false, isPersistant = true),
        UI_ProgressBar(minValue = 0f, maxValue = 1f, scene = UI_Scene.All)]
        public float PartUICoolSlider = 0.0f;

        [KSPField(guiName = "Air Pressure Purge", guiActiveEditor = false, guiActive = false, isPersistant = true),
        UI_ProgressBar(minValue = 0f, maxValue = 1f, scene = UI_Scene.All)]
        public float PartUICoolSlider2 = 0.0f;

        [KSPField(guiName = "HMI Reset", guiActiveEditor = false, guiActive = false, isPersistant = true),
        UI_ProgressBar(minValue = 0f, maxValue = 1f, scene = UI_Scene.All)]
        public float PartUICoolSlider3 = 0.0f;

        [KSPField(isPersistant = true, guiActive = true, guiName = "Ready To Repair")]
        public static bool readyRep = false;

#endregion
#region Methods

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiIcon = "Engineer CheckSystems", guiName = "Engineer CheckSystems", active = false)]
        public void CheckSystems()
        {

            List<ProtoCrewMember> protoCrewMembers = FlightGlobals.ActiveVessel.GetVesselCrew();
            foreach (Experience.ExperienceEffect exp in protoCrewMembers[0].experienceTrait.Effects)
            {
                if (exp.ToString() == "Experience.Effects.RepairSkill")
                {
                    Log.Info("Current kerbal is a Engineer you have passed");
                    readyRep = true;
                    Log.Error("Vessel Id For PartModule is " + vesselId + " Name is " + vesselName);
                    ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000250"), 15f);          // #autoLOC_MissionController2_1000250 = Your engineer has Prepared the vessel for Repair Open the panel, Then conduct the repair
                    if (GameSlider2)
                        MissionControllerEC.RepairUpdateText("Purge Air System");
                    if (GameSlider1)
                        MissionControllerEC.RepairUpdateText("Purge Battery Coolant");
                    if (GameSlider3)
                        MissionControllerEC.RepairUpdateText("Reset The HMI");
                }
                else
                {
                    Log.Info("Current kerbal is NOT an Engineer you don't pass... Bad boy!");
                    ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000251"), 15f);      // #autoLOC_MissionController2_1000251 = You need an Engineer to fix this Vessel!
                }
            }
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Start Repairs", active = false)]
        public void EnableRepair()
        {
            if (readyRep && currentRepair > 0)
            {
                repair = true;
                Log.Info("repairEnabled");
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000252"), 5f);		// #autoLOC_MissionController2_1000252 = Your engineer has repaired this vessel.  Good job!
                readyRep = false;
                MissionControllerEC.RepairUpdateText("Repair Complete!!");
            }
            else { ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000253"), 5f); }		// #autoLOC_MissionController2_1000253 = You need an Engineer class kerbal to conduct this repair!
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Open Door", active = true, guiActiveEditor = false)]
        public void OpenDoor()
        {
            vesselId = this.part.vessel.id.ToString();
            Log.Info(vesselId.ToString());
            vesselName = this.part.vessel.name;
            Log.Info(vesselName.ToString());
            Log.Info("Vessel Name and ID Got Past This point");
            Events["OpenDoor"].active = false;
            Events["EnableRepair"].active = true;
            Events["closeDoor"].active = true;
            Events["CheckSystems"].active = true;
            if (MissionControllerEC.RepairUICanvas == null)
            {
                MissionControllerEC.RepairShowGUI();
                PlayOpenAnimation(1, 0);
                openUI = true;
                MissionControllerEC.RepairUpdateText("Press Test Button");
            }
            else
            {
                MissionControllerEC.RepairUIDestroy();
                PlayOpenAnimation(-1, 1);
                openUI = false;
                MissionControllerEC.RepairUpdateText("Press Test Button");
            }
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Close Door", active = false, guiActiveEditor = false)]
        public void closeDoor()
        {
            Events["OpenDoor"].active = true;
            Events["EnableRepair"].active = false;
            Events["closeDoor"].active = false;
            Events["CheckSystems"].active = false;
            if (MissionControllerEC.RepairUICanvas == null && RepairGamewin)
            {
                MissionControllerEC.RepairShowGUI();
                PlayOpenAnimation(1, 0);
                openUI = true;
                PartUICoolSlider2 = 0;
                MissionControllerEC.RepairUpdateText("Press Test Button.");
            }
            else
            {
                MissionControllerEC.RepairUIDestroy();
                PlayOpenAnimation(-1, 1);
                openUI = false;
                MissionControllerEC.RepairUpdateText("Press Test Button..");
            }
        }

        private void GameSwitch()
        {
            switch (GameSwitchChange)
            {
                case 1:
                    GameSlider1 = true;
                    GameSlider2 = false;
                    GameSlider3 = false;
                    break;
                case 2:
                    GameSlider2 = true;
                    GameSlider1 = false;
                    GameSlider3 = false;
                    break;
                case 3:
                    GameSlider3 = true;
                    GameSlider2 = false;
                    GameSlider1 = false;
                    break;
                default:
                    GameSlider1 = false;
                    GameSlider2 = false;
                    GameSlider3 = false;
                    break;

            }
        }

#endregion
#region OnStart + Fixed Stuff
        public override void OnStart(PartModule.StartState state)
        {
            this.part.force_activate();
            GameSwitchChange = Tools.RandomNumber(0, 2);
            GameSwitch();
        }

        public void FixedUpdate()
        {
            if (MissionControllerEC.RepairUICanvas != null)
            {
                Events["OpenDoor"].active = false;
                Events["closeDoor"].active = true;
            }
            else
            {
                Events["OpenDoor"].active = true;
                Events["closeDoor"].active = false;
                return;
            }

            PartUICoolSlider = MissionControllerEC.RepairSliderPosition();
            PartUICoolSlider2 = MissionControllerEC.RepairCoolSliderPosition2();
            PartUICoolSlider3 = MissionControllerEC.RepairCoolSliderPosition3();

            if (PartUICoolSlider == 1f && GameSlider1)
            {
                MissionControllerEC.RepairUpdateText("Press Enter");
                RepairGamewin = true;
            }
            if (PartUICoolSlider2 == 1f && GameSlider2)
            {
                MissionControllerEC.RepairUpdateText("Press Enter");
                RepairGamewin = true;
            }
            if (PartUICoolSlider3 == 1f && GameSlider3)
            {
                MissionControllerEC.RepairUpdateText("Press Enter");
                RepairGamewin = true;
            }
            else { RepairGamewin = false; }
        }
#endregion
    }

    public class MCESatelliteCore : PartModule
    {
#region variables
        [KSPField(isPersistant = true, guiActive = true)]
        public static string[] SattypeList = { "Communication", "Navigation", "Weather", "Research" };

        [KSPField(isPersistant = true, guiActive = false)]
        public static int sattypenumber = 0;

        [KSPField(isPersistant = true, guiActive = true, guiName = "MC Data Locked")]
        private bool dataLocked = false;

        public bool haveAnimation = false;
        public string animationName = "None";
        public static bool StartDataTransfer = false;
        public static string GroundStationLockedText = "Free Roam";

        public static bool ControlsLocked = false;

        public Animation GetSatelliteCoreAnimation
        {
            get
            {
                return part.FindModelAnimators(animationName)[0];
            }
        }

        private void PlaySatelliteCoreAnimation(int speed, float time)
        {
            Log.Info("Running animation for MCESatelliteCore");
            GetSatelliteCoreAnimation[animationName].speed = speed;
            GetSatelliteCoreAnimation[animationName].normalizedTime = time;
            GetSatelliteCoreAnimation.Play(animationName);
        }

        [KSPField(guiName = "MC Satellite Panel", guiActiveEditor = true, guiActive = true, isPersistant = false),
        UI_Toggle(controlEnabled = true, disabledText = "Closed", enabledText = "Open", invertButton = false, scene = UI_Scene.All)]
        public bool openSatUI = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = " MC Satellite Type: ")]
        public string satTypeDisplay = SattypeList[sattypenumber];

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "MC Module Type: ")]
        public string satModuleType = "Select Module Type";

        [KSPField(isPersistant = true, guiActive = false)]
        public static float frequencyModulation = 1;

        [KSPField(isPersistant = true, guiActive = false)]
        public static float moduleTypeChange = 1;
#endregion
#region Methods And Switches

        public void StartDataMCE()
        {
            if (!dataLocked && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000241"), 10f);		// #autoLOC_MissionController2_1000241 = Sending Data Package, This Part core is now disabled and can't be used again
                dataStartup();
                if (haveAnimation)
                {
                    PlaySatelliteCoreAnimation(1, 0);
                    PlaySatelliteCoreAnimation(-1, 1);

                }
            }
            else if (dataLocked)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000242"), 10f);		// #autoLOC_MissionController2_1000242 = This data package has already been sent.  Only 1 data package can be activated per Module
                StartDataTransfer = false;
            }
            else
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000243"), 10f);		// #autoLOC_MissionController2_1000243 = You have to be in orbit to Do a Data Startup
                StartDataTransfer = false;
            }
        }

        public void dataStartup()
        {
            MCEParameters.satelliteCoreCheck sc = new MCEParameters.satelliteCoreCheck(false);
            sc.SetBoolSatelliteCoreValue(true);
            sc.SetSatelliteCoreCheck(satTypeDisplay, moduleTypeChange, frequencyModulation);
            dataLocked = true;
            StartDataTransfer = false;
            GroundStationLockedText = "Free Roam";
            MissionControllerEC.SatelliteUIDestroy();
        }
        private void ModuleTypeSwitch()
        {
            switch ((int)moduleTypeChange)
            {
                case 1:
                    satModuleType = "Tv Networks (1)";
                    break;
                case 2:
                    satModuleType = "Phone Systems (2)";
                    break;
                case 3:
                    satModuleType = "Goverment(3)";
                    break;
                case 4:
                    satModuleType = "Deep Space (4)";
                    break;
                default:
                    satModuleType = "Tv Networks (1)";
                    break;
            }
        }
        private void ModuleTypeSwitch2()
        {
            switch ((int)moduleTypeChange)
            {
                case 1:
                    satModuleType = "Civilian GPS(1)";
                    break;
                case 2:
                    satModuleType = "Goverment GPS(2)";
                    break;
                case 3:
                    satModuleType = "Maritime GPS(3)";
                    break;
                case 4:
                    satModuleType = "UAV Operations(4)";
                    break;
                default:
                    satModuleType = "Corperate (4)";
                    break;
            }
        }
        private void ModuleTypeSwitch3()
        {
            switch ((int)moduleTypeChange)
            {
                case 1:
                    satModuleType = "Visible spectrum(1)";
                    break;
                case 2:
                    satModuleType = "Infrared spectrum(2)";
                    break;
                case 3:
                    satModuleType = "Radiation(3)";
                    break;
                case 4:
                    satModuleType = "Radar(4)";
                    break;
                default:
                    satModuleType = "Atmospheric Sounder(4)";
                    break;
            }
        }
        private void ModuleTypeSwitch4()
        {
            switch ((int)moduleTypeChange)
            {
                case 1:
                    satModuleType = "Atmoshperic Studies(1)";
                    break;
                case 2:
                    satModuleType = "Radiation Studies(2)";
                    break;
                case 3:
                    satModuleType = "Zero-G Studies(3)";
                    break;
                case 4:
                    satModuleType = "Bacterial(4)";
                    break;
                default:
                    satModuleType = "Live Specimen(4)";
                    break;
            }
        }


#endregion
#region Onstart + Fixed
        public override void OnStart(PartModule.StartState state)
        {
            if (!dataLocked)
            {
                Fields[nameof(openSatUI)].uiControlEditor.onFieldChanged = delegate (BaseField a, System.Object b)
                {
                    if (MissionControllerEC.SatelliteUICanvas == null)
                    {
                        MissionControllerEC.SatelliteShowGUI(); //if the UI doesn't exist, create one and show it.
                    }
                    else
                    {
                        MissionControllerEC.SatelliteUIDestroy(); //if it does exist. they're closing it so get rid of it.
                    }
                };
                Fields[nameof(openSatUI)].uiControlFlight.onFieldChanged = delegate (BaseField a, System.Object b)
                {
                    if (MissionControllerEC.SatelliteUICanvas == null)
                    {
                        MissionControllerEC.SatelliteShowGUI(); //if the UI doesn't exist, create one and show it.
                    }
                    else
                    {
                        MissionControllerEC.SatelliteUIDestroy(); //if it does exist. they're closing it so get rid of it.
                    }
                };
            }
            else { ScreenMessages.PostScreenMessage("You Are Locked Out Of This System.  Mission Controlled Locked", 10f); }
        }
        public void FixedUpdate()
        {
            if (MissionControllerEC.SatelliteUICanvas != null && !dataLocked)
            { openSatUI = true; }
            else
            { openSatUI = false; return; }
            if (HighLogic.LoadedSceneIsEditor || FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH)
            {
                if (openSatUI && !dataLocked && MissionControllerEC.SatelliteUICanvas != null)
                {
                    MissionControllerEC.SatTypeUpdateText(satTypeDisplay.ToString());
                    MissionControllerEC.SatModTypeUpdateText(satModuleType.ToString());
                    MissionControllerEC.SatGroundLockUpdateText(GroundStationLockedText);
                    MCEParameters.GroundStationPostion gs = new MCEParameters.GroundStationPostion(0);
                    if (satTypeDisplay == "Communication")
                    {
                        ModuleTypeSwitch();
                    }
                    else if (satTypeDisplay == "Navigation")
                    {
                        ModuleTypeSwitch2();
                    }
                    else if (satTypeDisplay == "Weather")
                    {
                        ModuleTypeSwitch3();
                    }
                    else if (satTypeDisplay == "Research")
                    {
                        ModuleTypeSwitch4();
                    }
                    satTypeDisplay = SattypeList[sattypenumber];
                    gs.SetGroundStationCheck(frequencyModulation);
                }
                ControlsLocked = true;
            }
            ControlsLocked = false;
            MissionControllerEC.SatFreqUpdateText(frequencyModulation.ToString());
            if (StartDataTransfer) { StartDataMCE(); }
        }
#endregion
    }

    public class MCE_DataStorage : ConfigNodeStorage
    {
        [Persistent] public bool ComSatOn = false;

        [Persistent] public string supplyVesselName = "None";
        [Persistent] public string supplyVesselId = "none";
        [Persistent] public string supplyResource = "none";
        [Persistent] public string supplyContractName = "none";
        [Persistent] public int supplybodyIDX;
        [Persistent] public bool supplyContractOn = false;
        [Persistent] public double supplyResAmount = 0;

        [Persistent] public int LandingOrbitCrew;
        [Persistent] public string LandingOrbitDescription = "Place Title Here";
        [Persistent] public int LandingOrbitIDX;
        [Persistent] public string LandingOrbitName = "Title Here";
        [Persistent] public bool isOrbitLanding = false;
        [Persistent] public bool OrbitLandingOn = false;
        [Persistent] public bool OrbitCiviliansOn = false;
        [Persistent] public int LandingOrbitCivilians = 0;

        [Persistent] public string crewtransfername = "crew transfer";
        [Persistent] public string crewvesname = "none";
        [Persistent] public string crewvesId = "none";
        [Persistent] public int crewamount = 0;
        [Persistent] public double crewtime = 0;
        [Persistent] public int crewbodyIDX = 0;
        [Persistent] public bool crewcontracton = false;
        [Persistent] public bool crewCiviliansOn = false;
        [Persistent] public string transfercrewDesc = "Place Description Here";
        [Persistent] public int transferTouristAmount = 0;

        [Persistent] public bool noOrbitalContract = false;
        [Persistent] public bool noLandingContract = false;
        [Persistent] public bool noSatelliteContract = false;
        [Persistent] public bool noRepairContract = false;
        [Persistent] public bool noOrbitalPeriodContract = false;
        [Persistent] public bool noHistoricContracts = false;
        [Persistent] public bool noCivilianContracts = false;

        [Persistent] internal bool com_Sat_Start_Building = false;
        [Persistent] internal double com_Sat_maxOrbP = 70000;
        [Persistent] internal double com_Sat_minOrbP = 0;
        [Persistent] internal string com_Sat_contractName = "Deliever COMSAT Satellite";
        [Persistent] internal int com_Sat_bodyNumber = 1;
        [Persistent] internal bool hardcoreOn = true;

        [Persistent] internal double savedroverLat = 0;
        [Persistent] internal double savedroverlong = 0;
        [Persistent] internal bool roverislanded = false;
        [Persistent] internal string roversName = "Rover Name";
        [Persistent] internal int roverBody = 6;

        [Persistent] internal string satConDescript = "Place Contract Description Here";
        [Persistent] internal string ResourceTransferConDescript = "Place Contract Description Here";

        public override void OnDecodeFromConfigNode()
        {
            SaveInfo.SatelliteConDesc = satConDescript;
            SaveInfo.ResourceTransferConDesc = ResourceTransferConDescript;
            SaveInfo.SatContractReady = ComSatOn;
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
            SaveInfo.TransferCrewDesc = transfercrewDesc;
            SaveInfo.transferTouristTrue = crewCiviliansOn;
            SaveInfo.transferTouristAmount = transferTouristAmount;

            SaveInfo.SupplyVesName = supplyVesselName;
            SaveInfo.SupplyVesId = supplyVesselId;
            SaveInfo.ResourceName = supplyResource;
            SaveInfo.SupplyContractName = supplyContractName;
            SaveInfo.SupplyBodyIDX = supplybodyIDX;
            SaveInfo.supplyContractOn = supplyContractOn;
            SaveInfo.supplyAmount = supplyResAmount;

            SaveInfo.LandingOrbitCrew = LandingOrbitCrew;
            SaveInfo.LandingOrbitDesc = LandingOrbitDescription;
            SaveInfo.LandingOrbitIDX = LandingOrbitIDX;
            SaveInfo.LandingOrbitName = LandingOrbitName;
            SaveInfo.IsOrbitOrLanding = isOrbitLanding;
            SaveInfo.OrbitLandingOn = OrbitLandingOn;
            SaveInfo.OrbitAllowCivs = OrbitCiviliansOn;
            SaveInfo.LandingOrbitCivilians = LandingOrbitCivilians;

            SaveInfo.SavedRoverLat = savedroverLat;
            SaveInfo.savedRoverLong = savedroverlong;
            SaveInfo.RoverLanded = roverislanded;
            SaveInfo.RoverName = roversName;
            SaveInfo.RoverBody = roverBody;
        }

        public override void OnEncodeToConfigNode()
        {
            satConDescript = SaveInfo.SatelliteConDesc;
            ResourceTransferConDescript = SaveInfo.ResourceTransferConDesc;
            ComSatOn = SaveInfo.SatContractReady;

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
            crewCiviliansOn = SaveInfo.transferTouristTrue;
            transfercrewDesc = SaveInfo.TransferCrewDesc;
            transferTouristAmount = SaveInfo.transferTouristAmount;


            supplyVesselName = SaveInfo.SupplyVesName;
            supplyVesselId = SaveInfo.SupplyVesId;
            supplyResource = SaveInfo.ResourceName;
            supplyContractName = SaveInfo.SupplyContractName;
            supplybodyIDX = SaveInfo.SupplyBodyIDX;
            supplyContractOn = SaveInfo.supplyContractOn;
            supplyResAmount = SaveInfo.supplyAmount;

            LandingOrbitCrew = SaveInfo.LandingOrbitCrew;
            LandingOrbitDescription = SaveInfo.LandingOrbitDesc;
            LandingOrbitIDX = SaveInfo.LandingOrbitIDX;
            LandingOrbitName = SaveInfo.LandingOrbitName;
            isOrbitLanding = SaveInfo.IsOrbitOrLanding;
            OrbitLandingOn = SaveInfo.OrbitLandingOn;
            OrbitCiviliansOn = SaveInfo.OrbitAllowCivs;
            LandingOrbitCivilians = SaveInfo.LandingOrbitCivilians;


            savedroverLat = SaveInfo.SavedRoverLat;
            savedroverlong = SaveInfo.savedRoverLong;
            roverislanded = SaveInfo.RoverLanded;
            roversName = SaveInfo.RoverName;
            roverBody = SaveInfo.RoverBody;
        }


    }
}
