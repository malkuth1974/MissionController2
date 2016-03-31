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
        [Persistent]internal bool No_Fineprint_Station_Contracts = false;
        [Persistent]internal bool No_Fineprint_Base_Contracts = false;
        [Persistent]internal bool No_Fineprint_ISRU_Contracts = false;
        [Persistent]internal bool No_Fineprint_Tourism_Contracts = false;
        [Persistent]internal bool No_GrandTour_Contracts = false;
        [Persistent]internal bool No_Explore_Body = false;
        [Persistent]internal bool DebugMenu = false;
        [Persistent]internal double Satellite_Contract_Per_Cycle = 2;
        [Persistent]internal double Science_Contract_Per_Cycle = 1;      
        [Persistent]internal float Contract_Payment_Multiplier = 1;                
        [Persistent]internal int Margin_Of_Error_Contract_Orbits = 5000;
        [Persistent]internal int Margin_Of_Error_Contract_Inclination = 3;
        [Persistent]internal double Margin_Of_Error_Contract_Eccentric = .5;       
        [Persistent]internal List<String> SupplyResourceList = new List<string>() { "LiquidFuel", "Oxidizer", "MonoPropellant", "XenonGas", "Food", "Water", "Oxygen", "Kibbal" };        
    }
}
