using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fosc.Dolphin.Test.Pattern.Creational.FactoryMethod
{
    public abstract class Document
    {
        ArrayList pages = new ArrayList();

        // Constructor calls abstract Factory method
        public Document()
        {
            this.CreatePages();
        }

        public ArrayList Pages
        {
            get { return pages; }
        }

        // Factory Method
        public abstract void CreatePages();
    }
}
