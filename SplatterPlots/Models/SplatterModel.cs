using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using System.Data;
using System.ComponentModel;

namespace SplatterPlots
{
    public class SeriesMembership:INotifyPropertyChanged
    {
        private SeriesProjection m_Series;        
        private string m_Name;        
        private bool m_Member1;
        private bool m_Member2;
        private bool m_Member3;
        private bool m_Member4;
        private bool m_Member5;
        private bool m_Member6;
        private bool m_Member7;
        private bool m_Member8;
        private bool m_Updating = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public SeriesMembership(SeriesProjection series, int index)
        {
            m_Series = series;
            Name = series.Name;
            SetMember(index);
        }
        private void SetMember(int index)
        {
            if (m_Updating)
            {
                return;
            }
            m_Updating = true;
            Member = index;
            for (int i = 0; i < 8; i++)
            {
                if (i == index)
                {
                    SetBool(i, true);
                }
                else
                {
                    SetBool(i, false);
                }
            }
            m_Updating = false;
        }
        private void SetBool(int index, bool val)
        {
            switch (index)
            {
                case 0: Member1 = val; break;
                case 1: Member2 = val; break;
                case 2: Member3 = val; break;
                case 3: Member4 = val; break;
                case 4: Member5 = val; break;
                case 5: Member6 = val; break;
                case 6: Member7 = val; break;
                case 7: Member8 = val; break;
                default:
                    break;
            }
        }
        private bool GetBool(int index)
        {
            switch (index)
            {
                case 0: return m_Member1;
                case 1: return m_Member2;
                case 2: return m_Member3;
                case 3: return m_Member4;
                case 4: return m_Member5;
                case 5: return m_Member6;
                case 6: return m_Member7;
                case 7: return m_Member8;
                default: return false;
            }
        }

        public bool Enabled
        {
            get { return m_Series.Enabled; }
            set { m_Series.Enabled = value; NotifyPropertyChanged("Enabled"); }
        }
        
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; NotifyPropertyChanged("Name"); }
        }
        public bool Member1
        {
            get { return m_Member1; }
            set { m_Member1 = value; if (value) SetMember(0); NotifyPropertyChanged("Member1"); }
        }
        public bool Member2
        {
            get { return m_Member2; }
            set { m_Member2 = value; if (value) SetMember(1); NotifyPropertyChanged("Member2"); }
        }
        public bool Member3
        {
            get { return m_Member3; }
            set { m_Member3 = value; if (value) SetMember(2); NotifyPropertyChanged("Member3"); }
        }
        public bool Member4
        {
            get { return m_Member4; }
            set { m_Member4 = value; if (value) SetMember(3); NotifyPropertyChanged("Member4"); }
        }
        public bool Member5
        {
            get { return m_Member5; }
            set { m_Member5 = value; if (value) SetMember(4); NotifyPropertyChanged("Member5"); }
        }
        public bool Member6
        {
            get { return m_Member6; }
            set { m_Member6 = value; if (value) SetMember(5); NotifyPropertyChanged("Member6"); }
        }
        public bool Member7
        {
            get { return m_Member7; }
            set { m_Member7 = value; if (value) SetMember(6); NotifyPropertyChanged("Member7"); }
        }
        public bool Member8
        {
            get { return m_Member8; }
            set { m_Member8 = value; if (value) SetMember(7); NotifyPropertyChanged("Member8"); }
        }

        public int Member { get; private set; }
        public SeriesProjection Series { get { return m_Series; } }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
    

    public class SplatterModel
    {
        private List<Color> GroupColors = new List<Color>();

        public SplatterModel(List<DataSeries> datas, int d0, int d1): this(datas, datas.First().ColumnNames[d0],datas.First().ColumnNames[d1])
        {
        }
        public SplatterModel(List<DataSeries> datas, string dim0N, string dim1N)
        {
            Groups = new Dictionary<string, ISeriesProjection>();
            Series = new Dictionary<string,SeriesProjection>();
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
                Series[datas[i].Name]=sp;

                xmax = Math.Max(xmax, sp.Xmax);
                xmin = Math.Min(xmin, sp.Xmin);
                ymax = Math.Max(ymax, sp.Ymax);
                ymin = Math.Min(ymin, sp.Ymin);
            }
            SeriesMemberships = new BindingList<SeriesMembership>();
            int j = 0;
            foreach (var series in Series.Values)
            {
                SeriesMemberships.Add(new SeriesMembership(series,j));
                j++;
            }
            NumberOfGroups = Math.Min(Series.Count, 8);

            GroupColors = ColorConv.pickIsoCols(74.0f, NumberOfGroups, .5f, (float)Math.PI);
            SetGroupMembers();
            SeriesMemberships.ListChanged += new ListChangedEventHandler(SeriesMemberships_ListChanged);

        }

        void SeriesMemberships_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.PropertyDescriptor.Name == "Member8")
            {                
                SetGroupMembers();
            }
            if (e.PropertyDescriptor.Name == "Enabled")
            {                
                SetGroupMembers();
            }
        }
        public void Select(float xmin, float ymin, float xmax, float ymax)
        {
            foreach (var series in Series.Values)
            {
                series.dataPoints.ForEach(p => p.Selected = IsSelected(p, xmin, ymin, xmax, ymax));
            }
        }
        public List<ProjectedPoint> GetSelectedList(float xmin, float ymin, float xmax, float ymax)
        {
            List<ProjectedPoint> result = new List<ProjectedPoint>();
            foreach (var series in Series.Values)
            {
                result.AddRange(series.dataPoints.Where(p => IsSelected(p, xmin, ymin, xmax, ymax)));                
            }
            return result;
        }
        
        private void SetGroupMembers()
        {
            Groups.Clear();
            for (int i = 0; i < NumberOfGroups; i++)
            {
                var list = new List<SeriesProjection>();
                foreach (var series in SeriesMemberships)
                {
                    if (series.Member == i)
                    {
                        list.Add(series.Series);
                    }
                }
                var ser = new GroupSeriesProjection(list, GroupColors[i], "Member" + (i + 1));
                Groups[ser.Name] = ser;                
            }
            SendModelChanged();
        }
        private void SendModelChanged()
        {
            if (ModelChanged!=null)
            {
                ModelChanged(this, EventArgs.Empty);
            }
        }

        public Dictionary<string, ISeriesProjection> Groups { get; private set; }
        public BindingList<SeriesMembership> SeriesMemberships { get; set; }
        public Dictionary<string,SeriesProjection> Series { get; private set; }
        public int NumberOfGroups { get; set; }
        
        public bool showAllPoints;
        public float xmax;
        public float xmin;
        public float ymax;
        public float ymin;

        public string dim0Name;
        public string dim1Name;

        public event EventHandler ModelChanged;
        private bool IsSelected(ProjectedPoint p, float xmin, float ymin, float xmax, float ymax)
        {
            return  p.X >= xmin && p.X <= xmax &&
                    p.Y >= ymin && p.Y <= ymax;
        }
    }
}
