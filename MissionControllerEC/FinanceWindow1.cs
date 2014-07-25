using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        public void drawFinanceWind(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Mission COntroller Extended 2 Version PreRelease 1", GUILayout.Width(400));

            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent Kerbals",GUILayout.Width(150));
            GUILayout.Box("" + (int)saveinfo.TotalSpentKerbals, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            //GUILayout.Space(10);
            //GUILayout.BeginHorizontal();
            //GUILayout.Box("Total Saleries", GUILayout.Width(150));
            //GUILayout.Box("" + (int)saveinfo.TotalSpentOnSaleries, GUILayout.Width(200));
            //GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent RocketTest (Reverts)", GUILayout.Width(150));
            GUILayout.Box("" + (int)saveinfo.TotalSpentOnRocketTest, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            //GUILayout.Space(10);
            //GUILayout.BeginHorizontal();
            //GUILayout.Box("Next PayPeriod Will Be On", GUILayout.Width(150));
            //GUILayout.Box("" + Tools.formatTime(saveinfo.CurrentTimeCheck), GUILayout.Width(200));
            //GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.EndVertical();
            //if (GUILayout.Button("Settings menu"))
            //{               
            //    ShowMainWindow = true;
            //}
            if (GUILayout.Button("Exit Window"))
            {
                ShowfinanaceWindow = false;
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
    }
}
