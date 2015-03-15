using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.UI.Source
{
    public class GoldAccount:IBankAccount
    {
        public void PayIn(decimal amount)
        {
            throw new NotImplementedException();
        }

        public bool WithDraw(decimal amount)
        {
            throw new NotImplementedException();
        }

        public decimal Balance
        {
            get { throw new NotImplementedException(); }
        }
    }
}
