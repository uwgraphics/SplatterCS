using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Math;

namespace SplatterPlots
{
    class SplatterModel
    {
    }
    class SeriesProjection
    {

        public void Init(Color col, Data data, int dim0, int dim1);
        public void BuildZVals();
        public bool compX(int i, int j);
        public bool compY(int i, int j);
        public bool enabled;

        public List<Vector2> dataPoints;
        public List<float> dataZval;

        public Color color;
        public string name;

        public double xmax;
        public double ymax;
        public double xmin;
        public double ymin;

        public int dim0;
        public int dim1;
    }

    class SplatterModel
    {


        public SplatterModel(List<Data> datas, List<Color> colors, int d0, int d1)
        {
            xmax = float.MinValue;
            xmin = float.MaxValue;
            ymax = float.MinValue;
            ymin = float.MaxValue;
            showAllPoints = true;

            dim0Name = datas[0].ColumnNames[d0];
            dim1Name = datas[0].ColumnNames[d1];

            for (int i = 0; i < datas.Count; i++)
            {
                SeriesProjection sp = new SeriesProjection();
                sp.Init(colors[i], datas[i], d0, d1);
                seriesList.Add(sp);

                xmax = Math.Max(xmax, sp.xmax);
                xmin = Math.Min(xmin, sp.xmin);
                ymax = Math.Max(ymax, sp.ymax);
                ymin = Math.Min(ymin, sp.ymin);
            }
        }

        public void SetEnabled(string group, bool value);
        public List<SeriesProjection> seriesList;

        public bool showAllPoints;
        public double xmax;
        public double xmin;
        public double ymax;
        public double ymin;

        public string dim0Name;
        public string dim1Name;
    }
}
