using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionControllerEC
{
    public class Settings
    {
        public double HireCost = (Tools.Setting("HireCost",1000.0));
        public int difficutlylevel = (Tools.Setting("DifficultyMode", 1));

        public float EasyMode = (Tools.Setting("EasyMode", 1.0f));
        public float MediumMode = (Tools.Setting("MediumMode", 1.5f));
        public float HardCoreMode = (Tools.Setting("HardCoreMode", 2.0f));

        public double totalKerbalCost;
        public List<CurrentHires> NewHires = new List<CurrentHires>();
    }
   
    public class SettingsManager
    {

        private static SettingsManager manager = new SettingsManager();

        public static SettingsManager Manager
        {
            get
            { return manager; }
        }

        private Settings settings = new Settings();

        public Settings getSettings()
        {
            return manager.settings;
        }
    }   
}
