﻿using System;
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
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = array[i].Trim();
                }
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
        //groups the top limitOfGroups groups, and compares them with the rest of the dataset. 
        public List<List<DataSeries>> ConvertToOneVsAllDataSeries(string groupBy, string dim0, string dim1, int limitOfGroups)
        {             
            //need to make sure these guys are linked

            var res = new List<List<DataSeries>>();
            var schema = new DataFileSchema(this);
            schema.ColumnNames.ForEach(col => schema.ColumnNumericMap[col] = false);
            schema.ColumnNumericMap[dim0] = true;
            schema.ColumnNumericMap[dim1] = true;
            schema.GroupBy = groupBy;
            var groups = ConvertToDataSeries(schema, limitOfGroups);
            var ordered = groups.OrderByDescending(g => g.Rows.Count).ToList();
            for (int i = 0; i < Math.Min(10,groups.Count); i++)
            {
                var temp = new List<DataSeries>();
                temp.Add(ordered[i]);
                DataSeries Others = new DataSeries(this, schema, ".Others");
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
            return res;
        }        
        public List<DataSeries> ConvertToDataSeries(DataFileSchema schema, int limitOfGroups)
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
                    group row by row[schema.GroupBy] into set
                    orderby set.Count() descending
                    select set;
                int max = limitOfGroups > 0 ? limitOfGroups : int.MaxValue;
                int index = 0;
                DataSeries rest = new DataSeries(this, schema, ".Rest");
                foreach (var group in query)
                {
                    if (index < max)
                    {
                        DataSeries data = new DataSeries(this, schema, group.Key.ToString());
                        foreach (var row in group)
                        {
                            data.AddRow(new DataSeriesRow(schema, row));
                        }
                        data.EndInit();
                        res.Add(data);
                    }
                    else
                    {
                        foreach (var row in group)
                        {
                            rest.AddRow(new DataSeriesRow(schema, row));
                        }
                    }
                    index++;
                }
                if (rest.Rows.Count>0)
                {
                    rest.EndInit();
                    res.Add(rest);
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
