using Microsoft.Xna.Framework;
using TDGame.Internals.Common;
using TDGame.Internals.UI;
using TDGame.Internals.Common.GameUI;
using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using TDGame.Internals.Common.GameInput;

namespace TDGame.GameContent.UI
{
    public static class StageEditorMenu
    {
        public static UIElement MenuParent;

        public static int selectedType;

        private static Keybind SaveKey = new("Save Stage", Microsoft.Xna.Framework.Input.Keys.U);

        private static Keybind LoadKey = new("Load Stage", Microsoft.Xna.Framework.Input.Keys.I);

        public struct UIElements
        {
            public static UIPanel MenuTilesContainer;
            public static UITextButton PathBlock;
            public static UITextButton WallBlock;
            public static UIText SelectedBlockText;
            public static UIPanel SelectedBlockTextContainer;
            public static UITextButton SaveKeybindText;
            public static UITextButton LoadKeybindText;
        }

        internal static void Initialize() {
            MenuParent = new();
            UIElements.MenuTilesContainer = new()
            {
                InteractionBoxRelative = new(0.75f, 0.2f, 0.2f, 0.6f),
                BackgroundColor = Color.White
            };
            UIElements.PathBlock = new("", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.77f, 0.25f, 0.06f, 0.1f),
                BackgroundColor = Color.Gray
            };
            UIElements.WallBlock = new("", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.87f, 0.25f, 0.06f, 0.1f),
                BackgroundColor = Color.LightGray
            };
            UIElements.SelectedBlockTextContainer = new()
            {
                InteractionBoxRelative = new(0.77f, 0.7f, 0.16f, 0.07f),
                BackgroundColor = Color.Black
            };
            UIElements.SelectedBlockText = new("", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.79f, 0.71f, 0.16f, 0.07f),
                Visible = false
            };
            UIElements.SaveKeybindText = new("U - Save Stage", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.79f, 0.92f, 0.18f, 0.07f),
                BackgroundColor = Color.Black
            };
            UIElements.LoadKeybindText = new("I - Load Stage", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.6f, 0.92f, 0.18f, 0.07f),
                BackgroundColor = Color.Black
            };
            UIElements.PathBlock.OnClick += BlockOnClick;
            UIElements.PathBlock.OnMouseOver += BlockOnOver;
            UIElements.PathBlock.OnMouseLeave += BlockOnLeave;
            UIElements.WallBlock.OnClick += BlockOnClick;
            UIElements.WallBlock.OnMouseOver += BlockOnOver;
            UIElements.WallBlock.OnMouseLeave += BlockOnLeave;
            Tile.OnClick += Tile_OnClick;
            UIElements.SaveKeybindText.OnClick += SaveKeybindText_OnClick;
            UIElements.LoadKeybindText.OnClick += LoadKeybindText_OnClick;
            List<UIElement> toAppend = new() { UIElements.MenuTilesContainer, UIElements.PathBlock, UIElements.WallBlock, UIElements.SelectedBlockTextContainer, UIElements.SaveKeybindText, UIElements.LoadKeybindText, UIElements.SelectedBlockText };
            foreach (var element in toAppend) {
                MenuParent.AppendElement(element);
            }
            MenuParent.Visible = true; //trolling
            SaveKey.KeybindPressAction = s => SaveKeybindText_OnClick(UIElements.SaveKeybindText);
            LoadKey.KeybindPressAction = l => LoadKeybindText_OnClick(UIElements.LoadKeybindText);
        }

        private static void LoadKeybindText_OnClick(UIElement affectedElement) {
            if (!MenuParent.Visible)
                return;
            using OpenFileDialog fileDialog = new();

            fileDialog.InitialDirectory = Path.Combine(TowerDefense.ExePath, "Stages");
            fileDialog.Filter = "stage files (*.stg)|*.stg";
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK) {
                Stage test = Stage.LoadStage(fileDialog.SafeFileName.Remove(fileDialog.SafeFileName.Length - 4));

                Stage.SetStage(test);
            }
        }

        private static void SaveKeybindText_OnClick(UIElement affectedElement) {
            if (!MenuParent.Visible)
                return;
            using SaveFileDialog fileDialog = new();

            fileDialog.InitialDirectory = Path.Combine(TowerDefense.ExePath, "Stages");
            fileDialog.FileName = "MyStage";
            fileDialog.Filter = "stage files (*.stg)|*.stg";
            fileDialog.OverwritePrompt = true;

            if (fileDialog.ShowDialog() == DialogResult.OK) {
                Stage.currentLoadedStage = new(fileDialog.FileName.Remove(fileDialog.FileName.Length - 4));
                Stage.currentLoadedStage.TileMap.Clear();
                foreach (var tl in Tile.Tiles) {
                    Stage.currentLoadedStage.TileMap.Add(tl);
                }
                Stage.SaveStage(Stage.currentLoadedStage);
            }
        }

        internal static void Update() {

        }

        private static void BlockOnLeave(UIElement affectedElement) {
            if (selectedType <= 0) {
                UIElements.SelectedBlockText.Visible = false;
            }
        }

        private static void BlockOnOver(UIElement affectedElement) {
            UIElements.SelectedBlockText.Visible = true;
            string clickedTool = string.Empty;
            if (affectedElement == UIElements.PathBlock) {
                clickedTool = "Path Block";
            }
            else if (affectedElement == UIElements.WallBlock) {
                clickedTool = "Wall Block";
            }
            UIElements.SelectedBlockText.Text = clickedTool;
        }

        private static void Tile_OnClick(Tile tile) {
            if (selectedType > 0) {
                tile.type = selectedType;
                tile.RedrawTile();
            }
        }

        private static void BlockOnClick(UIElement affectedElement) {
            int GetSelectedType() {
                if (affectedElement == UIElements.PathBlock) {
                    return 1;
                }
                else if (affectedElement == UIElements.WallBlock) {
                    return 2;
                }
                return 0;
            }
            if (GetSelectedType() == selectedType) {
                selectedType = 0;
            }
            else {
                selectedType = GetSelectedType();
            }
        }
    }
}