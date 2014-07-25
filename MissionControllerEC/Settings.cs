using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControllerEC
{
    public class Settings : ConfigNodeStorage
    {       
        public Settings(String FilePath) : base(FilePath) { }

        //[Persistent]internal double TimeCheckDays = 604800;
        [Persistent]internal int difficutlylevel = 1;
        [Persistent]internal double HireCost = 1000;
        //[Persistent]internal double SaleryCost = 100;
        [Persistent]internal float EasyMode = 1.0f;
        [Persistent]internal float MediumMode = 2.0f;
        [Persistent]internal float HardCoreMode = 4.0f;
        //[Persistent]internal bool cheatsTrue = false;       
    }
}
