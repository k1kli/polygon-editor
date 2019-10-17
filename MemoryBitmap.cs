using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor
{
    //https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
    public class MemoryBitmap
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Width { get; }
        public int Height { get; }
        protected GCHandle BitsHandle { get; private set; }
        private Graphics g;
        public MemoryBitmap(int xSize, int ySize)
        {
            Width = xSize;
            Height = ySize;
            Bits = new Int32[Width * Height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }
        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.ToArgb());
        }
        public void SetPixel(int x, int y, int argb)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            RawSetPixel(x, y, argb);
        }
        private void RawSetPixel(int x, int y, int argb)
        {
            int index = x + (y * Width);
            int col = argb;

            Bits[index] = col;
        }
        public Color GetPixel(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) throw new IndexOutOfRangeException();
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }
        public void Clear()
        {
            int white = Color.White.ToArgb();
            for (int i = 0; i < Bits.Length; i++) Bits[i] = white;
        }
        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
        public void CreateGraphics()
        {
            g = Graphics.FromImage(Bitmap);
        }
        public void DisposeGraphics()
        {
            g.Dispose();
        }
        public Graphics GetGraphics()
        {
            return g;
        }

        public void DrawPoint(int posX, int posY, Color color, int size)
        {
            int sizeDiv2 = size / 2;
            int c = color.ToArgb();
            int startX = Math.Max((int)posX - sizeDiv2, 0), endX = Math.Min((int)posX + sizeDiv2, Width - 1);
            int startY = Math.Max((int)posY - sizeDiv2, 0), endY = Math.Min((int)posY + sizeDiv2, Height - 1);
            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    RawSetPixel(x, y, c);
                }
        }
        public void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            int argb = color.ToArgb();
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
                if (x0 > x1)
                    PlotLineLow(x1, y1, x0, y0, argb);
                else
                    PlotLineLow(x0, y0, x1, y1, argb);
            else
                if (y0 > y1)
                PlotLineHigh(x1, y1, x0, y0, argb);
            else
                PlotLineHigh(x0, y0, x1, y1, argb);
        }

        public void DrawLine(Figures.PolyPoint p1, Figures.PolyPoint p2, Color color)
        {
            DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, color);
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

            for (int y = y0; y <= y1; y++)
            {
                SetPixel(x, y, argb);
                if (D > 0)
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
