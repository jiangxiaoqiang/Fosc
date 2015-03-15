using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Behavioral.ChainOfResp
{
    public class President:Approver
    {
        public override void ProcessRequest(Purchase purchase)
        {
            if (purchase.Amount < 100000.0)
            {
                string str = string.Format("{0} approved request# {1}",this.GetType().Name, purchase.Number);
                MessageBox.Show(str);
            }
            else
            {
                string str = string.Format("Request# {0} requires an executive meeting!",purchase.Number);
                MessageBox.Show(str);
            }
        }
    }
}
