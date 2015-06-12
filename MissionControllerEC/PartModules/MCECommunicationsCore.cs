using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControllerEC.PartModules
{
    class MCESatelliteCore : PartModule
    {       
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
      
        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Satellite Type: ")]
        public string satTypeDisplay = "Not Loaded";

        [KSPField(isPersistant = true, guiActive = true, guiName = "Module Type: ")]
        public string satModuleType = "Communications";

        [KSPField(isPersistant = true, guiActive = true, guiName = "Frequency: ")]
        public float frequencyDisplay = 1;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "Module Type"), UI_FloatRange(minValue = 1f, maxValue = 4f, stepIncrement = 1f)]
        public float moduleType = 1;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "Set Frequency"), UI_FloatRange(minValue = 1f, maxValue = 50f, stepIncrement = .5f)]
        public float frequencyModulation = 1;

        [KSPEvent(guiActive = true, guiName = "Start Data Linkup", active = true)]
        public void StartDataMCE()
        {
            if (!dataLocked && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                if (MCEParameters.satelliteCoreCheck.APReady && MCEParameters.satelliteCoreCheck.PEReady)
                {
                    ScreenMessages.PostScreenMessage("Sending Data Package, This Part core is now disabled and can't be used again");
                    Debug.Log("current sattype is: " + satTypeDisplay + " Current frequency is: " + frequencyDisplay + " Current module type is: " + moduleName);
                    dataStartup();
                    if (haveAnimation)
                    {
                        PlaySatelliteCoreAnimation(1, 0);
                        PlaySatelliteCoreAnimation(-1, 1);
                       
                    }
                }
                else
                {
                    ScreenMessages.PostScreenMessage("You have not yet reached your orbital goal to Send this data package yet.  Please Reach correct APA and PEA");
                }
               
            }
            else if (dataLocked)
            {
                ScreenMessages.PostScreenMessage("This data package has already been sent.  Only 1 data package can be activated per Module");
                Debug.Log("current sattype is: " + satTypeDisplay + " Current frequency is: " + frequencyDisplay + " Current module type is: " + moduleName);
            }
            else
            {
                ScreenMessages.PostScreenMessage("You have to be in orbit to Do a Data Startup");
                Debug.Log("current sattype is: " + satTypeDisplay + " Current frequency is: " + frequencyDisplay + " Current module type is: " + moduleName);
            }
        }
        
        MCEParameters.satelliteCoreCheck sc = new MCEParameters.satelliteCoreCheck(false);
        MCEParameters.GroundStationPostion gs = new MCEParameters.GroundStationPostion(0);

        public void dataStartup()
        {
            sc.SetBoolSatelliteCoreValue(true);
            sc.SetSatelliteCoreCheck(satTypeDisplay, moduleType, frequencyDisplay);           
            dataLocked = true;
            Debug.Log("DataStartup Fired in PartModule");
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
            gs.SetGroundStationCheck(frequencyDisplay);
        }
        public override void OnFixedUpdate()
        {
            frequencyDisplay = frequencyModulation;
            gs.SetGroundStationCheck(frequencyDisplay);
        }
    }
}
