using PolygonEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    public class MovePointTool : Tool
    {
        static readonly string help =
            "Przesuwanie wierzchołka.\nNaciśnij i przytrzymaj lewy przycisk myszy nad wybranym wierzchołkiem i przeciągnij go" +
                " w wybrane miejsce.";
        public MovePointTool(EditorForm editorForm) : base(editorForm) { editorForm.Help(help); }
        private PolyPoint point;
        private int curX;
        private int curY;
        public override void MouseDown(int xPos, int yPos)
        {
            PolyPoint p = editorForm.SelectPoint(xPos, yPos);
            point = p;
            if (p != null)
            {
                curX = xPos;
                curY = yPos;
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
            if (point != null)
            {
                point.X += xPos - curX;
                point.Y += yPos - curY;
                curX = xPos;
                curY = yPos;
                editorForm.Redraw();
            }
        }

        public override void MouseUp(int xPos, int yPos)
        {
            point = null;
        }
    }
}
