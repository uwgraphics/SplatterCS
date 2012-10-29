namespace SplatterPlots
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewDataFiles = new System.Windows.Forms.ListView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listViewDataSeries = new System.Windows.Forms.ListView();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button1vsAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.buttonSplat = new System.Windows.Forms.Button();
            this.buttonSplam = new System.Windows.Forms.Button();
            this.buttonClutter = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(646, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromCSVToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadFromCSVToolStripMenuItem
            // 
            this.loadFromCSVToolStripMenuItem.Name = "loadFromCSVToolStripMenuItem";
            this.loadFromCSVToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadFromCSVToolStripMenuItem.Text = "Load From CSV...";
            this.loadFromCSVToolStripMenuItem.Click += new System.EventHandler(this.loadFromCSVToolStripMenuItem_Click);
            // 
            // listViewDataFiles
            // 
            this.listViewDataFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDataFiles.Location = new System.Drawing.Point(3, 3);
            this.listViewDataFiles.Name = "listViewDataFiles";
            this.listViewDataFiles.Size = new System.Drawing.Size(193, 286);
            this.listViewDataFiles.TabIndex = 2;
            this.listViewDataFiles.UseCompatibleStateImageBehavior = false;
            this.listViewDataFiles.View = System.Windows.Forms.View.List;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(0, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(91, 23);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listViewDataSeries
            // 
            this.listViewDataSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDataSeries.Location = new System.Drawing.Point(299, 3);
            this.listViewDataSeries.Name = "listViewDataSeries";
            this.listViewDataSeries.Size = new System.Drawing.Size(194, 286);
            this.listViewDataSeries.TabIndex = 4;
            this.listViewDataSeries.UseCompatibleStateImageBehavior = false;
            this.listViewDataSeries.View = System.Windows.Forms.View.List;
            this.listViewDataSeries.DoubleClick += new System.EventHandler(this.listViewDataSeries_DoubleClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(0, 61);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(91, 23);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewDataFiles, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewDataSeries, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(496, 305);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.buttonClutter);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button1vsAll);
            this.panel1.Controls.Add(this.buttonAdd);
            this.panel1.Controls.Add(this.buttonRemove);
            this.panel1.Location = new System.Drawing.Point(202, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(91, 145);
            this.panel1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Performance Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1vsAll
            // 
            this.button1vsAll.Location = new System.Drawing.Point(0, 32);
            this.button1vsAll.Name = "button1vsAll";
            this.button1vsAll.Size = new System.Drawing.Size(91, 23);
            this.button1vsAll.TabIndex = 7;
            this.button1vsAll.Text = "Add to 1vsAll";
            this.button1vsAll.UseVisualStyleBackColor = true;
            this.button1vsAll.Click += new System.EventHandler(this.button1vsAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Available Files";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Data Series";
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // buttonSplat
            // 
            this.buttonSplat.Location = new System.Drawing.Point(515, 285);
            this.buttonSplat.Name = "buttonSplat";
            this.buttonSplat.Size = new System.Drawing.Size(119, 23);
            this.buttonSplat.TabIndex = 8;
            this.buttonSplat.Text = "Show Splatterplot";
            this.buttonSplat.UseVisualStyleBackColor = true;
            this.buttonSplat.Click += new System.EventHandler(this.buttonSplat_Click);
            // 
            // buttonSplam
            // 
            this.buttonSplam.Location = new System.Drawing.Point(515, 314);
            this.buttonSplam.Name = "buttonSplam";
            this.buttonSplam.Size = new System.Drawing.Size(119, 23);
            this.buttonSplam.TabIndex = 9;
            this.buttonSplam.Text = "Show SPLAM";
            this.buttonSplam.UseVisualStyleBackColor = true;
            this.buttonSplam.Click += new System.EventHandler(this.buttonSplam_Click);
            // 
            // buttonClutter
            // 
            this.buttonClutter.Location = new System.Drawing.Point(0, 119);
            this.buttonClutter.Name = "buttonClutter";
            this.buttonClutter.Size = new System.Drawing.Size(91, 23);
            this.buttonClutter.TabIndex = 9;
            this.buttonClutter.Text = "Clutter";
            this.buttonClutter.UseVisualStyleBackColor = true;
            this.buttonClutter.Click += new System.EventHandler(this.buttonClutter_click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 345);
            this.Controls.Add(this.buttonSplam);
            this.Controls.Add(this.buttonSplat);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromCSVToolStripMenuItem;
        private System.Windows.Forms.ListView listViewDataFiles;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListView listViewDataSeries;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button buttonSplat;
        private System.Windows.Forms.Button buttonSplam;
        private System.Windows.Forms.Button button1vsAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonClutter;

    }
}

