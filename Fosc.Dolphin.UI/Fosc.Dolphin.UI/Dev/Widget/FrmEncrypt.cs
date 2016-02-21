using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.UI.Dev.Widget
{
    public partial class FrmEncrypt : Form
    {
        public FrmEncrypt()
        {
            InitializeComponent();
        }

        #region 猜测
        private void button1_Click(object sender, EventArgs e)
        {
            var sourceValue = this.txtSource.Text;
            var resultValue = this.txtResult.Text;
            if (string.IsNullOrEmpty(sourceValue) || string.IsNullOrEmpty(resultValue))
            {
                MessageBox.Show(@"原始值和结果值不能为空");
            }
            else
            {

            }
        }
        #endregion

    }
}
