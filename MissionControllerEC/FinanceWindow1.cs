using System;
using UnityEngine;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        public void drawEditorwindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            MCE_ScenarioStartup.scrollPosition = GUILayout.BeginScrollView(MCE_ScenarioStartup.scrollPosition, GUILayout.Width(408));
            GetPartsCost();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            showFuel = (GUILayout.Toggle(showFuel, "Toggle Fuel Type Visible"));
            showTons = (GUILayout.Toggle(showTons, "Show Mass Vessel"));
            if (showTons)
                showMiniTons = (GUILayout.Toggle(showMiniTons, "Show Individual Mass(Kg) Parts"));
            
            if (GUILayout.Button("Exit Window And Save"))
            {
                MCE_ScenarioStartup.ShowEditorWindow = false;
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        
        public void drawFinanceWind(int id)
        {
            CelestialBody targetbody = null;
            targetbody = FlightGlobals.Bodies[settings.bodyNumber];

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.Box(MCE_ScenarioStartup.mainWindowTitle + " " + MCE_ScenarioStartup.versionCode, GUILayout.Width(400));

            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent Kerbals",GUILayout.Width(200));
            GUILayout.Box("" + (int)SaveInfo.TotalSpentKerbals, GUILayout.Width(200));
            GUILayout.EndHorizontal();            

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent RocketTest (Reverts)", GUILayout.Width(200));
            GUILayout.Box("" + (int)SaveInfo.TotalSpentOnRocketTest, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.Label("ComSat Network Information");

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("ComSat Network Contracts On", GUILayout.Width(200));
            GUILayout.Box("" + settings.StartBuilding, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Turn On Contracts"))
            {
                settings.StartBuilding = true;
            }
            if (GUILayout.Button("Turn Off Contracts"))
            {
                settings.StartBuilding = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Box("ComSat Max Orbital Period", GUILayout.Width(200));
            GUILayout.Box("" + Tools.formatTime(settings.maxOrbP), GUILayout.Width(200));           
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("- Hour", GUILayout.Width(75))) { settings.maxOrbP -= 3600; }
            if (GUILayout.Button("+ Hour", GUILayout.Width(75))) { settings.maxOrbP += 3600; }
            if (GUILayout.Button("- Minute", GUILayout.Width(75))) { settings.maxOrbP -= 60; }
            if (GUILayout.Button("+ Minute", GUILayout.Width(75))) { settings.maxOrbP += 60; }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Box("ComSat Min Orbital Period", GUILayout.Width(200));
            GUILayout.Box("" + Tools.formatTime(settings.minOrbP), GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("- Hour", GUILayout.Width(75))) { settings.minOrbP -= 3600; }
            if (GUILayout.Button("+ Hour", GUILayout.Width(75))) { settings.minOrbP += 3600; }
            if (GUILayout.Button("- Minute", GUILayout.Width(75))) { settings.minOrbP -= 60; }
            if (GUILayout.Button("+ Minute", GUILayout.Width(75))) { settings.minOrbP += 60; }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Box("ComSat Body", GUILayout.Width(200));
            GUILayout.Box("" + targetbody.theName, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("- Previous"))
            {
                settings.bodyNumber--;              
                if (settings.bodyNumber < 1 || settings.bodyNumber > 16)
                {
                    settings.bodyNumber = 1;
                }                             
            }
            
            if (GUILayout.Button("+ Next")) 
            {
                settings.bodyNumber++;               
                if (settings.bodyNumber > 16 || settings.bodyNumber < 1)
                {
                    settings.bodyNumber = 1;
                }
                              
            }
            
            GUILayout.EndHorizontal();
            
            
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Contract Mission Name", GUILayout.Width(150), GUILayout.Height(30));
            settings.contractName = GUILayout.TextField(settings.contractName, 50);
            GUILayout.EndHorizontal();          

            GUILayout.Space(10);
            GUILayout.EndVertical();
            if (settings.DebugMenu && GUILayout.Button("Debug Menu"))
            {
               MCE_ScenarioStartup.ShowMainWindow = true;
            }
            if (GUILayout.Button("Settings Menu"))
            {
                MCE_ScenarioStartup.ShowSettingsWindow = true;
            }
            if (GUILayout.Button("Exit Window And Save"))
            {
                MCE_ScenarioStartup.ShowfinanaceWindow = false;
                settings.Save();
                settings.Load();
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
    }
}
