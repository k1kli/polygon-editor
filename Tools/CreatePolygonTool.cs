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
            "Tworzenie wielokąta.\nZaznacz 3 pozycje, które będą wierzchołkami nowo powstałego wielokąta.",
            "Tworzenie wielokąta.\nZaznacz jeszcze 2 pozycje.",
            "Tworzenie wielokąta.\nZaznacz jeszcze jedną pozycję.",
        };
        public CreatePolygonTool(EditorForm editorForm) : base(editorForm)
        {
            editorForm.Help(help[0]);
        }
        private Point[] points = new Point[3];
        int pointIndex = 0;
        public override void MouseDown(int xPos, int yPos)
        {
            points[pointIndex++] = new Point(xPos, yPos);
            if (pointIndex == 3)
            {
                editorForm.Polygons.Add(new Figures.Polygon(points, Helper.RandomColor()));
                pointIndex = 0;
                editorForm.Redraw();
            }
            editorForm.Help(help[pointIndex]);
        }

        public override void MouseDrag(int xPos, int yPos)
        {
        }

        public override void MouseUp(int xPos, int yPos)
        {
        }
    }
}
