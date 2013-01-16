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
    public partial class AddDialog : Form
    {
        private DataFileSchema m_Schema;
        private List<AddDialogRow> m_Rows = new List<AddDialogRow>();
        public AddDialog()
        {
            InitializeComponent();
        }
        public void SetDataFile(DataFile dataFile)
        {
            m_Schema = new DataFileSchema(dataFile);
            comboBox1.SelectedIndex = 0;
            foreach (var name in m_Schema.ColumnNames)
            {
                AddDialogRow row = new AddDialogRow();
                m_Rows.Add(row);
                row.SetColumnData(name, m_Schema.ColumnNumericMap[name]);
                flowLayoutPanel1.Controls.Add(row);
                comboBox1.Items.Add(name);
            }
            if (m_Rows.Count > 15)
            {
                m_Rows.ForEach(r => r.SetToIgnore());
            }
        }
        public DataFileSchema GetSchema()
        {
            foreach (var row in m_Rows)
            {
                m_Schema.ColumnNumericMap[row.ColumName] = row.IsNumeric;
            }
            m_Schema.GroupBy = comboBox1.SelectedItem as string;
            return m_Schema;
        }

        private void buttonIgnoreAll_Click(object sender, EventArgs e)
        {
            m_Rows.ForEach(r => r.SetToIgnore());
        }
    }
}
