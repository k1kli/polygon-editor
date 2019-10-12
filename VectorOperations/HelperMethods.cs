using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.VectorOperations
{
    public static class HelperMethods
    {
        public static bool CanFormTriangle(Vector v1, Vector v2, Vector v3)
        {
            return (v1 - v2).Magnitude < (v2 - v3).Magnitude + (v3 - v1).Magnitude &&
                   (v3 - v1).Magnitude < (v2 - v3).Magnitude + (v1 - v2).Magnitude &&
                   (v2 - v3).Magnitude < (v3 - v1).Magnitude + (v1 - v2).Magnitude;
        }
    }
}
