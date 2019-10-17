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

        public Vector GetPerpendicular()
        {
            return new Vector(-this.Y, this.X);
        }

        public static Vector? FindConnectionPoint(Vector v1Start, Vector v1End, Vector v2Start, Vector v2End)
        {
            var (A, B, C) = FindEquationOfLinePassingThrough(v1Start, v1End);
            var (D, E, F) = FindEquationOfLinePassingThrough(v2Start, v2End);
            if (Math.Abs(A) <= 0.01)
            {
                return new Vector((E * C + F * B) / (-D * B), C/B);
            }
            if (Math.Abs(A * E - B * D) <= 0.01) return null;
            Vector res = new Vector();
            res.Y = (D * C - F * A) / (E * A - B * D);
            res.X = (B * res.Y + C) / (-A);
            return res;
        }

        public static (float A, float B, float C) FindEquationOfLinePassingThrough(Vector v1, Vector v2)
        {
            return (v1.Y - v2.Y, v2.X - v1.X, v1.X * v2.Y - v2.X * v1.Y);
        }

        public static Vector operator+(Vector v1, Vector v2)
        {
            return new Vector() { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector() { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }
        public static Vector operator -(Vector v)
        {
            return new Vector() { X = -v.X, Y = -v.Y };
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
