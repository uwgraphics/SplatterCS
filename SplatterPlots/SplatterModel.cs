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
        private Data m_Data;
        public void Init(Color col, Data data, string d0, string d1)
        {
            m_Data = data;
            enabled = true;
            color = col;
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

        public Color color{ get; private set; }
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
        public SplatterModel(List<Data> datas, List<Color> colors, int d0, int d1)
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
                sp.Init(colors[i], datas[i], dim0Name, dim1Name);
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
        public Dictionary<string,SeriesProjection> seriesList;

        public bool showAllPoints;
        public double xmax;
        public double xmin;
        public double ymax;
        public double ymin;

        public string dim0Name;
        public string dim1Name;
    }
}
