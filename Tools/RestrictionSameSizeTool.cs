using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    class RestrictionSameSizeTool : Tool
    {
        Figures.Edge firstEdge = null, secondEdge = null;
        Figures.Edge highlightedEdge = null;
        public RestrictionSameSizeTool(EditorForm form) : base(form)
        {
            currentRestrictionColor = Helper.RandomColor();
        }
        int curX, curY;
        Color currentRestrictionColor;
        public override void MouseDown(int xPos, int yPos)
        {
            if(firstEdge == null && highlightedEdge != null)
            {
                firstEdge = highlightedEdge;
            }
            else
            {
                Figures.Edge secondEdge = highlightedEdge;
                if (secondEdge is null || secondEdge == firstEdge) return;
                if (firstEdge.RelatedEdge == secondEdge && firstEdge.EnactedRestriction == Figures.Edge.Restriction.SameSize)
                    firstEdge.ClearRestriction();
                else
                    Figures.Edge.SetRestriction(Figures.Edge.Restriction.SameSize, firstEdge, secondEdge, currentRestrictionColor.ToArgb());
                editorForm.Redraw();
                firstEdge = null;
                currentRestrictionColor = Helper.RandomColor();
            }
        }

        public override void MouseDrag(int xPos, int yPos)
        {
        }

        public override void MouseUp(int xPos, int yPos)
        {
        }
        public override void MouseMove(int xPos, int yPos)
        {
            curX = xPos;
            curY = yPos;
            Figures.Edge newHighlightedEdge = editorForm.SelectEdge(xPos, yPos);
            if (newHighlightedEdge == highlightedEdge) return;
            if (firstEdge != null && newHighlightedEdge != null && newHighlightedEdge.parent != firstEdge.parent) return;
            highlightedEdge = newHighlightedEdge;
            editorForm.Redraw();
        }

        public override void OnDrawGizmos()
        {
            if(firstEdge != null)
            {
                Helper.DrawRestrictionSameSize(firstEdge, editorForm.MemoryBitmap, currentRestrictionColor);
            }
            if(highlightedEdge != null)
                Helper.DrawRestrictionSameSize(highlightedEdge, editorForm.MemoryBitmap, currentRestrictionColor);
        }
    }
}
