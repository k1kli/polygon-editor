using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.VectorOperations
{
    public struct Vector
    {
        public Vector(float x, float y)
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

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(Vector v1, Vector v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }

        public static float DotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector vector &&
                   X == vector.X &&
                   Y == vector.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public Vector RotateClockwiseAround(Vector centerOfRotation, float rotationAngle)
        {
            var (magnitude, angle) = (this - centerOfRotation).ToPolar();
            return FromPolar(magnitude, angle + rotationAngle)+centerOfRotation;
        }

        public Vector Normalized => this / Magnitude;

        public (float magnitude, float angle) ToPolar()
        {
            return (Magnitude, (float)Math.Atan2(Y, X));
        }
        public static Vector FromPolar(float magnitude, float angle)
        {
            return new Vector((float)Math.Cos(angle), (float)Math.Sin(angle))*magnitude;
        }
    }
}
