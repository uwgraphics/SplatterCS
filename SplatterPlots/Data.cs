using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OpenTK;
using System.IO;

namespace SplatterPlots
{
    class ColumnData
    {
        public ColumnData(string name, float min, float max )
        {
            Name = name;
            Min = min;
            Max = max;
        }
        
        public string Name { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }        
    }
    public class Data
    {
        #region fields
        private HashSet<string> m_columnNameSet;
        private DataTable m_table;
        //private DataView m_view;
        private bool isSet = false;

        private List<double> m_X;
        private List<double> m_Y;
        private List<Vector2> m_XY;
        #endregion

        #region Construction
        public Data()
        {
            ColumnData = new Dictionary<string, ColumnData>();
            ColumnNames = new List<string>();
        }
        #endregion

        #region private
        private void process()
        {
            foreach (var col in m_table.Columns.Cast<DataColumn>())
            {
                var query = from row in m_table.AsEnumerable()
                            select row.Field<float>(col);
                var d = new ColumnData(col.ColumnName, query.Min(), query.Max());                
                ColumnData.Add(col.ColumnName, d);
                ColumnNames.Add(col.ColumnName);
            }
        }
        #endregion

        #region IData Members
        public List<string> ColumnNames { get; set; }
        public Dictionary<string, ColumnData> ColumnData { get; set; }
        
        public string Name { get; private set; }
        public void load(string path)
        {
            m_table = new DataTable(Path.GetFileName(path));
            Name = Path.GetFileName(path);
            List<string> lines = new List<string>(File.ReadAllLines(path));

            //need to check if there are colum names
            var first = lines[0];
            var cols = first.Split(',', '\t');
            bool hasNames = false;
            foreach (var col in cols)
            {
                double dummy;
                hasNames = hasNames || !double.TryParse(col, out dummy);
            }
            if (hasNames)
            {
                foreach (var col in cols)
                {
                    m_table.Columns.Add(col, typeof(double));
                }
                lines.RemoveAt(0);
            }
            else
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    m_table.Columns.Add(i.ToString(), typeof(double));
                }
            }

            foreach (var line in lines)
            {
                m_table.Rows.Add(line.Split(',', '\t').Select(obj => (object)(double.Parse(obj))).ToArray());
            }
            process();
        }

        

        public List<float> getValues(string ColumnName)
        {
            ColumnData cData = ColumnData[ColumnName];

            var query = from row in m_table.AsEnumerable()
                        select (row.Field<float>(ColumnName));
            
            return query.ToList();
        }



        public List<Vector2> getXYValues(string ColumnXName, string ColumnYName)
        {
            ColumnData cDX = ColumnData[ColumnXName];
            ColumnData cDY = ColumnData[ColumnYName];

            var query = from row in m_table.AsEnumerable()
                        select new Vector2((row.Field<float>(ColumnXName)), (row.Field<float>(ColumnYName)));
            return new List<Vector2>(query);
        }
        #endregion
    }
}
