namespace SplatterPlots
{
    partial class SplatterView
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
            ClearVBOs();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SplatterView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SplatterView";
            this.Load += new System.EventHandler(this.SplatterView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SplatterView_Paint);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SplatterView_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplatterView_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SplatterView_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SplatterView_MouseUp);
            this.Resize += new System.EventHandler(this.SplatterView_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
