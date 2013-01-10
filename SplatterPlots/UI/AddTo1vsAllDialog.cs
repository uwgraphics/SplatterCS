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
    public partial class AddTo1vsAllDialog : Form
    {
        public AddTo1vsAllDialog()
        {
            InitializeComponent();
        }
        public void SetDataFile(DataFile dataFile)
        {            
            foreach (var dim in dataFile.ColumnNames)
            {
                if (dataFile.IsNumeric(dim))
                {
                    comboBoxHorizontal.Items.Add(dim);
                    comboBoxVertical.Items.Add(dim);
                }
                comboBoxGroup.Items.Add(dim);
            }
        }
        public string GroupBy
        {
            get
            {
                return comboBoxGroup.SelectedItem as string;
            }
        }
        public string HorizontalDim
        {
            get
            {
                return comboBoxHorizontal.SelectedItem as string;
            }
        }
        public string VerticalDim
        {
            get
            {
                return comboBoxVertical.SelectedItem as string;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // test for validity
            bool valid = true;
            if (string.IsNullOrEmpty(GroupBy)||
                string.IsNullOrEmpty(HorizontalDim)||
                string.IsNullOrEmpty(VerticalDim))
            {
                valid = false;
            }else if (GroupBy.Equals(HorizontalDim)||
                        GroupBy.Equals(VerticalDim)||
                        HorizontalDim.Equals(VerticalDim))
            {
                valid = false;
            }
            if (!valid)
            {
                MessageBox.Show("Please use different values for each choice", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = System.Windows.Forms.DialogResult.None;
            }            
        }
    }
}
