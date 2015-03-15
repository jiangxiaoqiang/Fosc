using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Test.Pattern.Behavioral.ChainOfResp
{
    class ChainOfRespRunner
    {
        public void Run()
        {
            // Setup Chain of Responsibility
            Director Larry = new Director();
            VicePresident Sam = new VicePresident();
            President Tammy = new President();
            Larry.SetSuccessor(Sam);
            Sam.SetSuccessor(Tammy);

            // Generate and process purchase requests
            Purchase p = new Purchase(2034, 350.00, "Supplies");
            Larry.ProcessRequest(p);

            p = new Purchase(2035, 32590.10, "Project X");
            Larry.ProcessRequest(p);

            p = new Purchase(2036, 122100.00, "Project Y");
            Larry.ProcessRequest(p);
        }
    }
}
