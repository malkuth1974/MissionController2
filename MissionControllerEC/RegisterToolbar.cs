using UnityEngine;
using ToolbarControl_NS;
using KSP_Log;

namespace MissionControllerEC
{

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        internal static Log Log = null;
        static bool _gamePaused = false;
        static double lastTime = 0;

        public const string MODID = "MC_EC";
        public const string MODNAME = "Mission Controller";

        public static bool GamePaused
        {
            get
            {
                if (_gamePaused)
                {
                    if (Planetarium.GetUniversalTime() != lastTime)
                        _gamePaused = false;
                }
                lastTime = Planetarium.GetUniversalTime();
                return _gamePaused;
            }
        }

        void Awake()
        {
            if (Log == null)
#if DEBUG
                Log = new Log("MissionControllerED", Log.LEVEL.INFO);
#else
                Log = new Log("MissionControllerED", Log.LEVEL.ERROR);
#endif
            GameEvents.onGameSceneLoadRequested.Add(onGameSceneLoadRequested);

            DontDestroyOnLoad(this);
        }

        void onGamePause() { lastTime = Planetarium.GetUniversalTime(); _gamePaused = true; }
        void onGameUnpause() { if (HighLogic.CurrentGame != null) lastTime = Planetarium.GetUniversalTime(); _gamePaused = false; }

        void onGameSceneLoadRequested(GameScenes gs)
        {
            onGameUnpause();
        }

        void Start()
        {
            ToolbarControl.RegisterMod(MODID, MODNAME);
        }

        static public bool initted = false;
        static public GUIStyle buttonLeft;


        void OnGUI()
        {
            if (!initted)
            {
                GUI.skin = HighLogic.Skin;

                initted = true;
                buttonLeft = new GUIStyle(GUI.skin.label);
                buttonLeft.alignment = TextAnchor.MiddleLeft;

            }
        }
    }
}