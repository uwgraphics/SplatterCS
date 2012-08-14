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
        }
        public void SetModel(SplatterModel model)
        {
            var list = new List<SplatterView>();
            splatterView1.View.setSplatPM(model);
            list.Add(splatterView1.View);
            sliderController1.SetView(list);
        }
    }
}
