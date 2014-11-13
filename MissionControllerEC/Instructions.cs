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
            if ((HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.SPH) && this.EDMCEButton == null)
            {
                this.EDMCEButton = ApplicationLauncher.Instance.AddModApplication(
                    this.EDMCEOn,
                    this.EDMCEOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.VAB,
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

        private void EDMCEOn()
        {
            MCE_ScenarioStartup.ShowEditorWindow = true;
        }

        private void EDMCEOff()
        {
            MCE_ScenarioStartup.ShowEditorWindow = false;
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
            if (this.EDMCEButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.EDMCEButton);
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
            if (currentContractType == 1)
            {
                SaveInfo.RepairContractOn = true;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Repair Contract Selected");
            }            
            if (currentContractType == 2)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = true;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Civilian Low Orbit Contract Selected");
            }           
            if (currentContractType == 3)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = true;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Civilian Landing Contract Selected");
            }
            if (currentContractType == 4)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = true;
                SaveInfo.RepairStationContract = false;
                Debug.Log("Civilian Station Expedition Contract Selected");
            }
            if (currentContractType == 5)
            {
                SaveInfo.RepairContractOn = false;
                SaveInfo.CivilianLowOrbit = false;
                SaveInfo.CivilianLanding = false;
                SaveInfo.CivilianStationExpedition = false;
                SaveInfo.RepairStationContract = true;
                Debug.Log("Repair Station Contract Selected");
            }    
        }

        public void randomContractsCheck()
        {
            RandomContractsCheck = new Tools.MC2RandomWieghtSystem.Item<int>[6];
            RandomContractsCheck[0] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomContractsCheck[0].weight = 10;
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

        public void GetPartsCost()
        {            
            if (HighLogic.LoadedSceneIsEditor)
            {
                try               
                {
                    List<Part> parts;
                    parts = EditorLogic.SortedShipList;
                    resourceCost = 0;
                    vesselPartCost = 0;
                    vesseltons = 0;
                    vesselResourceTons = 0;
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Part Name", MCE_ScenarioStartup.StyleBold,GUILayout.MinWidth(300),GUILayout.MaxWidth(300));
                    GUILayout.Box("Part Cost", MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(8);

                    foreach (Part p in parts)
                    {                       
                        string name = p.partInfo.title;                      
                        vesselPartCost += p.partInfo.cost + p.GetModuleCosts();
                        vesseltons += p.mass;                                              
                    
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("" + name, MCE_ScenarioStartup.styleBoxWhite, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                        GUILayout.Box("" + (p.partInfo.cost + p.GetModuleCosts()), MCE_ScenarioStartup.styleBoxWhite, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                        GUILayout.EndHorizontal();

                        if (showTons && showMiniTons)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Part Mass Empty: " + p.mass.ToString("F2"), GUILayout.MinWidth(400));
                            GUILayout.EndHorizontal();
                        }

                        foreach (PartResource pr in p.Resources)
                        {
                            if (pr.amount > 0 && pr.info.unitCost > 0)
                            {
                                resourceCost += pr.info.unitCost  * (float)pr.amount;
                                vesselResourceTons += pr.info.density * (float)pr.amount;
                                double totalFuelCost = pr.info.unitCost * pr.amount;
                                double adjustedCost = pr.info.unitCost;
                                if (showFuel)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(pr.resourceName + " Amount: " + "(" + pr.amount.ToString("F2") + ")" + " Cost Per Unit: " + "(" + adjustedCost.ToString("F2") + ")" + " Total Cost: " + "(" + totalFuelCost.ToString("F2") + ")", MCE_ScenarioStartup.styleBlue, GUILayout.MinWidth(400));
                                    GUILayout.EndHorizontal();
                                }
                                if (showTons && showMiniTons)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Total Resource (" + pr.resourceName + ") Mass(Tons): " + vesselResourceTons.ToString("F2"), GUILayout.MinWidth(400));
                                    GUILayout.EndHorizontal();
                                }
                            }
                        }                        
                    }
                    float MinTotal = vesselPartCost - resourceCost;
                    float rMinTotal = vesseltons + vesselResourceTons;
                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Total Part Cost Without Fuel", MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                    GUILayout.Box("" + MinTotal.ToString("F2"), MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Total Fuel Cost", MCE_ScenarioStartup.styleBlueBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                    GUILayout.Box("" + resourceCost.ToString("F2"), MCE_ScenarioStartup.styleBlueBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Total Cost with Fuel", MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                    GUILayout.Box("" + vesselPartCost.ToString("F2"), MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();

                    if (showTons)
                    {
                        GUILayout.Space(15);
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Total Vessel Mass (Tons) ", MCE_ScenarioStartup.styleGreenBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                        GUILayout.Box("" + rMinTotal.ToString("F2"), MCE_ScenarioStartup.styleGreenBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                        GUILayout.EndHorizontal();
                    }                   
                }
                catch { };
                 
            }
            else
                Debug.Log("Not editor can't list parts");
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
               
        public void hireKerbals(ProtoCrewMember pc, ProtoCrewMember.KerbalType pc1, ProtoCrewMember.KerbalType pc2)
        {
            if (pc1 == ProtoCrewMember.KerbalType.Applicant && pc2 == ProtoCrewMember.KerbalType.Crew)
            {
                Debug.LogWarning("Kerbal Was Hired name is " + pc.name);
                StringBuilder Hiremessage = new StringBuilder();
                Hiremessage.AppendLine("You Just Hired " + pc.name);
                Hiremessage.AppendLine();
                Hiremessage.AppendLine("You hired him for Total of: " + settings.HireCost + " Funds.");
                Hiremessage.AppendLine();
                Hiremessage.AppendLine(pc.name + " Says he will serve your space Agency proud!");
                Hiremessage.AppendLine();              
                MessageSystem.Message m = new MessageSystem.Message("Hired Recruit", Hiremessage.ToString(), MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.MESSAGE);
                MessageSystem.Instance.AddMessage(m);
                Funding.Instance.AddFunds(- settings.HireCost,TransactionReasons.Any);                
            }
            
        }      

        public void chargeKerbalDeath(EventReport value)
        {
            Funding.Instance.AddFunds( - settings.Death_Insurance, TransactionReasons.Any);
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

        public void ActivateEVASpareParts()
        {
            if (!SaveInfo.spResourceSet)
            {
                try
                {
                    AvailablePart part = PartLoader.LoadedPartsList.Find(p => p.name.Equals("kerbalEVA"));
                    Part sPPart = part.partPrefab;
                    EvaAddResource(sPPart, sPResource);
                    SaveInfo.spResourceSet = true;
                }
                catch (Exception ex)
                {
                    Debug.LogError("MCE Failed EVA SpareParts On Load! .\n" + ex.Message + "\n" + ex.StackTrace);
                }
            }
            
        }
        

        public void EvaAddResource(Part part, string name)
        {
            try
            {
                PartResource resource = part.gameObject.AddComponent<PartResource>();
                resource.SetInfo(PartResourceLibrary.Instance.resourceDefinitions[name]);
                resource.maxAmount = 1;
                resource.part = part;
                resource.amount = 0;
                part.Resources.list.Add(resource);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Object reference not set"))
                {
                    Debug.LogError("MCE Error adding SpareParts to the EVA: " + ex.Message + "\n" + ex.StackTrace);
                }
            }
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
