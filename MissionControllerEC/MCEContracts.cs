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
        //public string sTitle = "Stage Satellite Once Get Into Assigned Orbit";
        //public string sStage = "Stage Satellite From InterStage";


            

        protected override bool Generate()
        {
            targetBody = Planetarium.fetch.Home;
            aMinPeA = aMaxApa - 5000;       
            this.AddParameter(new OrbitGoal(targetBody, (double)aMaxApa, (double)aMinPeA), null);
            //this.AddParameter(new StageEvent(sStage, sTitle), null);
            base.SetExpiry();
            base.SetScience(2.25f, targetBody);
            base.SetDeadlineYears(1f, targetBody);
            base.SetReputation(15f, 35f, targetBody);
            base.SetFunds(4000f, 10000f, 7000f, targetBody);
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
            //string stageID = (node.GetValue("stageID"));
            //sStage = stageID;
            //string titleID = (node.GetValue("titleID"));
            //sTitle = titleID;
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            double ApAID = aMaxApa;
            node.AddValue("aPa", ApAID);
            double PeAID = aMinPeA;
            node.AddValue("pEa", PeAID);
            //string stageID = sStage;
            //node.AddValue("stageID", stageID);
            //string titleID = sTitle;
            //node.AddValue("titleID", titleID);
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
}
