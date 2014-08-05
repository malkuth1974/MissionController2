using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using Contracts;
using Contracts.Parameters;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {

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
            Debug.Log("MCE Textures Loaded");
        }

        public void loadFiles()
        {           
            if (settings.FileExists) { settings.Load(); settings.Save(); }
            else { settings.Save(); settings.Load(); }
            Debug.Log("MCE Settings Loaded");
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
                Debug.Log("Creating MCEButton Buttons");
            }
            if (HighLogic.LoadedScene == GameScenes.FLIGHT && this.MCERevert == null)
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
                Debug.Log("creating MCERevert Buttons");
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


        public void onContractLoaded()
        {
            if (settings.NoRescueKerbalContracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.RescueKerbal)))
            {
                ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.RescueKerbal));
                Debug.Log("Removed RescueKerbal Type Contracts from Gererating");
            }
            Debug.Log("On Contract Loaded event fired at startup");
        }    
               
        public void hireKerbals(ProtoCrewMember pc, ProtoCrewMember.KerbalType pc1, ProtoCrewMember.KerbalType pc2)
        {
            if (pc1 == ProtoCrewMember.KerbalType.Applicant && pc2 == ProtoCrewMember.KerbalType.Crew)
            {
                Debug.LogWarning("Kerbal Was Hired name is " + pc.name);

                Funding.Instance.Funds -= settings.HireCost;

                StringBuilder HireMessage = new StringBuilder();
                HireMessage.AppendLine("You just hired " + pc.name + " and is now part of your crew roster");
                HireMessage.AppendLine();
                HireMessage.AppendLine("Total Cost Of New Recruit: " + settings.HireCost);
                HireMessage.AppendLine("Hired On Date: " + Tools.formatTime(Planetarium.GetUniversalTime()));
                SaveInfo.TotalSpentKerbals += settings.HireCost;
                MessageSystem.Message m = new MessageSystem.Message(
                "Hired New Kerbal",
                HireMessage.ToString(),
                MessageSystemButton.MessageButtonColor.YELLOW,
                MessageSystemButton.ButtonIcons.MESSAGE);
                MessageSystem.Instance.AddMessage(m);
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
            Debug.Log("Death Event " + value.msg);
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
