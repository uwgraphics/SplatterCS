using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SplatterPlots
{
    public partial class SplatterviewContainer : UserControl
    {
        SplatterView m_View;
        VerticalLabel verticalLabel1;
        public event EventHandler PointSelection;

        public SplatterviewContainer()
        {
            InitializeComponent();
            verticalLabel1 = new SplatterPlots.VerticalLabel();
            m_View = new SplatterView();
            m_View.Dock = DockStyle.Fill;
            m_View.ModelChanged += new EventHandler(m_View_ModelChanged);
            m_View.PointSelection += new EventHandler(m_View_PointSelection);
            if (Program.Runtime)
            {
                tableLayoutPanel1.Controls.Add(m_View, 1, 0);
                this.tableLayoutPanel1.Controls.Add(this.verticalLabel1, 0, 0);

                // 
                // verticalLabel1
                // 
                this.verticalLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.verticalLabel1.Location = new System.Drawing.Point(2, 15);
                this.verticalLabel1.Margin = new System.Windows.Forms.Padding(0);
                this.verticalLabel1.Name = "verticalLabel1";
                this.verticalLabel1.RenderingMode = System.Drawing.Text.TextRenderingHint.SystemDefault;
                this.verticalLabel1.Size = new System.Drawing.Size(15, 100);
                this.verticalLabel1.TabIndex = 0;
                this.verticalLabel1.Text = "verticalLabel1";
                this.verticalLabel1.TextDrawMode = SplatterPlots.DrawMode.BottomUp;
                this.verticalLabel1.TransparentBackground = false;
            }            
        }

        void m_View_PointSelection(object sender, EventArgs e)
        {
            if (PointSelection!=null)
            {
                PointSelection(this, EventArgs.Empty);
            }
        }

        void m_View_ModelChanged(object sender, EventArgs e)
        {
            label1.Text = m_View.Model.dim0Name;
            verticalLabel1.Text = m_View.Model.dim1Name;
            label1.Refresh();
            verticalLabel1.Refresh();
        }
        public SplatterView View { get { return m_View; } }
    }
}