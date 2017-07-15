using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using KSP.Localization;
using UnityEngine;
using System.Text.RegularExpressions;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        CelestialBody targetbody = null;
        public static bool comSatwin = false;
        public static bool supplywin = false;
        public static bool crewwin = false;
        public int count = 0;
        public int prCount = 0;
        private int DictCount;
        private string resourceAmountString;

        double revertcost = HighLogic.CurrentGame.Parameters.CustomParams<IntergratedSettings3>().MCERevertCost;
                         
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
        /// <summary>
        /// All Custom contracts are written like MainGUi. They are not Hdden though!! This Makes wrinting and placing the GUI much easier.  Its also easier to write more than one window by having a template.  
        /// All buttons are pulled
        /// from MissionControllerEC.cs.  All contract windows are dismissed which allow you to resuse the Buttons and boxes.. Etc. You can't use the same buttons if another window is using them.. Causing bad
        /// things.
        /// </summary>
        internal void ComSatContract()
        {
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
           
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Toggle1 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_ComsatNetConButton"),
                               delegate (bool b)
                               {
                                   SaveInfo.ComSateContractOn = !SaveInfo.ComSateContractOn;
                                   SaveInfo.CustomSatWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSatPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSatPop_dialg.RTrf.position.y) / Screen.height);
                                   ComSatContract();

                               }, Contract_Button_Large_W, Contract_Button_Large_H);
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

            Custom_Contract_GuiBox1 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ComsatOrbitLabel"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox(Tools.ConvertMinsHours(SaveInfo.comSatmaxOrbital), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ComSatMinOrbitalPeriod"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(Tools.ConvertMinsHours(SaveInfo.comSatminOrbital), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ComSatBodyLabel"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(targetbody.bodyName, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ComsatNetConLabel"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox10 = new DialogGUIBox(SaveInfo.ComSateContractOn.ToString(), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ContractNameLabel"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.ComSatContractName,
                                false,
                                200,
                                (string s) => {
                                    SaveInfo.ComSatContractName = s;
                                    return s;
                                }, 30f);

            if (SaveInfo.CustomSatWindowPos.x <= 0 || SaveInfo.CustomSatWindowPos.y <= 0)
                SaveInfo.CustomSatWindowPos = new Vector2(0.5f, 0.5f);

            CSatmulti_dialog = new MultiOptionDialog("ComSat", "", Localizer.Format("#autoLOC_MCE_ComSatContTitleLabel"), HighLogic.UISkin,
                new Rect(SaveInfo.CustomSatWindowPos.x, SaveInfo.CustomSatWindowPos.y, 310f, 60f),
                new DialogGUIBase[]
                {
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4,Custom_Contract_GuiBox10),
                    new DialogGUIVerticalLayout(Custom_Contract_Toggle1),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1,Custom_Contract_GuiBox7),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3,Custom_Contract_Button4,Custom_Contract_Button5),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2,Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7,Custom_Contract_Button8,Custom_Contract_Button9),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3,Custom_Contract_GuiBox9),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button10,Custom_Contract_Button11),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6,Custom_Contract_Input),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)
                });

            customSatPop_dialg = PopupDialog.SpawnPopupDialog(CSatmulti_dialog, true, HighLogic.UISkin, false);
        }
        internal void TransferContract()
        {
            getSupplyList(false);
            SaveInfo.SupplyVesName = SupVes[count].vesselName;
            SaveInfo.SupplyVesId = SupVes[count].vesselId.ToString();
            SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;

            SaveInfo.ResourceName = settings.SupplyResourceList[prCount];
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Toggle1 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Supply_Contracts_On_Button"),
                               delegate (bool b)
                               {
                                   SaveInfo.supplyContractOn = !SaveInfo.supplyContractOn;

                                   SaveInfo.CustomTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customSupPop_dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customSupPop_dialg.RTrf.position.y) / Screen.height);
                                   TransferContract();

                               }, Contract_Button_Large_W, Contract_Button_Large_H);
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

            Custom_Contract_GuiBox1 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Supply_Contracts_On_Label"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox(SaveInfo.supplyContractOn.ToString(), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Select_Vessel_To_Supply_Label"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(SaveInfo.SupplyVesName, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Select_Resource_To_Supply_Label"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(SaveInfo.ResourceName, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Set_Amount_Of_Label"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox10 = new DialogGUIBox(SaveInfo.ResourceName + "  [" + SaveInfo.supplyAmount.ToString() + "]", Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Vessel_Is_Located_At_Body_Label") + ":" + FlightGlobals.Bodies[SaveInfo.SupplyBodyIDX].bodyName, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ContractNameLabel"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.SupplyContractName,
                                false,
                                200,
                                (string s) => {
                                    SaveInfo.SupplyContractName = s;
                                    return s;
                                }, 30f);

            if (SaveInfo.CustomTransWindowPos.x <= 0 || SaveInfo.CustomTransWindowPos.y <= 0)
                SaveInfo.CustomTransWindowPos = new Vector2(0.5f, 0.5f);

            CSupplyMulti_Dialog = new MultiOptionDialog("ComSat", "", Localizer.Format("#autoLOC_MCE_SupplyContract_Custom_Title"), HighLogic.UISkin,
                new Rect(SaveInfo.CustomTransWindowPos.x, SaveInfo.CustomTransWindowPos.y, 310f, 60f),
                new DialogGUIBase[]
                {
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1,Custom_Contract_GuiBox7),
                    new DialogGUIVerticalLayout(Custom_Contract_Toggle1),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2,Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3,Custom_Contract_GuiBox9),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button4,Custom_Contract_Button5),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4,Custom_Contract_GuiBox10),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7,Custom_Contract_Button8,Custom_Contract_Button9),

                    new DialogGUIVerticalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6,Custom_Contract_Input),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)                   
                });

            customSupPop_dialg = PopupDialog.SpawnPopupDialog(CSupplyMulti_Dialog, true, HighLogic.UISkin, false);

        }
        internal void CrewTransferContract()
        {
            getSupplyList(false);
            SaveInfo.crewVesName = SupVes[count].vesselName;
            SaveInfo.crewVesid = SupVes[count].vesselId.ToString();
            SaveInfo.crewBodyIDX = SupVes[count].body.flightGlobalsIndex;
            targetbody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { }, Contract_Button_Large_W, Contract_Button_Large_H, true);
            Custom_Contract_Toggle1 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_ButtonCrew_Transfer_Contracts_On"),
                               delegate (bool b)
                               {

                                   SaveInfo.crewContractOn = !SaveInfo.crewContractOn;
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
                Contract_Button_Med_W, Contract_Button_Med_H, true);
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
            Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button4 = new DialogGUIButton(Localizer.Format("+"),
               delegate
               {
                   SaveInfo.crewAmount++;
                   SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                   CrewTransferContract();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button5 = new DialogGUIButton(Localizer.Format("-"),
              delegate
              {
                  SaveInfo.crewAmount--;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
              Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button6 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Add_A_Day"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime + 21600;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Add_A_Month"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime + 648000;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Min_A_Day"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime - 21600;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                    ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                    ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_ButtonCrew_Min_A_Month"),
              delegate
              {
                  SaveInfo.crewTime = SaveInfo.crewTime - 648000;
                  SaveInfo.CustomCrewTransWindowPos = new Vector2(
                                   ((Screen.width / 2) + customCrewPop_Dialg.RTrf.position.x) / Screen.width,
                                   ((Screen.height / 2) + customCrewPop_Dialg.RTrf.position.y) / Screen.height);
                  CrewTransferContract();
              },
              Contract_Button_Small_W, Contract_Button_Small_H, true);

            Custom_Contract_GuiBox1 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_LabelCrew_Transfer_Contracts_On"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox7 = new DialogGUIBox(SaveInfo.crewContractOn.ToString(), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox2 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ButtonCrew_Station_Base_Transfer_Crew_To"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox8 = new DialogGUIBox(SaveInfo.crewVesName, Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox3 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Set_Amount_Crew To_Transfer"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox9 = new DialogGUIBox(SaveInfo.crewAmount.ToString(), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox4 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Set_Amount_of_Time_Crew_At_Station"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox10 = new DialogGUIBox(Tools.formatTime(SaveInfo.crewTime), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_GuiBox5 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_Label_Station_Base_Is_Located_At_Body") + ":   " + FlightGlobals.Bodies[SaveInfo.crewBodyIDX].bodyName, Contract_Button_Large_W, Contract_Button_Large_H);
            Custom_Contract_GuiBox6 = new DialogGUIBox(Localizer.Format("#autoLOC_MCE_ContractNameLabel"), Contract_Button_Med_W, Contract_Button_Med_H);
            Custom_Contract_Input = new DialogGUITextInput(SaveInfo.crewTransferName,
                                false,
                                200,
                                (string s) => {
                                    SaveInfo.crewTransferName = s;
                                    return s;
                                }, 30f);

            if (SaveInfo.CustomCrewTransWindowPos.x <= 0 || SaveInfo.CustomCrewTransWindowPos.y <= 0)
                SaveInfo.CustomCrewTransWindowPos = new Vector2(0.5f, 0.5f);

            CCrewMulti_Dialog = new MultiOptionDialog("CrewTrans", "", Localizer.Format("#autoLOC_MCE_Title_Custom_Crew_Transfer_100"), HighLogic.UISkin,
                new Rect(SaveInfo.CustomCrewTransWindowPos.x, SaveInfo.CustomCrewTransWindowPos.y, 310f, 60f),
                new DialogGUIBase[]
                {
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox1,Custom_Contract_GuiBox7),
                    new DialogGUIVerticalLayout(Custom_Contract_Toggle1),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox2,Custom_Contract_GuiBox8),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button2,Custom_Contract_Button3),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox3,Custom_Contract_GuiBox9),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button4,Custom_Contract_Button5),

                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox4,Custom_Contract_GuiBox10),
                    new DialogGUIHorizontalLayout(Custom_Contract_Button6,Custom_Contract_Button7,Custom_Contract_Button8,Custom_Contract_Button9),

                    new DialogGUIVerticalLayout(Custom_Contract_GuiBox5),
                    new DialogGUIHorizontalLayout(Custom_Contract_GuiBox6,Custom_Contract_Input),
                    new DialogGUIVerticalLayout(Custom_Contract_Button1)
                });

            customCrewPop_Dialg = PopupDialog.SpawnPopupDialog(CCrewMulti_Dialog, true, HighLogic.UISkin, false);
        }
        internal void DebugMenuMce()
        {          
            Custom_Contract_Button1 = new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Button_Exit_Label"), () => { }, Contract_Button_Large_W, Contract_Button_Large_H, true);
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
            Custom_Contract_Button7 = new DialogGUIButton(Localizer.Format("Blank"),
               delegate
               {

                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button8 = new DialogGUIButton(Localizer.Format("Blank"),
               delegate
               {

                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);
            Custom_Contract_Button9 = new DialogGUIButton(Localizer.Format("Blank"),
               delegate
               {

                   SaveInfo.DebugWindowPos = new Vector2(
                                ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                   DebugMenuMce();
               },
               Contract_Button_Med_W, Contract_Button_Med_H, true);


            Custom_Contract_Toggle1 = new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_SetAgena_False") + " 1",
                              delegate (bool b)
                              {
                                  SaveInfo.Agena1Done = !SaveInfo.Agena1Done;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

                              }, Contract_Button_Med_W, Contract_Button_Med_H);
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
                                  SaveInfo.Vostok1Done = !SaveInfo.Vostok2Done;
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
            Custom_Contract_Toggle8= new DialogGUIToggleButton(false, Localizer.Format("#autoLOC_MCE_Toggle_MCERepair_True"),
                              delegate (bool b)
                              {
                                  SaveInfo.RepairContractGeneratedOn = true; SaveInfo.RepairStationContractGeneratedOn = true;
                                  SaveInfo.DebugWindowPos = new Vector2(
                                  ((Screen.width / 2) + customDebug_Dialg.RTrf.position.x) / Screen.width,
                                  ((Screen.height / 2) + customDebug_Dialg.RTrf.position.y) / Screen.height);
                                  DebugMenuMce();

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
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle3,Custom_Contract_Toggle4),
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle5,Custom_Contract_Toggle6),
                   new DialogGUIHorizontalLayout(Custom_Contract_Toggle7,Custom_Contract_Toggle8),                  
                   new DialogGUIVerticalLayout(Custom_Contract_Button1)
                });

            customDebug_Dialg = PopupDialog.SpawnPopupDialog(DebugMulti_Dialg, true, HighLogic.UISkin, false);
        }
        internal void KillMCePopups()
        {
            PopupDialog.ClearPopUps();
        }
    }
}
