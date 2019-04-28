using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simulation;
using Simulations.UsedSimulations.S3;


namespace S2
{
    class Program
    {
        static void Main(string[] args)
        {
            MySimulation sim = new MySimulation();

            sim.OnReplicationDidFinish((s)
                => {
               
                Console.WriteLine("KONIEC REPLIKACIE");
                Console.Clear();
            });

            sim.OnSimulationDidFinish((s)
                => {
                Console.WriteLine("Koniec.");
                Console.ReadKey();
            });

            sim.Simulate(Config.PocetReplikacii, Config.PocetReplikacii);
        }
    }
}
