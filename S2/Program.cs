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

            int count = 10000;

            for (int i = 0; i < count; i++)
            {
                S2_SimulationCore s2_SimulationCore =
                    new S2_SimulationCore(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0), 5, 14);

                var sim = s2_SimulationCore.Simulate();
                celkoOdislo += sim[0];
                casCakania += sim[1];

                prisloC += sim[2];
                celkoOdisloC += sim[3];
                plateny += sim[4];
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
