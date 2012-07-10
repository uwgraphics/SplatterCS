using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;

namespace SplatterPlots
{

    public class SeriesProjection
    {
        private DataSeries m_Data;
        public void Init(DataSeries data, string d0, string d1)
        {
            m_Data = data;
            enabled = true;            
            name = data.Name;
            dim0 = d0;
            dim1 = d1;
            //data.setIndeces(dim0, dim1);
            dataPoints = data.getXYValues(dim0,dim1);
            Random rand = new Random();
            dataZval = dataPoints.Select(vec=> (float)rand.NextDouble()).ToList();
        }

        public bool enabled { get; set; }

        public List<Vector2> dataPoints { get; private set; }
        public List<float> dataZval { get; private set; }

        public Color color { get { return m_Data.Color; } }
        public string name { get; private set; }

        public float xmax { get { return m_Data.ColumnData[dim0].Max; } }
        public float ymax { get { return m_Data.ColumnData[dim1].Max; } }
        public float xmin { get { return m_Data.ColumnData[dim0].Min; } }
        public float ymin { get { return m_Data.ColumnData[dim1].Min; } }

        public string dim0 { get; private set; }
        public string dim1 { get; private set; }
    }

    public class SplatterModel
    {
        public SplatterModel(List<DataSeries> datas, int d0, int d1)
        {
            xmax = float.MinValue;
            xmin = float.MaxValue;
            ymax = float.MinValue;
            ymin = float.MaxValue;
            showAllPoints = true;

            dim0Name = datas.First().ColumnNames[d0];
            dim1Name = datas.First().ColumnNames[d1];

            for (int i = 0; i < datas.Count; i++)
            {
                SeriesProjection sp = new SeriesProjection();
                sp.Init(datas[i], dim0Name, dim1Name);
                seriesList[datas[i].Name]=sp;

                xmax = Math.Max(xmax, sp.xmax);
                xmin = Math.Min(xmin, sp.xmin);
                ymax = Math.Max(ymax, sp.ymax);
                ymin = Math.Min(ymin, sp.ymin);
            }
        }

        public void SetEnabled(string group, bool value)
        {
            seriesList[group].enabled = value;
        }
        public Dictionary<string,SeriesProjection> seriesList=new Dictionary<string,SeriesProjection>();

        public bool showAllPoints;
        public float xmax;
        public float xmin;
        public float ymax;
        public float ymin;

        public string dim0Name;
        public string dim1Name;
    }
}
