using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Test.Pattern.Creational.AbstractFactory
{
    //"AbstractProductB"
    public abstract class Carnivore
    {
        public abstract void Eat(Herbivore h);
    }
}
