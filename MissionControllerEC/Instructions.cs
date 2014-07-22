using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MissionControllerEC
{
    public class Instructions
    {
        private bool TempCheck = false;       
        public List<CurrentHires> currentHires = new List<CurrentHires>();

        private Settings settings
        {
            get { return SettingsManager.Manager.getSettings(); }
        }
                
        public bool MceRDScience
        {
            get { return ResearchAndDevelopment.Instance != null; }
        }
       
        public float MceScience
        {
            get
            {
                return ResearchAndDevelopment.Instance.Science;
            }
            set
            {
                float previous = ResearchAndDevelopment.Instance.Science;
                ResearchAndDevelopment.Instance.Science = value;
                Debug.LogError("Mission Controller Changed Science by " + (ResearchAndDevelopment.Instance.Science - previous) + " to " + ResearchAndDevelopment.Instance.Science + ".");
            }
        }

        public float MceScienceCap
        {
            get { return ResearchAndDevelopment.Instance.ScienceCap; }
            set
            {
                float previous = ResearchAndDevelopment.Instance.ScienceCap;
                ResearchAndDevelopment.Instance.ScienceCap = value;
                Debug.LogError("Mission Controller Changed ScienceCap by " + (ResearchAndDevelopment.Instance.ScienceCap - previous) + " to " + ResearchAndDevelopment.Instance.ScienceCap + ".");

            }
        }
       
        public float DeductScience(float cost)
        {
            return MceScience -= cost;
        }

        public double MceFunds
        {
            get { return Funding.Instance.Funds; }
            set
            {
                double previous = Funding.Instance.Funds;
                Funding.Instance.Funds = value;
                Debug.LogError("Mission Controller Changed Funds by " + (Funding.Instance.Funds - previous) + " To " + Funding.Instance.Funds + ".");
            }           
        }

        public float MceReputation
        {
            get { return Reputation.Instance.reputation; }            
        }

        public void GetHiredKerbals()
        {            
            currentHires.Clear();
            foreach (ProtoCrewMember CrewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {               
                currentHires.Add(new CurrentHires(CrewMember.name));               
            }
            Debug.LogWarning("Crew Roster CurrentHires Populated!");
            TempCheck = false;
        }

        public void isKerbalHired()
        {          
            foreach (ProtoCrewMember CrewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Available || CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
                {

                    if (!currentHires.Exists(H => H.hiredKerbalName == CrewMember.name) && !HighLogic.LoadedSceneIsFlight)
                    {
                        settings.totalKerbalCost += 1000; 
                        settings.NewHires.Add(new CurrentHires(CrewMember.name));
                        Debug.LogWarning("This Kerbal Was Just Hired: " + CrewMember.name + " At " + Planetarium.GetUniversalTime() + " Crew Roster CurrentHires Re-Populated!");
                        Debug.LogWarning("totalKerbalFunds is: " + settings.totalKerbalCost);
                        TempCheck = true;                      
                    }

                }
            }
            if (TempCheck)
            {
                GetHiredKerbals();
                MissionControllerEC.ShowPopUpWindow = !MissionControllerEC.ShowPopUpWindow;
            }
 
        }       
    }

    public class CurrentHires
    {
        public string hiredKerbalName;        

        public CurrentHires()
        {
        }

        public CurrentHires(string kerbalname)
        {
            this.hiredKerbalName = kerbalname;
        }
    }
}
