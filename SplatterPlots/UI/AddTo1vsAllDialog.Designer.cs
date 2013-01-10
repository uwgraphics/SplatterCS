namespace SplatterPlots
{
    partial class AddTo1vsAllDialog
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
            this.comboBoxGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxHorizontal = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxVertical = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Location = new System.Drawing.Point(127, 63);
            this.comboBoxGroup.Name = "comboBoxGroup";
            this.comboBoxGroup.Size = new System.Drawing.Size(121, 21);
            this.comboBoxGroup.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Group By:";
            // 
            // comboBoxHorizontal
            // 
            this.comboBoxHorizontal.FormattingEnabled = true;
            this.comboBoxHorizontal.Location = new System.Drawing.Point(127, 9);
            this.comboBoxHorizontal.Name = "comboBoxHorizontal";
            this.comboBoxHorizontal.Size = new System.Drawing.Size(121, 21);
            this.comboBoxHorizontal.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Horizontal Dimension:";
            // 
            // comboBoxVertical
            // 
            this.comboBoxVertical.FormattingEnabled = true;
            this.comboBoxVertical.Location = new System.Drawing.Point(127, 36);
            this.comboBoxVertical.Name = "comboBoxVertical";
            this.comboBoxVertical.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVertical.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Vertical Dimension:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(88, 92);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(169, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AddTo1vsAllDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 127);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxVertical);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxHorizontal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxGroup);
            this.Controls.Add(this.label1);
            this.Name = "AddTo1vsAllDialog";
            this.Text = "AddTo1vsAllDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxHorizontal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxVertical;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}