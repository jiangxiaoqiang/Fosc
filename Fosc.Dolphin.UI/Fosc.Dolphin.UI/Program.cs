using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fosc.Dolphin.UI.Dev;
using System.Reflection;
using System.IO;
using log4net.Config;

namespace Fosc.Dolphin.UI
{
    /*
     * Fosc:Free,Open,Share,Collaborative
     */
    static class Program
    {
        /// <summary>
        /// The main entry point for the application. 
        /// </summary>
        [STAThread]
        static void Main()
        {
            string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            string configFilePath = assemblyDirPath + @"\Conf\log4net.config";

            /*
             * Aways check if the 'configFilePath' is the correct 'log4net.config' file path.
             */
            XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath)); 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmMain());
            Application.Run(new FrmLogin());
        }
    }
}
