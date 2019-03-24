using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Distributions;
using Simulations.UsedSimulations.S2;

namespace S2
{
    class Program
    {
        static void Main(string[] args)
        {
            double celkoOdislo = 0;
            double casCakania = 0;
            double celkoOdisloC = 0;
            double prisloC = 0;
            double plateny = 0;

            int count = 1000;

            for (int i = 0; i < count; i++)
            {
                S2_SimulationCore s2_SimulationCore =
                    new S2_SimulationCore(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), 2, 14);
                celkoOdislo += s2_SimulationCore.Simulate()[0];
                casCakania += s2_SimulationCore.Simulate()[1];

                prisloC += s2_SimulationCore.Simulate()[2];
                celkoOdisloC += s2_SimulationCore.Simulate()[3];
                plateny += s2_SimulationCore.Simulate()[4];
            }

            Console.WriteLine( $"Pocet odidenych {celkoOdislo / count}\n" +
                               $"Priemerny cas cakania {casCakania / count}\n" +
                               $"Pocet ostajucich {prisloC/count}\n" +
                               $"Pocet odidenych {celkoOdisloC/count},\n" +
                               $"Pocet zaplatenych {plateny/count}");

            Console.ReadLine();
        }
    }
}
