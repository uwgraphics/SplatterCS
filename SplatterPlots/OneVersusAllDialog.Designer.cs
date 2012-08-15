namespace SplatterPlots
{
    partial class OneVersusAllDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.sliderController1 = new SplatterPlots.SliderController();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 1000);
            this.panel1.TabIndex = 0;
            // 
            // sliderController1
            // 
            this.sliderController1.Location = new System.Drawing.Point(1018, 12);
            this.sliderController1.MaximumSize = new System.Drawing.Size(1000, 650);
            this.sliderController1.MinimumSize = new System.Drawing.Size(250, 450);
            this.sliderController1.Name = "sliderController1";
            this.sliderController1.Size = new System.Drawing.Size(449, 487);
            this.sliderController1.TabIndex = 1;
            // 
            // OneVersusAllDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1554, 1029);
            this.Controls.Add(this.sliderController1);
            this.Controls.Add(this.panel1);
            this.Name = "OneVersusAllDialog";
            this.Text = "OneVersusAllDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SliderController sliderController1;
    }
}