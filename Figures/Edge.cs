using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Figures
{
    public class Edge
    {
        public Polygon parent { get; }
        public PolyPoint Next { get; set; }
        public PolyPoint Previous { get; set; }
        public Edge PreviousEdge => Previous.PreviousEdge;
        public Edge NextEdge => Next.NextEdge;
        public Restriction EnactedRestriction { get; private set; }
        public Edge RelatedEdge { get; private set; }
        public int RestrictionData { get; set; }

        private Edge(Polygon p)
        {
            parent = p;
        }

        public float GetLength()
        {
            return (Previous.GetVector() - Next.GetVector()).Magnitude;
        }

        public void MoveWithRestrictions(VectorOperations.Vector delta)
        {
            parent.MoveEdgeWithRestrictions(this, delta);
        }


        public static void SetRestriction(Restriction newRestriction, Edge e1, Edge e2, int restrictionData = 0)
        {
            if (newRestriction == Restriction.None) throw new Exception("To clear restrictions use Clear method instead");
            if (e1.EnactedRestriction != Restriction.None) e1.ClearRestriction();
            if (e2.EnactedRestriction != Restriction.None) e2.ClearRestriction();
            e1.EnactedRestriction = newRestriction;
            e2.EnactedRestriction = newRestriction;
            e1.RelatedEdge = e2;
            e2.RelatedEdge = e1;
            e1.parent.EnactRestriction(e2, true);
            e1.RestrictionData = e2.RestrictionData = restrictionData;
        }
        public void ClearRestriction()
        {
            if (EnactedRestriction == Restriction.None) return;
            parent.ClearRestriction(this);
            RelatedEdge.EnactedRestriction = EnactedRestriction = Restriction.None;
            RelatedEdge.RelatedEdge = null;
            RelatedEdge = null;
        }
        public PolyPoint GetOpposite(PolyPoint p)
        {
            if (p == Next) return Previous;
            if (p == Previous) return Next;
            throw new Exception("Edge doesn't contain this point");
        }

        public static Edge Link(PolyPoint p1, PolyPoint p2)
        {
            if (p1.Parent != p2.Parent) throw new Exception("Cannot link points in different polygons");
            Edge e = new Edge(p1.Parent);
            e.Previous = p1;
            e.Next = p2;
            p1.NextEdge = p2.PreviousEdge = e;
            return e;
        }
        public static Edge Join(Edge e1, Edge e2)
        {
            if (e1.Next.NextEdge != e2)
            {
                throw new Exception("E1 and E2 should be next to each other");
            }
            return Link(e1.Previous, e2.Next);
        }
        public static (Edge e1, Edge e2) Split(Edge e, PolyPoint splittingPoint)
        {
            return (Link(e.Previous, splittingPoint), Link(splittingPoint, e.Next));
        }
        public enum Restriction
        {
            None, SameSize, Perpendicular
        }
    }
}
