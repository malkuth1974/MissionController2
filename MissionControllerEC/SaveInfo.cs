using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControllerEC
{
    public static class SaveInfo 
    {
        //public SaveInfo(String FilePath) : base(FilePath) { }
        //[Persistent]
        //internal double TotalSpentKerbals = 0;
        //[Persistent]
        //internal double CurrentTimeCheck = 604800;
        //[Persistent]
        //internal double TotalSpentOnSaleries = 0;
        //[Persistent]
        //internal double TotalSpentOnRocketTest = 0;
        //[Persistent]
        //public static bool SatContractReady = false;

        public static double TotalSpentKerbals = 0;
        public static double TotalSpentOnRocketTest = 0;
        public static bool SatContractReady = false;
    }
}
