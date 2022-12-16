using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theme_park_console
{
    class Program
    {
        static void Main()
        {
            AmusementPark park = new AmusementPark("Test", 10000.0);
            XMLSerialisator serialisator = new XMLSerialisator();

            park.CreateFerrisWheel("ferris_wheel", 12.12, 12.12, 12, 12, 12);
            park.CreateRollerCoater("roller_coaster", 12.12, 12.12, 12.12, 12, 12, 12, 200);
            park.CreateBumpingCars("bumping_cars", 12.12, 12, 12.12, 12.12);

            serialisator.Serialise(park.attractions, @"C:\Users\ПК\source\repos\theme_park_console\theme_park_console\XMLFile1.xml");

            Console.ReadKey();
        }
    }
}
