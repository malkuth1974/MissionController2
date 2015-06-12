using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControllerEC
{
    public class Settings : ConfigNodeStorage
    {       
        public Settings(String FilePath) : base(FilePath) 
        { 
        }
       
        [Persistent]internal double Revert_Cost = 1000;
        [Persistent]internal bool RevertOn = false;
        [Persistent]internal bool No_Rescue_Kerbal_Contracts = false;  //thanks flowerchild :)
        [Persistent]internal bool No_Part_Test_Contracts = false;
        [Persistent]internal bool No_Finprint_Satellite_Contracts = false;
        [Persistent]internal bool No_Fineprint_Survey_Contracts = false;
        [Persistent]internal bool DebugMenu = false;
        [Persistent]internal double Satellite_Contract_Per_Cycle = 2;
        [Persistent]internal double Science_Contract_Per_Cycle = 2;      
        [Persistent]internal int Contract_Payment_Multiplier = 1;        
        [Persistent]internal int contract_repair_Random_percent = 35;
        [Persistent]internal int contract_repair_Station_Random_percent = 35;        
        [Persistent]internal double vostok12height = 70000;
        [Persistent]internal double voshodheight = 82000;
        [Persistent]internal double skyLabheight = 90000;
        [Persistent]internal double skyLab4MaxApA = 120000;
        [Persistent]internal double skyLab4MaxPeA = 120000;
        [Persistent]internal List<String> SupplyResourceList = new List<string>() { "LiquidFuel", "Oxidizer", "MonoPropellant", "XenonGas", "Food", "Water", "Oxygen", "Kibbal" };        
    }
}
