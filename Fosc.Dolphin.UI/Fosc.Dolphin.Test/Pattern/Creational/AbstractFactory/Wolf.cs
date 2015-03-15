using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Creational.AbstractFactory
{
    // "ProductB2"
    class Wolf : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            MessageBox.Show(this.GetType().Name + " eats " + h.GetType().Name);              
        }
    }
}
