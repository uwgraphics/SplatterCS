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
    public partial class AddDialogRow : UserControl
    {
        public AddDialogRow()
        {
            InitializeComponent();
        }
        public void SetColumnData(string columnName, bool numeric)
        {
            label1.Text = columnName;
            ColumName = columnName;
            if (numeric)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }
        public void SetToIgnore() { comboBox1.SelectedIndex = 1; }
        public string ColumName { get; private set; }
        public bool IsNumeric { get { return comboBox1.SelectedIndex == 0; } }
    }
}
