using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Tools
{
    public abstract class Tool
    {
        public abstract void MouseDown(int xPos, int yPos);
        public abstract void MouseUp(int xPos, int yPos);
        public abstract void MouseDrag(int xPos, int yPos);
        protected EditorForm editorForm;
        public Tool(EditorForm editorForm)
        {
            this.editorForm = editorForm;
        }
    }
}
