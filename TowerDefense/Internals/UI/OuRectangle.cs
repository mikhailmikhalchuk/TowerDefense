using System;
using Microsoft.Xna.Framework;

namespace TDGame.Internals.UI
{
    public struct OuRectangle : IEquatable<OuRectangle>
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public Vector2 Center => new(X + Width * 0.5f, Y + Height * 0.5f);

        public Vector2 Position => new(X, Y);

        public OuRectangle(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle ToRectangle() {
            return new((int)X, (int)Y, (int)Width, (int)Height);
        }

        public bool Contains(Vector2 vector) {
            return Contains(vector.ToPoint());
        }

        public bool Contains(Point point) {
            return ToRectangle().Contains(point);
        }

        public static bool operator ==(OuRectangle first, OuRectangle second) {
            return first.X == second.X && first.Y == second.Y && first.Width == second.Width && first.Height == second.Height;
        }

        public static bool operator !=(OuRectangle first, OuRectangle second) {
            return !(first.X == second.X && first.Y == second.Y && first.Width == second.Width && first.Height == second.Height);
        }

        public bool Equals(OuRectangle other) {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj) {
            if (obj is OuRectangle rectangle) {
                return Equals(rectangle);
            }
            return false;
        }

        public override int GetHashCode() {
            return X.GetHashCode() + Y.GetHashCode() + Width.GetHashCode() + Height.GetHashCode();
        }

        public override string ToString() {
            return $"X:{X} Y:{Y} Width:{Width} Height:{Height}";
        }
    }
}