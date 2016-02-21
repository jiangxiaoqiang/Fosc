using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fosc.Dolphin.UI.Source;
using Fosc.Dolphin.UI.Dev.Widget;

namespace Fosc.Dolphin.UI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IBankAccount venusAccount = new SaverAccount();
            IBankAccount jupiterAccount = new GoldAccount();
            venusAccount.PayIn(200);
            venusAccount.WithDraw(20);
            MessageBox.Show(venusAccount.Balance.ToString());
        }

        #region Clear
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClear frmClear = new FrmClear();
            frmClear.MdiParent = this;
            frmClear.Show();
        }
        #endregion

        private void toolStripMenuItem_CodeGenerator_Click(object sender, EventArgs e)
        {
            var frmClear = new FrmAutoCode { MdiParent = this };
            frmClear.Show();
        }

        private void toolStripMenuItem_Encrypt_Click(object sender, EventArgs e)
        {
            var frmClear = new FrmAutoCode { MdiParent = this };
            frmClear.Show();
        }
    }
}
