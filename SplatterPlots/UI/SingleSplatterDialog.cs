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
    public partial class SingleSplatterDialog : Form
    {
        public event EventHandler PointSelection;
        SplatterModel m_Model;        

        private SelectionTable m_SelectionTable;
        private SelectionTable SelectionTableDialog
        {
            get
            {
                if (m_SelectionTable == null || m_SelectionTable.IsDisposed)
                {
                    m_SelectionTable = new SelectionTable();
                    m_SelectionTable.Text = this.Text;
                }
                return m_SelectionTable;
            }
        }

        public SingleSplatterDialog()
        {
            InitializeComponent();
            splatterView1.PointSelection += new EventHandler(splatterView1_PointSelection);
        }

        void splatterView1_PointSelection(object sender, EventArgs e)
        {
            if (PointSelection != null)
            {
                PointSelection(this, EventArgs.Empty);
            }
            else
            {
                Refresh();
                DataTable view = null;
                foreach (var series in m_Model.Series.Values)
                {
                    if (view == null)
                    {
                        var list = series.Data.GetSelectedRows();
                        if (list.Count() > 0)
                        {
                            view = list.CopyToDataTable();
                        }
                    }
                    else
                    {
                        series.Data.GetSelectedRows().CopyToDataTable(view, LoadOption.PreserveChanges);
                    }
                }
                SelectionTableDialog.SetDataView(view);
                if (!SelectionTableDialog.Visible)
                {
                    SelectionTableDialog.Show();
                }
                SelectionTableDialog.BringToFront();
            }            
        }
        public void SetModel(SplatterModel model)
        {
            m_Model = model;
            var list = new List<SplatterView>();
            View.setSplatPM(model);
            list.Add(splatterView1.View);
            sliderController1.SetView(list);
            GridViewSetup();
        }

        private void GridViewSetup()
        {            
            dataGridView1.AutoGenerateColumns = false;

            DataGridViewCheckBoxColumn enabledDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn member1DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member2DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member3DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member4DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member5DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member6DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member7DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn member8DataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();

            enabledDataGridViewCheckBoxColumn.DataPropertyName = "Enabled";
            enabledDataGridViewCheckBoxColumn.HeaderText = "Show";
            enabledDataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            enabledDataGridViewCheckBoxColumn.Name = "enabledDataGridViewCheckBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            nameDataGridViewTextBoxColumn.HeaderText = "Name";
            nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // member1DataGridViewCheckBoxColumn
            // 
            member1DataGridViewCheckBoxColumn.DataPropertyName = "Member1";
            member1DataGridViewCheckBoxColumn.HeaderText = "1";
            member1DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member1DataGridViewCheckBoxColumn.Name = "member1DataGridViewCheckBoxColumn";            
            // 
            // member2DataGridViewCheckBoxColumn
            // 
            member2DataGridViewCheckBoxColumn.DataPropertyName = "Member2";
            member2DataGridViewCheckBoxColumn.HeaderText = "2";
            member2DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member2DataGridViewCheckBoxColumn.Name = "member2DataGridViewCheckBoxColumn";
            // 
            // member3DataGridViewCheckBoxColumn
            // 
            member3DataGridViewCheckBoxColumn.DataPropertyName = "Member3";
            member3DataGridViewCheckBoxColumn.HeaderText = "3";
            member3DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member3DataGridViewCheckBoxColumn.Name = "member3DataGridViewCheckBoxColumn";
            // 
            // member4DataGridViewCheckBoxColumn
            // 
            member4DataGridViewCheckBoxColumn.DataPropertyName = "Member4";
            member4DataGridViewCheckBoxColumn.HeaderText = "4";
            member4DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member4DataGridViewCheckBoxColumn.Name = "member4DataGridViewCheckBoxColumn";
            // 
            // member5DataGridViewCheckBoxColumn
            // 
            member5DataGridViewCheckBoxColumn.DataPropertyName = "Member5";
            member5DataGridViewCheckBoxColumn.HeaderText = "5";
            member5DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member5DataGridViewCheckBoxColumn.Name = "member5DataGridViewCheckBoxColumn";
            // 
            // member6DataGridViewCheckBoxColumn
            // 
            member6DataGridViewCheckBoxColumn.DataPropertyName = "Member6";
            member6DataGridViewCheckBoxColumn.HeaderText = "6";
            member6DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member6DataGridViewCheckBoxColumn.Name = "member6DataGridViewCheckBoxColumn";
            // 
            // member7DataGridViewCheckBoxColumn
            // 
            member7DataGridViewCheckBoxColumn.DataPropertyName = "Member7";
            member7DataGridViewCheckBoxColumn.HeaderText = "7";
            member7DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member7DataGridViewCheckBoxColumn.Name = "member7DataGridViewCheckBoxColumn";
            // 
            // member8DataGridViewCheckBoxColumn
            // 
            member8DataGridViewCheckBoxColumn.DataPropertyName = "Member8";
            member8DataGridViewCheckBoxColumn.HeaderText = "8";
            member8DataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            member8DataGridViewCheckBoxColumn.Name = "member8DataGridViewCheckBoxColumn";

            dataGridView1.Columns.Add(enabledDataGridViewCheckBoxColumn);
            dataGridView1.Columns.Add(nameDataGridViewTextBoxColumn);

            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();
            columns.Add(member1DataGridViewCheckBoxColumn);
            columns.Add(member2DataGridViewCheckBoxColumn);
            columns.Add(member3DataGridViewCheckBoxColumn);
            columns.Add(member4DataGridViewCheckBoxColumn);
            columns.Add(member5DataGridViewCheckBoxColumn);
            columns.Add(member6DataGridViewCheckBoxColumn);
            columns.Add(member7DataGridViewCheckBoxColumn);
            columns.Add(member8DataGridViewCheckBoxColumn);
            for (int i = 0; i < m_Model.NumberOfGroups; i++)
            {
                columns[i].DefaultCellStyle.BackColor = m_Model.Groups[columns[i].DataPropertyName].Color;
                dataGridView1.Columns.Add(columns[i]);
            }

            dataGridView1.DataSource = m_Model.SeriesMemberships;
        }
        
        public SplatterView View { get { return splatterView1.View; } }
        public SliderController Slider { get { return sliderController1; } }

        private void DisableMouseClicks()
        {
            if (this.Filter == null)
            {
                this.Filter = new MouseClickMessageFilter();
                Application.AddMessageFilter(this.Filter);
            }
        }

        private void EnableMouseClicks()
        {
            if ((this.Filter != null))
            {
                Application.RemoveMessageFilter(this.Filter);
                this.Filter = null;
            }
        }

        private MouseClickMessageFilter Filter;

        private const int LButtonDown = 0x201;
        private const int LButtonUp = 0x202;
        private const int LButtonDoubleClick = 0x203;

        private class MouseClickMessageFilter : IMessageFilter
        {

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                switch (m.Msg)
                {
                    case LButtonDown:
                    case LButtonUp:
                    case LButtonDoubleClick:
                        return true;
                    default:
                        return false;
                }                
            }
        }
        
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell !=null && dataGridView1.CurrentCell is DataGridViewCheckBoxCell)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var color = dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor;
            var colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            colorDialog.SolidColorOnly = false;
            colorDialog.AnyColor = true;
            colorDialog.Color = color;

            var result = colorDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = colorDialog.Color;
                m_Model.Groups[dataGridView1.Columns[e.ColumnIndex].DataPropertyName].Color = colorDialog.Color;
                Refresh();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (m_SelectionTable != null)
            {
                m_SelectionTable.Close();
            }
            base.OnClosing(e);
        }
    }
}
