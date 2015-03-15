using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Develop.Core.Context;
using Develop.Core.Util;
//using Fosc.Dolphin.Bll.SysService;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;
using Fosc.Dolphin.IBll.Service.SysService;
using Fosc.Dolphin.Bll.SysService;
using Fosc.Dolphin.Bll.FileService;
using Fosc.Dolphin.Dto.SysDto;

namespace Fosc.Dolphin.UI.Dev
{
    public partial class FrmLogin : Form
    {
		private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FrmLogin));		
				
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = this.txtUserName.Text;/*0947809*/
                string password = this.txtPassword.Text;/*0947809*/
                if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(password))
                {
                    ISysLoginService service = Develop.Core.Context.ObjectContainer.GetObject<ISysLoginService>();
                    if (service != null)
                    {
                        SysLoginDto loginDto = service.QuerySysLoginByID(userName);
                        if (password == loginDto.Password)
                        {
                            MessageBox.Show("Welcome '"+loginDto.Cname+"'");
                            FrmMain frmMain = new FrmMain();
                            frmMain.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("User name and password is not correct!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login failed!");
                    }
                }
                else
                {
                    MessageBox.Show("User name and password must not be null!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
