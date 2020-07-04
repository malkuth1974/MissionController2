using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionControllerEC
{
    class Tools
    {
        public static int RandomNumber(int range1, int range2)
        {
            range1 = Math.Abs(range1);
            range2 = Math.Abs(range2);
            int value = UnityEngine.Random.Range(range1, range2);
            Debug.LogError("range 1 Is = " + range1 + "Range 2 is = " + range2 + "Total Value is = " + value);            
            return value;
        }
        public static float FloatRandomNumber(float range1, float range2)
        {
            float value = UnityEngine.Random.Range(range1, range2);

            return value;
        }

        public static double GetRandomLongOrLat(double BaseLatitude, int MaxDistance)
        {
            double NLatitude = BaseLatitude;
            int NegMaxDistance = 0 - MaxDistance;
            BaseLatitude = NLatitude + UnityEngine.Random.Range(MaxDistance, NegMaxDistance);
            //Debug.Log("Base Latitude is: " + NLatitude + "New Value is: " + BaseLatitude);
            return BaseLatitude;
        }
       
        public static double ConvertDays(double seconds)
        {
            double newValue;
            newValue = seconds / 86400;
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
        
        public static double getAOPCalc(double a, double b)
        {
            double result = a / b;
            return result;
        }
        public static double DeltaVCalc(double isp, double Fmass, double Emass)
        {
            double GravityCon = 9.807;
            double DeltaVCal = 0;
            DeltaVCal = Math.Round(isp * GravityCon * Math.Log(Fmass / Emass));
            return DeltaVCal;
        }

        /// <summary>
        /// Males for the Orbit and Landing Contracts.
        /// </summary>
        /// <param name="KerbNumbers"></param>
        public static void SpawnCivilianKerbMCE(int KerbNumbers)
        {
            int TestCivs = 1;                
            foreach (string name2 in SaveInfo.TourisNames)
            {
                if (TestCivs <= KerbNumbers)
                {
                    ProtoCrewMember newKerb = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Tourist);
                    newKerb.rosterStatus = ProtoCrewMember.RosterStatus.Available;
                    newKerb.ChangeName(name2);
                    TestCivs++;
                    //Debug.Log("MCE Created Civilian " + newKerb.name + " " + newKerb.gender);
                }
            }

        }
        /// <summary>
        /// Females for the Station Transfers Contracts
        /// </summary>
        /// <param name="KerbNumbers"></param>
        public static void SpawnCivilianKerbMCE2(int KerbNumbers)
        {
            int TestCivs = 1;            
            foreach (string name2 in SaveInfo.TourisNames2)
            {
                if (TestCivs <= KerbNumbers)
                {
                    ProtoCrewMember newKerb = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Tourist);
                    newKerb.rosterStatus = ProtoCrewMember.RosterStatus.Available;
                    newKerb.ChangeName(name2);
                    TestCivs++;
                    //Debug.Log("MCE Created Civilian " + newKerb.name + " " + newKerb.gender);
                }
            }

        }
        /// <summary>
        /// Both males and females are stored in sepeate list.
        /// </summary>
        public static void CivilianGoHome()
        {
            SaveInfo.TourisNames.Clear();
            CivilianName();         
            foreach (string name2 in SaveInfo.TourisNames)
            {
                HighLogic.CurrentGame.CrewRoster.Remove(name2);
                //Debug.Log("MCE Removed Civilian " + name2);
            }
        }
        public static void CivilianGoHome2()
        {
            SaveInfo.TourisNames2.Clear();
            CivilianName2();
            foreach (string name2 in SaveInfo.TourisNames2)
            {               
                HighLogic.CurrentGame.CrewRoster.Remove(name2);
                //Debug.Log("MCE Removed Civilian " + name2);
            }
        }
      
        public static void CivilianName()
        {
            SaveInfo.TourisNames.Add("Sam Kerbman");
            SaveInfo.TourisNames.Add("Tim Kerbman");
            SaveInfo.TourisNames.Add("Jean Kerbman");
            SaveInfo.TourisNames.Add("Frank Kerbman");
            SaveInfo.TourisNames.Add("Jackson Kerbman");
            SaveInfo.TourisNames.Add("Hoogan Kerbman");
            SaveInfo.TourisNames.Add("Dean Kerbman");
            SaveInfo.TourisNames.Add("John Kerbman");
            SaveInfo.TourisNames.Add("Fredrick Kerbman");
            SaveInfo.TourisNames.Add("Hillbilly Kerbman");
            SaveInfo.TourisNames.Add("Sampson Kerbman");
            SaveInfo.TourisNames.Add("Dick Kerbman");
            SaveInfo.TourisNames.Add("Eliot Kerbman");
            SaveInfo.TourisNames.Add("Father Kerbman");
            SaveInfo.TourisNames.Add("George Kerbman");
            SaveInfo.TourisNames.Add("Obama Kerbman");
            SaveInfo.TourisNames.Add("Hick Kerbman");
            SaveInfo.TourisNames.Add("Rude Kerbman");

           
        }
        public static void CivilianName2()
        {
            SaveInfo.TourisNames2.Add("Lisa Kerbet");
            SaveInfo.TourisNames2.Add("Kara Kerbet");
            SaveInfo.TourisNames2.Add("Wendy Kerbet");
            SaveInfo.TourisNames2.Add("Frita Kerbet");
            SaveInfo.TourisNames2.Add("Janet Kerbet");
            SaveInfo.TourisNames2.Add("Kitty Kerbet");
            SaveInfo.TourisNames2.Add("Frienda Kerbet");
            SaveInfo.TourisNames2.Add("Jill Kerbet");
            SaveInfo.TourisNames2.Add("Jane Kerbet");
            SaveInfo.TourisNames2.Add("Gloria Kerbet");
            SaveInfo.TourisNames2.Add("Glop Kerbet");
            SaveInfo.TourisNames2.Add("Hotstuff Kerbet");
            SaveInfo.TourisNames2.Add("Elizabeth Kerbet");
            SaveInfo.TourisNames2.Add("Michelle Kerbet");
            SaveInfo.TourisNames2.Add("Brenda Kerbet");
            SaveInfo.TourisNames2.Add("Orana Kerbet");
            SaveInfo.TourisNames2.Add("Hiedi Kerbet");
            SaveInfo.TourisNames2.Add("Kat Kerbet");

            
        }
       
        public static void GroundStationRangeHelper(Vessel v, string GstationName, bool inRangeTF)
        {
            ScreenMessages.PostScreenMessage("Ground Station " + GstationName + "In range? " + inRangeTF, .001f);
        }

        public static void ContractLoadCheck<t>(ConfigNode node, ref t value, t backupDefault, string valueName, string savedFile)
        {
            try
            {
                string i;
                i = string.Format(node.GetValue(savedFile));
                value = (t)(object)i;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value for strings " + valueName + " " + savedFile + "Backup Loaded: " + backupDefault + " Node Name: " + node.name);
                Debug.LogWarning(ex.Message + " " + ex.StackTrace);
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
                Debug.LogWarning("MCE Exeption failed to load contract saved value for floats " + valueName + " " + savedFile + "Backup Loaded " + backupDefault + " Node Name: " + node.name);
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
                Debug.LogWarning("MCE Exeption failed to load contract saved value for intergers " + valueName + " " + savedFile + "Backup Loaded " + backupDefault + " Node Name: " + node.name);
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
                Debug.LogWarning("MCE Exeption failed to load contract saved value for doubles " + valueName + " " + savedFile + "Backup Loaded " + backupDefault + " Node Name: " + node.name);
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
                Debug.LogWarning("MCE Exeption failed to load contract saved value for Bools " + valueName + " " + savedFile + " Backup Loaded " + backupDefault + " Node Name: " + node.name);
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
                        //Debug.LogError("Celestrial Body is null from Node Load have to load Backup Value " + backupDefault);
                        value = backupDefault;
                    }
                    else
                        value = (t)(object)cb;
                
            }
            catch (Exception ex)
            {
                Debug.LogWarning("MCE Exeption failed to load contract saved value For Celestrail Bodies " + savedFile + " Backup Loaded " + backupDefault + " Node Name: " + node.id);
                Debug.LogWarning("MCE CelestrialBody Failed Load " + valueName);
                Debug.LogWarning(ex.Source + " " + ex.TargetSite);
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

                public static float GetTotalWeight<GT>(Item<GT>[] p_itens)
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
