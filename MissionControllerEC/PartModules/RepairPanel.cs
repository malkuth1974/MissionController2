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

        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "CheckSystems", active = false)]
        public void CheckSystems()
        {
            currentRepair = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition("SpareParts").id).amount; 
            if (currentRepair == 0)
            {
                ScreenMessages.PostScreenMessage("You need to Transfer SpareParts to the Repair Panel To Proceed", 5f);               
            }
            else
            {
                readyRep = true;
                vesselId = this.part.vessel.id.ToString();
                vesselName = this.part.vessel.name;
                Debug.LogError("Vessel Id For PartModule is " + vesselId + " Name is " + vesselName);
                ScreenMessages.PostScreenMessage("Vessel Preped for Repair open the Panel.  Then conduct the repair", 5f);                
            }

            
        }
      
        [KSPEvent(externalToEVAOnly = true, unfocusedRange = 4f, guiActiveUnfocused = true, guiName = "Start Repairs", active = false)]
        public void EnableRepair()
        {
            if (readyRep && currentRepair > 0)
            {
                this.part.RequestResource("SpareParts", repairRate);
                repair = true;
                Debug.Log("repairEnabled");
                ScreenMessages.PostScreenMessage("Repairs Started and Finsished using Spare Parts.  Good Job", 5f);
                readyRep = false;
            }
            else { ScreenMessages.PostScreenMessage("You must first CheckSystems on the repair panel While on EVA. Right click part and choose Check If Systems Is Ready For Repair", 5f); }
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
