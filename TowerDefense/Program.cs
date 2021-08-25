using System;
using TDGame.GameContent;

namespace TDGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TowerDefense())
                game.Run();
        }
    }
}
