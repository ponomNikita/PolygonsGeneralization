namespace PolygonGenerator
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Tools = new System.Windows.Forms.Panel();
            this.UnionBtn = new System.Windows.Forms.Button();
            this.WithDeltaArea = new System.Windows.Forms.CheckBox();
            this.MinDistanceTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.SaveAsBtn = new System.Windows.Forms.Button();
            this.PolygonsCount = new System.Windows.Forms.Label();
            this.UndoBtn = new System.Windows.Forms.Button();
            this.Coords = new System.Windows.Forms.Label();
            this.Container = new System.Windows.Forms.Panel();
            this.DrawSourceBtn = new System.Windows.Forms.Button();
            this.Tools.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tools
            // 
            this.Tools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Tools.Controls.Add(this.DrawSourceBtn);
            this.Tools.Controls.Add(this.UnionBtn);
            this.Tools.Controls.Add(this.WithDeltaArea);
            this.Tools.Controls.Add(this.MinDistanceTextBox);
            this.Tools.Controls.Add(this.label1);
            this.Tools.Controls.Add(this.SaveBtn);
            this.Tools.Controls.Add(this.LoadBtn);
            this.Tools.Controls.Add(this.SaveAsBtn);
            this.Tools.Controls.Add(this.PolygonsCount);
            this.Tools.Controls.Add(this.UndoBtn);
            this.Tools.Controls.Add(this.Coords);
            this.Tools.Location = new System.Drawing.Point(790, 12);
            this.Tools.Name = "Tools";
            this.Tools.Size = new System.Drawing.Size(180, 466);
            this.Tools.TabIndex = 1;
            // 
            // UnionBtn
            // 
            this.UnionBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.UnionBtn.Location = new System.Drawing.Point(19, 212);
            this.UnionBtn.Name = "UnionBtn";
            this.UnionBtn.Size = new System.Drawing.Size(142, 23);
            this.UnionBtn.TabIndex = 11;
            this.UnionBtn.Text = "Union";
            this.UnionBtn.UseVisualStyleBackColor = true;
            this.UnionBtn.Click += new System.EventHandler(this.UnionBtn_Click);
            // 
            // WithDeltaArea
            // 
            this.WithDeltaArea.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.WithDeltaArea.AutoSize = true;
            this.WithDeltaArea.Location = new System.Drawing.Point(9, 175);
            this.WithDeltaArea.Name = "WithDeltaArea";
            this.WithDeltaArea.Size = new System.Drawing.Size(123, 17);
            this.WithDeltaArea.TabIndex = 10;
            this.WithDeltaArea.Text = "Draw with delta area";
            this.WithDeltaArea.UseVisualStyleBackColor = true;
            this.WithDeltaArea.CheckStateChanged += new System.EventHandler(this.WithDeltaArea_CheckStateChanged);
            // 
            // MinDistanceTextBox
            // 
            this.MinDistanceTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.MinDistanceTextBox.Location = new System.Drawing.Point(79, 143);
            this.MinDistanceTextBox.Name = "MinDistanceTextBox";
            this.MinDistanceTextBox.Size = new System.Drawing.Size(82, 20);
            this.MinDistanceTextBox.TabIndex = 9;
            this.MinDistanceTextBox.Text = "50";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Min distance";
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SaveBtn.Location = new System.Drawing.Point(19, 368);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(142, 23);
            this.SaveBtn.TabIndex = 7;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click_1);
            // 
            // LoadBtn
            // 
            this.LoadBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LoadBtn.Location = new System.Drawing.Point(19, 339);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(142, 23);
            this.LoadBtn.TabIndex = 6;
            this.LoadBtn.Text = "Load";
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // SaveAsBtn
            // 
            this.SaveAsBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SaveAsBtn.Location = new System.Drawing.Point(19, 397);
            this.SaveAsBtn.Name = "SaveAsBtn";
            this.SaveAsBtn.Size = new System.Drawing.Size(142, 23);
            this.SaveAsBtn.TabIndex = 5;
            this.SaveAsBtn.Text = "Save as";
            this.SaveAsBtn.UseVisualStyleBackColor = true;
            this.SaveAsBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // PolygonsCount
            // 
            this.PolygonsCount.AutoSize = true;
            this.PolygonsCount.Location = new System.Drawing.Point(19, 46);
            this.PolygonsCount.Name = "PolygonsCount";
            this.PolygonsCount.Size = new System.Drawing.Size(53, 13);
            this.PolygonsCount.TabIndex = 4;
            this.PolygonsCount.Text = "Polygons:";
            // 
            // UndoBtn
            // 
            this.UndoBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.UndoBtn.Location = new System.Drawing.Point(19, 426);
            this.UndoBtn.Name = "UndoBtn";
            this.UndoBtn.Size = new System.Drawing.Size(142, 23);
            this.UndoBtn.TabIndex = 3;
            this.UndoBtn.Text = "Undo";
            this.UndoBtn.UseVisualStyleBackColor = true;
            this.UndoBtn.Click += new System.EventHandler(this.UndoBtn_Click);
            // 
            // Coords
            // 
            this.Coords.AutoSize = true;
            this.Coords.Location = new System.Drawing.Point(16, 13);
            this.Coords.Name = "Coords";
            this.Coords.Size = new System.Drawing.Size(30, 13);
            this.Coords.TabIndex = 2;
            this.Coords.Text = "X: Y:";
            // 
            // Container
            // 
            this.Container.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Container.AutoScroll = true;
            this.Container.BackColor = System.Drawing.SystemColors.Window;
            this.Container.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Container.Location = new System.Drawing.Point(12, 12);
            this.Container.Name = "Container";
            this.Container.Size = new System.Drawing.Size(772, 466);
            this.Container.TabIndex = 3;
            // 
            // DrawSourceBtn
            // 
            this.DrawSourceBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.DrawSourceBtn.Location = new System.Drawing.Point(19, 241);
            this.DrawSourceBtn.Name = "DrawSourceBtn";
            this.DrawSourceBtn.Size = new System.Drawing.Size(142, 23);
            this.DrawSourceBtn.TabIndex = 12;
            this.DrawSourceBtn.Text = "Draw source";
            this.DrawSourceBtn.UseVisualStyleBackColor = true;
            this.DrawSourceBtn.Click += new System.EventHandler(this.DrawSourceBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 490);
            this.Controls.Add(this.Container);
            this.Controls.Add(this.Tools);
            this.Name = "Main";
            this.Text = "Polygons generator";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Tools.ResumeLayout(false);
            this.Tools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Tools;
        private System.Windows.Forms.Panel Container;
        private System.Windows.Forms.Label Coords;
        private System.Windows.Forms.Label PolygonsCount;
        private System.Windows.Forms.Button UndoBtn;
        private System.Windows.Forms.Button LoadBtn;
        private System.Windows.Forms.Button SaveAsBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.CheckBox WithDeltaArea;
        private System.Windows.Forms.TextBox MinDistanceTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button UnionBtn;
        private System.Windows.Forms.Button DrawSourceBtn;
    }
}

