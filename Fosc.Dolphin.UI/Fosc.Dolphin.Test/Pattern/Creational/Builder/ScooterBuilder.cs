using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Test.Pattern.Creational.Builder
{
    public class ScooterBuilder:VehicleBuilder
    {
        public override void BuildFrame()
        {
            vehicle = new Vehicle("Scooter");
            vehicle["frame"] = "Scooter Frame";
        }

        public override void BuildEngine()
        {
            vehicle["engine"] = "50 cc";
        }

        public override void BuildWheels()
        {
            vehicle["wheels"] = "2";
        }

        public override void BuildDoors()
        {
            vehicle["doors"] = "0";
        }
    }
}
