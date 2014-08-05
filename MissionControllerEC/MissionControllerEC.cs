
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using KSP;

namespace MissionControllerEC
{  
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class MCE_ScenarioStartup : MonoBehaviour
    {         
        public static bool DifficultyLevelCheck = false;
        public static float cst;

        public static Rect PopUpWindowPosition3;
        public static bool ShowPopUpWindow3;

        public static Rect MainWindowPosition;
        public static bool ShowMainWindow = false;

        public static Rect FinanceWindowPosition;
        public static bool ShowfinanaceWindow = false;

        public static bool RevertHalt = false;

        // Special thanks to Magico13 Of Kerbal Construction Time for showing me how to Get Scenario Persistance.

        void Start()
        {
            ProtoScenarioModule scenario = HighLogic.CurrentGame.scenarios.Find(s => s.moduleName == typeof(MissionControllerData).Name);
            if (scenario == null)
            {
                try
                {
                    HighLogic.CurrentGame.AddProtoScenarioModule(typeof(MissionControllerData), new GameScenes[] { GameScenes.FLIGHT, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.SPH, GameScenes.TRACKSTATION });
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
                Debug.LogWarning("[MCE] Scenario is not null.");
                if (!scenario.targetScenes.Contains(GameScenes.SPACECENTER))
                    scenario.targetScenes.Add(GameScenes.SPACECENTER);
                if (!scenario.targetScenes.Contains(GameScenes.FLIGHT))
                    scenario.targetScenes.Add(GameScenes.FLIGHT);
                if (!scenario.targetScenes.Contains(GameScenes.EDITOR))
                    scenario.targetScenes.Add(GameScenes.EDITOR);
                if (!scenario.targetScenes.Contains(GameScenes.SPH))
                    scenario.targetScenes.Add(GameScenes.SPH);
                if (!scenario.targetScenes.Contains(GameScenes.TRACKSTATION))
                    scenario.targetScenes.Add(GameScenes.TRACKSTATION);

            }
        }
    }

    public class MissionControllerData : ScenarioModule
    {
        // used TacLife Support way of loading MissionControllerEC Component by Taranis Elsu
        private readonly List<Component> children = new List<Component>();

        public override void OnAwake()
        {
            Debug.Log("OnAwake in " + HighLogic.LoadedScene);
            base.OnAwake();

            if (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.SPH)
            {
                Debug.Log("Adding MissionController Child");
                var c = gameObject.AddComponent<MissionControllerEC>();
                children.Add(c);
            }
        }

        public override void OnSave(ConfigNode node)
        {
            Debug.Log("[MCE] Writing to persistence.");
            base.OnSave(node);
            MCE_DataStorage mceData = new MCE_DataStorage();
            node.AddNode(mceData.AsConfigNode());
        }
        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("[MCE] Loading from persistence.");
            base.OnLoad(node);
            MCE_DataStorage mceData = new MCE_DataStorage();
            ConfigNode CN = node.GetNode(mceData.GetType().Name);
            if (CN != null)
                ConfigNode.LoadObjectFromConfig(mceData, CN);
        }

        void OnDestroy()
        {
            Debug.Log("MCE ScenarioModule OnDestroy");
            foreach (Component c in children)
            {
                Destroy(c);
                //MCELoaded = false;
            }
            children.Clear();
        }
    }
    public partial class MissionControllerEC : MonoBehaviour
    {
        private static Texture2D texture;
        private static Texture2D texture2;
        private ApplicationLauncherButton MCEButton;
        private ApplicationLauncherButton MCERevert;
        
        Settings settings = new Settings("Config.cfg");

        public void Awake()
        {
            loadTextures();
            loadFiles();
            CreateButtons();
            GameEvents.Contract.onContractsLoaded.Add(this.onContractLoaded);
            GameEvents.onCrewKilled.Add(this.chargeKerbalDeath);
            GameEvents.onKerbalTypeChange.Add(this.hireKerbals);
            Debug.Log("MCE Awake");           
        }    
                
        public void Start()
        {           
            Debug.LogError("MCE Onstart Files Loaded");                    
        }
      
                      
        void OnDestroy()
        {
            DestroyButtons();
            Debug.Log("MCE OnDestroy");
            GameEvents.onCrewKilled.Remove(this.chargeKerbalDeath);
            GameEvents.onKerbalTypeChange.Remove(this.hireKerbals);
            GameEvents.Contract.onContractsLoaded.Remove(this.onContractLoaded);
            Debug.Log("Game All values removed for MCE");
        }                
      
