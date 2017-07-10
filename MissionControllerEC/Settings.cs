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
        [Persistent]internal string Vostok1Desc = "";
        [Persistent]internal string Vostok1Complete = "";
        [Persistent]
        internal string Vostok2esc = "";
        [Persistent]
        internal string Vostok2Complete = "";
        [Persistent]internal List<String> SupplyResourceList = new List<string>() { "LiquidFuel", "Oxidizer", "MonoPropellant", "XenonGas", "Food", "Water", "Oxygen", "Kibbal" };       
    }
}
