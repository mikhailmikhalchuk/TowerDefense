using Microsoft.Xna.Framework;
using TDGame.Internals.Common;
using TDGame.Internals.UI;
using TDGame.Internals.Common.GameUI;
using System.Windows.Forms;
using System.IO;

namespace TDGame.GameContent.UI
{
    public static class StageEditorMenu
    {
        public static UIElement MenuParent;

        public static int selectedType;

        public struct UIElements
        {
            public static UIPanel MenuTilesContainer;
            public static UITextButton PathBlock;
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
            UIElements.PathBlock.OnClick += PathBlock_OnClick;
            UIElements.PathBlock.OnMouseOver += PathBlock_OnMouseOver;
            UIElements.PathBlock.OnMouseLeave += PathBlock_OnMouseLeave;
            Tile.OnClick += Tile_OnClick;
            UIElements.SaveKeybindText.OnClick += SaveKeybindText_OnClick;
            UIElements.LoadKeybindText.OnClick += LoadKeybindText_OnClick;
            UIElements.SelectedBlockTextContainer.AppendElement(UIElements.SelectedBlockText);
        }

        private static void LoadKeybindText_OnClick(UIElement affectedElement) {
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
            using SaveFileDialog fileDialog = new();

            fileDialog.InitialDirectory = Path.Combine(TowerDefense.ExePath, "Stages");
            fileDialog.FileName = "MyStage";
            fileDialog.Filter = "stage files (*.stg)|*.stg";
            fileDialog.OverwritePrompt = true;

            if (fileDialog.ShowDialog() == DialogResult.OK) {
                Stage.currentLoadedStage = new(fileDialog.FileName);
                Stage.currentLoadedStage.TileMap.Clear();
                foreach (var tl in Tile.Tiles) {
                    Stage.currentLoadedStage.TileMap.Add(tl);
                }
                Stage.SaveStage(Stage.currentLoadedStage);
            }
        }

        internal static void Update() {

        }

        private static void PathBlock_OnMouseLeave(UIElement affectedElement) {
            if (selectedType <= 0) {
                UIElements.SelectedBlockText.Visible = false;
            }
        }

        private static void PathBlock_OnMouseOver(UIElement affectedElement) {
            UIElements.SelectedBlockText.Visible = true;
            UIElements.SelectedBlockText.Text = "Path Block";
        }

        private static void Tile_OnClick(Tile tile) {
            if (selectedType > 0) {
                tile.type = selectedType;
                tile.RedrawTile();
            }
        }

        private static void PathBlock_OnClick(UIElement affectedElement) {
            if (selectedType != 1) {
                selectedType = 1;
            }
            else {
                selectedType = 0;
            }
        }
    }
}