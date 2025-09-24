using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using MissionControllerEC;
using System.Collections.Generic;
using KSP.Localization;
using System.Reflection;
using KSP.UI.Screens;
using System.Text;
using System.IO;
using KSP;
using static MissionControllerEC.RegisterToolbar;


namespace MissionControllerEC
{

    public partial class MissionControllerEC
    {

        #region Lets Build Space Station
        internal void BuildSpaceStation()
        {
            int targetbodyNum = FlightGlobals.Bodies.Count;
            targetbody = FlightGlobals.Bodies[SaveInfo.BuildSpaceStationIDX];

            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () =>
            {
                SaveInfo.GUIEnabled = true;
                if (bodySelWin != null)
                {
                    Destroy(bodySelWin); bodySelWin = null;
                }
            }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button10 = new DialogGUIButton(Localizer.Format(TRANSMIT),
                               delegate
                               {
                                   SaveInfo.BuildSpaceStationOn = !SaveInfo.BuildSpaceStationOn;
                                   SaveInfo.CustomBuildStationWinPos = new Vector2(
                                   ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.BuildSpaceStationName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   BuildSpaceStation();

                               }, delegate { return SaveInfo.BuildSpaceStationValid; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);

            Custom_Contract_Button10b = new DialogGUIButton(Localizer.Format(TRANSMIT_EXIT),
                               delegate
                               {
                                   SaveInfo.BuildSpaceStationOn = !SaveInfo.BuildSpaceStationOn;
                                   SaveInfo.CustomBuildStationWinPos = new Vector2(
                                   ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.BuildSpaceStationName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   SaveInfo.GUIEnabled = true;

                                   //BuildSpaceStation();

                               }, delegate { return SaveInfo.BuildSpaceStationValid; }, Contract_Button_Large_W, Contract_Button_Large_H, true, MCEGuiElements.ButtonPressMeToWorkStyle);

            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Previous"),
                delegate
                {
                    SaveInfo.BuildSpaceStationIDX--;
                    if (SaveInfo.BuildSpaceStationIDX < 1 || SaveInfo.BuildSpaceStationIDX > targetbodyNum)
                    {
                        SaveInfo.BuildSpaceStationIDX = 1;
                    }
                    SaveInfo.CustomBuildStationWinPos = new Vector2(
                    ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                    BuildSpaceStation();
                },
                Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);


            Custom_Contract_Button10a = new DialogGUIButton(Localizer.Format("Selection"),
                  delegate
                  {
                      if (bodySelWin == null)
                      {
                          BodySelection.StartBodySelection(BuildSpaceStation, buildSpaceStation: true);

                          SaveInfo.CustomBuildStationWinPos = new Vector2(
                          ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                          ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                          BuildSpaceStation();
                      }
                      else
                      {
                          Destroy(bodySelWin);
                          bodySelWin = null;
                          BuildSpaceStation();
                      }
                  },
                  Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);



            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
                delegate
                {
                    SaveInfo.BuildSpaceStationIDX++;
                    if (SaveInfo.BuildSpaceStationIDX < 1 || SaveInfo.BuildSpaceStationIDX > targetbodyNum)
                    {
                        SaveInfo.BuildSpaceStationIDX = 1;
                    }
                    SaveInfo.CustomBuildStationWinPos = new Vector2(
                    ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                    BuildSpaceStation();
                },
            Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("++"),
               delegate
               {
                   SaveInfo.BuildSpaceStationCrewAmount++;
                   SaveInfo.CustomBuildStationWinPos = new Vector2(
                    ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                   BuildSpaceStation();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("--"),
              delegate
              {
                  SaveInfo.BuildSpaceStationCrewAmount = Math.Max(1, SaveInfo.BuildSpaceStationCrewAmount - 1);
                  SaveInfo.CustomBuildStationWinPos = new Vector2(
                    ((Screen.width / 2) + BuildSpaceStatPop_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + BuildSpaceStatPop_dialg.RTrf.position.y) / Screen.height);
                  BuildSpaceStation();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);


            Custom_Contract_GuiBox7 = new DialogGUIBox("Contract Transmitted = " + SaveInfo.BuildSpaceStationOn.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox("Planet Station Will Be Located At? ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(" " + targetbody.bodyName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox("How Many Crew will Station Hold?" + " = " + SaveInfo.BuildSpaceStationCrewAmount.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox("Contract Description", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox("Contract Title", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);

            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.BuildSpaceStationName,
                                false,
                                260,
                                (string s) =>
                                {
                                    SaveInfo.BuildSpaceStationName = s;
                                    return s;
                                }, 30f);
            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.BuildSpaceStationDesc,
                                true,
                                350,
                                (string s) =>
                                {
                                    SaveInfo.BuildSpaceStationDesc = s;
                                    return s;
                                }, 120f);

            if (SaveInfo.CustomBuildStationWinPos.x <= 0 || SaveInfo.CustomBuildStationWinPos.y <= 0)
                SaveInfo.CustomBuildStationWinPos = new Vector2(0.5f, 0.5f);

            BuildSpaceStation_Dialg = new MultiOptionDialog("Lets Build Space Station", "", Localizer.Format("Lets Build A Space Station"), MCEGuiElements.MissionControllerSkin,
                new Rect(SaveInfo.CustomBuildStationWinPos.x, SaveInfo.CustomBuildStationWinPos.y, 315f, 500f),
                new DialogGUIBase[]
                {
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button10a,Custom_Contract_Button3),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button4, Custom_Contract_Button5),
                    new DialogGUISpace(4f),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIVerticalLayout(Custom_Contract_Input2),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox9),
                    new DialogGUIVerticalLayout(Custom_Contract_Input),
                    new DialogGUISpace(4f),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button10),
                    new DialogGUIVerticalLayout(Custom_Contract_Button10b),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)

                });

            BuildSpaceStatPop_dialg = PopupDialog.SpawnPopupDialog(BuildSpaceStation_Dialg, true, HighLogic.UISkin, false);

        }
        #endregion

   
    }
}
