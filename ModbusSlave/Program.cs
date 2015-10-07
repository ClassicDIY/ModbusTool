using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ModbusSlave
{
    static class Program
    {


        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadExit += Application_ApplicationExit;
            Application.Run(new SlaveForm());
            Application.ApplicationExit += Application_ApplicationExit;
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {

        }
    }
}
