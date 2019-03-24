using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.UsedSimulations.S2;

namespace S2
{
    class Program
    {
        static void Main(string[] args)
        {
            S2_SimulationCore s2_SimulationCore = 
                new S2_SimulationCore(new TimeSpan(11, 0, 0), new TimeSpan(20, 0, 0),10,3);
            s2_SimulationCore.Simulate();
        }
    }
}
