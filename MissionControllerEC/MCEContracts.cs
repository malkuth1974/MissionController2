using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using KSPAchievements;


namespace MissionControllerEC
{
    
    public class DeliverSatellite : Contract
    {
        CelestialBody targetBody = null;
        public double aMaxApa = UnityEngine.Random.Range(75000, 300000);
        public double aMinPeA = 0;
        public bool techUnlocked = false;
        public double MinInc = 0;
        public double MaxInc = UnityEngine.Random.Range(0, 90);
        public int test = UnityEngine.Random.Range(0, 100);

        protected override void OnDeclined()
        {
            MissionControllerEC.TestThis = false;
            Debug.Log("DelieverSat Period reset");
        }
        protected override void OnCancelled()
        {
            MissionControllerEC.TestThis = false;
            Debug.Log("DelieverSat Period reset");
        }

        protected override bool Generate()
        {
            if (MissionControllerEC.TestThis)
            {
                Debug.Log("contract is generated right now terminating DeliverSat");
                return false;
            }
            Debug.Log("contracts exist" + contractsInExistance);
            Debug.Log("Contract State" + ContractState);

            targetBody = Planetarium.fetch.Home;
            aMinPeA = aMaxApa - 5000;
            MinInc = MaxInc - 10;
            bool ifInclination = false;

            if (test >= 50)
                ifInclination = true;
            else
                ifInclination = false;
            this.AddParameter(new OrbitGoal(targetBody, (double)aMaxApa, (double)aMinPeA), null);
            if (ifInclination)
            {
                this.AddParameter(new Inclination(MinInc, MaxInc));
            }
            base.SetExpiry();
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(15f, 35f, targetBody);
            if (!ifInclination)
                base.SetFunds(6000f, 22000f, 7000f, targetBody);
            else
                base.SetFunds(8000f, 24000f, 9000f, targetBody);

            Debug.Log(contractsInExistance + ContractState);
            MissionControllerEC.TestThis = true;
            return true;

        }
       
        public override bool CanBeCancelled()
        {
            return true;
        }
        public override bool CanBeDeclined()
        {
            return true;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + aMaxApa.ToString() + aMinPeA.ToString();
        }
        protected override string GetTitle()
        {
            return "Launch Satellite Into Orbit. " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", new System.Random().Next());
        }
        protected override string GetSynopsys()
        {
            return "Bring satellite to orbit " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            MissionControllerEC.TestThis = false;
            return "You have successfully delivered our satellite to orbit " + targetBody.theName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double ApaID = double.Parse(node.GetValue("aPa"));
            aMaxApa = ApaID;
            double PeAID = double.Parse(node.GetValue("pEa"));
            aMinPeA = PeAID;
            double maxincID = double.Parse(node.GetValue("maxincID"));
            MaxInc = maxincID;
            double minincID = double.Parse(node.GetValue("minincID"));
            MinInc = minincID;          
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = aMaxApa;
            node.AddValue("aPa", ApAID);
            double PeAID = aMinPeA;
            node.AddValue("pEa", PeAID);
            double maxincID = MaxInc;
            node.AddValue("maxincID", MaxInc);
            double minincID = MinInc;
            node.AddValue("minincID", MinInc);           
        }
        
        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("flightControl") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }

    public class DeliverSatOrbitalPeriod : Contract
    {
        CelestialBody targetBody = null;

        public double MinOrb = 21480;
        public double MaxOrb = 21660;
        public double MinInc = 0;
        public double MaxInc = UnityEngine.Random.Range(0, 90);
        public int test = UnityEngine.Random.Range(0, 100);
        public bool testThis = false;

        protected override void OnDeclined()
        {
            MissionControllerEC.TestThis2 = false;
            Debug.Log("DelieverSat Period reset");
        }
        protected override void OnCancelled()
        {
            MissionControllerEC.TestThis2 = false;
            Debug.Log("DelieverSat Period reset");
        }

        protected override bool Generate()
        {            
            if (MissionControllerEC.TestThis2)
            {
                Debug.Log("contract is generated right now terminating OrbitalPeriod");
                return false;               
            }

            targetBody = Planetarium.fetch.Home;
            MinInc = MaxInc - 10;
            bool ifInclination = false;

            if (test >= 50)
                ifInclination = true;
            else
                ifInclination = false;
            this.AddParameter(new OrbitalPeriod(MinOrb, MaxOrb), null);
            if (ifInclination)
            {
                this.AddParameter(new Inclination(MinInc, MaxInc));
            }
            base.SetExpiry();
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(15f, 35f, targetBody);
            if (!ifInclination)
                base.SetFunds(8000f, 27000f, 10000f, targetBody);
            else
                base.SetFunds(10000f, 28000f, 11000f, targetBody);

            MissionControllerEC.TestThis2 = true;
            return true;
        }
       
        public override bool CanBeCancelled()
        {
            return true;
        }
        public override bool CanBeDeclined()
        {
            return true;
        }

        protected override string GetHashString()
        {
            return targetBody.bodyName + MaxOrb + MinOrb;
        }
        protected override string GetTitle()
        {
            return "Launch Satellite Into a 6 Hour Orbital Period. " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            //those 3 strings appear to do nothing
            return TextGen.GenerateBackStories(Agent.Name, Agent.GetMindsetString(), "Sat", "Light", "Oh pretty", new System.Random().Next());
        }
        protected override string GetSynopsys()
        {
            return "Bring satellite to orbital Period of 6 Hours " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            MissionControllerEC.TestThis2 = false;
            return "You have successfully delivered our satellite to orbit " + targetBody.theName;
        }

        protected override void OnLoad(ConfigNode node)
        {
            int bodyID = int.Parse(node.GetValue("targetBody"));
            foreach (var body in FlightGlobals.Bodies)
            {
                if (body.flightGlobalsIndex == bodyID)
                    targetBody = body;
            }
            double ApaID = double.Parse(node.GetValue("aPa"));
            MaxOrb = ApaID;
            double PeAID = double.Parse(node.GetValue("pEa"));
            MinOrb = PeAID;
            double maxincID = double.Parse(node.GetValue("maxincID"));
            MaxInc = maxincID;
            double minincID = double.Parse(node.GetValue("minincID"));
            MinInc = minincID;
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = MaxOrb;
            node.AddValue("aPa", ApAID);
            double PeAID = MinOrb;
            node.AddValue("pEa", PeAID);
            double maxincID = MaxInc;
            node.AddValue("maxincID", MaxInc);
            double minincID = MinInc;
            node.AddValue("minincID", MinInc);
        }

        //for testing purposes
        public override bool MeetRequirements()
        {
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advFlightControl") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
    }
}
