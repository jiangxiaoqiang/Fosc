using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fosc.Dolphin.Common.Db.Sqlite;

namespace Fosc.Dolphin.UI.Dev.Widget
{
    public partial class FrmSqlite : Form
    {
        public FrmSqlite()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = string.Empty;
            try
            {
                var sqliteTable = SqliteService.GetDataTable("logins");
                dataGridView1.DataSource = sqliteTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
