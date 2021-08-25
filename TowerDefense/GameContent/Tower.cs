using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TDGame.GameContent
{
    public class Tower
    {
        public static List<Tower> TotalTowers { get; private set; } = new();

        public Tower() {
            TotalTowers.Add(this);
        }

        internal void Update() {
        
        }

        internal void Draw() {
        
        }
    }
}