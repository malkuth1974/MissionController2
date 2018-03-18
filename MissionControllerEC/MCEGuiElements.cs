using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using KSP.UI.TooltipTypes;
using KSP.Localization;

namespace MissionControllerEC
{
    public static class MCEGuiElements
    {
        //Most of this next section is based on Hebarusan and his Astrogator mod.  Its been modified for my needs but I thank him for helping me out with
        //GUI Stuff for these changes in MCE 2.0!
        public const int fontSize = 8;

        private static Texture2D SolidColorTexture(Color c)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            tex.SetPixel(1, 1, c);
            tex.Apply();
            return tex;
        }

        private static Sprite SpriteFromTexture(Texture2D tex)
        {
            if (tex != null)
            {
                return Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    tex.width
                );
            }
            else
            {
                return null;
            }
        }

        private static Sprite SolidColorSprite(Color c)
        {
            return SpriteFromTexture(SolidColorTexture(c));
        }

        public static readonly Sprite halfTransparentBlack = SolidColorSprite(new Color(0f, 0f, 0f, 0.5f));
        public static readonly Sprite halfTransparentRed = SolidColorSprite(new Color(1f, 0f, 0f, 0.5f));
        public static readonly Sprite halfTransparentBlue = SolidColorSprite(new Color(0f, 0f, 1f, 0.5f));


        public static readonly UIStyleState colorRed = new UIStyleState()
        {
            background = halfTransparentBlack,
            textColor = Color.HSVToRGB(1f, 0.8f, 0.8f)
        };

        public static readonly UIStyleState colorBlue = new UIStyleState()
        {
            background = halfTransparentBlue,
            textColor = Color.blue
        };
        public static readonly UIStyleState colorBlueLabel = new UIStyleState()
        {
            background = halfTransparentBlue,
            textColor = Color.white
        };

        public static readonly UIStyleState colorYellow = new UIStyleState()
        {
            background = halfTransparentRed,
            textColor = Color.yellow
        };

        public static readonly UIStyleState ColorWhite = new UIStyleState()
        {
            background = halfTransparentBlue,
            textColor = Color.white
        };
       

        public static readonly UIStyle DescripStyle = new UIStyle()
        {
            normal = colorRed,
            active = colorRed,
            disabled = colorRed,
            highlight = colorRed,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
            fontStyle = FontStyle.Normal,
        };

        public static readonly UIStyle DescripStyle2 = new UIStyle()
        {
            normal = colorBlueLabel,
            active = colorBlueLabel,
            disabled = colorBlueLabel,
            highlight = colorBlueLabel,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
            fontStyle = FontStyle.Normal,
        };

        public static readonly UIStyle ButtonMenuMainSyle = new UIStyle()
        {
            normal = ColorWhite,
            active = ColorWhite,
            disabled = ColorWhite,
            highlight = ColorWhite,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 24,
            fontStyle = FontStyle.Bold,
        };

        public static readonly UIStyle ButtonPressMeToWorkStyle = new UIStyle()
        {
            normal = colorYellow,
            active = colorYellow,
            disabled = colorYellow,
            highlight = colorYellow,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 28,
            fontStyle = FontStyle.Bold,
        };

        public static readonly UIStyle toggleStyle = new UIStyle()
        {
            normal = UISkinManager.defaultSkin.toggle.normal,
            active = UISkinManager.defaultSkin.toggle.active,
            disabled = UISkinManager.defaultSkin.toggle.disabled,
            highlight = UISkinManager.defaultSkin.toggle.highlight,
            alignment = TextAnchor.MiddleLeft,
            fontSize = UISkinManager.defaultSkin.toggle.fontSize,
            lineHeight = UISkinManager.defaultSkin.toggle.fontSize + 1,
            fontStyle = UISkinManager.defaultSkin.toggle.fontStyle,
            wordWrap = true,
            stretchHeight = true,
        };

        public static readonly UIStyle subTitleStyle = new UIStyle()
        {
            normal = ColorWhite,
            active = ColorWhite,
            disabled = ColorWhite,
            highlight = ColorWhite,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 18,
            fontStyle = FontStyle.BoldAndItalic,           
        };

        public static readonly UISkinDef MissionControllerSkin = new UISkinDef()
        {
            name = "MissionController Skin",
            window = DescripStyle,
            box = UISkinManager.defaultSkin.box,
            font = UISkinManager.defaultSkin.font,
            label = subTitleStyle,
            toggle = toggleStyle,
            button = UISkinManager.defaultSkin.button,
            textField = DescripStyle,
            textArea = DescripStyle,
        };

    }
}
