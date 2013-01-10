using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SplatterPlots
{
    public partial class SplamDialog : Form
    {
        SplamModel m_Model;
        List<SplatterView> m_Views = new List<SplatterView>();
        private Dictionary<ListViewItem, DataSeries> m_Series = new Dictionary<ListViewItem, DataSeries>();

        public event EventHandler SplatterSelection;
        public event EventHandler PointSelection;

        public SplamDialog()
        {
            InitializeComponent();
        }

        public void SetModel(SplamModel model)
        {
            panel1.Controls.Clear();
            m_Model = model;
            m_Views.Clear();            
            float sz = (float)(panel1.Width) / (m_Model.numDim - 1.0f);
            int szi = (int)sz;
            for (int i = 0; i < m_Model.SplatList.Count; i++)
            {
                var view = new SplatterviewContainer();
                int I = m_Model.Iindex[i];
                int J = m_Model.Jindex[i] - 1;
                view.Name = "splom" + I + J;
                view.Size = new System.Drawing.Size(szi - 25, szi - 25);
                view.Location = new Point(I * szi + 25, J * szi);
                view.PointSelection += new EventHandler(view_PointSelection);
                m_Views.Add(view.View);
                view.View.DoubleClick += new EventHandler(view_DoubleClick);
                view.View.setSplatPM(m_Model.SplatList[i]);
                panel1.Controls.Add(view);
            }

            sliderController1.SetView(m_Views);

            foreach (var series in m_Model.Series)
            {
                ListViewItem item = new ListViewItem(series.Name);
                item.Name = series.Name;
                item.BackColor = series.Color;
                if (!listView1.Items.ContainsKey(item.Name))
                {
                    m_Series[item] = series;
                    listView1.Items.Add(item);
                }
            }
        }

        void view_PointSelection(object sender, EventArgs e)
        {
            if (PointSelection != null)
            {
                PointSelection(this, EventArgs.Empty);
            }
        }

        void view_DoubleClick(object sender, EventArgs e)
        {
            var view = sender as SplatterView;
            if (view != null && SplatterSelection!=null)
            {
                SplatterSelection(view, EventArgs.Empty);
            }
        }
    }
}
