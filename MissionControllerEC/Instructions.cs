using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;
using Contracts;
using Contracts.Parameters;


namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        public float resourceCost;
        public float vesselPartCost;
        public float vesseltons;
        public bool showFuel = false;
        public bool showTons = false;
        public bool showMiniTons = false;
        public float vesselResourceTons;
        public int currentContractType = 0;
        public string sPResource = "SpareParts";       

        public static int supplyCount;

        public static List<SupplyVesselList> SupVes = new List<SupplyVesselList>();

        public static List<string> CivName = new List<string>();

        Tools.MC2RandomWieghtSystem.Item<int>[] RandomContractsCheck;
        Tools.MC2RandomWieghtSystem.Item<int>[] RandomSatelliteContractsCheck;
        
        public void loadTextures()
        {
            if (texture == null)
            {
                texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCEStockToolbar.png")));
            }
            if (texture2 == null)
            {
                texture2 = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture2.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCERevert.png")));
            }
            //Debug.Log("MCE Textures Loaded");
        }

        public void loadFiles()
        {           
            if (settings.FileExists) { settings.Load(); settings.Save(); }
            else { settings.Save(); settings.Load(); }
            //Debug.Log("MCE Settings Loaded");
        }       

        public void CreateButtons()
        {
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER && this.MCEButton == null)
            {
                this.MCEButton = ApplicationLauncher.Instance.AddModApplication(
                    this.MCEOn,
                    this.MCEOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.SPACECENTER,
                    texture
                    );
                //Debug.Log("Creating MCEButton Buttons");
            }
            
            if (HighLogic.LoadedScene == GameScenes.FLIGHT && this.MCERevert == null && settings.RevertOn)
            {
                this.MCERevert = ApplicationLauncher.Instance.AddModApplication(
                    this.revertOn,
                    this.revertOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    texture2
                    );
                //Debug.Log("creating MCERevert Buttons");
            }
        }

        private void MCEOn()
        {           
            MCE_ScenarioStartup.ShowfinanaceWindow = true;
        }

        private void MCEOff()
        {           
            MCE_ScenarioStartup.ShowfinanaceWindow = false;
        }
       
        private void revertOff()
        {
            MCE_ScenarioStartup.ShowPopUpWindow3 = false;
        }
        private void revertOn()
        {
            MCE_ScenarioStartup.ShowPopUpWindow3 = true;
        }
        public void DestroyButtons()
        {
            if (this.MCEButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.MCEButton);
            }           
            if (this.MCERevert != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.MCERevert);
            }
        }

        public static void civNamesListAdd()
        {
            CivName.Add("Civilian Jason");
            CivName.Add("Civilian Carolyne");
            CivName.Add("Civilian Delila");
            CivName.Add("Civilian Aleta");
            CivName.Add("Civilian Jordon");
            CivName.Add("Civilian Leon");
            CivName.Add("Civilian Tonie");
            CivName.Add("Civilian AWen");
            CivName.Add("Civilian Huey");
            CivName.Add("Civilian Wilfred");
            CivName.Add("Civilian Jackson");
            CivName.Add("Civilian Alex");
            CivName.Add("Civilian Danny");
            CivName.Add("Civilian Malkuth");
            CivName.Add("Civilian Jebidiah");
            CivName.Add("Civilian Jeni");
            CivName.Add("Civilian Senyor");
            CivName.Add("Civilian Batman");
            CivName.Add("Civilian Timmy");
            CivName.Add("Civilian Myers");
            CivName.Add("Civilian Yoda");
            CivName.Add("Civilian Luke");
            CivName.Add("Civilian Jenny");
            CivName.Add("Civilian Sam");
        }

        public void FlightglobalsIndexCheck()
        {
            foreach (CelestialBody cb in FlightGlobals.Bodies)
            {
                Debug.Log("Planet name" + cb.theName + " Planet index " + cb.flightGlobalsIndex.ToString());
            }
        }       

        public void CheckRandomContractTypes(GameScenes gs)
        {
            randomContractsCheck();
            currentContractType = Tools.MC2RandomWieghtSystem.PickOne<int>(RandomContractsCheck);
            if (currentContractType == 0)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("No random contracts at this time");
            }            
            else if (currentContractType == 1)
            {
                SaveInfo.RepairContractOn = true;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Repair Contract Selected");
            }            
            else if (currentContractType == 2)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = true;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Civilian Low Orbit Contract Selected");
            }           
            else if (currentContractType == 3)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = true;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Civilian Landing Contract Selected");
            }
            else if (currentContractType == 4)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = true;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Civilian Station Expedition Contract Selected");
            }
            else if (currentContractType == 5)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = true;
                Debug.Log("Repair Station Contract Selected");
            }
            else
            {
                Debug.LogWarning("MCE failed to load random contract Check, defaulting");
            }
        }

        public void CheckRandomSatelliteContractTypes()
        {
            randomSatelliteContractsCheck();
            SaveInfo.SatelliteTypeChoice = Tools.MC2RandomWieghtSystem.PickOne<int>(RandomSatelliteContractsCheck); 
            Debug.LogWarning("Satellite Type Chosen Is Number " + SaveInfo.SatelliteTypeChoice);         
        }

        public void randomContractsCheck()
        {
            RandomContractsCheck = new Tools.MC2RandomWieghtSystem.Item<int>[6];
            RandomContractsCheck[0] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[0].weight = 0;
            RandomContractsCheck[0].value = 0;

            RandomContractsCheck[1] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[1].weight = settings.contract_repair_Random_percent;
            RandomContractsCheck[1].value = 1;

            RandomContractsCheck[2] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[2].weight = settings.contract_civilian_Low_Orbit_Percent;
            RandomContractsCheck[2].value = 2;

            RandomContractsCheck[3] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[3].weight = settings.contract_civilian_Landing_Percent;
            RandomContractsCheck[3].value = 3;

            RandomContractsCheck[4] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[4].weight = settings.contract_Civilian_Station_Expedition;
            RandomContractsCheck[4].value = 4;

            RandomContractsCheck[5] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[5].weight = settings.contract_repair_Station_Random_percent;
            RandomContractsCheck[5].value = 5;
        }

        public void randomSatelliteContractsCheck()
        {
            RandomSatelliteContractsCheck = new Tools.MC2RandomWieghtSystem.Item<int>[4];
            RandomSatelliteContractsCheck[0] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[0].weight = 25;
            RandomSatelliteContractsCheck[0].value = 0;

            RandomSatelliteContractsCheck[1] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[1].weight = 25;
            RandomSatelliteContractsCheck[1].value = 1;

            RandomSatelliteContractsCheck[2] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[2].weight = 25;
            RandomSatelliteContractsCheck[2].value = 2;

            RandomSatelliteContractsCheck[3] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[3].weight = 25;
            RandomSatelliteContractsCheck[3].value = 3;           
        }           

        public void onContractLoaded()
        {           
            if (settings.No_Rescue_Kerbal_Contracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.RescueKerbal)))          
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.RescueKerbal));
                    Debug.Log("Removed RescueKerbal Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoRescueKerbalContracts Returned Null");}
            }
            if (settings.No_Part_Test_Contracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.PartTest)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.PartTest));
                    Debug.Log("Removed PartTest Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoPartTest Returned Null"); }
            } 
        }    
                      
        public void chargeKerbalDeath(EventReport value)
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                Funding.Instance.AddFunds(-settings.Death_Insurance, TransactionReasons.Any);
                SaveInfo.TotalSpentKerbalDeaths += settings.Death_Insurance;
                StringBuilder deathmessage = new StringBuilder();
                deathmessage.AppendLine("A Kerbal named " + value.sender + " has died in the line of duty");
                deathmessage.AppendLine();
                deathmessage.AppendLine("This is a tragic loss and will cost you " + settings.Death_Insurance + " Funds.");
                deathmessage.AppendLine();
                deathmessage.AppendLine(value.sender + " will be remembered by the Kerbal People as a hero who though of Kerbal kind before his own safety");
                deathmessage.AppendLine();
                deathmessage.AppendLine("We send him to the Darkness in which we all are born, to rejoin the spark of life");
                MessageSystem.Message m = new MessageSystem.Message("Death Of Hero", deathmessage.ToString(), MessageSystemButton.MessageButtonColor.RED, MessageSystemButton.ButtonIcons.ALERT);
                MessageSystem.Instance.AddMessage(m);
                //Debug.Log("Death Event " + value.msg);
            }
        }

        public void getSupplyList(bool stationOnly)
        {
            bool boolStation = stationOnly;
            supplyCount = 0;
            SupVes.Clear();

            if (!stationOnly)
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.vesselType == VesselType.Station || v.vesselType == VesselType.Base)
                    {
                        SupVes.Add(new SupplyVesselList(v.name.Replace("(unloaded)", ""), v.id.ToString(), v.mainBody));
                        supplyCount++;
                        //Debug.Log("Found Vessel " + v.name + " " + v.vesselType + " Count is: " + supplyCount);
                    }
                }
            }
            else
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.vesselType == VesselType.Station)
                    {
                        SupVes.Add(new SupplyVesselList(v.name.Replace("(unloaded)", ""), v.id.ToString(), v.mainBody));
                        supplyCount++;
                        //Debug.Log("Found Vessel " + v.name + " " + v.vesselType + " Count is: " + supplyCount);
                    }
                }
            }
        }
        
        public void GetPlanetList()
        {
            foreach (var body in FlightGlobals.Bodies)
            {
                Debug.Log("Body Name: " + body.theName + " Body Number: " + body.flightGlobalsIndex);
            }
        }

        public void GetLatandLonDefault(Vessel vessel)
        {
            double LatValue;
            LatValue = vessel.latitude;
            double LonValue;
            LonValue = vessel.longitude;

            SaveInfo.apolloLandingLat = LatValue;

            SaveInfo.apolloLandingLon = LonValue;
        }
        public void GetEvaTypeKerbal()
        {
            List<ProtoCrewMember> protoCrewMembers = FlightGlobals.ActiveVessel.GetVesselCrew();
            foreach (Experience.ExperienceEffect exp in protoCrewMembers[0].experienceTrait.Effects)
            {
                if (exp.ToString() == "Experience.Effects.RepairSkill")
                {
                    Debug.Log("Current kerbal is a Engineer you have passed");
                }
                else
                {
                    Debug.Log("Current kerbal is NOT an Engineer you don't pass... Bad boy!");
                }
            }
        }
        public void SetVesselLaunchCurrentTime()
        {
            Debug.Log("Old LaunchTime was: " + Tools.ConvertDays(FlightGlobals.ActiveVessel.launchTime));
            double currentTime = Planetarium.GetUniversalTime();
            FlightGlobals.ActiveVessel.launchTime = currentTime;
            Debug.Log("New LaunchTime Is: " + Tools.ConvertDays(FlightGlobals.ActiveVessel.launchTime));
        }
    }
   
    public class RepairVesselsList
    {
        public string vesselName;
        public string vesselId;
        public int bodyidx;
        public double MaxApA;
        public RepairVesselsList()
        {
        }
        public RepairVesselsList(string name, string id,double maxAP,int Vbody)
        {
            this.vesselName = name;
            this.vesselId = id;
            this.MaxApA = maxAP;
            this.bodyidx = Vbody;
        }


    }

    public class SupplyVesselList
    {
        public string vesselName;
        public string vesselId;
        public CelestialBody body;
        public SupplyVesselList()
        {
        }
        public SupplyVesselList(string name, string vId, CelestialBody bod)
        {
            this.vesselName = name;
            this.vesselId = vId;
            this.body = bod;
        }
    }
}
