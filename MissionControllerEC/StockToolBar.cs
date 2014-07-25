#region Using Directives

using System.IO;
using System.Reflection;

using UnityEngine;

#endregion

namespace MissionControllerEC
{
    [KSPAddonFixed(KSPAddon.Startup.EveryScene, true, typeof(StockToolbar))]
    public class StockToolbar : MonoBehaviour
    {
        private static Texture2D texture;
        private static Texture2D texture2;
        private ApplicationLauncherButton MCEButton;
        private ApplicationLauncherButton MCERevert;

        void Awake()
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
            this.MCEButton.SetTrue();
            this.MCERevert.SetTrue();

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
            }
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER && this.MCERevert == null)
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
            }
        }

        private void MCEOn()
        {
            MissionControllerEC.ShowfinanaceWindow = true;
        }

        private void MCEOff()
        {
            MissionControllerEC.ShowfinanaceWindow = false;
        }

        private void revertOff()
        {
            MissionControllerEC.ShowPopUpWindow3 = false;
        }
        private void revertOn()
        {
            MissionControllerEC.ShowPopUpWindow3 = true;
        }

        private void OnDestroy()
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
    }
}
