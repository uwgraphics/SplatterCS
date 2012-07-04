using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SplatterPlots
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadFromCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data data = new Data();
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    string path = dialog.FileName;
                    data.load(path);
                    List<Data> datas = new List<Data>();
                    datas.Add(data);
                    List<Color> colors = new List<Color>();
                    colors.Add(Color.BurlyWood);
                    SplatterModel pm = new SplatterModel(datas,colors,0,1);
                    SplatterModel pm12 = new SplatterModel(datas, colors, 0, 1);
                    sw.Stop();
                    double ms = sw.ElapsedMilliseconds;
                    Console.WriteLine(ms);
                    //splatterView1.setSplatPM(pm);
                    //splatterView1.setBBox(pm.xmin, pm.ymin, pm.xmax, pm.ymax);

                    splatterView2.setSplatPM(pm12);
                    splatterView2.setBBox(pm12.xmin, pm12.ymin, pm12.xmax, pm12.ymax);
                }
                catch (Exception ex)
                {
                    int g = 0;
                }
            }
        }
    }
}
