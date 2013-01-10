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
    public partial class SliderController : UserControl
    {
        List<SplatterView> m_views;
        private bool allowUpdate = true;
        public SliderController()
        {
            InitializeComponent();
            if (Program.Runtime)
            {
                tableLayoutPanel2.Visible = false;
                SetAdvancedLabel();
            }            
        }

        private void SetAdvancedLabel()
        {
            if (tableLayoutPanel2.Visible)
            {
                buttonAdvanced.Text = "Advanced^";
            }
            else
            {
                buttonAdvanced.Text = "Advanced>>";
            }
            
        }
        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            base.OnLoad(e);
        }

        
        public void SetView(List<SplatterView> views)
        {
            m_views = views;
            allowUpdate = false;
            trackBarBandwith.Value = (int)m_views.First().Bandwidth;
            trackBarContourThresh.Value = (int)(m_views.First().ContourThreshold * 100);
            trackBarDensityThresh.Value = (int)(m_views.First().DensityThreshold * 100);
            trackBarChromaF.Value = (int)(m_views.First().ChromaF * 100);
            trackBarLightnessF.Value = (int)(m_views.First().LightnessF * 100);
            trackBarClutterRad.Value = (int)m_views.First().ClutterWindow;
            trackBarStripeWidth.Value = (int)m_views.First().StripeWidth;
            trackBarStripePeriod.Value = (int)m_views.First().StripePeriod;
            trackBarXScaling.Value = (int)(10 * Math.Log(m_views.First().ScaleFactorX, 1.0 / 9.0));
            trackBarYScaling.Value = (int)(10 * Math.Log(m_views.First().ScaleFactorY, 1.0 / 9.0));
            allowUpdate = true;
            buttonSplatter_Click(this, EventArgs.Empty);
        }
        public void SetAsScatter()
        {
            trackBarContourThresh.Value = 100;
            trackBarDensityThresh.Value = 100;
            trackBarClutterRad.Value = 0;
        }
        public void SetAsSplatter()
        {
            trackBarContourThresh.Value = 50;
            trackBarClutterRad.Value = 35;
            trackBarDensityThresh.Value = 0;
            trackBarBandwith.Value = 15;
            trackBarLightnessF.Value = 90;
            trackBarStripeWidth.Value = 0;
        }
        private void trackBarBandwith_ValueChanged(object sender, EventArgs e)
        {
            labelBandwidth.Text = string.Format("{0:G}", trackBarBandwith.Value);
            labelBandwidth.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.Bandwidth = trackBarBandwith.Value);
        }
        private void trackBarContourThresh_ValueChanged(object sender, EventArgs e)
        {
            float val = trackBarContourThresh.Value / 100.0f;
            labelContourThresh.Text = string.Format("{0:G}", val);
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.ContourThreshold = val);
        }
        private void trackBarDensityThresh_ValueChanged(object sender, EventArgs e)
        {
            float val = trackBarDensityThresh.Value / 100.0f;
            labelDensityThresh.Text = string.Format("{0:G}", val);
            labelDensityThresh.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.DensityThreshold = val);
        }
        private void trackBarChromaF_ValueChanged(object sender, EventArgs e)
        {
            float val = trackBarChromaF.Value / 100.0f;
            labelChromaF.Text = string.Format("{0:G}", val);
            labelChromaF.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.ChromaF = val);
        }
        private void trackBarLightnessF_ValueChanged(object sender, EventArgs e)
        {
            float val = trackBarLightnessF.Value / 100.0f;
            labelLightnessF.Text = string.Format("{0:G}", val);
            labelLightnessF.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.LightnessF = val);
        }
        private void trackBarClutterRad_ValueChanged(object sender, EventArgs e)
        {
            int val = trackBarClutterRad.Value;
            labelClutterRad.Text = string.Format("{0:G}", val);
            labelClutterRad.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.ClutterWindow = val);
        }
        private void trackBarStripeWidth_ValueChanged(object sender, EventArgs e)
        {
            int val = trackBarStripeWidth.Value;
            labelStripeWidth.Text = string.Format("{0:G}", val);
            labelStripeWidth.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.StripeWidth = val);
        }
        private void trackBarStripePeriod_ValueChanged(object sender, EventArgs e)
        {
            int val = trackBarStripePeriod.Value;
            labelStripePeriod.Text = string.Format("{0:G}", val);
            labelStripePeriod.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.StripePeriod = val);
        }
        private void trackBarXScaling_ValueChanged(object sender, EventArgs e)
        {
            float val = trackBarXScaling.Value / 10.0f;
            labelXScaling.Text = string.Format("{0:G}", val);
            labelXScaling.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.ScaleFactorX = (float)Math.Pow(1.0 / 9.0, val));
        }
        private void trackBarYScaling_ValueChanged(object sender, EventArgs e)
        {
            float val = trackBarYScaling.Value / 10.0f;
            labelYScaling.Text = string.Format("{0:G}", val);
            labelYScaling.Refresh();
            if (m_views == null || !allowUpdate)
            {
                return;
            }
            m_views.ForEach(view => view.ScaleFactorY = (float)Math.Pow(1.0 / 9.0, val));
        }

        private void buttonSplatter_Click(object sender, EventArgs e)
        {
            SetAsSplatter();
        }       

        private void buttonKDE_Click(object sender, EventArgs e)
        {
            trackBarContourThresh.Value = 100;
            trackBarClutterRad.Value = 10;
            trackBarDensityThresh.Value = 1;
            trackBarBandwith.Value = 12;
        }

        private void radioButtonGlobal_CheckedChanged(object sender, EventArgs e)
        {
            m_views.ForEach(view => view.MaxMode = radioButtonGlobal.Checked ? MaxMode.Global : MaxMode.PerGroup);
        }

        private void checkBoxGrid_CheckedChanged(object sender, EventArgs e)
        {
            m_views.ForEach(view => view.ShowGrid = checkBoxGrid.Checked);
        }

        private void buttonScatter_Click(object sender, EventArgs e)
        {
            SetAsScatter();
        }

        

        private void buttonScreenShot_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog()== DialogResult.OK)
            {
                m_views.ForEach(view => view.saveScreenShot(dialog.FileName));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel2.Visible = !tableLayoutPanel2.Visible;
            SetAdvancedLabel();
        }
    }
}
