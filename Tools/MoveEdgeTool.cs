using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonEditor.Figures;

namespace PolygonEditor.Tools
{
    public class MoveEdgeTool : Tool
    {
        static readonly string help =
            "Przesuwanie krawędzi.\nNaciśnij i przytrzymaj lewy przycisk myszy nad wybraną krawędzią i przeciągnij ją" +
                " w wybrane miejsce.";
        public MoveEdgeTool(EditorForm editorForm) : base(editorForm) { editorForm.Help(help); }
        (PolyPoint p1, PolyPoint p2)? selectedEdge;
        int curX, curY;
        public override void MouseDown(int xPos, int yPos)
        {
            selectedEdge = editorForm.SelectEdge(xPos, yPos);
            if(selectedEdge != null)
            {
                curX = xPos;
                curY = yPos;
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
            if (selectedEdge.HasValue)
            {
                selectedEdge.Value.p1.X += xPos - curX;
                selectedEdge.Value.p2.X += xPos - curX;
                selectedEdge.Value.p1.Y += yPos - curY;
                selectedEdge.Value.p2.Y += yPos - curY;
                curX = xPos;
                curY = yPos;
                editorForm.Redraw();
            }
        }

        public override void MouseUp(int xPos, int yPos)
        {
            selectedEdge = null;
        }
    }
}