        public void OnGUI()
        {
            if (!MCE_ScenarioStartup.DifficultyLevelCheck)
            {
                MCE_ScenarioStartup.DifficultyLevelCheck = true;

                Debug.LogWarning("** MCE2 Is Checking Difficulty Level and Adjusting Prices Parts set difficulties in Settings.cfg file to raise");
                if (settings.difficutlylevel == 1) { MCE_ScenarioStartup.cst = settings.EasyMode; Debug.Log("Difficulty is easyMode"); }
                if (settings.difficutlylevel == 2) { MCE_ScenarioStartup.cst = settings.MediumMode; Debug.Log("Difficulty is MediumMode"); }
                if (settings.difficutlylevel == 3) { MCE_ScenarioStartup.cst = settings.HardCoreMode; Debug.Log("Difficulty is HardcoreMode"); }

                foreach (AvailablePart ap in PartLoader.LoadedPartsList)
                {

                    try
                    {                        
                        //Debug.Log("MCE Changed Price Of Part " + ap.name + ": " + ap.title + "," + "from: " + ap.cost + " To: " + ap.cost * cst);
                        ap.cost = ap.cost * MCE_ScenarioStartup.cst;
                    }
                    catch
                    {
                    }
                }
                EditorPartList.Instance.Refresh();
            }


            if (MCE_ScenarioStartup.ShowMainWindow)
            {
                MCE_ScenarioStartup.MainWindowPosition = GUILayout.Window(971974, MCE_ScenarioStartup.MainWindowPosition, DrawMainWindow, "Maine MCE Window", GUILayout.MaxHeight(600), GUILayout.MaxWidth(400), GUILayout.MinHeight(300), GUILayout.MinWidth(200));
                MCE_ScenarioStartup.MainWindowPosition.x = Mathf.Clamp(MCE_ScenarioStartup.MainWindowPosition.x, 0, Screen.width - MCE_ScenarioStartup.MainWindowPosition.width);
                MCE_ScenarioStartup.MainWindowPosition.y = Mathf.Clamp(MCE_ScenarioStartup.MainWindowPosition.y, 0, Screen.height - MCE_ScenarioStartup.MainWindowPosition.height);
            }
            if (MCE_ScenarioStartup.ShowfinanaceWindow)
            {
                MCE_ScenarioStartup.FinanceWindowPosition = GUILayout.Window(981974, MCE_ScenarioStartup.FinanceWindowPosition, drawFinanceWind, "MCE Finances", GUILayout.MaxHeight(800), GUILayout.MaxWidth(400), GUILayout.MinHeight(250), GUILayout.MinWidth(390));
                MCE_ScenarioStartup.FinanceWindowPosition.x = Mathf.Clamp(MCE_ScenarioStartup.FinanceWindowPosition.x, 0, Screen.width - MCE_ScenarioStartup.FinanceWindowPosition.width);
                MCE_ScenarioStartup.FinanceWindowPosition.y = Mathf.Clamp(MCE_ScenarioStartup.FinanceWindowPosition.y, 0, Screen.height - MCE_ScenarioStartup.FinanceWindowPosition.height);
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
                Funding.Instance.Funds += 1000;
            }

            GUILayout.Label("Current Science: " + ResearchAndDevelopment.Instance.Science);
            if (GUILayout.Button("Add Science"))
            {
                ResearchAndDevelopment.Instance.Science += 1000;
            }           

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
        [Persistent]public double TotalSpentKerbals = 0;
        [Persistent]public double TotalSpentRockets = 0;
        [Persistent]public bool ComSatOn = false;

        public override void OnDecodeFromConfigNode()
        {
            SaveInfo.TotalSpentKerbals = TotalSpentKerbals;
            SaveInfo.TotalSpentOnRocketTest = TotalSpentRockets;
            SaveInfo.SatContractReady = ComSatOn;

        }

        public override void OnEncodeToConfigNode()
        {
            TotalSpentKerbals = SaveInfo.TotalSpentKerbals;
            TotalSpentRockets = SaveInfo.TotalSpentOnRocketTest;
            ComSatOn = SaveInfo.SatContractReady;

        }
    

    }

    public class KSPAddonFixed : KSPAddon, IEquatable<KSPAddonFixed>
    {
        private readonly Type type;

        public KSPAddonFixed(KSPAddon.Startup startup, bool once, Type type)
            : base(startup, once)
        {
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) { return false; }
            return Equals((KSPAddonFixed)obj);
        }

        public bool Equals(KSPAddonFixed other)
        {
            if (this.once != other.once) { return false; }
            if (this.startup != other.startup) { return false; }
            if (this.type != other.type) { return false; }
            return true;
        }

        public override int GetHashCode()
        {
            return this.startup.GetHashCode() ^ this.once.GetHashCode() ^ this.type.GetHashCode();
        }
    }
}
