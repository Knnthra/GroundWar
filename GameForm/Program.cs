using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GroundWar;

namespace GroundWar
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Splash());
            Application.Run(new mainMenu());
        }
    }
}
