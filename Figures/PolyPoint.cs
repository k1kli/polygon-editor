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
        public int X { get; set; }
        public int Y { get; set; }
        public Polygon Parent { get; }

        public PolyPoint Next { get; set; }
        public PolyPoint Previous { get; set; }
        public PolyPoint(int x, int y, Polygon parent)
        {
            X = x;
            Y = y;
            Parent = parent;
        }

        public bool Remove()
        {
            return Parent.Remove(this);
        }
    }
}
