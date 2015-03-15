using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Creational.AbstractFactory
{
    public class Lion:Carnivore
    {
        public override void Eat(Herbivore h)
        {
            //Eat Wildebeest
            MessageBox.Show(this.GetType().Name + " eats " + h.GetType().Name);              
        }
    }
}
