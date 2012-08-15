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
        OneVsAllModel m_Model;
        List<SplatterView> m_Views = new List<SplatterView>();
        public event EventHandler SplatterSelection;
        public event EventHandler PointSelection;
        public OneVersusAllDialog()
        {
            InitializeComponent();
        }
        public void SetModel(OneVsAllModel model)
        {
            panel1.Controls.Clear();
            m_Model = model;
            m_Views.Clear();

            //how to best utilize the space?? look for the closest perfect square
            int num = (int)(Math.Floor(Math.Sqrt(m_Model.SplatList.Count))) + 1;
            float sz = (float)(panel1.Width) / (num);
            int szi = (int)sz;
            
            for (int i = 0,k=0; i < num&&k<m_Model.SplatList.Count; i++)
            {
                for (int j = 0; j < num&&k<m_Model.SplatList.Count; j++,k++)
                {
                    var view = new SplatterviewContainer();
                    view.Name = "oneVsAll" + k;
                    view.Size = new Size(szi - 25, szi - 25);
                    view.Location = new Point(i * szi + 25, j * szi);
                    view.PointSelection += new EventHandler(view_PointSelection);
                    view.DoubleClick += new EventHandler(view_DoubleClick);
                    view.View.setSplatPM(m_Model.SplatList[k]);
                    m_Views.Add(view.View);
                    panel1.Controls.Add(view);
                }
            }
            sliderController1.SetView(m_Views);
        }

        void view_PointSelection(object sender, EventArgs e)
        {
            if (PointSelection != null)
            {
                PointSelection(this, EventArgs.Empty);
            }
        }

        void view_DoubleClick(object sender, EventArgs e)
        {
            var view = sender as SplatterView;
            if (view != null && SplatterSelection != null)
            {
                SplatterSelection(view, EventArgs.Empty);
            }
        }
    }
}
