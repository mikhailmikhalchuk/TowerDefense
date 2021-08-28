using System;
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

        public Texture2D Texture { get; private set; } = Resources.GetResourceBJ<Texture2D>("Assets/BrownTile");

        public float Speed { get; set; } = 1f;

        private List<Tile> _currentPath = new();

        private int _speedTimer = 0;

        public Enemy(Vector2 position, float speed = 1f) {
            TotalEnemies.Add(this);
            Position = position;
            Speed = speed;
        }

        internal List<Tile> GenerateRandomPath() {
            bool completedPath = false;
            int attempts = 0;
            List<Tile> path = new();
            List<Tile> tilesAlreadyChosen = new();
            while (!completedPath) {
                attempts++;
                if (attempts > 500) {
                    completedPath = true;
                    continue;
                }
                Tile tilePos = path.IndexInRange(path.Count - 1) ? path[path.Count - 1] : Tile.Tiles[(int)Position.X / 32, (int)Position.Y / 32];
                List<Tile> tilesToConsider = new();
                if (tilePos.GetTileAbove().type != TileID.Wall) {
                    if (tilePos.GetTileAbove().type == TileID.Exit) {
                        path.Add(tilePos.GetTileAbove());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileAbove().GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileAbove().GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileAbove().GetTileLeft())) {
                        tilesToConsider.Add(tilePos.GetTileAbove());
                    }
                }
                if (tilePos.GetTileBelow().type != TileID.Wall) {
                    if (tilePos.GetTileBelow().type == TileID.Exit) {
                        path.Add(tilePos.GetTileBelow());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileBelow()) && !tilesAlreadyChosen.Contains(tilePos.GetTileBelow().GetTileBelow()) && !tilesAlreadyChosen.Contains(tilePos.GetTileBelow().GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileBelow().GetTileLeft())) {
                        tilesToConsider.Add(tilePos.GetTileBelow());
                    }
                }
                if (tilePos.GetTileRight().type != TileID.Wall) {
                    if (tilePos.GetTileRight().type == TileID.Exit) {
                        path.Add(tilePos.GetTileRight());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileRight().GetTileRight()) && !tilesAlreadyChosen.Contains(tilePos.GetTileRight().GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileRight().GetTileBelow())) {
                        tilesToConsider.Add(tilePos.GetTileRight());
                    }
                }
                if (tilePos.GetTileLeft().type != TileID.Wall) {
                    if (tilePos.GetTileLeft().type == TileID.Exit) {
                        path.Add(tilePos.GetTileLeft());
                        return path;
                    }
                    if (!tilesAlreadyChosen.Contains(tilePos.GetTileLeft()) && !tilesAlreadyChosen.Contains(tilePos.GetTileLeft().GetTileLeft()) && !tilesAlreadyChosen.Contains(tilePos.GetTileLeft().GetTileAbove()) && !tilesAlreadyChosen.Contains(tilePos.GetTileLeft().GetTileBelow())) {
                        tilesToConsider.Add(tilePos.GetTileLeft());
                    }
                }
                if (tilesToConsider.Count > 0) {
                    int rand = new Random().Next(0, tilesToConsider.Count);
                    TowerDefense.BaseLogger.Write(rand);
                    path.Add(tilesToConsider[rand]);
                    tilesAlreadyChosen.Add(tilesToConsider[rand]);
                }
            }
            return path;
        }

        internal void Update() {
            if (_currentPath.Count == 0) {
                _currentPath = GenerateRandomPath();
            }
            if (_speedTimer < 100 * Speed) {
                _speedTimer = 400;
                Position = new(_currentPath[0].WorldX, _currentPath[0].WorldY);
                _currentPath.RemoveAt(0);
            }
            else {
                _speedTimer--;
            }
        }

        internal void Draw() {
            TowerDefense.spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, 32, 32), null, Color.White, 0f, Vector2.Zero, default, 0f);
        }
    }
}