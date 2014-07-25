using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControllerEC
{
    public class SaveInfo : ConfigNodeStorage
    {
        public SaveInfo(String FilePath) : base(FilePath) { }
        [Persistent]internal double TotalSpentKerbals = 0;
        [Persistent]internal double CurrentTimeCheck = 604800;
        [Persistent]internal double TotalSpentOnSaleries = 0;
        [Persistent]internal double TotalSpentOnRocketTest = 0;
        [Persistent]internal bool SatContractReady = false;
    }
}
