
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


namespace MissionControllerEC
{

    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION)]
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

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CoolUILoader : MonoBehaviour
    {
        private static GameObject panelPrefab;

        public static GameObject PanelPrefab
        {
            get { return panelPrefab; }
        }

        private void Awake()
        {
            // Mission Controller First use of Custom UI for Repair Panel.  Thanks to Fengist and Dmagic for their guides on how to do this.
            // I left most of Fengist words below intact to help others if they want to see how I did this.  Minor changes though, but should help.
            // Oh PS sorry for messy code.  Im like a below amatuer coder. :)
            //The way I was doing this which does seem to work.  But DMagic's method makes much more sense.
            //AssetBundle prefabs = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mycoolui.ksp"));
            //DMagic's method without the .ksp file extension            
            AssetBundle prefabs = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mycoolui.dat"));
            panelPrefab = prefabs.LoadAsset("MyCoolUiPanel") as GameObject;
            Debug.Log("MCE CoolUILoader Public Class Fired why?")
;        }
    }   

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public partial class MissionControllerEC : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject RepairUICanvas = null;
        public static GameObject RepairText = null;
        public static List<RepairPanel> RepairUIParts = new List<RepairPanel>();
        private static Vector2 Repairdragstart;
        private static Vector2 Repairaltstart;

        private static Texture2D texture;
        private static Texture2D texture2;
        private static ApplicationLauncherButton MCEButton;
        private static ApplicationLauncherButton MCERevert;        
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
        private static DialogGUIBase Custom_Contract_Button4;
        private static DialogGUIBase Custom_Contract_Button5;
        private static DialogGUIBase Custom_Contract_Button6;
        private static DialogGUIBase Custom_Contract_Button7;
        private static DialogGUIBase Custom_Contract_Button8;
        private static DialogGUIBase Custom_Contract_Button9;
        private static DialogGUIBase Custom_Contract_Button10;
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
        

        private static MissionControllerEC Instance
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
            Main_popup_dialog = PopupDialog.SpawnPopupDialog(Mainmulti_dialog, true, HighLogic.UISkin, false, "");
            Hide();
            loadTextures();
            Debug.LogWarning("[MCE] Textrues called OnAwake");
            MceCreateButtons();
            Debug.LogWarning("[MCE] Buttons called OnAwake");
            Debug.Log("MCE MissionControllerEC Main Awake called");
            //this creates a callback so that whenever the scene is changed we can destroy the UI
            GameEvents.onGameSceneSwitchRequested.Add(OnRepairSceneChange);

            if (Main_popup_dialog = null)
            {
                SaveInfo.MainGUIWindowPos = new Vector2(
                ((Screen.width / 2) + Main_popup_dialog.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + Main_popup_dialog.RTrf.position.y) / Screen.height);
            }

        }
        #region RepairPanel UI Stuff
        //If we don't get rid of the UI, it'll stay where it is indefinitely.  So, every time the scene is changed, we need to get rid of it.
        void OnRepairSceneChange(GameEvents.FromToAction<GameScenes, GameScenes> fromToScenes)
        {
            if (RepairUICanvas != null)
            {
                Destroy();
            }
        }

        //This actually destroys the UI.  But it also goes through the partmodule's toggle buttons and turns them all off.
        public static void Destroy()
        {
            RepairUICanvas.DestroyGameObject();
            RepairUICanvas = null;
            foreach (RepairPanel thisPart in RepairUIParts)
            {
                thisPart.openUI = false;
            }
        }

        public static void RepairShowGUI()
        {
            if (RepairUICanvas != null)  //if the UI is already showing, don't show another one.
                return;

            //Load up the UI and show it
            RepairUICanvas = (GameObject)Instantiate(CoolUILoader.PanelPrefab);
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

            ConfigNode node = new ConfigNode();
            var thisnode = GetConfig("RepairUI");
            float xpos = float.Parse(thisnode.GetValue("x"));
            float ypos = float.Parse(thisnode.GetValue("y"));
            RepairUICanvas.transform.position = new Vector3(xpos, ypos, RepairUICanvas.transform.position.z);
        }

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

        //this function is where we update the text component on the UI.
        public static void RepairUpdateText(string message)
        {
            RepairText.GetComponent<Text>().text = message;
        }

        //this event fires when a drag event begins
        public void OnBeginDrag(PointerEventData data)
        {
            Repairdragstart = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
            Repairaltstart = RepairUICanvas.transform.position;
        }

        //this event fires while we're dragging. It's constantly moving the UI to a new position
        public void OnDrag(PointerEventData data)
        {
            Vector2 dpos = new Vector2(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
            Vector2 dragdist = dpos - Repairdragstart;
            RepairUICanvas.transform.position = Repairaltstart + dragdist;
        }

        //this event fires when we let go of the mouse and stop dragging
        public void OnEndDrag(PointerEventData data)
        {
            SetConfig(RepairUICanvas.transform.position.x, RepairUICanvas.transform.position.y, "RepairUI");
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
        void OnDestroy()
        {
            if (MCEButton != null && HighLogic.LoadedScene != GameScenes.SPACECENTER)
            {
                ApplicationLauncher.Instance.RemoveModApplication(MCEButton);
                Debug.LogError("[MCE] Button OnDestroyed called");
            }
            if (MCERevert != null && !HighLogic.LoadedSceneIsFlight)
            {
                ApplicationLauncher.Instance.RemoveModApplication(MCERevert);
                Debug.LogError("[MCE] Revert OnDestroyed called");
            }           
            
            GameEvents.Contract.onContractsLoaded.Remove(this.onContractLoaded);          
           instance = null;
            Debug.Log("MCE MissioniControllerEC Main OnDestroye Called");

            try
            {
                Main_popup_dialog.Dismiss();
            }
            catch { Debug.Log("MCE Main_Popup_dialog already loaded"); }
                     
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

        static ConfigNode GetConfig(string NodeText)
        {
            string CurrentNode = NodeText;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MceGUILocation.dat");
            ConfigNode result = ConfigNode.Load(filePath).GetNode(NodeText);
            return result;
        }

        private static void SetConfig(float x, float y, string NodeText)
        {
            string CurrentNode = NodeText;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MceGUILocation.dat");
            ConfigNode thiscfg = new ConfigNode();
            var thisnode = thiscfg.AddNode(CurrentNode);
            thisnode.AddValue("x", x);
            thisnode.AddValue("y", y);
            thiscfg.Save(filePath);
        }

    }

    public class RepairPanel : PartModule
    {
       

        [KSPField(guiName = "Open Repair Panel", guiActiveEditor = true, guiActive = true, isPersistant = false),
        UI_Toggle(controlEnabled = true, disabledText = "Closed", enabledText = "Open", invertButton = false, scene = UI_Scene.All)]
        public bool openUI = false;

        //and a progress bar that we can play with
        [KSPField(guiName = "Battery Coolant", guiActiveEditor = true, guiActive = true, isPersistant = true),
        UI_ProgressBar(minValue = 0f, maxValue = 1f, scene = UI_Scene.All)]
        public float PartUICoolSlider = 0.0f;

        [KSPField(guiName = "Air Pressure Purge", guiActiveEditor = true, guiActive = true, isPersistant = true),
        UI_ProgressBar(minValue = 0f, maxValue = 1f, scene = UI_Scene.All)]
        public float PartUICoolSlider2 = 0.0f;

        [KSPField(guiName = "HMI Reset", guiActiveEditor = true, guiActive = true, isPersistant = true),
        UI_ProgressBar(minValue = 0f, maxValue = 1f, scene = UI_Scene.All)]
        public float PartUICoolSlider3 = 0.0f;

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

        int Maxtime = Tools.RandomNumber(30, 100);


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

        [KSPField(isPersistant = true, guiActive = true, guiName = "Ready To Repair")]
        public bool readyRep = false;

        public void CheckSystems()
        {
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                List<ProtoCrewMember> protoCrewMembers = FlightGlobals.ActiveVessel.GetVesselCrew();
                foreach (Experience.ExperienceEffect exp in protoCrewMembers[0].experienceTrait.Effects)
                {
                    if (exp.ToString() == "Experience.Effects.RepairSkill")
                    {
                        Debug.Log("Current kerbal is a Engineer you have passed");
                        readyRep = true;
                        vesselId = this.part.vessel.id.ToString();
                        vesselName = this.part.vessel.name;
                        Debug.LogError("Vessel Id For PartModule is " + vesselId + " Name is " + vesselName);
                        ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000250"), 10f);          // #autoLOC_MissionController2_1000250 = Your engineer has Prepared the vessel for Repair Open the panel, Then conduct the repair

                    }
                    else
                    {
                        Debug.Log("Current kerbal is NOT an Engineer you don't pass... Bad boy!");
                        ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000251"), 10f);     // #autoLOC_MissionController2_1000251 = You need an Engineer to fix this Vessel!
                    }
                }
            }
            else { ScreenMessages.PostScreenMessage("Hey you can only do this in flight?  How you manage this anyway?"); }
        }

        public void EnableRepair()
        {
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                if (readyRep && currentRepair > 0 && PartUICoolSlider == 0 && PartUICoolSlider2 == 0 && PartUICoolSlider3 == 1)
                {
                    repair = true;
                    Debug.Log("repairEnabled");
                    ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000252"), 10f);      // #autoLOC_MissionController2_1000252 = Your engineer has repaired this vessel.  Good job!
                    readyRep = false;
                }
                if (readyRep == false)
                {
                    ScreenMessages.PostScreenMessage("You have to Have an Engineer Test the Repair Systems.. That means Push the Test button!", 10f);
                }
                else { ScreenMessages.PostScreenMessage(Localizer.Format("You Don't Have The Right Combonation Of Sliders, Please Adjust!"), 10f); }
            }
            else { ScreenMessages.PostScreenMessage("Hey you can only do this in flight?  How you manage this anyway?"); }
        }        

        public override void OnStart(StartState state)
        {
            //This creates a callback so that whenver the UI_Toggle openUI is clicked, it either opens or closes the main UI
            Fields[nameof(openUI)].uiControlFlight.onFieldChanged = delegate (BaseField a, System.Object b)
            {
                if (MissionControllerEC.RepairUICanvas == null)
                {
                    MissionControllerEC.RepairShowGUI(); //if the UI doesn't exist, create one and show it.
                    PlayOpenAnimation(1, 0);                    
                }
                else
                {
                    MissionControllerEC.Destroy(); //if it does exist. they're closing it so get rid of it.
                    PlayOpenAnimation(-1, 1);                   
                }
            };
            this.part.force_activate();
        }


        public void FixedUpdate()
        {
            //This checks to see if the UI is being shown.  If so, it will update any other CoolUIPm partmodules so that they show the button as being clicked.
            if (MissionControllerEC.RepairUICanvas != null)
            { openUI = true; }
            else
            { return; }  //if not, we don't want to call UI functions because they'll create a null ref.

            //This is going to take the partmodule UI_ProgressBar and make it the same as the UI slider.
            PartUICoolSlider = MissionControllerEC.RepairSliderPosition();
            PartUICoolSlider2 = MissionControllerEC.RepairCoolSliderPosition2();
            PartUICoolSlider3 = MissionControllerEC.RepairCoolSliderPosition3();
            
                MissionControllerEC.RepairUpdateText("Reboot The HMI");                
        }
    }

    public class MCE_DataStorage : ConfigNodeStorage
    {        
        [Persistent]public bool ComSatOn = false;        
              
        [Persistent]public string supplyVesselName = "None";
        [Persistent]public string supplyVesselId = "none";
        [Persistent]public string supplyResource = "none";
        [Persistent]public string supplyContractName = "none";
        [Persistent]public int supplybodyIDX;
        [Persistent]public bool supplyContractOn = false;
        [Persistent]public double supplyResAmount = 0;

        [Persistent] public int LandingOrbitCrew;
        [Persistent] public string LandingOrbitDescription = "Place Title Here";
        [Persistent] public int LandingOrbitIDX;
        [Persistent] public string LandingOrbitName = "Title Here";
        [Persistent] public bool isOrbitLanding = false;
        [Persistent] public bool OrbitLandingOn = false;
        [Persistent] public bool OrbitCiviliansOn = false;
        [Persistent] public int LandingOrbitCivilians = 0;

        [Persistent]public string crewtransfername = "crew transfer";
        [Persistent]public string crewvesname = "none";
        [Persistent]public string crewvesId = "none";
        [Persistent]public int crewamount = 0;
        [Persistent]public double crewtime = 0;
        [Persistent]public int crewbodyIDX = 0;
        [Persistent]public bool crewcontracton = false;
        [Persistent]public bool crewCiviliansOn = false;
        [Persistent] public string transfercrewDesc = "Place Description Here";
        [Persistent] public int transferTouristAmount = 0;

        [Persistent]public bool noOrbitalContract = false;
        [Persistent]public bool noLandingContract = false;
        [Persistent]public bool noSatelliteContract = false;
        [Persistent]public bool noRepairContract = false;
        [Persistent]public bool noOrbitalPeriodContract = false;
        [Persistent]public bool noHistoricContracts = false;
        [Persistent]public bool noCivilianContracts = false;

        [Persistent]internal bool com_Sat_Start_Building = false;
        [Persistent]internal double com_Sat_maxOrbP = 70000;
        [Persistent]internal double com_Sat_minOrbP = 0;
        [Persistent]internal string com_Sat_contractName = "Deliever COMSAT Satellite";
        [Persistent]internal int com_Sat_bodyNumber = 1;
        [Persistent]internal bool hardcoreOn = true;
        
        [Persistent]internal double savedroverLat = 0;
        [Persistent]internal double savedroverlong = 0;
        [Persistent]internal bool roverislanded = false;
        [Persistent]internal string roversName = "Rover Name";
        [Persistent]internal int roverBody = 6;

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
