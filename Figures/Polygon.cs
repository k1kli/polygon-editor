using PolygonEditor.VectorOperations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Figures
{
    public class Polygon
    {
        public PolyPoint First { get; private set; }
        public int PointCount { get; private set; }
        Color color;
        public Polygon(Point[] points, Color color)
        {
            First = new PolyPoint(points[0].X, points[0].Y, this);
            PolyPoint p = First;
            for(int i = 1; i < points.Length;i++)
            {
                
                Edge.Link(p, new PolyPoint(points[i].X, points[i].Y, this));
                p = p.Next;
            }
            Edge.Link(p, First);
            this.color = color;
            PointCount = points.Length;
        }

        public bool Remove(PolyPoint polyPoint)
        {
            if (PointCount <= 3) return false;
            if (polyPoint == First) First = polyPoint.Next;
            Edge.Join(polyPoint.PreviousEdge, polyPoint.NextEdge);
            PointCount--;
            return true;
        }


        public void Draw(ArrayBitmap bitmap)
        {
            PolyPoint point = First;
            do
            {
                DrawPoint(bitmap, point);
                bitmap.DrawLine(point, point.Next, color);
                if(point.NextEdge.EnactedRestriction == Edge.Restriction.SameSize)
                {
                    DrawSameSizeRestrictionIcon(bitmap, point.NextEdge);
                }
                point = point.Next;
            } while (point != First);
        }


        const int pointSize = 10;
        const int pointSizeDiv2 = pointSize / 2;
        public void DrawPoint(ArrayBitmap bitmap, PolyPoint point)
        {
            int startX = point.X - pointSizeDiv2, endX = point.X + pointSizeDiv2;
            int startY = point.Y - pointSizeDiv2, endY = point.Y + pointSizeDiv2;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, color);
                }
        }
        public void DrawSameSizeRestrictionIcon(ArrayBitmap bitmap, Edge e)
        {
            Point point = new Point((e.Previous.X + e.Next.X)/2, (e.Previous.Y + e.Next.Y)/2);
            int startX = point.X - pointSizeDiv2, endX = point.X + pointSizeDiv2;
            int startY = point.Y - pointSizeDiv2, endY = point.Y + pointSizeDiv2;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, e.RestrictionData);
                }
            int padding = 3;
            for (int x = startX+ padding; x <= endX- padding; x++)
                for (int y = startY+ padding; y <= endY- padding; y++)
                {
                    bitmap.SetPixel(x, y, Color.White);
                }
        }

        public (int dist, PolyPoint p) TrySelectPoint(int x, int y, int distanceLimit)
        {
            PolyPoint res = null;
            PolyPoint point = First ;
            int minDist = Int32.MaxValue;
            do
            {
                int xDist = Math.Abs(point.X - x);
                int yDist = Math.Abs(point.Y - y);
                if ( xDist + yDist  < distanceLimit)
                {
                    if(res == null || xDist + yDist < minDist)
                    {
                        res = point;
                        minDist = xDist + yDist;
                    }
                }
                point = point.Next;
            } while (point != First);
            return (minDist, res);
        }

        public (int dist, Edge e) TrySelectEdge(int x, int y, int distanceLimit)
        {
            Edge res = null;
            Edge firstEdge = First.NextEdge;
            Edge edge = firstEdge;
            distanceLimit *= distanceLimit;
            int minDistDifference = Int32.MaxValue;
            int distDifference;
            do
            {
                distDifference = (int)SquareDistToLineSegment(new Vector(x, y), edge);
                if (distDifference < distanceLimit)
                {
                    if (res == null || distDifference < minDistDifference)
                    {
                        res = edge;
                        minDistDifference = distDifference;
                    }
                }
                edge = edge.Next.NextEdge;
            } while (edge != firstEdge);
            if(res != null) distDifference = (int)Math.Sqrt(distDifference);
            return (distDifference, res);
        }

        public void AddPointBetween(int x, int y, PolyPoint p1, PolyPoint p2)
        {
            if (p1.Parent != this || p2.Parent != this) throw new Exception($"This polygon is not the owner of {p1}, {p2}");
            if(p2.Next == p1)
            {
                PolyPoint buff = p2;
                p2 = p1;
                p1 = buff;
            }
            if (p1.Next != p2) throw new Exception($"These points aren't adjacent");
            Edge.Split(p1.NextEdge, new PolyPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, this));
            PointCount++;
            return;
        }

        private float SquareDistToLineSegment(Vector point, Edge e)
        {
            //https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
            Vector p1 = e.Previous.GetVector(), p2 = e.Next.GetVector();
            float mgnSquared = (p2-p1).MagnitudeSqr;
            if (Math.Abs(mgnSquared) < float.Epsilon) return (point-p1).MagnitudeSqr;
            float t = Math.Max(0, Math.Min(1, Vector.DotProduct(point-p1, p2-p1) / mgnSquared));
            Vector projection = p1 + t * (p2 - p1);
            return (projection-point).MagnitudeSqr;
        }
        public void EnactRestriction(Edge e)
        {
            //e is the edge to be changed to comform to restriction
            float desiredLen = e.RelatedEdge.GetLength();
            Vector previous = e.Previous.GetVector();
            Vector next = e.Next.GetVector();
            if (e.PreviousEdge.EnactedRestriction==Edge.Restriction.None)
            {
                Vector v = previous - next;
                v = v * desiredLen / v.Magnitude;
                e.Previous.FromVector(next + v);
            }
            else if (e.NextEdge.EnactedRestriction == Edge.Restriction.None)
            {
                Vector v = next - previous;
                v = v * desiredLen / v.Magnitude;
                e.Next.FromVector(previous + v);
            }
            //else
            //{
            //    float curLen = e.GetLength();

            //    PolyPoint flatPrevEnd = e.Previous;
            //    PolyPoint flatNextEnd = e.Next;
            //    float remainingLen = desiredLen - curLen;
            //    while(true)
            //    {
            //        float prevLen = flatPrevEnd.PreviousEdge.GetLength();
            //        if (prevLen > remainingLen) break;
            //        remainingLen -= prevLen;
            //        float nextLen = flatNextEnd.NextEdge.GetLength();
            //        if (nextLen > remainingLen) break;
            //    }
            //}
        }
        public void ClearRestriction(Edge e)
        {
        }

    }
}
