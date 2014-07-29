using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControllerEC
{
    class RepairPanel : PartModule
    {
        [KSPField]
        string DoorAnimation = "mceanim";

        [KSPField(isPersistant = false)]
        public static bool repair = false;        

        [KSPField(isPersistant = true)]
        public double currentRepair = 0;

        [KSPField(isPersistant = false)]
        public double repairRate = .1;
       
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
      
        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Start Repairs", active = false)]
        public void EnableRepair()
        {
            checkRepaired();
            Debug.Log("repairEnabled");
            if (repair)
                ScreenMessages.PostScreenMessage("Repairs Started and Finsished.  Good Job");
            else
                ScreenMessages.PostScreenMessage("Repair failed maker sure you have RepairParts inside the Repair Panel");
            
           
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Open Door",active = true)]
        public void OpenDoor()
        {
            PlayOpenAnimation(1,0);
            Events["OpenDoor"].active = false;
            Events["EnableRepair"].active = true;
            Events["closeDoor"].active = true;
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Close Door",active = false)]
        public void closeDoor()
        {
            PlayOpenAnimation(-1, 1);
            Events["OpenDoor"].active = true;
            Events["EnableRepair"].active = false;
            Events["closeDoor"].active = false;
        }       

        [KSPAction("Start repair")]
        public void ToggleAction(KSPActionParam param)
        {
            EnableRepair();
        }

        public void checkRepaired()
        {
            currentRepair = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition("repairParts").id).amount;
            Debug.Log("Current Repair is: " + currentRepair);
            if (currentRepair > 0)
            {
                this.part.RequestResource("repairParts", repairRate);
                repair = true;
                Debug.Log("Repaired " + repair);
                currentRepair = 0;
            }
            if (currentRepair == 0)
            {
                repair = false;
                Debug.Log("Not repaired " + repair);
            }
        }              
    }
}
