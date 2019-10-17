using PolygonEditor.Figures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonEditor
{
    public partial class EditorForm : Form
    {
        public MemoryBitmap MemoryBitmap { get; }
        private bool mouseDown;
        private Tools.Tool currentTool;
        private Thread clearThread;
        private ThreadStart clearThreadStart;
        public List<Figures.Polygon> Polygons { get; } = new List<Figures.Polygon>();
        private const int distanceLimit = 30;
        public EditorForm()
        {
            InitializeComponent();
            MemoryBitmap = new MemoryBitmap(canvasPictureBox.Width, canvasPictureBox.Height);

            Polygons.Add(new Figures.Polygon(new Point[]{ new Point(20, 30), new Point(55, 100), new Point(150, 70) }, Color.ForestGreen));
            currentTool = new Tools.MovePointTool(this);
            clearThreadStart = new ThreadStart(MemoryBitmap.Clear);
            clearThread = new Thread(clearThreadStart);
            clearThread.Start();

            Redraw();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            MemoryBitmap.Dispose();
        }

        public void Redraw()
        {
            canvasPictureBox.Invalidate();
        }

        public Figures.PolyPoint SelectPoint(int x, int y)
        {
            int minDist = Int32.MaxValue;
            Figures.PolyPoint res = null;
            foreach(Figures.Polygon polygon in Polygons)
            {
                var (dist, p) = polygon.TrySelectPoint(x, y, distanceLimit);
                if(p != null && dist < minDist)
                {
                    minDist = dist;
                    res = p;
                }
            }
            return res;
        }

        public Edge SelectEdge(int x, int y)
        {
            int minDist = Int32.MaxValue;
            Edge res = null;
            foreach (Figures.Polygon polygon in Polygons)
            {
                var (dist, edge) = polygon.TrySelectEdge(x, y, distanceLimit);
                if (edge != null && dist < minDist)
                {
                    dist = minDist;
                    res = edge;
                }
            }
            return res;
        }

        public Figures.Polygon SelectPolygon(int x, int y)
        {
            int minDist = Int32.MaxValue;
            Figures.Polygon res = null;
            foreach (Figures.Polygon polygon in Polygons)
            {
                var (dist, edge) = polygon.TrySelectEdge(x, y, distanceLimit);
                if (edge != null && dist < minDist)
                {
                    dist = minDist;
                    res = polygon;
                }
            }
            return res;
        }
        public void Help(string help)
        {
            helpLabel.Text = help;
        }

        private void CanvasPictureBox_Paint(object sender, PaintEventArgs e)
        {
            clearThread.Join();
            MemoryBitmap.CreateGraphics();
            foreach (Figures.Polygon polygon in Polygons)
            {
                polygon.Draw(MemoryBitmap);
            }
            currentTool.OnDrawGizmos();
            Bitmap b = MemoryBitmap.Bitmap;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            MemoryBitmap.DisposeGraphics();
            clearThread = new Thread(clearThreadStart);
            clearThread.Start();
        }

        private void CanvasPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            currentTool.MouseDown(e.X, e.Y);
        }

        private void CanvasPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseDown)
            {
                currentTool.MouseDrag(e.X, e.Y);
            }
            currentTool.MouseMove(e.X, e.Y);
        }

        private void CanvasPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            currentTool.MouseUp(e.X, e.Y);
        }

        private void CreatePolygonRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (createPolygonRadioButton.Checked)
            {
                currentTool = new Tools.CreatePolygonTool(this);
            }
        }

        private void MoveVertexRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(moveVertexRadioButton.Checked)
            {
                currentTool = new Tools.MovePointTool(this);
            }
        }

        private void MoveEdgeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (moveEdgeRadioButton.Checked)
            {
                currentTool = new Tools.MoveEdgeTool(this);
            }
        }

        private void AddInMiddleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AddInMiddleRadioButton.Checked)
            {
                currentTool = new Tools.VertexInMiddleTool(this);
            }
        }

        private void MovePolygonRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(movePolygonRadioButton.Checked)
            {
                currentTool = new Tools.MovePolygonTool(this);
            }
        }

        private void DeleteVertexRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (deleteVertexRadioButton.Checked)
            {
                currentTool = new Tools.DeleteVertexTool(this);
            }
        }

        private void RelationEqualRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (relationEqualRadioButton.Checked)
            {
                currentTool = new Tools.RestrictionSameSizeTool(this);
            }
        }


        private void RelationPerpendicularRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (relationPerpendicularRadioButton.Checked)
            {
                currentTool = new Tools.RestrictionPerpendicularTool(this);
            }
        }

        private void labelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelsToolStripMenuItem.Checked = !labelsToolStripMenuItem.Checked;
            Polygon.DrawLabels = labelsToolStripMenuItem.Checked;
            Redraw();
        }
    }
}
