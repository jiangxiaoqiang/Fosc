using System;
using System.IO;
using System.Windows.Forms;
using Fosc.Dolphin.Common.AutoCode;

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
            var codeGeneratePath = Environment.CurrentDirectory + @"\CodeOutPath\";
            var suceess = CodeCompilerHelper.CompileImplemet(codeGeneratePath);
            MessageBox.Show(suceess ? @"Generate Success" : @"Generate Failed");
        }
    }
}