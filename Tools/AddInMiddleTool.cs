using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    public class AddInMiddleTool : Tool
    {
        public AddInMiddleTool(EditorForm editorForm) : base(editorForm) { }
        public override void MouseDown(int xPos, int yPos)
        {
            editorForm.SelectEdge(xPos, yPos);
            if(editorForm.SelectedPolygon != null)
            {
                var edge = editorForm.SelectedPolygon.SelectedEdge;

            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
            throw new NotImplementedException();
        }

        public override void MouseUp(int xPos, int yPos)
        {
            throw new NotImplementedException();
        }
    }
}
