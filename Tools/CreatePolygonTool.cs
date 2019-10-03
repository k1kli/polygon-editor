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
        public CreatePolygonTool(EditorForm editorForm) : base(editorForm)
        {

        }
        private Figures.PolyPoint[] points = new Figures.PolyPoint[3];
        int pointIndex = 0;
        readonly static Random r = new Random();
        public override void MouseDown(int xPos, int yPos)
        {
            points[pointIndex++] = (xPos, yPos);
            if(pointIndex == 3)
            {
                editorForm.Polygons.Add(new Figures.Polygon(points[0], points[1], points[2], RandomColor()));
                pointIndex = 0;
                editorForm.Redraw();
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
        }

        public override void MouseUp(int xPos, int yPos)
        {
        }
        private Color RandomColor()
        {
            return Color.FromArgb(255, r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
        }
    }
}
