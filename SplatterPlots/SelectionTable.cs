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
    public partial class SelectionTable : Form
    {
        public SelectionTable()
        {
            InitializeComponent();
        }
        public void SetDataView(DataTable dataview)
        {
            dataGridView1.DataSource = dataview;
        }
    }
}
