using System;
using UnityEngine;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {                      
        public void drawPopUpWindow3(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            GUILayout.Label("You can use this Revert button to test your Rockets and Planes \n you will be charged 1000 Funds for the test!");
            if (MCE_ScenarioStartup.RevertHalt)
            {
                GUILayout.Label("WAIT FOR EDITOR TO LOAD BEFORE EXIT BUTTON!");
            }
            GUILayout.EndVertical();

            if (!MCE_ScenarioStartup.RevertHalt)
            {
                if (GUILayout.Button("Revert To VAB Pay 1000 Funds"))
                {
                    FlightDriver.RevertToPrelaunch(GameScenes.EDITOR);
                    MCE_ScenarioStartup.RevertHalt = true;
                }
                if (GUILayout.Button("Revert To SPH Pay 1000 Funds"))
                {
                    FlightDriver.RevertToPrelaunch(GameScenes.SPH);
                    MCE_ScenarioStartup.RevertHalt = true;
                }
                if (GUILayout.Button("Exit Without Reverting"))
                {
                    MCE_ScenarioStartup.ShowPopUpWindow3 = false;
                }
            }
            else
            {
                if (GUILayout.Button("Exit Window And Charge Funds"))
                {
                    MCE_ScenarioStartup.ShowPopUpWindow3 = false;
                    Funding.Instance.Funds -= 1000;
                    SaveInfo.TotalSpentOnRocketTest += 1000;
                    MCE_ScenarioStartup.RevertHalt = false;
                }
            }
        }
    }
}
