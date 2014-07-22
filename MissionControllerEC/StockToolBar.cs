#region Using Directives

using System.IO;
using System.Reflection;

using UnityEngine;

#endregion

namespace MissionControllerEC
{
    [KSPAddonFixed(KSPAddon.Startup.SpaceCentre, true, typeof(StockToolbar))]
    public class StockToolbar : MonoBehaviour
    {
        private static Texture2D texture;
        private ApplicationLauncherButton MCEButton;

        void Awake()
        {
            if (texture == null)
            {
                texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCEStockToolbar.png")));
            }
            this.MCEButton.SetTrue();

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
                    ApplicationLauncher.AppScenes.ALWAYS,
                    texture
                    );
            }
        }

        private void MCEOn()
        {
            MissionControllerEC.ShowMainWindow = true;
        }

        private void MCEOff()
        {
            MissionControllerEC.ShowMainWindow = false;
        }

        private void OnDestroy()
        {
            if (this.MCEButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.MCEButton);
            }
        }
    }
}
