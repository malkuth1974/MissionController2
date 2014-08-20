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
        [Persistent]internal bool NoRescueKerbalContracts = false;  //thanks flowerchild :)
        [Persistent]internal bool NoPartTestContracts = false;
        [Persistent]internal bool StartBuilding = false;
        [Persistent]internal double maxOrbP = 10860;
        [Persistent]internal double minOrbP = 10680;
        [Persistent]internal string contractName = "Deliever COMSAT Satellite";
        [Persistent]internal int bodyNumber = 1;
        [Persistent]internal bool RevertOn = true;
        [Persistent]internal bool DebugMenu = false;

        [Persistent]internal double contracSatelliteMaxApATrivial = 75000;
        [Persistent]internal double contracSatelliteMaxTotalHeightTrivial = 325000;
        [Persistent]internal double contracSatelliteMaxApASignificant = 85000;
        [Persistent]internal double contracSatelliteMaxTotalHeightSignificant = 320000;
        [Persistent]internal double contracSatelliteMaxApAExcept = 200000;
        [Persistent]internal double contracSatelliteMaxTotalHeightExcept = 300000;
        [Persistent]internal double contracSatelliteBetweenDifference = 5000;
        [Persistent]internal float contracSatelliteMaxAMassTrivial = 4;
        [Persistent]internal float contracSatelliteMaxMassSignificant = 6;
        [Persistent]internal float contracSatelliteMaxMassExcept = 7;
        [Persistent]internal double contractOrbitalPeriodMaxInSeconds = 21660;
        [Persistent]internal double contractOrbitalPeriodMinInSeconds = 21480;
        [Persistent]internal int ContractPaymentMultiplier = 1;
        
      
    }
}
