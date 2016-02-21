using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fosc.Dolphin.UI.Dev;
using System.Reflection;
using System.IO;
using System.Threading;
using Fosc.Dolphin.Common.LogCompenent;
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
            var assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            var configFilePath = assemblyDirPath + @"\Conf\log4net.config";

            /*
             * Aways check if the 'configFilePath' is the correct 'log4net.config' file path.
             */
            XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            //做一些极其简单的记录异常信息操作
            LogHelper.Logger.Error(ex);
        }
    }
}
