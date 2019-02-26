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
        public static string[] SattypeList = { "Communication", "Navigation", "Weather", "Research" };
        [KSPField(isPersistant = true,guiActive = true,guiName = "PartLocked")]
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
        
        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "Satellite Type: ")]
        public string satTypeDisplay = "Communications";

        [KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "Satellite Types: ")]
        public string satTypeDisplay2 = "Com=0,Nav=1,Weather=2,Research=3";

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "Choose Satellite Type"), UI_FloatRange(minValue = 0f, maxValue = 3f, stepIncrement = 1f)]
        public float SatTypeListNumber = 0;

        [KSPField(isPersistant = true, guiActive = true, guiName = "Module Type: ")]
        public string satModuleType = "Communications";

        [KSPField(isPersistant = true, guiActive = true, guiName = "Frequency: ")]
        public float frequencyDisplay = 1;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "Module Type"), UI_FloatRange(minValue = 1f, maxValue = 4f, stepIncrement = 1f)]
        public float moduleType = 1;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "Set Frequency"), UI_FloatRange(minValue = 1f, maxValue = 50f, stepIncrement = .5f)]
        public float frequencyModulation = 1;

        [KSPEvent(guiActive = true, guiIcon = "Start Data Linkup", guiName = "Start Data Linkup", active = true)]
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
            satTypeDisplay = SattypeList[(int)SatTypeListNumber];
            gs.SetGroundStationCheck(frequencyDisplay);            
        }
        public override void OnFixedUpdate()
        {         
            MCEParameters.GroundStationPostion gs = new MCEParameters.GroundStationPostion(0);
            frequencyDisplay = frequencyModulation;
            satTypeDisplay = SattypeList[(int)SatTypeListNumber];
            gs.SetGroundStationCheck(frequencyDisplay);
        }        
        
    }
}
