using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Contracts;
using Contracts.Parameters;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        private bool TempCheck = false; 
        public List<CurrentHires> currentHires = new List<CurrentHires>();
        public double totalKerbalCost;
        public List<CurrentHires> NewHires = new List<CurrentHires>();
        public List<CurrentHires> totalActiveKerbals = new List<CurrentHires>();


        public void onContractLoaded()
        {
            if (settings.NoRescueKerbalContracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.RescueKerbal)))
            {
                ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.RescueKerbal));
                Debug.Log("Removed RescueKerbal Type Contracts from Gererating");
            }
            Debug.Log("On Contract Loaded event fired at startup");
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
                        totalKerbalCost += settings.HireCost; 
                        NewHires.Add(new CurrentHires(CrewMember.name));
                        Debug.LogWarning("This Kerbal Was Just Hired: " + CrewMember.name + " At " + Planetarium.GetUniversalTime() + " Crew Roster CurrentHires Re-Populated!");
                        Debug.LogWarning("totalKerbalFunds is: " + totalKerbalCost);
                        TempCheck = true;                      
                    }

                }
            }
            if (TempCheck)
            {
                GetHiredKerbals();
                StringBuilder Message = new StringBuilder();
                Message.AppendLine("The Following Kerbals Were Hired");
                Message.AppendLine();
                foreach (CurrentHires ch in NewHires)
                {
                    Message.AppendLine(ch.hiredKerbalName + "For Cost of: " + settings.HireCost);
                }
                Message.AppendLine();
                Message.AppendLine("Total Cost Of New Recruits: " + totalKerbalCost);
                Message.AppendLine("Hired On Date: " + Tools.formatTime(Planetarium.GetUniversalTime()));
                while (HighLogic.LoadedScene == GameScenes.LOADING) { Debug.Log("Test"); }
                Funding.Instance.Funds -= totalKerbalCost;
                saveinfo.TotalSpentKerbals += totalKerbalCost;
                MessageSystem.Message m = new MessageSystem.Message(
                "Hired New Kerbals",
                Message.ToString(),
                MessageSystemButton.MessageButtonColor.YELLOW,
                MessageSystemButton.ButtonIcons.MESSAGE);
                MessageSystem.Instance.AddMessage(m);
                NewHires.Clear();
                totalKerbalCost = 0;
                saveinfo.Save();
            }
 
        }
        
        public void getActiveKerbals()
        {
            totalActiveKerbals.Clear();
            foreach (ProtoCrewMember CrewMember in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                if (CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Available || CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Assigned)
                {
                    totalActiveKerbals.Add(new CurrentHires(CrewMember.name));
                }
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
    public class RepairVesselsList
    {
        public string vesselName;
        public string vesselId;
        public RepairVesselsList()
        {
        }
        public RepairVesselsList(string name, string id)
        {
            this.vesselName = name;
            this.vesselId = id;
        }


    }
}
