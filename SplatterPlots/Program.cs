using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using OpenTK;
using System.Text;

namespace SplatterPlots
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ColorConv.DoColorExp();
            //ColorCombos();
            Runtime = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static void ColorCombos()
        {
            StringBuilder text = new StringBuilder();
            
            var colors = ColorConv.pickIsoColsVec(74.0f, 5, .5f, (float)Math.PI);
            List<List<int>> combos = new List<List<int>>();
            int num = colors.Count;
            for (int i = 0; i < Math.Pow(2, num) - 1; i++)
            {
                var bits = new BitArray(new int[] { i + 1 });
                var subl = new List<int>();
                for (int j = 0; j < num; j++)
                {
                    if (bits[j])
                    {
                        subl.Add(j);
                    }
                }
                if (subl.Count > 0)
                {

                    combos.Add(subl);
                }
            }
            
            var sortedCombos = from combo in combos
                               orderby combo.Count, combo.Sum()
                               let sorted = combo.OrderBy(i => i).ToList()
                               select sorted;



            foreach (var combo in sortedCombos)
            {
                combo.ForEach(i => text.Append("[" + i + "] "));
                text.Append(" => ");
                var col = ColorConv.PerceptualBlendRGB(combo.Select(index => colors[index]).ToList(), 1.0f, 1.12f);
                text.Append(string.Format("({0}, {1}, {2})", (int)(col.X * 255), (int)(col.Y * 255), (int)(col.Z * 255)));
                text.AppendLine();
            }
            string str = text.ToString();
            System.Console.WriteLine(str);
        }
        public static bool Runtime = false;
    }
}
