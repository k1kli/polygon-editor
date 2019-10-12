using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class RestrictionSameSizeTool : Tool
    {
        Figures.Edge firstEdge = null;
        public RestrictionSameSizeTool(EditorForm form) : base(form) { }
        public override void MouseDown(int xPos, int yPos)
        {
            if(firstEdge == null)
            {
                firstEdge = editorForm.SelectEdge(xPos, yPos);
            }
            else
            {
                Figures.Edge secondEdge = editorForm.SelectEdge(xPos, yPos);
                if (secondEdge is null || secondEdge == firstEdge) return;
                if (firstEdge.RelatedEdge == secondEdge && firstEdge.EnactedRestriction == Figures.Edge.Restriction.SameSize)
                    firstEdge.ClearRestriction();
                else
                    Figures.Edge.SetRestriction(Figures.Edge.Restriction.SameSize, firstEdge, secondEdge, Helper.RandomColor().ToArgb());
                editorForm.Redraw();
                firstEdge = null;
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
