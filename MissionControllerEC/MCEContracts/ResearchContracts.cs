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
using MissionControllerEC.PartModules;
using KSP.Localization;

namespace MissionControllerEC.MCEContracts
{      
    #region OrbitalScan Contract
    public class MCE_orbital_Scan_Contract : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        public double testpos = 0;
        int crewCount = 0;
        string partName = "Ionization Chamber";
        int partNumber = 1;
        double missionTime = 0;
        public int totalContracts = 0, TotalFinished = 0;
        public ContractParameter orbitresearch2;

        protected override bool Generate()
        {           
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            targetBody = GetUnreachedTargets();
            if (targetBody == null)
            {
                //Debug.LogWarning("Orbital Research Has No Valid Target bodies contract rejected");
                return false;                
            }
            //Debug.LogWarning("Orbit Research Body is " + targetBody.bodyName);          
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().OrbitalScienceContracts)
            {
                //Debug.LogWarning("Orbit Research Random Selection is false, contract not Generated.");
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<MCE_orbital_Scan_Contract>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<MCE_orbital_Scan_Contract>().Count();
            if (totalContracts >= HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().ScienceContractNumbers)
            {
                //Debug.LogWarning("Orbit Research Already Generated, only 1 contract at time please.");
                return false;
            }
            
            missionTime = Tools.RandomNumber(200, 1500);

            // Build New orbits Using KSP Build Orbits.. Simple inclanations becuase of ground stations
            Orbit o = FinePrint.Utilities.OrbitUtilities.GenerateOrbit(MissionSeed, targetBody, FinePrint.Utilities.OrbitType.EQUATORIAL, .1, 0, 0);
            //Using Fineprint to double check its own calculations.
            FinePrint.Utilities.OrbitUtilities.ValidateOrbit(MissionSeed, ref o, FinePrint.Utilities.OrbitType.EQUATORIAL, .1, 0);
            //Debug.Log("MCE Research Contract Orbit Values: " + " APA " + o.ApA + " PEA " + o.PeA + " Seed Number " + MissionSeed.ToString());

            this.AddParameter(new FinePrint.Contracts.Parameters.SpecificOrbitParameter(FinePrint.Utilities.OrbitType.EQUATORIAL, o.inclination, o.eccentricity, o.semiMajorAxis, o.LAN, o.argumentOfPeriapsis, o.meanAnomalyAtEpoch, o.epoch, targetBody, 4), null);           
            this.orbitresearch2 = this.AddParameter(new OrbialResearchPartCheck(targetBody, missionTime), null);
            orbitresearch2.SetFunds(5000, targetBody);
            orbitresearch2.SetReputation(5, targetBody);
            orbitresearch2.SetScience(2, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber,false), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(5f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(5f, 3f, targetBody);
            base.SetFunds(8000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 90000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 90000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            return targetBody.bodyName + " " + TotalFinished + this.MissionSeed.ToString();
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000167") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000167 = Ionization Scan of  
        }
        protected override string GetDescription()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000159") +		// #autoLOC_MissionController2_1000159 = An ionization chamber measures the charge from the number of ion pairs created within a gas caused by incident radiation.[2] It consists of a gas-filled chamber with two electrodes; known as anode and cathode. The electrodes may be in the form of parallel plates (Parallel Plate Ionization Chambers: PPIC), or a cylinder arrangement with a coaxially located internal anode wire.\n\n voltage potential is applied between the electrodes to create an electric field in the fill gas. When gas between the electrodes is ionized by incident ionizing radiation, ion-pairs are created and the resultant positive ions and dissociated electrons move to the electrodes of the opposite polarity under the influence of the electric field. This generates an ionization current which is measured by an electrometer circuit. The electrometer must be capable of measuring the very small output current which is in the region of femtoamperes to picoamperes, depending on the chamber design, radiation dose and applied voltage.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000160") +		// #autoLOC_MissionController2_1000160 = Each ion pair created deposits or removes a small electric charge to or from an electrode, such that the accumulated charge is proportional to the number of ion pairs created, and hence the radiation dose. This continual generation of charge produces an ionization current, which is a measure of the total ionizing dose entering the chamber. However, the chamber cannot discriminate between radiation types (beta or gamma) and cannot produce an energy spectrum of radiation.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000161") +		// #autoLOC_MissionController2_1000161 = The electric field also enables the device to work continuously by mopping up electrons, which prevents the fill gas from becoming saturated, where no more ions could be collected, and by preventing the recombination of ion pairs, which would diminish the ion current. This mode of operation is referred to as current mode, meaning that the output signal is a continuous current, and not a pulse output as in the cases of the Geiger-Müller tube or the proportional counter.\n\n

            Localizer.Format("#autoLOC_MissionController2_1000162");		// #autoLOC_MissionController2_1000162 = Referring to the accompanying ion pair collection graph, it can be seen that in the ion chamber operating region the collection of ion pairs is effectively constant over a range of applied voltage, as due to its relatively low electric field strength the ion chamber does not have any multiplication effect. This is in distinction to the Geiger-Müller tube or the proportional counter whereby secondary electrons, and ultimately multiple avalanches, greatly amplify the original ion-current charge.
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000163");		// #autoLOC_MissionController2_1000163 = Vessel must be a new vessel launched after accepting this contract!
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000164") + " " + targetBody.bodyName;		// #autoLOC_MissionController2_1000164 = Orbit And Conduct Ionization Scan Of 
        }
        protected override string MessageCompleted()
        {
            MCEOrbitalScanning.doOrbitResearch = false;
            return Localizer.Format("#autoLOC_MissionController2_1000165") + " " + targetBody.bodyName + " " + Localizer.Format("#autoLOC_MissionController2_1000166");       // #autoLOC_MissionController2_1000165 = You have reached the target body    		// #autoLOC_MissionController2_1000166 = , and conducted a Ionization Scan.  We have learned a lot of new information about the composition of this planetary body in preparation for a possible landing in the future by our manned program or Robotic Legions.

        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody,Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref partName, "Orbital Research Scanner", partName, "partname");
            Tools.ContractLoadCheck(node, ref partNumber, 1, partNumber, "maxcount");
            Tools.ContractLoadCheck(node, ref missionTime, 10000, missionTime, "missiontime");           

        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("crewcount", crewCount);
            node.AddValue("partname", partName);
            node.AddValue("maxcount", partNumber);
            node.AddValue("missiontime", missionTime);          
        }

        public override bool MeetRequirements()
        {           
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("basicScience") == RDTech.State.Available;
            if (techUnlock)
            {
                //Debug.LogWarning("Attempting To generate Orbital Research Contract");
                return true;
            }
            else
                return false;
        }

        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(true, false);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
            }
            else
            {
                return null;
            }
            return null;
        }
    }
    #endregion
    #region Lander Scanning Contract
    public class MCE_Lander_Research_Scan : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        int crewCount = 0;
        string partName = "Mass Spectrometry Tube";
        int partNumber = 1;
        double amountTime = 5000;
        public int totalContracts;
        public int TotalFinished;
        ContractParameter landerscan1;
        ContractParameter landerscan2;
        ContractParameter landerscan3;

        protected override bool Generate()
        {            
            if (HighLogic.LoadedSceneIsFlight) { return false; }                  
            if (!HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings>().GroundScienceContracts)
            {
                //Debug.LogWarning("Lander Research Contracts random set to false No contract generated.");
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<MCE_Lander_Research_Scan>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<MCE_Lander_Research_Scan>().Count();
            if (totalContracts >= HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().ScienceContractNumbers)
            {
                //Debug.LogWarning("Lander Research Already Generated, only 1 contract at time please.");
                return false;
            }
            amountTime = Tools.RandomNumber(200, 1500);
            targetBody = GetUnreachedTargets();
            if (targetBody == null)
            {
                return false;
            }
            this.landerscan1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            landerscan1.SetFunds(2500, targetBody);
            landerscan1.SetReputation(4, targetBody);
            landerscan1.SetScience(2, targetBody);
            this.landerscan2 = this.AddParameter(new LandingParameters(targetBody, true), null);
            landerscan2.SetFunds(2500, targetBody);
            landerscan2.SetReputation(5, targetBody);
            landerscan2.SetScience(3, targetBody);
            this.landerscan3 = this.AddParameter(new LanderResearchPartCheck(targetBody, amountTime), null);
            landerscan3.SetFunds(4000, targetBody);
            landerscan3.SetReputation(8, targetBody);
            landerscan3.SetScience(4, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber,false), null);
            this.AddParameter(new GetCrewCount(0), null);
            //this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(15f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(35f, 11f, targetBody);
            base.SetFunds(15000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 110000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, 110000 * HighLogic.CurrentGame.Parameters.CustomParams<MCE_IntergratedSettings3>().MCEContractPayoutMult, targetBody);

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
            return Localizer.Format("#autoLOC_MissionController2_1000168") + " " + this.MissionSeed.ToString();		// #autoLOC_MissionController2_1000168 = LanderResearch
        }
        protected override string GetTitle()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000169") + " " +  targetBody.bodyName;		// #autoLOC_MissionController2_1000169 = Land And Conduct Mass Spectrometry Analysis Of 
        }
        protected override string GetDescription()
        {

            return Localizer.Format("#autoLOC_MissionController2_1000170") +		// #autoLOC_MissionController2_1000170 = Mass spectrometry (MS) is an analytical chemistry technique that helps identify the amount and type of chemicals present in a sample by measuring the mass-to-charge ratio and abundance of gas-phase ions.\n\n A mass spectrum (plural spectra) is a plot of the ion signal as a function of the mass-to-charge ratio. The spectra are used to determine the elemental or isotopic signature of a sample, the masses of particles and of molecules, and to elucidate the chemical structures of molecules, such as peptides and other chemical compounds. Mass spectrometry works by ionizing chemical compounds to generate charged molecules or molecule fragments and measuring their mass-to-charge ratios.\n\n

                Localizer.Format("#autoLOC_MissionController2_1000171");		// #autoLOC_MissionController2_1000171 = In a typical MS procedure, a sample, which may be solid, liquid, or gas, is ionized, for example by bombarding it with electrons. This may cause some of the sample's molecules to break into charged fragments. These ions are then separated according to their mass-to-charge ratio, typically by accelerating them and subjecting them to an electric or magnetic field: ions of the same mass-to-charge ratio will undergo the same amount of deflection.[1] The ions are detected by a mechanism capable of detecting charged particles, such as an electron multiplier. Results are displayed as spectra of the relative abundance of detected ions as a function of the mass-to-charge ratio. The atoms or molecules in the sample can be identified by correlating known masses to the identified masses or through a characteristic fragmentation pattern.
        }
        protected override string GetNotes()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000172");		// #autoLOC_MissionController2_1000172 = Vessel must be a new vessel launched after accepting this contract!
        }
        protected override string GetSynopsys()
        {
            return Localizer.Format("#autoLOC_MissionController2_1000173") + " " + targetBody.bodyName + " and conduct Mass Spectrometry Analysis for our company.";		// #autoLOC_MissionController2_1000173 = Land an unmanned vessel on 
        }
        protected override string MessageCompleted()
        {
            MCELanderResearch.doLanderResearch = false;
            return Localizer.Format("#autoLOC_MissionController2_1000174") + " " + targetBody.bodyName + " " + Localizer.Format("#autoLOC_MissionController2_1000175") +		// #autoLOC_MissionController2_1000174 = You have successfully landed on and conducted research on 		// #autoLOC_MissionController2_1000175 = .  After landing we have discovered many fascinating secrets about what makes up the composition of the landing site.\n\n
            Localizer.Format("#autoLOC_MissionController2_1000176") + " " + targetBody.bodyName + ".";		// #autoLOC_MissionController2_1000176 = Further research missions, both manned and robotic, will be needed in the future to unlock the secrets of 
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
            Tools.ContractLoadCheck(node, ref partName, "Mass Spectrometry Tube", partName, "partname");
            Tools.ContractLoadCheck(node, ref partNumber, 1, partNumber, "maxcount");
            Tools.ContractLoadCheck(node, ref amountTime, 10000, amountTime, "amountTime");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
            node.AddValue("crewcount", crewCount);
            node.AddValue("partname", partName);
            node.AddValue("maxcount", partNumber);
            node.AddValue("amountTime", amountTime);
        }

        public override bool MeetRequirements()
        {           
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("landing") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(true, false);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
                else { return null; }
            }
            else { return null; }
        }
    }
    #endregion
   
    public class TechList
    {
        public string techName = "";

        public TechList(string name)
        {
            this.techName = name;
        }
    }
}
