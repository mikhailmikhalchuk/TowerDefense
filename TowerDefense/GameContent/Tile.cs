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

        public static Tile[,] Tiles = new Tile[1000, 1000];

        public int type;

        public bool Elevated { get; }

        public const int MAX_TILES = 16000;

        public bool mouseHovering;

        public int X { get; }
        public int Y { get; }

        public int WorldX { get; }
        public int WorldY { get; }

        public Texture2D Texture { get; private set; } = Resources.GetResourceBJ<Texture2D>("UIPanelBackground");

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

        public event MouseEvent OnMouseClick;

        public event MouseEvent OnMouseRightClick;

        public event MouseEvent OnMouseOver;

        public event MouseEvent OnMouseLeave;

        internal Tile(int x, int y, bool elevated, int type = 0) {
            X = x;
            Y = y;
            Elevated = elevated;
            this.type = type;
            switch (type) {
                case 1:
                    Texture = Resources.GetResourceBJ<Texture2D>("UIPanelBackground");
                    break;
            }
        }

        internal void Update() {
        }

        public void MouseClick() {
            OnMouseClick?.Invoke(this);
        }

        public void MouseRightClick() {
            OnMouseRightClick?.Invoke(this);
        }

        public void MouseOver() {
            OnMouseOver?.Invoke(this);
        }

        public void MouseLeave() {
            OnMouseLeave?.Invoke(this);
        }
    }
}