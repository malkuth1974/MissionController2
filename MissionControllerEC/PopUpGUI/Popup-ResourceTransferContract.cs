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

        #region Resource Transfer Custom Contract
        internal void ResourceTransferContract()
        {
            getSupplyList(false);
            if (SupVes.Count == 0)
            {
                NoBasesStations();
                return;
            }

            SaveInfo.SupplyVesName = SupVes[count].vesselName;
            SaveInfo.SupplyVesId = SupVes[count].vesselId.ToString();
            SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;

            SaveInfo.ResourceName = settings.SupplyResourceList[prCount];
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { SaveInfo.GUIEnabled = true; }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button10 = new DialogGUIButton(Localizer.Format(TRANSMIT),
                               delegate
                               {
                                   SaveInfo.supplyContractOn = !SaveInfo.supplyContractOn;
                                   SaveInfo.CustomTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.SupplyContractName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   ResourceTransferContract();

                               }, delegate { return SaveInfo.ResourceSupplyValid; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);

            Custom_Contract_Button10a = new DialogGUIButton(Localizer.Format(TRANSMIT_EXIT),
                               delegate
                               {
                                   SaveInfo.supplyContractOn = !SaveInfo.supplyContractOn;
                                   SaveInfo.CustomTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                                   MessageSystem.Message m = new MessageSystem.Message("Contract Bid Has Been Entered", "Hello your contract " + SaveInfo.SupplyContractName + " has been given to the finance Committee.  We will get back to you as soon as possible.", MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ALERT);
                                   MessageSystem.Instance.AddMessage(m);
                                   SaveInfo.GUIEnabled = true;

                                   //ResourceTransferContract();

                               }, delegate { return SaveInfo.ResourceSupplyValid; }, Contract_Button_Large_W, Contract_Button_Large_H, true, MCEGuiElements.ButtonPressMeToWorkStyle);


            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Previous"),
                delegate
                {
                    count--;
                    if (count < 1 || count > (supplyCount - 1))
                    {
                        count = 0;
                    }
                    SaveInfo.CustomTransWindowPos = new Vector2(
                   ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                   ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                    ResourceTransferContract();
                },
                Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
                delegate
                {
                    count++;
                    if (count > (supplyCount - 1) || count < 1)
                    {
                        count = 0;
                    }
                    SaveInfo.CustomTransWindowPos = new Vector2(
                    ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                    ResourceTransferContract();
                },
            Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Previous"),
               delegate
               {
                   prCount--;
                   if (prCount < 0 || prCount >= DictCount)
                   {
                       prCount = 0;
                   }
                   SaveInfo.CustomTransWindowPos = new Vector2(
                 ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                 ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                   ResourceTransferContract();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
              delegate
              {
                  prCount++;
                  if (prCount >= DictCount || prCount < 0)
                  {
                      prCount = 0;
                  }
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  ResourceTransferContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format(">"),
              delegate
              {

                  SaveInfo.supplyAmount = SaveInfo.supplyAmount + 100;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  ResourceTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format(">>"),
              delegate
              {
                  SaveInfo.supplyAmount = SaveInfo.supplyAmount + 1000;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  ResourceTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("<"),
              delegate
              {
                  SaveInfo.supplyAmount = SaveInfo.supplyAmount - 100;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  ResourceTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("<<"),
              delegate
              {
                  SaveInfo.supplyAmount = SaveInfo.supplyAmount - 1000;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  ResourceTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);

            Custom_Contract_GuiBox1 = new DialogGUIBox("Contract Transmitted:    " + SaveInfo.supplyContractOn.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox("Station To Supply ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(SaveInfo.SupplyVesName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox("Resources To Suppy ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(SaveInfo.ResourceName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox("Amount Resources   " + SaveInfo.ResourceName + "  [" + SaveInfo.supplyAmount.ToString() + "]", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox("Target Body Location   " + ":" + FlightGlobals.Bodies[SaveInfo.SupplyBodyIDX].bodyName, MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox("Contract Name", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox("Contract Description", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.SupplyContractName,
                                false,
                                200,
                                (string s) =>
                                {
                                    SaveInfo.SupplyContractName = s;
                                    return s;
                                }, 30f);
            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.ResourceTransferConDesc,
                                true,
                                350,
                                (string s) =>
                                {
                                    SaveInfo.ResourceTransferConDesc = s;
                                    return s;
                                }, 120f);

            if (SaveInfo.CustomTransWindowPos.x <= 0 || SaveInfo.CustomTransWindowPos.y <= 0)
                SaveInfo.CustomTransWindowPos = new Vector2(0.5f, 0.5f);

            CSupplyMulti_Dialog = new MultiOptionDialog("ComSat", "", Localizer.Format("#autoLOC_MCE_SupplyContract_Custom_Title"), MCEGuiElements.MissionControllerSkin,
                new Rect(SaveInfo.CustomTransWindowPos.x, SaveInfo.CustomTransWindowPos.y, 315f, 500f),
                new DialogGUIBase[]
                {

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox9),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button4,Custom_Contract_Button5),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7,Custom_Contract_Button8,Custom_Contract_Button9),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_GuiBox5),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox7),
                    new DialogGUIHorizontalLayout(Custom_Contract_Input2),
                     new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6),
                    new DialogGUIHorizontalLayout(Custom_Contract_Input),
                    new DialogGUISpace(4f),
                    //new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1),                   
                    new DialogGUIVerticalLayout(Custom_Contract_Button10),
                    new DialogGUIVerticalLayout(Custom_Contract_Button10a),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)
                });

            customSupPop_dialg = PopupDialog.SpawnPopupDialog(CSupplyMulti_Dialog, true, HighLogic.UISkin, false);

        }
        #endregion
    }
}
