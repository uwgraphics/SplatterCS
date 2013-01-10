using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Security;
using System.Diagnostics;

namespace SplatterPlots
{
    public partial class Form1 : Form
    {
        private Dictionary<ListViewItem, DataFile> m_Files = new Dictionary<ListViewItem, DataFile>();
        private Dictionary<ListViewItem, DataSeries> m_Series = new Dictionary<ListViewItem, DataSeries>();        
        private SingleSplatterDialog m_SingleSplatterDialog = null;
        private SplamDialog m_SplamDialog = null;
        private SelectionTable m_SelectionDialog = null;
        private OneVersusAllDialog m_OneVersusAllDialog = null;

        SingleSplatterDialog SingleSplatterDialog
        {
            get
            {
                if (m_SingleSplatterDialog == null||m_SingleSplatterDialog.IsDisposed)
                {
                    m_SingleSplatterDialog = new SingleSplatterDialog();
                    m_SingleSplatterDialog.PointSelection += new EventHandler(SplatterDialogs_PointSelection);
                }
                return m_SingleSplatterDialog;
            }
        }
        SelectionTable SelectionTableDialog
        {
            get
            {
                if (m_SelectionDialog==null||m_SelectionDialog.IsDisposed)
                {
                    m_SelectionDialog = new SelectionTable();                    
                }
                return m_SelectionDialog;
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
                    m_SplamDialog.PointSelection+= new EventHandler(SplatterDialogs_PointSelection);
                }
                return m_SplamDialog;
            }
        }
        OneVersusAllDialog OneVersusAllDialog
        {
            get
            {
                if (m_OneVersusAllDialog == null || m_OneVersusAllDialog.IsDisposed)
                {
                    m_OneVersusAllDialog = new OneVersusAllDialog();                    
                }
                return m_OneVersusAllDialog;
            }
        }
        void SplatterDialogs_PointSelection(object sender, EventArgs e)
        {
            if (m_SingleSplatterDialog != null)
            {
                m_SingleSplatterDialog.Refresh();
            }
            if (m_SplamDialog != null)
            {
                m_SplamDialog.Refresh();
            }
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
#if DEBUG
                try
                {
#endif
                    string path = dialog.FileName;                    
                    var dataFile = new DataFile(path);
                    var listViewItem = new ListViewItem(dataFile.Name);
                    listViewItem.Name = dataFile.Name;
                    if (!listViewDataFiles.Items.ContainsKey(listViewItem.Name))
                    {
                        m_Files[listViewItem] = dataFile;
                        listViewDataFiles.Items.Add(listViewItem);
                    }
#if DEBUG
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
#endif
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
                    if (SingleSplatterDialog != null)
                    {
                        SingleSplatterDialog.Close();
                    }
                    if (SplamDialog != null)
                    {
                        SplamDialog.Close();
                    }
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
                if (!SingleSplatterDialog.Visible)
                {
                    SingleSplatterDialog.Show(this);
                }
                SingleSplatterDialog.BringToFront();                
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
                if (SplamDialog != null)
                {
                    SplamDialog.Close();
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
                if (!SplamDialog.Visible)
                {
                    SplamDialog.Show(this);
                }
                SplamDialog.BringToFront();
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

        private void button1vsAll_Click(object sender, EventArgs e)
        {
            var dialog = new AddTo1vsAllDialog();
            if (listViewDataFiles.SelectedItems.Count > 0)
            {
                if (m_OneVersusAllDialog != null)
                {
                    m_OneVersusAllDialog.Close();
                }
                var item = listViewDataFiles.SelectedItems[0];
                var dataFile = m_Files[item];
                dialog.SetDataFile(dataFile);
                var res = dialog.ShowDialog(this);
                if (res == System.Windows.Forms.DialogResult.OK)
                {                    
                    var lists = dataFile.ConvertToOneVsAllDataSeries(dialog.GroupBy, dialog.HorizontalDim, dialog.VerticalDim);
                    OneVsAllModel model = new OneVsAllModel(lists, dialog.HorizontalDim, dialog.VerticalDim);
                    OneVersusAllDialog.SetModel(model);
                    if (!OneVersusAllDialog.Visible)
                    {
                        OneVersusAllDialog.Show(this);
                    }
                    OneVersusAllDialog.BringToFront();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            float[] bandwidths = { 1,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150};
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var files = Directory.EnumerateFiles(dialog.SelectedPath, "*.txt");
                foreach (var file in files)
                {
                    var datafile = new DataFile(file);
                    DataFileSchema schema = new DataFileSchema(datafile);
                    var splat = new SingleSplatterDialog();
                    schema.GroupBy = "G";
                    schema.ColumnNumericMap["G"] = false;
                    var dslist = datafile.ConvertToDataSeries(schema);
                    var model = new SplatterModel(dslist, 0, 1);
                    var colors = ColorConv.pickIsoCols(74.0f, dslist.Count, .5f, (float)Math.PI);
                    for (int i = 0; i < colors.Count; i++)
                    {
                        dslist[i].Color = colors[i];
                    }

                    splat.SetModel(model);
                    splat.View.Bandwidth = 1;
                    splat.Show(this);
                    splat.BringToFront();
                    foreach (var band in bandwidths)
                    {
                        splat.View.Bandwidth = band;

                        for (int i = 0; i < 100; i++)
                        {
                            splat.Refresh();
                        }
                    }
                    splat.Close();
                    splat.Dispose();
                }
            }
        }
        private void buttonClutter_click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            int I = 0;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var files = Directory.EnumerateFiles(dialog.SelectedPath, "*.txt");
                foreach (var file in files)
                {
                    var datafile = new DataFile(file);
                    DataFileSchema schema = new DataFileSchema(datafile);
                    var splat = new SingleSplatterDialog();
                    schema.GroupBy = "G";
                    schema.ColumnNumericMap["G"] = false;
                    var dslist = datafile.ConvertToDataSeries(schema);
                    var model = new SplatterModel(dslist, 0, 1);
                    var colors = ColorConv.pickIsoCols(74.0f, dslist.Count, .5f, (float)Math.PI);
                    for (int i = 0; i < colors.Count; i++)
                    {
                        dslist[i].Color = colors[i];
                    }

                    splat.SetModel(model);
                    //splat.View.Bandwidth = 1;
                    splat.View.ShowGrid = false;
                    splat.View.MaxMode = MaxMode.PerGroup;
                    splat.Show(this);
                    splat.BringToFront();
                    while (splat.View.NumberOfPointsInView() > 100)
                    {
                        string name = string.Format("{0:D8}",I++);
                        splat.Slider.SetAsScatter();
                        splat.View.Refresh();
                        splat.View.saveScreenShot("scatter" + name);
                        splat.Slider.SetAsSplatter();
                        splat.View.Refresh();
                        splat.View.saveScreenShot("splatter" + name);
                        splat.View.ZoomIn();
                    }
                    splat.Close();
                    splat.Dispose();
                }
            }
        }
    }
}
