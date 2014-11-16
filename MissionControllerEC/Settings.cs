using System;

namespace MissionControllerEC
{
    public class Settings : ConfigNodeStorage
    {       
        public Settings(String FilePath) : base(FilePath) { }
       
        [Persistent]internal double HireCost = 4000;
        [Persistent]internal double Death_Insurance = 20000;        
        [Persistent]internal bool No_Rescue_Kerbal_Contracts = false;  //thanks flowerchild :)
        [Persistent]internal bool No_Part_Test_Contracts = false;       
        [Persistent]internal bool RevertOn = false;
        [Persistent]internal bool DebugMenu = false;
        [Persistent]internal bool Civilian_Contracts_Off = false;
        [Persistent]internal bool all_Historical_Contracts_Off = false; 

        [Persistent]internal double contrac_Satellite_Max_ApA_Trivial = 75000;
        [Persistent]internal double contrac_Satellite_Max_Total_Height_Trivial = 325000;
        [Persistent]internal double contrac_Satellite_Max_ApA_Significant = 85000;
        [Persistent]internal double contrac_Satellite_Max_Total_Height_Significant = 320000;
        [Persistent]internal double contrac_Satellite_Max_ApA_Except = 200000;
        [Persistent]internal double contrac_Satellite_Max_Total_Height_Except = 300000;
        [Persistent]internal double contrac_Satellite_Between_Difference = 5000;
        [Persistent]internal float contrac_Satellite_Max_Mass_Trivial = 3.7f;
        [Persistent]internal float contrac_Satellite_Max_Mass_Significant = 5.8f;
        [Persistent]internal float contrac_Satellite_Max_Mass_Except = 6.5f;
        [Persistent]internal float contrac_Satellite_Min_Mass_Trivial = 2f;
        [Persistent]internal float contrac_Satellite_Min_Mass_Significant = 2.3f;
        [Persistent]internal float contrac_Satellite_Min_Mass_Except = 2.7f;
        [Persistent]internal double contract_Orbital_Period_Max_InSeconds = 21698;
        [Persistent]internal double contract_Orbital_Period_Min_InSeconds = 21598;
        [Persistent]internal int Contract_Payment_Multiplier = 1;
        [Persistent]internal double contract_Random_Orbital_Period_Difference = 600;
        [Persistent]internal double contract_Random_Orbital_Period_MinInSeconds = 76000;
        [Persistent]internal double contract_Random_Orbital_Period_MaxInSeconds = 2700;

        [Persistent]internal int contract_repair_Random_percent = 20;
        [Persistent]internal int contract_repair_Station_Random_percent = 20;
        [Persistent]internal int contract_Civilian_Station_Expedition = 20;
        [Persistent]internal int contract_civilian_Low_Orbit_Percent = 20;
        [Persistent]internal int contract_civilian_Landing_Percent = 20;

        [Persistent]internal double vostok12height = 70000;
        [Persistent]internal double voshodheight = 82000;
        [Persistent]internal double skyLabheight = 90000;
        [Persistent]internal double skyLab4MaxApA = 120000;
        [Persistent]internal double skyLab4MaxPeA = 120000;

        [Persistent]internal static bool ForcedUpdatePatch = false;
        
      
    }
}
