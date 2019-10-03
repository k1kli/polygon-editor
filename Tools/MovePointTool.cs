using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    public class MovePointTool : Tool
    {
        public MovePointTool(EditorForm editorForm) : base(editorForm) { }
        private int pointX = Int32.MaxValue;
        private int pointY = Int32.MaxValue;
        private int curX;
        private int curY;
        public override void MouseDown(int xPos, int yPos)
        {
            editorForm.SelectPoint(xPos, yPos);
            if (editorForm.SelectedPolygon != null)
            {
                pointX = editorForm.SelectedPolygon.SelectedPoint.Value.X;
                pointY = editorForm.SelectedPolygon.SelectedPoint.Value.Y;
                curX = xPos;
                curY = yPos;
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
            if (pointX != Int32.MaxValue)
            {
                editorForm.SelectedPolygon.MoveSelectedPoint(pointX + xPos - curX, pointY + yPos - curY);
                pointX = editorForm.SelectedPolygon.SelectedPoint.Value.X;
                pointY = editorForm.SelectedPolygon.SelectedPoint.Value.Y;
                curX = xPos;
                curY = yPos;
                editorForm.Redraw();
            }
        }

        public override void MouseUp(int xPos, int yPos)
        {
            pointX = Int32.MaxValue;
            pointY = Int32.MaxValue;
        }
    }
}
