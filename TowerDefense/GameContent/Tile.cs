using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDGame.Internals.Loaders;

namespace TDGame.GameContent
{
    public class Tile
    {
        public delegate void InteractionEvent<T>(T entity, Tile tile);

        public delegate void MouseEvent(Tile tile);

        public static Tile[,] Tiles = new Tile[250, 250];

        public int type;

        public bool Elevated { get; }

        public const int MAX_TILES = 400;

        public bool mouseHovering;

        public int X { get; }
        public int Y { get; }

        public int WorldX { get; }
        public int WorldY { get; }

        public Texture2D Texture { get; private set; } = Resources.GetResourceBJ<Texture2D>("Assets/BrownTile");

        public Vector2 Center => new(WorldX + CollisionBox.Width / 2, CollisionBox.Y + CollisionBox.Height / 2);
        public Vector2 Top => new(WorldX + (CollisionBox.Width / 2), WorldY);
        public Vector2 Bottom => new(WorldX + (CollisionBox.Width / 2), WorldY + CollisionBox.Height);
        public Vector2 Left => new(WorldX, WorldY + (CollisionBox.Height / 2));
        public Vector2 Right => new(WorldX + CollisionBox.Width, WorldY + (CollisionBox.Height / 2));

        public Rectangle CollisionBox => new(WorldX, WorldY, 32, 32);

        public event InteractionEvent<Tower> OnTowerPlace;

        public event InteractionEvent<Tower> OnTowerRemove;

        public event InteractionEvent<Enemy> OnEnemyEnter;

        public event InteractionEvent<Enemy> OnEnemyExit;

        public static event MouseEvent OnClick;

        public static event MouseEvent OnRightClick;

        public static event MouseEvent OnLeftRelease;

        public static event MouseEvent OnMouseRightRelease;

        public static event MouseEvent OnMouseOver;

        public static event MouseEvent OnMouseLeave;

        internal Tile(int x, int y, bool elevated = false, int type = 0) {
            X = x;
            Y = y;
            WorldX = x * 32;
            WorldY = y * 32;
            Elevated = elevated;
            this.type = type;
            switch (type) {
                case 1:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/DarkGrayTile");
                    break;
                case 2:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/LightGrayTile");
                    break;
            }
            Tiles[X, Y] = this;
        }

        internal void Update() {

        }

        public void RedrawTile() {
            switch (type) {
                case 1:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/DarkGrayTile");
                    break;
                case 2:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/LightGrayTile");
                    break;
            }
        }

        internal void Draw() {
            TowerDefense.spriteBatch.Draw(Texture, CollisionBox, null, Color.White, 0f, Vector2.Zero, default, 0f);
        }

        public void MouseClick() {
            OnClick?.Invoke(this);
        }

        public void MouseRightClick() {
            OnRightClick?.Invoke(this);
        }

        public void MouseLeftRelease() {
            OnLeftRelease?.Invoke(this);
        }

        public void MouseRightRelease() {
            OnMouseRightRelease?.Invoke(this);
        }

        public void MouseOver() {
            OnMouseOver?.Invoke(this);
        }

        public void MouseLeave() {
            OnMouseLeave?.Invoke(this);
        }
    }
}