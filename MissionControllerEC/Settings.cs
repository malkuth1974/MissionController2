using System;

namespace MissionControllerEC
{
    public class Settings : ConfigNodeStorage
    {       
        public Settings(String FilePath) : base(FilePath) { }

        [Persistent]internal int difficutlylevel = 1;
        [Persistent]internal double HireCost = 4000;
        [Persistent]internal double DeathInsurance = 20000;
        [Persistent]internal float EasyMode = 1.0f;
        [Persistent]internal float MediumMode = 2.0f;
        [Persistent]internal float HardCoreMode = 4.0f;
        [Persistent]internal bool NoRescueKerbalContracts = true;  //thanks flowerchild :)
        [Persistent]internal bool StartBuilding = false;
        [Persistent]internal double maxOrbP = 10860;
        [Persistent]internal double minOrbP = 10680;
        [Persistent]internal string contractName = "Deliever COMSAT Satellite";
        [Persistent]internal int bodyNumber = 1;
        [Persistent]internal bool RevertOn = true;
        [Persistent]internal bool DebugMenu = false;
      
    }
}
