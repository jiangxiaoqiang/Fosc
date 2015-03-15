using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Creational.Prototype
{
    public class Color:ColorPrototype
    {
        int red;
        int green;
        int blue;

        // Constructor
        public Color(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        // Create a shallow copy
        public override ColorPrototype Clone()
        {
            string str = string.Format("Cloning color RGB: {1,3},{1,3},{2,3}",red, green, blue);
            MessageBox.Show(str);
            return this.MemberwiseClone() as ColorPrototype;
        }
    }
}
