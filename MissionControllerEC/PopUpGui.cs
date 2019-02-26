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



namespace MissionControllerEC
{
    
    public partial class MissionControllerEC
    {
        CelestialBody targetbody = null;
        public static bool comSatwin = false;
        public static bool supplywin = false;
        public static bool crewwin = false;
        public static int tempOrbitNumber = 0;
        public static String tempOrbitLand = "Test";
        public int count = 0;
        public int prCount = 0;
        private int DictCount;
        string KerbalName = "PlaceNameHere";

        double revertcost = HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCERevertCost;
        #region Revert Gui                
        /// <summary>
        /// RevertPress is my early version of a GUI. I kept it this way to show that this is about the easiest way to use PopupDialog version.  But can get very confusing.
        /// If your GUI is not too complicated than this is easiest way to write a short GUI popup.
        /// </summary>
        internal void RevertPress()
        {                             
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                   new MultiOptionDialog("MCERevert", "",Localizer.Format("#autoLOC_MCE_MissionControllerRevertLabel"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIVerticalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIButton(Localizer.Format("#autoLOC_MCE_RevertToVABCost:$") + Math.Round(revertcost * temp / 100, 0),
                                delegate
                                {                                   
                                    FlightDriver.RevertToPrelaunch(EditorFacility.VAB);
                                    //RevertHalt = true;
                                    RevertHaultSet();                                
                                }, 200f, 30.0f, true),
                            new DialogGUIButton(Localizer.Format("#autoLOC_MCE_RRevertToSPHCost:$") + Math.Round(revertcost * temp / 100,0),
                                delegate
                                {
                                    FlightDriver.RevertToPrelaunch(EditorFacility.SPH);
                                    //RevertHalt = true; 
                                    RevertHaultSet();
                                }, 200f, 30.0f, true),
                            new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Exit_Button_WithoutReverting"), () => { }, 200f, 30.0f, true)
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
                      new MultiOptionDialog("MCERevertSet", "", Localizer.Format("#autoLOC_MCE_MissionControllerRevertLabel"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                           new DialogGUIFlexibleSpace(),
                           new DialogGUIVerticalLayout(
                               new DialogGUIFlexibleSpace(),
                               new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Revert_Total_Cost_Final_Window") + Math.Round(revertcost * temp / 100, 0), delegate
                               {
                                   Funding.Instance.AddFunds(- Math.Round(revertcost * temp / 100, 0), TransactionReasons.Progression);
                                   RevertHalt = false;
                               }, 200f, 30.0f, true)
                               )),
                       false,
                       HighLogic.UISkin);
            }
        }
        #endregion

