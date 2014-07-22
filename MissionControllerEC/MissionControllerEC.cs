#region Using Directives
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

        private Rect PopUpWindowPosition;
        public static bool ShowPopUpWindow = false;
        
        
        private bool DifficultyLevelCheck = false;
        private float cst;


        StockToolbar stb = new StockToolbar();
        Instructions mci = new Instructions();

        private Settings settings
        {
            get { return SettingsManager.Manager.getSettings(); }
        }
                
        public void Start()
        {
            Debug.LogError("MCE has been Loaded");           
            mci.GetHiredKerbals();
            stb.CreateButtons();                  
        }

        void OnLevelWasLoaded()
        {
            mci.isKerbalHired();
        }
        void Awake()
        {
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

                Tools.FindSettings();

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
                MainWindowPosition = GUILayout.Window(981974, MainWindowPosition, DrawMainWindow, "Maine MCE Window", GUILayout.MaxHeight(600), GUILayout.MaxWidth(400), GUILayout.MinHeight(300), GUILayout.MinWidth(200));
                MainWindowPosition.x = Mathf.Clamp(MainWindowPosition.x, 0, Screen.width - MainWindowPosition.width);
                MainWindowPosition.y = Mathf.Clamp(MainWindowPosition.y, 0, Screen.height - MainWindowPosition.height);
            }
            if (ShowPopUpWindow)
            {
                PopUpWindowPosition = GUILayout.Window(991974, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 450, 150), drawPopUpWindow, "MCE Information Window");
                PopUpWindowPosition.x = Mathf.Clamp(PopUpWindowPosition.x, 0, Screen.width - PopUpWindowPosition.width);
                PopUpWindowPosition.y = Mathf.Clamp(PopUpWindowPosition.y, 0, Screen.height - PopUpWindowPosition.height);
            }
        }

        private void DrawMainWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Label("Current Funds: " + mci.MceFunds);

            if (GUILayout.Button("Add Money"))
            {
                mci.MceFunds -= 1000;
            }

            GUILayout.Label("Current Science: " + mci.MceScience);
            if (GUILayout.Button("Add Science"))
            {
                mci.MceScience += 1000;
            }

            GUILayout.Label("Current Reputation: " + mci.MceReputation);
                               
            GUILayout.EndVertical();
            if (GUILayout.Button("Exit Window"))
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
