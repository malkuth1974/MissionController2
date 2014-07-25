#region 
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
#endregion

namespace MissionControllerEC
{
    [KSPAddonFixed(KSPAddon.Startup.SpaceCentre, true, typeof(MissionControllerEC))]
    public partial class MissionControllerEC : MonoBehaviour
    {        
        private Rect MainWindowPosition;
        public static bool ShowMainWindow = false;
        
        private Rect FinanceWindowPosition;
        public static bool ShowfinanaceWindow = false;
        
        private Rect PopUpWindowPosition3;
        public static bool ShowPopUpWindow3;
        
        
        private bool DifficultyLevelCheck = false;
        private float cst;

        public static bool TestThis = false;
        public static bool TestThis2 = false;

        StockToolbar stb = new StockToolbar();

        Settings settings = new Settings("Config.cfg");
        SaveInfo saveinfo = new SaveInfo(HighLogic.CurrentGame.Title + "SaveFile.cfg");
                
        public void Start()
        {
            Debug.LogError("MCE has been Loaded");           
            GetHiredKerbals();               
        }

        void OnLevelWasLoaded()
        {             
        }
        void Update()
        {
            //if (saveinfo.CurrentTimeCheck < Planetarium.GetUniversalTime())
            //{               
            //    mci.Check30DaySaleries();
            //}
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                isKerbalHired();
            }
        }
        void Awake()
        {
            if (saveinfo.FileExists)
            {
                saveinfo.Load();               
            }
            else 
            { 
                saveinfo.Save(); saveinfo.Load();                              
            }

            if (settings.FileExists)
            {
                settings.Load();
            }
            else
            {
                settings.Save(); settings.Load();
            }
            
            DontDestroyOnLoad(this);          
        }
        void OnDestroy()
        {
        }
      
        public void OnGUI()
        {
            if (!DifficultyLevelCheck)
            {
                DifficultyLevelCheck = true;

                Debug.LogWarning("** MCE2 Is Checking Difficulty Level and Adjusting Prices Parts set difficulties in Settings.cfg file to raise");
                if (settings.difficutlylevel == 1) { cst = settings.EasyMode; Debug.Log("Difficulty is easyMode"); }
                if (settings.difficutlylevel == 2) { cst = settings.MediumMode; Debug.Log("Difficulty is MediumMode"); }
                if (settings.difficutlylevel == 3) { cst = settings.HardCoreMode; Debug.Log("Difficulty is HardcoreMode"); }

                foreach (AvailablePart ap in PartLoader.LoadedPartsList)
                {

                    try
                    {                        
                        Debug.Log("MCE Changed Price Of Part " + ap.name + ": " + ap.title + "," + "from: " + ap.cost + " To: " + ap.cost * cst);
                        ap.cost = ap.cost * cst;
                    }
                    catch
                    {
                    }
                }
                EditorPartList.Instance.Refresh();
            }

            if (ShowMainWindow)
            {
                MainWindowPosition = GUILayout.Window(971974, MainWindowPosition, DrawMainWindow, "Maine MCE Window", GUILayout.MaxHeight(600), GUILayout.MaxWidth(400), GUILayout.MinHeight(300), GUILayout.MinWidth(200));
                MainWindowPosition.x = Mathf.Clamp(MainWindowPosition.x, 0, Screen.width - MainWindowPosition.width);
                MainWindowPosition.y = Mathf.Clamp(MainWindowPosition.y, 0, Screen.height - MainWindowPosition.height);
            }
            if (ShowfinanaceWindow)
            {
                FinanceWindowPosition = GUILayout.Window(981974, FinanceWindowPosition, drawFinanceWind, "MCE Finances", GUILayout.MaxHeight(800), GUILayout.MaxWidth(400), GUILayout.MinHeight(250), GUILayout.MinWidth(390));
                FinanceWindowPosition.x = Mathf.Clamp(FinanceWindowPosition.x, 0, Screen.width - FinanceWindowPosition.width);
                FinanceWindowPosition.y = Mathf.Clamp(FinanceWindowPosition.y, 0, Screen.height - FinanceWindowPosition.height);
            }                       
            if (ShowPopUpWindow3)
            {
                PopUpWindowPosition3 = GUILayout.Window(1011974, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 450, 150), drawPopUpWindow3, "MCE Information Window");
            }
        }

        private void DrawMainWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            
            GUILayout.Label("Current Funds: " + MceFunds);

            if (GUILayout.Button("Add Money"))
            {
                MceFunds += 1000;
            }

            GUILayout.Label("Current Science: " + MceScience);
            if (GUILayout.Button("Add Science"))
            {
                MceScience += 1000;
            }

            if (GUILayout.Button("Finance Window"))
            {
                ShowfinanaceWindow = true;
            }

            GUILayout.Label("Current Reputation: " + MceReputation);


            GUILayout.EndVertical();
            if (GUILayout.Button("Exit Save Settings"))
            {
                ShowMainWindow = false;
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
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
