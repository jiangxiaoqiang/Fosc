using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Behavioral.ChainOfResp
{
    class Director:Approver
    {
        public override void ProcessRequest(Purchase purchase)
        {
            if (purchase.Amount < 10000.0)
            {                
                string str = string.Format("{0} approved request# {1}",this.GetType().Name, purchase.Number);
                MessageBox.Show(str);
            }
            else if (successor != null)
            {
                successor.ProcessRequest(purchase);
            }
        }
    }
}
