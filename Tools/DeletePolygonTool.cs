using PolygonEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class DeletePolygonTool : Tool
    {
        static readonly string help =
            "Usuwanie wielokąta.\n\nNaciśnij dowolną krawędź wybranego wielokąta aby go usunąć";
        public DeletePolygonTool(EditorForm form) : base(form) { editorForm.Help(help); }
        Polygon selectedPolygon;
        public override void MouseDown(int xPos, int yPos)
        {
            selectedPolygon = editorForm.SelectPolygon(xPos, yPos);
            if (selectedPolygon != null)
            {
                editorForm.DeletePolygon(selectedPolygon);
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
