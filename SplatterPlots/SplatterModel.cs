﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using System.Data;

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
            dataTable = data.getXYValuesTable(dim0,dim1);
            dataPoints = data.getXYValues(dim0, dim1).ToArray();            
        }

        public DataTable dataTable { get; set; }
        public bool enabled { get; set; }
        public int[] Histogram { get; set; }

        public ProjectedPoint[] dataPoints { get; private set; }
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
        public SplatterModel(List<DataSeries> datas, int d0, int d1): this(datas, datas.First().ColumnNames[d0],datas.First().ColumnNames[d1])
        {
        }
        public SplatterModel(List<DataSeries> datas, string dim0N, string dim1N)
        {
            dim0Name = dim0N;
            dim1Name = dim1N;
            xmax = float.MinValue;
            xmin = float.MaxValue;
            ymax = float.MinValue;
            ymin = float.MaxValue;
            showAllPoints = true;

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
        public void Select(float xmin, float ymin, float xmax, float ymax)
        {
            foreach (var series in seriesList.Values)
            {
                //series.dataPoints.ForEach(p => p.Selected = IsSelected(p, xmin, ymin, xmax, ymax));
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

        private bool IsSelected(ProjectedPoint p, float xmin, float ymin, float xmax, float ymax)
        {
            return  p.X >= xmin && p.X <= xmax &&
                    p.Y >= ymin && p.Y <= ymax;
        }
    }
}
