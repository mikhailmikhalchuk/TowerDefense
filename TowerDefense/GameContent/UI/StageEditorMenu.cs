using Microsoft.Xna.Framework;
using TDGame.Internals.Common;
using TDGame.Internals.UI;
using TDGame.Internals.Common.GameUI;
using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using TDGame.Internals.Common.GameInput;
using TDGame.GameContent;

namespace TDGame.GameContent.UI
{
    public static class StageEditorMenu
    {
        public static UIElement MenuParent;

        public static int selectedType;

        private static Keybind SaveKey = new("Save Stage", Microsoft.Xna.Framework.Input.Keys.U);

        private static Keybind LoadKey = new("Load Stage", Microsoft.Xna.Framework.Input.Keys.I);

        private static Keybind HideKey = new("Hide Menus", Microsoft.Xna.Framework.Input.Keys.H);

        private static int removeTime;

        public struct UIElements
        {
            public static UIPanel MenuTilesContainer;
            public static UITextButton PathBlock;
            public static UITextButton WallBlock;
            public static UITextButton ExitBlock;
            public static UIText SelectedBlockText;
            public static UIPanel SelectedBlockTextContainer;
            public static UITextButton SaveKeybindText;
            public static UITextButton LoadKeybindText;
            public static UITextButton HideUIText;
            public static UIText StageLoaderStatus;
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
            UIElements.ExitBlock = new("", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.77f, 0.3f, 0.06f, 0.1f),
                BackgroundColor = Color.Brown
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
            UIElements.HideUIText = new("H - Hide Menus", TowerDefense.Fonts.DefaultFont, Color.White)
            {
                InteractionBoxRelative = new(0.8f, 0.03f, 0.18f, 0.07f),
                BackgroundColor = Color.Black
            };
            UIElements.StageLoaderStatus = new("", TowerDefense.Fonts.DefaultFont, Color.Red)
            {
                InteractionBoxRelative = new(0.03f, 0.03f, 0.1f, 0.1f)
            };
            UIElements.PathBlock.OnClick += BlockOnClick;
            UIElements.PathBlock.OnMouseOver += BlockOnOver;
            UIElements.PathBlock.OnMouseLeave += BlockOnLeave;
            UIElements.WallBlock.OnClick += BlockOnClick;
            UIElements.WallBlock.OnMouseOver += BlockOnOver;
            UIElements.WallBlock.OnMouseLeave += BlockOnLeave;
            UIElements.ExitBlock.OnClick += BlockOnClick;
            UIElements.ExitBlock.OnMouseOver += BlockOnOver;
            UIElements.ExitBlock.OnMouseLeave += BlockOnLeave;
            Tile.OnClick += Tile_OnClick;
            Tile.OnRightClick += Tile_OnRightClick;
            UIElements.SaveKeybindText.OnClick += SaveKeybindText_OnClick;
            UIElements.LoadKeybindText.OnClick += LoadKeybindText_OnClick;
            UIElements.HideUIText.OnClick += HideUIText_OnClick;
            List<UIElement> toAppend = new() { UIElements.MenuTilesContainer, UIElements.PathBlock, UIElements.WallBlock, UIElements.ExitBlock, UIElements.SelectedBlockTextContainer, UIElements.SaveKeybindText, UIElements.LoadKeybindText, UIElements.HideUIText, UIElements.SelectedBlockText };
            foreach (var element in toAppend) {
                MenuParent.AppendElement(element);
            }
            SaveKey.KeybindPressAction = s => SaveKeybindText_OnClick(UIElements.SaveKeybindText);
            LoadKey.KeybindPressAction = l => LoadKeybindText_OnClick(UIElements.LoadKeybindText);
            HideKey.KeybindPressAction = h => HideUIText_OnClick(UIElements.LoadKeybindText);
        }

        private static void Tile_OnRightClick(Tile tile) {
            new Enemy("Test", new Vector2(tile.WorldX, tile.WorldY), 3f);
        }

        private static void HideUIText_OnClick(UIElement affectedElement) {
            if (affectedElement != UIElements.LoadKeybindText && !MenuParent.Visible)
                return;
            MenuParent.Visible = !MenuParent.Visible;
            if (MenuParent.Visible)
                return;

            UIElements.StageLoaderStatus.Text = $"Press \"H\" to show menus";
            UIElements.StageLoaderStatus.Color = Color.LightGreen;
            removeTime = 150;
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

                if (Stage.ProhibitedNames.Contains(test.Name)) {
                    UIElements.StageLoaderStatus.Text = $"Failed to load {fileDialog.SafeFileName} (check logs)";
                    UIElements.StageLoaderStatus.Color = Color.Red;
                    removeTime = 150;
                    return;
                }

                Stage.SetStage(test);

                UIElements.StageLoaderStatus.Text = $"Loaded {fileDialog.SafeFileName}";
                UIElements.StageLoaderStatus.Color = Color.LightGreen;
                removeTime = 150;
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

                if (Stage.ProhibitedNames.Contains(fileDialog.FileName.Remove(fileDialog.FileName.Length - 4))) {
                    UIElements.StageLoaderStatus.Text = $"Cannot save with reserved name \"{fileDialog.FileName.Remove(fileDialog.FileName.Length - 4)}\"";
                    UIElements.StageLoaderStatus.Color = Color.LightGreen;
                    removeTime = 150;
                    return;
                }

                Stage.currentLoadedStage = new(fileDialog.FileName.Remove(fileDialog.FileName.Length - 4));
                Stage.currentLoadedStage.TileMap.Clear();
                foreach (var tl in Tile.Tiles) {
                    Stage.currentLoadedStage.TileMap.Add(tl);
                }
                Stage.SaveStage(Stage.currentLoadedStage);

                UIElements.StageLoaderStatus.Text = $"Saved {fileDialog.FileName.Remove(0, TowerDefense.ExePath.Length + 8)}";
                UIElements.StageLoaderStatus.Color = Color.LightGreen;
                removeTime = 150;
            }
        }

        internal static void Update() {
            if (removeTime > 0)
                removeTime--;

            if (removeTime <= 0 && UIElements.StageLoaderStatus.Text != "") {
                UIElements.StageLoaderStatus.Text = "";
            }
        }

        private static void BlockOnLeave(UIElement affectedElement) {
            if (selectedType == TileID.None) {
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
            else if (affectedElement == UIElements.ExitBlock) {
                clickedTool = "Exit Block";
            }
            UIElements.SelectedBlockText.Text = clickedTool;
        }

        private static void Tile_OnClick(Tile tile) {
            if (selectedType > TileID.None) {
                tile.type = selectedType;
                tile.RedrawTile();
            }
        }

        private static void BlockOnClick(UIElement affectedElement) {
            if (!MenuParent.Visible)
                return;
            int GetSelectedType() {
                if (affectedElement == UIElements.PathBlock) {
                    return TileID.Path;
                }
                else if (affectedElement == UIElements.WallBlock) {
                    return TileID.Wall;
                }
                else if (affectedElement == UIElements.ExitBlock) {
                    return TileID.Exit;
                }
                return TileID.None;
            }
            if (GetSelectedType() == selectedType) {
                selectedType = TileID.None;
            }
            else {
                selectedType = GetSelectedType();
            }
        }
    }
}