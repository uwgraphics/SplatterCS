using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace SplatterPlots
{
    public class DataFileSchema
    {
        public List<string> ColumnNames { get; private set; }
        public string GroupBy { get; set; }
        public Dictionary<string,bool> ColumnNumericMap { get; private set; }

        public DataFileSchema(DataFile datafile)
        {
            ColumnNames = new List<string>(datafile.ColumnNames);
            GroupBy = "None";
            ColumnNumericMap = new Dictionary<string, bool>();
            foreach (var col in datafile.ColumnNames)
            {
                ColumnNumericMap[col] = datafile.IsNumeric(col);
            }
        }
    }
    public class DataFile
    {
        #region Fields
        private DataTable m_table;
        private List<bool> m_isNumeric = new List<bool>();
        private List<string> m_ColumnNames = new List<string>();
        #endregion

        #region Construction
        public DataFile(string path)
        {
            m_table = new DataTable(Path.GetFileName(path));
            Name = Path.GetFileName(path);
            LinkedList<string> lines = new LinkedList<string>(File.ReadAllLines(path));

            //assume first line is column names            
            var first = lines.First.Value;
            var cols = first.Split(new char[]{',', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var col in cols)
            {
                m_table.Columns.Add(col, typeof(string));
                m_ColumnNames.Add(col);
            }
            lines.RemoveFirst();

            bool firstNumericLine = true;
            foreach (var line in lines)
            {
                var array = line.Split(new char[] { ',', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                m_table.Rows.Add(array);
                if (firstNumericLine)
                {
                    firstNumericLine = false;
                    float dummy;
                    var query = from item in array
                                select float.TryParse(item, out dummy);
                    m_isNumeric = query.ToList();
                    
                }
            }
        }
        #endregion
        #region Properties
        public DataTable Table { get { return m_table; } }
        public string Name { get; set; }
        public List<string> ColumnNames { get { return m_ColumnNames; } }
        #endregion
        #region Public
        public List<List<DataSeries>> ConvertToOneVsAllDataSeries(string groupBy, string dim0, string dim1)
        {           
            //var res = new List<List<DataSeries>>();
            //var schema = new DataFileSchema(this);
            //schema.ColumnNames.ForEach(col => schema.ColumnNumericMap[col] = false);
            //schema.ColumnNumericMap[dim0] = true;
            //schema.ColumnNumericMap[dim1] = true;
            //var groups = (from row in m_table.AsEnumerable()
            //              select row.Field<string>(groupBy)).Distinct();
            //var colors = ColorConv.pickIsoCols(74.0f, 2, .5f, (float)Math.PI);
            //foreach (var groupVal in groups)
            //{                
            //    var theGroupQuery = from row in m_table.AsEnumerable()
            //                        where row.Field<string>(groupBy) == groupVal
            //                        select row;
            //    var othersQuery = from row in m_table.AsEnumerable()
            //                      where row.Field<string>(groupBy) !=groupVal
            //                      select row;
            //    DataSeries theGroup = new DataSeries(this, schema, Name + "." + groupVal);
            //    theGroup.AddRowRange(theGroupQuery, schema);
            //    theGroup.EndInit();
            //    theGroup.Color = colors[0];
            //    DataSeries Others = new DataSeries(this, schema, Name + ".Others");
            //    Others.AddRowRange(othersQuery, schema);
            //    Others.EndInit();
            //    Others.Color = colors[1];
            //    var temp = new List<DataSeries>();
            //    temp.Add(theGroup);
            //    temp.Add(Others);
            //    res.Add(temp);                
            //}
                         
            //return res;
            //need to make sure these guys are linked

            var res = new List<List<DataSeries>>();
            var schema = new DataFileSchema(this);
            schema.ColumnNames.ForEach(col => schema.ColumnNumericMap[col] = false);
            schema.ColumnNumericMap[dim0] = true;
            schema.ColumnNumericMap[dim1] = true;
            schema.GroupBy = groupBy;
            var groups = ConvertToDataSeries(schema);
            var ordered = groups.OrderByDescending(g => g.Rows.Count).ToList();
            for (int i = 0; i < 10; i++)
            {
                var temp = new List<DataSeries>();
                temp.Add(ordered[i]);
                DataSeries Others = new DataSeries(this, schema, Name + ".Others");
                for (int j = 0; j < ordered.Count; j++)
                {
                    if (i != j)
                    {
                        ordered[j].Rows.ForEach(r => Others.AddRow(r));
                    }
                }
                Others.EndInit();
                temp.Add(Others);
                res.Add(temp);
            }
            for (int i = 10; i < groups.Count; i++)
            {
                var temp = new List<DataSeries>();
                temp.Add(groups[i]);
                DataSeries Others = new DataSeries(this, schema, Name + ".Others");
                for (int j = 0; j < groups.Count; j++)
                {
                    if (i != j)
                    {
                        groups[j].Rows.ForEach(r => Others.AddRow(r));
                    }
                }
                Others.EndInit();
                temp.Add(Others);
                res.Add(temp);
            }
            return res;
        }        
        public List<DataSeries> ConvertToDataSeries(DataFileSchema schema)
        {
            List<DataSeries> res = new List<DataSeries>();
            if (schema.GroupBy.Equals("None"))
            {
                DataSeries data = new DataSeries(this, schema, Name);
                foreach (var row in m_table.AsEnumerable())
                {
                    data.AddRow(new DataSeriesRow(schema, row));
                }
                data.EndInit();
                res.Add(data);
            }
            else
            {
                var query =
                    from row in m_table.AsEnumerable()
                    group row by row[schema.GroupBy];

                foreach (var group in query)
                {
                    DataSeries data = new DataSeries(this, schema, Name + "." + group.Key);
                    foreach (var row in group)
                    {
                        data.AddRow(new DataSeriesRow(schema, row));
                    }
                    data.EndInit();
                    res.Add(data);
                }
            }
            return res;
            
        }
        public bool IsNumeric(string columnName)
        {
            int index = m_ColumnNames.FindIndex(str => str.Equals(columnName));
            if (index >= 0)
            {
                return m_isNumeric[index];
            }
            return false;
        }
        #endregion

    }
}
