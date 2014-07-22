using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionControllerEC
{
    class Tools
    {
        // Tool Class Written by NathanKell for Original Mission Controller Extended.  Modified by Malkuth for MCE2.

        public static ConfigNode Settings = null;

        public static void FindSettings()
        {
            foreach (ConfigNode node in GameDatabase.Instance.GetConfigNodes("MISSIONCONTROLLER"))
                Settings = node;

            if (Settings == null)
                throw new UnityException("*MC* MCSettings not found!");
        }
       
        public static double atod(string a)
        {
            double o;
            double.TryParse(a, out o);
            return o;
        }
        public static bool atob(string a)
        {
            bool o;
            bool.TryParse(a, out o);
            return o;
        }
        public static int atoi(string a)
        {
            int o;
            int.TryParse(a, out o);
            return o;
        }

        public static float atof(string a)
        {
            float o;
            float.TryParse(a, out o);
            return o;
        }

        public static double GetValueDefault(ConfigNode node, string name, double val)
        {
            if (node.HasValue(name))
                val = atod(node.GetValue(name));
            //else Debug.Log("*MCEPC key not found: " + name);
            return val;
        }

        public static float GetValueDefault(ConfigNode node, string name, float val)
        {
            if (node.HasValue(name))
                val = atof(node.GetValue(name));
            return val;
        }

        public static bool GetValueDefault(ConfigNode node, string name, bool val)
        {
            if (node.HasValue(name))
                val = atob(node.GetValue(name));
            //else Debug.Log("*MCEPC key not found: " + name);
            return val;
        }

        public static int GetValueDefault(ConfigNode node, string name, int val)
        {
            if (node.HasValue(name))
                val = atoi(node.GetValue(name));
            //else Debug.Log("*MCEPC key not found: " + name);
            return val;
        }

        public static string GetValueDefault(ConfigNode node, string name, string vale)
        {
            if (node.HasValue(name))
                vale = (node.GetValue(name));
            //else Debug.Log("*MCEPC key not found: " + name);
            return vale;
        }

        public static string spaces(int num)
        {
            string str = "";
            for (int i = 0; i < num; i++)
                str += " ";
            return str;
        }

        public static string NodeToString(ConfigNode node, int depth)
        {
            string str = spaces(depth) + node.name + "\n" + spaces(depth) + "{\n";

            foreach (ConfigNode.Value val in node.values)
                str += spaces(depth + 1) + val.name + " = " + val.value + "\n";

            foreach (ConfigNode n in node.nodes)
                str += NodeToString(n, depth + 1);

            return str + spaces(depth) + "}\n";
        }

        public static double Setting(string key, double val)
        {
            if (Settings != null)
                return GetValueDefault(Settings, key, val);
            return val;
        }
        public static bool Setting(string key, bool val)
        {
            if (Settings != null)
                return GetValueDefault(Settings, key, val);
            return val;
        }
        public static int Setting(string key, int val)
        {
            if (Settings != null)
                return GetValueDefault(Settings, key, val);
            return val;
        }
        public static float Setting(string key, float val)
        {
            if (Settings != null)
                return GetValueDefault(Settings, key, val);
            return val;
        }
    
    
    }                
}
