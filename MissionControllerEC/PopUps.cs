using System;
using UnityEngine;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {                      
        public void drawPopUpWindow3(int id)
        {
            GUI.skin = HighLogic.Skin;
            double revertcost = settings.Revert_Cost;

            GUILayout.BeginVertical();
            GUILayout.Label("You can use this Revert button to test your Rockets and Planes \n you will be charged " + revertcost + " Funds for the test!");
            if (MCE_ScenarioStartup.RevertHalt)
            {
                GUILayout.Label("WAIT FOR EDITOR TO LOAD BEFORE EXIT BUTTON!");
            }
            GUILayout.EndVertical();

            if (!MCE_ScenarioStartup.RevertHalt)
            {
                if (GUILayout.Button("Revert To VAB Pay " + revertcost + " Funds"))
                {
                    FlightDriver.RevertToPrelaunch(EditorFacility.VAB);
                    MCE_ScenarioStartup.RevertHalt = true;
                }
                if (GUILayout.Button("Revert To SPH Pay " + revertcost + " Funds"))
                {
                    FlightDriver.RevertToPrelaunch(EditorFacility.SPH);
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
                    if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                    {
                        Funding.Instance.AddFunds(-revertcost, TransactionReasons.Any);
                    }
                    MCE_ScenarioStartup.RevertHalt = false;
                }
            }
        }
    }
}
