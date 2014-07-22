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
        [Persistent] internal Boolean TestBoolean = true;
    }
}
