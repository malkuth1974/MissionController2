using System;
using System.Collections.Generic;
using UnityEngine;

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
            int y = (int)(seconds / (6.0 * 60.0 * 60.0 * 426.08));
            seconds = seconds % (6.0 * 60.0 * 60.0 * 426.08);
            int d = (int)(seconds / (6.0 * 60.0 * 60.0));
            seconds = seconds % (6.0 * 60.0 * 60.0);
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
        public static void ObitalPeriodHelper(Vessel v)
        {            
            ScreenMessages.PostScreenMessage("Current Orbital Period is: " + Tools.formatTime(FlightGlobals.ActiveVessel.orbit.period) + "\n" +
                " ApA Is: " + (int)v.orbit.ApA + " PeA Is: "+ (int)v.orbit.PeA + "\n" +
                "Current eccentricity is: " + FlightGlobals.ActiveVessel.orbit.eccentricity.ToString("F2"), .001f);
        }

        public class MC2RandomWieghtSystem
        {
            public class Item<T>
            {
                public T value;
                public float weight;

                public static float GetTotalWeight<T>(Item<T>[] p_itens)
                {
                    float __toReturn = 0;
                    foreach (var item in p_itens)
                    {
                        __toReturn += item.weight;
                    }

                    return __toReturn;
                }
            }

            private static System.Random _randHolder;
            private static System.Random _random
            {
                get
                {
                    if (_randHolder == null)
                        _randHolder = new System.Random();

                    return _randHolder;
                }
            }

            public static T PickOne<T>(Item<T>[] p_itens)
            {
                if (p_itens == null || p_itens.Length == 0)
                {
                    return default(T);
                }

                float __randomizedValue = (float)_random.NextDouble() * (Item<T>.GetTotalWeight(p_itens));
                float __adding = 0;
                for (int i = 0; i < p_itens.Length; i++)
                {
                    float __cacheValue = p_itens[i].weight + __adding;
                    if (__randomizedValue <= __cacheValue)
                    {
                        return p_itens[i].value;
                    }

                    __adding = __cacheValue;
                }

                return p_itens[p_itens.Length - 1].value;

            }
        }
        
    }
                
}
