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
        private static int SplatterCount = 0;
        private static int SplamCount = 0;
        private static int OneVRestCount = 0;

        public Form1()
        {
            InitializeComponent();            
        }
        private void button1vsAll_Click(object sender, EventArgs e)
        {
            var addDialog = new AddTo1vsAllDialog();
            addDialog.Text = "Show one vs rest";
            if (listViewDataFiles.SelectedItems.Count > 0)
            {
                var oneVersusAllDialog = new OneVersusAllDialog();                
                var item = listViewDataFiles.SelectedItems[0];
                var dataFile = m_Files[item];
                oneVersusAllDialog.Text = string.Format("One versus rest({0}): {1}", OneVRestCount++, dataFile.Name);
                addDialog.SetDataFile(dataFile, false);
                var res = addDialog.ShowDialog(this);
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    var lists = dataFile.ConvertToOneVsAllDataSeries(addDialog.GroupBy, addDialog.HorizontalDim, addDialog.VerticalDim, 10);
                    OneVsAllModel model = new OneVsAllModel(lists, addDialog.HorizontalDim, addDialog.VerticalDim);
                    oneVersusAllDialog.SetModel(model);
                    oneVersusAllDialog.Show(this);                    
                    oneVersusAllDialog.BringToFront();
                }
            }
        }
        private void buttonSplatter_Click(object sender, EventArgs e)
        {
            var dialog = new AddTo1vsAllDialog();
            dialog.Text = "Show single splatterplot";
            if (listViewDataFiles.SelectedItems.Count > 0)
            {                
                var item = listViewDataFiles.SelectedItems[0];
                var dataFile = m_Files[item];
                dialog.SetDataFile(dataFile, true);
                var res = dialog.ShowDialog(this);
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    var schema = new DataFileSchema(dataFile);
                    schema.GroupBy = dialog.GroupBy;
                    var list = dataFile.ConvertToDataSeries(schema, -1);

                    SplatterModel model = new SplatterModel(list, dialog.HorizontalDim, dialog.VerticalDim, true);

                    var splatterDialog = new SingleSplatterDialog();
                    splatterDialog.Text = string.Format("Splatterplot ({0}): {1}", SplatterCount++, dataFile.Name);
                    splatterDialog.SetModel(model);
                    splatterDialog.Show();
                    splatterDialog.BringToFront();                    
                }
            }
        }
        private void buttonSplam_Click(object sender, EventArgs e)
        {
            AddDialog dialog = new AddDialog();
            dialog.Text = "Show splatterplot matrix";
            if (listViewDataFiles.SelectedItems.Count > 0)
            {
                var item = listViewDataFiles.SelectedItems[0];
                var dataFile = m_Files[item];
                dialog.SetDataFile(dataFile);
                var res = dialog.ShowDialog(this);
                if (res == System.Windows.Forms.DialogResult.OK)
                {                    
                    var schema = dialog.GetSchema();
                    var list = dataFile.ConvertToDataSeries(schema, -1);
                    var model = new SplamModel(list);
                    var splamDialog = new SplamDialog();
                    splamDialog.Text = string.Format("Splatterplot matrix({0}): {1}", SplamCount++, dataFile.Name);
                    splamDialog.SetModel(model);
                    splamDialog.Show();
                    splamDialog.BringToFront();
                }
            }
        }

        private void buttonLoadFile_Click(object sender, EventArgs e)
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
    }
}
