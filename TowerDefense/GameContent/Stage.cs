using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.GameContent
{
    public class Stage
    {
        public string Name { get; private set; } = string.Empty;

        public List<Tile> TileMap { get; private set; } = new();

        public static Stage currentLoadedStage;

        public Stage(string name) {
            Name = name;
        }
    }
}