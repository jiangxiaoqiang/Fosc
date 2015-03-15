using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Behavioral.Command
{
    public class Calculator
    {
        int curr = 0;

        public void Operation(char @operator, int operand)
        {
            switch (@operator)
            {
                case '+': curr += operand; break;
                case '-': curr -= operand; break;
                case '*': curr *= operand; break;
                case '/': curr /= operand; break;
            }
            string str = string.Format("Current value = {0,3} (following {1} {2})",curr, @operator, operand);
            MessageBox.Show(str);
        }
    }
}
