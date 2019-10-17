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
        private PolyPoint movedPoint = null;
        private Edge movedEdge = null;
        public Polygon(Point[] points, Color color)
        {
            First = new PolyPoint(points[0].X, points[0].Y, this);
            PolyPoint p = First;
            for (int i = 1; i < points.Length; i++)
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
            movedEdge = edge;
            edge.Previous.FromVector(edge.Previous.GetVector() + delta);
            edge.Next.FromVector(edge.Next.GetVector() + delta);
            if (edge.PreviousEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(edge.PreviousEdge.RelatedEdge, Direction.Forward);
            if (edge.NextEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(edge.NextEdge.RelatedEdge, Direction.Backwards);
            movedEdge = null;
        }

        public void MoveVertexWithRestrictions(PolyPoint point, Vector delta)
        {
            movedPoint = point;
            point.FromVector(point.GetVector() + delta);
            if (point.PreviousEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(point.PreviousEdge.RelatedEdge, Direction.Forward);
            if (point.NextEdge.EnactedRestriction != Edge.Restriction.None)
                EnactRestriction(point.NextEdge.RelatedEdge, Direction.Backwards);
            movedPoint = null;
        }

        public void Draw(MemoryBitmap bitmap)
        {
            PolyPoint point = First;
            do
            {
                bitmap.DrawPoint((int)point.X, (int)point.Y, color, Helper.pointSize);
                bitmap.DrawLine(point, point.Next, color);
                if (point.NextEdge.EnactedRestriction == Edge.Restriction.SameSize)
                {
                    Helper.DrawRestrictionSameSize(point.NextEdge, bitmap);
                }
                point = point.Next;
            } while (point != First);
        }



        public (int dist, PolyPoint p) TrySelectPoint(int x, int y, int distanceLimit)
        {
            PolyPoint res = null;
            PolyPoint point = First;
            int minDist = Int32.MaxValue;
            do
            {
                int xDist = Math.Abs((int)point.X - x);
                int yDist = Math.Abs((int)point.Y - y);
                if (xDist + yDist < distanceLimit)
                {
                    if (res == null || xDist + yDist < minDist)
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
            if (res != null) distDifference = (int)Math.Sqrt(distDifference);
            return (distDifference, res);
        }

        public void AddPointBetween(PolyPoint p1, PolyPoint p2)
        {
            if (p1.Parent != this || p2.Parent != this) throw new Exception($"This polygon is not the owner of {p1}, {p2}");
            if (p2.Next == p1)
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
            float mgnSquared = (p2 - p1).MagnitudeSqr;
            if (Math.Abs(mgnSquared) < float.Epsilon) return (point - p1).MagnitudeSqr;
            float t = Math.Max(0, Math.Min(1, Vector.DotProduct(point - p1, p2 - p1) / mgnSquared));
            Vector projection = p1 + t * (p2 - p1);
            return (projection - point).MagnitudeSqr;
        }
        private readonly Stack<(Edge e, Direction directionOfApplying)> enactionQueue = new Stack<(Edge, Direction)>();
        public void EnactRestriction(Edge e, Direction directionOfApplying)
        {
            enactionQueue.Push((e, directionOfApplying));
            while (enactionQueue.Count > 0)
            {
                EnactRestrictions();
            }
        }
        private void EnactRestrictions()
        {
            var (e, directionOfApplying) = enactionQueue.Pop();
            if(e.EnactedRestriction == Edge.Restriction.SameSize)
            {
                EnactRestrictionSameSize(e, directionOfApplying);
            }
            else if(e.EnactedRestriction == Edge.Restriction.Perpendicular)
            {
                EnactRestrictionPerpendicular(e, directionOfApplying);
            }
        }


        private void EnactRestrictionSameSize(Edge e, Direction directionOfApplying)
        {
            if (!IsSameSizeEnactionNeeded(e)) return;
            if (e.NextEdgeInDirection(directionOfApplying).EnactedRestriction == Edge.Restriction.SameSize)
            {
                if (TryEnactNeighborsSameSize(e, directionOfApplying)) return;
            }
            EnactRestrictionSameSizeBreakingNeighbor(e, directionOfApplying);
        }
        private bool IsSameSizeEnactionNeeded(Edge e)
        {
            float desiredLen = e.RelatedEdge.GetLength();
            return Math.Abs(e.GetLength() - desiredLen) >= 1;
        }
        private bool TryEnactNeighborsSameSize(Edge e, Direction neighborDirection)
        {
            Vector[] intersectionPoints =
                new Circle(
                    e.NextInDirection(neighborDirection).NextInDirection(neighborDirection).GetVector(),
                    e.NextEdgeInDirection(neighborDirection).GetLength())
                .GetIntersectionPointsWith(
                    new Circle(e.NextInDirection(neighborDirection.Opposite()).GetVector(), e.RelatedEdge.GetLength()));

            if (intersectionPoints.Length != 0)
            {
                e.NextInDirection(neighborDirection).FromVector(
                    ChooseBestReplacementPoint(
                        intersectionPoints,
                        e.NextInDirection(neighborDirection).GetVector(),
                        e.NextInDirection(neighborDirection.Opposite()).GetVector()));
                return true;
            }
            return false;
        }
        private void EnactRestrictionSameSizeBreakingNeighbor(Edge e, Direction directionOfApplying)
        {
            float desiredLen = e.RelatedEdge.GetLength();
            Vector previous = e.NextInDirection(directionOfApplying.Opposite()).GetVector();
            Vector v = e.GetVectorInDirection(directionOfApplying);
            v = v * desiredLen / v.Magnitude;
            e.NextInDirection(directionOfApplying).FromVector(previous + v);
            InvalidateNeighborsRelation(e, directionOfApplying);
        }
        private void EnactRestrictionPerpendicular(Edge e, Direction directionOfApplying)
        {
            if (!IsPerpendicularEnactionNeeded(e)) return;
            if(e.NextEdgeInDirection(directionOfApplying).EnactedRestriction == Edge.Restriction.Perpendicular)
            {
                if (TryEnactNeighborsPerpendicular(e, directionOfApplying)) return;
            }
            EnactRestrictionPerpendicularBreakingNeighbor(e, directionOfApplying);
        }

        private void EnactRestrictionPerpendicularBreakingNeighbor(Edge e, Direction directionOfApplying)
        {
            Vector myEdgeDirection = e.RelatedEdge.GetVectorInDirection(Direction.Forward).GetPerpendicular();
            myEdgeDirection = ChooseBestReplacementPoint(
                new Vector[] {
                e.NextInDirection(directionOfApplying.Opposite()).GetVector() + myEdgeDirection,
                e.NextInDirection(directionOfApplying.Opposite()).GetVector() - myEdgeDirection
                },
                e.NextInDirection(directionOfApplying).GetVector(),
                e.NextInDirection(directionOfApplying.Opposite()).GetVector()
            ) - e.NextInDirection(directionOfApplying.Opposite()).GetVector();
            myEdgeDirection *= e.GetLength() / myEdgeDirection.Magnitude;
            e.NextInDirection(directionOfApplying)
                .FromVector(myEdgeDirection + e.NextInDirection(directionOfApplying.Opposite()).GetVector());
            InvalidateNeighborsRelation(e, directionOfApplying);
        }

        private bool TryEnactNeighborsPerpendicular(Edge e, Direction directionOfApplying)
        {
            Vector myStart = e.NextInDirection(directionOfApplying.Opposite()).GetVector();
            Vector neighborStart = e.NextInDirection(directionOfApplying).NextInDirection(directionOfApplying).GetVector();
            Vector myEdgeDirection = e.RelatedEdge.GetVectorInDirection(Direction.Forward).GetPerpendicular();
            Vector neighborEdgeDirection = e.NextEdgeInDirection(directionOfApplying).GetVectorInDirection(Direction.Forward);
            Vector? intersectionPoint = Vector.FindConnectionPoint(
                myStart, myStart + myEdgeDirection,
                neighborStart, neighborStart + neighborEdgeDirection);
            if (intersectionPoint == null) return false;
            else e.NextInDirection(directionOfApplying).FromVector(intersectionPoint.Value);
            return true;
        }

        private bool IsPerpendicularEnactionNeeded(Edge e)
        {
            return Math.Abs(Vector.DotProduct(
                e.GetVectorInDirection(Direction.Forward),
                e.RelatedEdge.GetVectorInDirection(Direction.Forward))) > 0.1f;
        }
        private void InvalidateNeighborsRelation(Edge e, Direction neighborDirection)
        {
            if (e.NextEdgeInDirection(neighborDirection).EnactedRestriction != Edge.Restriction.None)
            {
                enactionQueue.Push((e.NextEdgeInDirection(neighborDirection).RelatedEdge, neighborDirection.Opposite()));
            }
        }
        private Vector ChooseBestReplacementPoint(Vector[] possibleReplacements, Vector pointToReplace, Vector anotherPointOnEdge)
        {
            Vector bestReplacement = possibleReplacements[0];
            float bestReplacementDot = Vector.DotProduct(
                (possibleReplacements[0] - anotherPointOnEdge).Normalized,
                (pointToReplace - anotherPointOnEdge).Normalized);
            for (int i = 1; i < possibleReplacements.Length; i++)
            {
                float thisReplacementDot = Vector.DotProduct(
                (possibleReplacements[i] - anotherPointOnEdge).Normalized,
                (pointToReplace - anotherPointOnEdge).Normalized);
                if (thisReplacementDot > bestReplacementDot)
                {
                    bestReplacement = possibleReplacements[i];
                    bestReplacementDot = thisReplacementDot;
                }
            }
            return bestReplacement;
        }
        

        
    }
}
