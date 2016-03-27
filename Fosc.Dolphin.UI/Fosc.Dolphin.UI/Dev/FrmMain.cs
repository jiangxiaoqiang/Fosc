using System;
using System.Globalization;
using System.Windows.Forms;
using Fosc.Dolphin.UI.Dev.Widget;
using Fosc.Dolphin.UI.Source;

namespace Fosc.Dolphin.UI.Dev
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
            MessageBox.Show(venusAccount.Balance.ToString(CultureInfo.InvariantCulture));
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
            var frmEncrypt = new FrmEncrypt { MdiParent = this };
            frmEncrypt.Show();
        }

        private void testTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmTest = new FrmTest { MdiParent = this };
            frmTest.Show();
        }

        private void toolStripMenuItem_Sqlite_Click(object sender, EventArgs e)
        {
            var frmTest = new FrmSqlite { MdiParent = this };
            frmTest.Show();
        }
    }
}
