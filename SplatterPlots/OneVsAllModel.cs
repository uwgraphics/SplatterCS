using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SplatterPlots
{
    public class OneVsAllModel
    {
        public List<SplatterModel> SplatList { get; private set; }
        public List<DataSeries> Groups { get; private set; }
        public List<DataSeries> Others { get; private set; }
        public string OthersName { get; private set; }
        public Color OtherColor { get; set; }
        public OneVsAllModel(List<List<DataSeries>> splats, string dim0, string dim1)
        {
            SplatList = new List<SplatterModel>();
            Groups = new List<DataSeries>();
            Others = new List<DataSeries>();
            OthersName = splats[0][1].Name;

            var colors = ColorConv.pickIsoCols(74.0f, splats.Count, .5f, (float)Math.PI);
            OtherColor = Color.Beige;
            
            int i = 0;
            foreach (var datas in splats)
            {
                Groups.Add(datas[0]);
                Others.Add(datas[1]);
                datas[0].Color = colors[i];
                datas[1].Color = OtherColor;
                var sm = new SplatterModel(datas, dim0, dim1);
                SplatList.Add(sm);
                i++;
            }
        }
        public void SetOTherColor(Color color)
        {
            OtherColor = color;
            Others.ForEach(o => o.Color = OtherColor);
        }
    }
}
