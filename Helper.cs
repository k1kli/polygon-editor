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
        public const int pointSize = 10;
        public static Color RandomColor()
        {
            return Color.FromArgb(255, r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }
        public static void DrawRestrictionSameSize(Figures.Edge e, MemoryBitmap bitmap, Color? color = null)
        {

            int pointSizeDiv2 = Helper.pointSize / 2;
            Point point = new Point(((int)e.Previous.X + (int)e.Next.X) / 2, ((int)e.Previous.Y + (int)e.Next.Y) / 2);
            int startX = point.X - pointSizeDiv2, endX = point.X + pointSizeDiv2;
            int startY = point.Y - pointSizeDiv2, endY = point.Y + pointSizeDiv2;
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, color.HasValue ? color.Value.ToArgb(): e.RestrictionData);
                }
            int padding = 3;
            for (int x = startX + padding; x <= endX - padding; x++)
                for (int y = startY + padding; y <= endY - padding; y++)
                {
                    bitmap.SetPixel(x, y, Color.White);
                }
        }
    }
}
