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
    public partial class SplamDialog : Form
    {
        SplamModel m_Model;
        List<SplatterView> m_Views = new List<SplatterView>();

        public event EventHandler SplatterSelection;

        public SplamDialog()
        {
            InitializeComponent();
        }

        public void SetModel(SplamModel model)
        {
            panel1.Controls.Clear();
            m_Model = model;
            m_Views.Clear();            
            float sz = (float)(panel1.Width) / (m_Model.numDim - 1.0f);
            int szi = (int)sz;
            for (int i = 0; i < m_Model.SplatList.Count; i++)
            {                
                var view = new SplatterView();
                int I = m_Model.Iindex[i];
                int J = m_Model.Jindex[i] - 1;
                view.Name = "splom" + I + J;
                view.Size = new System.Drawing.Size(szi - 25, szi - 25);
                view.Location = new Point(I * szi + 25, J * szi);
                m_Views.Add(view);
                view.DoubleClick += new EventHandler(view_DoubleClick);
                view.setSplatPM(m_Model.SplatList[i]);
                panel1.Controls.Add(view);

                //QLabel *label = new QLabel(ui.splomArea);
                //label->setText(QString::fromStdString(splom->splatList[i]->dim0Name));
                //label->setAlignment(Qt::Alignment(Qt::AlignmentFlag::AlignHCenter));
                //label->setGeometry(QRect(I*sz+25, J*sz + (sz-25), sz-25, 20));

                //MyLabel *label2 = new MyLabel(ui.splomArea);
                //label2->setText(QString::fromStdString(splom->splatList[i]->dim1Name));
                //label2->rotateText(-90+360);
                //label2->setAlignment(Qt::Alignment(Qt::AlignmentFlag::AlignCenter));		
                //label2->setGeometry(QRect(I*sz, J*sz, 20, sz-25));
            }

            sliderController1.SetView(m_Views);
        }

        void view_DoubleClick(object sender, EventArgs e)
        {
            var view = sender as SplatterView;
            if (view != null && SplatterSelection!=null)
            {
                SplatterSelection(view, EventArgs.Empty);
            }
        }
    }
}
