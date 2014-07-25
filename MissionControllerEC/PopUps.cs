using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        public bool RevertHalt = false;        
       
        public void drawPopUpWindow3(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            GUILayout.Label("You can use this Revert button to test your Rockets and Planes \n you will be charged 1000 Funds for the test!");
            if (RevertHalt)
            {
                GUILayout.Label("WAIT FOR EDITOR TO LOAD BEFORE EXIT BUTTON!");
            }
            GUILayout.EndVertical();

            if (!RevertHalt)
            {
                if (GUILayout.Button("Revert To VAB Pay 1000 Funds"))
                {
                    FlightDriver.RevertToPrelaunch(GameScenes.EDITOR);
                    RevertHalt = true;
                }
                if (GUILayout.Button("Revert To SPH Pay 1000 Funds"))
                {
                    FlightDriver.RevertToPrelaunch(GameScenes.SPH);
                    RevertHalt = true;
                }
                if (GUILayout.Button("Exit Without Reverting"))
                {
                    ShowPopUpWindow3 = false;
                }
            }
            else
            {
                if (GUILayout.Button("Exit Window And Charge Funds"))
                {
                    ShowPopUpWindow3 = false;
                    MceFunds -= 1000;
                    saveinfo.TotalSpentOnRocketTest += 1000;
                    saveinfo.Save();
                    RevertHalt = false;
                }
            }
        }
    }
}
