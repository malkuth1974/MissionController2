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

namespace MissionControllerEC.MCEContracts
{      
    #region OrbitalScan Contract
    public class OrbitalScanContract : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        int crewCount = 0;
        public double testpos = 0;
        string partName = "Ionization Chamber";
        int partNumber = 1;
        double missionTime = 0;
        public int totalContracts = 0;
        public int TotalFinished = 0;
        ContractParameter orbitresearch1;
        ContractParameter orbitresearch2;

        protected override bool Generate()
        {
            if (prestige != ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            targetBody = GetUnreachedTargets();
            if (targetBody != null)
            {
                return false;
            }
            else
            {
                targetBody = Planetarium.fetch.Home;
            }
            if (SaveInfo.NoOrbitalResearchContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<OrbitalScanContract>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<OrbitalScanContract>().Count();
            if (totalContracts >= st.Science_Contract_Per_Cycle)
            {                
               return false;
            }
            missionTime = Tools.RandomNumber(200, 1500);
            this.orbitresearch1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            orbitresearch1.SetFunds(8000, targetBody);
            orbitresearch1.SetReputation(3, targetBody);
            orbitresearch1.SetScience(2, targetBody);
            this.orbitresearch2 = this.AddParameter(new OrbialResearchPartCheck(targetBody, missionTime), null);
            orbitresearch2.SetFunds(9000, targetBody);
            orbitresearch2.SetReputation(5, targetBody);
            orbitresearch2.SetScience(2, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber,false), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(5f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(5f, 3f, targetBody);
            base.SetFunds(34000f * st.Contract_Payment_Multiplier, 53000f * st.Contract_Payment_Multiplier, 130000f * st.Contract_Payment_Multiplier, targetBody);

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
            return "Ionization Scan of  " + targetBody.theName;
        }
        protected override string GetDescription()
        {
            return "An ionization chamber measures the charge from the number of ion pairs created within a gas caused by incident radiation.[2] It consists of a gas-filled chamber with two electrodes; known as anode and "+
            "cathode. The electrodes may be in the form of parallel plates (Parallel Plate Ionization Chambers: PPIC), or a cylinder arrangement with a coaxially located internal anode wire.\n\n"+

            "A voltage potential is applied between the electrodes to create an electric field in the fill gas. When gas between the electrodes is ionized by incident ionizing radiation, ion-pairs are created and "+
            "the resultant positive ions and dissociated electrons move to the electrodes of the opposite polarity under the influence of the electric field. This generates an ionization current which is measured "+
            "by an electrometer circuit. The electrometer must be capable of measuring the very small output current which is in the region of femtoamperes to picoamperes, depending on the chamber design, radiation "+
            "dose and applied voltage.\n\n"+

            "Each ion pair created deposits or removes a small electric charge to or from an electrode, such that the accumulated charge is proportional to the number of ion pairs created, and hence the radiation dose. "+
                "This continual generation of charge produces an ionization current, which is a measure of the total ionizing dose entering the chamber. However, the chamber cannot discriminate between radiation types "+
                "(beta or gamma) and cannot produce an energy spectrum of radiation.\n\n"+

            "The electric field also enables the device to work continuously by mopping up electrons, which prevents the fill gas from becoming saturated, where no more ions could be collected, and by preventing the "+
            "recombination of ion pairs, which would diminish the ion current. This mode of operation is referred to as current mode, meaning that the output signal is a continuous current, and not a pulse output as "+
            "in the cases of the Geiger-Müller tube or the proportional counter.\n\n"+

            "Referring to the accompanying ion pair collection graph, it can be seen that in the ion chamber operating region the collection of ion pairs is effectively constant over a range of applied voltage, as due "+
            "to its relatively low electric field strength the ion chamber does not have any multiplication effect. This is in distinction to the Geiger-Müller tube or the proportional counter whereby secondary "+
            "electrons, and ultimately multiple avalanches, greatly amplify the original ion-current charge.";
        }
        protected override string GetNotes()
        {
            return "Vessel must be a new vessel launched after accepting this contract!";
        }
        protected override string GetSynopsys()
        {
            return "Orbit And Conduct Ionization Scan Of " + targetBody.theName;
        }
        protected override string MessageCompleted()
        {
            MCEOrbitalScanning.doOrbitResearch = false;
            return "You have reached the target body " + targetBody.theName + ", and conducted a Ionization Scan.  We have learned a lot of new information about the composition " +
                "of this planetary body in preparation for a possible landing in the future by our manned program or Robotic Legions.";
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
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("electrics") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }

        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(true, true);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
            }
            return null;
        }
    }
    #endregion
    #region Lander Scanning Contract
    public class LanderResearchScan : Contract
    {
        Settings st = new Settings("Config.cfg");
        CelestialBody targetBody = null;
        int crewCount = 0;
        public double testpos = 0;
        string partName = "Mass Spectrometry Tube";
        int partNumber = 1;
        double amountTime = Tools.RandomNumber(200, 1500);
        public int totalContracts;
        public int TotalFinished;
        ContractParameter landerscan1;
        ContractParameter landerscan2;
        ContractParameter landerscan3;

