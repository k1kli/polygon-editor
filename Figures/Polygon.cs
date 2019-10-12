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
            polyPoint.PreviousEdge.ClearRestriction();
            polyPoint.NextEdge.ClearRestriction();
            Edge.Join(polyPoint.PreviousEdge, polyPoint.NextEdge);
            PointCount--;
            return true;
        }

        public void MoveEdgeWithRestrictions(Edge edge, Vector delta)
        {
            edge.Previous.FromVector(edge.Previous.GetVector() + delta);
            edge.Next.FromVector(edge.Next.GetVector() + delta);
            if (edge.PreviousEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(edge.PreviousEdge.RelatedEdge);
            if (edge.NextEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(edge.NextEdge.RelatedEdge);
        }

        public void MoveVertexWithRestrictions(PolyPoint point, Vector delta)
        {
            point.FromVector(point.GetVector() + delta);
            if(point.PreviousEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(point.PreviousEdge.RelatedEdge);
            if (point.NextEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(point.NextEdge.RelatedEdge);
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
            int startX = (int)point.X - pointSizeDiv2, endX = (int)point.X + pointSizeDiv2;
            int startY = (int)point.Y - pointSizeDiv2, endY = (int)point.Y + pointSizeDiv2;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, color);
                }
        }
        public void DrawSameSizeRestrictionIcon(ArrayBitmap bitmap, Edge e)
        {
            Point point = new Point(((int)e.Previous.X + (int)e.Next.X)/2, ((int)e.Previous.Y + (int)e.Next.Y)/2);
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
                int xDist = Math.Abs((int)point.X - x);
                int yDist = Math.Abs((int)point.Y - y);
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

        public void AddPointBetween(PolyPoint p1, PolyPoint p2)
        {
            if (p1.Parent != this || p2.Parent != this) throw new Exception($"This polygon is not the owner of {p1}, {p2}");
            if(p2.Next == p1)
            {
                PolyPoint buff = p2;
                p2 = p1;
                p1 = buff;
            }
            if (p1.Next != p2) throw new Exception($"These points aren't adjacent");
            p1.NextEdge.ClearRestriction();
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
            if (Math.Abs((previous - next).MagnitudeSqr - desiredLen * desiredLen) < 1) return;
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
            else if (e.NextEdge == e.RelatedEdge)
            {
                //RotateToConnect(e, desiredLen, true);
                if (desiredLen * 2 < (previous - e.RelatedEdge.Next.GetVector()).Magnitude)
                {
                    e.Next.FromVector((previous + e.RelatedEdge.Next.GetVector()) / 2);
                }
                else
                {
                    Vector[] possibleConnectionPoints = new Circle(previous, desiredLen)
                        .GetIntersectionPointsWith(new Circle(e.RelatedEdge.Next.GetVector(), desiredLen));

                    if (possibleConnectionPoints.Length == 1 ||
                        Vector.DotProduct((possibleConnectionPoints[0] - previous).Normalized, (next - previous).Normalized) >
                        Vector.DotProduct((possibleConnectionPoints[1] - previous).Normalized, (next - previous).Normalized))
                        e.Next.FromVector(possibleConnectionPoints[0]);
                    else
                    {
                        e.Next.FromVector(possibleConnectionPoints[1]);
                    }
                }
            }
            else if (e.PreviousEdge == e.RelatedEdge)
            {
                //RotateToConnect(e, desiredLen, false);
                if (desiredLen * 2 < (next - e.RelatedEdge.Previous.GetVector()).Magnitude)
                {
                    e.Previous.FromVector((next + e.RelatedEdge.Previous.GetVector()) / 2);
                }
                else
                {
                    Vector[] possibleConnectionPoints = new Circle(next, desiredLen)
                        .GetIntersectionPointsWith(new Circle(e.RelatedEdge.Previous.GetVector(), desiredLen));

                    if (possibleConnectionPoints.Length == 1 ||
                        Vector.DotProduct((possibleConnectionPoints[0] - next).Normalized, (previous - next).Normalized) >
                        Vector.DotProduct((possibleConnectionPoints[1] - next).Normalized, (previous - next).Normalized))
                        e.Previous.FromVector(possibleConnectionPoints[0]);
                    else
                    {
                        e.Previous.FromVector(possibleConnectionPoints[1]);
                    }
                }
            }
            else
            {
                if (!RotateToConnect(e, desiredLen, true))
                    RotateToConnect(e, desiredLen, false);
            }
        }
        private bool RotateToConnect(Edge e, float desiredLength, bool forward)
        {
            Branch branch = GetBranchFromRelatedEdge(e, forward);
            (Vector edgeStart, Vector edgeEnd) = GetEdgeStartEnd(e, !forward);
            Vector? newConnectionPoint
                = branch.ConnectionPointWith(edgeStart, edgeStart + (edgeEnd - edgeStart).Normalized * desiredLength);
            if (newConnectionPoint.HasValue)
            {
                branch.RotateToEndAt(newConnectionPoint.Value);
                return true;
            }
            return false;
        }
        private Branch GetBranchFromRelatedEdge(Edge e, bool forward)
        {
            if (forward) return new Branch(e.RelatedEdge.Next, e.Previous, true);
            return new Branch(e.RelatedEdge.Previous, e.Next, false);
        }
        private (Vector edgeStart, Vector edgeEnd) GetEdgeStartEnd(Edge e, bool forward)
        {
            if (forward) return (e.Previous.GetVector(), e.Next.GetVector());
            return (e.Next.GetVector(), e.Previous.GetVector());
        }
        public void ClearRestriction(Edge e)
        {
        }

        private class Branch
        {
            public Branch(PolyPoint start, PolyPoint end, bool forward)
            {
                Start = start;
                End = end;
                Forward = forward;
            }

            public PolyPoint Start { get; set; }
            public PolyPoint End { get; set; }
            public bool Forward { get; set; }

            private PolyPoint NextOnBranch(PolyPoint point)
            {
                return Forward ? point.Next : point.Previous;
            }
            public float Length => (Start.GetVector() - End.GetVector()).Magnitude;
            public Vector? ConnectionPointWith(Vector edgeStart, Vector edgeEnd)
            {
                if (edgeStart == Start.GetVector()) return End.GetVector();
                Vector[] intersectionPoints = GetAllPossibleConnectionPointsWith(edgeStart, edgeEnd);
                return GetBestConnectionPoint(intersectionPoints, edgeStart, edgeEnd);
            }

            public void RotateToEndAt(Vector newEnd)
            {
                Vector startVector = Start.GetVector();
                Vector endVector = End.GetVector();
                float rotation = (newEnd-startVector).ToPolar().angle - (endVector-startVector).ToPolar().angle;
                PolyPoint point = NextOnBranch(Start);
                while(point != NextOnBranch(End))
                {
                    point.FromVector(point.GetVector().RotateClockwiseAround(startVector, rotation));
                    point = NextOnBranch(point);
                }
            }

            private Vector[] GetAllPossibleConnectionPointsWith(Vector edgeStart, Vector edgeEnd)
            {
                //edge and branch will be able to connect at intersection points of circles created by rotating them around their start point
                    return new Circle(Start.GetVector(), Length)
                    .GetIntersectionPointsWith(
                        new Circle(edgeStart, (edgeEnd - edgeStart).Magnitude)
                        );
            }

            private Vector? GetBestConnectionPoint(Vector[] allConnectionPoints, Vector edgeStart, Vector edgeEnd)
            {
                //circles don't intersect
                if (allConnectionPoints.Length == 0)
                    return null;
                //return only intersection point
                if (allConnectionPoints.Length == 1)
                    return allConnectionPoints[0];
                //return intersection point that makes new edge more similar to the older edge
                if (Vector.DotProduct(allConnectionPoints[0] - edgeStart, edgeEnd - edgeStart) >
                    Vector.DotProduct(allConnectionPoints[1] - edgeStart, edgeEnd - edgeStart))
                    return allConnectionPoints[0];
                else
                {
                    return allConnectionPoints[1];
                }
            }
        }
    }
}
