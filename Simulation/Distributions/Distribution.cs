using System;

namespace Simulation.Distributions
{
    abstract class Distribution
    {
        public abstract double GetNext();
        public Distribution(int seed)
        {
        }
    }
}
