using System;
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

        [KSPField(isPersistant = false, guiActive = true, guiName = "Ready To Repair")]
        public bool readyRep = false;

        [KSPEvent(guiActive = true, guiName = "Check If Systems is Ready For Repair", active = true)]
        public void CheckSystems()
        {
            currentRepair = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition("repairParts").id).amount;

            if (currentRepair < 1)
            {
                ScreenMessages.PostScreenMessage("You need to Transfer Repair Parts to the Repair Panel To Proceed",5f);
            }
            else
            {
                readyRep = true;
                ScreenMessages.PostScreenMessage("Vessel Preped for Repair, EVA to the repair Panel and open the Panel.  Then conduct the repair",5f); 
            }

            
        }
      
        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Start Repairs", active = false)]
        public void EnableRepair()
        {
            if (readyRep && currentRepair == 1)
            {
                this.part.RequestResource("repairParts", repairRate);
                repair = true;
                Debug.Log("repairEnabled");
                ScreenMessages.PostScreenMessage("Repairs Started and Finsished.  Good Job",5f);
                readyRep = false;
            }
            else ScreenMessages.PostScreenMessage("You must first prep the repair panel inside the vessel, when inside right click part and choose Check If Systems Is Ready For Repair",5f);
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Open Door", active = true, guiActiveEditor = true)]
        public void OpenDoor()
        {
            PlayOpenAnimation(1,0);
            Events["OpenDoor"].active = false;
            Events["EnableRepair"].active = true;
            Events["closeDoor"].active = true;
        }

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Close Door", active = false, guiActiveEditor = true)]
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

        public override void OnFixedUpdate()
        {                      
        }        
    }
}
