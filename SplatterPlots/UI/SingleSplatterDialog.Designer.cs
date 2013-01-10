namespace SplatterPlots
{
    partial class SingleSplatterDialog
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splatterView1 = new SplatterPlots.SplatterviewContainer();
            this.sliderController1 = new SplatterPlots.SliderController();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(768, 556);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(453, 206);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseDoubleClick);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // splatterView1
            // 
            this.splatterView1.Location = new System.Drawing.Point(12, 12);
            this.splatterView1.Name = "splatterView1";
            this.splatterView1.Size = new System.Drawing.Size(750, 750);
            this.splatterView1.TabIndex = 3;
            // 
            // sliderController1
            // 
            this.sliderController1.AutoSize = true;
            this.sliderController1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.sliderController1.Location = new System.Drawing.Point(768, 12);
            this.sliderController1.MaximumSize = new System.Drawing.Size(1000, 650);
            this.sliderController1.MinimumSize = new System.Drawing.Size(250, 450);
            this.sliderController1.Name = "sliderController1";
            this.sliderController1.Size = new System.Drawing.Size(453, 538);
            this.sliderController1.TabIndex = 1;
            // 
            // SingleSplatterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 767);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.splatterView1);
            this.Controls.Add(this.sliderController1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SingleSplatterDialog";
            this.Text = "SingleSplatterDialog";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SliderController sliderController1;
        private SplatterviewContainer splatterView1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}