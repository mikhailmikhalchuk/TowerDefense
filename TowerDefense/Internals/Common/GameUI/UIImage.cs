using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDGame.Internals.UI;
using TDGame.GameContent;

namespace TDGame.Internals.Common.GameUI
{
    public class UIImage : UIElement
    {
        public Texture2D Texture
        {
            get; set;
        }

        public float Scale
        {
            get; set;
        }

        public UIImage(Texture2D texture, float scale) {
            Texture = texture;
            Scale = scale;
        }

        public override void Draw() {
            base.Draw();
            if (!Visible)
                return;

            TowerDefense.spriteBatch.Draw(Texture, InteractionBox.Position, null, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}