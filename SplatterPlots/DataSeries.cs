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
        private DataSeriesRow m_Row;
        private static Random Rand = new Random();
        public ProjectedPoint(DataSeriesRow row, float x, float y)
        {
            X = x;
            Y = y;
            m_Row = row;
            Z = (float)ProjectedPoint.Rand.NextDouble();
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public bool Selected { get { return m_Row.Selected; } set { m_Row.Selected = value; } }
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
            Selected = false;
        }
        public DataRow DataRow { get { return m_Row; } }
        public float this[string s]
        {
            get
            {
                return m_Values[s];
            }
        }
        public bool Selected { get; set; }
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
        public List<DataSeriesRow> Rows { get { return m_Rows; } }
        
        public string Name { get; private set; }
        #endregion
        #region Public
        public void AddRow(DataSeriesRow row)
        {
            m_Rows.Add(row);
        }
        public void AddRowRange(IEnumerable<DataRow> rows, DataFileSchema schema)
        {
            var list = rows.Select(r => new DataSeriesRow(schema, r));
            m_Rows.AddRange(list);
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
        public IEnumerable<DataRow> GetSelectedRows()
        {
            return m_Rows.Where(row => row.Selected).Select(row=>row.DataRow);
        }
        public List<ProjectedPoint> getXYValues(string ColumnXName, string ColumnYName)
        {
            var query = from row in m_Rows
                        select new ProjectedPoint(row,(row[ColumnXName]), (row[ColumnYName]));
            return new List<ProjectedPoint>(query);
        }
        
        #endregion

    }
}
