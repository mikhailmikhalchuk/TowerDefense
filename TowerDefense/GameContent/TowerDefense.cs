using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TDGame.Internals;
using TDGame.Internals.UI;
using TDGame.Internals.Common.GameUI;
using TDGame.Internals.Common;
using System.Linq;

namespace TDGame.GameContent
{
    public class TowerDefense : Game
    {
        public static TowerDefense Instance
        {
            get; private set;
        }

        public static Logger BaseLogger { get; } = new(ExePath, "client_logger");

        public static string ExePath => Assembly.GetExecutingAssembly().Location.Replace(@$"\TowerDefense.dll", string.Empty);
        public static SpriteBatch spriteBatch;
        
        public readonly GraphicsDeviceManager graphics;

        public struct UITextures
        {
            public static Texture2D UIPanelBackground;
            public static Texture2D UIPanelBackgroundCorner;
        }

        public struct Fonts
        {
            public static SpriteFont DefaultFont;
        }

        private UIElement lastElementClicked;

        private Tile lastTileClicked;

        public TowerDefense() : base()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            for (int i = 0; i < Utils.WindowWidth / 16; i++) {
                for (int j = 0; j < Utils.WindowHeight / 16; j++) {
                    new Tile(i, j);
                }
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new(GraphicsDevice);
            UITextures.UIPanelBackground = Content.Load<Texture2D>("Assets/UIPanelBackground");
            UITextures.UIPanelBackgroundCorner = Content.Load<Texture2D>("Assets/UIPanelBackgroundCorner");
            Fonts.DefaultFont = Content.Load<SpriteFont>("Assets/DefaultFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            Input.HandleInput();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            foreach (var element in UIElement.TotalElements)
                element?.Draw();

            foreach (var tower in Tower.TotalTowers)
                tower?.Draw();

            foreach (var enemy in Enemy.TotalEnemies)
                enemy?.Draw();

            foreach (var tile in Tile.Tiles)
                tile?.Draw();

            if (Instance.IsActive) {
                foreach (var parent in UIElement.TotalElements.ToList()) {
                    foreach (var element in parent.Children) {
                        if (!element.MouseHovering && element.InteractionBox.Contains(Utils.MousePosition)) {
                            element?.MouseOver();
                            element.MouseHovering = true;
                        }
                        else if (element.MouseHovering && !element.InteractionBox.Contains(Utils.MousePosition)) {
                            element?.MouseLeave();
                            element.MouseHovering = false;
                        }
                        if (Input.MouseLeft && Utils.MouseOnScreenProtected && element != lastElementClicked) {
                            element?.MouseClick();
                            lastElementClicked = element;
                        }
                        if (Input.MouseRight && Utils.MouseOnScreenProtected && element != lastElementClicked) {
                            element?.MouseRightClick();
                            lastElementClicked = element;
                        }
                        if (Input.MouseMiddle && Utils.MouseOnScreenProtected && element != lastElementClicked) {
                            element?.MouseMiddleClick();
                            lastElementClicked = element;
                        }
                    }
                }
                foreach (var tile in Tile.Tiles) {
                    if (tile == null)
                        continue;
                    if (!tile.mouseHovering && tile.CollisionBox.Contains(Utils.MousePosition)) {
                        tile?.MouseOver();
                        tile.mouseHovering = true;
                    }
                    else if (tile.mouseHovering && !tile.CollisionBox.Contains(Utils.MousePosition)) {
                        tile?.MouseLeave();
                        tile.mouseHovering = false;
                    }
                    if (Input.MouseLeft && Utils.MouseOnScreenProtected && tile != lastTileClicked) {
                        tile?.MouseClick();
                        lastTileClicked = tile;
                    }
                    if (Input.MouseRight && Utils.MouseOnScreenProtected && tile != lastTileClicked) {
                        tile?.MouseRightClick();
                        lastTileClicked = tile;
                    }
                }
            }

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
