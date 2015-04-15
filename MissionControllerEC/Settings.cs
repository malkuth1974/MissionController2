using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControllerEC
{
    public class Settings : ConfigNodeStorage
    {       
        public Settings(String FilePath) : base(FilePath) 
        { 
        }
       
        [Persistent]internal double Death_Insurance = 20000;
        [Persistent]internal double Revert_Cost = 1000;

        [Persistent]internal bool No_Rescue_Kerbal_Contracts = false;  //thanks flowerchild :)
        [Persistent]internal bool No_Part_Test_Contracts = false;       
        [Persistent]internal bool RevertOn = false;
        [Persistent]internal bool DebugMenu = false;       

        [Persistent]internal double contract_Satellite_Max_Height = 75000;
        [Persistent]internal double contract_Satellite_MIn_Height = 325000;        
        [Persistent]internal double contract_Satellite_Between_Difference = 2000;        
        [Persistent]internal double contract_Orbital_Period_Max_InSeconds = 21698;
        [Persistent]internal double contract_Orbital_Period_Min_InSeconds = 21598;
        [Persistent]internal int Contract_Payment_Multiplier = 1;
        [Persistent]internal double contract_Random_Orbital_Period_Difference = 600;
        [Persistent]internal double contract_Random_Orbital_Period_MinInSeconds = 2000;
        [Persistent]internal double contract_Random_Orbital_Period_MaxInSeconds = 120000;

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
        [Persistent]
        internal List<String> SupplyResourceList = new List<string>() { "LiquidFuel", "Oxidizer", "MonoPropellant", "XenonGas", "Food", "Water", "Oxygen", "Kibbal" };    
    }
}
