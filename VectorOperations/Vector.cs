using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.VectorOperations
{
    public struct Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Magnitude =>
            (float)Math.Sqrt(MagnitudeSqr);
        public float MagnitudeSqr => X * X + Y * Y;

        public static Vector operator+(Vector v1, Vector v2)
        {
            return new Vector() { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector() { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }
        public static Vector operator *(Vector v, float factor)
        {
            return new Vector() { X = v.X*factor, Y = v.Y * factor };
        }
        public static Vector operator *(float factor, Vector v)
        {
            return v*factor;
        }
        public static Vector operator /(Vector v, float factor)
        {
            return new Vector() { X = v.X / factor, Y = v.Y / factor };
        }

        public static float DotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
    }
}
