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
        public bool showMiniTons = true;
        public float vesselResourceTons;

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
                        float cst = p.partInfo.cost;
                        string name = p.partInfo.title;
                        vesselPartCost += cst;
                        vesseltons += p.mass;
                    

                        GUILayout.BeginHorizontal();
                        GUILayout.Box("" + name, MCE_ScenarioStartup.styleBoxWhite, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                        GUILayout.Box("" + cst, MCE_ScenarioStartup.styleBoxWhite, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                        GUILayout.EndHorizontal();

                        if (showTons && !showMiniTons)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Part Mass Empty: " + p.mass, GUILayout.MinWidth(400));
                            GUILayout.EndHorizontal();
                        }

                        foreach (PartResource pr in p.Resources)
                        {
                            if (pr.amount > 0 && pr.info.unitCost > 0)
                            {
                                resourceCost += pr.info.unitCost * (float)pr.amount;
                                vesselResourceTons += pr.info.density * (float)pr.amount;
                                if (showFuel)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(pr.resourceName + " Amount: " + pr.amount + " Cost Unit " + pr.info.unitCost + " Total: " + (pr.info.unitCost + (float)pr.amount) + pr.amount,MCE_ScenarioStartup.styleBlue,GUILayout.MinWidth(400));
                                    GUILayout.EndHorizontal();
                                }
                                if (showTons && !showMiniTons)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Total Resource (" + pr.resourceName + ") Mass(Kg): " + vesselResourceTons, GUILayout.MinWidth(400));
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
                    GUILayout.Box("" + MinTotal, MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Total Fuel Cost", MCE_ScenarioStartup.styleBlueBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                    GUILayout.Box("" + resourceCost, MCE_ScenarioStartup.styleBlueBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Total Cost with Fuel", MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                    GUILayout.Box("" + vesselPartCost, MCE_ScenarioStartup.StyleBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();

                    if (showTons)
                    {
                        GUILayout.Space(15);
                        GUILayout.BeginHorizontal();
                        GUILayout.Box("Total Vessel Mass (Kg) ", MCE_ScenarioStartup.styleGreenBold, GUILayout.MinWidth(300), GUILayout.MaxWidth(300));
                        GUILayout.Box("" + rMinTotal, MCE_ScenarioStartup.styleGreenBold, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
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
            if (settings.NoRescueKerbalContracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.RescueKerbal)))          
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.RescueKerbal));
                    //Debug.Log("Removed RescueKerbal Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoRescueKerbalContracts Returned Null");}
            }           
        }    
               
        public void hireKerbals(ProtoCrewMember pc, ProtoCrewMember.KerbalType pc1, ProtoCrewMember.KerbalType pc2)
        {
            if (pc1 == ProtoCrewMember.KerbalType.Applicant && pc2 == ProtoCrewMember.KerbalType.Crew)
            {
                //Debug.LogWarning("Kerbal Was Hired name is " + pc.name);

                Funding.Instance.Funds -= settings.HireCost;                
            }
            
        }      

        public void chargeKerbalDeath(EventReport value)
        {
            Funding.Instance.Funds -= settings.DeathInsurance;
            StringBuilder deathmessage = new StringBuilder();
            deathmessage.AppendLine("A Kerbal named " + value.sender + " has died in the line of duty");
            deathmessage.AppendLine();
            deathmessage.AppendLine("This is a tragic loss and will cost you " + settings.DeathInsurance + " Funds.");
            deathmessage.AppendLine();
            deathmessage.AppendLine(value.sender + " Will be remember by the Kerbal People as a hero who though of Kerbal kind before his own safety");
            deathmessage.AppendLine();
            deathmessage.AppendLine("We send him to the Darkness in which we all are born, to rejoin the spark of life");
            MessageSystem.Message m = new MessageSystem.Message("Death Of Hero", deathmessage.ToString(), MessageSystemButton.MessageButtonColor.RED, MessageSystemButton.ButtonIcons.ALERT);
            MessageSystem.Instance.AddMessage(m);
            //Debug.Log("Death Event " + value.msg);
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
