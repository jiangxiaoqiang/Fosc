using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.UI.Source
{
    public class SaverAccount:IBankAccount
    {
        private decimal balance;
        public void PayIn(decimal amount)
        {
            balance += amount;
        }

        public bool WithDraw(decimal amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                return true;
            }            
            return false;
        }

        public decimal Balance
        {
            get 
            { 
                return balance; 
            }
        }
    }
}
