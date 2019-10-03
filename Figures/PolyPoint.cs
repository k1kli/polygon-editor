using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Figures
{
    public struct PolyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static implicit operator PolyPoint((int x, int y) v)
        {
            return new PolyPoint { X = v.x, Y = v.y };
        }

    }
}
