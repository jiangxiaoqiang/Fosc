using System;
using System.Windows.Forms;
using Fosc.Dolphin.Common.AutoCode;
using Fosc.Dolphin.Object.AutoCode;

namespace Fosc.Dolphin.UI.Dev.Widget
{
    public partial class FrmAutoCode : Form
    {
        public FrmAutoCode()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var configModel = new DatabaseConfig
            {
                DatabaseIpAddr = txtDatabaseIpAddr.Text,
                DatabaseName = txtDatabaseName.Text,
                DatabasePassword = txtPassword.Text,
                DatabaseUserName = txtUserName.Text,
                DatabaseAlias = txtPrev.Text
            };
            var suceess = CodeCompilerHelper.AutoGenerateImpl(configModel);
            MessageBox.Show(suceess ? @"Generate Success" : @"Generate Failed");
        }
    }
}