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

        #region Landing or Orbit Custom Contract
        internal void LandingOrbitCustomContract()
        {
            //getSupplyList(false);            
            //SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;
            int MaxTouristInContract = 6;
            int targetbodyNum = FlightGlobals.Bodies.Count();
            targetbody = FlightGlobals.Bodies[SaveInfo.LandingOrbitIDX];

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
                                   SaveInfo.OrbitLandingOn = !SaveInfo.OrbitLandingOn;
                                   SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                                   ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.LandingOrbitName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   LandingOrbitCustomContract();

                               }, delegate { return SaveInfo.LandingOrbitValid; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);
            Custom_Contract_Button10a = new DialogGUIButton(Localizer.Format(TRANSMIT_EXIT),
                               delegate
                               {
                                   SaveInfo.OrbitLandingOn = !SaveInfo.OrbitLandingOn;
                                   SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                                   ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.LandingOrbitName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   SaveInfo.GUIEnabled = true;

                                   //LandingOrbitCustomContract();

                               }, delegate { return SaveInfo.LandingOrbitValid; }, Contract_Button_Large_W, Contract_Button_Large_H, true, MCEGuiElements.ButtonPressMeToWorkStyle);

            Custom_Contract_Toggle2 = new DialogGUIToggleButton(false, ("Select To Allow Tourist On Mission"),
                               delegate (bool c)
                               {
                                   SaveInfo.OrbitAllowCivs = !SaveInfo.OrbitAllowCivs;
                                   SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                                   ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                                   LandingOrbitCustomContract();

                               }, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Previous"),
                delegate
                {
                    SaveInfo.LandingOrbitIDX--;
                    if (SaveInfo.LandingOrbitIDX < 1 || SaveInfo.LandingOrbitIDX > targetbodyNum)
                    {
                        SaveInfo.LandingOrbitIDX = 1;
                    }
                    SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                    LandingOrbitCustomContract();
                },
                Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);

            Custom_Contract_Button3a = new DialogGUIButton(Localizer.Format("Selection"),
              delegate
              {
                  if (bodySelWin == null)
                  {
                      BodySelection.StartBodySelection(LandingOrbitCustomContract, landingOrbit: true);

                      SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                      ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                      ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                      LandingOrbitCustomContract();
                  }
                  else
                  {
                      Destroy(bodySelWin);
                      bodySelWin = null;
                      LandingOrbitCustomContract();
                  }
              },
              Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);


            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
                delegate
                {
                    SaveInfo.LandingOrbitIDX++;
                    if (SaveInfo.LandingOrbitIDX < 1 || SaveInfo.LandingOrbitIDX > targetbodyNum)
                    {
                        SaveInfo.LandingOrbitIDX = 1;
                    }
                    SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                    LandingOrbitCustomContract();
                },
            Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("++"),
               delegate
               {
                   SaveInfo.LandingOrbitCrew++;
                   SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                   LandingOrbitCustomContract();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("--"),
              delegate
              {
                  if (SaveInfo.OrbitAllowCivs)
                      SaveInfo.LandingOrbitCrew = Math.Max(0, SaveInfo.LandingOrbitCrew - 1);
                  else
                      SaveInfo.LandingOrbitCrew = Math.Max(1, SaveInfo.LandingOrbitCrew - 1);
                  SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                  LandingOrbitCustomContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("++"),
               delegate
               {
                   SaveInfo.LandingOrbitCivilians++;
                   if (SaveInfo.LandingOrbitCivilians > MaxTouristInContract)
                   { SaveInfo.LandingOrbitCivilians = 0; }
                   SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                   LandingOrbitCustomContract();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("--"),
              delegate
              {
                  SaveInfo.LandingOrbitCivilians = Math.Max(0, SaveInfo.LandingOrbitCivilians - 1);
                  if (SaveInfo.LandingOrbitCivilians > MaxTouristInContract)
                  { SaveInfo.LandingOrbitCivilians = 0; }
                  SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                  LandingOrbitCustomContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);

            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonLandingOrbitSet1"),
              delegate
              {
                  SaveInfo.IsOrbitOrLanding = true;
                  SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                   ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                   ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                  LandingOrbitCustomContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonLandingOrbitSet2"),
              delegate
              {
                  SaveInfo.IsOrbitOrLanding = false;
                  SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                   ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                   ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                  LandingOrbitCustomContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);

            String tempOrbitLand;
            if (SaveInfo.IsOrbitOrLanding)
            {
                tempOrbitLand = (Localizer.Format("#autoLOC_MCE_Label_For_LandingOrbitContract_Orbit"));
            }
            else
            {
                tempOrbitLand = (Localizer.Format("#autoLOC_MCE_label_for_LandingOrbitContract_Landing"));
            }

            Custom_Contract_GuiBox1 = new DialogGUIBox("How Many Tourist = " + SaveInfo.LandingOrbitCivilians, MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox("Contract Transmitted = " + SaveInfo.OrbitLandingOn.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox("Planet You Want To Travel Too = ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(" " + targetbody.bodyName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox("How Many KSC Crew On Mission" + " = " + SaveInfo.LandingOrbitCrew.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox("Type Orbit Or Landing = ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(" " + tempOrbitLand, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox("Contract Description", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox("Contract Title", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox12 = new DialogGUIBox("Kerbal Tourist Allowed = " + SaveInfo.OrbitAllowCivs.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);

            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.LandingOrbitName,
                                false,
                                260,
                                (string s) =>
                                {
                                    SaveInfo.LandingOrbitName = s;
                                    return s;
                                }, 30f);
            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.LandingOrbitDesc,
                                true,
                                350,
                                (string s) =>
                                {
                                    SaveInfo.LandingOrbitDesc = s;
                                    return s;
                                }, 120f);

            if (SaveInfo.CustomLandingOrbitWinPos.x <= 0 || SaveInfo.CustomLandingOrbitWinPos.y <= 0)
                SaveInfo.CustomLandingOrbitWinPos = new Vector2(0.5f, 0.5f);

            LandOrbitMulti_Dialg = new MultiOptionDialog("CustomLandOrbit", "", Localizer.Format("Contract Builder Orbits Or Landing"), MCEGuiElements.MissionControllerSkin,
                new Rect(SaveInfo.CustomLandingOrbitWinPos.x, SaveInfo.CustomLandingOrbitWinPos.y, 315f, 500f),
                new DialogGUIBase[]
                {
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3a, Custom_Contract_Button3),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button4,Custom_Contract_Button5),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox12),
                    new DialogGUIVerticalLayout(Custom_Contract_Toggle2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button9,Custom_Contract_Button8),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIVerticalLayout(Custom_Contract_Input2),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox9),
                    new DialogGUIVerticalLayout(Custom_Contract_Input),
                    new DialogGUISpace(4f),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button10),
                    new DialogGUIVerticalLayout(Custom_Contract_Button10a),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)

                });

            customLandOrbit_dialg = PopupDialog.SpawnPopupDialog(LandOrbitMulti_Dialg, true, HighLogic.UISkin, false);

        }
        #endregion

    }
}
