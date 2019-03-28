using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;

namespace Simulations.UsedSimulations.Other
{

    public class Food
    {
        public double Time { get; set; }
        public Agent_S2 Agent { get; set; }

        public override string ToString()
        {
            return $"{Time} ";
        }
    }
}
