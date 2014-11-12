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
        public static bool Vostok1Done = false;
        public static bool Vostok2Done = false;
        public static bool Voskhod2Done = false;
        public static bool Luna2Done = false;
        public static bool Luna16Done = false;
        public static bool skylab1done = false;
        public static bool skylab2done = false;
        public static bool skylab3done = false;
        public static bool skylab4done = false;
        public static bool MessageHelpers = false;

        public static bool ComSateContractOn = false;
        public static double comSatmaxOrbital = 0;
        public static double comSatminOrbital = 0;
        public static string ComSatContractName = "None";
        public static int comSatBodyName = 1;

        public static string skyLabName = "none";
        public static string skyLabVesID = "none";

        public static bool supplyContractOn = false;
        public static int SupplyBodyIDX;
        public static string SupplyVesName = "None Loaded";
        public static string SupplyVesId = "None Loaded";
        public static string ResourceName = "None Loaded";
        public static string SupplyContractName = "Supply Contract";
        public static double supplyAmount = 0;

        public static string crewVesName = "none";
        public static string crewVesid = "none";
        public static int crewBodyIDX;
        public static bool crewContractOn = false;
        public static int crewAmount = 0;
        public static double crewTime = 0;
        public static string crewTransferName = "Crew Transfer";

        public static bool NoSatelliteContracts = false;
        public static bool NoOrbitalPeriodcontracts = false;
        public static bool NoLanderResearchContracts = false;
        public static bool NoOrbitalResearchContracts = false;
        public static bool NoRepairContracts = false;

        public static bool RepairContractOn = false;
        public static bool CivilianLowOrbit = false;
        public static bool CivilianLanding = false;

        public static bool CivilianApPod = false;
        public static string CivilianApName = "none";

        public static bool spResourceSet = false;
    }   
}
