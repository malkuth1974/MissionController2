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
        #region  variables
        CelestialBody targetbody = null;
        public static bool comSatwin = false;
        public static bool supplywin = false;
        public static bool crewwin = false;
        public static int tempOrbitNumber = 0;
        public static String tempOrbitLand = "Test";
        public int count = 0;
        public int prCount = 0;
        private int DictCount;
        //string KerbalName = "PlaceNameHere";

        double revertcost = HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCERevertCost;


        internal static BodySelection bodySelWin;

        const string TRANSMIT = "Transmit Contract To Finance Committee";
        const string TRANSMIT_EXIT = "Transmit Contract To Finance Committee & Exit";
        #endregion
        #region Revert Gui                
        /// <summary>
        /// RevertPress is my early version of a GUI. I kept it this way to show that this is about the easiest way to use PopupDialog version.  But can get very confusing.
        /// If your GUI is not too complicated than this is easiest way to write a short GUI popup.
        /// </summary>
        internal void RevertPress()
        {
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                if (FlightDriver.CanRevertToPrelaunch)
                {
                    GetRefundCost();
                    PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                       new MultiOptionDialog("MCERevert", "", Localizer.Format("#autoLOC_MCE_MissionControllerRevertLabel"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIVerticalLayout(
                                new DialogGUIFlexibleSpace(),
                                new DialogGUIButton(Localizer.Format("#autoLOC_MCE_RevertToVABCost:$") + Math.Round(revertcost * (RevertTotal + RevertAltitude + RevertOrbit) / 100, 0),
                                    delegate
                                    {
                                        toolbarControl_MCERevert.SetFalse();
                                        FlightDriver.RevertToPrelaunch(EditorFacility.VAB);
                                        //RevertHalt = true;
                                        RevertHaultSet();
                                    }, 200f, 30.0f, true),
                                new DialogGUIButton(Localizer.Format("#autoLOC_MCE_RRevertToSPHCost:$") + Math.Round(revertcost * (RevertTotal + RevertAltitude + RevertOrbit) / 100, 0),
                                    delegate
                                    {
                                        toolbarControl_MCERevert.SetFalse();
                                        FlightDriver.RevertToPrelaunch(EditorFacility.SPH);
                                        //RevertHalt = true; 
                                        RevertHaultSet();
                                    }, 200f, 30.0f, true),
                                new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Exit_Button_WithoutReverting"),
                                    delegate
                                    {
                                        toolbarControl_MCERevert.SetFalse();
                                    }, 200f, 30.0f, true)
                                )),
                        false,
                        HighLogic.UISkin);
                }
                else
                {
                    UnableToRevert();
                }
            }
            else
            {
                PopupDialog.ClearPopUps();
            }
        }

        internal double RevertCost { get { return Math.Round(revertcost * (RevertTotal + RevertAltitude + RevertOrbit) / 100, 0); } }
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
                               new DialogGUIButton(Localizer.Format("#autoLOC_MCE_Revert_Total_Cost_Final_Window") + RevertCost,
                               delegate
                               {
                                   Funding.Instance.AddFunds(-RevertCost, TransactionReasons.Progression);
                                   RevertHalt = false;
                                   SendRevertMessage();
                               }, 300f, 30.0f, true)
                               )),
                       false,
                       HighLogic.UISkin);
            }
        }

        internal void UnableToRevert()
        {
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                       new Vector2(0.5f, 0.5f),
                      new MultiOptionDialog("MCERevertSet", "",
                      Localizer.Format("#autoLOC_MCE_MissionControllerRevertLabel"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 210f, 60f),
                           new DialogGUIFlexibleSpace(),
                           new DialogGUIVerticalLayout(
                               new DialogGUIFlexibleSpace(),

                               new DialogGUIButton(Localizer.Format("Unable To Revert"),
                               delegate
                               {
                                   toolbarControl_MCERevert.SetFalse();
                               }, 300f, 30.0f, true)
                               )),
                       false,
                       HighLogic.UISkin);
            }
        }

        #endregion

        internal void NoBasesStations()
        {
            if (SupVes.Count == 0)
            {
                customCrewPop_Dialg = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                    new MultiOptionDialog("MCENoBasesVessels", "", Localizer.Format("Unable Create Contract"), HighLogic.UISkin, new Rect(0.5f, 0.5f, 330, 70f),
                    new DialogGUIFlexibleSpace(),
                    new DialogGUIVerticalLayout(
                    new DialogGUIFlexibleSpace(),
                    new DialogGUIHorizontalLayout(
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIButton(Localizer.Format("No Bases or Stations available"), delegate
                        {
                            return;
                        }, 300f, 50.0f, true),
                        new DialogGUIFlexibleSpace())
                )),
                false,
                HighLogic.UISkin);

            }

        }

        internal void KillMCePopups()
        {
            PopupDialog.ClearPopUps();
        }
    }
}
