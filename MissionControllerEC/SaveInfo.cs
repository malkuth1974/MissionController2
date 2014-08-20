using System;

namespace MissionControllerEC
{
    public static class SaveInfo 
    {       
        public static double TotalSpentKerbals = 0;
        public static double TotalSpentOnRocketTest = 0;
        public static bool SatContractReady = false;
        public static string AgenaTargetVesselID = "none";
        public static string AgenaTargetVesselName = "none";
        public static bool Agena1Done = false;
        public static bool Agena2Done = false;
        public static bool MessageHelpers = false;

        public static bool supplyContractOn = false;
        public static int SupplyBodyIDX;
        public static string SupplyVesName = "None Loaded";
        public static string SupplyVesId = "None Loaded";
        public static string ResourceName = "None Loaded";
        public static string SupplyContractName = "Supply Contract";
        public static double supplyAmount = 0;

        public static bool NoSatelliteContracts = false;
        public static bool NoOrbitalPeriodcontracts = false;
        public static bool NoLanderResearchContracts = false;
        public static bool NoOrbitalResearchContracts = false;
        public static bool NoRepairContracts = false;

        public static bool RepairContractOn = false;
    }   
}
