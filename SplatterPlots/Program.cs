using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            Runtime = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static bool Runtime = false;
    }
}
