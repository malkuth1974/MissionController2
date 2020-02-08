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

        public static bool ComSateContractOn = false;
        public static double comSatmaxOrbital = 0;
        public static double comSatminOrbital = 0;
        public static string ComSatContractName = "None";
        public static int comSatBodyName = 1;
       
        public static bool supplyContractOn = false;
        public static int SupplyBodyIDX;
        public static string SupplyVesName = "None Loaded";
        public static string SupplyVesId = "None Loaded";
        public static string ResourceName = "None Loaded";
        public static string SupplyContractName = "Place Contract Name Here";
        public static double supplyAmount = 0;

        public static string crewVesName = "none";
        public static string crewVesid = "none";
        public static int crewBodyIDX;
        public static bool crewContractOn = false;
        public static int crewAmount = 0;
        public static double crewTime = 0;
        public static string crewTransferName = "Place Contract Name Here";
        public static int transferTouristAmount = 0;
        public static bool transferTouristTrue = false;
        public static string TransferCrewDesc = "Place Contract Description Here.";

        public static string LandingOrbitName = "Contract Name";
        public static string LandingOrbitDesc = "Place Contract Description Here";
        public static int LandingOrbitIDX = 1;
        public static bool IsOrbitOrLanding = false;
        public static bool OrbitLandingOn = false;
        public static bool OrbitAllowCivs = false;
        public static int LandingOrbitCrew = 0;
        public static int LandingOrbitCivilians =0;

        public static string BuildSpaceStationName = "Fill In Name Of Station";
        public static int BuildSpaceStationIDX = 1;
        public static string BuildSpaceStationDesc = "Description of Mission";
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
        public static string SatelliteConDesc = "Place Contract Description Here";
        public static string ResourceTransferConDesc = "Place Contract Description Here";

        public static List<string> TourisNames = new List<string>();
        public static List<string> TourisNames2 = new List<string>();
        public static List<string> OrbitNamesList = new List<string>();        

        public static Dictionary<int, string> CustOrbLnd =
            new Dictionary<int, string>();
    }   
}
