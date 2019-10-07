using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class VertexInMiddleTool : Tool
    {
        static readonly string help =
            "Dodawanie wierzchołka na środku krawędzi.\nNaciśnij na krawędź, na środku której chcesz, aby utworzony został wierzchołek.";
        public VertexInMiddleTool(EditorForm form) : base(form) { editorForm.Help(help); }
        public override void MouseDown(int xPos, int yPos)
        {
            var edge = editorForm.SelectEdge(xPos, yPos);
            if(edge.HasValue)
            {
                int x = (edge.Value.p1.X + edge.Value.p2.X)/2;
                int y = (edge.Value.p1.Y + edge.Value.p2.Y) / 2;
                edge.Value.p1.Parent.AddPointBetween(x,y,edge.Value.p1, edge.Value.p2);
                editorForm.Redraw();
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
        }

        public override void MouseUp(int xPos, int yPos)
        {
        }
    }
}
