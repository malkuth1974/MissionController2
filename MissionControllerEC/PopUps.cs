using System;
using UnityEngine;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        double revertcost = HighLogic.CurrentGame.Parameters.CustomParams<IntergratedSettings3>().MCERevertCost;
        //bool CustomContractsShow = false;
        //string ComSatName = "None";           
        internal void MainMceWindow()
        {
            PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                   new MultiOptionDialog("MCEMain", "", "Mission Controller Main Menu Version: " + MCE_ScenarioStartup.versionCode, HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIVerticalLayout(
                            new DialogGUIFlexibleSpace(),                          
                            new DialogGUIButton("Debug Menu",
                                delegate
                                {
                                    MCE_ScenarioStartup.ShowMainWindow = true;
                                }, 200f, 30.0f, true),
                            new DialogGUIVerticalLayout(),
                            new DialogGUIFlexibleSpace(),                           
                            new DialogGUIButton("Edit ComSat Contract Values",
                                delegate
                                {
                                    comSatwin = true;
                                    MCE_ScenarioStartup.ShowCustomWindow = true; 
                                }
                                , 200f, 30.0f, true),
                            new DialogGUIButton("Edit Supply Contract Values",
                                delegate
                                {
                                    supplywin = true;
                                    MCE_ScenarioStartup.ShowCustomWindow = true;
                                }
                                , 200f, 30.0f, true),
                            new DialogGUIButton("Edit Crew Transfer Contract Values",
                                delegate
                                {
                                    crewwin = true;
                                    MCE_ScenarioStartup.ShowCustomWindow = true;
                                }
                                , 200f, 30.0f, true),
                            new DialogGUIButton("Exit Menu", () => { }, 200f, 30.0f, true)
                            )),
                    false,
                    HighLogic.UISkin);
        }
        internal void RevertPress()
        {                             
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                   new MultiOptionDialog("MCERevert", "", "Mission Controller Revert", HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIVerticalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIButton("Revert To VAB Pay $" + revertcost + " Funds",
                                delegate
                                {
                                    FlightDriver.RevertToPrelaunch(EditorFacility.VAB);
                                    //RevertHalt = true;
                                    RevertHaultSet();                                
                                }, 200f, 30.0f, true),
                            new DialogGUIButton("Revert To SPH Pay $" + revertcost + " Funds",
                                delegate
                                {
                                    FlightDriver.RevertToPrelaunch(EditorFacility.SPH);
                                    //RevertHalt = true; 
                                    RevertHaultSet();
                                }, 200f, 30.0f, true),
                            new DialogGUIButton("Exit Without Reverting", () => { }, 200f, 30.0f, true)
                            )),
                    false,
                    HighLogic.UISkin);               
            }
            else { PopupDialog.ClearPopUps(); }
        }
        internal void RevertHaultSet()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
            {
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                       new Vector2(0.5f, 0.5f),
                      new MultiOptionDialog("MCERevertSet", "", "Revert Window Info", HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                           new DialogGUIFlexibleSpace(),
                           new DialogGUIVerticalLayout(
                               new DialogGUIFlexibleSpace(),
                               new DialogGUIButton("You will now Be Charged $" + revertcost + " for Testing Charges", delegate
                               {
                                   Funding.Instance.AddFunds(-revertcost, TransactionReasons.Progression);
                                   RevertHalt = false;
                               }, 200f, 30.0f, true)
                               )),
                       false,
                       HighLogic.UISkin);
            }
        }
        //internal void ComSatContract()
        //{
        //    CelestialBody targetbody = null;
        //    targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
        //    PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
        //               new Vector2(0.5f, 0.5f),
        //              new MultiOptionDialog("ComSat", "", "ComSat Contract Editor", HighLogic.UISkin, new Rect(0.5f, 0.5f, 300, 60f),
        //                   new DialogGUIFlexibleSpace(),
        //                   new DialogGUIVerticalLayout(
        //                       new DialogGUIFlexibleSpace(),
        //                       Turn On Contract Start
        //                       new DialogGUILabel("ComSat Network Contracts " + SaveInfo.ComSateContractOn, false, false),
        //                       new DialogGUIToggle(false, "ComSat Network Contracts On",
        //                       delegate (bool b)
        //                       {
        //                           SaveInfo.ComSateContractOn = !SaveInfo.ComSateContractOn;
        //                           ComSatContract();
        //                       }, 200, 20),
        //                       Max Orbit Start
        //                       new DialogGUILabel("ComSat Max Orbital Period: " + Tools.ConvertMinsHours(SaveInfo.comSatmaxOrbital),
        //                       false, false),

        //                       new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(),
        //                       new DialogGUIButton("-Hour",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatmaxOrbital -= 3600;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false), new DialogGUIButton("+Hour",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatmaxOrbital += 3600;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false), new DialogGUIButton("-Min",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatmaxOrbital -= 60;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false), new DialogGUIButton("+Min",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatmaxOrbital += 60;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false)),
        //                       Min Orbit Start
        //                       new DialogGUILabel("ComSat Min Orbital Period: " + Tools.ConvertMinsHours(SaveInfo.comSatminOrbital),
        //                       false, false),
        //                       new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(),
        //                       new DialogGUIButton("-Hour",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatminOrbital -= 3600;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false), new DialogGUIButton("+Hour",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatminOrbital += 3600;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false), new DialogGUIButton("-Min",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatminOrbital -= 60;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false), new DialogGUIButton("+Min",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatminOrbital += 60;
        //                           ComSatContract();
        //                       }
        //                       , 50f, 30.0f, false)),
        //                       Body Selection Start
        //                       new DialogGUILabel("ComSat Body: " + targetbody.bodyName,
        //                       false, false),
        //                       new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(),
        //                       new DialogGUIButton("Next",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatBodyName++;
        //                           if (SaveInfo.comSatBodyName > 16 || SaveInfo.comSatBodyName < 1)
        //                           {
        //                               SaveInfo.comSatBodyName = 1;
        //                           }
        //                           ComSatContract();
        //                       }
        //                       , 100f, 30.0f, false), new DialogGUIButton("Previous",
        //                       delegate
        //                       {
        //                           SaveInfo.comSatBodyName--;
        //                           if (SaveInfo.comSatBodyName < 1 || SaveInfo.comSatBodyName > 16)
        //                           {
        //                               SaveInfo.comSatBodyName = 1;
        //                           }
        //                           ComSatContract();
        //                       }
        //                       , 100f, 30.0f, false)),

        //                       new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUILabel("Contract Name: ",
        //                       false, false), new DialogGUITextInput(ComSatName, true, 100, delegate
        //                       {
        //                           SaveInfo.ComSatContractName = ComSatName;
        //                           ComSatContract();
        //                           return SaveInfo.ComSatContractName;
        //                       }, 30f)),


        //                       new DialogGUIButton("Exit", () => { }, 200f, 30.0f, true)

        //                       )),
        //               false,
        //               HighLogic.UISkin);


        //}

        internal void KillMCePopups()
        {
            PopupDialog.ClearPopUps();
        }
    }
}
