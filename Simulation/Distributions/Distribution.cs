using System;

namespace Simulations.Distributions
{
    public abstract class Distribution
    {
        protected Random _random;
        public abstract double GetNext();
        public Distribution(int seed)
        {
            _random = new Random(seed);
        }
    }
}
