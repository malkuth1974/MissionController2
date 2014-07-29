using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionControllerEC
{
    class Tools
    {
        
        public static bool RandomBool(bool value)
        {
            int test = 0;
            test = UnityEngine.Random.Range(0, 100);
            if (test > 50)
                return true;
            else
                return false;
        }

        public static int RandomNumber(int range1, int range2)
        {
            int value = UnityEngine.Random.Range(range1, range2);

            return value;
        }

        public static double ConvertDays(double seconds)
        {
            double newValue;
            newValue = seconds / (24.0 * 60.0 * 60.0);
            return newValue;
        }
        public static double DivisionBy2Numbers(double val, double val2)
        {
            double divnum;
            divnum = val / val2;
            return divnum;
        }
        public static String formatTime(double seconds)
        {
            int y = (int)(seconds / (24.0 * 60.0 * 60.0 * 365.0));
            seconds = seconds % (24.0 * 60.0 * 60.0 * 365.0);
            int d = (int)(seconds / (24.0 * 60.0 * 60.0));
            seconds = seconds % (24.0 * 60.0 * 60.0);
            int h = (int)(seconds / (60.0 * 60.0));
            seconds = seconds % (60.0 * 60.0);
            int m = (int)(seconds / (60.0));
            seconds = seconds % (60.0);

            List<String> parts = new List<String>();

            if (y > 0)
            {
                parts.Add(String.Format("{0}:year ", y));
            }

            if (d > 0)
            {
                parts.Add(String.Format("{0}:days ", d));
            }

            if (h > 0)
            {
                parts.Add(String.Format("{0}:hours ", h));
            }

            if (m > 0)
            {
                parts.Add(String.Format("{0}:mins ", m));
            }

            if (seconds > 0)
            {
                parts.Add(String.Format("{0:00}:secs ", seconds));
            }

            if (parts.Count > 0)
            {
                return String.Join(" ", parts.ToArray());
            }
            else
            {
                return "0s";
            }
        }

        
    }
                
}
