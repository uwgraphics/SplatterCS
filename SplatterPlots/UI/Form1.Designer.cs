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
            this.listViewDataFiles = new System.Windows.Forms.ListView();
            this.button1vsAll = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.buttonSplatter = new System.Windows.Forms.Button();
            this.buttonSplam = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewDataFiles
            // 
            this.listViewDataFiles.Location = new System.Drawing.Point(12, 12);
            this.listViewDataFiles.Name = "listViewDataFiles";
            this.listViewDataFiles.Size = new System.Drawing.Size(193, 286);
            this.listViewDataFiles.TabIndex = 2;
            this.listViewDataFiles.UseCompatibleStateImageBehavior = false;
            this.listViewDataFiles.View = System.Windows.Forms.View.List;
            // 
            // button1vsAll
            // 
            this.button1vsAll.Location = new System.Drawing.Point(211, 70);
            this.button1vsAll.Name = "button1vsAll";
            this.button1vsAll.Size = new System.Drawing.Size(103, 23);
            this.button1vsAll.TabIndex = 7;
            this.button1vsAll.Text = "Add to 1vsRest";
            this.button1vsAll.UseVisualStyleBackColor = true;
            this.button1vsAll.Click += new System.EventHandler(this.button1vsAll_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(12, 304);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadFile.TabIndex = 10;
            this.buttonLoadFile.Text = "Load File...";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // buttonSplatter
            // 
            this.buttonSplatter.Location = new System.Drawing.Point(211, 12);
            this.buttonSplatter.Name = "buttonSplatter";
            this.buttonSplatter.Size = new System.Drawing.Size(103, 23);
            this.buttonSplatter.TabIndex = 11;
            this.buttonSplatter.Text = "Single Splatterplot";
            this.buttonSplatter.UseVisualStyleBackColor = true;
            this.buttonSplatter.Click += new System.EventHandler(this.buttonSplatter_Click);
            // 
            // buttonSplam
            // 
            this.buttonSplam.Location = new System.Drawing.Point(212, 41);
            this.buttonSplam.Name = "buttonSplam";
            this.buttonSplam.Size = new System.Drawing.Size(103, 23);
            this.buttonSplam.TabIndex = 12;
            this.buttonSplam.Text = "SPLAM";
            this.buttonSplam.UseVisualStyleBackColor = true;
            this.buttonSplam.Click += new System.EventHandler(this.buttonSplam_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 338);
            this.Controls.Add(this.buttonSplam);
            this.Controls.Add(this.buttonSplatter);
            this.Controls.Add(this.button1vsAll);
            this.Controls.Add(this.buttonLoadFile);
            this.Controls.Add(this.listViewDataFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Splatterplots";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewDataFiles;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1vsAll;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.Button buttonSplatter;
        private System.Windows.Forms.Button buttonSplam;

    }
}

