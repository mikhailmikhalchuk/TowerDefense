using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TDGame.Internals.Loaders;
using TDGame.Internals.Common;

namespace TDGame.GameContent
{
    public class Enemy
    {
        public static List<Enemy> TotalEnemies { get; private set; } = new();

        public string Name { get; private set; } = string.Empty;

        public Vector2 Position { get; set; }

        public Tile TileAtPosition => Tile.Tiles[(int)Position.X / 32, (int)Position.Y / 32];

        public Texture2D Texture { get; private set; } = Resources.GetResourceBJ<Texture2D>("Assets/BrownTile");

        public Action<Enemy> AI { get; private set; }

        public float Speed { get; set; } = 1f;

        public List<Tile> CurrentPath { get; private set; } = new();

        private int _speedTimer = 400;

        public Enemy(string name, Vector2 position, float speed = 1f, Action<Enemy> ai = null) {
            TotalEnemies.Add(this);
            Name = name;
            Position = position;
            Speed = speed;
            if (ai == null) {
                AI = e =>
                {
                    if (e.CurrentPath.Count == 0) {
                        e.CurrentPath = e.GenerateRandomPath();
                    }
                    if (e._speedTimer < 100 * Speed) {
                        e._speedTimer = 400;
                        if (e.CurrentPath.Count == 0) {
                            e.Remove();
                            TowerDefense.BaseLogger.Write($"Enemy \"{name}\" unexpectedly reached end of pathfinding", Internals.Logger.LogType.Error);
                            return;
                        }
                        e.Position = new(e.CurrentPath[0].WorldX, e.CurrentPath[0].WorldY);
                        e.CurrentPath.RemoveAt(0);
                        if (e.TileAtPosition.type == TileID.Exit) {
                            e.Remove();
                        }
                    }
                    else {
                        e._speedTimer--;
                    }
                };
            }
            else {
                AI = ai;
            }
        }

        internal List<Tile> GenerateRandomPath() {
            bool completedPath = false;
            int attempts = 0;
            List<Tile> path = new();
            List<Tile> tilesAlreadyChosen = new();
            Tile exit = Tile.Tiles[Stage.currentLoadedStage.TileMap.FirstOrDefault(t => t.type == TileID.Exit).X, Stage.currentLoadedStage.TileMap.FirstOrDefault(t => t.type == TileID.Exit).Y];
            while (!completedPath) {
                attempts++;
                if (attempts > 500) {
                    completedPath = true;
                    continue;
                }
                Tile tilePos = path.IndexInRange(path.Count - 1) ? path[^1] : TileAtPosition;
                List<Tile> tilesToConsider = new();
                if (tilePos.GetTileAbove().type != TileID.Wall && tilePos.GetTileAbove().X != 0 && tilePos.GetTileAbove().Y != 0) {
                    if (tilePos.GetTileAbove().type == TileID.Exit) {
                        path.Add(tilePos.GetTileAbove());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileAbove().GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileAbove().GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileAbove().GetTileLeft()) && tilePos.GetTileAbove().Distance(exit) < tilePos.Distance(exit)) {
                        tilesToConsider.Add(tilePos.GetTileAbove());
                    }
                }
                if (tilePos.GetTileBelow().type != TileID.Wall && tilePos.GetTileBelow().X != 0 && tilePos.GetTileBelow().Y != 0) {
                    if (tilePos.GetTileBelow().type == TileID.Exit) {
                        path.Add(tilePos.GetTileBelow());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileBelow()) && !tilesAlreadyChosen.Contains(tilePos.GetTileBelow().GetTileBelow()) && !tilesAlreadyChosen.Contains(tilePos.GetTileBelow().GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileBelow().GetTileLeft()) && tilePos.GetTileBelow().Distance(exit) < tilePos.Distance(exit)) {
                        tilesToConsider.Add(tilePos.GetTileBelow());
                    }
                }
                if (tilePos.GetTileRight().type != TileID.Wall && tilePos.GetTileRight().X != 0 && tilePos.GetTileRight().Y != 0) {
                    if (tilePos.GetTileRight().type == TileID.Exit) {
                        path.Add(tilePos.GetTileRight());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileRight().GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileRight().GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileRight().GetTileBelow()) && tilePos.GetTileRight().Distance(exit) < tilePos.Distance(exit)) {
                        tilesToConsider.Add(tilePos.GetTileRight());
                    }
                }
                if (tilePos.GetTileLeft().type != TileID.Wall && tilePos.GetTileLeft().X != 0 && tilePos.GetTileLeft().Y != 0) {
                    if (tilePos.GetTileLeft().type == TileID.Exit) {
                        path.Add(tilePos.GetTileLeft());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileLeft()) && !tilesAlreadyChosen.Contains(tilePos.GetTileLeft().GetTileLeft()) && !tilesAlreadyChosen.Contains(tilePos.GetTileLeft().GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileLeft().GetTileBelow()) && tilePos.GetTileLeft().Distance(exit) < tilePos.Distance(exit)) {
                        tilesToConsider.Add(tilePos.GetTileLeft());
                    }
                }
                if (tilesToConsider.Count > 0) {
                    int rand = new Random().Next(0, tilesToConsider.Count);
                    path.Add(tilesToConsider[rand]);
                    tilesAlreadyChosen.Add(tilesToConsider[rand]);
                }
            }
            return path;
        }

        internal void Update() {
            AI?.Invoke(this);
        }

        public void Remove() {
            TotalEnemies.Remove(this);
            Position = new(0, 0);
        }

        internal void Draw() {
            TowerDefense.spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), null, Color.White, 0f, Vector2.Zero, default, 0f);
        }
    }
}