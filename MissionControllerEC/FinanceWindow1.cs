using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;
using Contracts;
using Contracts.Parameters;
using System.Text.RegularExpressions;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        # region initializers
        public bool comSatwin = false;
        public bool supplywin = false;
        public bool crewwin = false;
        public int count = 0;
        public int prCount = 0;
        private int DictCount;
        private string resourceAmountString;
        #endregion        
        #region EditorWindow Ship Values
        public void drawEditorwindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            MCE_ScenarioStartup.scrollPosition = GUILayout.BeginScrollView(MCE_ScenarioStartup.scrollPosition, GUILayout.Width(408));
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
        #endregion
        #region finance Window
        public void drawFinanceWind(int id)
        {            
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.Box(MCE_ScenarioStartup.mainWindowTitle + " " + MCE_ScenarioStartup.versionCode, GUILayout.Width(400));

            GUILayout.EndHorizontal();
                           
            GUILayout.EndVertical();
            if (settings.DebugMenu && GUILayout.Button("Debug Menu"))
            {
               MCE_ScenarioStartup.ShowMainWindow = true;
            }
            if (GUILayout.Button("Custom Contracts"))
            {
                MCE_ScenarioStartup.ShowCustomWindow = true;
                getSupplyList(false);
                DictCount = settings.SupplyResourceList.Count();
                SaveInfo.ResourceName = settings.SupplyResourceList[prCount];              
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
        # endregion
        # region Custom GUI Window
        public void drawCustomGUI(int id)
        {
            CelestialBody targetbody = null;
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];           
           
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            comSatwin = GUILayout.Toggle(comSatwin, "Edit ComSat Contract Values");
            supplywin = GUILayout.Toggle(supplywin, "Edit Supply Contract Values");
            crewwin = GUILayout.Toggle(crewwin, "Edit Crew Transfer Contract Values");
            
            #region ComSatValues
            if (comSatwin)
            {
                supplywin = false;
                crewwin = false;
                GUILayout.Label("ComSat Network Information",MCE_ScenarioStartup.styleGreenBold);

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Network Contracts On", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + SaveInfo.ComSateContractOn,MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Turn On Contracts"))
                {
                    SaveInfo.ComSateContractOn = true;
                }
                if (GUILayout.Button("Turn Off Contracts"))
                {
                    SaveInfo.ComSateContractOn = false;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Max Orbital Period", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + Tools.ConvertMinsHours(SaveInfo.comSatmaxOrbital), MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("- Hour", GUILayout.Width(75))) { SaveInfo.comSatmaxOrbital -= 3600; }
                if (GUILayout.Button("+ Hour", GUILayout.Width(75))) { SaveInfo.comSatmaxOrbital += 3600; }
                if (GUILayout.Button("- Minute", GUILayout.Width(75))) { SaveInfo.comSatmaxOrbital -= 60; }
                if (GUILayout.Button("+ Minute", GUILayout.Width(75))) { SaveInfo.comSatmaxOrbital += 60; }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Min Orbital Period", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + Tools.ConvertMinsHours(SaveInfo.comSatminOrbital), MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("- Hour", GUILayout.Width(75))) { SaveInfo.comSatminOrbital -= 3600; }
                if (GUILayout.Button("+ Hour", GUILayout.Width(75))) { SaveInfo.comSatminOrbital += 3600; }
                if (GUILayout.Button("- Minute", GUILayout.Width(75))) { SaveInfo.comSatminOrbital -= 60; }
                if (GUILayout.Button("+ Minute", GUILayout.Width(75))) { SaveInfo.comSatminOrbital += 60; }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Body", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + targetbody.theName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("- Previous"))
                {
                    SaveInfo.comSatBodyName--;
                    if (SaveInfo.comSatBodyName < 1 || SaveInfo.comSatBodyName > 16)
                    {
                        SaveInfo.comSatBodyName = 1;
                    }
                }

                if (GUILayout.Button("+ Next"))
                {
                    SaveInfo.comSatBodyName++;
                    if (SaveInfo.comSatBodyName > 16 || SaveInfo.comSatBodyName < 1)
                    {
                        SaveInfo.comSatBodyName = 1;
                    }

                }

                GUILayout.EndHorizontal();
                
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Contract Name", MCE_ScenarioStartup.StyleBold, GUILayout.Width(150), GUILayout.Height(30));
                SaveInfo.ComSatContractName = GUILayout.TextField(SaveInfo.ComSatContractName, 50);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
            #endregion
            #region Supply Contract Values
            if (supplywin)
            {               
                comSatwin = false;
                crewwin = false;
                GUILayout.Label("Supply Contract Information", MCE_ScenarioStartup.styleGreenBold);

                if (supplyCount > 0)
                {
                    SaveInfo.SupplyVesName = SupVes[count].vesselName; 
                    SaveInfo.SupplyVesId = SupVes[count].vesselId.ToString();
                    SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;
                                      
                    SaveInfo.ResourceName = settings.SupplyResourceList[prCount];

                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Supply Contracts On", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.supplyContractOn, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Turn On Contracts"))
                    {
                        SaveInfo.supplyContractOn = true;
                    }
                    if (GUILayout.Button("Turn Off Contracts"))
                    {
                        SaveInfo.supplyContractOn = false;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Select Vessel To Supply", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.SupplyVesName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("- Previous"))
                    {
                        count--;
                        if (count < 1 || count > (supplyCount - 1))
                        {
                            count = 0;
                        }
                    }
                    if (GUILayout.Button("+ Next"))
                    {
                        count++;
                        if (count > (supplyCount - 1) || count < 1)
                        {
                            count = 0;
                        }
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);


                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Select Resource To Supply", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.ResourceName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("- Previous"))
                    {
                        Debug.Log("current list count: " + prCount + " Total Count of List is: " + DictCount);
                        prCount--;
                        if (prCount < 0 || prCount >= DictCount)
                        {
                            prCount = 0;                           
                        }
                    }

                    if (GUILayout.Button("+ Next"))
                    {
                        Debug.Log("current list count: " + prCount + " Total Count of List is: " + DictCount);
                        prCount++;                       
                        if (prCount >= DictCount || prCount < 0)
                        {
                            prCount = 0;                           
                        }
                    }
                    GUILayout.Space(15);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Set Amount Of " + SaveInfo.ResourceName, MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
                    resourceAmountString = SaveInfo.supplyAmount.ToString();
                    resourceAmountString = Regex.Replace(GUILayout.TextField(resourceAmountString), "[^.0-9]", "");
                    SaveInfo.supplyAmount = double.Parse(resourceAmountString);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Vessel Is Located At Body", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + FlightGlobals.Bodies[SaveInfo.SupplyBodyIDX].theName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Contract Name", MCE_ScenarioStartup.StyleBold, GUILayout.Width(150), GUILayout.Height(30));
                    SaveInfo.SupplyContractName = GUILayout.TextField(SaveInfo.SupplyContractName, 50);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                }
                else
                    GUILayout.Label("You have no Vessels of Type Station or Base in space This contract Only works On those Types Vessels!",MCE_ScenarioStartup.StyleBold);

            }
            # endregion
            #region Crew Transfer Values
            if (crewwin)
            {
                comSatwin = false;
                supplywin = false;
                GUILayout.Label("Crew Transfer Information", MCE_ScenarioStartup.styleGreenBold);

                if (supplyCount > 0)
                {
                    SaveInfo.crewVesName = SupVes[count].vesselName;
                    SaveInfo.crewVesid = SupVes[count].vesselId.ToString();
                    SaveInfo.crewBodyIDX = SupVes[count].body.flightGlobalsIndex;
                   
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Crew Transfer Contracts On", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.crewContractOn, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Turn On Contracts"))
                    {
                        SaveInfo.crewContractOn = true;
                    }
                    if (GUILayout.Button("Turn Off Contracts"))
                    {
                        SaveInfo.crewContractOn = false;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Station/Base Transfer Crew To:", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.crewVesName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("- Previous"))
                    {
                        count--;
                        if (count < 1 || count > (supplyCount - 1))
                        {
                            count = 0;
                        }
                    }
                    if (GUILayout.Button("+ Next"))
                    {
                        count++;
                        if (count > (supplyCount - 1) || count < 1)
                        {
                            count = 0;
                        }
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);


                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Set Amount Crew To Transfer", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.crewAmount, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("- Crew"))
                    {
                        SaveInfo.crewAmount --;                       
                    }

                    if (GUILayout.Button("+ Crew"))
                    {
                        SaveInfo.crewAmount ++;                      
                    }
                    GUILayout.Space(15);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Set Amount of Time Crew At Station?", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + Tools.formatTime(SaveInfo.crewTime), MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("- 1 Hour"))
                    {
                        SaveInfo.crewTime = SaveInfo.crewTime - 3600;
                    }
                    if (GUILayout.Button("+ 1 Hour"))
                    {
                        SaveInfo.crewTime = SaveInfo.crewTime + 3600;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("- 1 Day"))
                    {
                        SaveInfo.crewTime = SaveInfo.crewTime - 21600;
                    }

                    if (GUILayout.Button("+ 1 Day"))
                    {
                        SaveInfo.crewTime = SaveInfo.crewTime + 21600;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("- 1 Month"))
                    {
                        SaveInfo.crewTime = SaveInfo.crewTime - 648000;
                    }

                    if (GUILayout.Button("+ 1 Month"))
                    {
                        SaveInfo.crewTime = SaveInfo.crewTime + 648000;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Station/Base Is Located At Body", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + FlightGlobals.Bodies[SaveInfo.crewBodyIDX].theName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Contract Name", MCE_ScenarioStartup.StyleBold, GUILayout.Width(150), GUILayout.Height(30));
                    SaveInfo.crewTransferName = GUILayout.TextField(SaveInfo.crewTransferName, 50);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                }
                else
                    GUILayout.Label("You have no Vessels of Type Station or Base in space This contract Only works On those Types Vessels!", MCE_ScenarioStartup.StyleBold);

            }
            # endregion

            GUILayout.EndVertical();
            if (GUILayout.Button("Exit Window"))
            {
                MCE_ScenarioStartup.ShowCustomWindow = false;              
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        #endregion
    }   
}
