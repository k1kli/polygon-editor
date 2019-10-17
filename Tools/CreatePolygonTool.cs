using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class CreatePolygonTool : Tool
    {
        static readonly string[] help = {
            "Tworzenie wielokąta.\n\nZaznacz pierwszy punkt wielokąta.",
            "Tworzenie wielokąta.\n\nZaznacz więcej punktów wielokąta.",
            "Tworzenie wielokąta.\n\nZaznacz więcej punktów wielokąta. Aby zakończyć rysowanie wielokąta kliknij punkt początkowy",
        };
        public CreatePolygonTool(EditorForm editorForm) : base(editorForm)
        {
            editorForm.Help(help[0]);
            currentPolygonColor = Helper.RandomColor();
        }
        private List<Point> points = new List<Point>();
        Color currentPolygonColor;
        int curX, curY;
        const int tolerance = 10;
        bool withinToleranceForClosing = false;
        public override void MouseDown(int xPos, int yPos)
        {
            if (withinToleranceForClosing)
            {
                editorForm.Polygons.Add(new Figures.Polygon(points.ToArray(), currentPolygonColor));
                points.Clear();
                withinToleranceForClosing = false;
                currentPolygonColor = Helper.RandomColor();
                editorForm.Redraw();
            }
            else
            {
                points.Add(new Point(xPos, yPos));
            }
            switch(points.Count)
            {
                case 0:
                    editorForm.Help(help[0]);
                    break;
                case 1: case 2:
                    editorForm.Help(help[1]);
                    break;
                default:
                    editorForm.Help(help[2]);
                    break;
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
        }

        public override void MouseUp(int xPos, int yPos)
        {
        }
        public override void MouseMove(int xPos, int yPos)
        {
            curX = xPos;
            curY = yPos;
            withinToleranceForClosing = points.Count > 2 && Math.Abs(xPos - points[0].X) < tolerance && Math.Abs(yPos - points[0].Y) < tolerance;
            editorForm.Redraw();
        }
        public override void OnDrawGizmos()
        {
            if(withinToleranceForClosing)
            {
                curX = points[0].X;
                curY = points[0].Y;

            }
            for(int i = 0; i < points.Count; i++)
            {
                editorForm.MemoryBitmap.DrawPoint(points[i].X, points[i].Y, currentPolygonColor,Helper.pointSize);
                if (i != points.Count - 1) editorForm.MemoryBitmap.DrawLine(points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y, currentPolygonColor);
                else editorForm.MemoryBitmap.DrawLine(points[i].X, points[i].Y, curX, curY, currentPolygonColor);
            }
            editorForm.MemoryBitmap.DrawPoint(curX, curY, withinToleranceForClosing ? Color.Black : currentPolygonColor, Helper.pointSize);
        }
    }
}
