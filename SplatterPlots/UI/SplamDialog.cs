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

        private SingleSplatterDialog m_SplatterDialog;
        private SingleSplatterDialog SplatterDialog
        {
            get
            {
                if (m_SplatterDialog == null||m_SplatterDialog.IsDisposed)
                {
                    m_SplatterDialog = new SingleSplatterDialog();
                    m_SplatterDialog.PointSelection += new EventHandler(view_PointSelection);
                    m_SplatterDialog.Text = this.Text;
                } 
                return m_SplatterDialog;
            }
        }

        private SelectionTable m_SelectionTable;
        private SelectionTable SelectionTableDialog
        {
            get
            {
                if (m_SelectionTable == null || m_SelectionTable.IsDisposed)
                {
                    m_SelectionTable= new SelectionTable();
                    m_SelectionTable.Text = this.Text;
                }
                return m_SelectionTable;
            }
        }

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
            if (m_SplatterDialog != null && !m_SplatterDialog.IsDisposed)
            {
                m_SplatterDialog.Refresh();
            }
            Refresh();
            DataTable view = null;
            foreach (var series in m_Series.Values)
            {
                if (view == null)
                {
                    var list = series.GetSelectedRows();
                    if (list.Count() > 0)
                    {
                        view = list.CopyToDataTable();
                    }
                }
                else
                {
                    series.GetSelectedRows().CopyToDataTable(view, LoadOption.PreserveChanges);
                }
            }
            SelectionTableDialog.SetDataView(view);
            if (!SelectionTableDialog.Visible)
            {
                SelectionTableDialog.Show();
            }
            SelectionTableDialog.BringToFront();
        }

        void view_DoubleClick(object sender, EventArgs e)
        {
            var view = sender as SplatterView;
            if (view != null)
            {
                SplatterDialog.SetModel(view.Model);
                SplatterDialog.Show();
                SplatterDialog.BringToFront();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (m_SelectionTable!=null)
            {
                m_SelectionTable.Close();
            }
            if (m_SplatterDialog != null)
            {
                m_SplatterDialog.Close();
            }
            base.OnClosing(e);
        }
    }
}
