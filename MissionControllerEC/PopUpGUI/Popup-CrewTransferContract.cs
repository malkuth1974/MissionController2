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

        #region Crew Transfers Custom Contracts
        internal void CrewTransferContract()
        {
            int MaxTouristInContract = 6;
            getSupplyList(false);
            if (SupVes.Count == 0)
            {
                NoBasesStations();
                return;
            }
            SaveInfo.crewVesName = SupVes[count].vesselName;
            SaveInfo.crewVesid = SupVes[count].vesselId.ToString();
            SaveInfo.crewBodyIDX = SupVes[count].body.flightGlobalsIndex;
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
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

                                   SaveInfo.crewContractOn = !SaveInfo.crewContractOn;
                                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract d" + SaveInfo.crewTransferName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   CrewTransferContract();

                               }, delegate { return SaveInfo.CrewTransferValid; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);

            Custom_Contract_Button10a = new DialogGUIButton(Localizer.Format(TRANSMIT_EXIT),
                               delegate
                               {

                                   SaveInfo.crewContractOn = !SaveInfo.crewContractOn;
                                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract d" + SaveInfo.crewTransferName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   SaveInfo.GUIEnabled = true;

                                   //CrewTransferContract();

                               }, delegate { return SaveInfo.CrewTransferValid; }, Contract_Button_Large_W, Contract_Button_Large_H, true, MCEGuiElements.ButtonPressMeToWorkStyle);


            Custom_Contract_Toggle2 = new DialogGUIToggleButton(false, ("Select To Allow Tourist On Mission"),
                               delegate (bool c)
                               {
                                   SaveInfo.transferTouristTrue = !SaveInfo.transferTouristTrue;
                                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                                   CrewTransferContract();

                               }, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Previous"),
                delegate
                {
                    count--;
                    if (count < 1 || count > (supplyCount - 1))
                    {
                        count = 0;
                    }
                    SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                    CrewTransferContract();
                },
                 delegate { return true; }, Contract_Button_Med_W, Contract_Button_Med_H, false);
            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
                delegate
                {
                    count++;
                    if (count > (supplyCount - 1) || count < 1)
                    {
                        count = 0;
                    }
                    SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                    CrewTransferContract();
                },
             delegate { return true; }, Contract_Button_Med_W, Contract_Button_Med_H, false);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("+"),
               delegate
               {
                   SaveInfo.crewAmount++;
                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                   CrewTransferContract();
               },
                delegate { return true; }, Contract_Button_Med_W, Contract_Button_Med_H, false);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("-"),
              delegate
              {
                  if (SaveInfo.transferTouristTrue)
                      SaveInfo.crewAmount = Math.Max(0, SaveInfo.crewAmount - 1);
                  else
                      SaveInfo.crewAmount = Math.Max(1, SaveInfo.crewAmount - 1);
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
               delegate { return true; }, Contract_Button_Med_W, Contract_Button_Med_H, false);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Add_A_Day"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime + 21600;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
               delegate { return true; }, Contract_Button_Small_W, Contract_Button_Small_H, false);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Add_A_Month"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime + 648000;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
               delegate { return true; }, Contract_Button_Small_W, Contract_Button_Small_H, false);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Min_A_Day"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime - 21600;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                    ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                    ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
               delegate { return true; }, Contract_Button_Small_W, Contract_Button_Small_H, false);
            Custom_Contract_Button11 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Min_A_Month"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime - 648000;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
               delegate { return true; }, Contract_Button_Small_W, Contract_Button_Small_H, false);
            Custom_Contract_Button12 = new DialogGUIButton(Localizer.Format("++"),
               delegate
               {

                   SaveInfo.transferTouristAmount++;
                   if (SaveInfo.transferTouristAmount > MaxTouristInContract)
                   { SaveInfo.transferTouristAmount = 0; }
                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                    ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                   CrewTransferContract();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button13 = new DialogGUIButton(Localizer.Format("--"),
              delegate
              {
                  SaveInfo.transferTouristAmount = Math.Max(0, SaveInfo.transferTouristAmount - 1);
                  if (SaveInfo.transferTouristAmount > MaxTouristInContract)
                  { SaveInfo.transferTouristAmount = 0; }
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                    ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);

            Custom_Contract_GuiBox1 = new DialogGUIBox("Contract Has Been Transfered " + SaveInfo.crewContractOn.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox("Station Transfer: ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(SaveInfo.crewVesName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox10 = new DialogGUIBox("Contract Description", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox("Amount Of KSC Crew To Transfer: " + SaveInfo.crewAmount.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(" " + FlightGlobals.Bodies[SaveInfo.crewBodyIDX].bodyName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox("How Many Tourist = " + SaveInfo.transferTouristAmount, MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox("Mission Time At Station: ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox11 = new DialogGUIBox(" " + Tools.formatTime(SaveInfo.crewTime), MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox12 = new DialogGUIBox("Kerbal Tourist Allowed = " + SaveInfo.transferTouristTrue.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox("Station Is Located At: " + "   ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox("Contract Name", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.crewTransferName,
                                false,
                                200,
                                (string s) =>
                                {
                                    SaveInfo.crewTransferName = s;
                                    return s;
                                }, 30f);

            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.TransferCrewDesc,
                                true,
                                350,
                                (string s) =>
                                {
                                    SaveInfo.TransferCrewDesc = s;
                                    return s;
                                }, 120f);

            if (SaveInfo.CustomCrewTransWindowPos.x <= 0 || SaveInfo.CustomCrewTransWindowPos.y <= 0)
                SaveInfo.CustomCrewTransWindowPos = new Vector2(0.5f, 0.5f);

            CCrewMulti_Dialog = new MultiOptionDialog("CrewTrans", "", Localizer.Format("#autoLOC_MCE_Title_Custom_Crew_Transfer_100"), HighLogic.UISkin,
                new Rect(SaveInfo.CustomCrewTransWindowPos.x, SaveInfo.CustomCrewTransWindowPos.y, 315f, 500f),
                new DialogGUIBase[]
                {
                    new DialogGUIVerticalLayout(
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button4,Custom_Contract_Button5),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox11),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7,Custom_Contract_Button8,Custom_Contract_Button11),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox12),
                    new DialogGUIHorizontalLayout(Custom_Contract_Toggle2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox7),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button12,Custom_Contract_Button13),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox9),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox10),
                    new DialogGUIHorizontalLayout(Custom_Contract_Input2),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6),
                    new DialogGUIHorizontalLayout(Custom_Contract_Input),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button10),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button10a),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button1))
                });

            customCrewPop_Dialg = PopupDialog.SpawnPopupDialog(CCrewMulti_Dialog, true, HighLogic.UISkin, false);
        }
        #endregion

    }
}
