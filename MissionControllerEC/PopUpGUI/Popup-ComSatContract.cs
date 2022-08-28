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

        #region Comsat Custom Contract GUI
        /// <summary>
        /// All Custom contracts are written like MainGUi. They are not Hdden though!! This Makes printing and placing the GUI much easier.  Its also easier to write more than one window by having a template.  
        /// All buttons are pulled
        /// from MissionControllerEC.cs.  All contract windows are dismissed which allow you to resuse the Buttons and boxes.. Etc. You can't use the same buttons if another window is using them.. Causing bad
        /// things.
        /// </summary>
        internal void ComSatContract()
        {
            int targetbodyNum = FlightGlobals.Bodies.Count();
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            SaveInfo.comSatmaxOrbital = Math.Max(targetbody.atmosphereDepth + 1000, SaveInfo.comSatmaxOrbital);

            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () =>
            {
                SaveInfo.GUIEnabled = true;
                if (bodySelWin != null)
                {
                    Destroy(bodySelWin); bodySelWin = null;
                }
            }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button12 = new DialogGUIButton(Localizer.Format(TRANSMIT),
                               delegate
                               {
                                   //if (SaveInfo.ComSatContractOn == true) { SaveInfo.ComSatContractOn = false; }
                                   SaveInfo.ComSatContractOn = !SaveInfo.ComSatContractOn;
                                   SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.ComSatContractName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   ComSatContract();
                               }, delegate { return SaveInfo.ComSatValid; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);

            Custom_Contract_Button13 = new DialogGUIButton(Localizer.Format(TRANSMIT_EXIT),
                               () =>
                               {
                                   //if (SaveInfo.ComSatContractOn == true) { SaveInfo.ComSatContractOn = false; }
                                   SaveInfo.ComSatContractOn = !SaveInfo.ComSatContractOn;
                                   SaveInfo.CustomSatWindowPos = new Vector2(
                                       ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                       ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.ComSatContractName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   //ComSatContract();
                                   SaveInfo.GUIEnabled = true;
                                   if (bodySelWin != null)
                                   {
                                       Destroy(bodySelWin); bodySelWin = null;
                                   }
                               }, delegate { return SaveInfo.ComSatValid; }, Contract_Button_Large_W, Contract_Button_Large_H, true, MCEGuiElements.ButtonPressMeToWorkStyle);



            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("-10000 Meters"),
                delegate
                {
                    SaveInfo.comSatmaxOrbital = Math.Max(targetbody.atmosphereDepth, SaveInfo.comSatmaxOrbital - 10000);
                    SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                    ComSatContract();
                },
                Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("+10000 Meters"),
                delegate
                {
                    SaveInfo.comSatmaxOrbital += 10000;
                    SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                    ComSatContract();
                },
            Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("-1000 Meters"),
               delegate
               {
                   SaveInfo.comSatmaxOrbital = Math.Max(targetbody.atmosphereDepth, SaveInfo.comSatmaxOrbital - 1000);
                   SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                   ComSatContract();
               },
               Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("+1000 Meters"),
              delegate
              {
                  SaveInfo.comSatmaxOrbital += 1000;
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format("+10 Inclination"),
              delegate
              {
                  //SaveInfo.comSatminOrbital -= 10;

                  //if (SaveInfo.comSatminOrbital > 90)
                  //{ SaveInfo.comSatminOrbital = 90; }
                  //if (SaveInfo.comSatminOrbital < -90)
                  //{ SaveInfo.comSatminOrbital = -90; }
                  SaveInfo.comSatminOrbital = Math.Max(Math.Min(SaveInfo.comSatminOrbital - 10, 90), -90);
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                      ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                      ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("+10 Inclination"),
              delegate
              {
                  //SaveInfo.comSatminOrbital += 10;
                  //if (SaveInfo.comSatminOrbital > 90)
                  //{ SaveInfo.comSatminOrbital = 90; }
                  //if (SaveInfo.comSatminOrbital < -90)
                  //{ SaveInfo.comSatminOrbital = -90; }

                  SaveInfo.comSatminOrbital = Math.Max(Math.Min(SaveInfo.comSatminOrbital + 10, 90), -90);

                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("-1 Inclination"),
              delegate
              {
                  //SaveInfo.comSatminOrbital -= 1;
                  //if (SaveInfo.comSatminOrbital > 90)
                  //{ SaveInfo.comSatminOrbital = 90; }
                  //if (SaveInfo.comSatminOrbital < -90)
                  //{ SaveInfo.comSatminOrbital = -90; }

                  SaveInfo.comSatminOrbital = Math.Max(Math.Min(SaveInfo.comSatminOrbital - 1, 90), -90);

                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("+1 Inclination"),
              delegate
              {
                  //SaveInfo.comSatminOrbital += 1;
                  //if (SaveInfo.comSatminOrbital > 90)
                  //{ SaveInfo.comSatminOrbital = 90; }
                  //if (SaveInfo.comSatminOrbital < -90)
                  //{ SaveInfo.comSatminOrbital = -90; };

                  SaveInfo.comSatminOrbital = Math.Max(Math.Min(SaveInfo.comSatminOrbital + 1, 90), -90);

                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button10 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Previous"),
              delegate
              {
                  SaveInfo.comSatBodyName--;
                  if (SaveInfo.comSatBodyName < 0 || SaveInfo.comSatBodyName > targetbodyNum)
                  {
                      SaveInfo.comSatBodyName = 1;
                  }
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);


            Custom_Contract_Button10a = new DialogGUIButton(Localizer.Format("Selection"),
              delegate
              {
                  if (bodySelWin == null)
                  {
                      BodySelection.StartBodySelection(ComSatContract, comsat: true);

                      SaveInfo.CustomSatWindowPos = new Vector2(
                                  ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                      ComSatContract();
                  }
                  else
                  {
                      Destroy(bodySelWin);
                      bodySelWin = null;
                      ComSatContract();
                  }
              },
              Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);



            Custom_Contract_Button11 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
              delegate
              {
                  SaveInfo.comSatBodyName++;
                  if (SaveInfo.comSatBodyName < 0 || SaveInfo.comSatBodyName > targetbodyNum)
                  {
                      SaveInfo.comSatBodyName = 1;
                  }
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Med_W * 0.666f, Contract_Button_Med_H, true);

            Custom_Contract_GuiBox1 = new DialogGUIBox("Select Your Orbital Height", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox(SaveInfo.comSatmaxOrbital.ToString(), MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox("Select Your Inclination", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(SaveInfo.comSatminOrbital.ToString(), MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox("Target Body Location: ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(" " + targetbody.bodyName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox("Contract Been Transmitted         " + SaveInfo.ComSatContractOn.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ContractNameLabel"), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox(Localizer.Format("Contract Description"), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.ComSatContractName,
                                false,
                                200,
                                (string s) =>
                                {
                                    SaveInfo.ComSatContractName = s;
                                    return s;
                                }, 30f);

            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.SatelliteConDesc,
                                true,
                                350,
                                (string s) =>
                                {
                                    SaveInfo.SatelliteConDesc = s;
                                    return s;
                                }, 120f);

            if (SaveInfo.CustomSatWindowPos.x <= 0 || SaveInfo.CustomSatWindowPos.y <= 0)
                SaveInfo.CustomSatWindowPos = new Vector2(0.5f, 0.5f);

            CSatmulti_dialog = new MultiOptionDialog("ComSat", "", Localizer.Format("Satellite Orbit Builder Contract"), MCEGuiElements.MissionControllerSkin,
                new Rect(SaveInfo.CustomSatWindowPos.x, SaveInfo.CustomSatWindowPos.y, 315f, 500f),
                new DialogGUIBase[]
                {
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox7),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3,Custom_Contract_Button4,Custom_Contract_Button5),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7,Custom_Contract_Button8,Custom_Contract_Button9),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox9),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button10,Custom_Contract_Button10a,Custom_Contract_Button11),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIVerticalLayout(Custom_Contract_Input2),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6),
                    new DialogGUIHorizontalLayout(Custom_Contract_Input),
                    new DialogGUISpace(4f),
                    //new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4),
                    new DialogGUIVerticalLayout(Custom_Contract_Button12),
                    new DialogGUIVerticalLayout(Custom_Contract_Button13),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)
                });

            customSatPop_dialg = PopupDialog.SpawnPopupDialog(CSatmulti_dialog, true, HighLogic.UISkin, false);
        }
        #endregion

    }
}
