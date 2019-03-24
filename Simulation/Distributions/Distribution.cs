using System;

namespace Simulations.Distributions
{
    public abstract class Distribution
    {
        public abstract double GetNext();
        public Distribution(int seed)
        {
        }
    }
}
