using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Contracts;
using Contracts.Parameters;
using KSP;
using System.Text;
using KSPAchievements;
using MissionControllerEC.MCEParameters;

namespace MissionControllerEC.MCEContracts
{
    //public class RoverContracts : Contract
    //{
    //    private double RcLatitude;
    //    private double RcLongitude;
    //    public int totalContracts;
    //    public int TotalFinished;
    //    private string WheelModule = "ModuleWheel";
    //    CelestialBody targetBody; 


    //    protected override bool Generate()
    //    {
    //        if (HighLogic.LoadedSceneIsFlight) { return false; }
    //        totalContracts = ContractSystem.Instance.GetCurrentContracts<RoverContracts>().Count();
    //        TotalFinished = ContractSystem.Instance.GetCompletedContracts<RoverContracts>().Count();

    //        if (totalContracts >= 1)
    //        {
    //            return false;
    //        }
    //        targetBody = FlightGlobals.Bodies[6];
    //        if (targetBody == null)
    //        {
    //            return false;
    //        }
    //        RcLatitude = 1;
    //        RcLongitude = 1;

    //        this.AddParameter(new CheckLandingLonAndLat(targetBody, false, RcLongitude, RcLatitude, "Land Rover", true, true),null);
    //        this.AddParameter(new ModuleGoal(WheelModule, "Wheels"), null);
    //        this.AddParameter(new GetCrewCount(0), null);
    //        base.SetExpiry(3f, 10f);
    //        base.SetDeadlineYears(3f, targetBody);
    //        base.SetFunds(5000, 70000, 90000, targetBody);
    //        base.SetReputation(25, 50, targetBody);
    //        base.SetScience(5, targetBody);
    //        return true;
    //    }
    //    public override bool CanBeCancelled()
    //    {
    //        return true;
    //    }
    //    public override bool CanBeDeclined()
    //    {
    //        return true;
    //    }

    //    protected override string GetNotes()
    //    {
    //        return "Test Notes";
    //    }

    //    protected override string GetHashString()
    //    {
    //        return targetBody.bodyName + " Land rover " + " - Total Done: " + TotalFinished + this.MissionSeed.ToString();
    //    }
    //    protected override string GetTitle()
    //    {
    //        return "Launch Rover To " + targetBody.theName;
    //    }
    //    protected override string GetDescription()
    //    {
    //        return "Land on duna with a rover";
    //    }
    //    protected override string GetSynopsys()
    //    {
    //        return "You must land on duna with a rover yes";
    //    }
    //    protected override string MessageCompleted()
    //    {          
    //        return "Good job landing on " + targetBody.theName;
    //    }

    //    protected override void OnLoad(ConfigNode node)
    //    {
    //        Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "DunaTarget");
    //        Tools.ContractLoadCheck(node, ref RcLatitude, 0, RcLatitude, "RcLatitude");
    //        Tools.ContractLoadCheck(node, ref RcLongitude, 0, RcLongitude, "RcLongitude");
    //        Tools.ContractLoadCheck(node, ref WheelModule, "ModuleWheel", WheelModule, "WheelMod");         
    //    }
    //    protected override void OnSave(ConfigNode node)
    //    {
    //        int bodyID = targetBody.flightGlobalsIndex;
    //        node.AddValue("DunaTarget", bodyID);          
    //        node.AddValue("RcLatitude", RcLatitude);
    //        node.AddValue("RcLongitude", RcLongitude);
    //        node.AddValue("WheelMod", WheelModule);
    //    }

    //    public override bool MeetRequirements()
    //    {
    //        bool techUnlock = ResearchAndDevelopment.GetTechnologyState("basicScience") == RDTech.State.Available;
    //        bool techUnlock2 = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
    //        if (techUnlock && techUnlock2)
    //            return true;
    //        else
    //            return false;
    //    }
    //}
}
