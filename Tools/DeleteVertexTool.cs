using PolygonEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class DeleteVertexTool : Tool
    {
        static readonly string help =
            "Usuwanie wierzchołka.\nNaciśnij na wierzchołek który chcesz usunąć.\nWielokąt musi mieć co najmniej 3 wierzchołki.";
        public DeleteVertexTool(EditorForm form) : base(form) { editorForm.Help(help); }
        public override void MouseDown(int xPos, int yPos)
        {
            PolyPoint point = editorForm.SelectPoint(xPos, yPos);
            if(point != null)
            {
                if(!point.Remove())
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Wielokąt musi składać się z co najmniej 3 wierzchołków",
                        "Nie udało się usunąć punktu",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    editorForm.Redraw();
                }
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
