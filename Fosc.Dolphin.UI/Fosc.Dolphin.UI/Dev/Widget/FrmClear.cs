using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fosc.Dolphin.IBll.IPub;
//using Fosc.Dolphin.IBll.IPub;
//using Fosc.Dolphin.Bll.Pub;

namespace Fosc.Dolphin.UI.Dev.Widget
{
    public partial class FrmClear : Form
    {
        public FrmClear()
        {
            InitializeComponent();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            #region Clear System Cache
            try
            {
                ClearCacheFile();
                MessageBox.Show("Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        #region Clear System Cache
        private void ClearCacheFile()
        {
            List<string> filePath = new List<string>();
            //IFileOperHelper fileOper = new FileOperHelper();
            //string TempPath = @"G:\ECA\FTP\设计院平台最新\Install Package\CAD2011以下版本\Client" + @"\assemblyCache\";
            //string TempPath1 = @"G:\ECA\FTP\设计院平台最新\Install Package\CAD2011以下版本\Client" + @"\Config\model\";
            //filePath.Add(TempPath);
            //filePath.Add(TempPath1);
            //fileOper.ClearCacheFile(filePath);
        }
        #endregion
    }
}
