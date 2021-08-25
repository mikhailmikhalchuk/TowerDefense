using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.GameContent
{
    public class Enemy
    {
        public static List<Enemy> TotalEnemies { get; private set; } = new();

        public string Name { get; private set; } = string.Empty;

        public Texture2D Texture { get; private set; }

        public Enemy() {
            TotalEnemies.Add(this);
        }

        internal void Update() {
        
        }

        internal void Draw() {
        
        }
    }
}