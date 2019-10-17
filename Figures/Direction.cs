using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Figures
{

    public enum Direction
    {
        Forward, Backwards
    }
    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction d)
        {
            return 1 - d;
        }
    }
}
