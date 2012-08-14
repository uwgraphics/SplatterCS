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
        public event EventHandler SplatterSelection;
        public event EventHandler PointSelection;
        public OneVersusAllDialog()
        {
            InitializeComponent();
        }
    }
}
