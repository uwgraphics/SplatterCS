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
        public event EventHandler PointSelection;
        public SplatterviewContainer()
        {
            InitializeComponent();
            m_View = new SplatterView();
            m_View.Dock = DockStyle.Fill;
            m_View.ModelChanged += new EventHandler(m_View_ModelChanged);
            m_View.PointSelection += new EventHandler(m_View_PointSelection);
            if (Program.Runtime)
            {
                tableLayoutPanel1.Controls.Add(m_View, 1, 0);
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
