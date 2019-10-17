using PolygonEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class MovePolygonTool : Tool
    {
        static readonly string help =
            "Przesuwanie wielokąta.\n\nNaciśnij i przytrzymaj lewy przycisk myszy nad dowolną krawędzią wybranego wielokąta i przeciągnij ją" +
                " w wybrane miejsce.";
        public MovePolygonTool(EditorForm form) : base(form) { editorForm.Help(help); }
        Polygon selectedPolygon;
        int curX, curY;
        public override void MouseDown(int xPos, int yPos)
        {
            selectedPolygon = editorForm.SelectPolygon(xPos, yPos);
            if (selectedPolygon != null)
            {
                curX = xPos;
                curY = yPos;
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
            if (selectedPolygon != null)
            {
                PolyPoint point = selectedPolygon.First;
                do
                {
                    point.X += xPos - curX;
                    point.Y += yPos - curY;
                    point = point.Next;
                } while (point != selectedPolygon.First);
                curX = xPos;
                curY = yPos;
                editorForm.Redraw();
            }
        }

        public override void MouseUp(int xPos, int yPos)
        {
            selectedPolygon = null;
        }
    }
}
