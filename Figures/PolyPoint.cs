using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Figures
{
    public class PolyPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Polygon Parent { get; }

        public void FromVector(VectorOperations.Vector v)
        {
            X = v.X;
            Y = v.Y;
        }

        public PolyPoint Next => NextEdge.Next;
        public PolyPoint Previous => PreviousEdge.Previous;
        public Edge NextEdge { get; set; }
        public Edge PreviousEdge { get; set; }
        public PolyPoint(float x, float y, Polygon parent)
        {
            X = x;
            Y = y;
            Parent = parent;
        }

        public bool Remove()
        {
            return Parent.Remove(this);
        }
        public void MoveWithRestrictions(VectorOperations.Vector delta)
        {
            Parent.MoveVertexWithRestrictions(this, delta);
        }

        public VectorOperations.Vector GetVector() => new VectorOperations.Vector() { X = X, Y = Y };
    }
}
