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
