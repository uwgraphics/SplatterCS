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
        SplatterModel m_Model;
        Dictionary<ListViewItem, SeriesProjection> m_Series = new Dictionary<ListViewItem, SeriesProjection>();

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
            m_Model = model;
            var list = new List<SplatterView>();
            View.setSplatPM(model);
            list.Add(splatterView1.View);
            sliderController1.SetView(list);

            foreach (var series in m_Model.Series.Values)
            {
                ListViewItem item = new ListViewItem(series.Name);
                item.Name = series.Name;
                item.BackColor = series.Color;
                item.Checked = true;
                if (!listView1.Items.ContainsKey(item.Name))
                {
                    m_Series[item] = series;
                    listView1.Items.Add(item);
                }
            }
        }
        public SplatterView View { get { return splatterView1.View; } }
        public SliderController Slider { get { return sliderController1; } }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

            //DisableMouseClicks();
            var series = m_Series[e.Item];
            series.Enabled = e.Item.Checked;
            View.Refresh();
            //EnableMouseClicks();
        }

        private void DisableMouseClicks()
        {
            if (this.Filter == null)
            {
                this.Filter = new MouseClickMessageFilter();
                Application.AddMessageFilter(this.Filter);
            }
        }

        private void EnableMouseClicks()
        {
            if ((this.Filter != null))
            {
                Application.RemoveMessageFilter(this.Filter);
                this.Filter = null;
            }
        }

        private MouseClickMessageFilter Filter;

        private const int LButtonDown = 0x201;
        private const int LButtonUp = 0x202;
        private const int LButtonDoubleClick = 0x203;

        private class MouseClickMessageFilter : IMessageFilter
        {

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                switch (m.Msg)
                {
                    case LButtonDown:
                    case LButtonUp:
                    case LButtonDoubleClick:
                        return true;
                    default:
                        return false;
                }                
            }
        } 
    }
}
