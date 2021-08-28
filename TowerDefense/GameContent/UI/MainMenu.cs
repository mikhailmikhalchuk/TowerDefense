using Microsoft.Xna.Framework;
using TDGame.Internals.Common;
using TDGame.Internals.UI;
using TDGame.Internals.Common.GameUI;

namespace TDGame.GameContent.UI
{
    public static class MainMenu
    {
        public static UIElement MenuParent;

        public struct UIElements
        {
            public static UITextButton MenuButtonReturn;
            public static UIPanel Test;
        }

        internal static void Initialize() {
            /*MenuParent = new();
            UIElements.MenuButtonReturn = new("Return", TowerDefense.Fonts.DefaultFont, Color.Gray, 1.5f)
            {
                InteractionBoxRelative = new(0.35f, 0.25f, 0.3f, 0.1f),
                BackgroundColor = Color.White
            };
            UIElements.Test = new()
            {
                InteractionBox = new(200, 200, 100, 100),
                Rotation = 45f
            };
            MenuParent.AppendElement(UIElements.MenuButtonReturn);*/
        }
    }
}