using System;

namespace MissionControllerEC
{
    public class Settings : ConfigNodeStorage
    {       
        public Settings(String FilePath) : base(FilePath) { }
       
        [Persistent]internal double HireCost = 4000;
        [Persistent]internal double DeathInsurance = 20000;        
        [Persistent]internal bool NoRescueKerbalContracts = false;  //thanks flowerchild :)
        [Persistent]internal bool NoPartTestContracts = false;
        [Persistent]internal bool StartBuilding = false;
        [Persistent]internal double maxOrbP = 10860;
        [Persistent]internal double minOrbP = 10680;
        [Persistent]internal string contractName = "Deliever COMSAT Satellite";
        [Persistent]internal int bodyNumber = 1;
        [Persistent]internal bool RevertOn = false;
        [Persistent]internal bool DebugMenu = false;

        [Persistent]internal double contracSatelliteMaxApATrivial = 75000;
        [Persistent]internal double contracSatelliteMaxTotalHeightTrivial = 325000;
        [Persistent]internal double contracSatelliteMaxApASignificant = 85000;
        [Persistent]internal double contracSatelliteMaxTotalHeightSignificant = 320000;
        [Persistent]internal double contracSatelliteMaxApAExcept = 200000;
        [Persistent]internal double contracSatelliteMaxTotalHeightExcept = 300000;
        [Persistent]internal double contracSatelliteBetweenDifference = 5000;
        [Persistent]internal float contracSatelliteMaxAMassTrivial = 3.7f;
        [Persistent]internal float contracSatelliteMaxMassSignificant = 5.8f;
        [Persistent]internal float contracSatelliteMaxMassExcept = 6.5f;
        [Persistent]internal float contracSatelliteMinAMassTrivial = 2f;
        [Persistent]internal float contracSatelliteMinMassSignificant = 2.3f;
        [Persistent]internal float contracSatelliteMinMassExcept = 2.7f;
        [Persistent]internal float contractSatelliteMassDifference = 1.5f;
        [Persistent]internal double contractOrbitalPeriodMaxInSeconds = 21660;
        [Persistent]internal double contractOrbitalPeriodMinInSeconds = 21480;
        [Persistent]internal int ContractPaymentMultiplier = 1;
        
      
    }
}
