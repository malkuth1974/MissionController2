using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControllerEC
{
    public static class SaveInfo
    {
        public static Vector2 MainGUIWindowPos;
        public static Vector2 CustomSatWindowPos;
        public static Vector2 CustomTransWindowPos;
        public static Vector2 CustomCrewTransWindowPos;
        public static Vector2 DebugWindowPos;
        public static Vector2 CustomLandingOrbitWinPos;
        public static Vector2 CustomBuildStationWinPos;

        public static bool GUIEnabled;

        public static bool SatContractReady = false;

        public static bool ComSatContractOn = false;
        public static double comSatmaxOrbital = 0;
        public static double comSatminOrbital = 0;
        public static string ComSatContractName = "None";
        public static int comSatBodyName = 1;

        public static bool supplyContractOn = false;
        public static int SupplyBodyIDX;
        public static string SupplyVesName = "None Loaded";
        public static string SupplyVesId = "None Loaded";
        public static string ResourceName = "None Loaded";
        public static string SupplyContractName = "uninitialized";
        public static double supplyAmount = 0;

        public static string crewVesName = "none";
        public static string crewVesid = "none";
        public static int crewBodyIDX;
        public static bool crewContractOn = false;
        public static int crewAmount = 0;
        public static double crewTime = 0;
        public static string crewTransferName = "uninitialized";
        public static int transferTouristAmount = 0;
        public static bool transferTouristTrue = false;
        public static string TransferCrewDesc = "uninitialized.";

        public static string LandingOrbitName = "uninitialized";
        public static string LandingOrbitDesc = "uninitialized";
        public static int LandingOrbitIDX = 1;
        public static bool IsOrbitOrLanding = false;
        public static bool OrbitLandingOn = false;
        public static bool OrbitAllowCivs = false;
        public static int LandingOrbitCrew = 1;
        public static int LandingOrbitCivilians = 0;

        public static string BuildSpaceStationName = "uninitialized";
        public static int BuildSpaceStationIDX = 1;
        public static string BuildSpaceStationDesc = "uninitialized";
        public static bool BuildSpaceStationOn = false;
        public static int BuildSpaceStationCrewAmount = 1;


        public static bool RepairContractGeneratedOn = false;
        public static bool RepairStationContractGeneratedOn = false;
        public static int SatelliteTypeChoice = 0;

        public static bool spResourceSet = false;

        public static string SatelliteContractType = "none";
        public static float SatelliteContractFrequency = -1;
        public static float SatelliteContractModule = 0;
        public static double SavedRoverLat = 0;
        public static double savedRoverLong = 0;
        public static bool RoverLanded = false;
        public static string RoverName = "Rover Name";
        public static int RoverBody = 6;
        public static string SatelliteConDesc = "uninitialized";
        public static string ResourceTransferConDesc = "uninitialized";

        public static List<string> TourisNames = new List<string>();
        public static List<string> TourisNames2 = new List<string>();
        public static List<string> OrbitNamesList = new List<string>();

        public static Dictionary<int, string> CustOrbLnd =
            new Dictionary<int, string>();

        public static System.Random rnd = new System.Random();

        const string CONTRACTDESCR = "Place Contract Description Here";
        const string CONTRACTNAME = "Place Contract Name Here";

        const string BUILDSPACESTATIONNAME = "Fill In Name Of Station";
        const string BUILDSPACESTATIONDESCR = "Description of Mission";


        const string NONE = "None";
        public static bool ComSatValid { get { return (ComSatContractName != NONE && SatelliteConDesc != CONTRACTDESCR); } }
        public static bool LandingOrbitValid { get { return LandingOrbitName != CONTRACTNAME && LandingOrbitDesc != CONTRACTDESCR; } }
        public static bool BuildSpaceStationValid { get { return (BuildSpaceStationName != BUILDSPACESTATIONNAME && BuildSpaceStationDesc != BUILDSPACESTATIONDESCR); } }
        public static bool ResourceSupplyValid {  get { return SupplyContractName != CONTRACTNAME && ResourceTransferConDesc != CONTRACTDESCR; } }
        public static bool CrewTransferValid {  get { return crewTransferName != CONTRACTNAME && TransferCrewDesc != CONTRACTDESCR; } }
        public static void InitSaveInfo()
        {
            comSatmaxOrbital = 0;
            comSatminOrbital = 0;
            ComSatContractName = NONE;
            SatelliteConDesc = CONTRACTDESCR;

            comSatBodyName = 1;

            //public static int SupplyBodyIDX;
            SupplyVesName = "None Loaded";
            SupplyVesId = "None Loaded";
            ResourceName = "None Loaded";
            SupplyContractName = CONTRACTNAME;
            supplyAmount = 0;

            crewVesName = "none";
            crewVesid = "none";
            //public static int crewBodyIDX;
            crewAmount = 1;
            crewTime = 0;
            crewTransferName = CONTRACTNAME;
            transferTouristAmount = 0;
            TransferCrewDesc = CONTRACTDESCR;

            LandingOrbitName = CONTRACTNAME;
            LandingOrbitDesc = CONTRACTDESCR;
            LandingOrbitIDX = 1;
            IsOrbitOrLanding = false;
            OrbitLandingOn = false;
            OrbitAllowCivs = false;
            LandingOrbitCrew = 1;
            LandingOrbitCivilians = 0;

            BuildSpaceStationName = BUILDSPACESTATIONNAME;
            BuildSpaceStationIDX = 1;
            BuildSpaceStationDesc = BUILDSPACESTATIONDESCR;
            BuildSpaceStationOn = false;
            BuildSpaceStationCrewAmount = 1;


            SatelliteTypeChoice = 0;


            SatelliteContractType = "none";
            SatelliteContractFrequency = -1;
            SatelliteContractModule = 0;
            SavedRoverLat = 0;
            savedRoverLong = 0;
            RoverName = "Rover Name";
            RoverBody = 6;
            ResourceTransferConDesc = CONTRACTDESCR;

        }
    }
}
