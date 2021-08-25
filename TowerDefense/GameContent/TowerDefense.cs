using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TDGame.Internals.UI;
using TDGame.Internals.Common.GameUI;

namespace TDGame.GameContent
{
    public class TowerDefense : Game
    {
        public static TowerDefense Instance
        {
            get; private set;
        }

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

        private UIText test;

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
        }

        protected override void LoadContent()
        {
            spriteBatch = new(GraphicsDevice);
            UITextures.UIPanelBackground = Content.Load<Texture2D>("Assets/UIPanelBackground");
            UITextures.UIPanelBackgroundCorner = Content.Load<Texture2D>("Assets/UIPanelBackgroundCorner");
            Fonts.DefaultFont = Content.Load<SpriteFont>("Assets/DefaultFont");
            test = new("Hi", Fonts.DefaultFont, Color.White);
            test.InteractionBox = new OuRectangle(100, 100, 100, 50);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
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

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
