using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        public void drawPopUpWindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            foreach (CurrentHires ch in settings.NewHires)
            {
                GUILayout.Label("Hired: " + ch.hiredKerbalName + "For Cost of: " + settings.HireCost);
            }
                
            GUILayout.Label("Total Cost Of New Recruits: " + settings.totalKerbalCost);

            GUILayout.EndVertical();

            if (GUILayout.Button("Accept New Hire Cost"))
            {
                
                ShowPopUpWindow = false;
                mci.MceFunds -= settings.totalKerbalCost;
                settings.totalKerbalCost = 0;
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
    }
}
