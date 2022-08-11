using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ClickThroughFix;

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

        static internal void StartBodySelection(CallBackFunction c, bool comsat = false, bool landingOrbit = false,
            bool buildSpaceStation = false)
        {
            callback = c;
            GameObject gameObject = new GameObject();
            MissionControllerEC.bodySelWin = gameObject.AddComponent<BodySelection>();

            if (comsat)
                selectedBody = FlightGlobals.Bodies[SaveInfo.comSatBodyName];
            if (landingOrbit)
                selectedBody = FlightGlobals.Bodies[SaveInfo.LandingOrbitIDX];
            if (buildSpaceStation)
                selectedBody = FlightGlobals.Bodies[SaveInfo.BuildSpaceStationIDX];
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
        static CelestialBody selectedBody;
        int originalcomSatBodyName, originalLandingOrbitIDX, originalBuildSpaceStationIDX;

        static Vector2 sitesScrollPosition;
        static BodyTypeFilter  filter = BodyTypeFilter.ALL;


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
            win = ClickThruBlocker.GUILayoutWindow(activeWinID, win, BodySelectionWin, "Body Selection"); //, LifeSupportDisplay.layoutOptions);
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
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Currently selected body:");
                GUILayout.TextField(selectedBody.bodyDisplayName.Substring(0, selectedBody.bodyDisplayName.Length-2));

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
                                if (GUILayout.Button(indent + b.bodyName, RegisterToolbar.buttonLeft))
                                {
                                    selectedBody = b;
                                    SetSelectedbody(selectedBody);
                                }
                            }
                            else
                            {
                                string prefix;
                                prefix = indent + "   |--> ";
                                if (GUILayout.Button(prefix + b.bodyName, RegisterToolbar.buttonLeft))
                                {
                                    selectedBody = b;
                                    SetSelectedbody(selectedBody);
                                }
                            }
                        }
                        else
                        {
                            if (GUILayout.Button(indent + b.bodyName, RegisterToolbar.buttonLeft))
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
                    if (GUILayout.Button("Accept"))
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
