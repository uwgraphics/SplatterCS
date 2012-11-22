namespace SplatterPlots
{
    partial class SplamDialog
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
            this.listView1 = new System.Windows.Forms.ListView();
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
            this.sliderController1.AutoSize = true;
            this.sliderController1.Location = new System.Drawing.Point(1018, 12);
            this.sliderController1.MaximumSize = new System.Drawing.Size(1000, 650);
            this.sliderController1.MinimumSize = new System.Drawing.Size(250, 450);
            this.sliderController1.Name = "sliderController1";
            this.sliderController1.Size = new System.Drawing.Size(475, 512);
            this.sliderController1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(1018, 530);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(475, 229);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // SplamDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1505, 1022);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.sliderController1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SplamDialog";
            this.Text = "SplamDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private SliderController sliderController1;
        private System.Windows.Forms.ListView listView1;
    }
}