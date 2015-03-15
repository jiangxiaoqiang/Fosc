using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Creational.Builder
{
    public class Vehicle
    {
        string type;
        Hashtable parts = new Hashtable();

        // Constructor
        public Vehicle(string type)
        {
            this.type = type;
        }

        // Indexer (i.e. smart array)
        public object this[string key]
        {
            get { return parts[key]; }
            set { parts[key] = value; }
        }

        public void Show()
        {
            MessageBox.Show("\n---------------------------");
            MessageBox.Show("Vehicle Type: {0}", type);
            string frame = string.Format(" Frame : {0}", parts["frame"]);
            MessageBox.Show(frame);
            string engine = string.Format(" Engine : {0}", parts["engine"]);
            MessageBox.Show(engine);
            string wheels = string.Format(" #Wheels: {0}", parts["wheels"]);
            MessageBox.Show(wheels);
            string doors = string.Format(" #Doors : {0}", parts["doors"]);
            MessageBox.Show(doors);
        }
    }
}
