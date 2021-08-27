using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TDGame.GameContent;

namespace TDGame.Internals.Common
{
    public static class Utils
    {
        public static int WindowWidth => TowerDefense.Instance.Window.ClientBounds.Width;

        public static int WindowHeight => TowerDefense.Instance.Window.ClientBounds.Height;

        public static Vector2 WindowBounds => new(WindowWidth, WindowHeight);

        public static Vector2 WindowTopLeft => new(0, 0);

        public static bool WindowActive => TowerDefense.Instance.IsActive;

        public static Vector2 MousePosition => new(Input.CurrentMouseSnapshot.X, Input.CurrentMouseSnapshot.Y);

        public static bool MouseOnScreenProtected => MousePosition.X > 16 && MousePosition.X < WindowWidth - 16 && MousePosition.Y > 16 && MousePosition.Y < WindowHeight - 16;

        public static Point ToPoint(this Vector2 vector) => new((int)vector.X, (int)vector.Y);
    }
}