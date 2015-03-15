using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.UI.Source
{
    interface IBankAccount
    {
        void PayIn(decimal amount);
        bool WithDraw(decimal amount);
        decimal Balance
        {
            get;
        }
    }
}
