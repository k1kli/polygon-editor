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
                p.Next = new PolyPoint(points[i].X, points[i].Y, this);
                p.Next.Previous = p;
                p = p.Next;
            }
            p.Next = First;
            First.Previous = p;
            this.color = color;
            PointCount = points.Length;
        }

        public bool Remove(PolyPoint polyPoint)
        {
            if (PointCount <= 3) return false;
            polyPoint.Next.Previous = polyPoint.Previous;
            polyPoint.Previous.Next = polyPoint.Next;
            if (polyPoint == First) First = polyPoint.Next;
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

        public (int dist, PolyPoint p1, PolyPoint p2) TrySelectEdge(int x, int y, int distanceLimit)
        {
            PolyPoint res = null;
            PolyPoint point = First;
            PolyPoint next = null;
            distanceLimit *= distanceLimit;
            int minDistDifference = Int32.MaxValue;
            int distDifference;
            do
            {
                distDifference = (int)SquareDistToLineSegment(x, y, point, point.Next);
                if (distDifference < distanceLimit)
                {
                    if (res == null || distDifference < minDistDifference)
                    {
                        res = point;
                        minDistDifference = distDifference;
                        next = point.Next;
                    }
                }
                point = point.Next;
            } while (point != First);
            if(res != null) distDifference = (int)Math.Sqrt(distDifference);
            return (distDifference, res, next);
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
            PolyPoint p = p1.Next = p2.Previous = new PolyPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, this);
            p.Next = p2;
            p.Previous = p1;
            PointCount++;
            return;
        }

        private float SquareDistToLineSegment(int x, int y, PolyPoint p1, PolyPoint p2)
        {
            float squareDist(float x0, float y0, float x1, float y1)
            {
                return (x0 -x1) * (x0 - x1) + (y0 - y1) * (y0 - y1);
            }
            float dotProduct(float x0, float y0, float x1, float y1)
            {
                return x0 * x1 + y0 * y1;
            }
            //https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
            float lengthOfSegmentSquared = squareDist(p1.X,p1.Y,p2.X,p2.Y);
            if (Math.Abs(lengthOfSegmentSquared) < float.Epsilon) return squareDist(x, y, p1.X, p1.Y);
            float t = Math.Max(0, Math.Min(1, dotProduct(x - p1.X, y - p1.Y, p2.X - p1.X, p2.Y - p1.Y) / lengthOfSegmentSquared));
            float projectionX = p1.X + t * (p2.X - p1.X);
            float projectionY = p1.Y + t * (p2.Y - p1.Y);
            return squareDist(projectionX, projectionY, x, y);
        }

    }
}
