using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fosc.Dolphin.Test.Pattern.Creational.FactoryMethod
{
    class FMRunner : IPatternRunner
    {
        public void Run()
        {
            // Note: constructors call Factory Method
            Document[] documents = new Document[2];
            documents[0] = new Resume();
            documents[1] = new Report();

            // Display document pages
            foreach (Document document in documents)
            {
                MessageBox.Show("\n" + document.GetType().Name + "--");
                foreach (Page page in document.Pages)
                {
                    MessageBox.Show(" " + page.GetType().Name);
                }
            }
        }
    }
}
