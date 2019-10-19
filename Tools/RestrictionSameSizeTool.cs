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
        Figures.Edge firstEdge = null;
        Figures.Edge highlightedEdge = null;
        static readonly string[] help = {
            "Dodawanie relacji równej długości." +
                "\n\nZaznacz pierwszą z krawędzi która ma być w relacji." +
                "\nDruga krawędź dostosuje do niej swoją długość" +
                "\n\nAby usunąć relację zaznacz krawędzie będące w relacji równej długości",
            "Dodawanie relacji równej długości." +
                "\n\nZaznacz drugą z krawędzi która ma być w relacji." +
                "\nDostosuje ona swoją długość do pierwszej krawędzi" +
                "\n\nAby usunąć relację zaznacz krawędzie będące w relacji równej długości"
        };
        public RestrictionSameSizeTool(EditorForm form) : base(form)
        {
            currentRestrictionColor = Helper.RandomColor();
            editorForm.Help(help[0]);
        }
        Color currentRestrictionColor;
        public override void MouseDown(int xPos, int yPos)
        {
            if(firstEdge == null && highlightedEdge != null)
            {
                firstEdge = highlightedEdge;
                editorForm.Help(help[1]);
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
                editorForm.Help(help[0]);
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
                Helper.DrawRestrictionLabel(firstEdge, editorForm.MemoryBitmap,
                    currentRestrictionColor, Figures.Edge.Restriction.SameSize);
            }
            if(highlightedEdge != null)
                Helper.DrawRestrictionLabel(highlightedEdge, editorForm.MemoryBitmap,
                    currentRestrictionColor, Figures.Edge.Restriction.SameSize);
        }
    }
}
