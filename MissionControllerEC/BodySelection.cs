using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MissionControllerEC
{
    internal class BodySelection:MonoBehaviour
    {
        public delegate void CallBackFunction();

        static CallBackFunction callback;
        internal enum BodyTypeFilter
        {
            ALL,
            PLANETS,
            MOONS
        }

        static internal void StartBodySelection(CallBackFunction c)
        {
            callback = c;
            GameObject gameObject = new GameObject();
            MissionControllerEC.bodySelWin = gameObject.AddComponent<BodySelection>();
        }

        List<CelestialBody> GetAllowableBodies(BodyTypeFilter filter = BodyTypeFilter.ALL)
        {
            var allRegisteredBodies = FindObjectsOfType(typeof(CelestialBody)).OfType<CelestialBody>();
            List<CelestialBody> allowableBodies = allRegisteredBodies.Where(body => {
                CelestialBody parent = body.orbit != null && body.orbit.referenceBody != null ? body.orbit.referenceBody : null;
                var isPseudoObject = !body.isStar && (body.orbit == null || Double.IsInfinity(body.sphereOfInfluence));
                if (isPseudoObject)
                    return false;

                switch (filter)
                {
                    case BodyTypeFilter.ALL:
                        return parent != null && parent.isStar;
                        //return true;
                    case BodyTypeFilter.PLANETS:
                        return parent != null && parent.isStar;
                    case BodyTypeFilter.MOONS:
                        return parent != null && !parent.isStar;
                    default:
                        return false;
                }
            }).ToList();

            // This puts all the moons immediately following the parent
            if (filter == BodyTypeFilter.ALL)
            {
                List<CelestialBody> bodiesAndMoons = new List<CelestialBody>();
                foreach (var body in allowableBodies)
                {
                    bodiesAndMoons.Add(body);
                    if (body.orbitingBodies != null)
                    {
                        foreach (var moon in body.orbitingBodies)
                        {
                            bodiesAndMoons.Add(moon);
                        }
                    }
                }
                return bodiesAndMoons;
            }
            else
                return allowableBodies;
        }

        static int activeWinID = 353465756;
        static Rect win = new Rect(100,100, 300, 600);
        static List<CelestialBody> celestialBodies = new List<CelestialBody>();
        CelestialBody selectedBody;
        int originalcomSatBodyName, originalLandingOrbitIDX, originalBuildSpaceStationIDX;

        static Vector2 sitesScrollPosition;
        static BodyTypeFilter  filter = BodyTypeFilter.ALL;

        GUIStyle buttonLeft;
        bool initted = false;

        const string indent = "     ";
        void Start()
        {
            originalcomSatBodyName = SaveInfo.comSatBodyName;
            originalLandingOrbitIDX = SaveInfo.LandingOrbitIDX;
            originalBuildSpaceStationIDX = SaveInfo.BuildSpaceStationIDX;
        }

        void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            if (!initted)
            {
                initted = true;
                buttonLeft = new GUIStyle(GUI.skin.button);
                buttonLeft.alignment = TextAnchor.MiddleLeft;
            }
            win = GUILayout.Window(activeWinID, win, BodySelectionWin, "Body Selection"); //, LifeSupportDisplay.layoutOptions);
        }

        void BodySelectionWin(int id)
        {
            if (celestialBodies.Count == 0)
            {
                celestialBodies = GetAllowableBodies(filter);
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Planets"))
                {
                    filter = BodyTypeFilter.PLANETS;
                    celestialBodies.Clear();
                }
                if (GUILayout.Button("Moons"))
                {
                    filter = BodyTypeFilter.MOONS;
                    celestialBodies.Clear();
                }
                if (GUILayout.Button("All"))
                {
                    filter = BodyTypeFilter.ALL;
                    celestialBodies.Clear();
                }
            }

            using (new GUILayout.VerticalScope())
            {
                sitesScrollPosition = GUILayout.BeginScrollView(sitesScrollPosition);
                foreach (var b in celestialBodies)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (filter == BodyTypeFilter.ALL)
                        {
                            CelestialBody parent = b.orbit != null && b.orbit.referenceBody != null ? b.orbit.referenceBody : null;
                            if (parent != null && parent.isStar)
                            {
                                if (GUILayout.Button(indent + b.bodyName, buttonLeft))
                                {
                                    selectedBody = b;
                                    SetSelectedbody(selectedBody);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(indent + "   |--> " + b.bodyName, buttonLeft))
                                {
                                    selectedBody = b;
                                    SetSelectedbody(selectedBody);
                                }
                            }
                        }
                        else
                        {
                            if (GUILayout.Button(indent + b.bodyName, buttonLeft))
                            {
                                selectedBody = b;
                                SetSelectedbody(selectedBody);
                            }
                        }
                    }

                }
                GUILayout.EndScrollView();
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Close"))
                    {
                        MissionControllerEC.bodySelWin = null;
                        Destroy(this);
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        SaveInfo.comSatBodyName = originalcomSatBodyName;
                        SaveInfo.LandingOrbitIDX = originalLandingOrbitIDX;
                        SaveInfo.BuildSpaceStationIDX = originalBuildSpaceStationIDX;
                        callback();
                        MissionControllerEC.bodySelWin = null;
                        Destroy(this);
                    }
                }
            }
            GUI.DragWindow();
        }

        void SetSelectedbody(CelestialBody selectedBody)
        {
            for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
            {
                if (FlightGlobals.Bodies[i] == selectedBody)
                {
                    SaveInfo.comSatBodyName = i;
                    SaveInfo.LandingOrbitIDX = i;
                    SaveInfo.BuildSpaceStationIDX = i;
                }
            }
            callback();
        }

    }
}
