using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor
{
    public static class Helper
    {
        public static readonly Random r = new Random();
        public static Color RandomColor()
        {
            return Color.FromArgb(255, r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }
    }
}
