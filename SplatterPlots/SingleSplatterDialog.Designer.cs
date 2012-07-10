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
            this.sliderController1 = new SplatterPlots.SliderController();
            this.splatterView1 = new SplatterPlots.SplatterView();
            this.SuspendLayout();
            // 
            // sliderController1
            // 
            this.sliderController1.Location = new System.Drawing.Point(718, 12);
            this.sliderController1.MaximumSize = new System.Drawing.Size(1000, 450);
            this.sliderController1.MinimumSize = new System.Drawing.Size(250, 450);
            this.sliderController1.Name = "sliderController1";
            this.sliderController1.Size = new System.Drawing.Size(500, 450);
            this.sliderController1.TabIndex = 1;
            // 
            // splatterView1
            // 
            this.splatterView1.BackColor = System.Drawing.Color.Black;
            this.splatterView1.Bandwidth = 10F;
            this.splatterView1.ChromaF = 0.95F;
            this.splatterView1.ClutterWindow = 30F;
            this.splatterView1.Gain = 1F;
            this.splatterView1.LightnessF = 0.95F;
            this.splatterView1.Location = new System.Drawing.Point(12, 12);
            this.splatterView1.LowerLimit = 0F;
            this.splatterView1.Name = "splatterView1";
            this.splatterView1.ScaleFactorX = 1F;
            this.splatterView1.ScaleFactorY = 1F;
            this.splatterView1.Size = new System.Drawing.Size(700, 700);
            this.splatterView1.StripePeriod = 50F;
            this.splatterView1.StripeWidth = 1F;
            this.splatterView1.TabIndex = 2;
            this.splatterView1.VSync = false;
            // 
            // SingleSplatterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 724);
            this.Controls.Add(this.splatterView1);
            this.Controls.Add(this.sliderController1);
            this.Name = "SingleSplatterDialog";
            this.Text = "SingleSplatterDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private SliderController sliderController1;
        private SplatterView splatterView1;
    }
}