        #region Comsat Custom Contract GUI
        /// <summary>
        /// All Custom contracts are written like MainGUi. They are not Hdden though!! This Makes wrinting and placing the GUI much easier.  Its also easier to write more than one window by having a template.  
        /// All buttons are pulled
        /// from MissionControllerEC.cs.  All contract windows are dismissed which allow you to resuse the Buttons and boxes.. Etc. You can't use the same buttons if another window is using them.. Causing bad
        /// things.
        /// </summary>
        internal void ComSatContract()
        {
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
           
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { SaveInfo.GUIEnabled = true;}, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button12 = new DialogGUIButton(Localizer.Format("Transmit Contract To Mission Control"),
                               delegate
                               {
                                   SaveInfo.ComSateContractOn = !SaveInfo.ComSateContractOn;
                                   SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                                   ComSatContract();

                               }, delegate { return true; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);
            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Hour-"),
                delegate
                {
                    SaveInfo.comSatmaxOrbital -= 3600;
                    SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                    ComSatContract();
                },
                Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Hour+"),
                delegate
                {
                    SaveInfo.comSatmaxOrbital += 3600;
                    SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                    ComSatContract();
                },
            Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Min-"),
               delegate
               {
                   SaveInfo.comSatmaxOrbital -= 60;
                   SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                   ComSatContract();
               },
               Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Min+"),
              delegate
              {
                  SaveInfo.comSatmaxOrbital += 60;
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Hour-"),
              delegate
              {
                  SaveInfo.comSatminOrbital -= 3600;
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Hour+"),
              delegate
              {
                  SaveInfo.comSatminOrbital += 3600;
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Min-"),
              delegate
              {
                  SaveInfo.comSatminOrbital -= 60;
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Min+"),
              delegate
              {
                  SaveInfo.comSatminOrbital += 60;
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
                  if (SaveInfo.comSatBodyName < 1 || SaveInfo.comSatBodyName > 16)
                  {
                      SaveInfo.comSatBodyName = 1;
                  }
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button11 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
              delegate
              {
                  SaveInfo.comSatBodyName++;
                  if (SaveInfo.comSatBodyName < 1 || SaveInfo.comSatBodyName > 16)
                  {
                      SaveInfo.comSatBodyName = 1;
                  }
                  SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                  ComSatContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);

            Custom_Contract_GuiBox1 = new DialogGUIBox("Select Your Max Orbital Period", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox(Tools.ConvertMinsHours(SaveInfo.comSatmaxOrbital), MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox("Select Your Min Orbital Period", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(Tools.ConvertMinsHours(SaveInfo.comSatminOrbital), MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox("Target Body Location: ", MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(" " + targetbody.bodyName, MCEGuiElements.DescripStyle2, Contract_Button_Large_W, Contract_Button_Med_H);           
            Custom_Contract_GuiBox4 = new DialogGUIBox("Contract Been Transmitted         " + SaveInfo.ComSateContractOn.ToString(), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ContractNameLabel"), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox(Localizer.Format("Contract Description"), MCEGuiElements.DescripStyle, Contract_Button_Large_W, Contract_Button_Med_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.ComSatContractName,
                                false,
                                200,
                                (string s) => {
                                    SaveInfo.ComSatContractName = s;
                                    return s;
                                }, 30f);

            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.SatelliteConDesc,
                                true,
                                350,
                                (string s) => {
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
                    new DialogGUIHorizontalLayout(Custom_Contract_Button10,Custom_Contract_Button11),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIVerticalLayout(Custom_Contract_Input2),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6),
                    new DialogGUIHorizontalLayout(Custom_Contract_Input),                   
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4),
                    new DialogGUIVerticalLayout(Custom_Contract_Button12),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)
                });

            customSatPop_dialg = PopupDialog.SpawnPopupDialog(CSatmulti_dialog, true, HighLogic.UISkin, false);
        }
        #endregion

        #region Resource Transfer Custom Contract
        internal void TransferContract()
        {
            getSupplyList(false);
            SaveInfo.SupplyVesName = SupVes[count].vesselName;
            SaveInfo.SupplyVesId = SupVes[count].vesselId.ToString();
            SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;

            SaveInfo.ResourceName = settings.SupplyResourceList[prCount];
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { SaveInfo.GUIEnabled = true;}, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button10 = new DialogGUIButton("Transmit Contract To Mission Control",
                               delegate
                               {
                                   SaveInfo.supplyContractOn = !SaveInfo.supplyContractOn;

                                   SaveInfo.CustomTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                                   TransferContract();

                               }, delegate { return true; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);
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
                                    TransferContract();
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
                                    TransferContract();
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
                                    TransferContract();
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
                                    TransferContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format(">"),
              delegate
              {

                  SaveInfo.supplyAmount = SaveInfo.supplyAmount + 100;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  TransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format(">>"),
              delegate
              {
                  SaveInfo.supplyAmount = SaveInfo.supplyAmount + 1000;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  TransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("<"),
              delegate
              {
                  SaveInfo.supplyAmount = SaveInfo.supplyAmount - 100;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  TransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);         
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("<<"),
              delegate
              {
                  SaveInfo.supplyAmount = SaveInfo.supplyAmount - 1000;
                  SaveInfo.CustomTransWindowPos = new Vector2(
                ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                  TransferContract();
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
                                (string s) => {
                                    SaveInfo.SupplyContractName = s;
                                    return s;
                                }, 30f);
            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.ResourceTransferConDesc,
                                true,
                                350,
                                (string s) => {
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
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1),                   
                    new DialogGUIVerticalLayout(Custom_Contract_Button10),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)                   
                });

            customSupPop_dialg = PopupDialog.SpawnPopupDialog(CSupplyMulti_Dialog, true, HighLogic.UISkin, false);

        }
        #endregion

        #region Crew Transfers Custom Contracts
        internal void CrewTransferContract()
        {
            int MaxTouristInContract = 6;
            getSupplyList(false);
            SaveInfo.crewVesName = SupVes[count].vesselName;
            SaveInfo.crewVesid = SupVes[count].vesselId.ToString();
            SaveInfo.crewBodyIDX = SupVes[count].body.flightGlobalsIndex;
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { SaveInfo.GUIEnabled = true;}, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button10 = new DialogGUIButton(Localizer.Format("Transfer Contract To Mission Control"),
                               delegate
                               {

                                   SaveInfo.crewContractOn = !SaveInfo.crewContractOn;
                                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                                   CrewTransferContract();

                               }, delegate { return true; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);
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
                  SaveInfo.crewAmount--;
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
                  SaveInfo.transferTouristAmount--;
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
                                (string s) => {
                                    SaveInfo.crewTransferName = s;
                                    return s;
                                }, 30f);

            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.TransferCrewDesc,
                                true,
                                350,
                                (string s) => {
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
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button10),
                    new DialogGUISpace(4f),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button1))
                });

            customCrewPop_Dialg = PopupDialog.SpawnPopupDialog(CCrewMulti_Dialog, true, HighLogic.UISkin, false);
        }
        #endregion

        #region Bebug Gui
        internal void DebugMenuMce()
        {          
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { SaveInfo.GUIEnabled = true;}, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button2 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Cheat_Money"),
                delegate
                {
                    Funding.Instance.AddFunds(500000, TransactionReasons.Cheating);
                    SaveInfo.DebugWindowPos = new Vector2(
                                 ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                 ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                    DebugMenuMce();
                },
                Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Cheat_Science"),
                delegate
                {
                    ResearchAndDevelopment.Instance.AddScience(1000, TransactionReasons.Cheating);
                    SaveInfo.DebugWindowPos = new Vector2(
                                 ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                 ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                    DebugMenuMce();
                },
                Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_SetAgena_Vessel_Current"),
                delegate
                {
                    SaveInfo.AgenaTargetVesselName = FlightGlobals.ActiveVessel.vesselName;
                    SaveInfo.AgenaTargetVesselID = FlightGlobals.ActiveVessel.id.ToString();
                    SaveInfo.DebugWindowPos = new Vector2(
                                 ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                 ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                    DebugMenuMce();
                },
                Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Set_SkyLab_Current_Selected_Vessel"),
               delegate
               {
                   SaveInfo.skyLabName = FlightGlobals.ActiveVessel.vesselName;
                   SaveInfo.skyLabVesID = FlightGlobals.ActiveVessel.id.ToString();
                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Set_Landing_Site_for_Apollo_Missions"),
               delegate
               {
                   GetLatandLonDefault(FlightGlobals.ActiveVessel);
                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("Tiros = " + SaveInfo.tirosCurrentNumber),
               delegate
               {
                   SaveInfo.tirosCurrentNumber++;
                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("Tiros = " + SaveInfo.tirosCurrentNumber),
               delegate
               {
                   SaveInfo.tirosCurrentNumber--;
                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("Delete All MCE Named Tourist"),
               delegate
               {
                   Tools.CivilianGoHome();
                   Tools.CivilianGoHome2();
                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            label_1 = new DialogGUILabel(assemblyName + versionCode);


            Custom_Contract_Toggle1 = new DialogGUIToggleButton(false, Localizer.Format("Remove All Civilian Kerbals"/*"#autoLOC_MCE_Toggle_SetAgena_False"*/) + " 1",
                              delegate (bool b)
                              {
                                  Tools.DebugCivilianGoHome(KerbalName);
                                  //SaveInfo.Agena1Done = !SaveInfo.Agena1Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);

            Custom_Contract_Input = new DialogGUITextInput("",
                               false,
                               180,
                               (string s) => {
                                   KerbalName = s;
                                   return s;
                               }, 30f);

            Custom_Contract_Toggle2 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetAgena_False") + " 2",
                              delegate (bool b)
                              {
                                  SaveInfo.Agena2Done = !SaveInfo.Agena2Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Toggle3 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetVoskov_False") + "1",
                              delegate (bool b)
                              {
                                  SaveInfo.Vostok1Done = !SaveInfo.Vostok1Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Toggle4 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetVoskov_False") + "2",
                              delegate (bool b)
                              {
                                  SaveInfo.Vostok2Done = !SaveInfo.Vostok2Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Toggle5 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetVoskHod_False"),
                              delegate (bool b)
                              {
                                  SaveInfo.Voskhod2Done = !SaveInfo.Voskhod2Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Toggle6 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetLuna_False") + "2",
                              delegate (bool b)
                              {
                                  SaveInfo.Luna2Done = !SaveInfo.Luna2Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Toggle7= new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetLuna_False") + "3",
                              delegate (bool b)
                              {
                                  SaveInfo.Luna16Done = !SaveInfo.Luna16Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Toggle8= new DialogGUIToggleButton(false, Localizer.Format("Test Button"/*"#autoLOC_MCE_Toggle_MCERepair_True"*/),
                              delegate (bool b)
                              {
                                  
                                  
                                  //SaveInfo.RepairContractGeneratedOn = true; SaveInfo.RepairStationContractGeneratedOn = true;
                                  //SaveInfo.DebugWindowPos = new Vector2(
                                  //((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  //((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  //DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);

            //Custom_Contract_Input = new DialogGUITextInput(SaveInfo.SupplyContractName,
            //                    false,
            //                    200,
            //                    (string s) => {
            //                        SaveInfo.tirosCurrentNumber = s;
            //                        return s;
            //                    }, 30f);
            //Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.SupplyContractName,
            //                    false,
            //                    200,
            //                    (string s) => {
            //                        SaveInfo.marinerCurrentNumber = s;
            //                        return s;
            //                    }, 30f);
            //Custom_Contract_Input3 = new DialogGUITextInput(SaveInfo.SupplyContractName,
            //                    false,
            //                    200,
            //                    (string s) => {
            //                        SaveInfo.apolloCurrentNumber = s;
            //                        return s;
            //                    }, 30f);
            //Custom_Contract_Input4 = new DialogGUITextInput(SaveInfo.SupplyContractName,
            //                    false,
            //                    200,
            //                    (string s) => 
            //                    {
            //                        apolloNumber = SaveInfo.apolloCurrentNumber.ToString();
            //                        SaveInfo.apolloDunaCurrentNumber =  
            //                        return s;
            //                    }, 30f);

            Custom_Contract_GuiBox1 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Tiros_Number_1-3"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Mariner_Number_1-4"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Apollo_Number_1-6"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Apollo_Duna_Number_1-9"), Contract_Button_Med_W, Contract_Button_Med_H);

            if (SaveInfo.DebugWindowPos.x <= 0 || SaveInfo.DebugWindowPos.y <= 0)
                SaveInfo.DebugWindowPos = new Vector2(0.5f, 0.5f);

            DebugMulti_Dialg = new MultiOptionDialog("DebugM", "", Localizer.Format("#autoLOC_MCE_Label_BegugMenu_Window_Title"), HighLogic.UISkin,
                new Rect(SaveInfo.DebugWindowPos.x, SaveInfo.DebugWindowPos.y, 310f, 60f),
                new DialogGUIBase[]
                {
                   new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3),
                   new DialogGUIHorizontalLayout(Custom_Contract_Button4,Custom_Contract_Button5),
                   new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7),
                   new DialogGUIHorizontalLayout(Custom_Contract_Button8,Custom_Contract_Button9),
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle1,Custom_Contract_Toggle2),
                   new DialogGUIHorizontalLayout(Custom_Contract_Input),
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle3,Custom_Contract_Toggle4),
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle5,Custom_Contract_Toggle6),
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle7,Custom_Contract_Toggle8),                  
                   new DialogGUIVerticalLayout(Custom_Contract_Button1,label_1)
                });

            customDebug_Dialg = PopupDialog.SpawnPopupDialog(DebugMulti_Dialg, true, HighLogic.UISkin, false);
        }
        #endregion

        #region Landing or Orbit Custom Contract
        internal void LandingOrbitCustomContract()
        {
            //getSupplyList(false);            
            //SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;
            int MaxTouristInContract = 6;
            targetbody = FlightGlobals.Bodies[SaveInfo.LandingOrbitIDX];

            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { SaveInfo.GUIEnabled = true; }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Button10 = new DialogGUIButton(Localizer.Format("Transmit Contract To Mission Control"),
                               delegate
                               {
                                   SaveInfo.OrbitLandingOn = !SaveInfo.OrbitLandingOn;
                                   SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                                   ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                                   LandingOrbitCustomContract();

                               }, delegate { return true; }, Contract_Button_Large_W, Contract_Button_Large_H, false, MCEGuiElements.ButtonPressMeToWorkStyle);
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
                    if (SaveInfo.LandingOrbitIDX < 1 || SaveInfo.LandingOrbitIDX > 16)
                    {
                        SaveInfo.LandingOrbitIDX = 1;
                    }
                    SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                    LandingOrbitCustomContract();
                },
                Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button3 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Next"),
                delegate
                {
                    SaveInfo.LandingOrbitIDX++;
                    if (SaveInfo.LandingOrbitIDX < 1 || SaveInfo.LandingOrbitIDX > 16)
                    {
                        SaveInfo.LandingOrbitIDX = 1;
                    }
                    SaveInfo.CustomLandingOrbitWinPos = new Vector2(
                    ((Screen.width / 2) + customLandOrbit_dialg.RTrf.position.x) / Screen.width,
                    ((Screen.height / 2) + customLandOrbit_dialg.RTrf.position.y) / Screen.height);
                    LandingOrbitCustomContract();
                },
            Contract_Button_Med_W, Contract_Button_Med_H, true);
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
                  SaveInfo.LandingOrbitCrew--;
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
                  SaveInfo.LandingOrbitCivilians--;
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
                                (string s) => {
                                    SaveInfo.LandingOrbitName = s;
                                    return s;
                                }, 30f);
            Custom_Contract_Input2 = new DialogGUITextInput(SaveInfo.LandingOrbitDesc,
                                true,
                                350,
                                (string s) => {
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
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3),
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
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox7),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button10),
                    new DialogGUISpace(4f),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)

                });

            customLandOrbit_dialg = PopupDialog.SpawnPopupDialog(LandOrbitMulti_Dialg, true, HighLogic.UISkin, false);

        }
        #endregion
        internal void KillMCePopups()
        {
            PopupDialog.ClearPopUps();
        }
    }
}
