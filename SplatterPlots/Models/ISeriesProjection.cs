using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace SplatterPlots
{
    public interface ISeriesProjection
    {
        System.Drawing.Color Color { get; set; }
        System.Collections.Generic.List<ProjectedPoint> dataPoints { get; }
        
        bool Enabled { get; set; }
        int[] Histogram { get; set; }
        string Name { get; }
        float Xmax { get; }
        float Xmin { get; }
        float Ymax { get; }
        float Ymin { get; }
    }
    public class SeriesProjection : ISeriesProjection
    {
        private DataSeries m_Data;
        public void Init(DataSeries data, string d0, string d1)
        {
            m_Data = data;
            Enabled = true;
            Name = data.Name;
            m_Dim0 = d0;
            m_Dim1 = d1;
            
            dataPoints = data.getXYValues(this,m_Dim0, m_Dim1).ToList();
            for (int i = 0; i < dataPoints.Count; i++)
            {
                dataPoints[i].Index = i;
            }
        }
        public bool Enabled { get; set; }
        public int[] Histogram { get; set; }

        public List<ProjectedPoint> dataPoints { get; private set; }
        public Color Color { get { return m_Data.Color; } set { m_Data.Color = value; } }
        public string Name { get; private set; }

        public float Xmax { get { return m_Data.ColumnData[m_Dim0].Max; } }
        public float Ymax { get { return m_Data.ColumnData[m_Dim1].Max; } }
        public float Xmin { get { return m_Data.ColumnData[m_Dim0].Min; } }
        public float Ymin { get { return m_Data.ColumnData[m_Dim1].Min; } }

        public DataSeries Data { get { return m_Data; } }

        public string HorizontalDimName { get { return m_Dim0; } }
        public string VerticalDimName { get { return m_Dim1; } }

        private string m_Dim0;
        private string m_Dim1;
    }
    public class GroupSeriesProjection : ISeriesProjection
    {
        public GroupSeriesProjection(List<SeriesProjection> seriesList, Color color, string name)
        {
            Enabled = true;
            Color = color;
            Name = name;
            dataPoints = new List<ProjectedPoint>();
            int i = 0;
            
            Xmax = float.MinValue;
            Xmin = float.MaxValue;
            Ymax = float.MinValue;
            Ymin = float.MaxValue;

            foreach (var series in seriesList)
            {
                if (series.Enabled)
                {
                    foreach (var point in series.dataPoints)
                    {
                        point.Index = i++;
                        dataPoints.Add(point);
                    }
                    Xmax = Math.Max(Xmax, series.Xmax);
                    Ymax = Math.Max(Ymax, series.Ymax);
                    Xmin = Math.Min(Xmin, series.Xmin);
                    Ymin = Math.Min(Ymin, series.Ymin);
                }
            }
            Enabled = dataPoints.Count > 0;
        }
        public Color Color
        {
            get; set;
        }

        public List<ProjectedPoint> dataPoints
        {
            get;
            private set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public int[] Histogram
        {
            get;
            set;
        }

        public string Name
        {
            get;
            private set;
        }

        public float Xmax
        {
            get;
            private set;
        }

        public float Xmin
        {
            get;
            private set;
        }

        public float Ymax
        {
            get;
            private set;
        }

        public float Ymin
        {
            get;
            private set;
        }
    }
}
