using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.Data;
using System.Drawing;

namespace SplatterPlots
{
    public class ProjectedPoint
    {
        private DataRow m_Row;

        public ProjectedPoint(DataRow row,float x,float y)
        {
            X = x;
            Y = y;
            m_Row = row;
        }

        public float X { get; private set; }
        public float Y { get; private set; }
    }
    public class ColumnData
    {
        public ColumnData(string name, float min, float max)
        {
            Name = name;
            Min = min;
            Max = max;
        }

        public string Name { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
    }
    public class DataSeriesRow
    {
        Dictionary<string, float> m_Values = new Dictionary<string, float>();
        DataRow m_Row;
        public DataSeriesRow(DataFileSchema schema, DataRow row)
        {
            m_Row = row;
            foreach (var col in schema.ColumnNames.Where(c => schema.ColumnNumericMap[c]))
            {
                m_Values[col] = Convert.ToSingle(row.Field<string>(col));
            }
        }
        public DataRow DataRow { get { return m_Row; } }
        public float this[string s]
        {
            get
            {
                return m_Values[s];
            }
        }
    }
    public class DataSeries
    {
        #region Fields
        private DataFile m_DataFile;
        private DataFileSchema m_Schema;
        private List<DataSeriesRow> m_Rows=new List<DataSeriesRow>();
        #endregion

        #region Construction
        public DataSeries(DataFile dataFile, DataFileSchema schema, string name)
        {
            m_DataFile = dataFile;
            m_Schema = schema;
            ColumnData = new Dictionary<string, ColumnData>();
            ColumnNames = new List<string>();
            Name = name;
        }
        #endregion

        #region Properties
        public Color Color { get; set; }
        public List<string> ColumnNames { get; set; }
        public Dictionary<string, ColumnData> ColumnData { get; set; }
        
        public string Name { get; private set; }
        #endregion
        #region Public
        public void AddRow(DataSeriesRow row)
        {
            m_Rows.Add(row);
        }
        public void EndInit()
        {
            foreach (var col in m_Schema.ColumnNames.Where(c => m_Schema.ColumnNumericMap[c]))
            {
                var query = from row in m_Rows
                            select row[col];
                var d = new ColumnData(col, query.Min(), query.Max());
                ColumnData.Add(col, d);
                ColumnNames.Add(col);
            }
        }
        public List<ProjectedPoint> getXYValues(string ColumnXName, string ColumnYName)
        {
            ColumnData cDX = ColumnData[ColumnXName];
            ColumnData cDY = ColumnData[ColumnYName];

            var query = from row in m_Rows
                        select new ProjectedPoint(row.DataRow,(row[ColumnXName]), (row[ColumnYName]));
            return new List<ProjectedPoint>(query);
        }
        
        #endregion

    }
}
