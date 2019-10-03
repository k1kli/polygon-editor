using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonEditor
{
    public partial class EditorForm : Form
    {
        public ArrayBitmap ArrayBitmap { get; }
        private Bitmap FilledBitmap;
        private bool mouseDown;
        private Tools.Tool currentTool;
        public List<Figures.Polygon> Polygons { get; } = new List<Figures.Polygon>();
        public Figures.Polygon SelectedPolygon { get; private set; }
        private const int distanceLimit = 15;
        public EditorForm()
        {
            InitializeComponent();
            ArrayBitmap = new ArrayBitmap(canvasPictureBox.Width, canvasPictureBox.Height);
            FilledBitmap = new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height);

            Polygons.Add(new Figures.Polygon((20, 30), (55, 100), (150, 70), Color.ForestGreen));
            Redraw();
            currentTool = new Tools.MovePointTool(this);
        }

        public void Redraw()
        {
            ArrayBitmap.Clear();
            foreach(Figures.Polygon polygon in Polygons)
            {
                polygon.Draw(ArrayBitmap);
            }
            canvasPictureBox.Invalidate();
        }

        public void SelectPoint(int x, int y)
        {
            int minDist = Int32.MaxValue;
            SelectedPolygon = null;
            foreach(Figures.Polygon polygon in Polygons)
            {
                int dist = polygon.TrySelectPoint(x, y, distanceLimit);
                if(dist != -1 && dist < minDist)
                {
                    dist = minDist;
                    SelectedPolygon = polygon;
                }

            }
        }

        public void SelectEdge(int x, int y)
        {
            int minDist = Int32.MaxValue;
            SelectedPolygon = null;
            foreach (Figures.Polygon polygon in Polygons)
            {
                int dist = polygon.TrySelectEdge(x, y, distanceLimit);
                if (dist != -1 && dist < minDist)
                {
                    dist = minDist;
                    SelectedPolygon = polygon;
                }

            }
        }

        private void CanvasPictureBox_Paint(object sender, PaintEventArgs e)
        {
            ArrayBitmap.FillBitmap(FilledBitmap);
            e.Graphics.DrawImageUnscaled(FilledBitmap, 0, 0);
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
    }
}
