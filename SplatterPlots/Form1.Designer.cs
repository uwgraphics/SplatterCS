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
            this.splatterView1 = new SplatterPlots.SplatterView();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1014, 24);
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
            // splatterView1
            // 
            this.splatterView1.BackColor = System.Drawing.Color.Black;
            this.splatterView1.Bandwidth = 10F;
            this.splatterView1.ChromaF = 0.95F;
            this.splatterView1.ClutterWindow = 30F;
            this.splatterView1.Gain = 1F;
            this.splatterView1.LightnessF = 0.95F;
            this.splatterView1.Location = new System.Drawing.Point(12, 27);
            this.splatterView1.LowerLimit = 0.5F;
            this.splatterView1.Name = "splatterView1";
            this.splatterView1.ScaleFactorX = 1F;
            this.splatterView1.ScaleFactorY = 1F;
            this.splatterView1.Size = new System.Drawing.Size(700, 700);
            this.splatterView1.StripePeriod = 0F;
            this.splatterView1.StripeWidth = 0F;
            this.splatterView1.TabIndex = 0;
            this.splatterView1.VSync = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 790);
            this.Controls.Add(this.splatterView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplatterView splatterView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromCSVToolStripMenuItem;

    }
}

