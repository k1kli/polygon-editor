using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.VectorOperations
{
    public class Circle
    {
        public Vector Center { get; set; }
        public float Radius { get; set; }
        public Circle(Vector center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        public Vector[] GetIntersectionPointsWith(Circle another)
        {
            //https://stackoverflow.com/questions/3349125/circle-circle-intersection-points
            if (another == this)
            {
                throw new Exception("Same circles, infinite intersection points");
            }
            float d = (this.Center - another.Center).Magnitude;
            if (
                d > this.Radius + another.Radius ||         //circles are separate - no intersection points
                d < Math.Abs(this.Radius - another.Radius)  //one circle is contained within another - no intersection points
                )
                return new Vector[0];
            if (d == this.Radius + another.Radius) // circles touch at one point
                return new Vector[] { this.Center + (another.Center - this.Center) * this.Radius / d };

            //circles touch at two points
            float a = (this.Radius * this.Radius - another.Radius * another.Radius + d * d) / (2 * d);
            float h = (float)Math.Sqrt(this.Radius * this.Radius - a * a);

            Vector pointBetweenIntersections = this.Center + a* (another.Center - this.Center) / d;
            Vector fromPointBetweenIntersectionsToIntersectionVector
                = new Vector(another.Center.Y - this.Center.Y, this.Center.X - another.Center.X) * h / d;
            return new Vector[]
            {
                pointBetweenIntersections + fromPointBetweenIntersectionsToIntersectionVector,
                pointBetweenIntersections - fromPointBetweenIntersectionsToIntersectionVector
            };

        }

        private static readonly float[] qResults = new float[2];
        public Vector[] GetIntersectionPointsWith((float A, float B, float C) lineEq)
        {
            //solving pair of equations:
            //Ax+By+C = 0
            //(x-x1)^2+(y-y1)^2=r^2
            if(Math.Abs(lineEq.A) < 0.0001f)
            {
                int zerosCount = Helper.GetQuadraticZeros(
                    1,
                    -2 * Center.X,
                    Center.X * Center.X + lineEq.C * lineEq.C / (lineEq.B * lineEq.B)
                    + 2 * Center.Y * lineEq.C / lineEq.B + Center.Y * Center.Y - Radius * Radius,
                    qResults);
                Vector baseVec = new Vector(1, -lineEq.C / lineEq.B);
                if (zerosCount == 0) return new Vector[0];
                if (zerosCount == 1) return new Vector[1] { baseVec * qResults[0] };
                return new Vector[2] { baseVec * qResults[0], baseVec * qResults[1] };
            }
            else
            {
                int zerosCount = Helper.GetQuadraticZeros(
                    lineEq.B * lineEq.B + lineEq.A*lineEq.A,
                    2 * (lineEq.B * lineEq.C + Center.X * lineEq.A * lineEq.B - lineEq.A * lineEq.A * Center.Y),
                    lineEq.C * lineEq.C + 2 * lineEq.A * lineEq.C * Center.X + Center.X * Center.X * lineEq.A * lineEq.A
                    + lineEq.A * lineEq.A * Center.Y * Center.Y - lineEq.A * lineEq.A * Radius * Radius,
                    qResults);
                Vector baseVec = new Vector(-lineEq.B / lineEq.A, 1);
                Vector addVec = new Vector(-lineEq.C / lineEq.A, 0);
                if (zerosCount == 0) return new Vector[0];
                if (zerosCount == 1) return new Vector[1] { baseVec * qResults[0]+addVec };
                return new Vector[2] { baseVec * qResults[0] + addVec, baseVec * qResults[1] + addVec };

            }
        }

        public static bool operator ==(Circle v1, Circle v2)
        {
            return v1.Radius == v2.Radius && v1.Center == v2.Center;
        }
        public static bool operator !=(Circle v1, Circle v2)
        {
            return v1.Radius != v2.Radius || v1.Center != v2.Center;
        }

        public override bool Equals(object obj)
        {
            return obj is Circle circle &&
                   EqualityComparer<Vector>.Default.Equals(Center, circle.Center) &&
                   Radius == circle.Radius;
        }

        public override int GetHashCode()
        {
            var hashCode = 1641483799;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector>.Default.GetHashCode(Center);
            hashCode = hashCode * -1521134295 + Radius.GetHashCode();
            return hashCode;
        }
    }
}
