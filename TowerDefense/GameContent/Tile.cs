using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDGame.Internals.Loaders;
using TDGame.Internals.Common;

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

        public int WorldX => X * 32;
        public int WorldY => Y * 32;

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

        internal Tile(int x, int y, bool elevated = false, int type = TileID.None) {
            X = x;
            Y = y;
            Elevated = elevated;
            this.type = type;
            switch (type) {
                case TileID.Path:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/DarkGrayTile");
                    break;
                case TileID.Wall:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/LightGrayTile");
                    break;
                case TileID.Exit:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/BrownTile");
                    break;
            }
            Tiles[X, Y] = this;
        }

        internal void Update() {

        }

        public void RedrawTile() {
            switch (type) {
                case TileID.Path:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/DarkGrayTile");
                    break;
                case TileID.Wall:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/LightGrayTile");
                    break;
                case TileID.Exit:
                    Texture = Resources.GetResourceBJ<Texture2D>("Assets/BrownTile");
                    break;
            }
        }

        public int Distance(Tile tile) => (int)Vector2.Distance(new(WorldX, WorldY), new(tile.WorldX, tile.WorldY));

        public Tile GetTileAbove() => Tiles.IndexInRange(X, Y - 1) ? Tiles[X, Y - 1] : Tiles[0, 0];

        public Tile GetTileBelow() => Tiles.IndexInRange(X, Y + 1) ? Tiles[X, Y + 1] : Tiles[0, 0];

        public Tile GetTileRight() => Tiles.IndexInRange(X + 1, Y) ? Tiles[X + 1, Y] : Tiles[0, 0];

        public Tile GetTileLeft() => Tiles.IndexInRange(X - 1, Y) ? Tiles[X - 1, Y] : Tiles[0, 0];

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

    public static class TileID
    {
        public const int Invalid = -1;

        public const int None = 0;

        public const int Path = 1;

        public const int Wall = 2;

        public const int Entrance = 3;

        public const int Exit = 4;
    }
}