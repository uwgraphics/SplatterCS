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
    public partial class OneVersusAllDialog : Form
    {
        private OneVsAllModel m_Model;
        private List<SplatterView> m_Views = new List<SplatterView>();
        private Dictionary<ListViewItem, DataSeries> m_Series = new Dictionary<ListViewItem, DataSeries>();
        private SingleSplatterDialog m_SingleSplatterDialog = null;
        private SelectionTable m_SelectionDialog = null;

        
        public OneVersusAllDialog()
        {
            InitializeComponent();
        }
        public void SetModel(OneVsAllModel model)
        {
            panel1.Controls.Clear();
            m_Model = model;
            m_Views.Clear();

            //how to best utilize the space?? look for the closest perfect square
            int num = (int)(Math.Floor(Math.Sqrt(m_Model.SplatList.Count))) + 1;
            float sz = (float)(panel1.Width) / (num);
            int szi = (int)sz;
            
            for (int i = 0,k=0; i < num&&k<m_Model.SplatList.Count; i++)
            {
                for (int j = 0; j < num&&k<m_Model.SplatList.Count; j++,k++)
                {
                    var view = new SplatterviewContainer();
                    view.Name = "oneVsAll" + k;
                    view.Size = new Size(szi - 25, szi - 25);
                    view.Location = new Point(i * szi + 25, j * szi);
                    view.PointSelection += new EventHandler(view_PointSelection);
                    view.View.DoubleClick += new EventHandler(view_DoubleClick);
                    view.View.setSplatPM(m_Model.SplatList[k]);
                    m_Views.Add(view.View);
                    panel1.Controls.Add(view);
                }
            }
            sliderController1.SetView(m_Views);
            foreach (var series in m_Model.Groups)
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
            ListViewItem otherItem = new ListViewItem(m_Model.OthersName);
            otherItem.Name = m_Model.OthersName;
            otherItem.BackColor = m_Model.OtherColor;
            listView1.Items.Add(otherItem);
        }

        SingleSplatterDialog SingleSplatterDialog
        {
            get
            {
                if (m_SingleSplatterDialog == null || m_SingleSplatterDialog.IsDisposed)
                {
                    m_SingleSplatterDialog = new SingleSplatterDialog();
                    m_SingleSplatterDialog.PointSelection += new EventHandler(view_PointSelection);
                    m_SingleSplatterDialog.Text = this.Text;
                }
                return m_SingleSplatterDialog;
            }
        }
        SelectionTable SelectionTableDialog
        {
            get
            {
                if (m_SelectionDialog == null || m_SelectionDialog.IsDisposed)
                {
                    m_SelectionDialog = new SelectionTable();
                    m_SelectionDialog.Text = this.Text;
                }
                return m_SelectionDialog;
            }
        }      

        void view_PointSelection(object sender, EventArgs e)
        {
            if (m_SingleSplatterDialog != null)
            {
                m_SingleSplatterDialog.Refresh();
            }
            m_Views.ForEach(v => v.Refresh());
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
                SingleSplatterDialog.SetModel(view.Model);
                SingleSplatterDialog.Show();
                SingleSplatterDialog.BringToFront();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var selected = listView1.SelectedItems[0];
            var color = selected.BackColor;
            colorDialog1.Color = color;
            var result = colorDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                selected.BackColor = colorDialog1.Color;
                if (selected.Name == m_Model.OthersName)
                {
                    m_Model.SetOTherColor(colorDialog1.Color);
                }
                else
                {
                    m_Series[selected].Color = colorDialog1.Color;
                }
                listView1.SelectedItems.Clear();
                SingleSplatterDialog.Refresh();
                m_Views.ForEach(v => v.Refresh());
            }
        }
    }
}
