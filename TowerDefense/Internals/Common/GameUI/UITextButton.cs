﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDGame.GameContent;

namespace TDGame.Internals.Common.GameUI
{
    public class UITextButton : UIPanel
    {
        public string Text
        {
            get; set;
        }

        public SpriteFont Font
        {
            get; set;
        }

        public float Scale
        {
            get; set;
        }

        public Color TextColor
        {
            get; set;
        }

        private byte baseAlpha;

        public UITextButton(string text, SpriteFont font, Color textColor, float scale = 1f) {
            Text = text;
            Font = font;
            TextColor = textColor;
            Scale = scale;
        }

        public override void Draw() {
            base.Draw();
            if (!Visible)
                return;

            TowerDefense.spriteBatch.DrawString(Font, Text, InteractionBox.Center - Font.MeasureString(Text) / 2f, TextColor, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }

        public override void MouseOver() {
            base.MouseOver();
            baseAlpha = BackgroundColor.A;
            BackgroundColor.A = 100;
        }

        public override void MouseLeave() {
            base.MouseLeave();
            BackgroundColor.A = baseAlpha;
        }
    }
}