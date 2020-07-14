using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.Localization;

namespace MissionControllerEC.PartModules
{
    class MCESatelliteCore : PartModule
    {
         
        [KSPField(isPersistant = true, guiActive = true)]
        public static string[] SattypeList = { "Communication", "Navigation", "Weather", "Research" };

        [KSPField(isPersistant = true, guiActive = true)]
        public static int sattypenumber = 0;

        [KSPField(isPersistant = true,guiActive = true,guiName = "MC PartLocked")]
        private bool dataLocked = false;

        [KSPField]
        public bool haveAnimation = false;
        [KSPField]

        public string animationName = "None";
        
        public Animation GetSatelliteCoreAnimation
        {
            get
            {
                return part.FindModelAnimators(animationName)[0];
            }
        }

        private void PlaySatelliteCoreAnimation(int speed, float time)
        {
            Debug.Log("Running animation for MCESatelliteCore");
            GetSatelliteCoreAnimation[animationName].speed = speed;
            GetSatelliteCoreAnimation[animationName].normalizedTime = time;
            GetSatelliteCoreAnimation.Play(animationName);
        }
        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = " MC Satellite Type: ")]
        public string satTypeDisplay = SattypeList[sattypenumber];

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "MC Satellite Types: ")]
        public string satTypeDisplay2 = SattypeList[sattypenumber];

        [KSPEvent(guiActive = true,guiActiveEditor = true,guiName = "MC Satellite Type(Choose First)",active = true)]
        public void SatTypeSwitch()
        {
            if (sattypenumber > 2)
            {
                sattypenumber = 0;
                satTypeDisplay2 = SattypeList[sattypenumber];
                satTypeDisplay = SattypeList[sattypenumber];
            }
            else
            {
                sattypenumber++;
                Debug.Log("MCE Sat Number Now " + sattypenumber + "  " + SattypeList[sattypenumber]);
                satTypeDisplay2 = SattypeList[sattypenumber];
                satTypeDisplay = SattypeList[sattypenumber];
            }          
        }

        [KSPField(isPersistant = true, guiActive = true,guiActiveEditor = true, guiName = "MC Module Type: ")]
        public string satModuleType = "Select Module Type";

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Set Frequency"), UI_FloatRange(minValue = 1f, maxValue = 50f, stepIncrement = .5f)]
        public float frequencyModulation = 1;

        [KSPField(isPersistant = true, guiActive = true, guiName = "Frequency: ")]
        public float frequencyDisplay = 1;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "MC Module Type: ")]
        public float moduleType = 1;

        [KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "MC Push Set Frequency!", active = true)]
        public void FreqModSwitch()
        {
            MCEParameters.GroundStationPostion gs = new MCEParameters.GroundStationPostion(0);
            frequencyDisplay = frequencyModulation;
            gs.SetGroundStationCheck(frequencyDisplay);
        }

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "MC Module Type(Choose Second)", active = true)]
        public void SatModSwitch()
        {
            if (moduleType > 3)
            {
                moduleType = 0;
                if (satTypeDisplay == "Communication")
                {
                    ModuleTypeSwitch();
                }
                if (satTypeDisplay == "Navigation")
                {
                    ModuleTypeSwitch2();
                }
                if (satTypeDisplay == "Weather")
                {
                    ModuleTypeSwitch3();
                }
                if (satTypeDisplay == "Research")
                {
                    ModuleTypeSwitch4();
                }
            }
            else
            {
                moduleType++;
                Debug.Log("MCE Module Number Now " + moduleType + "  " + satModuleType[(int)moduleType]);
                if (satTypeDisplay == "Communication")
                {
                    ModuleTypeSwitch();
                }
                if (satTypeDisplay == "Navigation")
                {
                    ModuleTypeSwitch2();
                }
                if (satTypeDisplay == "Weather")
                {
                    ModuleTypeSwitch3();
                }
                if (satTypeDisplay == "Research")
                {
                    ModuleTypeSwitch4();
                }
            }
            
        }
      
        [KSPEvent(guiActive = true, guiIcon = "MC Start Data Linkup", guiName = "Start Data Linkup (MC Contracts)", active = true)]
        public void StartDataMCE()
        {
            if (!dataLocked && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000241"));		// #autoLOC_MissionController2_1000241 = Sending Data Package, This Part core is now disabled and can't be used again
                //Debug.Log("current sattype is: " + satTypeDisplay + " Current frequency is: " + frequencyDisplay + " Current module type is: " + moduleName);
                dataStartup();
                if (haveAnimation)
                {
                    PlaySatelliteCoreAnimation(1, 0);
                    PlaySatelliteCoreAnimation(-1, 1);

                }              
            }
            else if (dataLocked)
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000242"));		// #autoLOC_MissionController2_1000242 = This data package has already been sent.  Only 1 data package can be activated per Module
                //Debug.Log("current sattype is: " + satTypeDisplay + " Current frequency is: " + frequencyDisplay + " Current module type is: " + moduleName);
            }
            else
            {
                ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_MissionController2_1000243"));		// #autoLOC_MissionController2_1000243 = You have to be in orbit to Do a Data Startup
                //Debug.Log("current sattype is: " + satTypeDisplay + " Current frequency is: " + frequencyDisplay + " Current module type is: " + moduleName);
            }
        }
        
        public void dataStartup()
        {
            MCEParameters.satelliteCoreCheck sc = new MCEParameters.satelliteCoreCheck(false);           
            sc.SetBoolSatelliteCoreValue(true);
            sc.SetSatelliteCoreCheck(satTypeDisplay, moduleType, frequencyDisplay);           
            dataLocked = true;
            //Debug.Log("DataStartup Fired in PartModule");
        }
        private void ModuleTypeSwitch()
        {
            switch ((int)moduleType)
            {
                case 1:
                    satModuleType = "Tv Networks (1)";
                    break;
                case 2:
                    satModuleType = "Phone Systems (2)";
                    break;
                case 3:
                    satModuleType = "Goverment(3)";
                    break;
                case 4:
                    satModuleType = "Deep Space (4)";
                    break;
                default:
                    satModuleType = "Tv Networks (1)";
                    break;
            }
        }
        private void ModuleTypeSwitch2()
        {
            switch ((int)moduleType)
            {
                case 1:
                    satModuleType = "Civilian GPS(1)";
                    break;
                case 2:
                    satModuleType = "Goverment GPS(2)";
                    break;
                case 3:
                    satModuleType = "Maritime GPS(3)";
                    break;
                case 4:
                    satModuleType = "UAV Operations(4)";
                    break;
                default:
                    satModuleType = "Corperate (4)";
                    break;
            }
        }
        private void ModuleTypeSwitch3()
        {
            switch ((int)moduleType)
            {
                case 1:
                    satModuleType = "Visible spectrum(1)";
                    break;
                case 2:
                    satModuleType = "Infrared spectrum(2)";
                    break;
                case 3:
                    satModuleType = "Radiation(3)";
                    break;
                case 4:
                    satModuleType = "Radar(4)";
                    break;
                default:
                    satModuleType = "Atmospheric Sounder(4)";
                    break;
            }
        }
        private void ModuleTypeSwitch4()
        {
            switch ((int)moduleType)
            {
                case 1:
                    satModuleType = "Atmoshperic Studies(1)";
                    break;
                case 2:
                    satModuleType = "Radiation Studies(2)";
                    break;
                case 3:
                    satModuleType = "Zero-G Studies(3)";
                    break;
                case 4:
                    satModuleType = "Bacterial(4)";
                    break;
                default:
                    satModuleType = "Live Specimen(4)";
                    break;
            }
        }
             
        public override void OnStart(PartModule.StartState state)
        {
            MCEParameters.GroundStationPostion gs = new MCEParameters.GroundStationPostion(0);
            if (satTypeDisplay == "Communication")
            {
                ModuleTypeSwitch();              
            }
            else if (satTypeDisplay == "Navigation")
            {
                ModuleTypeSwitch2();
            }
            else if (satTypeDisplay == "Weather")
            {
                ModuleTypeSwitch3();
            }
            else if (satTypeDisplay == "Research")
            {
                ModuleTypeSwitch4();
            }
            frequencyDisplay = frequencyModulation;
            satTypeDisplay = SattypeList[sattypenumber];
            satTypeDisplay2 = SattypeList[sattypenumber];
            gs.SetGroundStationCheck(frequencyDisplay);            
        }
        public override void OnFixedUpdate()
        {         
            MCEParameters.GroundStationPostion gs = new MCEParameters.GroundStationPostion(0);
            if (satTypeDisplay == "Communication")
            {
                ModuleTypeSwitch();
            }
            else if (satTypeDisplay == "Navigation")
            {
                ModuleTypeSwitch2();
            }
            else if (satTypeDisplay == "Weather")
            {
                ModuleTypeSwitch3();
            }
            else if (satTypeDisplay == "Research")
            {
                ModuleTypeSwitch4();
            }
            frequencyDisplay = frequencyModulation;
            satTypeDisplay = SattypeList[sattypenumber];
            satTypeDisplay2 = SattypeList[sattypenumber];
            gs.SetGroundStationCheck(frequencyDisplay);
        }        
        
    }
}
