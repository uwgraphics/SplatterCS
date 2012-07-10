using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace SplatterPlots
{
    public partial class Form1 : Form
    {
        private Dictionary<ListViewItem, DataFile> m_Files = new Dictionary<ListViewItem, DataFile>();
        private Dictionary<ListViewItem, DataSeries> m_Series = new Dictionary<ListViewItem, DataSeries>();
        private SingleSplatterDialog m_SingleSplatterDialog = null;
        private SplamDialog m_SplamDialog = null;

        SingleSplatterDialog SingleSplatterDialog
        {
            get
            {
                if (m_SingleSplatterDialog == null||m_SingleSplatterDialog.IsDisposed)
                {
                    m_SingleSplatterDialog = new SingleSplatterDialog();
                }
                return m_SingleSplatterDialog;
            }
        }
        SplamDialog SplamDialog
        {
            get
            {
                if (m_SplamDialog == null || m_SplamDialog.IsDisposed)
                {
                    m_SplamDialog = new SplamDialog();
                    m_SplamDialog.SplatterSelection += new EventHandler(m_SplamDialog_SplatterSelection);
                }
                return m_SplamDialog;
            }
        }

        public Form1()
        {
            InitializeComponent();            
        }
        private void AddDataSeriesToList(List<DataSeries> series)
        {            
            foreach (var item in series)
            {
                ListViewItem listViewItem = new ListViewItem(item.Name);
                
                listViewItem.Name = item.Name;
                if (!listViewDataSeries.Items.ContainsKey(listViewItem.Name))
                {
                    m_Series[listViewItem] = item;
                    listViewDataSeries.Items.Add(listViewItem);
                }                    
            }
            SetColors();
        }

        private void SetColors()
        {
            var colors = ColorConv.pickIsoCols(74.0f, m_Series.Count, .5f, (float)Math.PI);
            var keys = m_Series.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].BackColor = colors[i];
                m_Series[keys[i]].Color = colors[i];
            }
        }
        private void loadFromCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = dialog.FileName;                    
                    var dataFile = new DataFile(path);
                    var listViewItem = new ListViewItem(dataFile.Name);
                    listViewItem.Name = dataFile.Name;
                    if (!listViewDataFiles.Items.ContainsKey(listViewItem.Name))
                    {
                        m_Files[listViewItem] = dataFile;
                        listViewDataFiles.Items.Add(listViewItem);
                    }                    
                }
                catch (Exception ex)
                {
                    int g = 0;
                }
            }
        }        
        private void buttonAdd_Click(object sender, EventArgs e)
        {            
            AddDialog dialog = new AddDialog();
            if (listViewDataFiles.SelectedItems.Count>0)
            {
                var item = listViewDataFiles.SelectedItems[0];
                var dataFile = m_Files[item];
                dialog.SetDataFile(dataFile);
                var res = dialog.ShowDialog(this);
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    var schema = dialog.GetSchema();
                    var list = dataFile.ConvertToDataSeries(schema);
                    AddDataSeriesToList(list);
                }
            }
        }

        private void listViewDataSeries_DoubleClick(object sender, EventArgs e)
        {
            var color = listViewDataSeries.SelectedItems[0].BackColor;
            colorDialog1.Color = color;
            var result = colorDialog1.ShowDialog();
            if (result== System.Windows.Forms.DialogResult.OK)
            {
                listViewDataSeries.SelectedItems[0].BackColor = colorDialog1.Color;
                m_Series[listViewDataSeries.SelectedItems[0]].Color = colorDialog1.Color;
                listViewDataSeries.SelectedItems.Clear();
                SingleSplatterDialog.Refresh();
                SplamDialog.Refresh();
            }
        }

        private void buttonSplat_Click(object sender, EventArgs e)
        {
            if (m_Series.Count > 0)
            {
                var model = new SplatterModel(m_Series.Values.ToList(), 0, 1);
                SingleSplatterDialog.SetModel(model);
                SingleSplatterDialog.Show(this);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewDataSeries.SelectedItems.Count>0)
            {
                if (SingleSplatterDialog!=null)
                {
                    SingleSplatterDialog.Close();
                }
                var list = new List<ListViewItem>(listViewDataSeries.SelectedItems.Cast<ListViewItem>());
                foreach (var item in list)
                {
                    m_Series.Remove(item);
                    listViewDataSeries.Items.Remove(item);
                }
                SetColors();
            }
        }

        private void buttonSplam_Click(object sender, EventArgs e)
        {
            if (m_Series.Count > 0)
            {
                var model = new SplamModel(m_Series.Values.ToList());
                SplamDialog.SetModel(model);
                SplamDialog.Show(this);
            }
        }

        private void m_SplamDialog_SplatterSelection(object sender, EventArgs e)
        {
            var view = sender as SplatterView;
            if (view != null)
            {
                SingleSplatterDialog.SetModel(view.Model);
                SingleSplatterDialog.Show();
                SingleSplatterDialog.BringToFront();
            }
        }
    }
}
