using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{
    #region Resource Supply Goal Check
    public class ResourceSupplyGoal : ContractParameter
    {
        private string targetName;
        private double ResourceAmount = 0.0f;
        private string contractTitle;
        private double resources = 0.0;
        private bool updated = false;

        public ResourceSupplyGoal()
        {
        }

        public ResourceSupplyGoal(string target, double RsAmount, string Ctitle)
        {
            this.targetName = target;
            this.ResourceAmount = RsAmount;
            this.contractTitle = Ctitle;
        }

        protected override string GetHashString()
        {
            return targetName;
        }
        protected override string GetTitle()
        {
            return contractTitle + " " + ResourceAmount + " " + targetName;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active && HighLogic.LoadedSceneIsFlight)
                OnResourceCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            targetName = node.GetValue("targetname");
            ResourceAmount = float.Parse(node.GetValue("resourceamount"));
            contractTitle = node.GetValue("contracttitle");
        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("targetname", targetName);
            node.AddValue("resourceamount", ResourceAmount);
            node.AddValue("contracttitle", contractTitle);
        }

        private void OnResourceCheck(Vessel v)
        {
            if (v.launchTime > this.Root.DateAccepted)
            {
                if (v != null)
                {
                    foreach (Part p in v.parts)
                    {
                        if (p.Resources[targetName])
                        {
                            resources = +p.Resources[targetName].amount;
                        }
                    }
                    if (resources > 0)
                    {
                        if (resources >= ResourceAmount)
                        {
                            base.SetComplete();
                        }
                    }
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region TotalMassGoal
    public class TotalMasGoal : ContractParameter
    {
        private CelestialBody targetBody;
        private float maxweight = 0.0f;
        private bool updated = false;

        public TotalMasGoal()
        {
        }

        public TotalMasGoal(CelestialBody target, float maxWeight)
        {
            this.targetBody = target;
            this.maxweight = maxWeight;
        }
        protected override string GetHashString()
        {
            return targetBody.bodyName;
        }
        protected override string GetTitle()
        {
            return "Satellite Mass Must Not Exceed: " + maxweight.ToString("F2") + " Tons. (InOrbit)";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                MassCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }

            maxweight = float.Parse(node.GetValue("maxtons"));


        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("maxtons", maxweight);

        }

        public void MassCheck(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    if (vessel.GetTotalMass() <= maxweight)
                    {
                        base.SetComplete();
                    }
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region Resource Goal Check
    public class ResourceGoal : ContractParameter
    {
        private string targetName;
        private double maxAmountt = 0.0f;
        private double minAmount = 0.0f;
        private bool updated = false;

        /// <summary>
        /// Returns the name of the Resource goal for this parameter
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string iTargetName(ContractParameter cp)
        {
            ResourceGoal instance = (ResourceGoal)cp;
            return instance.targetName;
        }

        public ResourceGoal()
        {
        }

        public ResourceGoal(string target, double maxAmt, double minAmt)
        {
            this.targetName = target;
            this.maxAmountt = maxAmt;
            this.minAmount = minAmt;
        }
        protected override string GetHashString()
        {
            return targetName;
        }
        protected override string GetTitle()
        {
            return "Must Have " + targetName + " Between " + minAmount + " and " + maxAmountt + " (InOrbit)";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                ResourceCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            targetName = node.GetValue("targetname");
            maxAmountt = float.Parse(node.GetValue("maxtons"));
            minAmount = float.Parse(node.GetValue("mintons"));


        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("targetname", targetName);
            node.AddValue("maxtons", maxAmountt);
            node.AddValue("mintons", minAmount);

        }

        public void ResourceCheck(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    double resources = 0;

                    if (vessel != null)
                    {
                        foreach (Part p in vessel.parts)
                        {
                            if (p.Resources[targetName] != null)
                            {
                                resources += p.Resources[targetName].amount;
                            }
                        }
                        if (resources > 0)
                        {
                            if (resources >= minAmount && resources <= maxAmountt)
                            {
                                base.SetComplete();
                            }
                        }
                    }
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region Resource Goal Cap Check
    public class ResourceGoalCap : ContractParameter
    {
        public bool updated = false;
        public string targetName;
        public double RsAmount = 0.0f;

        /// <summary>
        /// Returns the name of the Resource goal for this parameter
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string iTargetName(ContractParameter cp)
        {
            ResourceGoalCap instance = (ResourceGoalCap)cp;
            return instance.targetName;
        }

        public ResourceGoalCap()
        {
        }

        public ResourceGoalCap(string target, double rsAmount)
        {
            this.targetName = target;
            this.RsAmount = rsAmount;
        }
        protected override string GetHashString()
        {
            return targetName;
        }
        protected override string GetTitle()
        {
            return "Must Have " + targetName + " Greater Than " + RsAmount + " (InOrbit)";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                ResourceCheck(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {

            targetName = node.GetValue("targetname");
            RsAmount = float.Parse(node.GetValue("mintons"));


        }
        protected override void OnSave(ConfigNode node)
        {

            node.AddValue("targetname", targetName);
            node.AddValue("mintons", RsAmount);

        }

        public void ResourceCheck(Vessel vessel)
        {
            if (vessel.launchTime > this.Root.DateAccepted)
            {
                if (vessel.isActiveVessel)
                {
                    double resources = 0;

                    if (vessel != null)
                    {
                        foreach (Part p in vessel.parts)
                        {
                            if (p.Resources[targetName] != null)
                            {
                                resources += p.Resources[targetName].amount;
                            }
                        }
                        if (resources > 0)
                        {
                            if (resources >= RsAmount)
                            {
                                base.SetComplete();
                            }
                        }
                    }
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion

}
