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
        public SingleSplatterDialog()
        {
            InitializeComponent();
        }
        public void SetModel(SplatterModel model)
        {
            var list = new List<SplatterView>();
            splatterView1.setSplatPM(model);
            list.Add(splatterView1);
            sliderController1.SetView(list);
        }
    }
}