        protected override bool Generate()
        {
            if (prestige != ContractPrestige.Trivial)
            {
                return false;
            }
            if (HighLogic.LoadedSceneIsFlight) { return false; }
            targetBody = GetUnreachedTargets();

            if (targetBody != null)
            {
                return false;
            }
            else
            {
                return false;
            }
            if (SaveInfo.NoLanderResearchContracts)
            {
                return false;
            }
            totalContracts = ContractSystem.Instance.GetCurrentContracts<LanderResearchScan>().Count();
            TotalFinished = ContractSystem.Instance.GetCompletedContracts<LanderResearchScan>().Count();
            if (totalContracts >= st.Science_Contract_Per_Cycle)
            {               
                return false;
            }
            if (targetBody.flightGlobalsIndex == 8)
            {
                Debug.LogWarning("Landing Goal Body set to: " + targetBody.theName + " Contract Generate cancelled");
                return false;
            }
            this.landerscan1 = this.AddParameter(new InOrbitGoal(targetBody), null);
            landerscan1.SetFunds(8000, targetBody);
            landerscan1.SetReputation(4, targetBody);
            landerscan1.SetScience(2, targetBody);
            this.landerscan2 = this.AddParameter(new LandingParameters(targetBody, true), null);
            landerscan2.SetFunds(8500, targetBody);
            landerscan2.SetReputation(5, targetBody);
            landerscan2.SetScience(3, targetBody);
            this.landerscan3 = this.AddParameter(new LanderResearchPartCheck(targetBody, amountTime), null);
            landerscan3.SetFunds(10000, targetBody);
            landerscan3.SetReputation(8, targetBody);
            landerscan3.SetScience(4, targetBody);
            this.AddParameter(new PartGoal(partName, partNumber,false), null);
            this.AddParameter(new GetCrewCount(0), null);
            this.prestige = ContractPrestige.Significant;
            base.SetExpiry(3f, 10f);
            base.SetScience(15f, targetBody);
            base.SetDeadlineYears(3f, targetBody);
            base.SetReputation(35f, 11f, targetBody);
            base.SetFunds(37000f * st.Contract_Payment_Multiplier, 66000f * st.Contract_Payment_Multiplier, 150000f * st.Contract_Payment_Multiplier, targetBody);

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
            return "Land And Conduct Mass Spectrometry Analysis Of " + targetBody.theName;
        }
        protected override string GetDescription()
        {

            return "Mass spectrometry (MS) is an analytical chemistry technique that helps identify the amount and type of chemicals present in a sample by measuring the mass-to-charge ratio and abundance of gas-phase " +
                "ions.\n\n" +

                "A mass spectrum (plural spectra) is a plot of the ion signal as a function of the mass-to-charge ratio. The spectra are used to determine the elemental or isotopic signature of a sample, the masses " +
                "of particles and of molecules, and to elucidate the chemical structures of molecules, such as peptides and other chemical compounds. Mass spectrometry works by ionizing chemical compounds to generate " +
                "charged molecules or molecule fragments and measuring their mass-to-charge ratios.\n\n" +

                "In a typical MS procedure, a sample, which may be solid, liquid, or gas, is ionized, for example by bombarding it with electrons. This may cause some of the sample's molecules to break into charged " +
                "fragments. These ions are then separated according to their mass-to-charge ratio, typically by accelerating them and subjecting them to an electric or magnetic field: ions of the same mass-to-charge " +
                "ratio will undergo the same amount of deflection.[1] The ions are detected by a mechanism capable of detecting charged particles, such as an electron multiplier. Results are displayed as spectra of the " +
                "relative abundance of detected ions as a function of the mass-to-charge ratio. The atoms or molecules in the sample can be identified by correlating known masses to the identified masses or through a " +
                "characteristic fragmentation pattern.";
        }
        protected override string GetNotes()
        {
            return "Vessel must be a new vessel launched after accepting this contract!";
        }
        protected override string GetSynopsys()
        {
            return "Land an unmanned vessel on " + targetBody.theName + " and conduct Mass Spectrometry Analysis for our company.";
        }
        protected override string MessageCompleted()
        {
            MCELanderResearch.doLanderResearch = false;
            return "You have successfully landed on and conducted research on " + targetBody.theName + ".  After landing we have discovered many fascinating secrets about what makes up the composition of the landing site.\n\n" +

            "Further research missions, both manned and robotic, will be needed in the future to unlock the secrets of " + targetBody.theName + ".";
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
            bool techUnlock = ResearchAndDevelopment.GetTechnologyState("advElectrics") == RDTech.State.Available;
            if (techUnlock)
                return true;
            else
                return false;
        }
        protected static CelestialBody GetUnreachedTargets()
        {
            var bodies = Contract.GetBodies_Reached(false, false);
            if (bodies != null)
            {
                if (bodies.Count > 0)
                    return bodies[UnityEngine.Random.Range(0, bodies.Count)];
            }
            return null;
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
