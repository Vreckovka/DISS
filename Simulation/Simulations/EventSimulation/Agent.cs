using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Simulations.EventSimulation
{
    public class Agent
    {
        public int ID { get; }
        private static int count;
        public Agent()
        {
            ID = count;
            count++;
        }
    }
}
