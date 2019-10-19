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
            if (!LimitLineEnds(ref x0, ref y0, ref x1, ref y1)) return;
            int argb = color.ToArgb();
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
                if (x0 > x1)
                    PlotLineEW(x1, y1, x0, y0, argb);
                else
                    PlotLineEW(x0, y0, x1, y1, argb);
            else
                if (y0 > y1)
                PlotLineNS(x1, y1, x0, y0, argb);
            else
                PlotLineNS(x0, y0, x1, y1, argb);
        }

        public void DrawLine(Figures.PolyPoint p1, Figures.PolyPoint p2, Color color)
        {
            int x0 = (int)p1.X, y0 = (int)p1.Y, x1 = (int)p2.X, y1 = (int)p2.Y;
            DrawLine(x0, y0, x1, y1, color);
        }

        private class RefPoint
        {
            public int X;
            public int Y;
        }
        public bool LimitLineEnds(ref int x0, ref int y0, ref int x1, ref int y1)
        {
            RefPoint p0 = new RefPoint{ X=x0, Y=y0 };
            RefPoint p1 = new RefPoint { X = x1, Y = y1 };
            RefPoint smallerX = x0 < x1 ? p0 : p1;
            RefPoint smallerY = y0 < y1 ? p0 : p1;
            RefPoint biggerX = x0 >= x1 ? p0 : p1;
            RefPoint biggerY = y0 >= y1 ? p0 : p1;

            if (biggerX.X < 0 || biggerY.Y < 0 || smallerX.X > Width  || smallerY.Y > Height )
                return false;
            if(smallerX.X < 0)
            {
                smallerX.Y += (biggerX.Y - smallerX.Y) * (-smallerX.X) / (biggerX.X - smallerX.X);
                smallerX.X = 0;
            }
            if(biggerX.X > Width)
            {
                biggerX.Y -= (biggerX.Y - smallerX.Y) * (biggerX.X-Width) / (biggerX.X - smallerX.X);
                biggerX.X = Width;
            }
            if(smallerY.Y < 0)
            {
                smallerY.X += (biggerY.X - smallerY.X) * (-smallerY.Y) / (biggerY.Y - smallerY.Y);
                smallerY.Y = 0;
            }
            if(biggerY.Y > Height)
            {
                biggerY.X -= (biggerY.X - smallerY.X) * (biggerY.Y-Height) / (biggerY.Y - smallerY.Y);
                biggerY.Y = Height;
            }
            x0 = p0.X; y0 = p0.Y; x1 = p1.X; y1 = p1.Y;
            return true;
        }

        private void PlotLineNS(int x0, int y0, int x1, int y1, int argb)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int incrX = 1;
            if (dx < 0)
            {
                incrX = -1;
                dx = -dx;
            }
            int d = 2 * dx - dy;
            int incrS = 2 * dx;
            int incrSE = 2 * (dx - dy);
            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                if (d < 0)
                {
                    d += incrS;
                }
                else
                {
                    d += incrSE;
                    x += incrX;
                }
                SetPixel(x, y, argb);
            }
        }
        private void PlotLineEW(int x0, int y0, int x1, int y1, int argb)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int incrY = 1;
            if (dy < 0)
            {
                incrY = -1;
                dy = -dy;
            }
            int d = 2 * dy - dx;
            int incrE = 2 * dy;
            int incrNE = 2 * (dy - dx);
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                if(d<0)
                {
                    d += incrE;
                }
                else
                {
                    d += incrNE;
                    y += incrY;
                }
                SetPixel(x, y, argb);
            }
        }
    }
}
