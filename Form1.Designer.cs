namespace PolygonEditor
{
    partial class EditorForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolsGroupBox = new System.Windows.Forms.GroupBox();
            this.relationsToolsLabel = new System.Windows.Forms.Label();
            this.polygonToolsLabel = new System.Windows.Forms.Label();
            this.edgeToolsLabel = new System.Windows.Forms.Label();
            this.vertexToolsLabel = new System.Windows.Forms.Label();
            this.createPolygonRadioButton = new System.Windows.Forms.RadioButton();
            this.relationPerpendicularRadioButton = new System.Windows.Forms.RadioButton();
            this.relationEqualRadioButton = new System.Windows.Forms.RadioButton();
            this.movePolygonRadioButton = new System.Windows.Forms.RadioButton();
            this.moveEdgeRadioButton = new System.Windows.Forms.RadioButton();
            this.AddInMiddleRadioButton = new System.Windows.Forms.RadioButton();
            this.deleteVertexRadioButton = new System.Windows.Forms.RadioButton();
            this.moveVertexRadioButton = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.plikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpLabel = new System.Windows.Forms.Label();
            this.canvasPictureBox = new System.Windows.Forms.PictureBox();
            this.toolsGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolsGroupBox
            // 
            this.toolsGroupBox.Controls.Add(this.relationsToolsLabel);
            this.toolsGroupBox.Controls.Add(this.polygonToolsLabel);
            this.toolsGroupBox.Controls.Add(this.edgeToolsLabel);
            this.toolsGroupBox.Controls.Add(this.vertexToolsLabel);
            this.toolsGroupBox.Controls.Add(this.createPolygonRadioButton);
            this.toolsGroupBox.Controls.Add(this.relationPerpendicularRadioButton);
            this.toolsGroupBox.Controls.Add(this.relationEqualRadioButton);
            this.toolsGroupBox.Controls.Add(this.movePolygonRadioButton);
            this.toolsGroupBox.Controls.Add(this.moveEdgeRadioButton);
            this.toolsGroupBox.Controls.Add(this.AddInMiddleRadioButton);
            this.toolsGroupBox.Controls.Add(this.deleteVertexRadioButton);
            this.toolsGroupBox.Controls.Add(this.moveVertexRadioButton);
            this.toolsGroupBox.Location = new System.Drawing.Point(12, 31);
            this.toolsGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolsGroupBox.Name = "toolsGroupBox";
            this.toolsGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolsGroupBox.Size = new System.Drawing.Size(231, 383);
            this.toolsGroupBox.TabIndex = 1;
            this.toolsGroupBox.TabStop = false;
            this.toolsGroupBox.Text = "Narzędzia";
            // 
            // relationsToolsLabel
            // 
            this.relationsToolsLabel.Location = new System.Drawing.Point(3, 309);
            this.relationsToolsLabel.Name = "relationsToolsLabel";
            this.relationsToolsLabel.Size = new System.Drawing.Size(225, 17);
            this.relationsToolsLabel.TabIndex = 11;
            this.relationsToolsLabel.Text = "Relacje krawędzi";
            this.relationsToolsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // polygonToolsLabel
            // 
            this.polygonToolsLabel.Location = new System.Drawing.Point(3, 217);
            this.polygonToolsLabel.Name = "polygonToolsLabel";
            this.polygonToolsLabel.Size = new System.Drawing.Size(225, 17);
            this.polygonToolsLabel.TabIndex = 10;
            this.polygonToolsLabel.Text = "Edycja wielokąta";
            this.polygonToolsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // edgeToolsLabel
            // 
            this.edgeToolsLabel.Location = new System.Drawing.Point(3, 110);
            this.edgeToolsLabel.Name = "edgeToolsLabel";
            this.edgeToolsLabel.Size = new System.Drawing.Size(225, 17);
            this.edgeToolsLabel.TabIndex = 9;
            this.edgeToolsLabel.Text = "Edycja krawędzi";
            this.edgeToolsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // vertexToolsLabel
            // 
            this.vertexToolsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.vertexToolsLabel.Location = new System.Drawing.Point(3, 17);
            this.vertexToolsLabel.Name = "vertexToolsLabel";
            this.vertexToolsLabel.Size = new System.Drawing.Size(225, 17);
            this.vertexToolsLabel.TabIndex = 8;
            this.vertexToolsLabel.Text = "Edycja wierzchołka";
            this.vertexToolsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // createPolygonRadioButton
            // 
            this.createPolygonRadioButton.AutoSize = true;
            this.createPolygonRadioButton.Location = new System.Drawing.Point(5, 263);
            this.createPolygonRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.createPolygonRadioButton.Name = "createPolygonRadioButton";
            this.createPolygonRadioButton.Size = new System.Drawing.Size(126, 21);
            this.createPolygonRadioButton.TabIndex = 7;
            this.createPolygonRadioButton.Text = "Utwórz wielokąt";
            this.createPolygonRadioButton.UseVisualStyleBackColor = true;
            this.createPolygonRadioButton.CheckedChanged += new System.EventHandler(this.CreatePolygonRadioButton_CheckedChanged);
            // 
            // relationPerpendicularRadioButton
            // 
            this.relationPerpendicularRadioButton.AutoSize = true;
            this.relationPerpendicularRadioButton.Location = new System.Drawing.Point(5, 356);
            this.relationPerpendicularRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.relationPerpendicularRadioButton.Name = "relationPerpendicularRadioButton";
            this.relationPerpendicularRadioButton.Size = new System.Drawing.Size(105, 21);
            this.relationPerpendicularRadioButton.TabIndex = 6;
            this.relationPerpendicularRadioButton.Text = "Prostopadłe";
            this.relationPerpendicularRadioButton.UseVisualStyleBackColor = true;
            this.relationPerpendicularRadioButton.CheckedChanged += new System.EventHandler(this.RelationPerpendicularRadioButton_CheckedChanged);
            // 
            // relationEqualRadioButton
            // 
            this.relationEqualRadioButton.AutoSize = true;
            this.relationEqualRadioButton.Location = new System.Drawing.Point(5, 329);
            this.relationEqualRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.relationEqualRadioButton.Name = "relationEqualRadioButton";
            this.relationEqualRadioButton.Size = new System.Drawing.Size(125, 21);
            this.relationEqualRadioButton.TabIndex = 5;
            this.relationEqualRadioButton.Text = "Równa długość";
            this.relationEqualRadioButton.UseVisualStyleBackColor = true;
            this.relationEqualRadioButton.CheckedChanged += new System.EventHandler(this.RelationEqualRadioButton_CheckedChanged);
            // 
            // movePolygonRadioButton
            // 
            this.movePolygonRadioButton.AutoSize = true;
            this.movePolygonRadioButton.Location = new System.Drawing.Point(5, 238);
            this.movePolygonRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.movePolygonRadioButton.Name = "movePolygonRadioButton";
            this.movePolygonRadioButton.Size = new System.Drawing.Size(135, 21);
            this.movePolygonRadioButton.TabIndex = 4;
            this.movePolygonRadioButton.Text = "Przesuń wielokąt";
            this.movePolygonRadioButton.UseVisualStyleBackColor = true;
            this.movePolygonRadioButton.CheckedChanged += new System.EventHandler(this.MovePolygonRadioButton_CheckedChanged);
            // 
            // moveEdgeRadioButton
            // 
            this.moveEdgeRadioButton.AutoSize = true;
            this.moveEdgeRadioButton.Location = new System.Drawing.Point(7, 174);
            this.moveEdgeRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.moveEdgeRadioButton.Name = "moveEdgeRadioButton";
            this.moveEdgeRadioButton.Size = new System.Drawing.Size(137, 21);
            this.moveEdgeRadioButton.TabIndex = 3;
            this.moveEdgeRadioButton.Text = "Przesuń krawędź";
            this.moveEdgeRadioButton.UseVisualStyleBackColor = true;
            this.moveEdgeRadioButton.CheckedChanged += new System.EventHandler(this.MoveEdgeRadioButton_CheckedChanged);
            // 
            // AddInMiddleRadioButton
            // 
            this.AddInMiddleRadioButton.Location = new System.Drawing.Point(7, 129);
            this.AddInMiddleRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AddInMiddleRadioButton.Name = "AddInMiddleRadioButton";
            this.AddInMiddleRadioButton.Size = new System.Drawing.Size(219, 39);
            this.AddInMiddleRadioButton.TabIndex = 2;
            this.AddInMiddleRadioButton.Text = "Dodaj wierzchołek na środku krawędzi";
            this.AddInMiddleRadioButton.UseVisualStyleBackColor = true;
            this.AddInMiddleRadioButton.CheckedChanged += new System.EventHandler(this.AddInMiddleRadioButton_CheckedChanged);
            // 
            // deleteVertexRadioButton
            // 
            this.deleteVertexRadioButton.AutoSize = true;
            this.deleteVertexRadioButton.Location = new System.Drawing.Point(5, 65);
            this.deleteVertexRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deleteVertexRadioButton.Name = "deleteVertexRadioButton";
            this.deleteVertexRadioButton.Size = new System.Drawing.Size(139, 21);
            this.deleteVertexRadioButton.TabIndex = 1;
            this.deleteVertexRadioButton.Text = "Usuń wierzchołek";
            this.deleteVertexRadioButton.UseVisualStyleBackColor = true;
            this.deleteVertexRadioButton.CheckedChanged += new System.EventHandler(this.DeleteVertexRadioButton_CheckedChanged);
            // 
            // moveVertexRadioButton
            // 
            this.moveVertexRadioButton.AutoSize = true;
            this.moveVertexRadioButton.Checked = true;
            this.moveVertexRadioButton.Location = new System.Drawing.Point(5, 38);
            this.moveVertexRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.moveVertexRadioButton.Name = "moveVertexRadioButton";
            this.moveVertexRadioButton.Size = new System.Drawing.Size(158, 21);
            this.moveVertexRadioButton.TabIndex = 0;
            this.moveVertexRadioButton.TabStop = true;
            this.moveVertexRadioButton.Text = "Przesuń wierzchołek";
            this.moveVertexRadioButton.UseVisualStyleBackColor = true;
            this.moveVertexRadioButton.CheckedChanged += new System.EventHandler(this.MoveVertexRadioButton_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem,
            this.widokToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1061, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            this.plikToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.plikToolStripMenuItem.Text = "Plik...";
            // 
            // widokToolStripMenuItem
            // 
            this.widokToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelsToolStripMenuItem});
            this.widokToolStripMenuItem.Name = "widokToolStripMenuItem";
            this.widokToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.widokToolStripMenuItem.Text = "Widok";
            // 
            // labelsToolStripMenuItem
            // 
            this.labelsToolStripMenuItem.Checked = true;
            this.labelsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.labelsToolStripMenuItem.Name = "labelsToolStripMenuItem";
            this.labelsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.labelsToolStripMenuItem.Text = "Znaczniki relacji";
            this.labelsToolStripMenuItem.Click += new System.EventHandler(this.labelsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.helpLabel);
            this.panel1.Location = new System.Drawing.Point(12, 421);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 206);
            this.panel1.TabIndex = 3;
            // 
            // helpLabel
            // 
            this.helpLabel.Location = new System.Drawing.Point(-3, 0);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.helpLabel.Size = new System.Drawing.Size(223, 202);
            this.helpLabel.TabIndex = 0;
            this.helpLabel.Text = "label4";
            // 
            // canvasPictureBox
            // 
            this.canvasPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvasPictureBox.Location = new System.Drawing.Point(249, 31);
            this.canvasPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.canvasPictureBox.Name = "canvasPictureBox";
            this.canvasPictureBox.Size = new System.Drawing.Size(799, 600);
            this.canvasPictureBox.TabIndex = 0;
            this.canvasPictureBox.TabStop = false;
            this.canvasPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasPictureBox_Paint);
            this.canvasPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasPictureBox_MouseDown);
            this.canvasPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CanvasPictureBox_MouseMove);
            this.canvasPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CanvasPictureBox_MouseUp);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 639);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolsGroupBox);
            this.Controls.Add(this.canvasPictureBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "EditorForm";
            this.Text = "Edytor Wielokątów";
            this.toolsGroupBox.ResumeLayout(false);
            this.toolsGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox toolsGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.RadioButton createPolygonRadioButton;
        private System.Windows.Forms.RadioButton relationPerpendicularRadioButton;
        private System.Windows.Forms.RadioButton relationEqualRadioButton;
        private System.Windows.Forms.RadioButton movePolygonRadioButton;
        private System.Windows.Forms.RadioButton moveEdgeRadioButton;
        private System.Windows.Forms.RadioButton AddInMiddleRadioButton;
        private System.Windows.Forms.RadioButton deleteVertexRadioButton;
        private System.Windows.Forms.RadioButton moveVertexRadioButton;
        private System.Windows.Forms.ToolStripMenuItem plikToolStripMenuItem;
        private System.Windows.Forms.Label relationsToolsLabel;
        private System.Windows.Forms.Label polygonToolsLabel;
        private System.Windows.Forms.Label edgeToolsLabel;
        private System.Windows.Forms.Label vertexToolsLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label helpLabel;
        private System.Windows.Forms.PictureBox canvasPictureBox;
        private System.Windows.Forms.ToolStripMenuItem widokToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsToolStripMenuItem;
    }
}

