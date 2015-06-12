using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionControllerEC
{
    class Tools
    {
        public static bool RandomBool(int randomvalue)
        {
            int test = 0;
            test = UnityEngine.Random.Range(0, 100);
            if (test > randomvalue)
            {
                Debug.Log("MCE Random Check for " + randomvalue + " is TRUE");
                return true;                
            }
            else
            {
                Debug.Log("MCE Random Check for " + randomvalue + " is FALSE");
                return false;
            }
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
        public static String ConvertMinsHours(double seconds)
        {          
            int h = (int)(seconds / (60.0 * 60.0));
            seconds = seconds % (60.0 * 60.0);
            int m = (int)(seconds / (60.0));
            seconds = seconds % (60.0);

            List<String> count = new List<String>();                     

            if (h > 0)
            {
                count.Add(String.Format("{0}:hours ", h));
            }

            if (m > 0)
            {
                count.Add(String.Format("{0}:mins ", m));
            }

            if (seconds > 0)
            {
                count.Add(String.Format("{0:00}:secs ", seconds));
            }

            if (count.Count > 0)
            {
                return String.Join(" ", count.ToArray());
            }
            else
            {
                return "0s";
            }

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

            List<String> count = new List<String>();

            if (y > 0)
            {
                count.Add(String.Format("{0}:year ", y));
            }

            if (d > 0)
            {
                count.Add(String.Format("{0}:days ", d));
            }

            if (h > 0)
            {
                count.Add(String.Format("{0}:hours ", h));
            }

            if (m > 0)
            {
                count.Add(String.Format("{0}:mins ", m));
            }

            if (seconds > 0)
            {
                count.Add(String.Format("{0:00}:secs ", seconds));
            }

            if (count.Count > 0)
            {
                return String.Join(" ", count.ToArray());
            }
            else
            {
                return "0s";
            }
        }

        public static double getBodyAltitude(CelestialBody body)
        {
            double atmosphere = 0.0;

            if (!body.atmosphere)
            {
                atmosphere = body.Radius + 40000;
                Debug.Log("Body radius is " + body.Radius);
            }
            else
            {
                atmosphere = body.atmosphereDepth;

                Debug.Log("Body and AtmoshpereDepth is " + atmosphere);
            }

            return Math.Round(atmosphere);
        }

        public static void ObitalPeriodHelper(Vessel v)
        {            
            ScreenMessages.PostScreenMessage("Current Orbital Period is: " + Tools.formatTime(FlightGlobals.ActiveVessel.orbit.period) + "\n" +
                "ApA Is: " + (int)v.orbit.ApA + " PeA Is: "+ (int)v.orbit.PeA + "\n" +
                "Current eccentricity is: " + FlightGlobals.ActiveVessel.orbit.eccentricity.ToString("F2") +"\n" +
            "Current Biome Is: " + FlightGlobals.ActiveVessel.mainBody.BiomeMap.GetAtt(FlightGlobals.ActiveVessel.latitude * Math.PI / 180d, FlightGlobals.ActiveVessel.longitude * Math.PI / 180d), .001f);
        }

        public static void ContractLoadCheck<t>(ConfigNode node, ref t value, t backupDefault, string valueName, string savedFile)
        {
            try
            {
                string i;
                i = node.GetValue(savedFile);
                value = (t)(object)i;
            }
            catch (Exception ex)
            {
                //Debug.LogWarning("MCE Exeption failed to load contract saved value " + valueName + " " + savedFile + "Backup Loaded: " + backupDefault + " Node Name: " + node);
                //Debug.LogWarning(ex.Message + " " + ex.StackTrace);
                value = backupDefault;

            }
        }
        public static void ContractLoadCheck<t>(ConfigNode node, ref t value, t backupDefault, float valueName, string savedFile)
        {
            try
            {
                float i;
                i = float.Parse(node.GetValue(savedFile));
                value = (t)(object)i;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value " + valueName + " " + savedFile + "Backup Loaded");
                Debug.LogWarning(ex.Message + " " + ex.StackTrace);
                value = backupDefault;

            }
        }
        public static void ContractLoadCheck<t>(ConfigNode node, ref t value, t backupDefault, int valueName, string savedFile)
        {
            try
            {
                int i;
                i = int.Parse(node.GetValue(savedFile));
                value = (t)(object)i;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value " + valueName + " " + savedFile + "Backup Loaded");
                Debug.LogWarning(ex.Message + " " + ex.StackTrace);
                value = backupDefault;
            }
        }
        public static void ContractLoadCheck<t>(ConfigNode node, ref t value, t backupDefault, double valueName, string savedFile)
        {
            try
            {
                double i;
                i = double.Parse(node.GetValue(savedFile));
                value = (t)(object)i;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value " + valueName + " " + savedFile + "Backup Loaded");
                Debug.LogWarning(ex.Message + " " + ex.StackTrace);
                value = backupDefault;
            }
        }
        public static void ContractLoadCheck<t>(ConfigNode node, ref t value, t backupDefault, bool valueName, string savedFile)
        {
            try
            {
                bool i;
                i = bool.Parse(node.GetValue(savedFile));
                value = (t)(object)i;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value " + valueName + " " + savedFile + "Backup Loaded");
                Debug.LogWarning(ex.Message + " " + ex.StackTrace);
                value = backupDefault;
            }
        }
        public static void ContractLoadCheck<t>(ConfigNode node, ref t value,t backupDefault,CelestialBody valueName, string savedFile)
        {
            try
            {
                CelestialBody cb = null;
                int bodyID = int.Parse(node.GetValue(savedFile));
                cb = FlightGlobals.Bodies[bodyID];
                if (cb == null)
                    {
                        Debug.LogError("Celestrial Body is null from Node Load have to load Backup Value");
                        value = backupDefault;
                    }
                    else
                        value = (t)(object)cb;
                
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value " + savedFile + "Backup Loaded");
                Debug.LogWarning("MCE CelestrialBody Failed Load");
                Debug.LogWarning(ex.Message + " " + ex.StackTrace);
                value = backupDefault;
                Debug.LogWarning("MCE Set value to default of " + backupDefault);

            }           
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
