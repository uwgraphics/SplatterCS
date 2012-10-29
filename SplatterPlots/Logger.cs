using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SplatterPlots
{
    public static class Logger
    {
        private static string colorFileName = "ColorLog.txt";
        private static string FileName = "Log.txt";

        public static void Log(Stat stat)
        {            
            if (!File.Exists(FileName))
            {
                using (var stream = new StreamWriter(File.Open(FileName, FileMode.Append, FileAccess.Write)))
                {
                    stream.WriteLine(string.Format("Bandwidth,ClutterWindow,GroupNum,Milliseconds,PointNum,Threshold,Width,Height"));
                }
            }
            using (var stream = new StreamWriter(File.Open(FileName, FileMode.Append, FileAccess.Write)))
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
        public static void ClearColorLog()
        {
            if (File.Exists(colorFileName))
            {
                File.Delete(colorFileName);
            }
        }
        public static void Log(ColorConv.ColorInfo stat)
        {

            using (var stream = new StreamWriter(File.Open(colorFileName, FileMode.Append, FileAccess.Write)))
            {
                stream.WriteLine("******************************************");
                stream.WriteLine("Number of Colors: {0}",stat.Num);
                stream.WriteLine("Chorma Factor: {0}",stat.Cf);
                stream.WriteLine("Lightness Factor: {0}",stat.Lf);
                stream.WriteLine("Minimum diff: {0}",stat.TotalDiff);
                stream.WriteLine("Colors:");
                //foreach (var col in stat.ColorList)
                //{
                //    stream.WriteLine("({0},{1},{2})", (int)(col.X * 255), (int)(col.Y * 255), (int)(col.Z * 255));
                //}
                /*for (int i = 0; i < stat.DiffMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < stat.DiffMatrix.GetLength(1); j++)
                    {
                        stream.Write("{0}\t", stat.DiffMatrix[i, j]);
                    }
                    stream.WriteLine();
                }*/
            }
        }
    }
}
