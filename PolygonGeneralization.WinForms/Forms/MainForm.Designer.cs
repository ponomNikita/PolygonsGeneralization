namespace PolygonGeneralization.WinForms.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Canvas = new System.Windows.Forms.Panel();
            this.Meta = new System.Windows.Forms.Panel();
            this.GeneralizeBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MinDistanceTextBox = new System.Windows.Forms.TextBox();
            this.DrawSourceBtn = new System.Windows.Forms.Button();
            this.TopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.BackColor = System.Drawing.SystemColors.Control;
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(1252, 24);
            this.TopMenu.TabIndex = 0;
            this.TopMenu.Text = "TopMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // Canvas
            // 
            this.Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Canvas.AutoSize = true;
            this.Canvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Canvas.Location = new System.Drawing.Point(12, 40);
            this.Canvas.MinimumSize = new System.Drawing.Size(760, 509);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(899, 509);
            this.Canvas.TabIndex = 1;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            // 
            // Meta
            // 
            this.Meta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Meta.AutoSize = true;
            this.Meta.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Meta.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Meta.Location = new System.Drawing.Point(927, 40);
            this.Meta.MinimumSize = new System.Drawing.Size(193, 400);
            this.Meta.Name = "Meta";
            this.Meta.Size = new System.Drawing.Size(313, 400);
            this.Meta.TabIndex = 2;
            // 
            // GeneralizeBtn
            // 
            this.GeneralizeBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.GeneralizeBtn.Location = new System.Drawing.Point(958, 497);
            this.GeneralizeBtn.Name = "GeneralizeBtn";
            this.GeneralizeBtn.Size = new System.Drawing.Size(251, 23);
            this.GeneralizeBtn.TabIndex = 3;
            this.GeneralizeBtn.Text = "Generalize";
            this.GeneralizeBtn.UseVisualStyleBackColor = true;
            this.GeneralizeBtn.Click += new System.EventHandler(this.GeneralizeBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(955, 478);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Min distance";
            // 
            // MinDistanceTextBox
            // 
            this.MinDistanceTextBox.Location = new System.Drawing.Point(1037, 471);
            this.MinDistanceTextBox.Name = "MinDistanceTextBox";
            this.MinDistanceTextBox.Size = new System.Drawing.Size(172, 20);
            this.MinDistanceTextBox.TabIndex = 5;
            // 
            // DrawSourceBtn
            // 
            this.DrawSourceBtn.Location = new System.Drawing.Point(958, 526);
            this.DrawSourceBtn.Name = "DrawSourceBtn";
            this.DrawSourceBtn.Size = new System.Drawing.Size(251, 23);
            this.DrawSourceBtn.TabIndex = 6;
            this.DrawSourceBtn.Text = "Draw Source";
            this.DrawSourceBtn.UseVisualStyleBackColor = true;
            this.DrawSourceBtn.Click += new System.EventHandler(this.DrawSourceBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1252, 561);
            this.Controls.Add(this.DrawSourceBtn);
            this.Controls.Add(this.MinDistanceTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GeneralizeBtn);
            this.Controls.Add(this.Meta);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.TopMenu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.TopMenu;
            this.MinimumSize = new System.Drawing.Size(1010, 600);
            this.Name = "MainForm";
            this.Text = "Polygons generalization";
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel Canvas;
        private System.Windows.Forms.Panel Meta;
        private System.Windows.Forms.Button GeneralizeBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MinDistanceTextBox;
        private System.Windows.Forms.Button DrawSourceBtn;
    }
}