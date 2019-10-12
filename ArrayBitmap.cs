using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor
{
    public class ArrayBitmap
    {
        public int XSize { get; }
        public int YSize { get; }
        private int[] bitmap;
        public ArrayBitmap(int xSize, int ySize)
        {
            bitmap = new int[xSize*ySize];
            XSize = xSize;
            YSize = ySize;
        }
        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.ToArgb());
        }
        public void SetPixel(int x, int y, int argb)
        {
            if (x < 0 || x >= XSize || y < 0 || y >= YSize) return;
            bitmap[x + y * XSize] = argb;
        }
        public void Clear()
        {
            int white = Color.White.ToArgb();
            for (int i = 0; i < bitmap.Length; i++) bitmap[i] = white;
        }
        //https://docs.microsoft.com/pl-pl/dotnet/api/system.drawing.bitmap.lockbits?view=netframework-4.8
        public void FillBitmap(Bitmap toFill)
        {
            Rectangle rect = new Rectangle(0, 0, toFill.Width, toFill.Height);
            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Bitmap bmp = toFill.Clone(rect, pxf);

            BitmapData bmpData = toFill.LockBits(rect, ImageLockMode.ReadWrite,toFill.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(bitmap, 0, ptr, bitmap.Length);
            toFill.UnlockBits(bmpData);
        }

        public void DrawLine(Figures.PolyPoint p1, Figures.PolyPoint p2, Color color)
        {
            int argb = color.ToArgb();
            if (Math.Abs(p2.Y - p1.Y) < Math.Abs(p2.X - p1.X))
                if (p1.X > p2.X)
                    PlotLineLow((int)p2.X, (int)p2.Y, (int)p1.X, (int)p1.Y, argb);
                else
                    PlotLineLow((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, argb);
            else
                if (p1.Y > p2.Y)
                    PlotLineHigh((int)p2.X, (int)p2.Y, (int)p1.X, (int)p1.Y, argb);
                else
                    PlotLineHigh((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, argb);
        }

        private void PlotLineHigh(int x0, int y0, int x1, int y1, int argb)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            int D = 2 * dx - dy;
            int x = x0;

            for(int y = y0; y <= y1; y++)
            {
                SetPixel(x, y, argb);
                if(D>0)
                {
                    x = x + xi;
                    D = D - 2 * dy;
                }
                D = D + 2 * dx;
            }
        }
        private void PlotLineLow(int x0, int y0, int x1, int y1, int argb)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            int D = 2 * dy - dx;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                SetPixel(x, y, argb);
                if (D > 0)
                {
                    y = y + yi;
                    D = D - 2 * dx;
                }
                D = D + 2 * dy;
            }
        }
    }
}
