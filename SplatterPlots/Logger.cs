using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SplatterPlots
{
    public static class Logger
    {
        public static void Log(Stat stat)
        {
            string fileName = "Log.txt";
            if (!File.Exists(fileName))
            {
                using (var stream = new StreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write)))
                {
                    stream.WriteLine(string.Format("Bandwidth,ClutterWindow,GroupNum,Milliseconds,PointNum,Threshold,Width,Height"));
                }
            }
            using (var stream = new StreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write)))
            {
                stream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",   stat.Bandwidth,
                                                                                stat.ClutterWindow,
                                                                                stat.GroupNum,
                                                                                stat.Milliseconds,
                                                                                stat.PointNum,
                                                                                stat.Threshold,
                                                                                stat.Width,
                                                                                stat.Height));
            }
        }
    }
}
