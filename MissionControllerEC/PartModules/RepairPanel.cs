using System;
using UnityEngine;
using System.Collections.Generic;

namespace MissionControllerEC.PartModules
{
    class RepairPanel : PartModule
    {
        [KSPField]
        string DoorAnimation = "mceanim";

        [KSPField(isPersistant = false)]
        public static bool repair = false;        

        [KSPField(isPersistant = true)]
        public double currentRepair = 1;

        [KSPField(isPersistant = true)]
        public static string vesselId = "Test";

        [KSPField(isPersistant = true)]
        public static string vesselName = "TestName";

        [KSPField(isPersistant = false)]
        public double repairRate = 1;

        public bool startrepair = false;
        
        public Animation GetDeployDoorAnim
        {
            get
            {
                return part.FindModelAnimators(DoorAnimation)[0];
            }
        }

        private void PlayOpenAnimation(int speed, float time)
        {
            print("Opening");
            GetDeployDoorAnim[DoorAnimation].speed = speed;           
            GetDeployDoorAnim[DoorAnimation].normalizedTime = time;
            GetDeployDoorAnim.Play(DoorAnimation);
        }
       
        public override void OnStart(PartModule.StartState state)
        {
            this.part.force_activate();            
        }

        [KSPField(isPersistant = true, guiActive = true, guiName = "Ready To Repair")]
        public bool readyRep = false;

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Engineer CheckSystems", active = false)]
        public void CheckSystems()
        {

            List<ProtoCrewMember> protoCrewMembers = FlightGlobals.ActiveVessel.GetVesselCrew();
            foreach (Experience.ExperienceEffect exp in protoCrewMembers[0].experienceTrait.Effects)
            {
                if (exp.ToString() == "Experience.Effects.RepairSkill")
                {
                    Debug.Log("Current kerbal is a Engineer you have passed");
                    readyRep = true;
                    vesselId = this.part.vessel.id.ToString();
                    vesselName = this.part.vessel.name;
                    Debug.LogError("Vessel Id For PartModule is " + vesselId + " Name is " + vesselName);
                    ScreenMessages.PostScreenMessage("Your engineer has Prepared the vessel for Repair Open the panel, Then conduct the repair", 5f);  
                    
                }
                else
                {
                    Debug.Log("Current kerbal is NOT an Engineer you don't pass... Bad boy!");
                    ScreenMessages.PostScreenMessage("You need an Engineer to fix this Vessel!", 5f);
                }
            }                                    
        }
      
        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Start Repairs", active = false)]
        public void EnableRepair()
        {
            if (readyRep && currentRepair > 0)
            {
                repair = true;
                Debug.Log("repairEnabled");
                ScreenMessages.PostScreenMessage("Your engineer has repaired this vessel.  Good job!", 5f);
                readyRep = false;
            }
            else { ScreenMessages.PostScreenMessage("You need an Engineer class kerbal to conduct this repair!", 5f); }
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Open Door", active = true, guiActiveEditor = true)]
        public void OpenDoor()
        {
            PlayOpenAnimation(1,0);
            Events["OpenDoor"].active = false;
            Events["EnableRepair"].active = true;
            Events["closeDoor"].active = true;
            Events["CheckSystems"].active = true;
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Close Door", active = false, guiActiveEditor = true)]
        public void closeDoor()
        {
            PlayOpenAnimation(-1, 1);
            Events["OpenDoor"].active = true;
            Events["EnableRepair"].active = false;
            Events["closeDoor"].active = false;
            Events["CheckSystems"].active = false;
        }       

        [KSPAction("Start repair")]
        public void ToggleAction(KSPActionParam param)
        {
            EnableRepair();
        }

        public override void OnFixedUpdate()
        {                      
        }        
    }
}
