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

        [KSPField(isPersistant = false)]
        public static bool dooropen = false;

        [KSPField(isPersistant = true)]
        public double currentRepair = 0;

        [KSPField(isPersistant = false)]
        public double repairRate = .1;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Ready To Repair")]
        public bool readyRep = false;

        public Animation GetDeployDoorAnim
        {
            get
            {
                return part.FindModelAnimators(DoorAnimation)[0];
            }
        }

        private void PlayDeployAnimation(int speed = 1)
        {
            print("Inflating");
            GetDeployDoorAnim[DoorAnimation].speed = speed;
            GetDeployDoorAnim.Play(DoorAnimation);           
        }
        public void DeflateLifeboat(int speed)
        {
            print("Deflating");
            GetDeployDoorAnim[DoorAnimation].time = GetDeployDoorAnim[DoorAnimation].length;
            GetDeployDoorAnim[DoorAnimation].speed = speed;
        }
        
        public override void OnStart(PartModule.StartState state)
        {
            this.part.force_activate();
        }
      
        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiActive = false, guiName = "Start Repairs", active = true)]
        public void EnableRepair()
        {
            dooropen = true;
            PlayDeployAnimation(1);
            Debug.Log("Door activated");
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiActive = false, guiName = "Open Door", active = true)]
        public void OpenDoor()
        {
            PlayDeployAnimation(1);
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiActive = false, guiName = "Close Door", active = true)]
        public void closeDoor()
        {
            DeflateLifeboat(1);
        }       

        [KSPAction("Start repair")]
        public void ToggleAction(KSPActionParam param)
        {
            EnableRepair();
        }       

        public override void OnFixedUpdate()
        {
            if (currentRepair > 0)
            {
                readyRep = !readyRep;
            }

            if (dooropen.Equals(true))
            {
                currentRepair = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition("repairParts").id).amount;
                this.part.RequestResource("repairParts", repairRate);
                if (currentRepair > 0)
                { 
                    repair = true;
                    ScreenMessages.PostScreenMessage("Repair Finished Good Job"); 
                }
                if (currentRepair == 0)
                {
                    repair = false;
                    dooropen = false;
                    ScreenMessages.PostScreenMessage("You Need to Transfer Repair Parts to the RepairPanel For Repair To work");
                }
            }
        }
    }
}
