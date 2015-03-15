using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fosc.Dolphin.Test.Pattern.Creational.Prototype
{
    class ColorManager
    {
        Hashtable colors = new Hashtable();

        // Indexer
        public ColorPrototype this[string name]
        {
            get
            {
                return colors[name] as ColorPrototype;
            }
            set
            {
                colors.Add(name, value);
            }
        }
    }
}
