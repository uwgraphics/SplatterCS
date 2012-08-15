using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplatterPlots
{
    public class OneVsAllModel
    {
        public List<SplatterModel> SplatList { get; private set; }
        public OneVsAllModel(List<List<DataSeries>> splats, string dim0, string dim1)
        {
            SplatList = new List<SplatterModel>();
            foreach (var datas in splats)
            {
                var sm = new SplatterModel(datas, dim0, dim1);
                SplatList.Add(sm);
            }
        }
    }
}
