using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Figures;

namespace PolygonEditor
{
    public static class Helper
    {
        public static readonly Random r = new Random();
        public const int pointSize = 10;
        public const int restrictionBoxSize = 20;
        public const int padding = 3;
        public static Color RandomColor()
        {
            return Color.FromArgb(255, r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }
        private static Font f = new Font("Verdana", restrictionBoxSize - padding*2, GraphicsUnit.Pixel);
        public static void DrawRestrictionLabel(Figures.Edge e, MemoryBitmap bitmap,
            Color? color = null, Figures.Edge.Restriction? restriction = null)
        {

            int restrictionBoxSizeDiv2 = Helper.restrictionBoxSize / 2;
            Point point = new Point(((int)e.Previous.X + (int)e.Next.X) / 2, ((int)e.Previous.Y + (int)e.Next.Y) / 2);
            int startX = point.X - restrictionBoxSize, endX = point.X + restrictionBoxSize;
            int midX = (endX + startX) / 2;
            int startY = point.Y - restrictionBoxSizeDiv2, endY = point.Y + restrictionBoxSizeDiv2;
            for (int x = startX; x <= midX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, color.HasValue ? color.Value.ToArgb() : e.RestrictionData);
                }
            for (int x = midX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, color.HasValue ? color.Value.ToArgb() : e.RestrictionData);
                }
            for (int x = startX + 1; x <= midX - 1; x++)
                for (int y = startY + 1; y <= endY - 1; y++)
                {
                    bitmap.SetPixel(x, y, Color.White);
                }
            for (int x = startX + 1; x <= endX - 1; x++)
                for (int y = startY + 1; y <= endY - 1; y++)
                {
                    bitmap.SetPixel(x, y, Color.White);
                }
            Graphics g = bitmap.GetGraphics();
            g.DrawString(restriction.HasValue ? "X" : e.RestrictionNum.ToString(), f, Brushes.Black, midX + padding, startY + padding);
            if (!restriction.HasValue) restriction = e.EnactedRestriction;
            DrawRestrictionIcon(bitmap, restriction.Value, startX + padding, startY + padding, midX - padding, endY - padding);
        }

        public static int GetQuadraticZeros(float a, float b, float c, float[] results)
        {
            float determinant = b * b - 4 * a * c;
            if(determinant < 0)
            {
                return 0;
            }
            if(Math.Abs(determinant) < 0.0001f)
            {
                results[0] = -b / (2 * a);
                return 1;
            }
            determinant = (float)Math.Sqrt(determinant);
            results[0] = (-b - determinant) / (2 * a);
            results[1] = (-b + determinant) / (2 * a);
            return 2;

        }

        private static void DrawRestrictionIcon(MemoryBitmap bitmap, Edge.Restriction restriction, int startX, int startY, int endX, int endY)
        {
            switch (restriction)
            {
                case Edge.Restriction.SameSize:
                    DrawRestrictionSameSizeIcon(bitmap, startX, startY, endX, endY);
                    break;
                case Edge.Restriction.Perpendicular:
                    DrawRestrictionPerpendicularIcon(bitmap, startX, startY, endX, endY);
                    break;
            }
        }

        private static void DrawRestrictionPerpendicularIcon(MemoryBitmap bitmap, int startX, int startY, int endX, int endY)
        {
            startX = (startX + endX) / 2 - 1;
            endX = startX + 2;
            startY = (startY + endY) / 2 - 1;
            endY = startY + 2;

            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    bitmap.SetPixel(x, y, Color.Black);
                }
        }

        private static void DrawRestrictionSameSizeIcon(MemoryBitmap bitmap, int startX, int startY, int endX, int endY)
        {
            startX = (startX + endX) / 2 - 3;
            endX = startX + 6;
            startY++;
            endY--;

            for (int y = startY; y <= endY; y++)
            {
                bitmap.SetPixel(startX, y, Color.Black);
                bitmap.SetPixel(endX, y, Color.Black);
            }
        }
    }
}
