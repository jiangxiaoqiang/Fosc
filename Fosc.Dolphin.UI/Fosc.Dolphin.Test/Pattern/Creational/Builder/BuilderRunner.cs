using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosc.Dolphin.Test.Pattern.Creational.Builder
{
    class BuilderRunner
    {
        public void Run()
        {
            // Create shop with vehicle builders
            Shop shop = new Shop();
            VehicleBuilder b1 = new ScooterBuilder();
            VehicleBuilder b2 = new CarBuilder();
            VehicleBuilder b3 = new MotorCycleBuilder();

            // Construct and display vehicles
            shop.Construct(b1);
            b1.Vehicle.Show();

            shop.Construct(b2);
            b2.Vehicle.Show();

            shop.Construct(b3);
            b3.Vehicle.Show();

            // Wait for user
            Console.Read();
        }
    }
}
