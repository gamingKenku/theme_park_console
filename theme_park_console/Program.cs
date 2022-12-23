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

            park.CreateFerrisWheel("абвыввы", 12.12, 12.12, 12, 12, 12);
            park.CreateRollerCoater("12в25ка", 12.12, 12.12, 12.12, 12, 12, 12, 200);
            park.CreateBumpingCars("абырвалг", 12.12, 12, 12.12, 12.12);
            park.CreateBumpingCars("абырвалг", 12.12, 12, 12.12, 12.12);
            park.CreateBumpingCars("абырвалг", 12.12, 12, 12.12, 12.12);
            park.CreateBumpingCars("абырвалг", 12.12, 12, 12.12, 12.12);
            park.SaveListToXML();
            park.LoadListFromXML();

            Console.ReadKey();
        }
    }
}
