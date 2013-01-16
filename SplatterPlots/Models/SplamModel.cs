using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SplatterPlots
{
    public class SplamModel
    {
        public int numDim { get; private set; }
        private List<string> dimNames;        
        public List<SplatterModel> SplatList { get; private set; }
        public List<int> Iindex{ get; private set; }
        public List<int> Jindex{ get; private set; }
        public List<DataSeries> Series { get; private set; }

        public SplamModel(List<DataSeries> datas)
        {
            Series = new List<DataSeries>(datas);
            var colors = ColorConv.pickIsoCols(74.0f, Series.Count, .5f, (float)Math.PI);
            for (int i = 0; i < Series.Count; i++)
            {
                Series[i].Color = colors[i];
            }
            SplatList = new List<SplatterModel>();
            Iindex = new List<int>();
            Jindex = new List<int>();
            numDim = datas.First().ColumnNames.Count;            
            dimNames = datas.First().ColumnNames;


            for (int i = 0; i < numDim - 1; i++)
            {
                for (int j = i + 1; j < numDim; j++)
                {
                    var spm = new SplatterModel(Series, i, j, false);
                    SplatList.Add(spm);

                    Iindex.Add(i);
                    Jindex.Add(j);
                }
            }            
        }
    }
}
