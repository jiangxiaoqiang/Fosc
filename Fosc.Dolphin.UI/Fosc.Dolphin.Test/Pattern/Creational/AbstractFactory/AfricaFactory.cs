using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Test.Pattern.Creational.AbstractFactory
{
    public class AfricaFactory : ContinentFactory
    {
        public override Herbivore CreateHerbivore()
        {
            return new Wildebeest();
        }
        public override Carnivore CreateCarnivore()
        {
            return new Lion();
        }
    }
}
