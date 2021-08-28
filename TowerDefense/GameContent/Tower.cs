using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TDGame.GameContent
{
    public class Tower
    {
        public static List<Tower> TotalTowers { get; private set; } = new();

        public int Health { get; set; } = 1000;

        public int Damage { get; set; } = 10;

        public Tower() {
            TotalTowers.Add(this);
        }

        internal void Update() {
        
        }

        internal void Draw() {
        
        }
    }
}