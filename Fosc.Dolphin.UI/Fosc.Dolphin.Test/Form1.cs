using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fosc.Dolphin.Test.Pattern.Creational.AbstractFactory;
using Fosc.Dolphin.Test.Pattern.Creational.FactoryMethod;
using Fosc.Dolphin.Test.Pattern.Creational.Prototype;
using Fosc.Dolphin.Test.Pattern.Creational.Builder;
using Fosc.Dolphin.Test.Pattern.Behavioral.ChainOfResp;
using Fosc.Dolphin.Test.Pattern.Behavioral.Command;

namespace Fosc.Dolphin.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Runner run = new Runner();
            //run.Run();

            /*Factory Method*/
            //FMRunner FMrun = new FMRunner();
            //FMrun.Run();

            //PrototypeRun ProtoRun = new PrototypeRun();
            //ProtoRun.Run();

            //BuilderRunner brun = new BuilderRunner();
            //brun.Run();

            //ChainOfRespRunner corr = new ChainOfRespRunner();
           // corr.Run();

            CommandRunner commandRunner = new CommandRunner();
            commandRunner.Run();
        }
    }
}
